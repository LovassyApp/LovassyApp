using Blueboard.Core.Auth.Services;
using Blueboard.Core.Lolo.Exceptions;
using Blueboard.Core.Lolo.Services.Options;
using Blueboard.Infrastructure.Persistence;
using Blueboard.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace Blueboard.Core.Lolo.Services;

/// <summary>
///     The scoped service responsible for managing the user's lolo coins. (That includes loading, spending and
///     generating/saving
///     coins)
/// </summary>
public class LoloManager
{
    private readonly ApplicationDbContext _context;
    private readonly HashManager _hashManager;
    private readonly LoloOptions _loloOptions;
    private readonly IMemoryCache _memoryCache;
    private readonly UserAccessor _userAccessor;

    private Guid? _userId;

    public LoloManager(IOptions<LoloOptions> loloOptions, ApplicationDbContext context, IMemoryCache memoryCache,
        HashManager hashManager, UserAccessor userAccessor)
    {
        _loloOptions = loloOptions.Value;
        _context = context;
        _memoryCache = memoryCache;
        _hashManager = hashManager;
        _userAccessor = userAccessor;
    }

    public int? Balance { get; private set; }
    public List<Infrastructure.Persistence.Entities.Lolo>? Coins { get; private set; }

    /// <summary>
    ///     Loads the user's lolo coins and balance from the database.
    /// </summary>
    public async Task LoadAsync()
    {
        if (_userId == null)
            Init();

        var userIdHashed = _hashManager.HashWithHasherSalt(_userId.ToString());

        var grades = await _context.Grades
            .Where(g => g.UserIdHashed == userIdHashed && g.LoloIdHashed != null)
            .AsNoTracking()
            .ToListAsync();

        Coins = await _context.Lolos.Where(l => l.UserId == _userId)
            .AsNoTracking()
            .ToListAsync();

        foreach (var coin in Coins.Where(c => c.LoloType == LoloType.FromGrades))
        {
            coin.Grades ??= new List<Grade>();

            coin.Grades.AddRange(
                grades.Where(g => g.LoloIdHashed == _hashManager.HashWithHasherSalt(coin.Id.ToString())));
        }

        Balance = Coins.Count(c => !c.IsSpent);
    }

    /// <summary>
    ///     Spends a given amount of lolo coins.
    /// </summary>
    /// <param name="amount">How many coins to spend.</param>
    /// <param name="updateManager">
    ///     Whether to update the <see cref="Coins" /> and <see cref="Balance" /> properties of the
    ///     <see cref="LoloManager" />.
    /// </param>
    /// <exception cref="InsufficientFundsException">The exception thrown when the current user doesn't have enough lolo coins.</exception>
    public async Task SpendAsync(int amount, bool updateManager = true)
    {
        if ((bool?)_memoryCache.Get(_loloOptions.LoloManagerLockPrefix + _userId) == true)
            return;

        _memoryCache.Set(_loloOptions.LoloManagerLockPrefix + _userId, true, TimeSpan.FromSeconds(10));

        if (_userId == null)
            Init();

        var coins = await _context.Lolos.Where(l => l.UserId == _userId && !l.IsSpent)
            .Take(amount)
            .ToListAsync();

        if (coins.Count < amount)
            throw new InsufficientFundsException();

        foreach (var coin in coins)
            coin.IsSpent = true;

        await _context.SaveChangesAsync();

        if (updateManager)
            await LoadAsync();

        _memoryCache.Remove(_loloOptions.LoloManagerLockPrefix + _userId);
    }

    /// <summary>
    ///     Saves the given amount of lolo coins when a request is accepted.
    /// </summary>
    /// <param name="request">The request from which the coins are generated.</param>
    /// <param name="amount">The amount of new coins to save.</param>
    public async Task SaveFromRequestAsync(LoloRequest request, int amount)
    {
        if (_userId == null)
            Init();

        var coins = Enumerable.Repeat(new Infrastructure.Persistence.Entities.Lolo
        {
            UserId = _userId!.Value,
            Reason = $"Kérvényből generálva: {request.Title}",
            LoloType = LoloType.FromRequest,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        }, amount).ToArray();

        await _context.Database.BeginTransactionAsync();

        foreach (var coin in coins) await _context.Lolos.AddAsync(coin);

        await _context.SaveChangesAsync();

        await _context.Database.CommitTransactionAsync();
    }

    /// <summary>
    ///     Generates lolo coins from grades.
    /// </summary>
    public async Task GenerateAsync()
    {
        if (_userId == null)
            Init();

        if ((bool?)_memoryCache.Get(_loloOptions.LoloManagerLockPrefix + _userId) == true)
            return;

        _memoryCache.Set(_loloOptions.LoloManagerLockPrefix + _userId, true, TimeSpan.FromSeconds(10));

        //We can't do this in parallel because the DbContext really doesn't like being used by two threads at once 
        await GenerateFromFivesAsync();
        await GenerateFromFoursAsync();

        _memoryCache.Remove(_loloOptions.LoloManagerLockPrefix + _userId);
    }

    private async Task GenerateFromFivesAsync()
    {
        var userIdHashed = _hashManager.HashWithHasherSalt(_userId.ToString()!);

        var grades = await _context.Grades
            .Where(g => g.UserIdHashed == userIdHashed && g.LoloIdHashed == null &&
                        g.GradeType == GradeType.RegularGrade && g.GradeValue == 5)
            .ToListAsync();

        var chunkSize = _loloOptions.FiveThreshold;
        var fullChunksCount = grades.Count / chunkSize;

        for (var i = 0; i < fullChunksCount; i++)
        {
            var gradeGroup = grades.Skip(i * chunkSize).Take(chunkSize).ToArray();
            await SaveLoloFromGrades(gradeGroup, _loloOptions.FiveReason);
        }
    }

    private async Task GenerateFromFoursAsync()
    {
        var userIdHashed = _hashManager.HashWithHasherSalt(_userId.ToString()!);

        var grades = await _context.Grades
            .Where(g => g.UserIdHashed == userIdHashed && g.LoloIdHashed == null &&
                        g.GradeType == GradeType.RegularGrade && g.GradeValue == 4)
            .ToListAsync();

        var chunkSize = _loloOptions.FourThreshold;
        var fullChunksCount = grades.Count / chunkSize;

        for (var i = 0; i < fullChunksCount; i++)
        {
            var gradeGroup = grades.Skip(i * chunkSize).Take(chunkSize).ToArray();
            await SaveLoloFromGrades(gradeGroup, _loloOptions.FourReason);
        }
    }

    private async Task SaveLoloFromGrades(Grade[] gradeGroup, string reason)
    {
        var lolo = new Infrastructure.Persistence.Entities.Lolo
        {
            UserId = _userId!.Value,
            Reason = reason,
            LoloType = LoloType.FromGrades
        };

        await using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            await _context.Lolos.AddAsync(lolo);
            await _context.SaveChangesAsync();

            var loloIdHashed = _hashManager.HashWithHasherSalt(lolo.Id.ToString());

            foreach (var grade in gradeGroup) grade.LoloIdHashed = loloIdHashed;

            await _context.SaveChangesAsync();

            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    private void Init()
    {
        _userId = _userAccessor.User.Id;
    }
}
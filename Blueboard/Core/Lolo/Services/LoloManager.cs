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
public class LoloManager(IOptions<LoloOptions> loloOptions, ApplicationDbContext context, IMemoryCache memoryCache,
    HashManager hashManager, UserAccessor userAccessor)
{
    private Guid? _userId;

    public int? Balance { get; private set; }
    public List<Infrastructure.Persistence.Entities.Lolo>? Coins { get; private set; }

    /// <summary>
    ///     Loads the user's lolo coins and balance from the database.
    /// </summary>
    public async Task LoadAsync()
    {
        if (_userId == null)
            Init();

        var userIdHashed = hashManager.HashWithHasherSalt(_userId.ToString());

        var grades = await context.Grades
            .Where(g => g.UserIdHashed == userIdHashed && g.LoloIdHashed != null)
            .AsNoTracking()
            .ToListAsync();

        Coins = await context.Lolos.Where(l => l.UserId == _userId)
            .AsNoTracking()
            .ToListAsync();

        foreach (var coin in Coins.Where(c => c.LoloType == LoloType.FromGrades))
        {
            coin.Grades ??= new List<Grade>();

            coin.Grades.AddRange(
                grades.Where(g => g.LoloIdHashed == hashManager.HashWithHasherSalt(coin.Id.ToString())));
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
        if ((bool?)memoryCache.Get(loloOptions.Value.LoloManagerLockPrefix + _userId) == true)
            return;

        memoryCache.Set(loloOptions.Value.LoloManagerLockPrefix + _userId, true, TimeSpan.FromSeconds(10));

        if (_userId == null)
            Init();

        var coins = await context.Lolos.Where(l => l.UserId == _userId && !l.IsSpent)
            .Take(amount)
            .ToListAsync();

        if (coins.Count < amount)
            throw new InsufficientFundsException();

        foreach (var coin in coins)
            coin.IsSpent = true;

        await context.SaveChangesAsync();

        if (updateManager)
            await LoadAsync();

        memoryCache.Remove(loloOptions.Value.LoloManagerLockPrefix + _userId);
    }

    /// <summary>
    ///     Saves the given amount of lolo coins when a request is accepted.
    /// </summary>
    /// <param name="request">The request from which the coins are generated.</param>
    /// <param name="amount">The amount of new coins to save.</param>
    public async Task SaveFromRequestAsync(LoloRequest request, int amount)
    {
        var coins = Enumerable.Range(1, amount).Select(i => new Infrastructure.Persistence.Entities.Lolo
        {
            UserId = request.UserId,
            Reason = $"Kérvényből generálva: {request.Title}",
            LoloType = LoloType.FromRequest,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        }).ToArray();

        await context.Database.BeginTransactionAsync();

        await context.Lolos.AddRangeAsync(coins);

        await context.SaveChangesAsync();

        await context.Database.CommitTransactionAsync();
    }

    /// <summary>
    ///     Generates lolo coins from grades.
    /// </summary>
    public async Task GenerateAsync()
    {
        if (_userId == null)
            Init();

        if ((bool?)memoryCache.Get(loloOptions.Value.LoloManagerLockPrefix + _userId) == true)
            return;

        memoryCache.Set(loloOptions.Value.LoloManagerLockPrefix + _userId, true, TimeSpan.FromSeconds(10));

        //We can't do this in parallel because the DbContext really doesn't like being used by two threads at once 
        await GenerateFromFivesAsync();
        await GenerateFromFoursAsync();

        memoryCache.Remove(loloOptions.Value.LoloManagerLockPrefix + _userId);
    }

    private async Task GenerateFromFivesAsync()
    {
        var userIdHashed = hashManager.HashWithHasherSalt(_userId.ToString()!);

        var grades = await context.Grades
            .Where(g => g.UserIdHashed == userIdHashed && g.LoloIdHashed == null &&
                        g.GradeType == GradeType.RegularGrade && g.GradeValue == 5)
            .ToListAsync();

        var chunkSize = loloOptions.Value.FiveThreshold;
        var fullChunksCount = grades.Count / chunkSize;

        for (var i = 0; i < fullChunksCount; i++)
        {
            var gradeGroup = grades.Skip(i * chunkSize).Take(chunkSize).ToArray();
            await SaveLoloFromGrades(gradeGroup, loloOptions.Value.FiveReason);
        }
    }

    private async Task GenerateFromFoursAsync()
    {
        var userIdHashed = hashManager.HashWithHasherSalt(_userId.ToString()!);

        var grades = await context.Grades
            .Where(g => g.UserIdHashed == userIdHashed && g.LoloIdHashed == null &&
                        g.GradeType == GradeType.RegularGrade && g.GradeValue == 4)
            .ToListAsync();

        var chunkSize = loloOptions.Value.FourThreshold;
        var fullChunksCount = grades.Count / chunkSize;

        for (var i = 0; i < fullChunksCount; i++)
        {
            var gradeGroup = grades.Skip(i * chunkSize).Take(chunkSize).ToArray();
            await SaveLoloFromGrades(gradeGroup, loloOptions.Value.FourReason);
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

        await using var transaction = await context.Database.BeginTransactionAsync();

        try
        {
            await context.Lolos.AddAsync(lolo);
            await context.SaveChangesAsync();

            var loloIdHashed = hashManager.HashWithHasherSalt(lolo.Id.ToString());

            foreach (var grade in gradeGroup) grade.LoloIdHashed = loloIdHashed;

            await context.SaveChangesAsync();

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
        _userId = userAccessor.User.Id;
    }
}
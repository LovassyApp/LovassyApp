using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using WebApi.Core.Cryptography.Services;
using WebApi.Core.Lolo.Exceptions;
using WebApi.Core.Lolo.Services.Options;
using WebApi.Infrastructure.Persistence;
using WebApi.Infrastructure.Persistence.Entities;

namespace WebApi.Core.Lolo.Services;

public class LoloManager
{
    private readonly ApplicationDbContext _context;
    private readonly HashManager _hashManager;
    private readonly LoloOptions _loloOptions;
    private readonly IMemoryCache _memoryCache;

    private Guid? _userId;

    public LoloManager(IOptions<LoloOptions> loloOptions, ApplicationDbContext context, IMemoryCache memoryCache,
        HashManager hashManager)
    {
        _loloOptions = loloOptions.Value;
        _context = context;
        _memoryCache = memoryCache;
        _hashManager = hashManager;
    }

    public int? Balance { get; private set; }
    public List<Infrastructure.Persistence.Entities.Lolo>? Coins { get; private set; }

    /// <summary>
    ///     Initializes the <see cref="LoloManager" /> with the current user object.
    /// </summary>
    /// <param name="user">The user's, for which we want to manage lolos.</param>
    public void Init(Guid userId)
    {
        _userId = userId;
    }

    /// <summary>
    ///     Loads the user's lolo coins and balance from the database.
    /// </summary>
    public async Task LoadAsync()
    {
        if (_userId == null)
            throw new LoloManagerUserNotFoundException();

        var userIdHashed = _hashManager.HashWithHasherSalt(_userId.ToString());

        var grades = await _context.Grades
            .Where(g => g.UserIdHashed == userIdHashed && g.LoloIdHashed != null)
            .ToListAsync();

        Coins = await _context.Lolos.Where(l => l.UserId == _userId)
            .ToListAsync();

        foreach (var coin in Coins)
        {
            if (coin.Grades == null)
                coin.Grades = new List<Grade>();

            coin.Grades.AddRange(
                grades.Where(g => g.LoloIdHashed == _hashManager.HashWithHasherSalt(coin.Id.ToString())));
        }

        Balance = await _context.Lolos
            .Where(l => l.UserId == _userId && !l.IsSpent)
            .CountAsync();
    }

    /// <summary>
    ///     Generates lolo coins from grades.
    /// </summary>
    /// <exception cref="LoloManagerUserNotFoundException">
    ///     The exception thrown when the <see cref="LoloManager" /> has not
    ///     been initialized yet.
    /// </exception>
    public async Task GenerateAsync()
    {
        if (_userId == null)
            throw new LoloManagerUserNotFoundException();

        if ((bool?)_memoryCache.Get(_loloOptions.ManagerLockPrefix + _userId) == true)
            return;

        _memoryCache.Set(_loloOptions.ManagerLockPrefix + _userId, true, TimeSpan.FromSeconds(10));

        //We can't do this in parallel because the DbContext really doesn't like being used by two threads at once 
        await GenerateFromFivesAsync();
        await GenerateFromFoursAsync();

        _memoryCache.Remove(_loloOptions.ManagerLockPrefix + _userId);
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
}
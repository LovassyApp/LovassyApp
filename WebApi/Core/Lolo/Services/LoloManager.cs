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

    private string? _userId;

    //TODO: Optimize this class, it's pretty slow (although it only does generation in the background)
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
    public void Init(string userId)
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

        var grades = await _context.Grades
            .Where(g => g.UserIdHashed == _hashManager.HashWithHasherSalt(_userId))
            .Where(g => g.LoloIdHashed != null)
            .GroupBy(g => g.LoloIdHashed)
            .ToListAsync();

        Coins = await _context.Lolos.Where(l => l.UserId == Guid.Parse(_userId))
            .ToListAsync();

        Coins = Coins.Select(c => new Infrastructure.Persistence.Entities.Lolo
        {
            Id = c.Id, IsSpent = c.IsSpent, UserId = c.UserId,
            Grades = grades.Where(g => g.Key == _hashManager.HashWithHasherSalt(c.Id.ToString())).SelectMany(g => g)
                .ToList()
        }).ToList();

        Balance = await _context.Lolos.Where(l => l.UserId == Guid.Parse(_userId)).Where(l => l.IsSpent == false)
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

        await GenerateFromFivesAsync();
        await GenerateFromFoursAsync();

        _memoryCache.Remove(_loloOptions.ManagerLockPrefix + _userId);
    }

    private async Task GenerateFromFivesAsync()
    {
        var grades = await _context.Grades
            .Where(g => g.UserIdHashed == _hashManager.HashWithHasherSalt(_userId))
            .Where(g => g.LoloIdHashed == null).Where(g => g.GradeType == GradeType.RegularGrade)
            .Where(g => g.GradeValue == 5).ToListAsync();

        var chunkedGrades = grades.Chunk(_loloOptions.FiveThreshold).ToList();

        foreach (var gradeGroup in chunkedGrades)
        {
            if (gradeGroup.Length < _loloOptions.FiveThreshold)
                continue;

            await SaveLoloFromGrades(gradeGroup, _loloOptions.FiveReason);
        }
    }

    private async Task GenerateFromFoursAsync()
    {
        var grades = await _context.Grades
            .Where(g => g.UserIdHashed == _hashManager.HashWithHasherSalt(_userId))
            .Where(g => g.LoloIdHashed == null).Where(g => g.GradeType == GradeType.RegularGrade)
            .Where(g => g.GradeValue == 4).ToListAsync();

        var chunkedGrades = grades.Chunk(_loloOptions.FourThreshold).ToList();

        foreach (var gradeGroup in chunkedGrades)
        {
            if (gradeGroup.Length < _loloOptions.FourThreshold)
                continue;

            await SaveLoloFromGrades(gradeGroup, _loloOptions.FourReason);
        }
    }

    private async Task SaveLoloFromGrades(Grade[] gradeGroup, string reason)
    {
        var lolo = new Infrastructure.Persistence.Entities.Lolo
        {
            UserId = Guid.Parse(_userId),
            Reason = reason,
            LoloType = LoloType.FromGrades
        };

        await _context.Lolos.AddAsync(lolo);
        await _context.SaveChangesAsync();

        try
        {
            foreach (var grade in gradeGroup) grade.LoloIdHashed = _hashManager.HashWithHasherSalt(lolo.Id.ToString());

            await _context.SaveChangesAsync();
        }
        catch
        {
            _context.Lolos.Remove(lolo);
            await _context.SaveChangesAsync();
            throw;
        }
    }
}
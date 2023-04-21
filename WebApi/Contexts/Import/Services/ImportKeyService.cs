using System.Security.Cryptography;
using Mapster;
using Microsoft.EntityFrameworkCore;
using WebApi.Contexts.Import.Models;
using WebApi.Helpers.Cryptography.Services;
using WebApi.Helpers.Cryptography.Traits.Extensions;
using WebApi.Persistence;
using WebApi.Persistence.Entities;

namespace WebApi.Contexts.Import.Services;

public class ImportKeyService
{
    private readonly ApplicationDbContext _context;
    private readonly EncryptionService _encryptionService;
    private readonly HashService _hashService;

    public ImportKeyService(ApplicationDbContext context, EncryptionService encryptionService,
        HashService hashService)
    {
        _context = context;
        _encryptionService = encryptionService;
        _hashService = hashService;
    }

    public async Task<List<ImportKey>> IndexImportKeysAsync()
    {
        return await _context.ImportKeys.ToListAsync();
    }

    public async ValueTask<(ImportKey? importKey, string? key)> GetImportKeyAsync(int id)
    {
        var importKey = await _context.ImportKeys.FindAsync(id);
        var key = importKey == null ? null : _encryptionService.Unprotect(importKey.KeyProtected);

        return (importKey, key);
    }

    public async Task UpdateImportKeyAsync(ImportKey importKey, UpdateImportKeyDto data)
    {
        data.Adapt(importKey);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> IsImportKeyValidAsync(string key)
    {
        var keyHashed = _hashService._Hash(key);

        var importKey = await _context.ImportKeys.Where(i => i.KeyHashed == keyHashed).FirstOrDefaultAsync();

        return importKey is { Enabled: true };
    }

    public async Task<string> CreateImportKeyAsync(CreateImportKeyDto data)
    {
        var importKey = data.Adapt<ImportKey>();
        var key = Convert.ToBase64String(RandomNumberGenerator.GetBytes(512 / 8));

        importKey.KeyProtected = _encryptionService.Protect(key);
        importKey.KeyHashed = _hashService._Hash(key);

        await _context.ImportKeys.AddAsync(importKey);
        await _context.SaveChangesAsync();

        return key;
    }

    public async Task DeleteImportKeyAsync(ImportKey importKey)
    {
        _context.ImportKeys.Remove(importKey);
        await _context.SaveChangesAsync();
    }
}
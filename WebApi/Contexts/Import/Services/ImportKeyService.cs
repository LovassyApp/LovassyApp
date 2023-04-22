using System.Security.Cryptography;
using Mapster;
using Microsoft.EntityFrameworkCore;
using WebApi.Contexts.Import.Models;
using WebApi.Helpers.Cryptography.Services;
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

    /// <summary>
    ///     Retrieves all <c>ImportKey</c>s.
    /// </summary>
    /// <returns>The list of <c>ImportKey</c>s.</returns>
    public async Task<List<ImportKey>> IndexImportKeysAsync()
    {
        return await _context.ImportKeys.ToListAsync();
    }

    /// <summary>
    ///     Retrieves an <c>ImportKey</c> and its key in unencrypted form.
    /// </summary>
    /// <param name="id">The id of the <c>ImportKey</c> model.</param>
    /// <returns>The import key model, and the unencrypted key string.</returns>
    public async ValueTask<(ImportKey? importKey, string? key)> GetImportKeyAsync(int id)
    {
        var importKey = await _context.ImportKeys.FindAsync(id);
        var key = importKey == null ? null : _encryptionService.Unprotect(importKey.KeyProtected);

        return (importKey, key);
    }

    /// <summary>
    ///     Updates the fields of an <c>ImportKey</c> model.
    /// </summary>
    /// <param name="importKey">The <c>ImportKey</c> model.</param>
    /// <param name="data">The dto containing the new data to be set.</param>
    public async Task UpdateImportKeyAsync(ImportKey importKey, UpdateImportKeyDto data)
    {
        data.Adapt(importKey);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    ///     Creates a new <c>ImportKey</c>.
    /// </summary>
    /// <param name="data">The dto containing the fields of the new <c>ImportKey</c> model.</param>
    /// <returns>The key string in unencrypted form.</returns>
    public async Task<string> CreateImportKeyAsync(CreateImportKeyDto data)
    {
        var importKey = data.Adapt<ImportKey>();
        var key = Convert.ToBase64String(RandomNumberGenerator.GetBytes(512 / 8));

        importKey.KeyProtected = _encryptionService.Protect(key);
        importKey.KeyHashed = _hashService.Hash(key);

        await _context.ImportKeys.AddAsync(importKey);
        await _context.SaveChangesAsync();

        return key;
    }

    /// <summary>
    ///     Deletes an <c>ImportKey</c> from the database.
    /// </summary>
    /// <param name="importKey">The model itself to delete.</param>
    public async Task DeleteImportKeyAsync(ImportKey importKey)
    {
        _context.ImportKeys.Remove(importKey);
        await _context.SaveChangesAsync();
    }
}
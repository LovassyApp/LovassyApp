using WebApi.Contexts.Import.Models;
using WebApi.Persistence.Entities;

namespace WebApi.Contexts.Import.Services;

public interface IImportKeyService
{
    public Task<List<ImportKey>> IndexImportKeysAsync();
    public ValueTask<(ImportKey? importKey, string? key)> GetImportKeyAsync(int id);

    public Task<string> CreateImportKeyAsync(CreateImportKeyDto data);
    public Task DeleteImportKeyAsync(ImportKey importKey);

    public Task UpdateImportKeyAsync(ImportKey importKey, UpdateImportKeyDto data);

    public Task<bool> IsImportKeyValidAsync(string key);
}
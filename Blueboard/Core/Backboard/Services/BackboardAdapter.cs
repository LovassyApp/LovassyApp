using System.Text.Json;
using Blueboard.Core.Auth.Services;
using Blueboard.Core.Backboard.Exceptions;
using Blueboard.Core.Backboard.Models;
using Blueboard.Core.Backboard.Services.Options;
using Blueboard.Core.Backboard.Utils;
using Blueboard.Infrastructure.Persistence;
using Blueboard.Infrastructure.Persistence.Entities;
using EFCore.BulkExtensions;
using Helpers.Cryptography.Implementations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace Blueboard.Core.Backboard.Services;

/// <summary>
///     The scoped service responsible for converting the encrypted contents of <see cref="GradeImport" /> models from
///     Backboard into <see cref="Grade" /> models.
/// </summary>
public class BackboardAdapter(ApplicationDbContext context, EncryptionManager encryptionManager,
    HashManager hashManager,
    IMemoryCache memoryCache, UserAccessor userAccessor, IOptions<BackboardOptions> backboardOptions)
{
    private User? _user;

    /// <summary>
    ///     Attempts to update the user's grades, real name and class. It only works if a <see cref="GradeImport" /> for the
    ///     user exists.
    /// </summary>
    /// <exception cref="InvalidImportException">
    ///     The exception thrown when invalid grade import data is found in the database
    ///     (not decryptable or deserializable).
    /// </exception>
    public async Task UpdateAsync()
    {
        if (_user == null)
            Init();

        if ((bool?)memoryCache.Get(backboardOptions.Value.BackboardAdapterLockPrefix + _user.Id) == true)
            return;

        memoryCache.Set(backboardOptions.Value.BackboardAdapterLockPrefix + _user.Id, true,
            TimeSpan.FromSeconds(10)); // Lock thread

        BackboardGradeCollection? gradeCollection;
        try
        {
            gradeCollection = await GetUpdatedGradeCollectionAsync();
        }
        catch (Exception)
        {
            throw new InvalidImportException();
        }

        if (gradeCollection == null)
            return;

        _user.RealName = gradeCollection.StudentName.Trim();
        if (gradeCollection.SchoolClass != null)
            _user.Class = gradeCollection.SchoolClass.Trim(); // For some reason, the class can contain leading spaces

        var grades = TransformGrades(gradeCollection);

        await context.BulkInsertOrUpdateAsync(grades, new BulkConfig
        {
            UpdateByProperties = new List<string> { nameof(Grade.Uid) },
            PropertiesToExclude = new List<string> { nameof(Grade.Id), nameof(Grade.LoloIdHashed) }
        });
        await context.GradeImports.Where(i => i.UserId == _user.Id).BatchDeleteAsync();

        _user.ImportAvailable = false;

        await context.SaveChangesAsync(); // only necessary for the changes to the user

        memoryCache.Remove(backboardOptions.Value.BackboardAdapterLockPrefix + _user.Id);
    }

    private async Task<BackboardGradeCollection?> GetUpdatedGradeCollectionAsync()
    {
        if (!_user!.ImportAvailable)
            return null;

        var gradeImport = await context.GradeImports.Where(i => i.UserId == _user.Id).OrderByDescending(i => i.Id)
            .FirstOrDefaultAsync();

        var keyPair = new KyberKeypair(encryptionManager.Decrypt(_user.PrivateKeyEncrypted));
        var gradeCollectionString = keyPair.Decrypt(gradeImport!.JsonEncrypted);

        var gradeCollection = JsonSerializer.Deserialize<BackboardGradeCollection>(gradeCollectionString);

        return gradeCollection;
    }

    private List<Grade> TransformGrades(BackboardGradeCollection gradeCollection)
    {
        var grades = new List<Grade>();

        foreach (var grade in gradeCollection.Grades)
            grades.Add(BackboardUtils.TransformBackboardGrade(grade,
                hashManager.HashWithHasherSalt(_user!.Id.ToString())));

        return grades;
    }

    private void Init()
    {
        _user = userAccessor.User;
    }
}
using Microsoft.EntityFrameworkCore;
using WebApi.Contexts.Users.Models;
using WebApi.Core.Cryptography.Exceptions;
using WebApi.Core.Cryptography.Models;
using WebApi.Core.Cryptography.Services;
using WebApi.Persistence;
using WebApi.Persistence.Entities;

namespace WebApi.Contexts.Users.Services;

public class UsersService
{
    private readonly ApplicationDbContext _context;
    private readonly EncryptionManager _encryptionManager;
    private readonly HashService _hashService;
    private readonly ResetService _resetService;

    public UsersService(ApplicationDbContext context, HashService hashService, EncryptionManager encryptionManager,
        ResetService resetService)
    {
        _context = context;
        _hashService = hashService;
        _encryptionManager = encryptionManager;
        _resetService = resetService;
    }

    /// <summary>
    ///     Creates a new user. (Used for registration)
    /// </summary>
    /// <param name="data">The name, email, password ect. of the user.</param>
    /// <exception cref="ResetKeyPasswordMissingException">
    ///     The exception thrown when no reset key password has been set since
    ///     the application is running. (Without it, future password resets would be impossible)
    /// </exception>
    public async Task CreateUserAsync(CreateUserDto data)
    {
        if (!_resetService.IsResetKeyPasswordSet()) throw new ResetKeyPasswordMissingException();

        var masterKeySalt = _hashService.GenerateSalt();
        var masterKey = new EncryptableKey();
        _encryptionManager.Init(masterKey.GetKey());

        var storedKey = masterKey.Lock(data.Password, masterKeySalt);

        var keys = new KyberKeypair();

        var hasherSalt = _hashService.GenerateSalt();

        var user = new User
        {
            Email = data.Email,
            Name = data.Name,
            PasswordHashed = _hashService.HashPassword(data.Password),
            PublicKey = keys.PublicKey,
            PrivateKeyEncrypted = _encryptionManager.Encrypt(keys.PrivateKey),
            MasterKeyEncrypted = storedKey,
            MasterKeySalt = masterKeySalt,
            ResetKeyEncrypted = _resetService.EncryptMasterKey(_encryptionManager.MasterKey!),
            HasherSaltEncrypted = _encryptionManager.Encrypt(hasherSalt),
            HasherSaltHashed = _hashService.Hash(hasherSalt),
            OmCodeEncrypted = _encryptionManager.Encrypt(data.OmCode),
            OmCodeHashed = _hashService.Hash(data.OmCode)
        };

        await _context.Users.AddAsync(user);

        //TODO: Add user to default group

        await _context.SaveChangesAsync();
    }

    /// <summary>
    ///     Checks weather the email ends with the given suffix.
    /// </summary>
    /// <param name="email">The email to check.</param>
    /// <param name="suffix">The suffix to check for.</param>
    /// <returns>A boolean indicating whether it ends with the suffix or not.</returns>
    public bool CheckEmailSuffix(string email, string suffix)
    {
        return email.EndsWith(suffix);
    }

    /// <summary>
    ///     Checks weather a user with the given email is already registered.
    /// </summary>
    /// <param name="email">The email to check for.</param>
    /// <returns>A boolean indicating whether a user already exists with that email or not.</returns>
    public async Task<bool> CheckEmailRegisteredAsync(string email)
    {
        return await _context.Users.AnyAsync(u => u.Email == email);
    }

    /// <summary>
    ///     Checks weather a user with the given om code is already registered.
    /// </summary>
    /// <param name="omCode">The om code to check for.</param>
    /// <returns>A boolean indicating whether a user already exists with that om code or not.</returns>
    public async Task<bool> CheckOmCodeRegisteredAsync(string omCode)
    {
        return await _context.Users.AnyAsync(u => u.OmCodeHashed == _hashService.Hash(omCode));
    }
}
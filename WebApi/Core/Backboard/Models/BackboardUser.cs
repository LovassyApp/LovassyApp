using WebApi.Infrastructure.Persistence.Entities;

namespace WebApi.Core.Backboard.Models;

/// <summary>
///     The model representing a user from Backboard, should map to a <see cref="User" /> in the database.
/// </summary>
public class BackboardUser
{
    public Guid Id { get; set; }
    public string PublicKey { get; set; }
    public string OmCodeHashed { get; set; }
}
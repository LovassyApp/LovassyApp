using WebApi.Contexts.Users.Models;
using WebApi.Persistence;

namespace WebApi.Contexts.Users.Services;

public class UsersService
{
    private readonly ApplicationDbContext _context;


    public Task CreateUserAsync(CreateUserDto data)
    {
        throw new NotImplementedException();
    }
}
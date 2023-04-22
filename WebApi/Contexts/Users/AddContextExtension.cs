using WebApi.Contexts.Users.Services;

namespace WebApi.Contexts.Users;

public static class AddContextExtension
{
    public static void AddUsersContext(this IServiceCollection services)
    {
        services.AddScoped<UsersService>();
    }
}
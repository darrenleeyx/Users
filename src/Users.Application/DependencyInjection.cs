using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Users.Application.Models;
using Users.Application.Providers;
using Users.Application.Repositories;
using Users.Application.Services;

namespace Users.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services,
        List<User>? users)
    {
        services.AddSingleton<IUserRepository>(x => new UserRepository(users));
        services.AddTransient<IUserService, UserService>();
        services.AddTransient<IDateTimeProvider, DateTimeProvider>();
        services.AddValidatorsFromAssemblyContaining<IApplicationMarker>(ServiceLifetime.Singleton);
        return services;
    }
}

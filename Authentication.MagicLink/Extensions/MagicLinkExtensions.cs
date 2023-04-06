using Microsoft.Extensions.DependencyInjection;

namespace Authentication.MagicLink.Extensions;

public static class MagicLinkExtensions
{
    public static IServiceCollection AddMagicLinkAuthentication(this IServiceCollection services, Action<MagicLinkSettings> configureOptions)
    {
        services.Configure(configureOptions);
        services.AddSingleton<IUserService, UserService>();
        services.AddSingleton<IMagicLinkService, MagicLinkService>();
        services.AddSingleton<ITokenGenerator, JwtTokenGenerator>();
        services.AddSingleton<ITokenValidator, JwtTokenValidator>();

        return services;
    }

    public static IServiceCollection AddEmailProvider<TEmailService>(this IServiceCollection services)
        where TEmailService : class, IEmailService
    {
        services.AddSingleton<IEmailService, TEmailService>();
        return services;
    }
}

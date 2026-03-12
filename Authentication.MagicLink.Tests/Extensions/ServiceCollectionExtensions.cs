using Authentication.MagicLink.Tests.Mocks;

namespace Authentication.MagicLink.Tests.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddNoSendEmailService(this IServiceCollection services, out NoSendEmailService emailService)
    {
        emailService = new NoSendEmailService();
        services.AddSingleton<IEmailService>(emailService);
        return services;
    }
}

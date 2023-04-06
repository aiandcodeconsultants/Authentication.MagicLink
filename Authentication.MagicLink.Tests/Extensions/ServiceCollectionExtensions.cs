using Authentication.MagicLink.Tests.Mocks;
//using MailKit.Net.Smtp;
//using Moq;

namespace Authentication.MagicLink.Tests.Extensions;

public static class ServiceCollectionExtensions
{
    //public static IServiceCollection AddSmtpClient(this IServiceCollection services)
    //    => services.AddTransient<System.Net.Mail.SmtpClient>();

    //public static IServiceCollection AddMailKitSmtpClient(this IServiceCollection services)
    //    //=> services.AddTransient<ISmtpClient>(serviceProvider => new SmtpClient());
    //    => services.AddTransient<ISmtpClient>(serviceProvider =>
    //    {
    //        var settings = serviceProvider.GetRequiredService<IOptions<MagicLinkSettings>>().Value;
    //        var client = new SmtpClient();
    //        client.Connect(settings.MailServer, settings.MailKitPort, settings.MailKitUseSsl);
    //        if(!string.IsNullOrEmpty(settings.MailKitUsername) && !string.IsNullOrEmpty(settings.MailKitPassword))
    //        {
    //            client.Authenticate(settings.MailKitUsername, settings.MailKitPassword);
    //        }
    //        return client;
    //    });

    //public static IServiceCollection AddMockEmailService(this IServiceCollection services, out Mock<IEmailService> mockEmailService, Action<Mock<IEmailService>> configureMock = null)
    //{
    //    mockEmailService = new Mock<IEmailService>();
    //    configureMock?.Invoke(mockEmailService);
    //    services.AddSingleton(mockEmailService.Object);
    //    return services;
    //}
    
    public static IServiceCollection AddNoSendEmailService(this IServiceCollection services, out NoSendEmailService emailService)
    {
        emailService = new NoSendEmailService();
        services.AddSingleton<IEmailService>(emailService);
        return services;
    }
}

# Authentication.MagicLink

## 1. Introduction

Authentication.MagicLink is a library designed to provide magic link authentication support for ASP.NET Core applications. It streamlines the process of implementing passwordless authentication via one-time-use tokens sent to users through email. The library allows you to configure email providers, token generation, and validation for seamless integration with your application.

## 2. Prerequisites

Before you can use Authentication.MagicLink, ensure your development environment meets the following requirements:

- .NET 7.0 or later
- ASP.NET Core 7.0 or later

## 3. Installation

To install the Authentication.MagicLink library, use the NuGet Package Manager. You can either use the command line or search for the package within Visual Studio. To install via the command line, run the following command:

```sh
dotnet add package Authentication.MagicLink
```

In Visual Studio, you can search for the package by navigating to Tools > NuGet Package Manager > Manage NuGet Packages for Solution, then search for "Authentication.MagicLink" and install it to your desired project.

## 4. Basic Usage

To use Authentication.MagicLink in your ASP.NET Core application, follow these steps:

1. Add the necessary `using` statements at the top of your `Program.cs` or `Startup.cs` file:

   ```csharp
   using Authentication.MagicLink.Extensions;
   using Authentication.MagicLink.Services;
   ```

2. Configure the `Authentication.MagicLink` services by calling the `AddAuthenticationMagicLink` extension method inside your `ConfigureServices` method:

   ```csharp
   services
     .AddAuthenticationMagicLink(Configuration)
     .AddEmailProvider<MailKitEmailService>(); // Replace with your desired email provider
   ```

3. In your `Configure` method, add the following lines to enable authentication:
   ```csharp
   app.UseAuthentication();
   app.UseAuthorization();
   ```

4. Use the `MapMagicLink` extension method to set up magic link authentication endpoints:
   ```csharp
   app.MapMagicLink();
   ```
5. Secure your endpoints with the `[Authorize]` attribute or the `RequireAuthorization` method.

## 5. Configuration

The `AddAuthenticationMagicLink` method accepts an `IConfiguration` instance that is used to configure magic link options. You can set these options in your appsettings.json file, with the following structure:
   ```json
   {
  "MagicLink": {
      "Issuer": "YourAppName",
      "TokenLifeTimeMinutes": 15,
      "TokenSecretKey": "YourTokenSecretKey",
      "JwtIssuer": "YourJwtIssuer",
      "JwtAudience": "YourJwtAudience",
      "JwtSecretKey": "YourJwtSecretKey"
    }
  }
   ```
Replace the placeholder values with your actual values.

## 6. Customization
You can customize various aspects of the magic link authentication process:

### 6.1. Email Provider
The library supports custom email providers. You'll need to create a class implementing the IEmailService interface and register it with the AddEmailProvider method.

### 6.2. Token Generation and Validation
You can customize token generation and validation by implementing the IMagicLinkService interface and registering your custom implementation with the AddMagicLinkService method.

### 6.3. Endpoints
The default endpoints for magic link authentication can be customized using the MapMagicLink method's optional parameters.

## 7. Contributors
To contribute to the Authentication.MagicLink project, follow these steps:

1. Clone the repository.
2. Create a branch for your changes.
3. Make changes and run tests.
4. Submit a pull request with your changes.

### 7.1. Building and Publishing NuGet Package
To build and publish the NuGet package, use the following commands:
    ```sh
    dotnet pack -c Release --include-symbols
    dotnet nuget push <PathToYour.nupkgFile> -k <YourNuGetApiKey> -s https://api.nuget.org/v3/index.json
    ```

Replace `<PathToYour.nupkgFile>` with the path to the generated `.nupkg` file and `<YourNuGetApiKey>` with your NuGet API key.

## 8. License

This code is provided for educational purposes and use in private projects and dev/test instances.  Use in production environments is unsupported and at your own risk.  Open derivative works are encouraged, but closed-source repurposing of this code is not.
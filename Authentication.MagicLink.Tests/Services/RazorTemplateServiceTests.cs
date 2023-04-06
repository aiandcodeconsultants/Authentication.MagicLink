using RazorLight;
using RazorLight.Compilation;
using System.Reflection;

namespace Authentication.MagicLink.Tests.Services;

public class RazorTemplateServiceTests
{
    private readonly IRenderTemplate _razorTemplateService;
    public RazorTemplateServiceTests()
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.test.json", false)
            .Build();

        var basePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        //var razorTemplateService = new RazorTemplateService(basePath);
        //var razorTemplateService = new RazorTemplateService(basePath);

        var services = new ServiceCollection()
            .AddAuthenticationMagicLink(configuration)
            .AddSingleton<IRenderTemplate>(sp => new RazorTemplateService(basePath!))
            .BuildServiceProvider();

        _razorTemplateService = services.GetRequiredService<IRenderTemplate>();
    }

    [Fact]
    public async Task RenderAsync_WithValidTemplateAndModel_ReturnsCorrectOutput()
    {
        // Arrange
        var templateName = "ValidTemplate";
        var model = new
        {
            Name = "John Doe",
            Age = 30
        };

        // Act
        var result = await _razorTemplateService.RenderAsync(templateName, model);

        // Assert
        Assert.Contains("John Doe", result);
        Assert.Contains("30", result);
    }

    [Fact]
    public async Task RenderAsync_WithInvalidTemplate_ThrowsException()
    {
        // Arrange
        var templateName = "InvalidTemplate";
        var model = new
        {
            Name = "John Doe",
            Age = 30
        };

        // Act & Assert
        //await Assert.ThrowsAsync<InvalidOperationException>(async () => await _razorTemplateService.RenderAsync(templateName, model));
        await Assert.ThrowsAsync<TemplateCompilationException>(async () => await _razorTemplateService.RenderAsync(templateName, model));
    }

    [Fact]
    public async Task RenderAsync_WithMissingTemplate_ThrowsException()
    {
        // Arrange
        var templateName = "MissingTemplate";
        var model = new
        {
            Name = "John Doe",
            Age = 30
        };

        // Act & Assert
        //await Assert.ThrowsAsync<FileNotFoundException>(async () => await _razorTemplateService.RenderAsync(templateName, model));
        await Assert.ThrowsAsync<TemplateNotFoundException>(async () => await _razorTemplateService.RenderAsync(templateName, model));
    }

    [Fact]
    public async Task RenderAsync_WithDifferentModels_ReturnsCorrectOutput()
    {
        // Arrange
        var templateName = "ValidTemplate";
        var model1 = new
        {
            Name = "John Doe",
            Age = 30
        };
        var model2 = new
        {
            Name = "Jane Doe",
            Age = 28
        };

        // Act
        var result1 = await _razorTemplateService.RenderAsync(templateName, model1);
        var result2 = await _razorTemplateService.RenderAsync(templateName, model2);

        // Assert
        Assert.Contains("John Doe", result1);
        Assert.Contains("30", result1);
        Assert.Contains("Jane Doe", result2);
        Assert.Contains("28", result2);
    }
}

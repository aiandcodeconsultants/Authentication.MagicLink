namespace Authentication.MagicLink.Services;

using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp;

public class RazorTemplateService : IRenderTemplate
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public RazorTemplateService(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    public async Task<string> RenderAsync<TModel>(string templateName, TModel model)
    {
        var templatePath = GetTemplatePath(templateName);

        using var serviceScope = _serviceScopeFactory.CreateScope();
        var razorEngine = serviceScope.ServiceProvider.GetRequiredService<RazorProjectEngine>();

        var projectItem = new FileSystemRazorProjectItem(templatePath, templateName);
        //var codeDocument = await razorEngine.ProcessAsync(projectItem);
        var codeDocument = razorEngine.Process(projectItem);

        var csharpDocument = codeDocument.GetCSharpDocument();
        if (csharpDocument.Diagnostics.Any(d => d.Severity == RazorDiagnosticSeverity.Error))
        {
            throw new InvalidOperationException("Unable to compile Razor template.");
        }

        var assembly = CompileTemplate(csharpDocument);
        var templateType = assembly.GetType($"{templateName}.CompiledTemplate");
        var templateInstance = Activator.CreateInstance(templateType) as ITemplate<TModel>;

        if (templateInstance == null)
        {
            throw new InvalidOperationException("Unable to create an instance of the compiled Razor template.");
        }

        return await templateInstance.ExecuteAsync(model);
    }

    private string GetTemplatePath(string templateName)
    {
        var assemblyLocation = Assembly.GetExecutingAssembly().Location;
        var assemblyDirectory = Path.GetDirectoryName(assemblyLocation);
        return Path.Combine(assemblyDirectory, "Templates", $"{templateName}.cshtml");
    }

    private Assembly CompileTemplate(RazorCSharpDocument csharpDocument)
    {
        // Compile the CSharpDocument into an assembly
        // This code assumes that you have the necessary packages installed for runtime compilation
        // If not, you will need to install the following packages:
        // - Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation
        // - Microsoft.CodeAnalysis.CSharp
        // - Microsoft.CodeAnalysis.Razor

        // Additional implementation details should be added here
        var syntaxTree = CSharpSyntaxTree.ParseText(csharpDocument.GeneratedCode);
        var references = new[]
        {
            MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(TemplateBase<>).Assembly.Location),
            MetadataReference.CreateFromFile(Assembly.Load("netstandard, Version=2.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51").Location),
            MetadataReference.CreateFromFile(Assembly.Load("Microsoft.AspNetCore.Razor.Runtime, Version=6.0.0.0, Culture=neutral, PublicKeyToken=adb9793829ddae60").Location)
        };

        var compilation = CSharpCompilation.Create(
            $"TemplateAssembly_{Guid.NewGuid()}",
            new[] { syntaxTree },
            references,
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

        using var assemblyStream = new MemoryStream();
        var result = compilation.Emit(assemblyStream);

        if (!result.Success)
        {
            var errors = string.Join(Environment.NewLine, result.Diagnostics.Select(d => d.ToString()));
            throw new InvalidOperationException($"Unable to compile Razor template: {errors}");
        }

        assemblyStream.Seek(0, SeekOrigin.Begin);
        return Assembly.Load(assemblyStream.ToArray());
    }
}

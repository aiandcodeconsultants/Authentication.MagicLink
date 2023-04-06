using RazorLight;

namespace Authentication.MagicLink.Tests.Services;
public class RazorTemplateService : IRenderTemplate
{
    private readonly RazorLightEngine _engine;

    public RazorTemplateService(string basePath)
    {
        _engine = new RazorLightEngineBuilder()
            .UseFileSystemProject(basePath)
            .UseMemoryCachingProvider()
            .Build();
    }

    public async Task<string> RenderAsync<TModel>(string templateName, TModel model)
    {
        var templatePath = $"Templates/{templateName}.cshtml";
        string result = await _engine.CompileRenderAsync(templatePath, model);
        return result;
    }
}

/* Previous ASP.NET Core Razor Template Service:-
using System.Reflection;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Razor.Extensions;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.Extensions.FileProviders;

namespace Authentication.MagicLink.Tests.Services
{
    public class RazorTemplateService : IRenderTemplate
    {
        private readonly string _basePath;

        public RazorTemplateService(string basePath)
        {
            _basePath = basePath;
        }

        public async Task<string> RenderAsync<TModel>(string templateName, TModel model)
        {
            //var basePath = Path.GetDirectoryName(typeof(RazorTemplateService).Assembly.Location);
            var basePath = _basePath;
            //var templatePath = Path.Combine(basePath, "Templates", templateName);
            var templatePath = Path.Combine(basePath, "Templates", $"{templateName}.cshtml");

            var fileProvider = new PhysicalFileProvider(basePath);
            var razorFileSystem = new PhysicalFileProviderRazorProjectFileSystem(fileProvider);
            var razorEngine = RazorProjectEngine.Create(RazorConfiguration.Default, razorFileSystem, (builder) =>
            {
                RazorExtensions.Register(builder);
            });

            var projectItem = razorEngine.FileSystem.GetItem(templatePath);
            //var codeDocument = await razorEngine.ProcessAsync(projectItem);
            var codeDocument = razorEngine.Process(projectItem);

            var csharpDocument = codeDocument.GetCSharpDocument();
            if (csharpDocument.Diagnostics.Count > 0)
            {
                throw new InvalidOperationException("Unable to compile Razor template: " + string.Join("\n", csharpDocument.Diagnostics));
            }

            var assembly = CompileTemplate(csharpDocument);

            var type = assembly.GetType("AspNetCore.ValidTemplate");
            var instance = Activator.CreateInstance(type);
            type.GetProperty("Model").SetValue(instance, model);

            var executeAsyncMethod = type.GetMethod("ExecuteAsync", BindingFlags.Instance | BindingFlags.NonPublic);

            await (Task)executeAsyncMethod.Invoke(instance, new object[] { });

            return ((StringWriter)type.GetProperty("Output").GetValue(instance)).ToString();
        }

        private Assembly CompileTemplate(RazorCSharpDocument csharpDocument)
        {
            var syntaxTree = CSharpSyntaxTree.ParseText(csharpDocument.GeneratedCode);

            var compilationOptions = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary)
                .WithOptimizationLevel(OptimizationLevel.Debug)
                .WithPlatform(Platform.AnyCpu)
                .WithMetadataImportOptions(MetadataImportOptions.All);

            var compilation = CSharpCompilation.Create("RazorTemplate_" + Guid.NewGuid(), new[] { syntaxTree }, GetMetadataReferences(), compilationOptions);

            using var stream = new MemoryStream();
            var emitResult = compilation.Emit(stream);

            if (!emitResult.Success)
            {
                throw new InvalidOperationException("Unable to compile Razor template: " + string.Join("\n", emitResult.Diagnostics));
            }

            stream.Seek(0, SeekOrigin.Begin);
            return Assembly.Load(stream.ToArray());
        }

        private IEnumerable<MetadataReference> GetMetadataReferences()
        {
            var basePath = Path.GetDirectoryName(typeof(object).Assembly.Location);

            return new[]
            {
                MetadataReference.CreateFromFile(Path.Combine(basePath, "mscorlib.dll")),
                MetadataReference.CreateFromFile(Path.Combine(basePath, "System.Private.CoreLib.dll")),
                MetadataReference.CreateFromFile(Path.Combine(basePath, "System.Runtime.dll")),
                MetadataReference.CreateFromFile(Path.Combine(basePath, "System.ObjectModel.dll")),
                MetadataReference.CreateFromFile(Path.Combine(basePath, "System.Linq.Expressions.dll")),
                MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(Task).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(RazorTemplateService).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(RazorPage<>).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(Microsoft.CSharp.RuntimeBinder.CSharpArgumentInfo).Assembly.Location),
            };
        }
    }
}
*/
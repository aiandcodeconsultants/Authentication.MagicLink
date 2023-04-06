using Microsoft.AspNetCore.Razor.Language;
using Microsoft.Extensions.FileProviders;

public class FileProviderRazorProjectItem : RazorProjectItem
{
    private readonly IFileInfo _fileInfo;

    public FileProviderRazorProjectItem(IFileInfo fileInfo, string basePath, string filePath)
    {
        _fileInfo = fileInfo;
        BasePath = basePath;
        FilePath = filePath;
    }

    public override string BasePath { get; }

    public override string FilePath { get; }

    public override string? PhysicalPath => _fileInfo.PhysicalPath;

    //public override bool Exists => _fileInfo.Exists;
    public override bool Exists => File.Exists(_fileInfo.Name);

    //public override Stream Read() => _fileInfo.CreateReadStream();
    public override Stream Read() => File.OpenRead(_fileInfo.Name);
}
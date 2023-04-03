using Microsoft.AspNetCore.Razor.Language;

namespace Authentication.MagicLink.Models;

public class FileSystemRazorProjectItem : RazorProjectItem
{
    private readonly string _filePath;

    public FileSystemRazorProjectItem(string filePath, string fileKind)
    {
        _filePath = filePath;
        FileKind = fileKind;
    }

    public override string BasePath => "/";

    public override string FilePath => _filePath;

    public override string PhysicalPath => _filePath;

    public override bool Exists => File.Exists(_filePath);

    public override string FileKind { get; }

    public override Stream Read() => new FileStream(_filePath, FileMode.Open, FileAccess.Read);
}

using Microsoft.AspNetCore.Razor.Language;
using Microsoft.Extensions.FileProviders;

namespace Authentication.MagicLink.Models;

public class PhysicalFileProviderRazorProjectFileSystem : RazorProjectFileSystem
{
    private readonly IFileProvider _fileProvider;

    public PhysicalFileProviderRazorProjectFileSystem(IFileProvider fileProvider) => _fileProvider = fileProvider;

    public override IEnumerable<RazorProjectItem> EnumerateItems(string basePath)
    {
        basePath = NormalizeAndEnsureValidPath(basePath);
        var prefix = basePath.Length == 0 ? string.Empty : basePath + '/';

        var contents = _fileProvider.GetDirectoryContents(basePath);
        foreach (var file in contents)
        {
            if (file.IsDirectory) continue;
            yield return new FileProviderRazorProjectItem(file, basePath, filePath: null);
        }
    }

    public override RazorProjectItem GetItem(string path)
    {
        path = NormalizeAndEnsureValidPath(path);
        return new FileProviderRazorProjectItem(_fileProvider.GetFileInfo(path), basePath: string.Empty, path);
    }

    private static string NormalizeAndEnsureValidPath(string path)
        => Path.DirectorySeparatorChar == '\\'
            ? path.Replace('/', '\\')
            : path.Replace('\\', '/');

    public override RazorProjectItem GetItem(string path, string fileKind) => throw new NotImplementedException();
}
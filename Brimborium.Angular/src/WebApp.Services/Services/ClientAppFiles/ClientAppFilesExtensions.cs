using Microsoft.Extensions.Configuration;

namespace Brimborium.Angular.WebApp.Services.ClientAppFiles;

public static class ClientAppFilesExtensions {
    public static void Bind(this ClientAppFilesOptions options, IConfiguration configuration) {
        if (configuration.GetSection(nameof(options.ListRequestPath)).GetChildren() is { } listChild
            && listChild.Any()) {
            var listRequestPath = new List<PathDocument>();
            foreach (var child in listChild) {
                if (child.GetSection(nameof(PathDocument.Path)).Value is { } path) {
                    if (!(child.GetSection(nameof(PathDocument.Document)).Value is { Length: > 0 } document)) {
                        document = path + "/index.html";
                    }
                    listRequestPath.Add(new PathDocument(path, document));
                }
            }
            options.ListRequestPath = listRequestPath.ToArray();
        }
        if (configuration.GetSection(nameof(options.DefaultFile)).Value is { } defaultFile) {
            options.DefaultFile = defaultFile;
        }
    }
}

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;

namespace Microsoft.AspNetCore.Builder;

internal static class Helpers {
    internal static IFileProvider ResolveFileProvider(IWebHostEnvironment hostingEnv) {
        if (hostingEnv.WebRootFileProvider == null) {
            throw new InvalidOperationException("Missing FileProvider.");
        }
        return hostingEnv.WebRootFileProvider;
    }

    internal static bool IsGetOrHeadMethod(string method) {
        return HttpMethods.IsGet(method) || HttpMethods.IsHead(method);
    }
}
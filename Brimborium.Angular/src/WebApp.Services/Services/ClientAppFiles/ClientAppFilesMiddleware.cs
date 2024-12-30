using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;

namespace Microsoft.AspNetCore.Builder;

internal sealed class ClientAppFilesMiddleware {
    private readonly RequestDelegate _Next;
    private readonly IWebHostEnvironment _HostingEnv;
    private readonly ILogger<ClientAppFilesMiddleware> _Logger;
    private readonly IFileProvider _FileProvider;

    public ClientAppFilesMiddleware(
        RequestDelegate next,
        IWebHostEnvironment hostingEnv,
        ILogger<ClientAppFilesMiddleware> logger) {
        this._Next = next;
        this._HostingEnv = hostingEnv;
        this._Logger = logger;
        this._FileProvider = /*_options.FileProvider ??*/ Helpers.ResolveFileProvider(hostingEnv);
    }

    public Task Invoke(HttpContext context) {
        var requestMethod = context.Request.Method;
        if (!Helpers.IsGetOrHeadMethod(requestMethod)) { 
            return this._Next(context);
        }
        
        var requestPath = context.Request.Path;
        _Logger.LogInformation($"Received request: {context.Request.Path}");
        var isProcess = (!requestPath.HasValue || requestPath == "/");
        if (isProcess) {
            //context.Request.Path = new PathString(Helpers.GetPathValueWithSlash(context.Request.Path) + defaultFile);
            context.Request.Path = new PathString("/static/index.html");
        } else {
            context.Request.Path = new PathString("/static/${requestPaths}");
        }
        return _Next(context);
    }
}
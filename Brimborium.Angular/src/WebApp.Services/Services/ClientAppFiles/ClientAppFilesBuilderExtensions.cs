using Brimborium.Angular.WebApp.Services.ClientAppFiles;

using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.StaticAssets;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;

namespace Microsoft.AspNetCore.Builder;

public static class ClientAppFilesBuilderExtensions {
    public static IEndpointRouteBuilder MapClientAppFiles(this IEndpointRouteBuilder endpoints) {
        //var options = new StaticFileOptions();
        //options.FileProvider = new PhysicalFileProvider(
        //    Path.Combine(Directory.GetCurrentDirectory(), "ClientAppFiles"));
        //options.RequestPath = "/ClientAppFiles";
        //endpoints.MapFallbackToFile("/ClientAppFiles/{*path:nonfile}", options);

        return endpoints;
    }

    public static IApplicationBuilder UseClientAppFiles(this IApplicationBuilder app) {
        //var options = new StaticFileOptions();
        //options.FileProvider = new PhysicalFileProvider(
        //    Path.Combine(Directory.GetCurrentDirectory(), "ClientAppFiles"));
        //options.RequestPath = "/ClientAppFiles";
        //endpoints.MapFallbackToFile("/ClientAppFiles/{*path:nonfile}", options);
        // var options = app.ApplicationServices.GetRequiredService<IOptions<ClientAppFilesOptions>>().Value;
        app.UseMiddleware<ClientAppFilesMiddleware>();
        return app;
    }
    

}

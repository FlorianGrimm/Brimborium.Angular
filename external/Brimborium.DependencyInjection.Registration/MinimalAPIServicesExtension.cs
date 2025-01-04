using Microsoft.OpenApi;
using Microsoft.AspNetCore.OpenApi;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Brimborium.DependencyInjection.Registration;

public static class MinimalAPIServicesExtension {
    /// <summary>
    /// Maps all minimal APIs registered in the dependency injection container to the specified <see cref="WebApplication"/>.
    /// </summary>
    /// <param name="app">The <see cref="WebApplication"/> to which the minimal APIs will be mapped.</param>
    public static void MapMinimalAPI(this WebApplication app) {
        var minimalAPIAppBuilder = new MinimalAPIAppBuilder(app);
        var groupAPI = minimalAPIAppBuilder.MapGroup("_api").WithOpenApi();
        var listMinimalAPI = app.Services.GetServices<IMinimalAPI>();
        foreach (var minimalAPI in listMinimalAPI) {
            minimalAPI.MapEndpoints(app, groupAPI);
        }
    }
}

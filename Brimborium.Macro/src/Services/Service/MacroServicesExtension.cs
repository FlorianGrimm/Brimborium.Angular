using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Brimborium.Macro.Service;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection;

public static class MacroServicesExtension {
    public static IServiceCollection AddMacroServices(
        this IServiceCollection services
        //IConfiguration configuration
        ) {
        services.TryAddSingleton<WorkspaceService>();
        services.TryAddSingleton<SolutionService>();
        services.TryAddSingleton<CommandService>();
        return services;
    }
}

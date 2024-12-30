using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Brimborium.Angular.WebApp.Services.ClientAppFiles;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection;

public static class ClientAppFilesServicesExtension {
    public static IServiceCollection AddClientAppFiles(
        this IServiceCollection services,
        IConfiguration? configuration=default,
        Action<ClientAppFilesOptions>? configureOptions=default
    ) {
        if (configuration is { } || configureOptions is { }) {
            services.Configure<ClientAppFilesOptions>((options) => {
                configuration?.GetSection("ClientAppFiles").Bind(options);
                configureOptions?.Invoke(options);
            });
        }
        //services.AddSingleton<ClientAppFilesService>();
        return services;
    }
}
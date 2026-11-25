namespace Brimborium.Angular;

public sealed partial class Program {
    public static async Task<int> Main(string[] args) {
        try {
            Program program = new();
            var version = program.GetType().Assembly.GetName().Version?.ToString() ?? "?";
            System.Console.Out.WriteLine($"Version: {version}");

            var builder = WebApplication.CreateBuilder(args);
            // setup applicationFeatures
            ApplicationFeatures applicationFeatures;
            {
                ApplicationFeaturesOptions applicationFeaturesOptions = new();
                builder.Configuration.GetSectionApplication().Bind(applicationFeaturesOptions);
                applicationFeatures = new ApplicationFeatures(
                    AuthenticationNegotiate: applicationFeaturesOptions.AuthenticationNegotiate.GetValueOrDefault(true),
                    AuthenticationAPIKey: applicationFeaturesOptions.AuthenticationAPIKey,
                    EnableOpenApi: applicationFeaturesOptions.EnableOpenApi.GetValueOrDefault(true),
                    EnableOpenTelemetry: applicationFeaturesOptions.EnableOpenTelemetry.GetValueOrDefault(true),
                    EnableHttpLogging: applicationFeaturesOptions.EnableHttpLogging.GetValueOrDefault(false),
                    EnableHttpsRedirection: applicationFeaturesOptions.EnableHttpsRedirection.GetValueOrDefault(true),
                    EnableCors: applicationFeaturesOptions.EnableCors.GetValueOrDefault(true),
                    EnableRateLimits: applicationFeaturesOptions.EnableRateLimits.GetValueOrDefault(false)
                    );
            }
            await program.ExecuteAsync(builder, applicationFeatures);
            return 0;
        } catch (AggregateException ex) {
            ex.Handle((error) => {
                WriteException(error);
                return true;
            });
            return 1;
        } catch (Exception ex) {
            WriteException(ex);
            return 1;
        }

        static void WriteException(Exception ex) {
            System.Console.Error.WriteLine(ex.ToString());
        }
    }

    public async Task ExecuteAsync(
        WebApplicationBuilder builder,
        ApplicationFeatures applicationFeatures) {
        this.ConfigureWebApplicationBuilder(builder, applicationFeatures);
        WebApplication app = builder.Build();
        await this.RunWebApplicationAsync(app);
    }

    public void ConfigureWebApplicationBuilder(
        WebApplicationBuilder builder,
        ApplicationFeatures applicationFeatures) {
        // Add services to the container.
        _ = builder.Services.AddSingleton<ApplicationFeatures>(applicationFeatures);

        // services with Injectio Attribute
        _ = builder.Services.AddBrimboriumAngular();

        builder.ConfigureServices(applicationFeatures);
    }

    public async Task RunWebApplicationAsync(
        WebApplication app
        ) {
        app.ConfigureWebApplication();
        await app.RunAsync().ConfigureAwait(false);
    }
}

public sealed partial class ApplicationFeaturesOptions {
    public bool? AuthenticationNegotiate { get; set; }
    public string? AuthenticationAPIKey { get; set; }
    public bool? EnableOpenApi { get; set; }
    public bool? EnableOpenTelemetry { get; set; }
    public bool? EnableHttpLogging { get; set; }
    public bool? EnableHttpsRedirection { get; set; }
    public bool? EnableCors { get; set; }
    public bool? EnableRateLimits { get; set; }
}

/// <summary>
/// Enable Features
/// </summary>
public sealed partial record class ApplicationFeatures(
    bool AuthenticationNegotiate,
    string? AuthenticationAPIKey,
    bool EnableOpenApi,
    bool EnableOpenTelemetry,
    bool EnableHttpLogging,
    bool EnableHttpsRedirection,
    bool EnableCors,
    bool EnableRateLimits
    );

public static class ApplicationFeaturesExtension {
    extension(WebApplicationBuilder builder) {
        public void ConfigureServices(ApplicationFeatures applicationFeatures, LogLevel logLevel = LogLevel.Trace) {
            // this creates a second ServiceCollection/ServiceProvider
            var serviceBuilder = new Microsoft.Extensions.DependencyInjection.ServiceCollection();
            _ = serviceBuilder.AddLogging((loggingBuilder) => {
                loggingBuilder.AddConsole((consoleLoggerOptions) => { });
                loggingBuilder.SetMinimumLevel(logLevel);
            });
            serviceBuilder.AddSingleton<ApplicationFeatures>(applicationFeatures);
            // using the Injectio attributed services.
            serviceBuilder.AddBrimboriumAngular();
            using (var configureServiceProvider = serviceBuilder.BuildServiceProvider()) {
                configureServiceProvider.ConfigureServices(builder);
            }
            // this seams to run long (100ms),
            // but this thing that eats the time with type loading - which will be loaded anyway
        }

    }
}

public sealed record class ConfigureServicesBuilder(List<IConfigureWebApplication> ListConfigureWebApplication);

public static partial class ConfigureServicesBuilderExtension {
    extension(IServiceProvider serviceProvider) {
        public void ConfigureServices(
            WebApplicationBuilder builder
        ) {
            if (serviceProvider.GetServices<IConfigureServices>() is { } listService) {
                List<IConfigureServices> listConfigureService = new(listService);
                {
                    listConfigureService.Sort((a, b) => {
                        var orderA = (a is IConfigureOrderedServices csoA) ? csoA.GetOrder() : ConfigureServicesOrder.Normal;
                        var orderB = (b is IConfigureOrderedServices csoB) ? csoB.GetOrder() : ConfigureServicesOrder.Normal;
                        return ((int)orderA).CompareTo((int)orderB);
                    });
                }

                List<IConfigureWebApplication> listConfigureWebApplication = new(listConfigureService.Count);
                {
                    foreach (var item in listConfigureService) {
                        item.ConfigureServices(builder);
                        if (item is IConfigureWebApplication configureWebApplication) {
                            listConfigureWebApplication.Add(configureWebApplication);
                        }
                    }
                }
                ConfigureServicesBuilder configureServicesBuilder = new(listConfigureWebApplication);
                _ = builder.Services.AddSingleton<ConfigureServicesBuilder>(configureServicesBuilder);
            } else {
                ConfigureServicesBuilder configureServicesBuilder = new([]);
                _ = builder.Services.AddSingleton<ConfigureServicesBuilder>(configureServicesBuilder);
            }
        }
    }

    extension(WebApplication app) {
        /// <summary>
        /// iterate over the <see cref="IConfigureWebApplication"/>, in order, and call <see cref="IConfigureWebApplication.ConfigureWebApplication(WebApplication)"/>.
        /// </summary>
        /// <param name="app">The app that is about to run.</param>
        public void ConfigureWebApplication() {
            ConfigureServicesBuilder configureServicesBuilder = app.Services.GetRequiredService<ConfigureServicesBuilder>();
            var listNewConfigureWebApplication = app.Services.GetServices<IConfigureWebApplication>();
            var listOldConfigureWebApplication = configureServicesBuilder.ListConfigureWebApplication;
            IEnumerable<IConfigureWebApplication> listUnordered = (n: listNewConfigureWebApplication.Any(), o: listOldConfigureWebApplication.Any()) switch {
                (true, true) => mergeByType(listNewConfigureWebApplication, listOldConfigureWebApplication),
                (true, false) => listNewConfigureWebApplication,
                (false, true) => listOldConfigureWebApplication,
                _ => []
            };

            var listOrdered = listUnordered.OrderBy(item => item.GetOrder());
            foreach (var item in listOrdered) {
                item.ConfigureWebApplication(app);
            }

            List<IConfigureWebApplication> mergeByType(
                IEnumerable<IConfigureWebApplication> listNew,
                IEnumerable<IConfigureWebApplication> listOld
                ) {
                List<IConfigureWebApplication> result = new();
                var dictOld = listOld.ToDictionary(keySelector: (item) => item.GetType());
                foreach (var itemNew in listNew) {
                    var itemNewType = itemNew.GetType();
                    if (dictOld.TryGetValue(itemNewType, out var itemOld)) {
                        dictOld.Remove(itemNew.GetType());
                        result.Add(itemNew);
                        if (itemOld is IConfigureSharedState itemOldConfigureSharedState
                            && itemNew is IConfigureSharedState itemNewConfigureSharedState) {
                            var sharedState = itemOldConfigureSharedState.GetSharedState();
                            itemNewConfigureSharedState.SetSharedState(sharedState);
                        }
                    } else { 
                        result.Add(itemNew);
                    }
                }
                foreach (var itemOld in dictOld.Values) {
                    result.Add(itemOld);
                }
                return result;
            }
        }
    }
}

/// <summary>
/// Configure Services for the WebApplicationBuilder
/// </summary>
public interface IConfigureServices {
    /// <summary>
    /// Configure the services for <paramref name="builder"/>.
    /// </summary>
    /// <param name="builder">The builder to polish.</param>
    /// <example>
    /// [Injectio.Attributes.RegisterSingleton<IConfigureServices>(Duplicate = DuplicateStrategy.Append)]
    /// public class FeatureAbc(ApplicationFeatures applicationFeatures) : IConfigureServices {
    ///     ...
    /// }
    /// </example>
    void ConfigureServices(WebApplicationBuilder builder);
}

public enum ConfigureServicesOrder {
    First,
    Normal,
    Last
}
public interface IConfigureOrderedServices {
    /// <summary>
    /// The Order to call
    /// </summary>
    /// <returns>The Order to call</returns>
    ConfigureServicesOrder GetOrder();
}

public static partial class IConfigureServicesLogExtension {
    // ConfigureServices
    [LoggerMessage(
        Level = LogLevel.Information,
        Message = "ConfigureServices-Feature({featureName}) {additional}"
        )]
    public static partial void LogConfigureServicesFeature(this ILogger logger, string featureName, string additional);

    [LoggerMessage(
        Level = LogLevel.Information,
        Message = "ConfigureServices-Feature-disabled({featureName})"
        )]
    public static partial void LogConfigureServicesFeatureDisabled(this ILogger logger, string featureName);

    // ConfigureWebApplication
    [LoggerMessage(
        Level = LogLevel.Information,
        Message = "ConfigureWebApplication-Feature({featureName})"
        )]
    public static partial void LogConfigureWebApplicationFeature(this ILogger logger, string featureName);

    [LoggerMessage(
        Level = LogLevel.Information,
        Message = "ConfigureWebApplication-Feature-disabled({featureName})"
        )]
    public static partial void LogConfigureWebApplicationFeatureDisabled(this ILogger logger, string featureName);

}

/// <summary>
/// Order for the Middleware to call.
/// The order matters.
/// </summary>
public enum ConfigureWebApplicationOrder {
    First,
    Cors,
    HttpLogging,
    OpenApi,
    HtmlUseStaticFiles,
    Authentication,
    RateLimits,
    MinimalApiEndpoint,
    OpenTelemetry,
    HtmlStaticAssets,
    Last
}

/// <summary>
/// Configure the WebApplication middleware
/// </summary>
/// <example>
/// <para>create one instance after <see cref="WebApplicationBuilder.Build"/></para>
/// <code>
/// [Injectio.Attributes.RegisterSingleton<IConfigureWebApplication>(Duplicate = DuplicateStrategy.Append)]
/// public class FeatureAbc : IConfigureWebApplication {
///     ...
/// }
/// </code>
/// <para>create one instance in the <see cref="ApplicationFeaturesExtension.ConfigureServices(WebApplicationBuilder, ApplicationFeatures, LogLevel)"/>.</para>
/// <code>
/// [Injectio.Attributes.RegisterSingleton<IConfigureServices>(Duplicate = DuplicateStrategy.Append)]
/// public class FeatureAbc : IConfigureServices, IConfigureWebApplication {
///     ...
/// }
/// </code>
/// <para>create one instance in the <see cref="ApplicationFeaturesExtension.ConfigureServices(WebApplicationBuilder, ApplicationFeatures, LogLevel)"/>.</para>
/// <para>create one instance after <see cref="WebApplicationBuilder.Build"/></para>
/// <code>
/// [Injectio.Attributes.RegisterSingleton<IConfigureServices>(Duplicate = DuplicateStrategy.Append)]
/// [Injectio.Attributes.RegisterSingleton<IConfigureWebApplication>(Duplicate = DuplicateStrategy.Append)]
/// public class FeatureAbc : IConfigureServices,IConfigureWebApplication {
///     ...
/// }
/// </code>
/// </example>
public interface IConfigureWebApplication {
    /// <summary>
    /// The Order to call
    /// </summary>
    /// <returns>The Order to call</returns>
    ConfigureWebApplicationOrder GetOrder();

    /// <summary>
    /// Configure the WebApplication middleware
    /// </summary>
    /// <param name="app">The app that is about to run.</param>
    void ConfigureWebApplication(WebApplication app);
    
}

public interface IConfigureSharedState {
    object GetSharedState();
    void SetSharedState(object value);
}

    /// <summary>
    /// Allows to register the minimalapi-endpoint(s).
    /// </summary>
    /// <example>
    /// [Injectio.Attributes.RegisterSingleton<IMinimalApiEndpoint>(Duplicate = DuplicateStrategy.Append)]
    /// </example>
    public interface IMinimalApiEndpoint {
    /// <summary>
    /// Map the minimalapi
    /// </summary>
    /// <param name="builder">The app that is abount to run.</param>
    void MapMinimalApi(WebApplication builder);
}


[Injectio.Attributes.RegisterSingleton<IConfigureWebApplication>(Duplicate = DuplicateStrategy.Append)]
public sealed class MinimalApiEndpointConfigureWebApplication(
        ILogger<MinimalApiEndpointConfigureWebApplication> logger
    ) : IConfigureWebApplication {
    private const string _FeatureName = "MinimalAPIEndpoints";
    private readonly ILogger<MinimalApiEndpointConfigureWebApplication> _Logger = logger;

    public ConfigureWebApplicationOrder GetOrder() => ConfigureWebApplicationOrder.MinimalApiEndpoint;

    public void ConfigureWebApplication(WebApplication app) {
        // register all MinimalAPI endpoints
        this._Logger.LogConfigureWebApplicationFeature(_FeatureName);
        if (app.Services.GetServices<IMinimalApiEndpoint>() is { } list) {
            foreach (var minimalApiEndpoint in list) {
                minimalApiEndpoint.MapMinimalApi(app);
            }
        }
    }
}

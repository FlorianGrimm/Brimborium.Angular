namespace Brimborium.Angular.Feature;

[Injectio.Attributes.RegisterSingleton<IConfigureServices>(Duplicate = DuplicateStrategy.Append)]
public sealed partial class CorsFeature(
    ApplicationFeatures applicationFeatures,
    ILogger<CorsFeature> logger
    ) : IConfigureServices, IConfigureWebApplication {
    private const string _FeatureName = "Cors";
    private readonly ApplicationFeatures _ApplicationFeatures = applicationFeatures;
    private readonly ILogger<CorsFeature> _Logger = logger;

    public void ConfigureServices(WebApplicationBuilder builder) {
        if (!this._ApplicationFeatures.EnableCors) {
            this._Logger.LogConfigureServicesFeatureDisabled(_FeatureName);
            return;
        }

        this._Logger.LogConfigureServicesFeature(_FeatureName, string.Empty);
        /* https://github.com/Azure-Samples/containerapps-albumapi-csharp/blob/main/src/Program.cs */
        builder.Services.AddCors(options => {
            options.AddDefaultPolicy(builder => {
                builder.AllowAnyOrigin();
                builder.AllowAnyHeader();
                builder.AllowAnyMethod();
            });
        });

    }

    public ConfigureWebApplicationOrder GetOrder() => ConfigureWebApplicationOrder.Cors;

    public void ConfigureWebApplication(WebApplication app) {
        if (!this._ApplicationFeatures.EnableCors) {
            this._Logger.LogConfigureWebApplicationFeatureDisabled(_FeatureName);
            return;
        }

        {
            this._Logger.LogConfigureWebApplicationFeature(_FeatureName);
            app.UseCors();
        }
    }
}
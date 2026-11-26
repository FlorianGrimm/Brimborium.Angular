namespace Brimborium.Angular.Feature;

[Injectio.Attributes.RegisterSingleton<IConfigureServices>(Duplicate = DuplicateStrategy.Append)]
public sealed partial class OpenApiFeature(
    ApplicationFeatures applicationFeatures,
    ILogger<OpenApiFeature> logger
    ) : IConfigureServices, IConfigureWebApplication, IConfigureSharedState {
    internal sealed record SharedState(
        bool EnableOpenApiStage
        );

    private const string _FeatureName = "OpenApi";
    private readonly ApplicationFeatures _ApplicationFeatures = applicationFeatures;
    private readonly ILogger<OpenApiFeature> _Logger = logger;
    private SharedState _SharedState = new(false);

    public void ConfigureServices(WebApplicationBuilder builder) {

        // for OpenAPI and avoiding dynanic assembly/code
        _ = builder.Services.ConfigureHttpJsonOptions(options => {
            options.SerializerOptions.TypeInfoResolverChain.Add(BrimboriumAngularContractJsonSerializerContext.Default);
            options.SerializerOptions.PropertyNamingPolicy = default;
        });

        if (!this._ApplicationFeatures.EnableOpenApi) {
            this._Logger.LogConfigureServicesFeatureDisabled(_FeatureName);
            return;
        }

        this._Logger.LogConfigureServicesFeature(_FeatureName, string.Empty);

        var enableOpenApiStage
            = builder.Environment.IsDevelopment();
        this._SharedState = new SharedState(
            EnableOpenApiStage: enableOpenApiStage
            );
        if (enableOpenApiStage) {
            _ = builder.Services.AddOpenApi("v2", (options) => {
                options.OpenApiVersion = Microsoft.OpenApi.OpenApiSpecVersion.OpenApi2_0;
            });
            _ = builder.Services.AddOpenApi("v3", (options) => {
                options.OpenApiVersion = Microsoft.OpenApi.OpenApiSpecVersion.OpenApi3_2;
            });
        }
    }

    public ConfigureWebApplicationOrder GetOrder() => ConfigureWebApplicationOrder.OpenApi;

    public void ConfigureWebApplication(WebApplication app) {
        if (!this._SharedState.EnableOpenApiStage) {
            this._Logger.LogConfigureWebApplicationFeatureDisabled(_FeatureName);
            return;
        }

        this._Logger.LogConfigureWebApplicationFeature(_FeatureName, string.Empty);
        _ = app.MapOpenApi().AllowAnonymous();
    }

    object IConfigureSharedState.GetSharedState() => this._SharedState;

    void IConfigureSharedState.SetSharedState(object value) {
        this._SharedState = (SharedState)value;
    }
}

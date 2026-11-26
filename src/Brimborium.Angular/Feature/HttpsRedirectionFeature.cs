using System.Runtime.CompilerServices;

namespace Brimborium.Angular.Feature;

[Injectio.Attributes.RegisterSingleton<IConfigureServices>(Duplicate = DuplicateStrategy.Append)]
public sealed partial class HttpsRedirectionFeature(
    ApplicationFeatures applicationFeatures,
    ILogger<HttpsRedirectionFeature> logger
    ) : IConfigureServices, IConfigureWebApplication {
    private const string _FeatureName = "HttpsRedirection";
    private readonly ApplicationFeatures _ApplicationFeatures = applicationFeatures;
    private readonly ILogger<HttpsRedirectionFeature> _Logger = logger;

    public void ConfigureServices(WebApplicationBuilder builder) {
    }

    public ConfigureWebApplicationOrder GetOrder() => ConfigureWebApplicationOrder.Cors;

    public void ConfigureWebApplication(WebApplication app) {
        if (!this._ApplicationFeatures.EnableCors) { 
            this._Logger.LogConfigureWebApplicationFeatureDisabled(_FeatureName);
            return;
        }

        this._Logger.LogConfigureWebApplicationFeature(_FeatureName, string.Empty);
        app.UseHttpsRedirection();
    }
}

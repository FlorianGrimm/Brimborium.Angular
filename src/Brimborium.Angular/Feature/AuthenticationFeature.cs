using Microsoft.AspNetCore.Authentication.Negotiate;

namespace Brimborium.Angular.Feature;

[Injectio.Attributes.RegisterSingleton<IConfigureServices>(Duplicate = DuplicateStrategy.Append)]
public sealed partial class AuthenticationFeature(
    ApplicationFeatures applicationFeatures,
    ILogger<AuthenticationFeature> logger
    ) : IConfigureServices, IConfigureWebApplication {
    private const string _FeatureName = "Authentication";
    private readonly ApplicationFeatures _ApplicationFeatures = applicationFeatures;
    private readonly ILogger<AuthenticationFeature> _Logger = logger;
    public void ConfigureServices(WebApplicationBuilder builder) {
        // Configure authentication services here
        this._Logger.LogConfigureServicesFeature($"{_FeatureName}.Negotiate");

        var authenticationBuilder = builder.Services
            .AddAuthentication(NegotiateDefaults.AuthenticationScheme);

        {
            _ = authenticationBuilder
                .AddNegotiate();
        }

        if (this._ApplicationFeatures.AuthenticationAPIKey is { Length: > 0 } authenticationAPIKey) {
            _ = authenticationBuilder
                .AddAPIKey(
                    authenticationScheme: global::Microsoft.AspNetCore.Authentication.ApiKey.APIKeyDefaults.AuthenticationScheme,
                    configure: options => {
                        options.APIKey = authenticationAPIKey;
                    });
        }

        _ = builder.Services.AddAuthorization(options => {
            // By default, all incoming requests will be authorized according to the default policy.
            options.FallbackPolicy = options.DefaultPolicy;
        });
    }

    public ConfigureWebApplicationOrder GetOrder() => ConfigureWebApplicationOrder.Authentication;

    public void ConfigureWebApplication(WebApplication app) {
        this._Logger.LogConfigureWebApplicationFeature(_FeatureName);

        _ = app.UseAuthentication();
        _ = app.UseAuthorization();
    }
}

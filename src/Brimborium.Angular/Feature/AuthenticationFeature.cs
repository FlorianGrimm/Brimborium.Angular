using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.ApiKey;
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
        AuthenticationBuilder authenticationBuilder;
        var authenticationAPIKey = this._ApplicationFeatures.AuthenticationAPIKey ?? string.Empty;
        var useAuthenticationNegotiate = this._ApplicationFeatures.AuthenticationNegotiate;
        var useAuthenticationAPIKey = (authenticationAPIKey is { Length: > 0 });

        if (useAuthenticationNegotiate && useAuthenticationAPIKey) {
            this._Logger.LogConfigureServicesFeature(_FeatureName, "Negotiate,APIKey");
            authenticationBuilder = builder.Services
                .AddAuthentication("Default")
                .AddPolicyScheme("Default", "Default", options => {
                    options.ForwardDefaultSelector = forwardDefaultSelector;

                    static string forwardDefaultSelector(HttpContext httpContext) {
                        var hasAPIKey = APIKeyAuthenticationHandler.HasAPIKey(httpContext);
                        if (hasAPIKey) {
                            return global::Microsoft.AspNetCore.Authentication.ApiKey.APIKeyDefaults.AuthenticationScheme;
                        } else {
                            return global::Microsoft.AspNetCore.Authentication.Negotiate.NegotiateDefaults.AuthenticationScheme;
                        }
                    };
                });
        } else if (useAuthenticationNegotiate && !useAuthenticationAPIKey) {
            this._Logger.LogConfigureServicesFeature(_FeatureName, "Negotiate");
            authenticationBuilder = builder.Services
                .AddAuthentication(NegotiateDefaults.AuthenticationScheme);
        } else if (!useAuthenticationNegotiate && useAuthenticationAPIKey) {
            this._Logger.LogConfigureServicesFeature(_FeatureName, "APIKey");
            authenticationBuilder = builder.Services
                .AddAuthentication(APIKeyDefaults.AuthenticationScheme);
        } else if (!useAuthenticationNegotiate && !useAuthenticationAPIKey) {
            this._Logger.LogConfigureServicesFeature(_FeatureName, "No Authentication");
            return;
        } else {
            throw new NotSupportedException();
        }

        if (useAuthenticationNegotiate) {
            _ = authenticationBuilder
                .AddNegotiate();
        }

        if (useAuthenticationAPIKey) {
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
        this._Logger.LogConfigureWebApplicationFeature(_FeatureName, string.Empty);

        _ = app.UseAuthentication();
        _ = app.UseAuthorization();
    }
}

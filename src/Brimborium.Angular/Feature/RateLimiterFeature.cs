using System.Threading.RateLimiting;

namespace Brimborium.Angular.Feature;


[Injectio.Attributes.RegisterSingleton<IConfigureServices>(Duplicate = DuplicateStrategy.Append)]
public sealed partial class RateLimiterFeature(
    ApplicationFeatures applicationFeatures,
    ILogger<RateLimiterFeature> logger
    ) : IConfigureServices, IConfigureWebApplication {
    private const string _FeatureName = "RateLimits";
    private readonly ApplicationFeatures _ApplicationFeatures = applicationFeatures;
    private readonly ILogger<RateLimiterFeature> _Logger = logger;

    public void ConfigureServices(WebApplicationBuilder builder) {
        AppRateLimiterOptions appRateLimiterOptions = new();
        builder.Services.AddSingleton<AppRateLimiterOptions>(appRateLimiterOptions);

        if (!this._ApplicationFeatures.EnableRateLimits) {
            this._Logger.LogConfigureServicesFeatureDisabled(_FeatureName);
            return;
        }

        this._Logger.LogConfigureServicesFeature(_FeatureName, string.Empty);

        builder.Services.AddRateLimiter(
            (Microsoft.AspNetCore.RateLimiting.RateLimiterOptions rateLimiterOptions) => {
                builder.GetSectionRateLimit().Bind(appRateLimiterOptions);
                rateLimiterOptions.GlobalLimiter = appRateLimiterOptions.GetRateLimitPartition();
            });
    }

    public ConfigureWebApplicationOrder GetOrder() => ConfigureWebApplicationOrder.RateLimits;

    public void ConfigureWebApplication(WebApplication app) {
        if (!this._ApplicationFeatures.EnableRateLimits) {
            this._Logger.LogConfigureWebApplicationFeatureDisabled("RateLimits");
            return;
        }

        this._Logger.LogConfigureWebApplicationFeature(_FeatureName);

        app.UseRateLimiter();
    }
}

public sealed partial class AppRateLimiterOptions {
    public FixedWindowRateLimiterOptions RateLimiterOptionsUser { get; set; } = new();
    public string ServiceAccount { get; set; } = "";
    public FixedWindowRateLimiterOptions RateLimiterOptionsServiceAccount { get; set; } = new();

    public AppRateLimiterOptions() {
        this.RateLimiterOptionsUser = new FixedWindowRateLimiterOptions() {
            // avoid that RateLimitPartition.GetFixedWindowLimiter will make copy with AutoReplenishment = false
            AutoReplenishment = false,
            QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
            PermitLimit = 60,
            QueueLimit = 0,
            Window = TimeSpan.FromMinutes(1),
        };
        this.RateLimiterOptionsServiceAccount = new FixedWindowRateLimiterOptions() {
            AutoReplenishment = false,
            PermitLimit = 4 * 60,
            QueueLimit = 0,
            Window = TimeSpan.FromMinutes(1),
        };
    }

    public PartitionedRateLimiter<HttpContext> GetRateLimitPartition() {
        AppRateLimiterConfiguration? rateLimiterConfigurationInstance = default;
        return PartitionedRateLimiter.Create<HttpContext, string>((httpContext) => {
            if (rateLimiterConfigurationInstance is not { } rateLimiterConfiguration) {
                rateLimiterConfiguration
                    = rateLimiterConfigurationInstance
                    = httpContext.RequestServices.GetRequiredService<AppRateLimiterConfiguration>();
            }
            return rateLimiterConfiguration.GetRateLimitPartitioner(httpContext);
        });
    }
}

[Injectio.Attributes.RegisterSingleton<AppRateLimiterConfiguration>()]
public sealed partial class AppRateLimiterConfiguration {
    private readonly ILogger<AppRateLimiterConfiguration> _Logger;

    private readonly FixedWindowRateLimiterOptions _RateLimiterOptionsUser;
    private readonly string _ServiceAccount;
    private readonly FixedWindowRateLimiterOptions _RateLimiterOptionsServiceAccount;

    public AppRateLimiterConfiguration(
        AppRateLimiterOptions rateLimiterOptions,
        ILogger<AppRateLimiterConfiguration> logger
        ) {
        this._Logger = logger;

        if (rateLimiterOptions.RateLimiterOptionsUser is { } rateLimiterOptionsUser) {
            this._RateLimiterOptionsUser = new FixedWindowRateLimiterOptions() {
                // avoid that RateLimitPartition.GetFixedWindowLimiter will make copy with AutoReplenishment = false
                AutoReplenishment = false,
                QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                PermitLimit = rateLimiterOptionsUser.PermitLimit,
                QueueLimit = rateLimiterOptionsUser.QueueLimit,
                Window = rateLimiterOptionsUser.Window,
            };
        } else {
            this._RateLimiterOptionsUser = new FixedWindowRateLimiterOptions() {
                AutoReplenishment = false,
                QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                PermitLimit = 4 * 60,
                QueueLimit = 0,
                Window = TimeSpan.FromMinutes(1),
            };
        }
        this._ServiceAccount = rateLimiterOptions.ServiceAccount ?? string.Empty;
        if (rateLimiterOptions.RateLimiterOptionsServiceAccount is { } rateLimiterOptionsServiceAccount) {
            this._RateLimiterOptionsServiceAccount = new FixedWindowRateLimiterOptions() {
                AutoReplenishment = false,
                QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                PermitLimit = rateLimiterOptionsServiceAccount.PermitLimit,
                QueueLimit = rateLimiterOptionsServiceAccount.QueueLimit,
                Window = rateLimiterOptionsServiceAccount.Window,
            };
        } else {
            this._RateLimiterOptionsServiceAccount = new FixedWindowRateLimiterOptions() {
                AutoReplenishment = false
            };
        }
    }

    public PartitionedRateLimiter<HttpContext> GetRateLimitPartition() {
        return PartitionedRateLimiter.Create<HttpContext, string>(httpContext => {
            return this.GetRateLimitPartitioner(httpContext);
        });
    }

    public RateLimitPartition<string> GetRateLimitPartitioner(HttpContext httpContext) {
        string partitionKey;
        if (httpContext.User.Identity is { } identity
            && identity.Name is { Length: > 0 } identityName) {
            partitionKey = identityName;
            // if the ServiceAccount is configured, check and if match use it
            if (this._ServiceAccount is { Length: > 0 } serviceAccount) {
                if (string.Equals(serviceAccount, identityName, StringComparison.Ordinal)) {
                    this._Logger.LogRateLimitPartitionerServiceAccount(identityName);
                    return RateLimitPartition.GetFixedWindowLimiter(
                        partitionKey: partitionKey,
                        factory: partition => this._RateLimiterOptionsServiceAccount);
                }
            }
            this._Logger.LogRateLimitPartitionerUser(identityName);
            return RateLimitPartition.GetFixedWindowLimiter(
                partitionKey: partitionKey,
                factory: partition => this._RateLimiterOptionsUser);
        } else {
            partitionKey = httpContext.Request.Headers.Host.FirstOrDefault()?.ToString() ?? "-";
            return RateLimitPartition.GetFixedWindowLimiter(
                partitionKey: partitionKey,
                factory: partition => this._RateLimiterOptionsUser);
        }
    }
}
public static partial class AppRateLimiterConfigurationLogExtension {
    [LoggerMessage(
        Level = LogLevel.Debug,
        Message = "RateLimit-Partitioner-User({identityName})"
        )]
    public static partial void LogRateLimitPartitionerUser(this ILogger<AppRateLimiterConfiguration> logger, string identityName);

    [LoggerMessage(
        Level = LogLevel.Debug,
        Message = "RateLimit-Partitioner-ServiceAccount({identityName})"
        )]
    public static partial void LogRateLimitPartitionerServiceAccount(this ILogger<AppRateLimiterConfiguration> logger, string identityName);
}
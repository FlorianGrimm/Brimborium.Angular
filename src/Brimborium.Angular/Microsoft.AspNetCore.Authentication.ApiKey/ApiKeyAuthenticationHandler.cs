using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.ApiKey;

using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;

using System.Security.Claims;
using System.Text.Encodings.Web;

namespace Microsoft.AspNetCore.Authentication.ApiKey {
    public static class APIKeyDefaults {
        public const string AuthenticationScheme = "APIKey";
    }

    public sealed class APIKeyOptions : AuthenticationSchemeOptions {
        public APIKeyOptions() { }

        public string APIKey { get; set; } = "";
        public string Scope { get; set; } = "user_impersonation";
    }

    public sealed class APIKeyAuthenticationHandler : AuthenticationHandler<APIKeyOptions> {
        public static bool HasAPIKey(HttpContext httpContext) {
            StringValues apiKey;
            if (httpContext.Request.Headers.TryGetValue("APIKey", out apiKey)) {
                // OK 
            } else if (httpContext.Request.Headers.TryGetValue("X-API-Key", out apiKey)) {
                // OK 
            } else {
                return false;
            }
            if (apiKey.FirstOrDefault() is not { Length: > 0 } apiKeyValue) {
                return true;
            } else { 
                return false;
            }
        }
        public APIKeyAuthenticationHandler(
            IOptionsMonitor<APIKeyOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder) : base(options, logger, encoder) { }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync() {
            var expectedAPIKey = this.Options.APIKey;
            if (string.IsNullOrEmpty(expectedAPIKey)) {
                this.Logger.LogWarning("Options.APIKey is empty.");
                return AuthenticateResult.NoResult();
            }
            StringValues apiKey;
            if (this.Request.Headers.TryGetValue("APIKey", out apiKey)) {
                // OK 
            } else if (this.Request.Headers.TryGetValue("X-API-Key", out apiKey)) {
                // OK 
            } else {
                this.Logger.LogDebug("no APIKey-header.");
                return AuthenticateResult.NoResult();
            }
            if (apiKey.FirstOrDefault() is not { Length: > 0 } apiKeyValue) {
                this.Logger.LogDebug("empty APIKey-header.");
                return AuthenticateResult.NoResult();
            }
            if (string.Equals(apiKeyValue, expectedAPIKey, StringComparison.Ordinal)) {
                // OK
            } else {
                this.Logger.LogWarning($"missmatched APIKey-header. '{apiKeyValue}'!='{expectedAPIKey}'");
                return AuthenticateResult.Fail(new Exception("APIKey does not matched."));
            }

            var claims = new[] {
                new Claim(ClaimTypes.Name, "ApiKeyUser"),
                //new Claim(ClaimConstants.Scp, "user_impersonation")
                new Claim("scp", "user_impersonation")
            };
            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            return AuthenticateResult.Success(ticket);
        }
    }
}
namespace Microsoft.Extensions.DependencyInjection {
    public static class ApiKeyAuthenticationExtensions {
        public static AuthenticationBuilder AddAPIKey(
            this AuthenticationBuilder builder,
            string? authenticationScheme,
            Action<APIKeyOptions>? configure) {
            if (string.IsNullOrEmpty(authenticationScheme)) { authenticationScheme = APIKeyDefaults.AuthenticationScheme; }
            if (configure is null) { configure = ConfigureDefault; }
            return builder.AddScheme<APIKeyOptions, APIKeyAuthenticationHandler>(authenticationScheme, configure);
        }

        private static void ConfigureDefault(APIKeyOptions options) { }
    }
}
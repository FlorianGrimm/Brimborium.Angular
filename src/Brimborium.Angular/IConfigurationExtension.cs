namespace Brimborium.Angular;

public static class IConfigurationExtension {
    /// <summary>
    /// BrimboriumAngular
    /// </summary>
    public const string SectionNameApplication = "BrimboriumAngular";

    /// <summary>
    /// RateLimit
    /// </summary>
    public const string SectionNameRateLimit = "RateLimit";

    extension(WebApplicationBuilder builder) {
        /// <summary>
        /// Get configuration Section for the application
        /// </summary>
        /// <returns>Section</returns>
        public IConfigurationSection GetSectionApplication()
            => builder.Configuration.GetSectionApplication();

        /// <summary>
        /// Get configuration Section for the RateLimit
        /// </summary>
        /// <returns>Section "RateLimit"</returns>
        public IConfigurationSection GetSectionRateLimit()
            => builder.GetSectionApplication().GetSectionRateLimit();
    }

    extension(IConfiguration configuration) {
        /// <summary>
        /// Get configuration Root-Section "BrimboriumAngular"
        /// </summary>
        /// <returns>Section "BrimboriumAngular"</returns>
        public IConfigurationSection GetSectionApplication()
            => configuration.GetSection(SectionNameApplication);

        /// <summary>
        /// Get configuration Section "MaPPSAPGateway:RateLimit"
        /// </summary>
        /// <returns>Section "MaPPSAPGateway:RateLimit"</returns>
        public IConfigurationSection GetSectionRateLimit()
            => ((configuration is IConfigurationRoot configurationRoot)
                ? configurationRoot.GetSectionApplication()
                : configuration)
                .GetSection(SectionNameRateLimit);

        /// <summary>
        /// Get configuration Section "ApiClient" as child of "BrimboriumAngular".
        /// </summary>
        /// <returns>Section "ApiClient"</returns>
        public IConfigurationSection GetSectionApiClient()
            => configuration.GetSection("ApiClient");

        /// <summary>
        /// Get configuration Root-Section "AzureAd"
        /// </summary>
        /// <returns>Section "AzureAd"</returns>
        public IConfigurationSection GetSectionAzureAd()
            => configuration.GetSection("AzureAd");

    }
}

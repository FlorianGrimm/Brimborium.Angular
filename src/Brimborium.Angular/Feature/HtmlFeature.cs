using Microsoft.Extensions.FileProviders;

namespace Brimborium.Angular.Feature;

[Injectio.Attributes.RegisterSingleton<IConfigureServices>(Duplicate = DuplicateStrategy.Append)]
//[Injectio.Attributes.RegisterSingleton<IConfigureWebApplication>(Duplicate = DuplicateStrategy.Append)]
public sealed partial class HtmlFeature(
    ApplicationFeatures applicationFeatures,
    ILogger<HtmlFeature> logger
    ) : IConfigureServices, IConfigureWebApplication
    //, IConfigureSharedState
    {
    private const string _FeatureName = "HtmlFeature";
    private readonly ApplicationFeatures _ApplicationFeatures = applicationFeatures;
    private readonly ILogger<HtmlFeature> _Logger = logger;
    private bool _UseDevelopmentFiles;

    public void ConfigureServices(WebApplicationBuilder builder) {
        bool useDevelopmentFiles;
        if (builder.Environment.IsDevelopment()) {
            try {
                var frontendProject = global::Brimborium.Angular.FrontendLocation.GetFrontendProject();
                useDevelopmentFiles = System.IO.File.Exists(frontendProject);
            } catch {
                useDevelopmentFiles = false;
            }
        } else { 
            useDevelopmentFiles = false;
        }
        this._UseDevelopmentFiles = useDevelopmentFiles;
        //this._SharedState = new SharedState(
        //    UseDevelopmentFiles: useDevelopmentFiles
        //    );
     }

    public ConfigureWebApplicationOrder GetOrder() => 
        this._UseDevelopmentFiles
        ? ConfigureWebApplicationOrder.HtmlUseStaticFiles
        : ConfigureWebApplicationOrder.HtmlStaticAssets
        ;

    public void ConfigureWebApplication(WebApplication app) {
        // this._Logger.LogConfigureWebApplicationFeatureDisabled(_FeatureName);
        if (this._UseDevelopmentFiles) {
            var webRootFQN = global::Brimborium.Angular.FrontendLocation.GetFrontendWebRootPath();
            _ = app.UseStaticFiles(new StaticFileOptions {
                FileProvider = new PhysicalFileProvider(webRootFQN),
                RequestPath = ""
            });
        } else { 
            app.MapStaticAssets().ShortCircuit();
        }
    }

    //internal sealed record SharedState(
    //    bool UseDevelopmentFiles
    //);

    //private SharedState _SharedState = new(false);
    //private bool _UseDevelopmentFiles;

    //object IConfigureSharedState.GetSharedState() => this._SharedState;

    //void IConfigureSharedState.SetSharedState(object value) {
    //    this._SharedState = (SharedState)value;
    //}
}

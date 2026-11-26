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
    private string _WebRootFQN = string.Empty;

    public void ConfigureServices(WebApplicationBuilder builder) {
        (this._UseDevelopmentFiles, this._WebRootFQN) = calc(builder.Environment.IsDevelopment());

        (bool useDevelopmentFiles, string webRootFQN) calc(bool isDevelopment) {
            if (isDevelopment) {
                try {
                    var frontendProject = global::Brimborium.Angular.FrontendLocation.GetFrontendProject();
                    var useDevelopmentFiles = System.IO.File.Exists(frontendProject);
                    if (useDevelopmentFiles) {
                        var webRootFQN = global::Brimborium.Angular.FrontendLocation.GetFrontendWebRootPath();
                        if (webRootFQN is { Length: > 0 }) {
                            if (System.IO.Directory.Exists(webRootFQN)) {
                                return (useDevelopmentFiles: true, webRootFQN: webRootFQN);
                            }
                        }
                    }
                } catch {
                }
            }
            return (useDevelopmentFiles: false, webRootFQN: string.Empty);
        }
    }

    public ConfigureWebApplicationOrder GetOrder() =>
        this._UseDevelopmentFiles
        ? ConfigureWebApplicationOrder.HtmlUseStaticFiles
        : ConfigureWebApplicationOrder.HtmlStaticAssets
        ;

    public void ConfigureWebApplication(WebApplication app) {
        // this._Logger.LogConfigureWebApplicationFeatureDisabled(_FeatureName);
        IFileProvider fileProvider;
        if (this._UseDevelopmentFiles) {
            this._Logger.LogConfigureWebApplicationFeature(_FeatureName, this._WebRootFQN);
            fileProvider = new PhysicalFileProvider(this._WebRootFQN);
            var contentTypeProvider = new Microsoft.AspNetCore.StaticFiles.FileExtensionContentTypeProvider();
            _ = app.UseStaticFiles(new StaticFileOptions {
                FileProvider = fileProvider,
                RequestPath = "",
                ContentTypeProvider = contentTypeProvider
            });
        } else {
            this._Logger.LogConfigureWebApplicationFeature(_FeatureName, "MapStaticAssets");
            app.MapStaticAssets().ShortCircuit();
            fileProvider = app.Environment.WebRootFileProvider;
        }
        {
            CachedValue cachedValue = new(fileProvider);
            mapGetIndexHtml(app, "/", cachedValue);
            mapGetIndexHtml(app, "/ui/{**rest}", cachedValue);
            //mapGetIndexHtml(app, "/", fileProvider);
            //mapGetIndexHtml(app, "/ui/{**rest}", fileProvider);
        }
    }

    public sealed record class CachedValueState(
        IFileInfo FileInfo,
        byte[] Content,
        DateTimeOffset LastModified);

    public sealed partial class CachedValue {
        private readonly IFileProvider _FileProvider;
        private CachedValueState? _State;

        public CachedValue(IFileProvider fileProvider) {
            this._FileProvider = fileProvider;
            fileProvider.Watch("index.html").RegisterChangeCallback((state) => {
                if (state is CachedValue that) {
                    that._State = default;
                }
            }, this);
        }

        public async Task<CachedValueState?> ReadContentAsync(
            CancellationToken cancellation
            ) {
            if (this._FileProvider.GetFileInfo("index.html") is not { } fileInfo) {
                return default;
            }
            if (!fileInfo.Exists) {
                return default;
            }

            {
                byte[] contentNew;
                var lastModified = fileInfo.LastModified;
                using (var stream = fileInfo.CreateReadStream()) {
                    using (var memoryStream = new MemoryStream()) {
                        await stream.CopyToAsync(memoryStream, cancellation).ConfigureAwait(false);
                        contentNew = memoryStream.ToArray();
                    }
                }
                return new CachedValueState(fileInfo, contentNew, lastModified);
            }
        }

        public async Task<IResult> ExecuteAsync(HttpContext httpContext) {
            byte[]? content;
            DateTimeOffset? lastModified;
            if (this._State is { } stateCurrent) {
                content = stateCurrent.Content;
                lastModified = stateCurrent.LastModified;
            } else {
                var stateNew = await this.ReadContentAsync(httpContext.RequestAborted).ConfigureAwait(false);
                if (stateNew is { }) {
                    content = stateNew.Content;
                    lastModified = stateNew.LastModified;
                    this._State = stateNew;
                } else {
                    content = default; 
                    lastModified = default;
                }
            }

            httpContext.SetEndpoint(null);

            if (content is { }) {
                return Results.File(fileContents: content, contentType: "text/html; charset=utf-8", lastModified: lastModified);
            } else {
                return Results.NotFound();
            }
        }
    }
    private IEndpointConventionBuilder mapGetIndexHtml(
        WebApplication app,
        [StringSyntax("Route")] string pattern,
        CachedValue cachedValue) {
        var result = app.MapGet(
                pattern: pattern,
                handler: (HttpContext httpContext) => cachedValue.ExecuteAsync(httpContext))
            .DisableHttpMetrics()
            .RequireAuthorization()
            ;
        return result;
    }
}

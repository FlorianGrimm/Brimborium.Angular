namespace Brimborium.Angular.API;

[Injectio.Attributes.RegisterSingleton<IMinimalApiEndpoint>(Duplicate = DuplicateStrategy.Append)]
public class ProjectAPI(
    ILogger<ProjectAPI> logger
    ) : IMinimalApiEndpoint {
    private readonly ILogger<ProjectAPI> _Logger = logger;

    private List<global::Brimborium.Angular.API.Project> _ListProject = new() {
        new global::Brimborium.Angular.API.Project(
            ProjectId: Guid.Parse("00000000-0000-0000-0000-000000000001"),
            Name: "one"
        ),
        new global::Brimborium.Angular.API.Project(
            ProjectId: Guid.Parse("00000000-0000-0000-0000-000000000002"),
            Name: "two"
        )
    };

    public void MapMinimalApi(WebApplication builder) {
        builder.MapGet("/api/Project", async (HttpContext httpContext) => {
            this._Logger.LogInformation("GET /api/Project");
            var user = httpContext.User;
            var result = await this.GetListProject(user, httpContext.RequestAborted).ConfigureAwait(false);
            return result;
        });
        builder.MapGet("/api/Project/{projectId}", async (HttpContext httpContext, Guid projectId) => {
            this._Logger.LogInformation("GET /api/Project");
            var user = httpContext.User;
            var result = await this.GetProject(projectId, user, httpContext.RequestAborted).ConfigureAwait(false);
            return result is null ? Results.NotFound() : Results.Ok(result);
        });
    }

    public async Task<List<global::Brimborium.Angular.API.Project>> GetListProject(System.Security.Claims.ClaimsPrincipal user, CancellationToken requestAborted) {
        await Task.Delay(1);
        List<global::Brimborium.Angular.API.Project> result;
        result = new(this._ListProject);
        return result;
    }

    public async Task<global::Brimborium.Angular.API.Project?> GetProject(Guid projectId, System.Security.Claims.ClaimsPrincipal user, CancellationToken requestAborted) {
        await Task.Delay(1);
        return this._ListProject.FirstOrDefault(p => projectId == p.ProjectId);
    }
}

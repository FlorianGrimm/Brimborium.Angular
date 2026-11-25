using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Brimborium.Angular.API;

[Injectio.Attributes.RegisterSingleton<IMinimalApiEndpoint>(Duplicate = DuplicateStrategy.Append)]
public partial class ProjectAPI(
    IProjectLogic projectLogic,
    ILogger<ProjectAPI> logger
    ) : IMinimalApiEndpoint {
    private readonly IProjectLogic _ProjectLogic = projectLogic;
    private readonly ILogger<ProjectAPI> _Logger = logger;

    public void MapMinimalApi(WebApplication builder) {
        builder.MapGet("/api/Project", async (HttpContext httpContext) => await this.GetListProjectAsync(httpContext.User, httpContext.RequestAborted).ConfigureAwait(false));
        builder.MapGet("/api/Project/{projectId}", async (HttpContext httpContext, Guid projectId) => await this.GetProjectAsync(httpContext.User, projectId, httpContext.RequestAborted).ConfigureAwait(false));
        builder.MapPost("/api/Project", async (HttpContext httpContext, [FromBody] Project project) => await this.UpdateProjectAsync(httpContext.User, project, httpContext.RequestAborted).ConfigureAwait(false));
    }

    public async Task<Results<UnauthorizedHttpResult, ProblemHttpResult, Ok<List<Project>>>> GetListProjectAsync(
        System.Security.Claims.ClaimsPrincipal httpContextUser,
        CancellationToken cancellation
        ) {
        Results<UnauthorizedHttpResult, ProblemHttpResult, Ok<List<Project>>> typedResults;
        try {
            this._Logger.LogInformation("GET /api/Project");

            if (!User.Convert(httpContextUser).TryGet(out var user, out var userProblem)) {
                return typedResults = TypedResults.Unauthorized();
            }

            var resultListProject = await this._ProjectLogic.GetListProjectAsync(user, cancellation).ConfigureAwait(false);
            if (!resultListProject.TryGet(out var listProject, out var problem)) {
                return typedResults = TypedResults.Problem(problem?.Detail);
            } else {
                return typedResults = TypedResults.Ok(listProject);
            }
        } catch (System.Exception error) {
            this._Logger.LogError(error, "Error occurred while processing GET /api/Project");
            return typedResults = TypedResults.Problem("An unexpected error occurred.");
        }
    }

    public async Task<Results<UnauthorizedHttpResult, ProblemHttpResult, Ok<Project>>> GetProjectAsync(
        System.Security.Claims.ClaimsPrincipal httpContextUser,
        Guid projectId,
        CancellationToken cancellation) {
        Results<UnauthorizedHttpResult, ProblemHttpResult, Ok<Project>> typedResults;
        try {
            this._Logger.LogInformation("GET /api/Project");
            if (!User.Convert(httpContextUser).TryGet(out var user, out var userProblem)) {
                return typedResults = TypedResults.Unauthorized();
            }

            var result = await this._ProjectLogic.GetProjectAsync(projectId, user, cancellation).ConfigureAwait(false);
            if (!result.TryGet(out var project, out var problem)) {
                return typedResults = TypedResults.Problem(problem.Detail);
            } else {
                return typedResults = TypedResults.Ok(project);
            }
        } catch (System.Exception error) {
            this._Logger.LogError(error, "Error occurred while processing GET /api/Project/{{projectId}}");
            return typedResults = TypedResults.Problem("An unexpected error occurred.");
        }
    }
    
    public async Task<Results<UnauthorizedHttpResult, ProblemHttpResult, Ok<Project>>> UpdateProjectAsync(
        System.Security.Claims.ClaimsPrincipal httpContextUser,
        Project project, 
        CancellationToken cancellation
        ) {
        Results<UnauthorizedHttpResult, ProblemHttpResult, Ok<Project>> typedResults;
        try {
            this._Logger.LogInformation("GET /api/Project");
            if (!User.Convert(httpContextUser).TryGet(out var user, out var userProblem)) {
                return typedResults = TypedResults.Unauthorized();
            }

            var result = await this._ProjectLogic.UpdateAsync(project, user, cancellation).ConfigureAwait(false);
            if (!result.TryGet(out var resultProject, out var problem)) {
                return typedResults = TypedResults.Problem(problem.Detail);
            } else {
                return typedResults = TypedResults.Ok(resultProject);
            }
        } catch (System.Exception error) {
            this._Logger.LogError(error, "Error occurred while processing POST /api/Project");
            return typedResults = TypedResults.Problem("An unexpected error occurred.");
        }
    }
}
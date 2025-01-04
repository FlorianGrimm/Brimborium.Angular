using Brimborium.DependencyInjection.Registration;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using Microsoft.OpenApi;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.Extensions.Options;
using Brimborium.Macro.Service;

namespace Brimborium.Macro.WebApp.API;

public class Code : IMinimalAPI {
    private readonly SolutionService _SolutionService;
    private readonly WorkspaceService _WorkspaceService;

    public Code(
        SolutionService solutionService,
        WorkspaceService workspaceService
        ) {
        this._SolutionService = solutionService;
        this._WorkspaceService = workspaceService;
    }

    public void MapEndpoints(ApplicationBuilder app, RouteGroupBuilder endpoints) {
        var groupCode = endpoints.MapGroup("Code").WithOpenApi(
            (op) => {
                // op.OperationId= "Code";
                op.Summary = "";
                op.Description = "";
                return op;
            });

        groupCode.MapPost("OpenSolution", async (string solutionFilePath, HttpContext context) => {
            await this._SolutionService.OpenSolutionAsync(solutionFilePath);
            return;
            
        }).WithOpenApi(
            (op) => {
                // op.OperationId= "Code";
                op.Summary = "";
                op.Description = "";
                return op;
            });
    }
}

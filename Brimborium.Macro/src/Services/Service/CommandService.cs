using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.MSBuild;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brimborium.Macro.Service;
public class CommandService {
    private readonly SolutionService _SolutionService;

    public CommandService(
        SolutionService solutionService
        ) {
        this._SolutionService = solutionService;
    }

    public async Task<Solution> OpenSolution(
        string solutionFilePath,
        IProgress<ProjectLoadProgress>? progress = null,
        CancellationToken cancellationToken = default) {
        return await this._SolutionService.OpenSolutionAsync(solutionFilePath, progress, cancellationToken);
    }
}

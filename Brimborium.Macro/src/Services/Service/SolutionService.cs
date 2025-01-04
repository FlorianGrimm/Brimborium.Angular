using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.MSBuild;

namespace Brimborium.Macro.Service;

public class SolutionService {
    public SolutionService(WorkspaceService? workspaceService) {
        this.WorkspaceService = workspaceService;
        this.ProgressProjectLoad = new(null);
    }

    public WorkspaceService? WorkspaceService { get; set; }

    public MSBuildWorkspace? Workspace { get; set; }

    public Solution? Solution { get; set; }
    public SolutionProgressProjectLoad ProgressProjectLoad { get; private set; }

    /// <summary>
    /// Open a solution file and all referenced projects.
    /// </summary>
    /// <param name="solutionFilePath">The path to the solution file to be opened. This may be an absolute path or a path relative to the current working directory.</param>
    /// <param name="progress">An optional <see cref="IProgress{T}"/> that will receive updates as the solution is opened.</param>
    /// <param name="cancellationToken">An optional <see cref="CancellationToken"/> to allow cancellation of this operation.</param>
    public async Task<Solution> OpenSolutionAsync(
        string solutionFilePath,
        IProgress<ProjectLoadProgress>? progress = null,
        CancellationToken cancellationToken = default) {
        var workspace = (this.Workspace ??= (
                this.WorkspaceService ?? throw new InvalidOperationException("WorkspaceService is null")
                ).CreateWorkspace());
        var progressProjectLoad = new SolutionProgressProjectLoad(progress);
        var solution = await workspace.OpenSolutionAsync(solutionFilePath, progressProjectLoad, cancellationToken);
        this.Solution = solution;
        this.ProgressProjectLoad = progressProjectLoad;
        return solution;
    }

    }
public class SolutionProgressProjectLoad : IProgress<ProjectLoadProgress> {
    private IProgress<ProjectLoadProgress>? _Progress;

    public SolutionProgressProjectLoad(IProgress<ProjectLoadProgress>? progress) {
        this._Progress = progress;
    }

    public List<ProjectLoadProgress> ListProjectLoadProgress { get; } = [];

    public void Report(ProjectLoadProgress value) {
        this.ListProjectLoadProgress.Add(value);
        if (this._Progress is not null) {
            this._Progress.Report(value);
        }
    }
}

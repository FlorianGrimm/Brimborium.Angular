namespace Brimborium.Angular.Logic;

public interface IProjectLogic {
    Task<ResultAPI<List<Project>>> GetListProjectAsync(User user, CancellationToken requestAborted);
    Task<ResultAPI<Project>> GetProjectAsync(Guid projectId, User user, CancellationToken requestAborted);
    Task<ResultAPI<Project>> UpdateAsync(Project project, User user, CancellationToken requestAborted);
}

[Injectio.Attributes.RegisterSingleton<IProjectLogic>()]
public class ProjectLogic : IProjectLogic {
    private readonly ProjectRepository _ProjectRepository;
    private readonly IQueueService<IPostRequestItem> _QueueService;

    public ProjectLogic(
        ProjectRepository projectRepository,
        IQueueService<IPostRequestItem> queueService) {
        this._ProjectRepository = projectRepository;
        this._QueueService = queueService;
    }

    public async Task<ResultAPI<List<Project>>> GetListProjectAsync(User user, CancellationToken requestAborted) {
        var result = await this._ProjectRepository.GetListProjectAsync(user, requestAborted);
        return result;
    }

    public async Task<ResultAPI<Project>> GetProjectAsync(Guid projectId, User user, CancellationToken requestAborted) {
        var result = await this._ProjectRepository.GetProjectAsync(projectId, user, requestAborted);
        return result;
    }

    public async Task<ResultAPI<Project>> UpdateAsync(Project project, User user, CancellationToken requestAborted) {
        RequestContext requestContext = new();
        var result = await this._ProjectRepository.UpdateAsync(project, user, requestContext, requestAborted);
        await requestContext.ExecutePostCommitActionAsync(requestAborted).ConfigureAwait(false);
        await requestContext.TransferPostRequestActionAsync(this._QueueService, requestAborted).ConfigureAwait(false);
        return result;
    }
}

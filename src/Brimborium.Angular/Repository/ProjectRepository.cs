using Brimborium.Angular.Service;

namespace Brimborium.Angular.Repository;

[Injectio.Attributes.RegisterSingleton]
public sealed partial class ProjectRepository {
    private readonly Lock _Lock = new();
    private readonly IEventService<EventMessage<Project>> _EventService;
    private readonly ILogger<ProjectRepository> _Logger;
    private readonly List<global::Brimborium.Angular.API.Project> _ListProject = new() {
        new Project(
            ProjectId: Guid.Parse("00000000-0000-0000-0000-000000000001"),
            Name: "one"
        ),
        new Project(
            ProjectId: Guid.Parse("00000000-0000-0000-0000-000000000002"),
            Name: "two"
        )
    };

    public ProjectRepository(
        IEventService<EventMessage<Project>> eventService,
        ILogger<ProjectRepository> logger
        ) {
        this._EventService = eventService;
        this._Logger = logger;
    }

    public Task<ResultAPI<List<Project>>> GetListProjectAsync(User user, CancellationToken requestAborted) {
        using (this._Lock.EnterScope()) {
            List<global::Brimborium.Angular.API.Project> result;
            result = new(this._ListProject);
            return result.ToTaskResult();
        }
    }

    public Task<ResultAPI<Project>> GetProjectAsync(Guid projectId, User user, CancellationToken requestAborted) {
        using (this._Lock.EnterScope()) {
            foreach (var project in this._ListProject) {
                if (project.ProjectId == projectId) {
                    return project.ToTaskResult();
                }
            }
            return (NotFoundProblem.Create(nameof(Project), projectId.ToString())).ToTaskResult<Project>();
        }
    }

    public Task<ResultAPI<Project>> UpdateAsync(Project project, User user, RequestContext requestContext, CancellationToken requestAborted) {
        using (this._Lock.EnterScope()) {
            var projectId = project.ProjectId;
            for (var index = 0; index < this._ListProject.Count; index++) {
                var existingProject = this._ListProject[index];
                if (existingProject.ProjectId == projectId) {
                    this._ListProject[index] = project;
                    requestContext.AddPostRequestAction(new PostRequestItem<EventUpdate<Project>>(new(project)));
                    return project.ToTaskResult();
                }
            }
            return (NotFoundProblem.Create(nameof(Project), projectId.ToString())).ToTaskResult<Project>();
        }
    }
}

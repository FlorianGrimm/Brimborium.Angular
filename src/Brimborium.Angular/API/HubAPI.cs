namespace Brimborium.Angular.API;

public sealed partial class Receiver : IClientContract {
    public async Task OnUpdated(Project project) {
        await Task.CompletedTask;
    }
}

[Injectio.Attributes.RegisterSingleton<IConfigureServices>(Duplicate = DuplicateStrategy.Append)]
[Injectio.Attributes.RegisterSingleton<IConfigureWebApplication>(Duplicate = DuplicateStrategy.Append)]
public sealed partial class HubAPIConfigureWebApplication : IConfigureServices, IConfigureWebApplication {
    public void ConfigureServices(WebApplicationBuilder builder) {
        builder.Services.AddSignalR();
    }

    public void ConfigureWebApplication(WebApplication app) {
        app.MapHub<HubAPI>("/hub/api");
    }

    public ConfigureWebApplicationOrder GetOrder() => ConfigureWebApplicationOrder.MinimalApiEndpoint;
}

public sealed partial class HubAPI : Hub, IHubContract {
    private readonly IProjectLogic _ProjectLogic;
    private readonly ILogger<HubAPI> _Logger;

    public HubAPI(
        IProjectLogic projectLogic,
        ILogger<HubAPI> logger) {
        this._ProjectLogic = projectLogic;
        this._Logger = logger;
    }

    public async Task<ResponseAPI<Project>> Update(Project project) {
        this._Logger.LogInformation("GET /api/Project");
        var resultUser = User.Convert(this.Context.User);
        if (!resultUser.TryGet(out var user, out var userProblem)) {
            return new ResponseAPI<Project>(Status: userProblem.Status ?? 401, Content: default, Problem: userProblem);
        }

        var result = await this._ProjectLogic.UpdateAsync(project, user, this.Context.ConnectionAborted).ConfigureAwait(false);
        if (!result.TryGet(out var resultProject, out var problem)) {
            return new ResponseAPI<Project>(Status: problem.Status ?? 400, Content: default, Problem: problem);
        } else {
            await this.Clients.All.SendAsync(nameof(IClientContract.OnUpdated), resultProject);
            return new ResponseAPI<Project>(Status: 200, Content: resultProject, Problem: default);
        }
    }
}

[Injectio.Attributes.RegisterSingleton<IPostRequestItemReceiver<EventUpdate<Project>>>()]
public class PostRequestItemReceiverEventUpdateProject : IPostRequestItemReceiver<EventUpdate<Project>> {
    private readonly HubReceiver _HubReceiver;

    public PostRequestItemReceiverEventUpdateProject(
        HubReceiver hubReceiver
        ) {
        this._HubReceiver = hubReceiver;
    }

    public async Task HandleAsync(EventUpdate<Project> item, IServiceProvider serviceProvider, CancellationToken cancellation) {
        await this._HubReceiver.OnUpdateAsync(item.Item, cancellation).ConfigureAwait(false);
        //builder.Services.AddSignalR();
    }
}

[Injectio.Attributes.RegisterSingleton<HubReceiver>()]
public sealed class HubReceiver {
    private readonly IHubContext<HubAPI> _HubContext;

    public HubReceiver(
        IHubContext<HubAPI> hubContext
        ) {
        this._HubContext = hubContext;
    }

    public async Task OnUpdateAsync(Project item, CancellationToken cancellation) {
        //await this._HubContext.Clients.All.OnUpdated(item).ConfigureAwait(false);
        await this._HubContext.Clients.All.SendAsync(nameof(IClientContract.OnUpdated), item, cancellation);
    }
}
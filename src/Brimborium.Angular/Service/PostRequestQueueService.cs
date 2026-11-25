namespace Brimborium.Angular.Service;

public sealed partial class PostRequestQueueServiceHost : QueueServiceHost<IQueueService<IPostRequestItem>> {
    [Injectio.Attributes.RegisterServices]
    public static void RegisterServices(IServiceCollection services) {
        _ = services.AddHostedService<PostRequestQueueServiceHost>();
        _ = services.AddSingleton<IQueueService<IPostRequestItem>, PostRequestQueueService>();
    }

    public PostRequestQueueServiceHost(
        IQueueService<IPostRequestItem> queueService,
        ILogger<PostRequestQueueServiceHost> logger
        ) : base(queueService, logger) {
    }
}

public sealed partial class PostRequestQueueService : QueueService<IPostRequestItem> {
    public PostRequestQueueService(
        IServiceProvider serviceProvider,
        System.TimeProvider timeProvider,
        ILogger<PostRequestQueueService> logger
        ) : base(serviceProvider, timeProvider, logger) {
    }
}

public interface IPostRequestItem : IQueueItem, ISharedScopeQueueItem {
}

public record class PostRequestItem<T>(T Item) : IPostRequestItem, IQueueItem, ISharedScopeQueueItem {
    public bool AllowSharedScope { get; set; } = true;

    public async Task HandleAsync(IServiceProvider serviceProvider, CancellationToken cancellation) {
        if (serviceProvider.GetServices<IPostRequestItemReceiver<T>>() is { } list) {
            foreach (var receiver in list) {
                if (receiver is ISharedScopeQueueItem sharedScopeQueueItem) {
                    if (sharedScopeQueueItem.AllowSharedScope) {
                        //
                    } else {
                        AllowSharedScope = false;
                    }
                }
                await receiver.HandleAsync(this.Item, serviceProvider, cancellation).ConfigureAwait(false);
            }
        }
    }
}

public interface IPostRequestItemReceiver<T> {
    Task HandleAsync(T item, IServiceProvider serviceProvider, CancellationToken cancellation);
}

namespace Brimborium.Angular.Service;

public sealed partial class LongRunningQueueServiceHost : QueueServiceHost<LongRunningQueueService> {
    [Injectio.Attributes.RegisterServices]
    public static void RegisterServices(IServiceCollection services) {
        _ = services.AddHostedService<LongRunningQueueServiceHost>();
        _ = services.AddSingleton<LongRunningQueueService>();
    }

    public LongRunningQueueServiceHost(
        LongRunningQueueService queueService,
        ILogger<LongRunningQueueServiceHost> logger
        ) : base(queueService, logger) {
    }
}

public sealed partial class LongRunningQueueService : QueueService<IQueueItem> {
    public LongRunningQueueService(
        IServiceProvider serviceProvider,
        System.TimeProvider timeProvider,
        ILogger<LongRunningQueueService> logger
        ) : base(serviceProvider, timeProvider, logger) {
    }
}

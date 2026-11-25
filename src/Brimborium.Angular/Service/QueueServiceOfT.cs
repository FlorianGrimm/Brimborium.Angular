using System.Threading.Channels;

namespace Brimborium.Angular.Service;

public class QueueServiceHost<TQueueService> : BackgroundService
    where TQueueService : IQueueService {

    private readonly TQueueService _QueueService;
    private readonly ILogger _Logger;

    protected QueueServiceHost(
        TQueueService queueService,
        ILogger logger
        ) {
        this._QueueService = queueService;
        this._Logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
        try {
            await this._QueueService.ExecuteAsync(stoppingToken).ConfigureAwait(false);
        } catch (System.Exception error) {
            this._Logger.LogError(error, "QueueServiceHost");
        }
    }
}

public interface IQueueService {
    Task ExecuteAsync(CancellationToken cancellation);
}

public interface IQueueService<TQueueItem> : IQueueService where TQueueItem : IQueueItem {
    Task Add(TQueueItem queueItem, CancellationToken cancellation);
}

public interface IQueueServiceReport {
    ValueTask ReportAddAsync(IQueueItem queueItem, DateTimeOffset dtoAdd);
    ValueTask ReportStartAsync(IQueueItem queueItem, DateTimeOffset dtoStart);
    ValueTask ReportFinishAsync(IQueueItem queueItem, Exception? error, DateTimeOffset dtoStart, DateTimeOffset dtoFinish);
}

public partial class QueueService<TQueueItem>
    : IQueueService<TQueueItem>
    , IQueueService
    where TQueueItem : IQueueItem {
    //public static void RegisterQueueServiceHost(IServiceCollection services) {
    //    _ = services.AddSingleton<IQueueService, QueueService>();
    //    _ = services.AddSingleton<System.TimeProvider>(System.TimeProvider.System);
    //}

    private readonly Channel<TQueueItem> _Channel;
    private readonly IServiceProvider _ServiceProvider;
    private readonly TimeProvider _TimeProvider;
    private readonly ILogger _Logger;
    private readonly IQueueServiceReport[] _ListQueueServiceReport;

    protected QueueService(
        IServiceProvider serviceProvider,
        System.TimeProvider timeProvider,
        ILogger logger
        ) {
        this._ServiceProvider = serviceProvider;
        this._TimeProvider = timeProvider;
        this._Logger = logger;
        this._Channel = System.Threading.Channels.Channel.CreateBounded<TQueueItem>(
            new System.Threading.Channels.BoundedChannelOptions(100) {
                FullMode = System.Threading.Channels.BoundedChannelFullMode.Wait
            });
        this._ListQueueServiceReport = this._ServiceProvider.GetServices<IQueueServiceReport>().ToArray();
    }

    public async Task Add(TQueueItem queueItem, CancellationToken cancellation) {
        {
            await ReportAdd(queueItem, cancellation).ConfigureAwait(false);

            var writer = this._Channel.Writer;
            await writer.WriteAsync(queueItem).ConfigureAwait(false);
            writer.Complete();
        }

        async Task ReportAdd(IQueueItem queueItem, CancellationToken cancellation) {
            if (0 < this._ListQueueServiceReport.Length) {
                System.DateTimeOffset dtoAdd = this._TimeProvider.GetUtcNow();
                foreach (var queueServiceReport in this._ListQueueServiceReport) {
                    await queueServiceReport.ReportAddAsync(queueItem, dtoAdd).ConfigureAwait(false);
                }
            }
        }
    }

    public async Task ExecuteAsync(CancellationToken cancellation) {
        var reader = this._Channel.Reader;
        ServiceProviderScope serviceProviderScope = new(this._ServiceProvider);

        while (false == cancellation.IsCancellationRequested) {
            if (reader.TryRead(out var queueItem)) {
                if (queueItem is ISharedScopeQueueItem sharedScopeQueueItem) {
                    if (sharedScopeQueueItem.AllowSharedScope) {
                        // keep scope
                    } else {
                        serviceProviderScope.DisposeScope();
                    }
                }
                {
                    System.DateTimeOffset dtoStart = await ReportStartAsync(queueItem).ConfigureAwait(false);
                    var error = await this.HandleAsync(queueItem, serviceProviderScope, cancellation).ConfigureAwait(false); ;
                    await ReportFinishAsync(queueItem, error, dtoStart).ConfigureAwait(false);
                }
                continue;
            }
            serviceProviderScope.DisposeScope();
            try {
                var doLoop = await reader.WaitToReadAsync(cancellation).ConfigureAwait(false);
                if (doLoop) {
                    continue;
                } else {
                    break;
                }
            } catch (OperationCanceledException) {
                break;
            }
        }

        async Task<DateTimeOffset> ReportStartAsync(IQueueItem queueItem) {
            System.DateTimeOffset dtoStart = this._TimeProvider.GetUtcNow();
            if (0 < this._ListQueueServiceReport.Length) {
                foreach (var queueServiceReport in this._ListQueueServiceReport) {
                    await queueServiceReport.ReportStartAsync(queueItem, dtoStart).ConfigureAwait(false);
                }
            }
            return dtoStart;
        }

        async Task ReportFinishAsync(IQueueItem queueItem, Exception? error, DateTimeOffset dtoStart) {
            var dtoFinish = this._TimeProvider.GetUtcNow();
            if (0 < this._ListQueueServiceReport.Length) {
                foreach (var queueServiceReport in this._ListQueueServiceReport) {
                    await queueServiceReport.ReportFinishAsync(queueItem, error, dtoStart, dtoFinish).ConfigureAwait(false);
                }
            }
        }
    }


    public virtual async Task<System.Exception?> HandleAsync(IQueueItem queueItem, ServiceProviderScope serviceProviderScope, CancellationToken stoppingToken) {
        try {
            var serviceProvider = serviceProviderScope.ServiceProvider;
            await queueItem.HandleAsync(serviceProvider, stoppingToken).ConfigureAwait(false);
            return default;
        } catch (System.Exception error) {
            this._Logger.LogInformation(error, "Handle");
            return error;
        }
    }
}


public class ServiceProviderScope : IDisposable {
    private IServiceProvider? _GlobalServiceProvider;
    private IServiceScope? _ScopeServiceScope;
    private IServiceProvider? _ScopedServiceProvider;

    public ServiceProviderScope(IServiceProvider globalServiceProvider) {
        this._GlobalServiceProvider = globalServiceProvider;
    }
    public IServiceProvider ServiceProvider {
        get {
            if (this._ScopedServiceProvider is { } scopedServiceProvider) {
                return scopedServiceProvider;
            }
            if (this._GlobalServiceProvider is not { } globalServiceProvider) {
                throw new ObjectDisposedException(nameof(ServiceProviderScope));
            }

            var globalServiceScope = this._ScopeServiceScope = globalServiceProvider.CreateScope();
            return this._ScopedServiceProvider = globalServiceScope.ServiceProvider;
        }
    }

    public void Dispose() {
        using (var scope = this._ScopeServiceScope) {
            this._ScopedServiceProvider = null;
            this._ScopeServiceScope = null;
            this._ScopedServiceProvider = null;
        }
    }

    public void DisposeScope() {
        using (var scope = this._ScopeServiceScope) {
            this._ScopeServiceScope = null;
            this._ScopedServiceProvider = null;
        }
    }
}

public interface IQueueItem {
    Task HandleAsync(IServiceProvider serviceProvider, CancellationToken cancellation);
}

public interface ISharedScopeQueueItem {
    bool AllowSharedScope { get; }
}

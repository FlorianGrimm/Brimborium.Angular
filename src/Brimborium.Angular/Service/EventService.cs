namespace Brimborium.Angular.Service;

[Injectio.Attributes.RegisterSingleton()]
public class EventFactoryService {
    [Injectio.Attributes.RegisterServices()]
    public static void RegisterServices(IServiceCollection services) {
        services.Add(ServiceDescriptor.Transient(typeof(IEventService<>), typeof(EventService<>)));
    }
    public IEventService<T> Create<T>() {
        return new EventService<T>();
    }
}
public interface IEventService<T> {
    IDisposable RegisterAsyncEventHandler(Func<T, Task> eventHandler);
    Task SendAsync(T message, CancellationToken cancellation);
}

public record EventMessage<T>(
    string EventName,
    T Data);

public sealed partial class EventService<T> : IEventService<T>, IDisposable {
    private readonly List<RegisteredAsyncEventHandler<T>> _ListEventHandler = new();
    private RegisteredAsyncEventHandler<T>[] _ArrEventHandler = Array.Empty<RegisteredAsyncEventHandler<T>>();

    public EventService() {
    }

    public IDisposable RegisterAsyncEventHandler(Func<T, Task> eventHandler) {
        var registeredEventHandler = new RegisteredAsyncEventHandler<T>(eventHandler);
        lock (this._ListEventHandler) {
            this._ListEventHandler.Add(registeredEventHandler);
            this._ArrEventHandler = this._ListEventHandler.ToArray();
        }
        return registeredEventHandler;
    }

    public bool HasEventHandler() {
        return this._ArrEventHandler.Length > 0;
    }

    public async Task SendAsync(T message, CancellationToken cancellation) {
        if (0 == this._ArrEventHandler.Length) {
            return;
        }
        bool refreshNeeded = false;
        foreach (var registeredEventHandler in this._ArrEventHandler) {
            await registeredEventHandler.SendAsync(message).ConfigureAwait(false);
            if (registeredEventHandler.IsDisposed) {
                refreshNeeded = true;
            }
        }
        if (refreshNeeded) {
            lock (this._ListEventHandler) {
                for (var index = this._ArrEventHandler.Length - 1; 0 <= index; index--) {
                    var registeredEventHandler = this._ArrEventHandler[index];
                    if (registeredEventHandler.IsDisposed) {
                        this._ListEventHandler.RemoveAt(index);
                    }
                }
                this._ArrEventHandler = this._ListEventHandler.ToArray();
            }
        }
    }

    private void Dispose(bool disposing) {

    }

    ~EventService() {
        this.Dispose(disposing: false);
    }

    public void Dispose() {
        this.Dispose(disposing: true);
        System.GC.SuppressFinalize(this);
    }
}

public sealed partial class RegisteredAsyncEventHandler<T> : IDisposable {
    private Func<T, Task>? _EventHandler;

    public RegisteredAsyncEventHandler(Func<T, Task> eventHandler) {
        this._EventHandler = eventHandler;
    }

    public Task SendAsync(T data) {
        if (this._EventHandler is { } eventHandler) {
            return eventHandler(data);
        } else {
            return Task.CompletedTask;
        }
    }

    public bool IsDisposed => this._EventHandler is null;

    public void Dispose() {
        this._EventHandler = default;
    }
}

public sealed partial class DisposableCollection : IDisposable {
    private IDisposable[]? _Disposables;
    public DisposableCollection() { }

    public void Add(IDisposable disposable) {
        while (true) {
            var currentDisposables = this._Disposables;
            if (currentDisposables is null) {

                var newDisposables = new IDisposable[] { disposable };

                var oldDisposables = System.Threading.Interlocked.CompareExchange(ref this._Disposables, newDisposables, currentDisposables);
                if (object.ReferenceEquals(oldDisposables, currentDisposables)) {
                    break;
                } else {
                    continue;
                }
            } else {
                var newDisposables = new IDisposable[currentDisposables.Length + 1];
                Array.Copy(currentDisposables, newDisposables, currentDisposables.Length);
                newDisposables[newDisposables.Length - 1] = disposable;

                var oldDisposables = System.Threading.Interlocked.CompareExchange(ref this._Disposables, newDisposables, currentDisposables);
                if (object.ReferenceEquals(oldDisposables, currentDisposables)) {
                    break;
                } else {
                    continue;
                }
            }
        }
    }

    private void Dispose(bool disposing) {
        if (this._Disposables is null) {
            return;
        }
        var disposables = System.Threading.Interlocked.Exchange(ref this._Disposables, null);
        if (disposables is not null) {
            foreach (var disposable in disposables) {
                disposable.Dispose();
            }
        }
    }

    ~DisposableCollection() {
        this.Dispose(disposing: false);
    }

    public void Dispose() {
        this.Dispose(disposing: true);
        System.GC.SuppressFinalize(this);
    }
}
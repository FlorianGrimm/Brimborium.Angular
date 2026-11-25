namespace Brimborium.Angular.Service;

public class LazyResolver<T> {
    public static LazyResolver<T> CreateFor(T value) {
        LazyResolver<T> result = new(NullServiceProvider.Instance);
        result.Value = value;
        return result;
    }

    private readonly IServiceProvider _ServiceProvider;
    private T? _Value;
    public LazyResolver(IServiceProvider serviceProvider) {
        this._ServiceProvider = serviceProvider;
    }

    public T Value {
        get {
            return this._Value ??= (T)this._ServiceProvider.GetRequiredService(typeof(T));
        }
        set {
            this._Value = value;
        }
    }
}

public sealed class NullServiceProvider : IServiceProvider {
    public static NullServiceProvider Instance { get; } = new NullServiceProvider();
    public object? GetService(Type serviceType) => null;
}
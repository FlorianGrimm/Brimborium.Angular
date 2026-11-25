namespace Brimborium.Angular.Service;

public interface IServiceProviderLifetimeDetection {
    bool GetIsScoped();
}

public static class ServiceProviderLifetimeDetection {
    public static bool IsScoped(this IServiceProvider serviceProvider) {
        if (serviceProvider.GetServices<IServiceProviderLifetimeDetection>() is { } list) {
            foreach (var item in list) {
                if (item.GetIsScoped()) {
                    return true;
                }
            }
        }
        return false;
    }
}

[Injectio.Attributes.RegisterSingleton<IServiceProviderLifetimeDetection>(Duplicate = DuplicateStrategy.Append)]
public class ServiceProviderLifetimeDetectionGlobal : IServiceProviderLifetimeDetection {
    public bool GetIsScoped() => false;
}

[Injectio.Attributes.RegisterScoped<IServiceProviderLifetimeDetection>(Duplicate = DuplicateStrategy.Append)]
public class ServiceProviderLifetimeDetectionScope : IServiceProviderLifetimeDetection {
    public bool GetIsScoped() => true;
}
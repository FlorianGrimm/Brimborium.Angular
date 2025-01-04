using Brimborium.DependencyInjection.Registration;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brimborium.Macro.WebApp.API;
public static class APIServiceExtension {
    public static void AddAPIService(
        this IServiceCollection services
        ) {
        services.AddTransient<IMinimalAPI, Code>();
    }
}

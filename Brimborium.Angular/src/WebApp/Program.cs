using Brimborium.Angular.WebApp.Services.ClientAppFiles;

using Microsoft.AspNetCore.Authentication.Negotiate;
using Microsoft.AspNetCore.Builder;

namespace Brimborium.Angular.WebApp;

public class Program {
    public static void Main(string[] args) {
        var builder = WebApplication.CreateBuilder(args);

        builder.Logging.AddConsole();

        builder.Services
            .AddAuthentication(NegotiateDefaults.AuthenticationScheme)
            .AddNegotiate();

        builder.Services.AddAuthorization(options => {
            // By default, all incoming requests will be authorized according to the default policy.
            options.FallbackPolicy = options.DefaultPolicy;
        });

        builder.Services.AddRazorPages();

        builder.Services.AddClientAppFiles(configureOptions: (options) => {
            options.ListRequestPath = [
                new PathDocument(new PathString("/en-US"), new PathString("/en-US/index.html")),
                new PathDocument(new PathString("/de-DE"), new PathString("/de-DE/index.html")),
                ];
        });

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment()) {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseAuthorization();

        app.MapClientAppFiles();
        //app.UseClientAppFiles();
        app.MapStaticAssets();
        app.UseStaticFiles();

        app.Run();
    }
}

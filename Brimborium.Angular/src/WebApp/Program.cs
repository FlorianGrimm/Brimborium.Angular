using Brimborium.Angular.WebApp.Services.CopyClientAppFiles;

using Microsoft.AspNetCore.Authentication.Negotiate;

namespace Brimborium.Angular.WebApp;

public class Program {
    public static void Main(string[] args) {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services
            .AddAuthentication(NegotiateDefaults.AuthenticationScheme)
            .AddNegotiate();

        builder.Services.AddAuthorization(options => {
            // By default, all incoming requests will be authorized according to the default policy.
            options.FallbackPolicy = options.DefaultPolicy;
        });

        // builder.Services.AddRazorPages();

        builder.Services.AddClientAppFiles();

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

        app.UseDefaultFiles();
        app.MapStaticAssets();
        app.UseStaticFiles();

        //app.MapRazorPages().WithStaticAssets();
        //app.MapFallbackToFile("/index.html");

        app.MapClientAppFiles();
        app.UseMiddleware
        app.Run();
    }
}

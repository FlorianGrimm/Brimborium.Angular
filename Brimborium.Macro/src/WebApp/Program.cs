using Brimborium.DependencyInjection.Registration;

using Microsoft.AspNetCore.Authentication.Negotiate;
using Microsoft.OpenApi;

namespace Brimborium.Macro.WebApp;

public class Program {
    public static void Main(string[] args) {
        VisualStudioInstanceUtility.RegisterInstance();
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services
            .AddAuthentication(NegotiateDefaults.AuthenticationScheme)
            .AddNegotiate();

        builder.Services.AddAuthorization(options => {
            // By default, all incoming requests will be authorized according to the default policy.
            options.FallbackPolicy = options.DefaultPolicy;
        });

        builder.Services.AddOpenApi("v1", (options) => {
            options.OpenApiVersion = OpenApiSpecVersion.OpenApi3_0;
        });

        //builder.Services.AddRazorPages();

        builder.Services.AddClientAppFiles(configuration: builder.Configuration.GetSection("ClientAppFiles"));

        builder.Services.AddMacroServices();

        builder.Services.AddOptions<ProgramOptions>().BindConfiguration("");

        builder.Services.AddSignalR((options) => {
            options.EnableDetailedErrors = true;
        }).AddJsonProtocol((options) => {
            options.PayloadSerializerOptions.PropertyNamingPolicy = null;
        }).AddMessagePackProtocol((options) => { 
            options.SerializerOptions.WithCompression(MessagePack.MessagePackCompression.Lz4BlockArray);
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

        app.MapOpenApi();
        app.MapClientAppFiles();
        app.MapStaticAssets();
        app.UseStaticFiles();

        app.MapMinimalAPI();
        
        app.Run();
    }

}

using Brimborium.Macro.Service;

using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Brimborium.Macro.ConsoleServices;

public class Program {
    public static async Task Main(string[] args) {
        var solutionFilePath = args.Length > 0 ? args[0] : @"D:\github.com\FlorianGrimm\Brimborium.Angular\Brimborium.Angular\Brimborium.Angular.sln";
        var serviceProvider = ConfigureServices();
        await serviceProvider.GetRequiredService<Program>().RunAsync(solutionFilePath);
    }

    private static IServiceProvider ConfigureServices() {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", false)
            .Build();
        var services = new ServiceCollection();
        services.AddSingleton<IConfiguration>(configuration);
        services.AddSingleton<Program>();
        services.AddMacroServices();
        services.AddLogging((loggingBuilder) => {
            loggingBuilder.AddConfiguration(configuration.GetSection("Logging"));
            loggingBuilder.AddConsole();
        });
        return services.BuildServiceProvider();
    }

    private readonly SolutionService _SolutionService;

    public Program(
        SolutionService solutionService
        ) {
        this._SolutionService = solutionService;
    }

    public async Task RunAsync(string solutionFilePath) {
        var solution = await _SolutionService.OpenSolutionAsync(solutionFilePath);
        if (solution is null) { return; }
        foreach (var project in solution.Projects) {
            System.Console.Out.WriteLine($"{project.Name} - {project.Id}");
            foreach (var projectReferences in project.ProjectReferences) {
                System.Console.Out.WriteLine(projectReferences.ProjectId);
            }
            //project.AllProjectReferences
            var compilation = await project.GetCompilationAsync();
            if (compilation is null) { continue; }
            var x = compilation.SyntaxTrees.Count();
            System.Console.Out.WriteLine($"{project.Name} - {project.Id} - #{x}");

        }
        await Task.CompletedTask;
    }
}

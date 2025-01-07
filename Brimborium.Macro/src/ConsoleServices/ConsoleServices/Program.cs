using Brimborium.Macro.Service;

using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using System.Collections.Generic;

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
            if (!project.SupportsCompilation) { continue; }
            foreach (var projectReferences in project.ProjectReferences) {
                System.Console.Out.WriteLine(projectReferences.ProjectId);
            }
            //project.AllProjectReferences
            var compilation = await project.GetCompilationAsync();
            if (compilation is null) { continue; }

            var listDiagnostics = compilation.GetDiagnostics()
                .Where(diagnostics => diagnostics.Severity != DiagnosticSeverity.Hidden)
                .ToList();
            var listDiagnosticsError = new List<Diagnostic>();
            var listDiagnosticsWarning = new List<Diagnostic>();
            var listDiagnosticsInformation = new List<Diagnostic>();
            foreach (var diagnostic in listDiagnostics) {
                if (diagnostic.IsSuppressed) { continue; }
                if (diagnostic.Severity == DiagnosticSeverity.Hidden) {
                    continue; 
                }
                if (diagnostic.Severity == DiagnosticSeverity.Error) {
                    listDiagnosticsError.Add(diagnostic);
                    continue;
                }
                if (diagnostic.Severity == DiagnosticSeverity.Warning) {
                    listDiagnosticsWarning.Add(diagnostic);
                    continue;
                }
                if (diagnostic.Severity == DiagnosticSeverity.Info) {
                    listDiagnosticsInformation.Add(diagnostic);
                    continue;
                }
                listDiagnosticsError.Add(diagnostic);
                System.Console.Out.WriteLine(diagnostic.ToString());
            }

            if (0 < listDiagnosticsError.Count) {
                System.Console.Out.WriteLine($"{listDiagnosticsError.Count} errors");
                foreach (var diagnostic in listDiagnosticsError) {
                    if (diagnostic.IsSuppressed) { continue; }
                    System.Console.Out.WriteLine(diagnostic.ToString());
                }
            } else {
                System.Console.Out.WriteLine("no errors");
            }

            if (0 < listDiagnosticsWarning.Count) {
                System.Console.Out.WriteLine($"{listDiagnosticsWarning.Count} warnings");
                foreach (var diagnostic in listDiagnosticsWarning) {
                    if (diagnostic.IsSuppressed) { continue; }
                    System.Console.Out.WriteLine(diagnostic.ToString());
                }
            } else {
                System.Console.Out.WriteLine("no warnings");
            }

            if (0 < listDiagnosticsInformation.Count) {
                System.Console.Out.WriteLine($"{listDiagnosticsInformation.Count} information");
                foreach (var diagnostic in listDiagnosticsInformation) {
                    if (diagnostic.IsSuppressed) { continue; }
                    System.Console.Out.WriteLine(diagnostic.ToString());
                }
            } else {
                System.Console.Out.WriteLine("no information");
            }

            foreach (var document in project.Documents) {
                if (!document.SupportsSyntaxTree) { continue; }
                if (!document.SupportsSemanticModel) { continue; }
                var root = await document.GetSyntaxRootAsync();
                if (root is null) { continue; }
                var semanticModel = await document.GetSemanticModelAsync();
                if (semanticModel is null) { continue; }
            }

            var x = compilation.SyntaxTrees.Count();
            System.Console.Out.WriteLine($"{project.Name} - {project.Id} - #{x}");

        }
        await Task.CompletedTask;
    }
}

using Microsoft.Extensions.DependencyInjection;

using System;
using System.Collections.Generic;
using System.Text;

using Brimborium.Angular.Service;

namespace Brimborium.Angular.Service;

public class QueueServiceTests {
    [Test]
    public async Task QueueServiceTest() {
        TestQueueServiceReport testQueueServiceReport = new();
        CancellationTokenSource cts = new();
        ServiceProvider serviceProvider;
        { 
            Microsoft.Extensions.DependencyInjection.ServiceCollection serviceBuilder = new();
            serviceBuilder.AddSingleton<IQueueServiceReport>(testQueueServiceReport);
            LongRunningQueueServiceHost.RegisterServices(serviceBuilder);
            serviceProvider = serviceBuilder.BuildServiceProvider();
        }

        var queueService = serviceProvider.GetRequiredService<LongRunningQueueService>();
        var task = queueService.ExecuteAsync(cts.Token);

        await queueService.Add(new TestQueueItem() { Name = "One" }, cts.Token);
        await queueService.Add(new TestQueueItem() { Name = "Two" }, cts.Token);
        await queueService.Add(new TestQueueItem() { Name = "Three" }, cts.Token);

        await task.ConfigureAwait(false);

        string actual = string.Join(";", testQueueServiceReport.ListReport.Select(x => $"{x.Kind},{((TestQueueItem)x.QueueItem).Name},{x.Error?.Message ?? "OK"}"));
        await Assert.That(actual).IsEqualTo("Add,One,OK;Start,One,OK;Finish,One,OK;Add,Two,OK;Start,Two,OK;Finish,Two,OK;Add,Three,OK;Start,Three,OK;Finish,Three,Error");

        serviceProvider.Dispose();
        await Task.CompletedTask;
    }

    public class TestQueueItem : IQueueItem {
        public string Name { get; set; } = string.Empty;

        public Task HandleAsync(IServiceProvider serviceProvider, CancellationToken stoppingToken) {
            if (this.Name == "Three") {
                throw new InvalidOperationException("Error");
            }
            return Task.CompletedTask;
        }
        public override string ToString() {
            return this.Name;
        }
    }

    public record class TestQueueItemReport(string Kind, IQueueItem QueueItem, Exception? Error) {
        public override string ToString() {
            return $"{this.Kind},{this.QueueItem},{this.Error?.Message ?? "OK"}";
        }
    }

    public class TestQueueServiceReport : IQueueServiceReport {
        public readonly List<TestQueueItemReport> ListReport = new();
        public ValueTask ReportAddAsync(IQueueItem queueItem, DateTimeOffset dtoAdd) {
            this.ListReport.Add(new("Add", queueItem, default));
            return ValueTask.CompletedTask;
        }
        public ValueTask ReportStartAsync(IQueueItem queueItem, DateTimeOffset dtoStart) {
            this.ListReport.Add(new("Start", queueItem, default));
            return ValueTask.CompletedTask;
        }
        public ValueTask ReportFinishAsync(IQueueItem queueItem, Exception? error, DateTimeOffset dtoStart, DateTimeOffset dtoFinish) {
            this.ListReport.Add(new("Finish", queueItem, error));
            return ValueTask.CompletedTask;
        }
    }
}

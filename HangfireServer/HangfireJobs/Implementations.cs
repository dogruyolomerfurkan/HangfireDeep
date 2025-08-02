using Hangfire;
using Signatures;

namespace HangfireServer.HangfireJobs;

public class Implementations(IBackgroundJobClient backgroundJobClient) : IRecurringJobs
{
    // [Queue("concurrent")]
    public void TestConcurrentExecutionAndCancellation(IJobCancellationToken cancellationToken)
    {
        Console.WriteLine($"Test execution is working at: {DateTime.Now}");
        for (var i = 0; i < 100_000; i++)
        {
            cancellationToken.ThrowIfCancellationRequested();
            backgroundJobClient.Enqueue<Implementations>(x => x.PerformWorkCancellation(i, cancellationToken)
            );
        }
    }

    public void TestConcurrentExecutionWithoutCancellation()
    {
        Console.WriteLine($"Test execution is working at: {DateTime.Now}");
        for (var i = 0; i < 100_000; i++)
        {
            backgroundJobClient.Enqueue<Implementations>(x => x.PerformWork(i));
        }
    }
    [Queue("concurrent")]
    public async Task PerformWorkCancellation(int i, IJobCancellationToken? cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        Console.WriteLine($"Doing work #{i}");
    }
    
    [JobDisplayName("Perform Work {0}")]
    [Queue("concurrent")]
    public async Task PerformWork(int i)
    {
        Console.WriteLine($"Doing work #{i}");
    }
}
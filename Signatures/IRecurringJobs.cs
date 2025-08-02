using Hangfire;

namespace Signatures;

public interface IRecurringJobs
{
    // [DisableConcurrentExecution(60 * 2)]
    [AutomaticRetry(Attempts = 3, LogEvents = true, OnAttemptsExceeded = AttemptsExceededAction.Delete)]
    [Queue("concurrent")]
    void TestConcurrentExecutionAndCancellation(IJobCancellationToken stoppingToken);
    
    [Queue("concurrent")]
    void TestConcurrentExecutionWithoutCancellation();
}
using Hangfire;

namespace Signatures;

public interface IRecurringJobs
{
    [DisableConcurrentExecution(60 * 2)]
    [AutomaticRetry(Attempts = 3, LogEvents = true, OnAttemptsExceeded = AttemptsExceededAction.Delete)]
    [Queue("Concurrent")]
    void TestConcurrentExecution();
}
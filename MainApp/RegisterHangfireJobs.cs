using Hangfire;
using Signatures;

namespace MainApp;

public class RegisterHangfireJobs
{
    public void RegisterJobs()
    {
        RecurringJob.AddOrUpdate<IRecurringJobs>("TestConcurrentExecutionJob",
            queue: "concurrent",
            x => x.TestConcurrentExecution(),
            "* * * * *");
    }
}
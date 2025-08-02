using Hangfire;
using Signatures;

namespace MainApp;

public class RegisterHangfireJobs
{
    public static void RegisterJobs()
    {
        RecurringJob.AddOrUpdate<IRecurringJobs>("TestConcurrentExecutionJob",
            queue: "concurrent",
            x => x.TestConcurrentExecutionAndCancellation(JobCancellationToken.Null),
            "5 * * * *",
            options: new RecurringJobOptions
            {
                TimeZone = TimeZoneInfo.Utc,
                MisfireHandling = MisfireHandlingMode.Relaxed
            });
    }
}
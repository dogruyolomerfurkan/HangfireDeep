using Hangfire;
using Signatures;

namespace HangfireServer;

public class RegisterHangfireJobs(IRecurringJobManager recurring)
{
    public void RegisterJobs()
    {
        recurring.AddOrUpdate<IRecurringJobs>(
            "TestConcurrentExecutionJob", // job ID
            job => job.TestConcurrentExecutionAndCancellation(JobCancellationToken.Null),
            "5 * * * *", // cron
            new RecurringJobOptions
            {
                TimeZone = TimeZoneInfo.Utc,
                MisfireHandling = MisfireHandlingMode.Relaxed
            });
    }
}
using Signatures;

namespace HangfireServer.HangfireJobs;

public class Implementations : IRecurringJobs
{
    public void TestConcurrentExecution()
    {
        Console.WriteLine("Test execution is working at : " + DateTime.Now);
    }
}
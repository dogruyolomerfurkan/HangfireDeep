namespace HangfireServer.HangfireSetup;

public class HangfireSettings
{
    public List<HangfireServerOptions>? Servers { get; set; } 
}

public class HangfireServerOptions
{
    public string? Name { get; set; }
    public int WorkerCount { get; set; }
    public string[]? Queues { get; set; }
}

using Hangfire;
using Hangfire.PostgreSql;
using HangfireServer.HangfireJobs;
using HangfireServer.HangfireSetup;
using Signatures;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.Configure<HangfireSettings>(builder.Configuration.GetSection(nameof(HangfireSettings)));
builder.Services.AddScoped<IRecurringJobs, Implementations>();
builder.Services.AddHangfire(gc =>
{
    gc.UseSimpleAssemblyNameTypeSerializer()
        .UseDefaultTypeSerializer()
        .UseFilter(new AutomaticRetryAttribute() { Attempts = 3, DelaysInSeconds = [10, 15, 20] })
        .UsePostgreSqlStorage(
            options =>
            {
                options.UseNpgsqlConnection(builder.Configuration.GetConnectionString("HangfirePostgresql"));
            },
            new PostgreSqlStorageOptions
            {
                QueuePollInterval = TimeSpan.FromSeconds(50),
                PrepareSchemaIfNecessary = true,
                UseSlidingInvisibilityTimeout = true,
                InvisibilityTimeout = TimeSpan.FromHours(2),
                CountersAggregateInterval = TimeSpan.FromSeconds(60)
            });
});

var hangfireServers = builder.Configuration.GetSection(nameof(HangfireSettings)).Get<HangfireSettings>();

if (hangfireServers?.Servers is null) throw new NotImplementedException();

foreach (var hangfireServer in hangfireServers.Servers)
{
    builder.Services.AddHangfireServer(opt =>
    {
        opt.SchedulePollingInterval = TimeSpan.FromSeconds(50);
        opt.ServerCheckInterval = TimeSpan.FromSeconds(60);
        opt.WorkerCount = hangfireServer.WorkerCount;
        opt.Queues = hangfireServer.Queues;
    });
}

var app = builder.Build();
app.UseHangfireDashboard();
app.UseHttpsRedirection();

app.Run();
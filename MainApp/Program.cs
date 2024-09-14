using Hangfire;
using Hangfire.PostgreSql;
using MainApp;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddSingleton<RegisterHangfireJobs>();

builder.Services.AddHangfire(gc =>
{
    gc.UsePostgreSqlStorage(
        options => { options.UseNpgsqlConnection(builder.Configuration.GetConnectionString("HangfirePostgresql")); });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseHangfireDashboard();
app.UseAuthorization();
var hangfireJobs = app.Services.GetRequiredService<RegisterHangfireJobs>();
hangfireJobs.RegisterJobs();
app.MapControllers();

app.Run();
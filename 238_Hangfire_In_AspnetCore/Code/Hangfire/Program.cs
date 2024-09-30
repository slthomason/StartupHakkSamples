using Hangfire;
using Hangfire.Interface;
using Hangfire.Models;
using Hangfire.Services;
using Hangfire.SQLite;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<DatabaseContext>(options =>
    options.UseSqlite(builder.Configuration["database:local"]));


//HangFire
builder.Services.AddHangfire(config =>
    config.UseSQLiteStorage("Data Source=hangfire.db;")); // Use a valid connection string

builder.Services.AddHangfireServer();

builder.Services.AddScoped<IBackgroundJobService, BackgroundJobService>();

//Hangfire


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();





var app = builder.Build();


if (app.Environment.IsDevelopment())
{
}

//HangFire
using (var scope = app.Services.CreateScope())
{
    var jobService = scope.ServiceProvider.GetRequiredService<IBackgroundJobService>();
    await jobService.ScheduleJobs(); 
}

app.UseHangfireDashboard();
app.MapGet("/", (context) =>
{
    context.Response.Redirect("/hangfire");
    return Task.CompletedTask;
});
//HangFire

app.UseSwagger();
app.UseSwaggerUI();

app.UseRouting();
app.MapControllers();

app.UseHttpsRedirection();


app.Run();

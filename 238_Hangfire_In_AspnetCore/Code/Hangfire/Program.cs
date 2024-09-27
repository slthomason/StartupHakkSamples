using Hangfire;
using Hangfire.Interface;
using Hangfire.Models;
using Hangfire.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;


var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<DatabaseContext>(options =>
    options.UseSqlServer(builder.Configuration["database:local"]));


//HangFire
builder.Services.AddHangfire(config =>
    config.UseSqlServerStorage(builder.Configuration["database:local"])); // Use a valid connection string

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
app.MapGet("/", () => "Hangfire with .NET Core!");
//HangFire

app.UseSwagger();
app.UseSwaggerUI();

app.UseRouting();
app.MapControllers();

app.UseHttpsRedirection();


app.Run();



using HealthCheck;
using HealthCheck.Database;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var _config = builder.Configuration;


builder.Services.AddControllers();


builder.Services.AddSingleton<IDbConnectionService>(provider =>
    new DbConnectionService(_config["database:local"]));


builder.Services.AddHealthChecks();
builder.Services.AddHealthChecks()
    .AddCheck<DatabaseHealthCheck>("Sample");


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();  // Enable detailed error pages in development
    app.UseSwagger();
    app.UseSwaggerUI();
}



app.MapHealthChecks("/healthz");

app.UseRouting();
app.UseHttpsRedirection();
app.MapControllers();

app.Run();


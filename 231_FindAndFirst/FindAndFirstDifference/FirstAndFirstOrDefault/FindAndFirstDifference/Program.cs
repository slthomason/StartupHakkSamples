using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;
using FindAndFirstDifference;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Configure BenchmarkDotNet
var config = ManualConfig.Create(DefaultConfig.Instance)
                         .WithArtifactsPath("Your local path to the harddisk to store the logs");

var summary = BenchmarkRunner.Run<ListTest>(config);
// Configure BenchmarkDotNet

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    
}

app.UseHttpsRedirection();


app.UseRouting();
app.MapControllers();

app.Run();
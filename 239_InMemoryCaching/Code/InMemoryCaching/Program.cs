using InMemoryCaching.Database;
using InMemoryCaching.Interface;
using InMemoryCaching.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddMemoryCache();

builder.Services.AddScoped<ICatRepo, CatRepo>();
builder.Services.AddDbContext<DatabaseContext>(options =>
        options.UseSqlite(builder.Configuration["database:local"]));

        
builder.Services.AddControllers();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
}
app.UseSwagger();
app.UseSwaggerUI();

app.UseRouting();
app.MapControllers();

app.UseHttpsRedirection();

app.Run();


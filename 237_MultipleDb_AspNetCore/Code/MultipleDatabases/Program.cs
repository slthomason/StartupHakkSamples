using Microsoft.EntityFrameworkCore;
using MultipleDatabases.Database;
using MultipleDatabases.Interface;
using MultipleDatabases.Service;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<UsersContext>(options =>
            options.UseSqlite(builder.Configuration.GetConnectionString("user")), ServiceLifetime.Transient);

builder.Services.AddDbContext<RestaurantContext>(options =>
            options.UseSqlite(builder.Configuration.GetConnectionString("restaurant")), ServiceLifetime.Transient);

builder.Services.AddScoped<IDbContextProvider, DbContextProvider>();
builder.Services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.MapControllers();
app.UseHttpsRedirection();

app.Run();

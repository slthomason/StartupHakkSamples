using DBContexts.Contracts;
using DBContexts.DAL;

using Microsoft.EntityFrameworkCore;

namespace DBContexts
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<FirstDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("FirstConnectionString")), ServiceLifetime.Transient);

            builder.Services.AddDbContext<SecondDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("SecondConnectionString")), ServiceLifetime.Transient);

            builder.Services.AddScoped<IDbContextProvider, DbContextProvider>();
            builder.Services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));

            builder.Services.AddControllers();
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

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
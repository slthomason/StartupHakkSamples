using ExampleApi.Filters;
using ExampleBusinessLayer;
using ExampleBusinessLayer.Models;
using ExampleBusinessLayer.Validators;
using FluentValidation;
using FluentValidation.AspNetCore;
using Searchlight;
using SecurityBlanket;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ExampleApi
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(options =>
            {
                options.Filters.Add<SecurityBlanketActionFilter>();
            });
            services.AddValidatorsFromAssemblyContaining<BlogModelValidator>();
            services.AddLogging();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(opt =>
            {
                // Add XML comments from main assembly
                AddXmlDocForAssembly(opt, typeof(Startup));
                AddXmlDocForAssembly(opt, typeof(BusinessLayer));
                opt.OperationFilter<SearchlightSwashbuckleFilter>();
                opt.SchemaFilter<SearchlightSwashbuckleFilter>();
            });
            var engine = new SearchlightEngine().AddAssembly(typeof(BlogModel).Assembly);
            services.AddSingleton(engine);
            services.AddSingleton<IBusinessLayer, BusinessLayer>();
            services.AddSingleton<IModelEntityMapper, ModelEntityMapper>();
            services.AddRazorPages();
            services.AddHttpContextAccessor();
            
            // Add validation logic
            services.AddFluentValidationAutoValidation();
            services.AddTransient<IValidator<BlogModel>, BlogModelValidator>();
            services.AddTransient<IValidator<List<BlogModel>>, BlogModelListValidator>();
            services.AddTransient<IValidator<PostModel>, PostModelValidator>();
        }

        private static void AddXmlDocForAssembly(SwaggerGenOptions opt, Type type)
        {
            var assembly = type.Assembly;
            var basePath = Path.GetDirectoryName(assembly.Location);
            var fileName = assembly.GetName().Name + ".xml";
            opt.IncludeXmlComments(Path.Combine(basePath ?? "", fileName));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Do NOT hide swagger on non-development environments.
            app.UseSwagger(opt => { });
            app.UseSwaggerUI(options => { });
            app.UseHttpsRedirection();
        }
    }
}

using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace WebApplication21.Modules.Swagger
{
    public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
    {
        private readonly IApiVersionDescriptionProvider _provider;
        public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider) => _provider = provider;
        //In C# the tag ---> <inheritdoc/>
        //states that a documentation comment must inherit
        //documentation from a base class or implemented interface.
        /// <inheritdoc />
        public void Configure(SwaggerGenOptions options)
        {
            foreach (var description in _provider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));
            }
        }
        static OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description)
        {
            var info = new OpenApiInfo
            {
                Version = description.ApiVersion.ToString(),
                Title = "Net Core Web Api Versioning",
                Description = "Example on how to versioning a Web Api in Net Core",
                Contact = new OpenApiContact
                {
                    Name = "Hamza Khawaja",
                    Email = "hamzakhawaja@startuphakk.com",
                    Url = new Uri("https://www.linkedin.com/in/hamza-khawaja-473352286/")
                },
                License = new OpenApiLicense
                {
                    Name = "Use under LICX"
                }
            };
            if (description.IsDeprecated)
            {
                info.Description += "This API version has been deprecated.";
            }
            return info;
        }
    }
}

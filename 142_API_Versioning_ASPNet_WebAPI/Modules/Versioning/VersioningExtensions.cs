using Asp.Versioning;

namespace WebApplication21.Modules.Versioning
{
    public static class VersioningExtensions
    {
        public static IServiceCollection AddVersioning(this IServiceCollection services)
        {
            services.AddApiVersioning(o =>
            {
                o.DefaultApiVersion = new ApiVersion(1, 0);
                o.AssumeDefaultVersionWhenUnspecified = true;
                o.ReportApiVersions = true;
                o.ApiVersionReader = new UrlSegmentApiVersionReader();
                                    //new QueryStringApiVersionReader("version");
                                    //new HeaderApiVersionReader("x-version");
            }).AddApiExplorer(options =>
            {
                //semantic versioning
                //first character is the principal or greater version
                //second character is the minor version
                //third character is the patch
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });
            return services;
        }
    }
}

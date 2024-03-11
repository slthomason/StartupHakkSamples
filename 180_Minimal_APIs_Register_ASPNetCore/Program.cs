public interface IEndpoint
{
    void MapEndpoint(IEndpointRouteBuilder app);
}

public class GetFollowerStats : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("users/{userId}/followers/stats", async (
            Guid userId,
            ISender sender) =>
        {
            var query = new GetFollowerStatsQuery(userId);

            Result<FollowerStatsResponse> result = await sender.Send(query);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.Users);
    }
}

public static IServiceCollection AddEndpoints(
    this IServiceCollection services,
    Assembly assembly)
{
    ServiceDescriptor[] serviceDescriptors = assembly
        .DefinedTypes
        .Where(type => type is { IsAbstract: false, IsInterface: false } &&
                       type.IsAssignableTo(typeof(IEndpoint)))
        .Select(type => ServiceDescriptor.Transient(typeof(IEndpoint), type))
        .ToArray();

    services.TryAddEnumerable(serviceDescriptors);

    return services;
}

builder.Services.AddEndpoints(typeof(Program).Assembly);

public static IApplicationBuilder MapEndpoints(
    this WebApplication app,
    RouteGroupBuilder? routeGroupBuilder = null)
{
    IEnumerable<IEndpoint> endpoints = app.Services
        .GetRequiredService<IEnumerable<IEndpoint>>();

    IEndpointRouteBuilder builder =
        routeGroupBuilder is null ? app : routeGroupBuilder;

    foreach (IEndpoint endpoint in endpoints)
    {
        endpoint.MapEndpoint(builder);
    }

    return app;
}

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddEndpoints(typeof(Program).Assembly);

WebApplication app = builder.Build();

ApiVersionSet apiVersionSet = app.NewApiVersionSet()
    .HasApiVersion(new ApiVersion(1))
    .ReportApiVersions()
    .Build();

RouteGroupBuilder versionedGroup = app
    .MapGroup("api/v{version:apiVersion}")
    .WithApiVersionSet(apiVersionSet);

app.MapEndpoints(versionedGroup);

app.Run();
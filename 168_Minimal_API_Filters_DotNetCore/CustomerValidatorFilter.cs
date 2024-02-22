using FluentValidation;
using MinimalAPIWithFilters.Model;

public class CustomerValidatorFilter<T> : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext ctx, EndpointFilterDelegate next)
    {
        var validator = ctx.HttpContext.RequestServices.GetService<IValidator<T>>();
        if (validator is not null)
        {
            var entity = ctx.Arguments
                .OfType<T>()
                .FirstOrDefault(a => a?.GetType() == typeof(T));
            if (entity is not null)
            {
                var results = await validator.ValidateAsync((entity));
                if (!results.IsValid)
                {
                    return Results.ValidationProblem(results.ToDictionary());
                }
            }
            else
            {
                return Results.Problem("Error Not Found");
            }
        }

        return await next(ctx);
    }
}
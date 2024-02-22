using FluentValidation;
using FluentValidation.AspNetCore;
using MinimalAPIWithFilters;
using MinimalAPIWithFilters.Model;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Registering validators from the assembly containing CustomerValidator
builder.Services.AddValidatorsFromAssemblyContaining<CustomerValidator>();

var app = builder.Build();

var customerService = new CustomerService();

app.MapGet("/customer/{id}", (int id) =>
{
    var customer = customerService.GetCustomer(id);
    return customer is not null ? Results.Ok(customer) : Results.NotFound();
});

app.MapPost("/customer", async (Customer customer, IValidator<Customer> validator) =>
{
    var results = await validator.ValidateAsync(customer);

    if (!results.IsValid)
    {
        return Results.ValidationProblem(results.ToDictionary());
    }

    customerService.AddCustomer(customer);
    return Results.Created($"/customer/{customer.Id}", customer);
});

app.MapPut("/customer/{id}", (int id, Customer updatedCustomer) =>
{
    var customer = customerService.GetCustomer(id);
    if (customer is null) return Results.NotFound();

    customerService.UpdateCustomer(id, updatedCustomer);
    return Results.NoContent();
});

app.MapDelete("/customer/{id}", (int id) =>
{
    var customer = customerService.GetCustomer(id);
    if (customer is null) return Results.NotFound();

    customerService.DeleteCustomer(id);
    return Results.Ok();
});

app.MapPost("/customer", (Customer customer, IValidator<Customer> validator) =>
{
    customerService.AddCustomer(customer);
    return Results.Created($"/customer/{customer.Id}", customer);
}).AddEndpointFilter<CustomerValidatorFilter<Customer>>();

app.MapGet("/customer/{id}", (int id) =>
{
    var customer = customerService.GetCustomer(id);
    return customer is not null ? Results.Ok(customer) : Results.NotFound();
}).AddEndpointFilter(async (efiContext, next) =>
    {
        app.Logger.LogInformation("Before first filter");
        var result = await next(efiContext);
        app.Logger.LogInformation("After first filter");
        return result;
    })
    .AddEndpointFilter(async (efiContext, next) =>
    {
        app.Logger.LogInformation(" Before 2nd filter");
        var result = await next(efiContext);
        app.Logger.LogInformation(" After 2nd filter");
        return result;
    });
using Microsoft.Identity.Web;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMicrosoftIdentityWebApiAuthentication(builder.Configuration, "AzureAd");

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("admin", policy => policy.RequireRole("admin"));
    options.AddPolicy("contributor", policy => policy.RequireRole("contributor"));
});

var app = builder.Build();

app.UseAuthentication();

app.MapGet("/", () =>
{
    return "Hello World";
}).RequireAuthorization("contributor");

app.MapPost("/", () =>
{
    return "Hello World";
}).RequireAuthorization("admin");

app.Run();

/*
For Auth URL use: https://login.microsoftonline.com/{YOUR_TENANT_ID}/oauth2/v2.0/authorize

For Access Token URL use: https://login.microsoftonline.com/{YOUR_TENANT_ID}/oauth2/v2.0/token
*/
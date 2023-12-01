builder.Services.AddSwaggerGen(
    c=>c.ExampleFilters()
    );
builder.Services.AddSwaggerExamplesFromAssemblyOf<WeatherForecastExample>();

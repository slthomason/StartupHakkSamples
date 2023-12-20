using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication21.Controllers
{
    /*
    Header versioning:

    [ApiController]
[Route("api/[controller]")]
//Here we defined the version of the API, in this case is set as 1.0 [ApiVersion("1.0")]
    */

    /*
    Query String versioning:
    [ApiController]
//adding the version attribute to the request path
[Route("api/[controller]")]
//here we defined the version of the API, in this case is set as 1.0 [ApiVersion("1.0")]
    */
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    //Here we defined the version of the API, in this case is set as 1.0 
    [ApiVersion("1.0")]
    
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}

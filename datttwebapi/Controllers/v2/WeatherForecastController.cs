using datttwebapi.Data;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace datttwebapi.Controllers.v2
{
    [ApiVersion(2.0)]
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries =
        [
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        ];

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecastV2> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecastV2
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)],
                Duration = 4
            })
            .ToArray();
        }
    }
}

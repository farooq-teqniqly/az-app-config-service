using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace WebAppWithKeyVault.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly IOptions<ApiConfiguration> _configurationOptions;
        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(IOptions<ApiConfiguration> configurationOptions, ILogger<WeatherForecastController> logger)
        {
            _configurationOptions = configurationOptions;
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)],
                Configuration = new ApiConfiguration {Message = _configurationOptions.Value.Message, SecretMessage = _configurationOptions.Value.SecretMessage}
            })
            .ToArray();
        }
    }
}

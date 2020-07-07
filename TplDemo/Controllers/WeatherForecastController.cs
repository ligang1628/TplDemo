using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TplDemo.IRepository;
using TplDemo.IServices;

namespace TplDemo.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    [AllowAnonymous]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly ISystemServices system;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, ISystemServices system)
        {
            _logger = logger;
            this.system = system;
        }

        /// <summary>
        /// get
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return Summaries;
        }

        /// <summary>
        /// getsystem
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<JsonResult> GetSystem()
        {
            return new JsonResult(new { v1 = await system.GetUserModule(), v2 = await system.GetUserModuleVM() });
        }
    }
}

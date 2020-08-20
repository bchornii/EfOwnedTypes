using EfOwnedTypes.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EfOwnedTypes.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly CustomDbContext _context;

        public WeatherForecastController(CustomDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Get()
        {
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();         

            return Ok();
        }
    }
}

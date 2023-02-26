using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Server;
using FerryAppApi.Models;

namespace FerryAppApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PortController : ControllerBase
    {
        
        private readonly ILogger<PortController> _logger;
        private readonly ACE42023Context db;

        public PortController(ACE42023Context db, ILogger<PortController> logger)
        {
            this.db = db;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var result = db.ManasPorts.ToList();
            return Ok(result);
        }
    }
}

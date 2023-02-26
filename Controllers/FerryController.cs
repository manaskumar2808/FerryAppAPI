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
    public class FerryController : ControllerBase
    {
        
        private readonly ILogger<FerryController> _logger;
        private readonly ACE42023Context db;

        public FerryController(ACE42023Context db, ILogger<FerryController> logger)
        {
            this.db = db;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            string origin = HttpContext.Request.Query["origin"];
            string destination = HttpContext.Request.Query["destination"];
            string name = HttpContext.Request.Query["name"];
            string departureStr = HttpContext.Request.Query["departure"];
            string minChargeStr = HttpContext.Request.Query["min_charge"];
            string maxChargeStr = HttpContext.Request.Query["max_charge"];

            var result = db.ManasFerries.OrderBy(ferry => ferry.Departure).ToList();
            if(origin != null && origin.Length > 0) {
                origin = origin.ToLower().Trim();
                result = result.Where(ferry => ferry.Origin.Name.ToLower().Contains(origin)).ToList();
            }
            if(destination != null && destination.Length > 0) {
                destination = destination.ToLower().Trim();
                result = result.Where(ferry => ferry.Destination.Name.ToLower().Contains(destination)).ToList();
            }
            if(name != null && name.Length > 0) {
                name = name.ToLower().Trim();
                result = result.Where(ferry => ferry.Name.ToLower().Contains(name)).ToList();
            }
            if(departureStr != null && departureStr.Length > 0) {
                DateTime departure = DateTime.Parse(departureStr); 
                result = result.Where(ferry => ferry.Departure >= departure).ToList();
            }
            if(minChargeStr != null && minChargeStr.Length > 0) {
                float minCharge = float.Parse(minChargeStr);
                result = result.Where(ferry => ferry.Charge >= minCharge).ToList();
            }
            if(maxChargeStr != null && maxChargeStr.Length > 0) {
                float maxCharge = float.Parse(maxChargeStr);
                result = result.Where(ferry => ferry.Charge <= maxCharge).ToList();
            }
            return Ok(result);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Server;
using FerryAppApi.Models;
using Microsoft.AspNetCore.Authorization;

namespace FerryAppApi.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        
        private readonly ILogger<UserController> _logger;
        private readonly ACE42023Context db;

        public UserController(ACE42023Context db, ILogger<UserController> logger)
        {
            this.db = db;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var result = db.ManasUsers.ToList();
            return Ok(result);
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> Show(int? id) {
            if(id == null)
                return BadRequest();
            var user = await db.ManasUsers.FindAsync(id);
            if(user == null)
                return NotFound();
            return Ok(user);
        }
    }
}
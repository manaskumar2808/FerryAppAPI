using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Server;
using FerryAppApi.Models;
using FerryAppApi.Repository;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using BCrypt.Net;

namespace FerryAppApi.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        
        private readonly ILogger<AuthController> _logger;
        private readonly ACE42023Context db;
        private readonly IJWTManagerRepository jwtManager;

        public AuthController(ACE42023Context db, IJWTManagerRepository jwtManager, ILogger<AuthController> logger)
        {
            this.db = db;
            this.jwtManager = jwtManager;
            _logger = logger;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("Login")]
        public IActionResult Login([Bind(include: "UserName, Password")] ManasUser manasUser) {
            var user = db.ManasUsers.Where(u => u.UserName == manasUser.UserName).SingleOrDefault();
            if(user == null)
                throw new UserNotFoundException();
            
            string password = manasUser.Password;
            string passwordHash = user.Password;

            bool passwordVerification = BCrypt.Net.BCrypt.Verify(password, passwordHash);
            if(!passwordVerification)
                throw new IncorrectPasswordException();

            var token = jwtManager.Authenticate(user);
            if(token == null)
                throw new TokenGenerationException();
            
            return Ok(token);
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("Signup")]
        public async Task<IActionResult> Signup([Bind(include: "UserName, Password, Name, Email")] ManasUser manasUser) {
            if(manasUser.UserName == null || manasUser.UserName.Length == 0)
                throw new InvalidUsernameException();
            if(manasUser.Password == null || manasUser.Password.Length < 7)
                throw new ShortPasswordException();
            
            var user = db.ManasUsers.Where(u => u.UserName == manasUser.UserName).SingleOrDefault();
            if(user != null)
                throw new UsernameTakenException();

            string password = manasUser.Password;
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(password);
            manasUser.Password = passwordHash;

            manasUser.Wallet = 10000;
            await db.ManasUsers.AddAsync(manasUser);
            await db.SaveChangesAsync();

            var token = jwtManager.Authenticate(manasUser);
            if(token == null)
                throw new TokenGenerationException();

            return Ok(token);
        }

        public async Task<IActionResult> Profile() {
            int currentUserId = Convert.ToInt16(ClaimTypes.NameIdentifier);  
            var user = await db.ManasUsers.FindAsync(currentUserId);
            if(user == null)
                return NotFound();
            return Ok(user);  
        }
    }
}
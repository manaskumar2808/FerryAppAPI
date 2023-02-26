using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Server;
using FerryAppApi.Models;
using Microsoft.AspNetCore.Session;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace FerryAppApi.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class TicketController : ControllerBase
    {
        private readonly ILogger<TicketController> _logger;
        private readonly ACE42023Context db;

        public TicketController(ACE42023Context db, ILogger<TicketController> logger) {
            this.db = db;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index() {
            string name = HttpContext.Request.Query["name"];
            string origin = HttpContext.Request.Query["origin"];
            string destination = HttpContext.Request.Query["destination"];
            string ferry = HttpContext.Request.Query["ferry"];
            string departureStr = HttpContext.Request.Query["departure"];
            string minCostStr = HttpContext.Request.Query["min_cost"];
            string maxCostStr = HttpContext.Request.Query["max_cost"];

            int currentUserId = Convert.ToInt16(HttpContext.User.Claims.FirstOrDefault().Value);

            var result = db.ManasTickets.Where(ticket => ticket.UserId == currentUserId).OrderBy(ticket => ticket.Ferry.Departure).ToList();
            if(name != null && name.Length > 0) {
                name = name.ToLower().Trim();
                result = result.Where(ticket => ticket.User.Name.ToLower().Contains(name)).ToList();
            }
            if(origin != null && origin.Length > 0) {
                origin = origin.ToLower().Trim();
                result = result.Where(ticket => ticket.Ferry.Origin.Name.ToLower().Contains(origin)).ToList();
            }
            if(destination != null && destination.Length > 0) {
                destination = destination.ToLower().Trim();
                result = result.Where(ticket => ticket.Ferry.Destination.Name.ToLower().Contains(destination)).ToList();
            }
            if(ferry != null && ferry.Length > 0) {
                ferry = ferry.ToLower().Trim();
                result = result.Where(ticket => ticket.Ferry.Name.ToLower().Contains(ferry)).ToList();
            }
            if(departureStr != null && departureStr.Length > 0) {
                DateTime departure = DateTime.Parse(departureStr); 
                result = result.Where(ticket => ticket.Ferry.Departure >= departure).ToList();
            }
            if(minCostStr != null && minCostStr.Length > 0) {
                float minCost = float.Parse(minCostStr);
                result = result.Where(ticket => ticket.Cost >= minCost).ToList();
            }
            if(maxCostStr != null && maxCostStr.Length > 0) {
                float maxCost = float.Parse(maxCostStr);
                result = result.Where(ticket => ticket.Cost <= maxCost).ToList();
            }
            return Ok(result);
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> Create([Bind(include: "AdultCount, UserId, FerryId")] ManasTicket manasTicket) {
            ManasUser user = await db.ManasUsers.FindAsync(manasTicket.UserId);
            ManasFerry ferry = await db.ManasFerries.FindAsync(manasTicket.FerryId);
            if(user == null)
                throw new UnlinkedUserException();
            if(ferry == null)
                throw new UnlinkedFerryException();

            double chargePerAdult = ferry.Charge;
            manasTicket.Cost = chargePerAdult * manasTicket.AdultCount;
            if(user.Wallet < manasTicket.Cost)
                throw new InsufficientBalanceException();

            int lastRoomNoAvailable = ferry.RoomsLeft;
            manasTicket.RoomNo = lastRoomNoAvailable;
            await db.ManasTickets.AddAsync(manasTicket);
            ferry.RoomsLeft -= 1;
            db.ManasFerries.Update(ferry);
            user.Wallet -= manasTicket.Cost;
            db.ManasUsers.Update(user);
            await db.SaveChangesAsync();

            return Ok(manasTicket);
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> Show(int? id) {
            if(id == null)
                return BadRequest();

            ManasTicket manasTicket = await db.ManasTickets.FindAsync(id);
            if(manasTicket == null)
                return NotFound();

            int currentUserId = Convert.ToInt16(HttpContext.User.Claims.FirstOrDefault().Value);
            if(manasTicket.UserId != currentUserId)
                throw new UnauthorizedUserException();

            return Ok(manasTicket); 
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> Update(int? id, [Bind(include: "AdultCount, UserId, FerryId")] ManasTicket manasTicket) {
            if(id == null)
                return BadRequest();

            ManasTicket ticket = await db.ManasTickets.FindAsync(id);
            if(ticket == null)
                throw new TicketNotFoundException();

            int currentUserId = Convert.ToInt16(HttpContext.User.Claims.FirstOrDefault().Value);
            if(ticket.UserId != currentUserId)
                throw new UnauthorizedUserException();

            ManasFerry ferry = await db.ManasFerries.FindAsync(manasTicket.FerryId);
            ManasUser user = await db.ManasUsers.FindAsync(manasTicket.UserId);
            if(ferry == null)
                throw new UnlinkedFerryException();
            if(user == null)
                throw new UnlinkedUserException();

            int prevAdultCount = ticket.AdultCount;
            int adultCount = manasTicket.AdultCount;
            int countChange = adultCount - prevAdultCount;
            double costChange = countChange * ferry.Charge;

            if(user.Wallet < costChange)
                throw new InsufficientBalanceException();

            ticket.AdultCount = adultCount;
            ticket.Cost = adultCount * ferry.Charge;
            user.Wallet -= costChange;

            db.ManasTickets.Update(ticket);
            db.ManasUsers.Update(user);
            await db.SaveChangesAsync();

            return Ok(ticket);
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete(int? id) {
            if(id == null)
                return BadRequest();

            ManasTicket manasTicket = await db.ManasTickets.FindAsync(id);

            int currentUserId = Convert.ToInt16(HttpContext.User.Claims.FirstOrDefault().Value);
            if(manasTicket.UserId != currentUserId)
                throw new UnauthorizedUserException();

            ManasUser user = await db.ManasUsers.FindAsync(manasTicket.UserId);
            user.Wallet += 0.9 * manasTicket.Cost;
            db.ManasTickets.Remove(manasTicket);
            db.ManasUsers.Update(user);
            await db.SaveChangesAsync();

            return Ok(manasTicket);
        }
    }
}
using Homies.Data;
using Homies.Data.Models;
using Homies.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Security.Claims;

namespace Homies.Controllers
{
    [Authorize]
    public class EventController : Controller
    {
        private readonly HomiesDbContext homies;
        public EventController(HomiesDbContext context)
        {
            homies = context;
        }
        public async Task<IActionResult> All()
        {
            var model = await homies.Events
                .Select(e => new EventInfoViewModel(
                    e.Id,
                    e.Name,
                    e.Start,
                    e.Type.Name,
                    e.Organiser.UserName
                    )).ToListAsync();

            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> Joined()
        {
            var userId = GetUser();

            var events = await homies.EventParticipants
                .Where(ep => ep.HelperId == userId)
                .AsNoTracking()
                .Select(e => new EventInfoViewModel(
                        e.EventId,
                        e.Event.Name,
                        e.Event.Start,
                        e.Event.Type.Name,
                        e.Event.Organiser.UserName
                    ))
                .ToListAsync();
            return View(events);
        }
        [HttpPost]
        public async Task<IActionResult> Join(int id)
        {
            //Get event
            var entity = await homies.Events
                .Where(e => e.Id == id)
                .Include(ep => ep.EventsParticipants)
                .FirstOrDefaultAsync();

            if (entity == null)
            {
                return BadRequest();
            }
            //Getting user
            var userId = GetUser();
            //Check if user is is joined in this event
            if (!entity.EventsParticipants.Any(ep => ep.HelperId == userId))
            {
                entity.EventsParticipants.Add(new EventParticipant()
                {
                    EventId = entity.Id,
                    HelperId = userId
                });
                await homies.SaveChangesAsync();
            }
            return RedirectToAction("Joined", "Event");
        }
        private string GetUser()
        {
            return User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
        }
    }
}

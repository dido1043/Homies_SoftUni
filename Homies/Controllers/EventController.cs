﻿using Homies.Data;
using Homies.Data.Models;
using Homies.Data.ValidationConstants;
using Homies.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
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
        [HttpPost]
        public async Task<IActionResult> Leave(int id)
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
            var userId = GetUser();

            var ep = entity.EventsParticipants.FirstOrDefault(ep => ep.HelperId == userId);

            if (ep == null)
            {
                return BadRequest();
            }
            entity.EventsParticipants.Remove(ep);
            await homies.SaveChangesAsync();
            return RedirectToAction("All", "Event");
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var eventForm = new EventFormViewModel();
            eventForm.Types = await GetTypes();


            return View(eventForm);
        }


        [HttpPost]
        public async Task<IActionResult> Add(EventFormViewModel model)
        {
            DateTime start = DateTime.Now;
            DateTime end = DateTime.Now;
            //Check start and end date format
            if (!DateTime.TryParseExact(
                model.Start,
                ValidationConstants.DateTimeFormat,
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out start))
            {
                ModelState.AddModelError(nameof(model.Start), $"Invalid date: Format must be {ValidationConstants.DateTimeFormat}");
            }
            if (!DateTime.TryParseExact(
                model.End,
                ValidationConstants.DateTimeFormat,
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out end))
            {
                ModelState.AddModelError(nameof(model.End), $"Invalid date: Format must be {ValidationConstants.DateTimeFormat}");
            }
            if (!ModelState.IsValid)
            {
                model.Types = await GetTypes();
                return View(model);
            }
            //creating entity
            var entity = new Event
            {
                Name = model.Name,
                Description = model.Description,
                OrganiserId = GetUser(),
                TypeId = model.TypeId,
                CreatedOn = DateTime.Now,
                Start = start,
                End = end
            };
            //Adding entity
            await homies.Events.AddAsync(entity);
            await homies.SaveChangesAsync();

            return RedirectToAction("All", "Event");
        }

        private string GetUser()
        {
            return User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
        }
        private async Task<IEnumerable<TypeViewModel>> GetTypes()
        {
            return await homies.Types
                .AsNoTracking()
                .Select(t => new TypeViewModel()
                {
                    Id = t.Id,
                    Name = t.Name,
                })
                .ToListAsync();
        }
    }
}

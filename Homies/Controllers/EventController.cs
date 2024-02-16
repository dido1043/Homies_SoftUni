using Homies.Data;
using Homies.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

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
    }
}

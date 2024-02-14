using Homies.Data;
using Homies.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.WebSockets;

namespace Homies.Controllers
{
    public class EventController : Controller
    {
        private readonly HomiesDbContext data;
        public EventController(HomiesDbContext context)
        {
            data = context;
        }
        public async Task<IActionResult> All()
        {
            var entity = await data.Events
                .Select(e => new EventInfoViewModel(
                        e.Id,
                        e.Name,
                        e.Start,
                        e.Type.Name,
                        e.Organiser.UserName

                    )).ToListAsync(); 
                  
            return View(entity);
        }
    }
}

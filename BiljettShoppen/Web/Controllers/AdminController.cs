using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [Route("[controller]/[action]")]
    public class AdminController : Controller
    {
        public async Task<IActionResult> Browse()
        {
            
            return View();
        }

        public async Task<IActionResult> BrowseArena() 
        {
            return View();
        }

        public async Task<IActionResult> CreateArena() 
        {
            return View();
        }

        public async Task<IActionResult> CreateSeatLayout() 
        {
            // TODO: Sida för att skapa sittplatser för arena.
            return View();
        }

        public async Task<IActionResult> Dashboard() 
        {
            return View();
        }

        public async Task<IActionResult> UpdateArena() 
        {
            // TODO: Sida för att uppdatera Arena.
            return View();
        }

        public async Task<IActionResult> UpdateEvent() 
        {
            // TODO: Sida för att uppdatera Event.
            return View();
        }
    }
}

using Application.Features.Arenas;
using Application.Features.Arenas.Create;
using Application.Features.Events.Browse;
using Application.Features.Events.BrowseAll;
using Application.Features.Events.Create;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Models.Enums;

namespace Web.Controllers
{
    [Route("[controller]")]
    [Authorize]
    public class AdminController : Controller
    {
        private readonly IMediator _mediator;

        public AdminController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("Event")]
        [Authorize]
        public async Task<IActionResult> Browse(
            [FromQuery] string? searchWord,
            [FromQuery] EventType? type,
            [FromQuery] bool? isFamilyFriendly,
            [FromQuery] DateOnly? FromDate,
            [FromQuery] DateOnly? ToDate,
            [FromQuery] string? sortBy,
            [FromQuery] bool ascending = true,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 24
        )
        {
            var query = new BrowseAllEventsQuery(
                searchWord,
                type,
                isFamilyFriendly,
                FromDate,
                ToDate,
                sortBy,
                ascending,
                pageNumber,
                pageSize
            );

            var events = await _mediator.Send(query);

            return View(events);
        }

        [HttpGet("Arena")]
        [Authorize]
        public async Task<IActionResult> BrowseArena() 
        {
            var query = new ListArenasQuery();
            var arenas = await _mediator.Send(query);
            ViewBag.Arenas = arenas.Select(a => new SelectListItem(a.Name, a.Id.ToString()));

            return View(arenas);
        }

        [HttpGet("Create/Arena")]
        [Authorize]
        public async Task<IActionResult> CreateArena() 
        {
            return View();
        }

        [HttpGet("Create/Event")]
        [Authorize]
        public async Task<IActionResult> CreateEvent()
        {
            var query = new ListArenasQuery();
            var arenas = await _mediator.Send(query);
            ViewBag.Arenas = arenas.Select(a => new SelectListItem(a.Name, a.Id.ToString()));

            // Listan ska vara tom förts, denna fylls på senare beroende på vald arena.
            ViewBag.SeatLayouts = new List<SelectListItem>();

            return View();
        }

        [HttpGet("Create/SeatLayout")]
        [Authorize]
        public async Task<IActionResult> CreateSeatLayout() 
        {
            // TODO: Sida för att skapa sittplatser för arena.
            return View();
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Dashboard() 
        {
            return View();
        }

        [HttpGet("Arena/{arenaId:int}")]
        [Authorize]
        public async Task<IActionResult> UpdateArena(int arenaId) 
        {
            // TODO: Skriv ut befintlig data om arenan.
            return View();
        }

        [HttpGet("Event/{eventId:int}")]
        [Authorize]
        public async Task<IActionResult> UpdateEvent(int eventId) 
        {
            // TODO: Skriv ut befintlig data om eventet.
            return View();
        }

        [HttpPost("Create/Event")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateEvent(CreateEventCommand command)
        {
            var created = await _mediator.Send(command);
            return RedirectToAction("Browse");
        }

        [HttpPost("Create/Arena")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateArenaCommand command)
        {
            var created = await _mediator.Send(command);
            return RedirectToAction("Browse");
        }

        [HttpPut("Update/Arena")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateArena() 
        {
            // TODO: Lägg till handler.
            return RedirectToAction("Browse");
        }


        [HttpPut("Update/Event")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateEvent() 
        {
            // TODO: Lägg till handler.
            return RedirectToAction("Browse");
        }
    }
}

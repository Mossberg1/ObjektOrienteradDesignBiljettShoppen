using Application.Features.Arenas;
using Application.Features.Arenas.Create;
using Application.Features.Arenas.GetArenaById;
using Application.Features.Arenas.Update;
using Application.Features.Events.BrowseAll;
using Application.Features.Events.Create;
using Application.Features.Events.GetById;
using Application.Features.Events.Update;
using Application.Features.SeatLayouts;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Models.Entities;
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
        public async Task<IActionResult> CreateSeatLayout([FromQuery] int? arenaId)
        {
            var query = new ListArenasQuery();
            var arenas = await _mediator.Send(query);

            ViewBag.Arenas = arenas.Select(a => new SelectListItem(a.Name, a.Id.ToString())).ToList();

            if (arenaId.HasValue)
            {
                var arena = arenas.FirstOrDefault(a => a.Id == arenaId.Value);
                if (arena != null)
                {
                    ViewBag.SelectedArena = arena;
                    ViewBag.Entrances = arena.EntrancesNavigation
                        .Select(e => new SelectListItem(e.Name, e.Id.ToString()))
                        .ToList();
                }
            }

            return View();
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Dashboard()
        {
            return View();
        }

        [HttpGet("Update/Arena/{arenaId:int}")]
        [Authorize]
        public async Task<IActionResult> UpdateArena(int arenaId)
        {
            var query = new GetArenaByIdQuery(arenaId);
            var arena = await _mediator.Send(query);

            if (arena == null)
            {
                TempData["ErrorMessage"] = $"Ingen arena med {arenaId} hittades.";
                return RedirectToAction("BrowseArena");
            }

            return View(arena);
        }

        [HttpGet("Update/Event/{eventId:int}")]
        [Authorize]
        public async Task<IActionResult> UpdateEvent(int eventId)
        {
            var query = new GetEventByIdQuery(eventId);
            var eventEntity = await _mediator.Send(query);
            if (eventEntity == null)
            {
                TempData["ErrorMessage"] = $"Inget event med {eventId} hittades.";
                return RedirectToAction("Browse");
            }

            return View(eventEntity);
        }

        [HttpPost("Create/Arena")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateArena(CreateArenaCommand command)
        {
            var created = await _mediator.Send(command);
            return RedirectToAction("BrowseArena");
        }

        [HttpPost("Create/Event")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateEvent(CreateEventCommand command)
        {
            var created = await _mediator.Send(command);
            return RedirectToAction("Browse");
        }

        [HttpPost("Create/SeatLayout")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateSeatLayout([FromBody] CreateSeatLayoutCommand command)
        {
            var layoutId = await _mediator.Send(command);
            return Ok(new { success = true, layoutId });
        }

        [HttpPost("Update/Arena/{arenaId:int}")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateArena([FromRoute] int arenaId, [FromForm] UpdateArenaCommand command)
        {
            if (arenaId != command.Id)
            {
                TempData["ErrorMessage"] = "Någonting gick fel. Försök igen";
                return RedirectToAction("BrowseArena");
            }

            var result = await _mediator.Send(command);
            if (result == null)
            {
                TempData["ErrorMessage"] = $"Ingen arena med {arenaId} hittades.";
                return RedirectToAction("BrowseArena");
            }

            return RedirectToAction("BrowseArena");
        }


        [HttpPost("Update/Event/{eventId:int}")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateEvent([FromRoute] int eventId, [FromForm] UpdateEventCommand command)
        {
            if (eventId != command.Id)
            {
                TempData["ErrorMessage"] = "Någonting gick fel. Försök igen";
                return RedirectToAction("Browse");
            }

            var result = await _mediator.Send(command);
            if (result == null)
            {
                TempData["ErrorMessage"] = $"Inget event med {eventId} hittades.";
                return RedirectToAction("Browse");
            }

            return RedirectToAction("Browse");
        }
    }
}

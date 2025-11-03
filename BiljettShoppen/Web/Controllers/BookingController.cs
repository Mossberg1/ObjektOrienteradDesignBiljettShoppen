using Application.Features.Bookings.Cancle;
using Application.Features.Bookings.Delete;
using Application.Features.Bookings.Search;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [Route("[controller]")]
    public class BookingController : Controller
    {
        private readonly IMediator _mediator;

        public BookingController(IMediator mediator) 
        {
            _mediator = mediator;
        }

        [HttpGet("Search")]
        public async Task<IActionResult> Search([FromQuery] string? referenceNumber) 
        {
            if (string.IsNullOrEmpty(referenceNumber))
            {
                TempData["ErrorMessage"] = "Inget referensnummer anget.";
                return RedirectToAction("Index", "Home");
            }

            var query = new GetBookingByReferenceQuery(referenceNumber);
            var booking = await _mediator.Send(query);

            if (booking == null)
            {
                TempData["ErrorMessage"] = $"Ingen bokning hittades med referensnummer: {referenceNumber}";
                return RedirectToAction("Index", "Home");
            }

            return View("Details", booking);
        }

        [HttpGet("Details/{referenceNumber}")]
        public async Task<IActionResult> Details([FromRoute] string referenceNumber) 
        {
            if (string.IsNullOrEmpty(referenceNumber))
            {
                TempData["ErrorMessage"] = "Inget referensnummer anget.";
                return RedirectToAction("Index", "Home");
            }

            var query = new GetBookingByReferenceQuery(referenceNumber);
            var booking = await _mediator.Send(query);

            if (booking == null)
            {
                TempData["ErrorMessage"] = $"Ingen bokning hittades med referensnummret: {referenceNumber}";
                return RedirectToAction("Index", "Home");
            }

            return View(booking);
        }

        [HttpPost("Cancle")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Remove([FromForm] string referenceNumber)
        {
            if (string.IsNullOrEmpty(referenceNumber))
            {
                TempData["ErrorMessage"] = "Inget referensnummer angivet.";
                return RedirectToAction("Index", "Home");
            }

            var command = new DeleteBookingCommand(referenceNumber);
            var result = await _mediator.Send(command);

            if (!result)
            {
                TempData["ErrorMessage"] = $"Avbokning misslyckades";
                return RedirectToAction("Details", new { referenceNumber });
            }

            TempData["SuccessMessage"] = "Bokningen har avbokats.";
            return RedirectToAction("Index", "Home");
        }
    }
}

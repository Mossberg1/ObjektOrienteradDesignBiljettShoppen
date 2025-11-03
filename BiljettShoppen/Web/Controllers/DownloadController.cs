using Application.Features.Bookings.GetBookingConfirmationFileName;
using Application.Features.Bookings.GetBookingInvoiceFileName;
using Application.Features.Bookings.Search;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [Route("[controller]")]
    public class DownloadController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IWebHostEnvironment _env;

        public DownloadController(IMediator mediator, IWebHostEnvironment env) 
        {
            _mediator = mediator;
            _env = env;
        }

        [HttpGet("BookingConfirmation/{referenceNumber}")]
        public async Task<IActionResult> DownloadBookingConfirmation([FromRoute] string referenceNumber) 
        {
            var query = new GetBookingConfirmationFileNameQuery(referenceNumber);
            var filename = await _mediator.Send(query);

            if (string.IsNullOrEmpty(filename))
            { 
                TempData["ErrorMessage"] = "Ingen bokningsbekräftelse hittades för den angivna bokningen.";
                return RedirectToAction("Index", "Home");
            }

            var filepath = Path.Combine(_env.WebRootPath, "..", filename.TrimStart('/'));

            if (!System.IO.File.Exists(filepath))
            {
                TempData["ErrorMessage"] = "Filen för bokningsbekräftelsen kunde inte hittas.";
                return RedirectToAction("Index", "Home");
            }

            return File(System.IO.File.ReadAllBytes(filepath), "application/pdf", $"Bekräftelse_{referenceNumber}.pdf");
        }

        [HttpGet("BookingInvoice/{referenceNumber}")]
        public async Task<IActionResult> DownloadBookingInvoice([FromRoute] string referenceNumber) 
        {
            var query = new GetBookingInvoiceFileNameQuery(referenceNumber);
            var filename = await _mediator.Send(query);

            if (string.IsNullOrEmpty(filename))
            {
                TempData["ErrorMessage"] = "Ingen faktura hittades för den angivna bokningen.";
                return RedirectToAction("Index", "Home");
            }

            var filepath = Path.Combine(_env.WebRootPath, "..", filename.TrimStart('/'));

            if (!System.IO.File.Exists(filepath))
            {
                TempData["ErrorMessage"] = "Filen för fakturan kunde inte hittas.";
                return RedirectToAction("Index", "Home");
            }

            return File(System.IO.File.ReadAllBytes(filepath), "application/pdf", $"Faktura_{referenceNumber}.pdf");
        }
    }
}

using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Models.Entities;
using Microsoft.AspNetCore.Authorization;
using MediatR;
using Application.Features.Bookings.Delete;

namespace Web.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BookingController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpDelete("cancle/{ReferenceNumber}")]
        public async Task<IActionResult> CancleBooking(string ReferenceNumber, CancellationToken cancellationToken)
        {

            var result = await _mediator.Send(new CancleBookingCommand(ReferenceNumber), cancellationToken);

            if (!result)
                return Ok(new { message = $"Ingen bokning med referensnummer: {ReferenceNumber} hittades" });

            return Ok(new { message = $"Bokning med referensnummer: {ReferenceNumber} har tagits bort." });
        }
    }
}


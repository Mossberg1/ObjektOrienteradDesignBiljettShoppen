using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Models.Entities;
using Microsoft.AspNetCore.Authorization;
using MediatR;
using Application.Features.Bookings.Cancle;

namespace Web.Controllers.Api
{
    [ApiController]
    [Route("api/booking")]
    public class ApiBookingController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ApiBookingController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpDelete("cancle/{ReferenceNumber}")]
        public async Task<IActionResult> CancleBooking(string ReferenceNumber, CancellationToken cancellationToken)
        {

            var result = await _mediator.Send(new CancelBookingCommand(ReferenceNumber), cancellationToken);

            if (!result)
                return Ok(new { message = $"Ingen bokning med referensnummer: {ReferenceNumber} hittades" });

            return Ok(new { message = $"Bokning med referensnummer: {ReferenceNumber} har tagits bort." });
        }
    }
}


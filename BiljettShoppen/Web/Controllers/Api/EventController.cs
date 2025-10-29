using Application.Features.Events.Delete;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventController : ControllerBase
    {
        private readonly IMediator _mediator;

        public EventController(IMediator mediator) 
        {
            _mediator = mediator;
        }

        [Authorize]
        [HttpDelete("{eventId:int}")]
        public async Task<IActionResult> DeleteEvent([FromRoute] int eventId) 
        {
            var command = new DeleteEventCommand(eventId);
            var result = await _mediator.Send(command);
            if (result)
                return NoContent();

            return NotFound();
        }
    }
}

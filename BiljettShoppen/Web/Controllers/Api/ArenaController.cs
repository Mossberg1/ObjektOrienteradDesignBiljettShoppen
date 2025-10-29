using Application.Features.Arenas.Delete;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class ArenaController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ArenaController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpDelete("{arenaId:int}")]
        [Authorize]
        public async Task<IActionResult> DeleteArena([FromRoute] int arenaId)
        {
            var command = new DeleteArenaCommand(arenaId);
            var result = await _mediator.Send(command);
            if (result)
                return NoContent();

            return NotFound();
        }
    }
}

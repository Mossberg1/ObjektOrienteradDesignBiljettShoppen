using Application.Features.Arenas.Delete;
using Application.Features.Arenas.GetArenaById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers.Api
{
    [ApiController]
    [Route("api/arena")]
    public class ApiArenaController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ApiArenaController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{arenaId:int}/entrances")]
        public async Task<IActionResult> GetEntrances([FromRoute] int arenaId) 
        {
            var query = new GetArenaByIdQuery(arenaId);
            var arena = await _mediator.Send(query);

            if (arena == null)
                return NotFound();

            var entrances = arena.EntrancesNavigation.Select(e => new
            {
                e.Id, e.Name
            });

            return Ok(entrances);
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

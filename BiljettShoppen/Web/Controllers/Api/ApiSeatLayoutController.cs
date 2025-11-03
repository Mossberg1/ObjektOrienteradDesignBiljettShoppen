using Application.Features.Arenas.GetSeatLayoutsForArena;
using DataAccess;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers.Api
{
    [Route("api/seatlayout")]
    [ApiController]
    public class ApiSeatLayoutController : ControllerBase
    {
        private readonly IMediator _mediator;

        private readonly ApplicationDbContext _dbContext; // TODO: Byt ut mot handlers sen bara för testning.

        public ApiSeatLayoutController(IMediator mediator, ApplicationDbContext dbContext)
        {
            _mediator = mediator;
            _dbContext = dbContext;
        }

        [HttpGet("ForArena/{arenaId:int}")]
        public async Task<IActionResult> ListSeatLayoutsForArena([FromRoute] int arenaId)
        {
            var query = new GetSeatLayoutForArenaIdQuery(arenaId);
            var layouts = await _mediator.Send(query);

            return Ok(layouts);
        }
    }
}

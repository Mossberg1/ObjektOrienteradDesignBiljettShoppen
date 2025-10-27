using DataAccess;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.Entities;

namespace Web.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeatLayoutController : ControllerBase
    {
        private readonly IMediator _mediator;

        private readonly ApplicationDbContext _dbContext; // TODO: Byt ut mot handlers sen bara för testning.

        public SeatLayoutController(IMediator mediator, ApplicationDbContext dbContext) 
        {
            _mediator = mediator;
            _dbContext = dbContext;
        }

        [HttpGet("ForArena/{arenaId:int}")]
        public async Task<IActionResult> ListSeatLayoutsForArena([FromRoute] int arenaId) 
        {
            // TODO: Byt ut mot handler.
            var layouts = await _dbContext.SeatLayouts.AsNoTracking()
                .Where(sl => sl.ArenaId == arenaId)
                .ToListAsync();

            return Ok(layouts);
        }
    }
}

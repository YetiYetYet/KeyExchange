using API.Db;
using API.DTO.Game;
using API.Models;
using API.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers.Game
{
    [Route("[controller]")]
    [ApiController]
    public class PublicGameController : ControllerBase
    {
        private readonly ContextApi _context;
        public PublicGameController(ContextApi context)
        {
            _context = context;
        }
        
        [AllowAnonymous]
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<GameDto>>> GetAllGames()
        {
            var gameList = await _context.Games.Where(game => !game.SoftDeleted).Include(game => game.User).ToListAsync();
            var gameDtos = AutoMapperUtils.TupleAutoMapper<Models.Game, GameDto>(gameList, new List<(Type, Type)>() { (typeof(Models.User), typeof(UserGameDto)) });
            return Ok(gameDtos);
        }
        
        [HttpGet("all-available")]
        public async Task<ActionResult<IEnumerable<GameDto>>> GetAllAvailableGames()
        {
            var gameList = await _context.Games.Where(game => !game.SoftDeleted && game.IsAvailable).Include(game => game.User).ToListAsync();
            var gameDtos = AutoMapperUtils.TupleAutoMapper<Models.Game, GameDto>(gameList, new List<(Type, Type)>() { (typeof(Models.User), typeof(UserGameDto)) });
            return Ok(gameDtos);
        }
        
        [HttpGet("all-by-user-{id:guid}")]
        public async Task<ActionResult<IEnumerable<GameDto>>> GetAllGamesOfUserId(Guid id)
        {
            Models.User? user = await _context.ApplicationUsers.Where(user => user.Id == id).FirstOrDefaultAsync();
            if (user == null)
            {
                return NotFound("This user didn't exist");
            }
            if (!user.IsPublic || user.SoftDeleted)
            {
                return Forbid("Forbidden");
            }
            var gameList = await _context.Games.Where(game => !game.SoftDeleted && game.IsAvailable && game.UserId == id).Include(game => game.User).ToListAsync();
            var gameDtos = AutoMapperUtils.TupleAutoMapper<Models.Game, GameDto>(gameList, new List<(Type, Type)>() { (typeof(Models.User), typeof(UserGameDto)) });
            return Ok(gameDtos);
        }
    }
}
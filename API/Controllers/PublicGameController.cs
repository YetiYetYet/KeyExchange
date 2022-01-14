using API.Db;
using API.DTO;
using API.Models;
using API.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
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
        
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<GameDto>>> GetAllGames()
        {
            var gameList = await _context.Games.Where(game => !game.SoftDeleted).Include(game => game.User).ToListAsync();
            var gameDtos = AutoMapperUtils.TupleAutoMapper<Game, GameDto>(gameList, new List<(Type, Type)>() { (typeof(User), typeof(UserGameDto)) });
            return Ok(gameDtos);
        }
    }
}
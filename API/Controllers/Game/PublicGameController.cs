using API.Db;
using API.DTO.GameDto;
using API.Service.User;
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
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IUserService _userService;
        public PublicGameController(ApplicationDbContext applicationDbContext, IUserService userService)
        {
            _applicationDbContext = applicationDbContext;
            _userService = userService;
        }
        
        [AllowAnonymous]
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<GameDto>>> GetAllGames()
        {
            var gameList = await _applicationDbContext.Games.Where(game => !game.SoftDeleted).Include(game => game.User).ToListAsync();
            var gameDtos = AutoMapperUtils.TupleAutoMapper<Models.Game, GameDto>(gameList, new List<(Type, Type)>() { (typeof(Models.User), typeof(UserGameDto)) });
            return Ok(gameDtos);
        }
        
        [AllowAnonymous]
        [HttpGet("by-id-{id:int}")]
        public async Task<ActionResult<GameDto>> GetGameById(int id)
        {
            Models.Game? game = await _applicationDbContext.Games.Where(game => !game.SoftDeleted && game.Id == id).Include(game => game.User).FirstOrDefaultAsync();
            if (game == null)
                return NotFound("This game didn't exist");
            GameDto gameDto = AutoMapperUtils.TupleAutoMapper<Models.Game, GameDto>(game, new List<(Type, Type)>() { (typeof(Models.User), typeof(UserGameDto)) });
            return Ok(gameDto);
        }
        
        [HttpGet("all-available")]
        public async Task<ActionResult<IEnumerable<GameDto>>> GetAllAvailableGames()
        {
            var gameList = await _applicationDbContext.Games.Where(game => !game.SoftDeleted && game.IsAvailable).Include(game => game.User).ToListAsync();
            var gameDtos = AutoMapperUtils.TupleAutoMapper<Models.Game, GameDto>(gameList, new List<(Type, Type)>() { (typeof(Models.User), typeof(UserGameDto)) });
            return Ok(gameDtos);
        }
        
        [HttpGet("all-by-user-{id:int}")]
        public async Task<ActionResult<IEnumerable<GameDto>>> GetAllGamesOfUserId(int id)
        {
            Models.User? user = await _applicationDbContext.Users.Where(user => user.Id == id).FirstOrDefaultAsync();
            if (user == null)
            {
                return NotFound("This user didn't exist");
            }
            if (!user.IsPublic || user.SoftDeleted)
            {
                return Forbid("Forbidden");
            }
            var gameList = await _applicationDbContext.Games.Where(game => !game.SoftDeleted && game.IsAvailable && game.UserId == id).Include(game => game.User).ToListAsync();
            var gameDtos = AutoMapperUtils.TupleAutoMapper<Models.Game, GameDto>(gameList, new List<(Type, Type)>() { (typeof(Models.User), typeof(UserGameDto)) });
            return Ok(gameDtos);
        }
    }
}
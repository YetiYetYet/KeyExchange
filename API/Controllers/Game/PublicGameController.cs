using System.Linq.Expressions;
using API.Db;
using API.DTO.GameDto;
using API.Models;
using API.Repository;
using API.Service.User;
using API.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace API.Controllers.Game
{
    [Route("[controller]")]
    [ApiController]
    public class PublicGameController : ControllerBase
    {
        private readonly IUserService _userService;
        private ApplicationDbContext _applicationDbContext;
        private RepositoryAsync _repositoryAsync;
        public PublicGameController(ApplicationDbContext applicationDbContext, IUserService userService, IStringLocalizer<RepositoryAsync> localizer)
        {
            _applicationDbContext = applicationDbContext;
            _userService = userService;
            _repositoryAsync = new RepositoryAsync(_applicationDbContext, localizer);
        }
        
        [AllowAnonymous]
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<GameDto>>> GetAllGames()
        {
            // //var gameList = await _repositoryAsync.GetListAsync<Models.Game>(game => game.IsAvailable, includes: );
            // // var gameList = await _applicationDbContext.Games.Where(game => !game.SoftDeleted).Include(game => game.ApplicationUser).ToListAsync();
            // var gameDtos = AutoMapperUtils.TupleAutoMapper<Models.Game, GameDto>(gameList, new List<(Type, Type)>() { (typeof(Models.ApplicationUser), typeof(UserGameDto)) });
            // return Ok(gameDtos);
            return Ok();
        }
        
        [AllowAnonymous]
        [HttpGet("by-id-{id:int}")]
        public async Task<ActionResult<GameDto>> GetGameById(int id)
        {
            // Models.Game? game = await _applicationDbContext.Games.Where(game => !game.SoftDeleted && game.Id == id).Include(game => game.ApplicationUser).FirstOrDefaultAsync();
            // if (game == null)
            //     return NotFound("This game didn't exist");
            // GameDto gameDto = AutoMapperUtils.TupleAutoMapper<Models.Game, GameDto>(game, new List<(Type, Type)>() { (typeof(Models.ApplicationUser), typeof(UserGameDto)) });
            // return Ok(gameDto);
            return Ok();
        }
        
        [HttpGet("all-available")]
        public async Task<ActionResult<IEnumerable<GameDto>>> GetAllAvailableGames()
        {
            // var gameList = await _applicationDbContext.Games.Where(game => !game.SoftDeleted && game.IsAvailable).Include(game => game.ApplicationUser).ToListAsync();
            // var gameDtos = AutoMapperUtils.TupleAutoMapper<Models.Game, GameDto>(gameList, new List<(Type, Type)>() { (typeof(Models.ApplicationUser), typeof(UserGameDto)) });
            // return Ok(gameDtos);
            return Ok();
        }
        
        [HttpGet("all-by-user-{id:int}")]
        public async Task<ActionResult<IEnumerable<GameDto>>> GetAllGamesOfUserId(int id)
        {
            // Models.ApplicationUser? user = await _applicationDbContext.Users.Where(user => user.Id == id).FirstOrDefaultAsync();
            // if (user == null)
            // {
            //     return NotFound("This user didn't exist");
            // }
            // if (!user.IsPublic || user.SoftDeleted)
            // {
            //     return Forbid("Forbidden");
            // }
            // var gameList = await _applicationDbContext.Games.Where(game => !game.SoftDeleted && game.IsAvailable && game.UserId == id).Include(game => game.ApplicationUser).ToListAsync();
            // var gameDtos = AutoMapperUtils.TupleAutoMapper<Models.Game, GameDto>(gameList, new List<(Type, Type)>() { (typeof(Models.ApplicationUser), typeof(UserGameDto)) });
            // return Ok(gameDtos);
            return Ok();
        }
    }
}
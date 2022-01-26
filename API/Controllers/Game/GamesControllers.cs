// using System.Security.Claims;
// using API.Db;
// using API.Models;
// using API.DTO.GameDto;
// using API.Service.GoogleApi;
// using API.Service.ApplicationUser;
// using API.Utils;
// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.EntityFrameworkCore;
//
// namespace API.Controllers.Game;
//
// [Route("[controller]")]
// [ApiController]
// [Authorize]
// public class GamesControllers : ControllerBase
// {
//     private readonly ApplicationDbContext _applicationDbContext;
//     private IGoogleSearchService _googleSearchService;
//     private IUserService _userService;
//
//     public GamesControllers(ApplicationDbContext applicationDbContext, IGoogleSearchService googleSearchService, IUserService userService)
//     {
//         _applicationDbContext = applicationDbContext;
//         _googleSearchService = googleSearchService;
//         _userService = userService;
//     }
//
//     // GET: api/GamesControllers
//     [HttpGet("admin"), Authorize(Roles = "root")]
//     public async Task<ActionResult<IEnumerable<Models.Game>>> GetGamesAdmin()
//     {
//         return await _applicationDbContext.Games.ToListAsync();
//     }
//
//     // GET: api/GamesControllers/5
//     [AllowAnonymous]
//     [HttpGet("{id}")]
//     public async Task<ActionResult<Models.Game>> GetGame(Guid id)
//     {
//         var game = await _applicationDbContext.Games.FindAsync(id);
//         
//         if (game == null)
//         {
//             return NotFound();
//         }
//         
//         return game;
//     }
//
//     // PUT: api/GamesControllers/5
//     // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
//     [HttpPut("{id:int}")]
//     public async Task<IActionResult> PutGame(int id, GameDto gameDto)
//     {
//         var game = await _applicationDbContext.Games.Where(g => g.Id == id && !g.SoftDeleted).FirstOrDefaultAsync();
//         if (id != game.Id)
//         {
//             return BadRequest("This object is not the reference of this game");
//         }
//         Console.WriteLine("Before Auto Mapper :" + JsonUtils.ConstructJson(game));
//         game = AutoMapperUtils.BasicAutoMapper<GameDto, Models.Game>(gameDto);
//         Console.WriteLine("After Auto Mapper :" + JsonUtils.ConstructJson(game));
//         _applicationDbContext.Entry(game).State = EntityState.Modified;
//
//         try
//         {
//             await _applicationDbContext.SaveChangesAsync();
//         }
//         catch (DbUpdateConcurrencyException e)
//         {
//             if (!GameExists(id))
//             {
//                 return NotFound($"Object {id.ToString()} Not Found");
//             }
//
//             throw new DbUpdateException(e.Message, e.InnerException);
//         }
//
//         return Ok();
//     }
//
//     // POST: api/GamesControllers
//     // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
//     [HttpPost]
//     public async Task<ActionResult<GameDto>> PostGame(PostGameDto gameDto)
//     {
//         Models.Game game = AutoMapperUtils.BasicAutoMapper<PostGameDto, Models.Game>(gameDto);
//         Models.Game gameInserted = _applicationDbContext.Games.Add(game).Entity;
//         await _applicationDbContext.SaveChangesAsync();
//         Console.WriteLine("game inserted : \n" + JsonUtils.ConstructJson(gameInserted));
//         Console.WriteLine("game base : \n" + JsonUtils.ConstructJson(game));
//         return Ok();
//         return CreatedAtAction("GetGame", new { id = game.Id }, game);
//     }
//     
//     // DELETE: api/GamesControllers/5
//     [HttpDelete("{id:guid}"), Authorize]
//     public async Task<IActionResult> SoftDeleteGame(Guid id)
//     {
//         Models.Game? game = await _applicationDbContext.Games.FindAsync(id);
//         if (game == null)
//         {
//             return NotFound();
//         }
//         game.SoftDeleted = true;
//         await _applicationDbContext.SaveChangesAsync();
//
//         return NoContent();
//     }
//
//     // DELETE: api/GamesControllers/5
//     [HttpDelete("hard-delete-{id:guid}"), Authorize(Roles = "root")]
//     public async Task<IActionResult> DeleteGame(Guid id)
//     {
//         Models.Game? game = await _applicationDbContext.Games.FindAsync(id);
//         if (game == null)
//         {
//             return NotFound();
//         }
//         _applicationDbContext.Games.Remove(game);
//         await _applicationDbContext.SaveChangesAsync();
//
//         return NoContent();
//     }
//
//
//     //GET: api/GamesControllers
//     [HttpGet("get-a-steam-link"), Authorize(Roles = "root")]
//     public async Task<ActionResult<IEnumerable<Models.Game>>> GetASteamLink()
//     {
//         var steamGameList = await _applicationDbContext.Games.Where(game => game.Platforme == "Steam" && (game.Link == null || game.Link == "No Link found")).Take(1).ToListAsync();
//         foreach (Models.Game game in steamGameList)
//         {
//             Console.WriteLine($"{game.Name}");
//             PlatformInfoDto platformGameInfoDto = await _googleSearchService.GetSteamInfo(game.Name);
//             game.Title = platformGameInfoDto.Title;
//             game.Link = platformGameInfoDto.Link;
//             game.Description = platformGameInfoDto.Description;
//             game.Price = platformGameInfoDto.Price;
//             game.Reviews = platformGameInfoDto.Reviews;
//             game.TumbnailUrl = platformGameInfoDto.TumbnailUrl;
//
//             game.GeneratedInfo = true;
//             await _applicationDbContext.SaveChangesAsync();
//         }
//         return Ok(steamGameList);
//     }
//     
//     //GET: api/GamesControllers
//     [HttpGet("get-a-blizzard-link"), Authorize(Roles = "root")]
//     public async Task<ActionResult<IEnumerable<Models.Game>>> GetABlizzardLink()
//     {
//         var steamGameList = await _applicationDbContext.Games.Where(game => game.Platforme == "Blizzard" && (game.Link == null || game.Link == "No Link found")).Take(1).ToListAsync();
//         foreach (Models.Game game in steamGameList)
//         {
//             Console.WriteLine($"{game.Name}");
//             PlatformInfoDto platformGameInfoDto = await _googleSearchService.GetSteamInfo(game.Name);
//             game.Title = platformGameInfoDto.Title;
//             game.Link = platformGameInfoDto.Link;
//             game.Description = platformGameInfoDto.Description;
//             game.Price = platformGameInfoDto.Price;
//             game.Reviews = platformGameInfoDto.Reviews;
//             game.TumbnailUrl = platformGameInfoDto.TumbnailUrl;
//
//             game.GeneratedInfo = true;
//             await _applicationDbContext.SaveChangesAsync();
//         }
//         return Ok(steamGameList);
//     }
//
//     private bool GameExists(int id)
//     {
//         return _applicationDbContext.Games.Any(e => e.Id == id);
//     }
// }
using System.Security.Claims;
using API.Db;
using API.DTO.Game;
using API.Service.GoogleApi;
using API.Service.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers.Game;

[Route("[controller]")]
[ApiController]
[Authorize]
public class GamesControllers : ControllerBase
{
    private readonly ContextApi _context;
    private IGoogleSearchService _googleSearchService;
    private IUserService _userService;

    public GamesControllers(ContextApi context, IGoogleSearchService googleSearchService, IUserService userService)
    {
        _context = context;
        _googleSearchService = googleSearchService;
        _userService = userService;
    }

    // GET: api/GamesControllers
    [HttpGet("admin"), Authorize(Roles = "root")]
    public async Task<ActionResult<IEnumerable<Models.Game>>> GetGamesAdmin()
    {
        return await _context.Games.ToListAsync();
    }

    // GET: api/GamesControllers/5
    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<ActionResult<Models.Game>> GetGame(Guid id)
    {
        var game = await _context.Games.FindAsync(id);

        if (game == null)
        {
            return NotFound();
        }

        return game;
    }

    // PUT: api/GamesControllers/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id:guid}"), Authorize(Roles = "root")]
    public async Task<IActionResult> PutGame(Guid id, Models.Game game)
    {
        if (id != game.Id)
        {
            return BadRequest("This object is not the reference of this game");
        }

        _context.Entry(game).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException e)
        {
            if (!GameExists(id))
            {
                return NotFound($"Object {id.ToString()} Not Found");
            }

            throw new DbUpdateException(e.Message, e.InnerException);
        }

        return NoContent();
    }

    // POST: api/GamesControllers
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost, Authorize(Roles = "root")]
    public async Task<ActionResult<Models.Game>> PostGame(Models.Game game)
    {
        _context.Games.Add(game);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetGame", new { id = game.Id }, game);
    }

    // DELETE: api/GamesControllers/5
    [HttpDelete("{id}"), Authorize(Roles = "root")]
    public async Task<IActionResult> DeleteGame(Guid id)
    {
        var game = await _context.Games.FindAsync(id);
        if (game == null)
        {
            return NotFound();
        }

        _context.Games.Remove(game);
        await _context.SaveChangesAsync();

        return NoContent();
    }


    //GET: api/GamesControllers
    [HttpGet("get-a-steam-link"), Authorize(Roles = "root")]
    public async Task<ActionResult<IEnumerable<Models.Game>>> GetASteamLink()
    {
        var steamGameList = await _context.Games.Where(game => game.Platforme == "Steam" && (game.Link == null || game.Link == "No Link found")).Take(1).ToListAsync();
        foreach (Models.Game game in steamGameList)
        {
            Console.WriteLine($"{game.Name}");
            PlatformInfoDto platformGameInfoDto = await _googleSearchService.GetSteamInfo(game.Name);
            game.Title = platformGameInfoDto.Title;
            game.Link = platformGameInfoDto.Link;
            game.Description = platformGameInfoDto.Description;
            game.Price = platformGameInfoDto.Price;
            game.Reviews = platformGameInfoDto.Reviews;
            game.TumbnailUrl = platformGameInfoDto.TumbnailUrl;

            game.GeneratedInfo = true;
            game.LastModifiedOn = DateTime.Now;
            
            game.LastModifiedBy = Guid.Parse((ReadOnlySpan<char>)HttpContext.User.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier).Value);
            await _context.SaveChangesAsync();
        }
        return Ok(steamGameList);
    }
    
    //GET: api/GamesControllers
    [HttpGet("get-a-blizzard-link"), Authorize(Roles = "root")]
    public async Task<ActionResult<IEnumerable<Models.Game>>> GetABlizzardLink()
    {
        var steamGameList = await _context.Games.Where(game => game.Platforme == "Blizzard" && (game.Link == null || game.Link == "No Link found")).Take(1).ToListAsync();
        foreach (Models.Game game in steamGameList)
        {
            Console.WriteLine($"{game.Name}");
            PlatformInfoDto platformGameInfoDto = await _googleSearchService.GetSteamInfo(game.Name);
            game.Title = platformGameInfoDto.Title;
            game.Link = platformGameInfoDto.Link;
            game.Description = platformGameInfoDto.Description;
            game.Price = platformGameInfoDto.Price;
            game.Reviews = platformGameInfoDto.Reviews;
            game.TumbnailUrl = platformGameInfoDto.TumbnailUrl;

            game.GeneratedInfo = true;
            game.LastModifiedOn = DateTime.Now;
            
            game.LastModifiedBy = Guid.Parse((ReadOnlySpan<char>)HttpContext.User.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier).Value);
            await _context.SaveChangesAsync();
        }
        return Ok(steamGameList);
    }

    private bool GameExists(Guid id)
    {
        return _context.Games.Any(e => e.Id == id);
    }
}
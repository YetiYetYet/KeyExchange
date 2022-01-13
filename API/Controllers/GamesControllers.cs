using System.Net;
using API.Db;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.DTO;
using API.Models;
using API.Service;
using API.Utils;
using Microsoft.AspNetCore.Authorization;
using NuGet.Protocol;

namespace API.Controllers;

[Route("[controller]")]
[ApiController]
[Authorize]
public class GamesControllers : ControllerBase
{
    private readonly ContextApi _context;
    private IGoogleSearchService _googleSearchService;

    public GamesControllers(ContextApi context, IGoogleSearchService googleSearchService)
    {
        _context = context;
        _googleSearchService = googleSearchService;
    }

    // GET: api/GamesControllers
    [HttpGet("admin"), Authorize(Roles = "root")]
    public async Task<ActionResult<IEnumerable<Game>>> GetGamesAdmin()
    {
        return await _context.Games.ToListAsync();
    }

    // GET: api/GamesControllers/5
    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<ActionResult<Game>> GetGame(Guid id)
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
    public async Task<IActionResult> PutGame(Guid id, Game game)
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
    public async Task<ActionResult<Game>> PostGame(Game game)
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
    
    // GET: api/GamesControllers
    [HttpGet("get-a-link"), Authorize(Roles = "root")]
    public async Task<ActionResult<IEnumerable<Game>>> GetALink()
    {
        var steamGameList = await _context.Games.Where(game => game.Platforme == "Steam" && game.GameInfoFromPlatform == null).Take(10).ToListAsync();
        foreach (var game in steamGameList)
        {
            Console.WriteLine($"{game.Name}");
            var steamGameInfoDto = await _googleSearchService.GetSteamInfo(game.Name);
            GameInfoFromPlatform gameInfoFromPlatform = AutoMapperUtils.BasicAutoMapper<GameInfoFromPlatformDto, GameInfoFromPlatform>(steamGameInfoDto);
            var entityGameInfo = _context.GameInfoFromPlatforms.Add(gameInfoFromPlatform);
            var entity = _context.GameInfoFromPlatforms.Add(gameInfoFromPlatform).Entity;
            game.GameInfoFromPlatform = entity;
            await _context.SaveChangesAsync();
        }
        
        return await _context.Games.ToListAsync();
    }

    private bool GameExists(Guid id)
    {
        return _context.Games.Any(e => e.Id == id);
    }
}
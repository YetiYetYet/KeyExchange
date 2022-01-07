using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Catalog;
using API.DbContext;
using API.DTO;
using API.Utils;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using NuGet.Protocol;

namespace API.Controllers;

[Route("[controller]")]
[ApiController]
[Authorize]
public class GamesControllers : ControllerBase
{
    private readonly ContextApi _context;

    public GamesControllers(ContextApi context)
    {
        _context = context;
    }
    
    [AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<GameDto>>> GetGames()
    {
        var games = await _context.Games.ToListAsync();
        var gameDtos = AutoMapperUtils.BasicAutoMapper<List<Game>, List<GameDto>>(games);
        return Ok(gameDtos.ToJson());
    }
    
    // GET: api/GamesControllers
    [HttpGet("admin")]
    public async Task<ActionResult<IEnumerable<Game>>> GetGamesAdmin()
    {
        return await _context.Games.ToListAsync();
    }

    // GET: api/GamesControllers/5
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
    [HttpPut("{id:guid}")]
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
    [HttpPost]
    public async Task<ActionResult<Game>> PostGame(Game game)
    {
        _context.Games.Add(game);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetGame", new { id = game.Id }, game);
    }

    // DELETE: api/GamesControllers/5
    [HttpDelete("{id}")]
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

    private bool GameExists(Guid id)
    {
        return _context.Games.Any(e => e.Id == id);
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Db;
using API.DTO;
using API.Models;
using API.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NuGet.Protocol;

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
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GameDto>>> GetGames()
        {
            var gameList = await _context.Games.Include(game => game.GameInfoFromPlatform).Include(game => game.User).ToListAsync();
            var gameDtos = AutoMapperUtils.TupleAutoMapper<Game, GameDto>(gameList, new List<(Type, Type)>() { (typeof(ApplicationUser), typeof(UserDto)), (typeof(GameInfoFromPlatform), typeof(GameInfoFromPlatformDto))});
            return Ok(gameDtos.ToJson(Formatting.Indented));
        }
    }
}
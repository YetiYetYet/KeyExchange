using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
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
        
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<GameDto>>> GetGames()
        {
            var gameList = await _context.Games.Where(game => !game.SoftDeleted).ToListAsync();
            var gameDtos = AutoMapperUtils.TupleAutoMapper<Game, GameDto>(gameList, new List<(Type, Type)>() { (typeof(User), typeof(UserDto)) });
            Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(gameDtos));
            return Ok(gameDtos.ToJson(Formatting.Indented));
        }
    }
}
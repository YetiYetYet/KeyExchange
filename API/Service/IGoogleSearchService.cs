using System.Text.Json.Nodes;
using API.DTO;

namespace API.Service;

public interface IGoogleSearchService
{
    public Task<GameInfoFromPlatformDto> GetSteamInfo(string gameName);
    public string GetDefaultQueryOptions(string gameName);
    public Task<JsonNode?> SearchAsync(string query);
}
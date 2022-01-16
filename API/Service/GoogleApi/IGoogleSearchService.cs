using System.Text.Json.Nodes;
using API.DTO.Game;

namespace API.Service.GoogleApi;

public interface IGoogleSearchService
{
    public Task<PlatformInfoDto> GetSteamInfo(string gameName);
    public string GetDefaultQueryOptions(string gameName);
    public Task<JsonNode?> SearchAsync(string query);
}
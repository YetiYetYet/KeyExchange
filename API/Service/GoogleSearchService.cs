using System.Net;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
using API.DTO;
using Newtonsoft.Json;
using NuGet.Protocol;

namespace API.Service;

public class GoogleSearchService : IGoogleSearchService
{
    private IConfiguration _configuration;

    public GoogleSearchService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<SteamInfoDto> GetSteamInfo(string gameName)
    {
        var query = GetDefaultQueryOptions(gameName);
        JsonNode? json = await SearchAsync($"{query}+steam");
        Console.WriteLine(json.ToJsonString().ToJson(Formatting.Indented));
        try
        {
            //JsonNode? json = JsonNode.Parse(File.ReadAllText("Mock/GoogleSearchApi.json"));
            
            SteamInfoDto gameDto = new()
            {
                Title = json["items"][0]["pagemap"]["metatags"][0]["og:title"].ToString(),
                Link = json["items"][0]["link"].ToString(),
                Description = json["items"][0]["pagemap"]["metatags"][0]["og:description"].ToString(),
                Price = json["items"][0]["pagemap"]["offer"][0]["price"].ToString(),
                Reviews = json["items"][0]["pagemap"]["aggregaterating"][0]["description"].ToString(),
                TumbnailUrl = json["items"][0]["pagemap"]["product"][0]["image"].ToString()
            };
            return gameDto;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new SteamInfoDto()
            {
                Link = "No Link found",
            };
        }
    }

    public string GetDefaultQueryOptions(string gameName)
    {
        var parsedGameName = gameName.ToLower();
        parsedGameName = Regex.Replace(parsedGameName, "[^a-zA-Z0-9 ]",""); 
        parsedGameName = parsedGameName.Replace(" ", "+");
        var apiQuerry = _configuration.GetValue<string>("GoogleSearchApiUrl");
        var fullQuerry = $"{apiQuerry}&num=1&q={parsedGameName}";
        return fullQuerry;
    }
    
    public async Task<JsonNode?> SearchAsync(string query)
    {
        HttpClient httpClient = new();
        using HttpClient client = httpClient;
        var responseMessage = await Task.FromResult(httpClient.GetAsync(query));
        if (responseMessage.Result.StatusCode != HttpStatusCode.OK)
        {
            throw new Exception(await responseMessage.Result.Content.ReadAsStringAsync());
        }
        var responseContent = await responseMessage.Result.Content.ReadAsStringAsync();
        JsonNode? jsonNode = JsonNode.Parse(responseContent);
        return jsonNode;
    }
}
using System.Net;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
using API.DTO.Game;
using API.Utils;
using Newtonsoft.Json;
using NuGet.Protocol;

namespace API.Service.GoogleApi;

public class GoogleSearchService : IGoogleSearchService
{
    private IConfiguration _configuration;

    public GoogleSearchService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<PlatformInfoDto> GetSteamInfo(string gameName)
    {
        var query = GetDefaultQueryOptions(gameName);
        var json = await SearchAsync($"{query}+steam");
        Console.WriteLine(JsonUtils.ConstructJson(json));
        try
        {
            var title = json?["items"]?[0]?["pagemap"]?["metatags"]?[0]?["og:title"]?.ToString();
            var link = json?["items"]?[0]?["link"]?.ToString();
            var description = json?["items"]?[0]?["pagemap"]?["metatags"]?[0]?["og:description"]?.ToString();
            var price = json?["items"]?[0]?["pagemap"]?["offer"]?[0]?["price"]?.ToString();
            var reviews = json?["items"]?[0]?["pagemap"]?["aggregaterating"]?[0]?["description"]?.ToString();
            var tumbnailUrl = json?["items"]?[0]?["pagemap"]?["product"]?[0]?["image"]?.ToString();
            PlatformInfoDto gameDto = new()
            {
                Title = title,
                Link = link,
                Description = description,
                Price = price,
                Reviews = reviews,
                TumbnailUrl = tumbnailUrl
            };
            return gameDto;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new PlatformInfoDto()
            {
                Link = "No Link found",
            };
        }
    }
    
    public async Task<PlatformInfoDto> GetBlizzardInfo(string gameName)
    {
        var query = GetDefaultQueryOptions(gameName);
        var json = await SearchAsync($"{query}+steam");
        Console.WriteLine(JsonUtils.ConstructJson(json));
        try
        {
            var title = json?["items"]?[0]?["pagemap"]?["metatags"]?[0]?["og:title"]?.ToString();
            var link = json?["items"]?[0]?["link"]?.ToString();
            var description = json?["items"]?[0]?["pagemap"]?["metatags"]?[0]?["og:description"]?.ToString();
            var price = json?["items"]?[0]?["pagemap"]?["offer"]?[0]?["price"]?.ToString();
            var reviews = json?["items"]?[0]?["pagemap"]?["aggregaterating"]?[0]?["description"]?.ToString();
            var tumbnailUrl = json?["items"]?[0]?["pagemap"]?["cse_thumbnail"]?[0]?["src"]?.ToString();
            PlatformInfoDto gameDto = new()
            {
                Title = title,
                Link = link,
                Description = description,
                Price = price,
                Reviews = reviews,
                TumbnailUrl = tumbnailUrl
            };
            return gameDto;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new PlatformInfoDto()
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
        Console.WriteLine($"querry to google : {query}");
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
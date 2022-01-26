using System.Text.Json;
using System.Text.Json.Serialization;

namespace API.Service.GoogleApi;

public class SerializerService : ISerializerService
{
    public T Deserialize<T>(string text)
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        return JsonSerializer.Deserialize<T>(text, options) ?? throw new InvalidOperationException();
    }

    public string Serialize<T>(T obj)
    {
        var options = new JsonSerializerOptions 
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            WriteIndented = true,
        };
        return JsonSerializer.Serialize(obj, options);
    }

    public string Serialize<T>(T obj, Type type)
    {
        return JsonSerializer.Serialize(obj, type, new JsonSerializerOptions());
    }
    
}
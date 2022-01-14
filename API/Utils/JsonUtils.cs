using System.Text.Json;
using System.Text.Json.Serialization;

namespace API.Utils;

public static class JsonUtils
{
    public static string ConstructJson<T>(T data, bool indented = true, bool ignoreNull = false, bool snake_case = false)
    {
        var jsonOption = new JsonSerializerOptions();
        jsonOption.WriteIndented = indented;
        jsonOption.ReferenceHandler = ReferenceHandler.Preserve;
        jsonOption.DefaultIgnoreCondition =
            ignoreNull ? JsonIgnoreCondition.WhenWritingNull : JsonIgnoreCondition.Never;
        jsonOption.PropertyNamingPolicy = snake_case ? JsonNamingPolicy.CamelCase : SnakeCaseNamingPolicy.Instance;
        return JsonSerializer.Serialize(data, jsonOption);
    }
}

public class SnakeCaseNamingPolicy : JsonNamingPolicy
{
    public static SnakeCaseNamingPolicy Instance { get; } = new SnakeCaseNamingPolicy();

    public override string ConvertName(string name)
    {
        // Conversion to other naming convention goes here. Like SnakeCase, KebabCase etc.
        return name.ToSnakeCase();
    }
}
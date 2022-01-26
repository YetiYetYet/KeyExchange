

namespace API;

public class OrderByConverter
{
    public string[] Convert(string? item)
    {
        if (!string.IsNullOrWhiteSpace(item))
        {
            return item
                .Split(',')
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(x => x.Trim()).ToArray();
        }

        return Array.Empty<string>();
    }

    public string? ConvertBack(string[]? item)
    {
        return item?.Any() == true ? string.Join(",", item) : null;
    }
}
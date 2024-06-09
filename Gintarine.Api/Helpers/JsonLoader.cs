using System.Text.Json;

namespace Gintarine.Helpers;

public static class JsonLoader
{
    public static T LoadClientsFromJson<T>(string filePath)
    {
        using var reader = new StreamReader(filePath);
        var json = reader.ReadToEnd();
        return JsonSerializer.Deserialize<T>(json);
    }
}
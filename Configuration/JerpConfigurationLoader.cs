using System.Text.Json;
using NNTReverseProxy.Model;

namespace NNTReverseProxy.Configuration;

public class JerpConfigurationLoader
{
    public static JerpGateway Load(string path)
    {
        if (!File.Exists(path))
            throw new Exception($"Config file not found: {path}");

        var json = File.ReadAllText(path);

        var config = JsonSerializer.Deserialize<JerpGateway>(
            json,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

        if (config == null)
            throw new Exception("Failed to deserialize config");

        Validate(config);

        return config;
    }
    
    private static void Validate(JerpGateway config)
    {
        if (config.Services.Count == 0)
            throw new Exception("No services defined");

        foreach (var s in config.Services)
        {
            if (s.Instances.Count == 0)
                throw new Exception($"Service '{s.Name}' has no destinations");

            if (!s.Path.StartsWith("/"))
                throw new Exception($"Service '{s.Name}' path must start with '/'");
        }
    }
}
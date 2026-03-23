using System.Text.Json;
using NNTReverseProxy.Model;

namespace NNTReverseProxy.Configuration;

public static class JerpConfigurationLoader
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
            throw new Exception("Failed to deserialize config. Check config file.");

        ValidateGateway(config);

        return config;
    }
    
    private static void ValidateGateway(JerpGateway config)
    {
        if (config.Services.Count == 0)
            throw new Exception("No services defined");

        foreach (var s in config.Services)
        {
            ValidateService(s);
        }
    }

    private static void ValidateService(JerpService service)
    {
        if (service.Instances.Count == 0)
            throw new Exception($"Service '{service.Name}' has no instances defined.");
        
        if (!service.Path.StartsWith('/'))
            throw new Exception($"Service '{service.Name}' path must start with '/'");

        if (service.LoadBalancingPolicy != null &&
            !JerpService.LoadBalancingPolicies.Contains(service.LoadBalancingPolicy))
        {
            throw new Exception($"Service {service.Name} doesn't have a valid LoadBalancingPolicy defined.");
        }
        
    }
}
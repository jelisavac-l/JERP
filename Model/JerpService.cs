using NNTReverseProxy.Configuration;

namespace NNTReverseProxy.Model;

public class JerpService
{
    public required string Name { get; set; }
    public required string Path { get; set; }
    public required List<JerpInstance> Instances { get; set; }

    public string? LoadBalancingPolicy { get; set; }
    public bool StripPrefix { get; set; } = false;
    public int? Timeout { get; set; }
    public int Retries { get; set; } = 0;
    public Dictionary<string, string>? AddHeaders { get; set; }
    
}
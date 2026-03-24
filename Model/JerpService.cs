using NNTReverseProxy.Configuration;

namespace NNTReverseProxy.Model;

public class JerpService
{

    /*
     * rr - Round-Robin: Goes round like a record baby
     * wrr - Weighted Round-Robin: Assigns weight to the rr algorithm
     * iph - IP (Consistent) Hashing: Always routes same client to the same instance 
     */
    public static readonly List<string> LoadBalancingPolicies = ["rr", "wrr", "iph", "rnd"];
    
    public required string Name { get; set; }
    public required string Path { get; set; }
    public required List<JerpInstance> Instances { get; set; }
    
    public required string LoadBalancingPolicy { get; set; }
    public bool StripPrefix { get; set; } = true;
    public required int Timeout { get; set; } = 30;
    public int Retries { get; set; } = 0;
    
    public Dictionary<string, string>? AddHeaders { get; set; }
    
}
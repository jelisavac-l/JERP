using NNTReverseProxy.Configuration;
using NNTReverseProxy.LoadBalancer;

namespace NNTReverseProxy.Model;

public class JerpService
{

    /*
     * rr - Round-Robin: Goes round like a record baby
     * wrr - Weighted Round-Robin: Assigns weight to the rr algorithm
     * iph - IP (Consistent) Hashing: Always routes same client to the same instance 
     */
    public static readonly List<string> LoadBalancingPolicies = ["rr", "wrr", "iph", "rnd", "fa"];
    
    public required string Name { get; set; }
    public required string Path { get; set; }
    public required List<JerpInstance> Instances { get; set; }
    
    public string LoadBalancingPolicy { get; set; } = "fa";
    public bool StripPrefix { get; set; } = true;
    public required int Timeout { get; set; } = 30;
    public int Retries { get; set; } = 0;
    
    public Dictionary<string, string>? AddHeaders { get; set; }

    // For Round-Robin load balancer
    public int Index = -1;

    public ILoadBalancer LoadBalancer { get; set; } = null!;
}
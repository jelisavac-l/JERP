namespace NNTReverseProxy.LoadBalancer;

public static class LoadBalancerFactory
{
    public static ILoadBalancer Create(string policy)
    {
        return policy switch
        {
            "rr" => new RoundRobinLoadBalancer(),
            "wrr" => new WeightedRoundRobinLoadBalancer(),
            "iph" => new IpHashingLoadBalancer(),
            "rnd" => new RandomLoadBalancer(),
            _ => new FirstAvailableLoadBalancer()
        };
    }
}
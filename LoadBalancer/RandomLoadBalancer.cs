using NNTReverseProxy.Model;

namespace NNTReverseProxy.LoadBalancer;

public class RandomLoadBalancer : ILoadBalancer
{
    public string Topic => nameof(RoundRobinLoadBalancer);
    
    private readonly Random _random =  new Random();
    
    public JerpInstance ChooseInstance(JerpService service)
    {
        if (service.Instances.Count == 0)
            throw new InvalidOperationException("No instances defined.");
        
        var instances = service.Instances.Where(i => i.IsHealthy).ToList();
        if (instances.Count == 0)
            throw new InvalidOperationException("No healthy instances.");
        
        
        return instances[_random.Next(instances.Count)];
    }
}
using NNTReverseProxy.Model;

namespace NNTReverseProxy.LoadBalancer;

public class RoundRobinLoadBalancer : ILoadBalancer
{
    
    public string Topic => nameof(RoundRobinLoadBalancer);
    
    public JerpInstance ChooseInstance(JerpService service)
    {
        
        if (service.Instances.Count == 0)
            throw new InvalidOperationException("No instances defined.");
        
        var instances = service.Instances.Where(i => i.IsHealthy).ToList();
        if (instances.Count == 0)
            throw new InvalidOperationException("No healthy instances.");

        // Concurrency!
        var next = Interlocked.Increment(ref service.Index);
        return instances[next % instances.Count];
    }
}
using NNTReverseProxy.Model;

namespace NNTReverseProxy.LoadBalancer;

public class FirstAvailableLoadBalancer : ILoadBalancer
{
    
    public string Topic => nameof(RoundRobinLoadBalancer);
    
    public JerpInstance ChooseInstance(JerpService service)
    {
        var instances = service.Instances.Where(i => i.IsHealthy).ToList();
        return instances.Count == 0 ? throw new InvalidOperationException("No healthy instances.") : instances[0];
    }
}
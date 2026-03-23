using NNTReverseProxy.Model;

namespace NNTReverseProxy.LoadBalancer;

public class RoundRobinLoadBalancer : ILoadBalancer
{

    private int _index = -1;
    
    public JerpInstance ChooseInstance(JerpService service)
    {
        if (service.Instances.Count == 0)
            throw new InvalidOperationException("No instances defined.");

        // Concurrency!
        var next = Interlocked.Increment(ref _index);
        return service.Instances[next % service.Instances.Count];
    }
}
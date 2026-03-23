using NNTReverseProxy.Model;

namespace NNTReverseProxy.LoadBalancer;

public class WeightedRoundRobinLoadBalancer : ILoadBalancer
{
    public JerpInstance ChooseInstance(JerpService service)
    {
        throw new NotImplementedException();
    }
}
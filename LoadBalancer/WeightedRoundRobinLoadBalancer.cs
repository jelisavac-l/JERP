using NNTReverseProxy.Model;

namespace NNTReverseProxy.LoadBalancer;

public class WeightedRoundRobinLoadBalancer : ILoadBalancer
{
    public override string ToString()
    {
        return nameof(RoundRobinLoadBalancer);
    }
    
    public JerpInstance ChooseInstance(JerpService service)
    {
        throw new NotImplementedException();
    }
}
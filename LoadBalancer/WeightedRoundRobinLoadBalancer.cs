using NNTReverseProxy.Model;

namespace NNTReverseProxy.LoadBalancer;

public class WeightedRoundRobinLoadBalancer : ILoadBalancer
{
    public string Topic => nameof(RoundRobinLoadBalancer);
    public JerpInstance ChooseInstance(JerpService service)
    {
        throw new NotImplementedException();
    }
}
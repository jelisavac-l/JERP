using NNTReverseProxy.Model;

namespace NNTReverseProxy.LoadBalancer;

public class IpHashingLoadBalancer : ILoadBalancer
{
    public JerpInstance ChooseInstance(JerpService service)
    {
        throw new NotImplementedException();
    }
}
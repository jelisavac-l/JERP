using NNTReverseProxy.Model;

namespace NNTReverseProxy.LoadBalancer;

public class IpHashingLoadBalancer : ILoadBalancer
{
    public override string ToString()
    {
        return nameof(IpHashingLoadBalancer);
    }
    
    public JerpInstance ChooseInstance(JerpService service)
    {
        throw new NotImplementedException();
    }
}
using NNTReverseProxy.Model;

namespace NNTReverseProxy.LoadBalancer;

public interface ILoadBalancer
{
    JerpInstance ChooseInstance(JerpService service);
    string ToString();
}
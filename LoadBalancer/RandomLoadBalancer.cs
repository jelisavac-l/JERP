using NNTReverseProxy.Model;

namespace NNTReverseProxy.LoadBalancer;

public class RandomLoadBalancer : ILoadBalancer
{
    
    private readonly Random _random =  new Random();
    
    public JerpInstance ChooseInstance(JerpService service)
    {
        return service.Instances[_random.Next(service.Instances.Count)];
    }
}
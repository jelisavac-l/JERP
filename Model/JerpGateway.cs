namespace NNTReverseProxy.Model;

public class JerpGateway
{
    public required string ServerName { get; set; }
    public required List<JerpService> Services { get; set; }

    public void PrintConfig()
    {
        Console.WriteLine($"Jerp Gateway Configuration for {ServerName}");
        Console.WriteLine("-----------------------------------------------------------------");
        foreach (var service in Services)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"\n~ {service.Name}");
            Console.ResetColor();
            
            Console.WriteLine($"  Path: {service.Path}");
            Console.WriteLine($"  Load Balancer: {service.LoadBalancingPolicy}");
            
            Console.WriteLine($"  Strip prefix: {service.StripPrefix}");
            Console.WriteLine($"  Retries: {service.Retries}");
            Console.WriteLine($"  Timeout: {service.Timeout}s");
            Console.WriteLine($"  Instances ({service.Instances.Count}):");
            foreach (var instance in service.Instances)
            {
                Console.WriteLine($"    - {instance.Url} ({instance.HealthCheckPath})");
            }
        }
        Console.WriteLine();
    }
}
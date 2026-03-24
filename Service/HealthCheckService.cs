using NNTReverseProxy.Model;

namespace NNTReverseProxy.Service;

public class HealthCheckService : BackgroundService
{
    private readonly HttpClient _httpClient;
    private readonly JerpGateway _config;

    public HealthCheckService(HttpClient httpClient, JerpGateway config)
    {
        _httpClient = httpClient;
        _config = config;
    }

    public async Task<bool> IsInstanceHealthy(JerpInstance instance)
    {
        try
        {
            var healthCheckUrl = instance.Url + instance.HealthCheckPath;
            var response = await _httpClient.GetAsync(healthCheckUrl);
            return response.IsSuccessStatusCode;
        }
        catch (Exception e)
        {
            return false;
        }
        
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            foreach (var service in _config.Services)
            {
                foreach (var instance in service.Instances)
                {
                    if (DateTime.UtcNow - instance.LastChecked < TimeSpan.FromSeconds((double)service.Timeout))
                        continue;
                    
                    instance.LastChecked = DateTime.UtcNow;
                    
                    // TODO: Tasks to List?
                    _ = Task.Run(async () =>
                    {
                        instance.IsHealthy = await IsInstanceHealthy(instance);
                        Console.WriteLine($"Service {service.Name}'s instance {instance.Url} is healthy: {instance.IsHealthy}");
                    }, stoppingToken);
                }
            }

            await Task.Delay(1000, stoppingToken);
        }
    }
}
using NNTReverseProxy.Model;

namespace NNTReverseProxy.Service;

public class HealthCheckService : BackgroundService
{
    private readonly HttpClient _httpClient;
    private readonly JerpGateway _config;
    private readonly ILogger<GatewayService> _logger;

    public HealthCheckService(HttpClient httpClient, JerpGateway config, ILogger<GatewayService> logger)
    {
        _httpClient = httpClient;
        _config = config;
        _logger = logger;
    }

    public async Task<bool> IsInstanceHealthy(JerpInstance instance)
    {
        var healthCheckUrl = instance.Url + instance.HealthCheckPath;

        try
        {
            var response = await _httpClient.GetAsync(healthCheckUrl);
            return response.IsSuccessStatusCode;
        }
        catch (HttpRequestException e)
        {
            _logger.LogWarning("Healthcheck request failed: {Message}", e.Message);
            return false;
        }
        catch (Exception e)
        {
            _logger.LogWarning("Healthcheck failed: {Message}", e.Message);
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
                        if (!instance.IsHealthy)
                            _logger.LogWarning(
                                "Instance {Instance} of {Service} is {Status}",
                                instance.Url,
                                service.Name,
                                instance.IsHealthy ? "HEALTHY" : "UNHEALTHY"
                            );
                    }, stoppingToken);
                }
            }

            await Task.Delay(1000, stoppingToken);
        }
    }
}
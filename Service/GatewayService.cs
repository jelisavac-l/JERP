using NNTReverseProxy.Model;
using NNTReverseProxy.Networking;

namespace NNTReverseProxy.Service;

public class GatewayService
{
    private readonly JerpGateway _gateway;
    private readonly Forwarder _forwarder;
    private readonly ILogger<GatewayService> _logger;

    public GatewayService(JerpGateway gateway, Forwarder forwarder, ILogger<GatewayService> logger)
    {
        _gateway = gateway;
        _forwarder = forwarder;
        _logger = logger;
    }
    
    private JerpService? MatchService(HttpContext context)
    {
        return _gateway.Services
            .Where(s => context.Request.Path.StartsWithSegments(s.Path))
            .OrderByDescending(s => s.Path.Length)
            .FirstOrDefault();
    }
    
    private string PickDestination(JerpService service)
    {
        // TODO: later replace with load balancer
        return service.Instances[0].Url;
    }
    
    public async Task HandleRequest(HttpContext context)
    {
        var service = MatchService(context);

        if (service == null)
        {
            _logger.LogWarning("No route matched for {Path}", context.Request.Path);
            context.Response.StatusCode = 404;
            await context.Response.WriteAsync("No matching service");
            return;
        }
        
        var destination = PickDestination(service);

        _logger.LogInformation("Routing {Path} → {Service} → {Destination}",
            context.Request.Path,
            service.Name,
            destination);

        if (service.StripPrefix)
        {
            context.Request.Path = context.Request.Path.Value!.Substring(service.Path.Length);

            if (string.IsNullOrEmpty(context.Request.Path))
                context.Request.Path = "/";
        }
        
        // Append custom headers
        if (service.AddHeaders != null)
            foreach (var header in service.AddHeaders)
            {
                context.Request.Headers.Append(header.Key, header.Value);
            }

        await _forwarder.Forward(context, destination);
        
    }
}
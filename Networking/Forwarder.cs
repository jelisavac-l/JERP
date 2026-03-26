namespace NNTReverseProxy.Networking;

public class Forwarder
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<Forwarder> _logger;
    
    public Forwarder(IHttpClientFactory factory, ILogger<Forwarder> logger)
    {
        _logger = logger;
        _httpClient = factory.CreateClient();
    }
    
    public async Task Forward(HttpContext context, string destination)
    {
        
        _logger.LogInformation(
            "→ {Method} {Path} → {Destination}",
            context.Request.Method,
            context.Request.Path,
            destination
        );
        
        var targetUri = destination + context.Request.Path.Value + context.Request.QueryString.Value;
        
        var requestMessage = new HttpRequestMessage
        {
            Method = new HttpMethod(context.Request.Method),
            RequestUri = new Uri(targetUri),
        };

        // TODO: Implement better filters!
        foreach (var header in context.Request.Headers)
        {
            if (!requestMessage.Headers.TryAddWithoutValidation(header.Key, header.Value.ToArray()))
            {
                requestMessage.Content?.Headers.TryAddWithoutValidation(header.Key, header.Value.ToArray());
            }
        }
        
        requestMessage.Headers.Add("X-Gateway", "JERP");
        
        if (context.Request.ContentLength > 0 || context.Request.Body.CanRead)
        {
            // Use StreamContent to avoid loading entire request into memory.
            requestMessage.Content = new StreamContent(context.Request.Body);
        }

        HttpResponseMessage responseMessage;
        
        // Send request
        try
        {
            responseMessage = await _httpClient.SendAsync(
                requestMessage,
                HttpCompletionOption.ResponseHeadersRead,
                context.RequestAborted
            );
        }
        catch (Exception e)
        {
            context.Response.StatusCode = 500;
            await context.Response.WriteAsync(e.Message);
            throw;
        }
        
        foreach (var header in responseMessage.Headers)
        {
            context.Response.Headers[header.Key] = header.Value.ToArray();
        }
        
        foreach (var header in responseMessage.Content.Headers)
        {
            context.Response.Headers[header.Key] = header.Value.ToArray();
        }
        
        context.Response.Headers.Remove("transfer-encoding");
        
        // Copy body
        await responseMessage.Content.CopyToAsync(context.Response.Body);
    }
}
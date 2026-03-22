namespace NNTReverseProxy.Forwarder;

public class Forwarder
{
    private readonly HttpClient _httpClient;
    
    public Forwarder(IHttpClientFactory factory)
    {
        _httpClient = factory.CreateClient();
    }
    
    public async Task Forward(HttpContext context, string destination)
    {
        
        Console.WriteLine($"Forwarding {context.Request.Path} to {destination}");
        
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
        
        if (context.Request.ContentLength > 0 || context.Request.Body.CanRead)
        {
            // Use StreamContent to avoid loading entire request into memory.
            requestMessage.Content = new StreamContent(context.Request.Body);
        }
        
        Console.WriteLine($"Request message: {requestMessage}");
        
        Console.WriteLine("Waiting for response...");
        
        // Send request
        var responseMessage = await _httpClient.SendAsync(
            requestMessage,
            HttpCompletionOption.ResponseHeadersRead,
            context.RequestAborted
        );
        
        Console.WriteLine($"Response received. Status code: {responseMessage.StatusCode}");
        
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
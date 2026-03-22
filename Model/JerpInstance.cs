namespace NNTReverseProxy.Model;

public class JerpInstance
{
    public required string Url { get; set; }
    public string? HealthCheckPath { get; set; }
}
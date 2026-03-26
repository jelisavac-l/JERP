namespace NNTReverseProxy.Model;

public class JerpInstance
{
    public required string Url { get; set; }
    public string? HealthCheckPath { get; set; }
    public bool IsHealthy { get; set; }
    public DateTime LastChecked { get; set; }
}
using NNTReverseProxy.Configuration;
using NNTReverseProxy.Networking;
using NNTReverseProxy.Service;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient();
builder.Logging.ClearProviders();

builder.Logging.AddSimpleConsole(options =>
{
    options.SingleLine = true;
    options.TimestampFormat = "HH:mm:ss ";
    options.UseUtcTimestamp = false;

});

builder.Services.AddSingleton<Forwarder>();
builder.Services.AddSingleton<GatewayService>();
builder.Services.AddHostedService<HealthCheckService>();

var config = JerpConfigurationLoader.Load("config.json");
builder.Services.AddSingleton(config);

var app = builder.Build();

// TODO: Remove hardcoded values
const string host = "http://localhost:5540/";
const string version = "0.0.1-alpha";
Console.WriteLine($"""
                      / \__
                     (    @\___         🐾 JERP {version}
                     /         O        Rutting @ {host}
                    /   (_____/
                   /_____/   
                   """);

config.PrintConfig();   

app.Run(async context =>
{
    var gateway = context.RequestServices.GetRequiredService<GatewayService>();
    await gateway.HandleRequest(context);
});

app.Run(host);
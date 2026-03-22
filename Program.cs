using NNTReverseProxy.Configuration;
using NNTReverseProxy.Forwarder;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient();
builder.Services.AddSingleton<Forwarder>();
// builder.Services.AddSingleton(JerpConfigurationLoader.Load("config.json"));

var app = builder.Build();

var config = JerpConfigurationLoader.Load("config.json");
config.PrintConfig();

app.Run(async context =>
{
    var forwarder = context.RequestServices.GetRequiredService<Forwarder>();
    // var config = context.RequestServices.GetRequiredService<JerpGatewayConfiguration>();
    // Console.WriteLine(config);
    await forwarder.Forward(context, "http://localhost:8080/");
});

app.Run();
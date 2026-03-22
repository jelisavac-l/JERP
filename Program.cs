using NNTReverseProxy.Forwarder;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient();
builder.Services.AddSingleton<Forwarder>();

var app = builder.Build();

app.Run(async context =>
{
    var forwarder = context.RequestServices.GetRequiredService<Forwarder>();
    await forwarder.Forward(context, "http://localhost:8080/");
});

app.Run();
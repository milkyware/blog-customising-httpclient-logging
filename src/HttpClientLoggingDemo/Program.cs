using HttpClientLoggingDemo;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var httpBuilder = builder.Services.AddHttpClient<SampleService>();
httpBuilder.AddHttpMessageHandler(sp =>
{
    var loggerFactory = sp.GetRequiredService<ILoggerFactory>();
    string loggerName = !string.IsNullOrEmpty(httpBuilder.Name) ? httpBuilder.Name : "Default";
    var logger = loggerFactory.CreateLogger($"System.Net.Http.HttpClient.{loggerName}.ClientBodyHandler");
    var handler = new LoggingHttpBodyHandler(logger);
    return handler;
});
httpBuilder.AddLogger(sp =>
    {
        string loggerName = !string.IsNullOrEmpty(httpBuilder.Name) ? httpBuilder.Name : "Default";
        var category = $"System.Net.Http.HttpClient.{loggerName}.ClientBodyHandler2";
        var loggerFactory = sp.GetRequiredService<ILoggerFactory>();
        var logger = loggerFactory.CreateLogger(category);
        return new HttpClientBodyLogger(logger);
    }, false);
builder.Services.AddHostedService<SampleWorker>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/sample", (ILogger<Program> logger, [FromBody] SampleRequest request) =>
{
    logger.LogInformation("value={value}", request.Value);
    return Results.Ok(new SampleRequest { Value = Guid.NewGuid().ToString() });
})
.WithName("Sample")
.WithOpenApi();

await app.RunAsync();

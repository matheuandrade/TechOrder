using CustomerOrderService.Application;
using CustomerOrderService.Extensions;
using CustomerOrderService.Infrastructure;
using Polly.Extensions.Http;
using Polly;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services
    .AddInfrastructure()
    .AddApplication()    
    .AddControllers();

var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

builder.Services.AddHttpClient("SupplierApi", client =>
{
    client.BaseAddress = new Uri($"{config.GetValue<string>("SupplierBaseURI")}");
})
.AddPolicyHandler(GetRetryPolicy()) // Retry policy
.AddPolicyHandler(GetCircuitBreakerPolicy()) // Circuit breaker policy
.AddPolicyHandler(Policy.TimeoutAsync<HttpResponseMessage>(TimeSpan.FromSeconds(10)));


builder.Services.AddScoped<CustomerOrderService.Core.External.Suppliers.Interfaces.ISupplierApiService, CustomerOrderService.Infrastructure.External.Suppliers.SupplierApiService>();

builder.Services.AddEndpoints(Assembly.GetExecutingAssembly());

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.MapEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
{
    return HttpPolicyExtensions
        .HandleTransientHttpError() // Handles HTTP 5xx, 408 (Request Timeout), and network failures
        .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
            (result, timeSpan, retryCount, context) =>
            {
                Console.WriteLine($"Retry {retryCount} after {timeSpan.TotalSeconds}s due to {result.Exception?.Message}");
            });
}

static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
{
    return HttpPolicyExtensions
        .HandleTransientHttpError()
        .CircuitBreakerAsync(3, TimeSpan.FromSeconds(30),
            (result, timeSpan) =>
            {
                Console.WriteLine("Circuit breaker opened");
            },
            () =>
            {
                Console.WriteLine("Circuit breaker reset");
            });
}

using ResellerService.Aplication;
using ResellerService.Extensions;
using ResellerService.Infrastructure;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services
    .AddApplication()
    .AddInfrastructure()
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
});
builder.Services.AddScoped<ResellerService.Core.External.Supplier.ISupplierApiService, ResellerService.Infrastructure.External.SupplierApiService>();

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

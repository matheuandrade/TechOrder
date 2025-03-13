using CustomerOrderService.Application;
using CustomerOrderService.Extensions;
using CustomerOrderService.Infrastructure;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services
    .AddInfrastructure()
    .AddApplication()    
    .AddControllers();

builder.Services.AddHttpClient();
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

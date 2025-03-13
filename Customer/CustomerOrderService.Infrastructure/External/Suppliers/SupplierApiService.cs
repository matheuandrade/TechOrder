using CustomerOrderService.Core.External.Suppliers.Dtos;
using CustomerOrderService.Core.External.Suppliers.Interfaces;
using CustomerOrderService.Core.External.Suppliers.Models;
using Newtonsoft.Json;
using System.Text;

namespace CustomerOrderService.Infrastructure.External.Suppliers;

public class SupplierApiService(IHttpClientFactory httpClientFactory) : ISupplierApiService
{
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient("SupplierApi");

    public async Task<Guid?> CreateSupplyOrder(CreateOrderSupplierDto model, CancellationToken cancellationToken)
    {
        var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync("orders", content, cancellationToken);
        var contentAsString = await response.Content.ReadAsStringAsync(cancellationToken);

        if (response.StatusCode != System.Net.HttpStatusCode.Created)
            return null;

        var responseObject = JsonConvert.DeserializeObject<CreateSupplierResponseDto>(contentAsString);

        return responseObject?.Id;
    }
}

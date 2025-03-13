using CustomerOrderService.Infrastructure.External.Suppliers.Interfaces;
using CustomerOrderService.Infrastructure.External.Suppliers.Models;
using Newtonsoft.Json;
using System.Text;

namespace CustomerOrderService.Infrastructure.External.Suppliers;

public class SupplierApiService(IHttpClientFactory httpClientFactory) : ISupplierApiService
{
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient("ExternalApiClient");

    public async Task<Guid?> CreateSupplyOrder(CreateOrderSupplierDto model, CancellationToken cancellationToken)
    {
        var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync("supplier/reseller", content, cancellationToken);
        var contentAsString = await response.Content.ReadAsStringAsync(cancellationToken);

        if (response.StatusCode != System.Net.HttpStatusCode.Created)
            return null;

        return JsonConvert.DeserializeObject<Guid>(contentAsString);
    }
}

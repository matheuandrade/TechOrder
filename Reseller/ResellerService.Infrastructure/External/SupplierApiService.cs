using Newtonsoft.Json;
using ResellerService.Core.External.Supplier;
using System.Text;

namespace ResellerService.Infrastructure.External;

public class SupplierApiService(IHttpClientFactory httpClientFactory) : ISupplierApiService
{
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient("ExternalApiClient");

    public async Task<Guid?> CreateSupplier(string cnpj, CancellationToken cancellationToken)
    {
        var requestBody = new
        {
            CNPJ = cnpj
        };

        var content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync("reseller", content, cancellationToken);
        var contentAsString = await response.Content.ReadAsStringAsync(cancellationToken);

        if (response.StatusCode != System.Net.HttpStatusCode.Created)
            return null;

        return JsonConvert.DeserializeObject<Guid>(contentAsString);
    }
}

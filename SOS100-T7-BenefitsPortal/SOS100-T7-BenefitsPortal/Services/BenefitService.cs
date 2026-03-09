using System.Net.Http.Json;
using System.Text.Json;
using SOS100_T7_BenefitsPortal.Models;

namespace SOS100_T7_BenefitsPortal.Services;

public class BenefitService(HttpClient httpClient, IConfiguration config)
{
    private static readonly JsonSerializerOptions JsonOptions = new() { PropertyNameCaseInsensitive = true };
    private string ApiKey => config["BenefitServiceApiKey"] ?? string.Empty;

    public async Task<List<BenefitViewModel>> GetBenefitsAsync()
    {
        var response = await httpClient.GetAsync("api/benefits");
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<List<BenefitViewModel>>(json, JsonOptions) ?? [];
    }

    public async Task<BenefitViewModel?> GetByIdAsync(int id)
    {
        var response = await httpClient.GetAsync($"api/benefits/{id}");
        if (response.StatusCode == System.Net.HttpStatusCode.NotFound) return null;
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<BenefitViewModel>(json, JsonOptions);
    }

    public async Task<List<BenefitViewModel>> GetByCategoryAsync(int categoryId)
    {
        var response = await httpClient.GetAsync($"api/benefits/category/{categoryId}");
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<List<BenefitViewModel>>(json, JsonOptions) ?? [];
    }

    public async Task CreateAsync(BenefitFormModel model)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, "api/benefits");
        request.Headers.Add("X-Api-Key", ApiKey);
        request.Content = JsonContent.Create(model);
        var response = await httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();
    }

    public async Task UpdateAsync(BenefitFormModel model)
    {
        var request = new HttpRequestMessage(HttpMethod.Put, $"api/benefits/{model.Id}");
        request.Headers.Add("X-Api-Key", ApiKey);
        request.Content = JsonContent.Create(model);
        var response = await httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteAsync(int id)
    {
        var request = new HttpRequestMessage(HttpMethod.Delete, $"api/benefits/{id}");
        request.Headers.Add("X-Api-Key", ApiKey);
        var response = await httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();
    }
}

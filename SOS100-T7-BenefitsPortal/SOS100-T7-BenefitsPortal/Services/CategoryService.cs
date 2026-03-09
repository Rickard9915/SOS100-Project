using System.Net.Http.Json;
using System.Text.Json;
using SOS100_T7_BenefitsPortal.Models;

namespace SOS100_T7_BenefitsPortal.Services;

public class CategoryService(HttpClient httpClient, IConfiguration config)
{
    private static readonly JsonSerializerOptions JsonOptions = new() { PropertyNameCaseInsensitive = true };
    private string ApiKey => config["BenefitServiceApiKey"] ?? string.Empty;

    public async Task<List<CategoryViewModel>> GetAllAsync()
    {
        var response = await httpClient.GetAsync("api/categories");
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<List<CategoryViewModel>>(json, JsonOptions) ?? [];
    }

    public async Task<CategoryViewModel?> GetByIdAsync(int id)
    {
        var response = await httpClient.GetAsync($"api/categories/{id}");
        if (response.StatusCode == System.Net.HttpStatusCode.NotFound) return null;
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<CategoryViewModel>(json, JsonOptions);
    }

    public async Task CreateAsync(string name)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, "api/categories");
        request.Headers.Add("X-Api-Key", ApiKey);
        request.Content = JsonContent.Create(new { name });
        var response = await httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();
    }

    public async Task UpdateAsync(int id, string name)
    {
        var request = new HttpRequestMessage(HttpMethod.Put, $"api/categories/{id}");
        request.Headers.Add("X-Api-Key", ApiKey);
        request.Content = JsonContent.Create(new { id, name });
        var response = await httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteAsync(int id)
    {
        var request = new HttpRequestMessage(HttpMethod.Delete, $"api/categories/{id}");
        request.Headers.Add("X-Api-Key", ApiKey);
        var response = await httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();
    }
}

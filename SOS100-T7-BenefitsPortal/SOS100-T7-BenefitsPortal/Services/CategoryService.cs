using System.Net.Http.Json;
using System.Text.Json;
using SOS100_T7_BenefitsPortal.Models;

namespace SOS100_T7_BenefitsPortal.Services;

public class CategoryService(HttpClient httpClient)
{
    private static readonly JsonSerializerOptions JsonOptions = new() { PropertyNameCaseInsensitive = true };

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
        var response = await httpClient.PostAsJsonAsync("api/categories", new { name });
        response.EnsureSuccessStatusCode();
    }

    public async Task UpdateAsync(int id, string name)
    {
        var response = await httpClient.PutAsJsonAsync($"api/categories/{id}", new { id, name });
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteAsync(int id)
    {
        var response = await httpClient.DeleteAsync($"api/categories/{id}");
        response.EnsureSuccessStatusCode();
    }
}

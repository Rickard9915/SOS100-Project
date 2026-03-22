using System.Net.Http.Json;

namespace SOS100_T7_BenefitsPortal.Services;

public class UserApiService
{
    private readonly HttpClient _httpClient;

    public UserApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<UserDto?> LoginAsync(string email, string password)
    {
        var response = await _httpClient.PostAsync(
            $"api/auth/login?email={Uri.EscapeDataString(email)}&password={Uri.EscapeDataString(password)}",
            null);

        if (!response.IsSuccessStatusCode)
            return null;

        return await response.Content.ReadFromJsonAsync<UserDto>();
    }
}

public class UserDto
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Email { get; set; } = "";
    public string Role { get; set; } = "";
}

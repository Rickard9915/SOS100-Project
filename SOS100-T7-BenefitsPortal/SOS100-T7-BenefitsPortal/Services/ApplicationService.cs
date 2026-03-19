using System.Net.Http.Json;

namespace SOS100_T7_BenefitsPortal.Services;

public class ApplicationService
{
    private readonly HttpClient _httpClient;

    public ApplicationService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<ApplicationDto>> GetApplicationsAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<ApplicationDto>>("api/Applications")
               ?? new List<ApplicationDto>();
    }

    public async Task CreateApplicationAsync(CreateApplicationDto application)
    {
        var response = await _httpClient.PostAsJsonAsync("api/Applications", application);
        response.EnsureSuccessStatusCode();
    }
}

public class ApplicationDto
{
    public int Id { get; set; }
    public string EmployeeName { get; set; } = "";
    public int BenefitId { get; set; }
    public string Status { get; set; } = "";
}

public class CreateApplicationDto
{
    public string EmployeeName { get; set; } = "";
    public int BenefitId { get; set; }
}
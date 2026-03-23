using System.Net.Http.Json;

namespace SOS100_T7_BenefitsPortal.Services;

public class ApplicationService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _config;

    public ApplicationService(HttpClient httpClient, IConfiguration config)
    {
        _httpClient = httpClient;
        _config = config;
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

    public async Task ReviewApplicationAsync(int id, string decision, string reviewedBy)
    {
        var apiKey = _config["ApiKeys:ApplicationService"] ?? "";
        var request = new HttpRequestMessage(HttpMethod.Post, $"api/Applications/{id}/review");
        request.Headers.Add("X-API-Key", apiKey);
        request.Content = JsonContent.Create(new ReviewDto
        {
            Decision = decision,
            ReviewedBy = reviewedBy,
            Comment = ""
        });
        var response = await _httpClient.SendAsync(request);
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

public class ReviewDto
{
    public string Decision { get; set; } = "";
    public string ReviewedBy { get; set; } = "";
    public string Comment { get; set; } = "";
}
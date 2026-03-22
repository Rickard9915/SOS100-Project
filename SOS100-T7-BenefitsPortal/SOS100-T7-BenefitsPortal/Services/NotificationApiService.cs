using System.Net.Http.Json;

namespace SOS100_T7_BenefitsPortal.Services;

public class NotificationApiService
{
    private readonly HttpClient _httpClient;

    public NotificationApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<NotificationDto>> GetNotificationsAsync(int userId)
    {
        return await _httpClient.GetFromJsonAsync<List<NotificationDto>>($"api/notification/{userId}")
               ?? new List<NotificationDto>();
    }

    public async Task MarkAsReadAsync(int id)
    {
        await _httpClient.PutAsync($"api/notification/{id}/read", null);
    }
}

public class NotificationDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int ApplicationId { get; set; }
    public string Status { get; set; } = "";
    public string Message { get; set; } = "";
    public bool IsRead { get; set; }
    public DateTime CreatedAt { get; set; }
}

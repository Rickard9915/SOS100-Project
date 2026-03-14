namespace SOS100_NotificationService.Dtos
{
    public class CreateNotificationDto
    {
        public int UserId { get; set; }

        public int ApplicationId { get; set; }

        public string Status { get; set; } = string.Empty;

        public string Message { get; set; } = string.Empty;
    }
}
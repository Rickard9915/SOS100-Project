using Microsoft.EntityFrameworkCore;
using SOS100_NotificationService.Models;

namespace SOS100_NotificationService.Data
{
    public class NotificationDbContext : DbContext
    {
        public NotificationDbContext(DbContextOptions<NotificationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Notification> Notifications { get; set; }
    }
}
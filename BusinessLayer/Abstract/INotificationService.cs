using EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Abstract
{
    public interface INotificationService: IGenericService<Notification>
    {
        Task<List<Notification>> GetNotificationsForUserAsync(string userId, bool? isRead = null);
        Task<int> GetUnreadNotificationCountAsync(string userId);
        Task MarkNotificationAsReadAsync(int notificationId);
        Task MarkAllNotificationsAsReadAsync(string userId);
        Task CreateNotificationAsync(string userId, string fromUserId, NotificationType type, string title, int? relatedEntityId = null);
    }
}

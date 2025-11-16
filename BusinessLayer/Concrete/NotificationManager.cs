using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using EntityLayer.Entities;
using Microsoft.AspNetCore.SignalR;
using RealTimeLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Concrete
{
    public class NotificationManager : GenericManager<Notification>, INotificationService
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IHubContext<NotificationHub, INotificationClient> _hub;

        public NotificationManager(INotificationRepository notificationRepository, 
            IHubContext<NotificationHub, INotificationClient> hub) : base(notificationRepository)
        {
            _notificationRepository = notificationRepository;
            _hub = hub;
        }

        public async Task CreateNotificationAsync(string userId, string fromUserId, NotificationType type, string title, int? relatedEntityId = null)
        {
            var notification = new Notification
            {
                UserId = userId,
                FromUserId = fromUserId,
                Type = type,
                Title = title,
                EntityId = relatedEntityId,
                IsRead = false,
                CreatedAt = DateTime.UtcNow
            };
            await _notificationRepository.AddAsync(notification);
            await _hub.Clients.Users(userId).ReceiveNotification(notification);
        }

        public Task<List<Notification>> GetNotificationsForUserAsync(string userId, bool? isRead = null)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetUnreadNotificationCountAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public Task MarkAllNotificationsAsReadAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public Task MarkNotificationAsReadAsync(int notificationId)
        {
            throw new NotImplementedException();
        }
    }
}

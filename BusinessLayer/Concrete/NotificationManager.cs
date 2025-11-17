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

        public async Task<List<Notification>> GetNotificationsForUserAsync(string userId, bool? isRead = null)
        {
            return await _notificationRepository.GetNotificationsForUserAsync(userId, isRead);
        }

        public async Task<int> GetUnreadNotificationCountAsync(string userId)
        {
            return await _notificationRepository.GetUnreadNotificationCountAsync(userId);
        }

        public async Task MarkAllNotificationsAsReadAsync(string userId)
        {
            var notifications = await _notificationRepository.GetNotificationsForUserAsync(userId, false);
            foreach (var notification in notifications)
            {
                notification.IsRead = true;
                await _notificationRepository.UpdateAsync(notification);
            }
        }

        public async Task MarkNotificationAsReadAsync(int notificationId)
        {
            var notification = await _notificationRepository.GetByIdAsync(notificationId);
            if (notification != null && !notification.IsRead)
            {
                notification.IsRead = true;
                await _notificationRepository.UpdateAsync(notification);
            }
        }
    }
}

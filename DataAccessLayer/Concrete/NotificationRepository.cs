using DataAccessLayer.Abstract;
using DataAccessLayer.DbContext;
using EntityLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Concrete
{
    public class NotificationRepository : GenericRepository<Notification, AppDbContext>, INotificationRepository
    {
        public NotificationRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<List<Notification>> GetNotificationsForUserAsync(string userId, bool? isRead = null)
        {
            var query = _context.Notifications
                .Include(n => n.FromUser).Where(n => n.UserId == userId);
            if (isRead.HasValue)
            {
                query = query.Where(n => n.IsRead == isRead.Value);
            }
            return await query.OrderByDescending(n => n.CreatedAt).ToListAsync();
        }

        public async Task<int> GetUnreadNotificationCountAsync(string userId)
        {
            return await _context.Notifications
                .Where(n => n.UserId == userId && !n.IsRead)
                .CountAsync();
        }
    }
}

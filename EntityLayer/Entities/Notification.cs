using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Entities
{
    public enum NotificationType
    {
        Like = 0,
        Comment = 1,
        Follow = 2,
        FollowRequest = 3,
        Message = 4
    }
    public class Notification
    {
        public int Id { get; set; }
        public string UserId { get; set; } = null!;
        public AppUser User { get; set; } = null!;
        public string? FromUserId { get; set; }
        public AppUser? FromUser { get; set; }
        public NotificationType Type { get; set; }
        public int? EntityId { get; set; }
        public string? Title { get; set; }
        public bool IsRead { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}

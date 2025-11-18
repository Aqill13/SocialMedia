using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Entities
{
    public class AppUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? ImageUrl { get; set; }
        public string? Bio { get; set; }
        public bool IsPrivate { get; set; }
        public bool IsOnline { get; set; }
        public DateTime? LastActive { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public int ControleCode { get; set; }
        public int PostsCount { get; set; }
        public int FollowersCount { get; set; }
        public int FollowingCount { get; set; }

        public ICollection<Post> Posts { get; set; } = new List<Post>();
        public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
        public ICollection<UserFollow> Followers { get; set; } = new List<UserFollow>();
        public ICollection<UserFollow> Following { get; set; } = new List<UserFollow>();
        public ICollection<Message> SentMessages { get; set; } = new List<Message>();
        public ICollection<Message> ReceivedMessages { get; set; } = new List<Message>();
    }
}

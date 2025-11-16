using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Entities
{
    public class UserFollow
    {
        public int Id { get; set; }
        public string FollowerId { get; set; }
        public AppUser Follower { get; set; }
        public string FollowingId { get; set; }
        public AppUser Following { get; set; }
        public DateTime FollowedAt { get; set; } = DateTime.UtcNow;
    }
}

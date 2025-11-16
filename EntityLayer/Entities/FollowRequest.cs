using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Entities
{
    public enum FollowRequestStatus
    {
        Pending = 0,
        Accepted = 1,
        Rejected = 2,
    }
    public class FollowRequest
    {
        public int Id { get; set; }
        public string FromUserId { get; set; }
        public AppUser FromUser { get; set; }
        public string ToUserId { get; set; }
        public AppUser ToUser { get; set; }
        public FollowRequestStatus Status { get; set; } = FollowRequestStatus.Pending;
        public DateTime RequestedAt { get; set; } = DateTime.UtcNow;
        public DateTime? RespondedAt { get; set; }
    }
}

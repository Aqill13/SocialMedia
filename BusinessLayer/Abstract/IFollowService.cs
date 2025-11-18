using EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Abstract
{
    public interface IFollowService 
    {
        // Public account follow/unfollow
        Task<bool> FollowUserAsync(string currentUserId, string targetUserId);
        Task<bool> UnfollowUserAsync(string currentUserId, string targetUserId);

        // Private account follow request
        Task<FollowRequest?> SendFollowRequestAsync(string currentUserId, string targetUserId);
        Task<bool> AcceptFollowRequestAsync(int requestId);
        Task<bool> RejectFollowRequestAsync(int requestId);

        // Check follow status and lists
        Task<bool> IsFollowingAsync(string followerId, string followingId);
        Task<List<UserFollow>> GetFollowersAsync(string userId);
        Task<List<UserFollow>> GetFollowingsAsync(string userId);
    }
}

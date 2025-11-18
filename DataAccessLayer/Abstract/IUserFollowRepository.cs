using EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Abstract
{
    public interface IUserFollowRepository : IGenericRepository<UserFollow>
    {
        Task<bool> IsFollowingAsync(string followerId, string followingId);
        Task<List<UserFollow>> GetFollowersAsync(string userId);
        Task<List<UserFollow>> GetFollowingsAsync(string userId);
        Task<UserFollow?> GetFollowRecordAsync(string followerId, string followingId);
    }
}

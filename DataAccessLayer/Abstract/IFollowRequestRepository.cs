using EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Abstract
{
    public interface IFollowRequestRepository : IGenericRepository<FollowRequest>
    {
        Task<List<FollowRequest>> GetPendingRequestsForUserAsync(string toUserId);
        Task<FollowRequest?> GetPendingAsync(string fromUserId, string toUserId);
    }
}

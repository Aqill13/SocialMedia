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
    public class FollowRequestRepository : GenericRepository<FollowRequest, AppDbContext>, IFollowRequestRepository
    {
        public FollowRequestRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<FollowRequest?> GetPendingAsync(string fromUserId, string toUserId)
        {
            return await _context.FollowRequests
                .FirstOrDefaultAsync(fr => fr.FromUserId == fromUserId && fr.ToUserId == toUserId && fr.Status == FollowRequestStatus.Pending);
        }

        public async Task<List<FollowRequest>> GetPendingRequestsForUserAsync(string toUserId)
        {
            return await _context.FollowRequests
                .Where(fr => fr.ToUserId == toUserId && fr.Status == FollowRequestStatus.Pending)
                .ToListAsync();
        }
    }
}

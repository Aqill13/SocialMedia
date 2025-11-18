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
    public class UserFollowRepository : GenericRepository<UserFollow, AppDbContext>, IUserFollowRepository
    {
        public UserFollowRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<List<UserFollow>> GetFollowersAsync(string userId)
        {
            return await _context.UserFollows
                .Where(uf => uf.FollowingId == userId)
                .Include(uf => uf.Follower)
                .ToListAsync();
        }

        public Task<List<UserFollow>> GetFollowingsAsync(string userId)
        {
            return _context.UserFollows
                .Where(uf => uf.FollowerId == userId)
                .Include(uf => uf.Following)
                .ToListAsync();
        }

        public async Task<bool> IsFollowingAsync(string followerId, string followingId)
        {
            return await _context.UserFollows.AnyAsync(uf => uf.FollowerId == followerId && uf.FollowingId == followingId);
        }

    }
}

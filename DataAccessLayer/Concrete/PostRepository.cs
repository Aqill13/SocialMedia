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
    public class PostRepository : GenericRepository<Post, AppDbContext>, IPostRepository
    {
        public PostRepository(AppDbContext context) : base(context)
        {
        }

        public Task<List<Post>> GetFeedPostsAsync(string currentUserId)
        {
            var followedUserIds = _context.UserFollows
                .Where(f => f.FollowerId == currentUserId)
                .Select(f => f.FollowingId)
                .ToList();
            followedUserIds.Add(currentUserId); 
            return _context.Posts
                .Where(p => followedUserIds.Contains(p.UserId))
                .Include(p => p.AppUser)
                .Include(p => p.PostComments)
                .Include(p => p.PostLikes)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<Post>> GetUserPostsAsync(string userId)
        {
            return await _context.Posts
                .Where(p => p.UserId == userId)
                .Include(p => p.AppUser)
                .Include(p => p.PostComments)
                .Include(p => p.PostLikes)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }
    }
}

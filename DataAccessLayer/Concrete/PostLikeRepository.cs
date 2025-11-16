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
    public class PostLikeRepository : GenericRepository<PostLike, AppDbContext>, IPostLikeRepository
    {
        public PostLikeRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<bool> UserLikedPostAsync(string userId, int postId)
        {
            return await _context.PostLikes.AnyAsync(pl => pl.UserId == userId && pl.PostId == postId);
        }
    }
}

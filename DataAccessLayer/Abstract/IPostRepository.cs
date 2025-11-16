using EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Abstract
{
    public interface IPostRepository : IGenericRepository<Post>
    {
        Task<List<Post>> GetUserPostsAsync(string userId);
        Task<List<Post>> GetFeedPostsAsync(string currentUserId);
    }
}

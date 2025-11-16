using EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Abstract
{
    public interface IPostLikeRepository:IGenericRepository<PostLike>
    {
        Task<bool> UserLikedPostAsync(string userId, int postId);

    }
}

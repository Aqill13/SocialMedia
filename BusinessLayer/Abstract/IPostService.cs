using EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Abstract
{
    public interface IPostService : IGenericService<Post>
    {
        // Post
        Task<List<Post>> GetProfilePostsAsync(string profileUserId, string viewerId);
        Task<List<Post>> GetFeedPostsAsync(string currentUserId);
        Task<Post> CreatePostAsync(string userId, string? context, string? imageUrl);

        // Like
        Task<bool> LikePostAsync(int postId, string userId);
        Task<bool> UnlikePostAsync(int postId, string userId);
        Task<bool> HasUserLikedPostAsync(int postId, string userId);  
        Task<int> GetPostLikeCountAsync(int postId);

        // Comment
        Task<PostComment> AddCommentAsync(int postId, string userId, string content);
        Task DeleteCommentAsync(int commentId, string userId);
        Task<List<PostComment>> GetPostCommentsAsync(int postId);
    }
}

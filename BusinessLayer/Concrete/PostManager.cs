using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using EntityLayer.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Concrete
{
    public class PostManager : GenericManager<Post>, IPostService
    {
        private readonly IPostRepository _postRepository;
        private readonly IPostLikeRepository _postLikeRepository;
        private readonly IPostCommentRepository _postCommentRepository;
        private readonly INotificationService _notificationService;
        private readonly UserManager<AppUser> _userManager;
        private readonly IUserFollowRepository _userFollowRepository;

        public PostManager(IPostRepository postRepository, IPostLikeRepository postLikeRepository,
            IPostCommentRepository postCommentRepository, INotificationService notificationService,
            UserManager<AppUser> userManager, IUserFollowRepository userFollowRepository) : base(postRepository)
        {
            _postRepository = postRepository;
            _postLikeRepository = postLikeRepository;
            _postCommentRepository = postCommentRepository;
            _notificationService = notificationService;
            _userManager = userManager;
            _userFollowRepository = userFollowRepository;
        }

        // Post
        public async Task<Post> CreatePostAsync(string userId, string? context, string? imageUrl)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return null;
            var post = new Post
            {
                UserId = userId,
                Context = context,
                ImageUrl = imageUrl,
                CreatedAt = DateTime.UtcNow
            };
            await _postRepository.AddAsync(post);

            user.PostsCount += 1;
            await _userManager.UpdateAsync(user);

            return post;
        }

        public async Task<List<Post>> GetFeedPostsAsync(string currentUserId)
        {
            return await _postRepository.GetFeedPostsAsync(currentUserId);
        }

        public async Task<List<Post>> GetProfilePostsAsync(string profileUserId, string viewerId)
        {
            if (profileUserId == viewerId)
                return await _postRepository.GetUserPostsAsync(profileUserId);

            var profileUser = await _userManager.FindByIdAsync(profileUserId);
            if (profileUser == null)
                return new List<Post>();

            var isPrivate = profileUser.IsPrivate;
            if (!isPrivate)
                return await _postRepository.GetUserPostsAsync(profileUserId);
            else
            {
                var isFollowing = await _userFollowRepository.IsFollowingAsync(viewerId, profileUserId);
                if (isFollowing)
                    return await _postRepository.GetUserPostsAsync(profileUserId);
                else
                    return new List<Post>();
            }
        }

        // Like
        public async Task<int> GetPostLikeCountAsync(int postId)
        {
            var likes = await _postLikeRepository.GetAllAsync(pl => pl.PostId == postId);
            return likes.Count;
        }

        public async Task<bool> HasUserLikedPostAsync(int postId, string userId)
        {
            return await _postLikeRepository.UserLikedPostAsync(userId, postId);
        }

        public async Task<bool> LikePostAsync(int postId, string userId)
        {
            var existingLike = await _postLikeRepository.UserLikedPostAsync(userId, postId);
            if (existingLike)
                return false;
            var postLike = new PostLike
            {
                PostId = postId,
                UserId = userId,
                CreatedAt = DateTime.UtcNow
            };
            var post = await _postRepository.GetByIdAsync(postId);
            if (post == null)
                return false;
            var fromUser = await _userManager.FindByIdAsync(userId);
            if (fromUser == null)
                return false;
            await _postLikeRepository.AddAsync(postLike);
            await _notificationService.CreateNotificationAsync(
                post.UserId,
                userId,
                NotificationType.Like,
                $"{fromUser.UserName} liked your post.",
                postId
            );
            post.LikesCount += 1;
            await _postRepository.UpdateAsync(post);
            return true;
        }

        public async Task<bool> UnlikePostAsync(int postId, string userId)
        {
            var like = await _postLikeRepository.GetFirstAsync(pl => pl.PostId == postId && pl.UserId == userId);
            if (like == null)
                return false;
            await _postLikeRepository.DeleteAsync(like);
            var post = await _postRepository.GetByIdAsync(postId);
            if (post == null)
                return false;
            post.LikesCount -= 1;
            await _postRepository.UpdateAsync(post);
            return true;
        }

        // Comment
        public async Task<PostComment> AddCommentAsync(int postId, string userId, string content)
        {
            var post = await _postRepository.GetByIdAsync(postId);
            if (post == null)
                return null;
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return null;
            var comment = new PostComment
            {
                PostId = postId,
                UserId = userId,
                Comment = content,
                CreatedAt = DateTime.UtcNow
            };
            await _postCommentRepository.AddAsync(comment);
            post.CommentsCount += 1;
            await _postRepository.UpdateAsync(post);
            await _notificationService.CreateNotificationAsync(
                post.UserId,
                userId,
                NotificationType.Comment,
                $"{user.UserName} commented on your post.",
                postId
            );
            return comment;
        }

        public async Task DeleteCommentAsync(int commentId, string userId)
        {
            var comment = await _postCommentRepository.GetByIdAsync(commentId);
            if (comment == null || comment.UserId != userId)
                return;
            await _postCommentRepository.DeleteAsync(comment);
            var post = await _postRepository.GetByIdAsync(comment.PostId);
            if (post != null)
            {
                post.CommentsCount -= 1;
                await _postRepository.UpdateAsync(post);
            }
        }

        public async Task<List<PostComment>> GetPostCommentsAsync(int postId)
        {
            var comments = await _postCommentRepository.GetAllAsync(pc => pc.PostId == postId, "AppUser");
            return comments.OrderByDescending(c => c.CreatedAt).ToList();
        }
    }
}

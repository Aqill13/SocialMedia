using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Entities
{
    public class Post
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public AppUser AppUser { get; set; }
        public string? Context { get; set; }
        public string? ImageUrl { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public bool IsActive { get; set; }
        public int LikesCount { get; set; }
        public int CommentsCount { get; set; }
        public ICollection<PostLike> PostLikes { get; set; } = new List<PostLike>();
        public ICollection<PostComment> PostComments { get; set; } = new List<PostComment>();
    }
}

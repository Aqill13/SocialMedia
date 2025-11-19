using EntityLayer.Entities;

namespace WebUI.Areas.User.Models.Profile
{
    public class ProfilePostsViewModel
    {
        public AppUser ProfileUser { get; set; }
        public List<Post> Posts { get; set; }
        public string ActiveTopTab { get; set; } = "posts";
        public bool IsOwner { get; set; }
    }
}

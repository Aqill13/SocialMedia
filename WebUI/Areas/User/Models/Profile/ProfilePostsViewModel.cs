using EntityLayer.Entities;

namespace WebUI.Areas.User.Models.Profile
{
    public class ProfilePostsViewModel : ProfileBaseViewModel
    {
        public List<Post> Posts { get; set; }
    }
}

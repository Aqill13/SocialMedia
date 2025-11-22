using EntityLayer.Entities;

namespace WebUI.Areas.User.Models.Profile
{
    public class ProfileBaseViewModel
    {
        public AppUser ProfileUser { get; set; } = null!;
        public List<UserSocialLink> SocialLinks { get; set; } = new();
        public string ActiveTopTab { get; set; } = "post";

    }
}

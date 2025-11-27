using EntityLayer.Entities;

namespace WebUI.Areas.User.Models.Profile
{
    public class ProfileBaseViewModel
    {
        public AppUser ProfileUser { get; set; } = null!;
        public List<UserSocialLink> SocialLinks { get; set; } = new();
        public string ActiveTopTab { get; set; } = "post";
        public bool IsFriend { get; set; }
        public bool IsOwner { get; set; }
        public Dictionary<ProfileField, VisibilityLevel> VisibilitySettings { get; set; } = new Dictionary<ProfileField, VisibilityLevel>();
        public ProfileVisibilityViewModel? VisibilityViewModel { get; set; } = new ProfileVisibilityViewModel();


    }
}

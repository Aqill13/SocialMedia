using EntityLayer.Entities;

namespace WebUI.Areas.User.Models.Profile
{
    public class ProfileAboutViewModel : ProfileBaseViewModel
    {
        public string ActiveSection { get; set; } = "profile";   
        public string ActiveSubSection { get; set; } = "personalInfo";
        public bool IsOwner { get; set; }
        public bool IsFriend { get; set; }
        public Dictionary<ProfileField, VisibilityLevel> VisibilitySettings { get; set; } = new Dictionary<ProfileField, VisibilityLevel>();
        public ProfileVisibilityViewModel? VisibilityViewModel { get; set; } = new ProfileVisibilityViewModel();
    }
}

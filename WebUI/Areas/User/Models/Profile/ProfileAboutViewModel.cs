using EntityLayer.Entities;

namespace WebUI.Areas.User.Models.Profile
{
    public class ProfileAboutViewModel : ProfileBaseViewModel
    {
        public string ActiveSection { get; set; } = "profile";   
        public string ActiveSubSection { get; set; } = "personalInfo";
    }
}

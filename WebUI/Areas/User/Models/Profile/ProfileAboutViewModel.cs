using EntityLayer.Entities;

namespace WebUI.Areas.User.Models.Profile
{
    public class ProfileAboutViewModel
    {
        public AppUser ProfileUser { get; set; }
        public string ActiveTopTab { get; set; } = "about";
        public string ActiveSection { get; set; } = "profile";   
        public string ActiveSubSection { get; set; } = "personalInfo";
        public bool IsOwner { get; set; } 
    }
}

using EntityLayer.Entities;

namespace WebUI.Areas.User.Models.Profile
{
    public class ProfileVisibilityItemViewModel
    {
        public ProfileField Field { get; set; }
        public VisibilityLevel Visibility { get; set; }
        public string Label { get; set; }
        public string Group { get; set; }
    }
}

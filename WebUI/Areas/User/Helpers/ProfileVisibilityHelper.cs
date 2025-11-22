using EntityLayer.Entities;
using WebUI.Areas.User.Models.Profile;

namespace WebUI.Areas.User.Helpers
{
    public class ProfileVisibilityHelper
    {
        public static bool CanViewAboutInfo(ProfileAboutViewModel model, ProfileField field)
        {
            if (model == null) return false;
            if (model.IsOwner) return true;

            if (!model.VisibilitySettings.TryGetValue(field, out var visibilityLevel))
            {
                visibilityLevel = VisibilityLevel.Everyone;
            }
            return visibilityLevel switch
            {
                VisibilityLevel.Everyone => true,
                VisibilityLevel.Friends => model.IsFriend,
                VisibilityLevel.OnlyMe => false,
                _ => false,
            };
        }
    }
}

using EntityLayer.Entities;

namespace WebUI.Areas.User.Models.Profile
{
    public class ProfileFriendsViewModel : ProfileBaseViewModel
    {
        public string ActiveSection { get; set; } = "friends";
        public List<UserFollow> Followers { get; set; } = new();
        public List<UserFollow> Following { get; set; } = new();
        public List<UserFollow> MutualUsers { get; set; } = new();
    }
}

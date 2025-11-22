using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Entities
{
    public class UserProfileVisibility
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public AppUser User { get; set; }
        public ProfileField Field { get; set; }
        public VisibilityLevel Visibility { get; set; } = VisibilityLevel.Everyone;

    }
    public enum VisibilityLevel
    {
        Everyone = 1,
        Friends = 2,
        OnlyMe = 3
    }

    public enum ProfileField
    {
        Bio = 1,
        Location = 2,
        BirthDate = 3,
        Gender = 4,
        Hobbies = 5,
        FavoriteMovies = 6,
        FavoriteGames = 7,
        FavoriteBooks = 8,
        Birthplace = 9,
        LivesIn = 10,
        Status = 11,
        PhoneNumber = 12,
        SocialLinks = 20,
        WorkExperience = 30,
        Education = 40
    }
}

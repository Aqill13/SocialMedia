using EntityLayer.Entities;
using System.ComponentModel.DataAnnotations;

namespace WebUI.Areas.User.Models.Profile
{
    public class ProfileEditViewModel
    {
        // Basic Information
        [Required(ErrorMessage = "Firstname is required")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Lastname is required")]
        public string LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public IFormFile? ProfileImageFile { get; set; }
        public IFormFile? CoverImageFile { get; set; }
        public string? ImageUrl { get; set; }
        public string? CoverImageUrl { get; set; }
        public string? Bio { get; set; }
        public string? Location { get; set; }

        [DataType(DataType.Date)]
        public DateTime? BirthDate { get; set; }
        public string? Birthplace { get; set; }
        public string? LivesIn { get; set; }
        public string? Status { get; set; }
        public string? Gender { get; set; }

        // Hobbies and Interests
        public string? Hobbies { get; set; }
        public string? FavoriteMovies { get; set; }
        public string? FavoriteGames { get; set; }
        public string? FavoriteBooks { get; set; }

        // Social Media Links
        public List<UserSocialLink> SocialLinks { get; set; } = new List<UserSocialLink>();

        // Educations
        public List<UserEducation> Educations { get; set; } = new List<UserEducation>();

        // Work Experiences
        public List<UserWorkExperience> WorkExperiences { get; set; } = new List<UserWorkExperience>();
    }
}

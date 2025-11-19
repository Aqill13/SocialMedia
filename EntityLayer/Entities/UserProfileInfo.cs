using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Entities
{
    public class UserProfileInfo
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public AppUser User { get; set; }
        public string? Bio { get; set; }
        public string? Location { get; set; }
        public DateTime? BirthDate { get; set; }
        public string? Gender { get; set; }
        public string? Hobbies { get; set; }
        public string? FavoriteMovies { get; set; }
        public string? FavoriteGames { get; set; }
        public string? FavoriteBooks { get; set; }
        public string? Birthplace { get; set; }
        public string? LivesIn { get; set; }
        public string? Status { get; set; }
    }
}

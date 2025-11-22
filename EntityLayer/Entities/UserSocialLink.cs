using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Entities
{
    public class UserSocialLink
    {
        public int Id { get; set; }
        public string UserId { get; set; } = null!;
        public AppUser User { get; set; } = null!;
        public SocialPlatform Platform { get; set; }
        public string Url { get; set; }
        public bool IsVisible { get; set; } = true;
        public enum SocialPlatform { 
            Instagram = 1, 
            Facebook = 2, 
            Twitter = 3, 
            Github = 4, 
            Linkedin = 5, 
            Youtube = 6 
        }
    }
}

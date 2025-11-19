using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Entities
{
    public class UserEducation
    {
        public int Id { get; set; }

        public string UserId { get; set; }
        public AppUser User { get; set; }
        public string SchoolName { get; set; }
        public string Degree { get; set; }  
        public string Field { get; set; }    
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}

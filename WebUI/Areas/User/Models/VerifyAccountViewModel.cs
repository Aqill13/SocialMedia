using System.ComponentModel.DataAnnotations;

namespace WebUI.Areas.User.Models
{
    public class VerifyAccountViewModel
    {
        public string Email { get; set; }
        public string FullName { get; set; }

        [Required(ErrorMessage = "Enter the code sent to your email")]
        public int ControleCode { get; set; }
    }
}

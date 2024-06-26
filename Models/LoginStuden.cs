using System.ComponentModel.DataAnnotations;

namespace Scheduler.Models
{
    public class LoginStuden
    {

        [Required(ErrorMessage = "please fill data")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "please fill data")]
        public string password { get; set; }
    }
}

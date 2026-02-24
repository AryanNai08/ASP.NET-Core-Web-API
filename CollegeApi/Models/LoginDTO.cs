using System.ComponentModel.DataAnnotations;

namespace CollegeApi.Models
{
    public class LoginDTO
    {
        public string Policy { get; set; }

        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}

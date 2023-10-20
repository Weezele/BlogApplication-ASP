using System.ComponentModel.DataAnnotations;

namespace BlogApplication.UI.Models.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        public string? UserName { get; set; }

        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        [MinLength(6, ErrorMessage = "Password has to be at least 6 charcters ")]
        public string? Password { get; set; }
    }
}

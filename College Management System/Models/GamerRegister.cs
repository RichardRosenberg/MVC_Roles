using System.ComponentModel.DataAnnotations;

namespace College_Management_System.Models
{
    public class GamerRegister
    {
        [Required]
        [RegularExpression(@"^\d{1}[A-Za-z]{1}$", ErrorMessage = "GamerId must be 2 characters with 1 number followed by 1 letter.")]
        public string? GamerId { get; set; }

        [Required]
        [MaxLength(10)]
        [RegularExpression(@"^[a-zA-Z0-9]*$", ErrorMessage = "Username must be letters or numbers with a maximum length of 10 characters.")]
        public string? Username { get; set; }

        [Required]
        [Range(18, 45, ErrorMessage = "Age must be between 18 and 45.")]
        public int Age { get; set; }

        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string? ConfirmPassword { get; set; }
    }
}

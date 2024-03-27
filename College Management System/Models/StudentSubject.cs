using System.ComponentModel.DataAnnotations;
namespace College_Management_System
{
    public class StudentSubject
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Code is required")]
        [RegularExpression("^[A-Za-z]{2}[0-9]{2}$", ErrorMessage = "Code must be 2 letters followed by 2 numbers")]
        public string? Code { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [MaxLength(25, ErrorMessage = "Description cannot exceed 25 characters")]
        [RegularExpression("^[A-Za-z ]+$", ErrorMessage = "Description must contain letters only")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Status is required")]
        public string? Status { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;

namespace College_Management_System.Models
{
	public class StaffRegister
	{
		[Required]
		[RegularExpression(@"^(514|438)\d{7}$", ErrorMessage = "Phone number must start with 514 or 438 and be 10 digits long.")]
		public string? PhoneNumber { get; set; }

		[Required]
		[RegularExpression(@"^[A-Za-z]{1,15}$", ErrorMessage = "Name must be letters and maximum 15 characters.")]
		public string? Name { get; set; }

		[Required]
		[RegularExpression(@"^[A-Za-z]{1,15}$", ErrorMessage = "Surname must be letters and maximum 15 characters.")]
		public string? Surname { get; set; }

		[Required]
		[RegularExpression(@"^\d{3}[A-Za-z]{2}$", ErrorMessage = "StaffId must be 5 characters with 3 numbers followed by 2 letters.")]
		public string? StaffId { get; set; }

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

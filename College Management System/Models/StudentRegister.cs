using System.ComponentModel.DataAnnotations;

public class StudentRegister
{
    [Required]
    [StringLength(6)]
    public string? StudentId { get; set; }

    [Required]
    [StringLength(10)]
    public string? Name { get; set; }

    [Required]
    [StringLength(10)]
    public string? Surname { get; set; }

    [Required]
    public string? Address { get; set; }

    [Required]
    public string? PostalCode { get; set; }

    [Required]
    [EmailAddress]
    public string? Email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string? Password { get; set; }

    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
    public string? ConfirmPassword { get; set; }
}

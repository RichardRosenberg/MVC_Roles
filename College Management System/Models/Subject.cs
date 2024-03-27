using System.ComponentModel.DataAnnotations;

public class Subject
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Code is required")]
    public string? Code { get; set; }

    [Required(ErrorMessage = "Description is required")]
    public string? Description { get; set; }

    [Required(ErrorMessage = "Location is required")]
    public string? Location { get; set; }

    [Required(ErrorMessage = "Status is required")]
    public string? Status { get; set; }
}

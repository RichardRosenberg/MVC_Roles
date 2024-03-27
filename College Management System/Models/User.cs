using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace College_Management_System.Models
{
    public class User : IdentityUser
    {
        //student
        public string? StudentId { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? Address { get; set; }
        public string? PostalCode { get; set; }

        //staff
        public string? StaffId { get; set; }
		public string? PhoneNumber { get; set; }

        //gamer
        public string? GamerId { get; set; } 
        public int Age { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace MvcAdoDemo.Models
{
    public class Account
    {
        public int UserId { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string Role { get; set; } = "student";

        public string StudentId { get; set; }

        public string Name { get; set; }

        public string Gender { get; set; }

        public string City { get; set; }
    }
}

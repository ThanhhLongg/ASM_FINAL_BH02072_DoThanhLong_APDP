using System.ComponentModel.DataAnnotations;
namespace MvcAdoDemo.Models
{
    public class User
        {
            public int UserID { get; set; }

            [Required]
            public string Username { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            public string Role { get; set; } // "teacher" hoặc "student"
        }
}
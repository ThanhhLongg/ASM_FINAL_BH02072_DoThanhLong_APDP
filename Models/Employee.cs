using System.ComponentModel.DataAnnotations;

namespace MvcAdoDemo.Models
{
    public class Employee
    {
        public int ID { get; set; }

        public int UserID { get; set; }
        [Required]
        public string Name { get; set; }

        public string Gender { get; set; }

        public string Department { get; set; }

        public string City { get; set; }
    }
}

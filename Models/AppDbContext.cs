using Microsoft.EntityFrameworkCore;

namespace MvcAdoDemo.Models
{
    public class AppDbContext : DbContext
    {
        public DbSet<Employee> Employees { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        
        public DbSet<User> Users { get; set; }

    }
    
}

using Microsoft.EntityFrameworkCore;

namespace SendMailProject.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Models.RegisterViewModel> Users { get; set; }
    }
}

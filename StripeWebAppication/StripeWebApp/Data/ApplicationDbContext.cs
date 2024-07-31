using Microsoft.EntityFrameworkCore;
using StripeWebApp.Models;

namespace StripeWebApp.Data
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Product>Products { get; set; }
    }
}

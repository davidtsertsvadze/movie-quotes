using Microsoft.EntityFrameworkCore;
using MovieQuotesAPI.Models;

namespace MovieQuotesAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Quote> Quotes { get; set; }
        public DbSet<Admin> Admins { get; set; }
    }
  
}

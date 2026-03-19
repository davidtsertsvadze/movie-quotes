using Microsoft.EntityFrameworkCore;

namespace MovieQuotesAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
    }
  
}

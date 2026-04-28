using Microsoft.EntityFrameworkCore;
using Sentinel.Models;
namespace Sentinel.Data
{
    public class SentinelDbContext : DbContext
    {
        public SentinelDbContext(DbContextOptions<SentinelDbContext> options) : base(options)
        {

        }

        public DbSet<SentinelUser> SentinelUsers { get; set; }
    }
}

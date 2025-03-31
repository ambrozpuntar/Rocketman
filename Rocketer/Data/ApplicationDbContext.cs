using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Rocketer.Data.Models.Database;

namespace Rocketer.Data;
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
    public DbSet<Launch> Launches { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Launch>().HasKey(x => x.Id);
    }
}

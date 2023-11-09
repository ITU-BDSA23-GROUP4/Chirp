using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Chirp.Infrastructure;

public class ChirpDBContext : DbContext
{
    public DbSet<Cheep> Cheeps { get; set; }
    public DbSet<Author> Authors { get; set; }

    public ChirpDBContext()
    {
    }
    public ChirpDBContext(DbContextOptions<ChirpDBContext> options) : base(options)
    {
    }
     protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlServer();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //Sets the max length, and makes sure that the text field is not empty for the Cheep
        modelBuilder.Entity<Cheep>().Property(C => C.Text).HasMaxLength(160);
        modelBuilder.Entity<Cheep>().Property(C => C.Text).IsRequired(true);
        modelBuilder.Entity<Author>().Property(a => a.Name).HasMaxLength(50);
        modelBuilder.Entity<Author>().Property(a => a.Email).HasMaxLength(50);
        modelBuilder.Entity<Author>().HasIndex(a => a.Name).IsUnique(true);
        modelBuilder.Entity<Author>().HasIndex(a => a.Email).IsUnique(true);
    }
}
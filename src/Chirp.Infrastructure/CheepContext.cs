using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Chirp.Infrastructure;
public class ChirpDBContext : DbContext
{
    public DbSet<Cheep> Cheeps { get; set; }
    public DbSet<Author> Authors { get; set; }
    public DbSet<Follow> Follows { get; set; }
    public string? DbPath { get; }

    public ChirpDBContext() //Finds the current db in temp folder
    {
        if (String.IsNullOrEmpty(Environment.GetEnvironmentVariable("CHIRPDBPATH")))
        {
            DbPath = Path.GetTempPath() + "chirp.db";
        }
        else
        {
            DbPath = Environment.GetEnvironmentVariable("CHIRPDBPATH");
        }
    }
    public ChirpDBContext(string repoName) //If wanting to create a db
    {
        if (String.IsNullOrEmpty(Environment.GetEnvironmentVariable("CHIRPDBPATH")))
        {
            DbPath = Path.GetTempPath() + repoName +".db";
        }
        else
        {
            DbPath = Environment.GetEnvironmentVariable("CHIRPDBPATH");
        }


    }

    public ChirpDBContext(DbContextOptions<ChirpDBContext> options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={DbPath}");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //Sets the max length, and makes sure that the text field is not empty for the Cheep
        modelBuilder.Entity<Cheep>()
            .Property(C => C.Text)
            .HasMaxLength(160);
        modelBuilder.Entity<Cheep>()
            .Property(C => C.Text)
            .IsRequired(true);
        modelBuilder.Entity<Author>()
            .Property(a => a.Name)
            .HasMaxLength(50);
        modelBuilder.Entity<Author>()
            .Property(a => a.Email)
            .HasMaxLength(50);
        modelBuilder.Entity<Author>()
            .HasIndex(a => a.Name)
            .IsUnique(true);
        modelBuilder.Entity<Author>()
            .HasIndex(a => a.Email)
            .IsUnique(true);
        // The configuration was made with the help of:
        //https://stackoverflow.com/questions/49214748/many-to-many-self-referencing-relationship
        modelBuilder.Entity<Follow>()
            .HasOne(f => f.Follower)
            .WithMany(a => a.Followed)
            .HasForeignKey(f => f.FollowerId)
            .OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Follow>()
            .HasOne(f => f.Followee)
            .WithMany(a => a.Followers)
            .HasForeignKey(f => f.FolloweeId);
    }
}
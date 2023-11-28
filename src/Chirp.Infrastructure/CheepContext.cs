using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Chirp.Infrastructure;
public class ChirpDBContext : DbContext
{
    public DbSet<Cheep> Cheeps { get; set; }
    public DbSet<Author> Authors { get; set; }
    public DbSet<Follow> Follows { get; set; }

    public ChirpDBContext(DbContextOptions options) : base(options)
    { 
    }

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
            .HasForeignKey(f => f.AuthorId)
            .OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Follow>()
            .HasOne(f => f.Author)
            .WithMany(a => a.Followers)
            .HasForeignKey(f => f.FolloweeId);
        modelBuilder.Entity<Follow>()
            .HasKey(f => new {f.AuthorId, f.FolloweeId});
    }
}
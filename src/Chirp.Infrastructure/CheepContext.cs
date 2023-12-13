using Microsoft.EntityFrameworkCore;

/*
<Summary>
This is the DBContext, which is the main class that we use to work with our database.
This is where we set restrains on our entities and have dbSets for our entities.
</Summary>
*/

namespace Chirp.Infrastructure;

public class ChirpDBContext : DbContext
{
    public DbSet<Cheep> Cheeps { get; set; }
    public DbSet<Author> Authors { get; set; }

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
    }
}
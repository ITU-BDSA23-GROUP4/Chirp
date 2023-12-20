using Microsoft.EntityFrameworkCore;

/*
<Summary>
This is the DBContext
It is instanciated every time we query or save changes to an entity in the database.
Here we apply custom constrains to the database.
</Summary>
*/
namespace Chirp.Infrastructure;

public class ChirpDBContext : DbContext
{
    /* A collection of Cheep objcts, 
    each representing a tuple (row) from the Cheep entity. */
    public DbSet<Cheep> Cheeps { get; set; }
    /* A collection of Author objects, 
    each representing a tuple (row) from the Author entity */
    public DbSet<Author> Authors { get; set; }

    public ChirpDBContext(DbContextOptions options) : base(options)
    { 
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Defining constraints on the entities at the database level
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
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace CheepDB;

public class ChirpDBContext : DbContext
{
    public DbSet<Cheep> Cheeps { get; set; }
    public DbSet<Author> Authors { get; set; }

    public string? DbPath { get; }



    public ChirpDBContext()
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
    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={DbPath}");
}

public class Author
{
    public int AuthorId { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required List<Cheep> Cheeps;

}

public class Cheep
{
    public int CheepId { get; set; }
    public required Author Author { get; set; }
    public required string Text { get; set; }
    public required DateTime TimeStamp { get; set; }


}
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace CheepDB;

public class ChirpDBContext : DbContext
{
    public DbSet<Cheep> Cheeps { get; set; }
    public DbSet<Author> Authors { get; set; }

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

/*Cheep DTO is the information that we want the client to know
In the future, this will by example not include password*/
public class CheepDTO
{
    public required int AuthorId { get; set; }
    public required string Author { get; set; }
    public required string Message { get; set; }
    public required string Timestamp { get; set; }
}
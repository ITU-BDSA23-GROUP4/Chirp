using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

public class CheepContext : DbContext
{
    public DbSet<Message> messages;
    public DbSet<User> users;

    public string DbPath { get; }



    public CheepContext()
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

public class User
{
    public int UserId { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string PWHash { get; set; }

}

public class Message
{
    public int MessageId { get; set; }
    public int AuthorId { get; set; }
    public string text { get; set; }
    public string pubDate { get; set; }

    public User Author { get; set; }
}
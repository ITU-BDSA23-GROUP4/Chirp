using Microsoft.Data.Sqlite;
using SQLDB;
using System.Globalization;



DB db = DB.GetInstance();

int authorID;


Task<IEnumerable<Cheep>> GetCheepsByAuthorAsync(int authorID)
{
    return Task.Run(() => GetCheepsByAuthor(authorID));
}

Task<IEnumerable<Cheep>> GetCheepsAsync()
{
    return Task.Run(() => GetCheeps());
}

Task AddCheepAsync(Cheep cheep, int authorID)
{
    return Task.Run(() => AddCheep(cheep, authorID));
}

IEnumerable<Cheep> GetCheepsByAuthor(int authorID)
{
    List<Cheep> returnList = new List<Cheep>();
    SqliteConnection? connection = db.GetConnection();
    if (connection != null)
    {
        SqliteCommand command = connection.CreateCommand();
        command.CommandText = @"
            SELECT U.username, M.text, M.pub_date
            FROM user U, message M
            WHERE U.user_id = $authorID
        ";
        command.Parameters.AddWithValue("$authorID", authorID);

        using (var reader = command.ExecuteReader())
        {
            while (reader.Read())
            {
                returnList.Add(new Cheep { Author = reader.GetString(0), Message = reader.GetString(1), Timestamp = reader.GetInt64(2) });
            }
        }
    }
    return returnList;
}

IEnumerable<Cheep> GetCheeps()
{
    List<Cheep> returnList = new List<Cheep>();
    SqliteConnection? connection = db.GetConnection();
    if (connection != null)
    {
        SqliteCommand command = connection.CreateCommand();

        command.CommandText = @"
            SELECT U.username, M.text, M.pub_date
            FROM user U, message M
            WHERE U.user_id = M.author_id
        ";

        using (var reader = command.ExecuteReader())
        {
            while (reader.Read())
            {
                returnList.Add(new Cheep { Author = reader.GetString(0), Message = reader.GetString(1), Timestamp = reader.GetInt64(2) });
            }
        }
    }
    return returnList;
}

async void AddCheep(Cheep cheep, int authorID)
{
    List<Cheep> returnList = new List<Cheep>();
    SqliteConnection? connection = db.GetConnection();
    if (connection != null)
    {
        SqliteCommand command = connection.CreateCommand();

        command.CommandText = @"
            INSERT INTO message (authorid, text, pub_date)
            ADD VALUES $authorID, $message, $timeStamp
        ";
        command.Parameters.AddWithValue("$authorID", authorID);
        command.Parameters.AddWithValue("$txtString", cheep.Message);
        command.Parameters.AddWithValue("$timeStamp", cheep.Timestamp);
    }
}

public record Cheep
{
    // [Index(0)]
    public string Author { get; set; }
    // [Index(1)]
    public string Message { get; set; }
    // [Index(2)]
    public long Timestamp { get; set; }
    public override string ToString()
    {
        var printMessage = Message.Replace("/comma/", ","); //Replaces what we did earlier, for a cleaner output
                                                            //Changing cutlture date and time format from this source: https://code-maze.com/csharp-datetime-format/
        return $"{Author} @ {CreateTimeString(Timestamp)}: {printMessage}";
    }

    private string CreateTimeString(long TimeStamp)
    {
        DateTimeOffset utcTime = DateTimeOffset.FromUnixTimeSeconds(Timestamp);
        return utcTime.ToLocalTime().ToString("dd/MM/yyyy HH:mm:ss", new CultureInfo("sw-SW"));
    }
}

namespace SQLDB
{
    public class  DB {
        
        private static readonly DB instance = new DB();
        private static SqliteConnection? connection;
        
        private DB() {
            connection = new SqliteConnection("Data Source=../../data/chirp.db");
            connection.Open();
        }

        public static DB GetInstance() {
            return instance;
        }

        public SqliteConnection? GetConnection() {
            return connection;
        }
    }
}

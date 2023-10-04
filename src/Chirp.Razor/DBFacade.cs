using Microsoft.Data.Sqlite;
using CheepRecord;

namespace SQLDB
{
    public class DB {
        
        private static readonly DB instance = new DB();
        private static SqliteConnection? connection;
        
        private DB() {
            connection = new SqliteConnection("Data Source=../../data/chirp.db");
            connection.Open();
        }

        public static DB GetInstance() {
            return instance;
        }

        public Task AddCheepAsync(Cheep cheep, int authorID)
        {
            return Task.Run(() => AddCheep(cheep, authorID));
        }

        public Task<IEnumerable<Cheep>> GetCheepsAsync()
        {
            return Task.Run(() => GetCheeps());
        }

        public Task<IEnumerable<Cheep>> GetCheepsByAuthorAsync(int authorID)
        {
            return Task.Run(() => GetCheepsByAuthor(authorID));
        }

        public void AddCheep(Cheep cheep, int authorID)
        {
            //SqliteConnection? connection = instance.GetConnection();
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

        public IEnumerable<Cheep> GetCheeps()
        {
            List<Cheep> returnList = new List<Cheep>();
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

        public IEnumerable<Cheep> GetCheepsByAuthor(int authorID)
        {
            List<Cheep> returnList = new List<Cheep>();
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
    }
}
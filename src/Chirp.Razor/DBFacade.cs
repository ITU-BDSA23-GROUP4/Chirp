using Microsoft.Data.Sqlite;
using CheepRecord;

namespace SQLDB
{
    public class DB {
        
        private static readonly DB instance = new DB();
        private static SqliteConnection? connection;
        
        private DB() {
            if(!File.Exists("../../data/chirp.db")) {
                // https://stackoverflow.com/questions/46084560/how-do-i-create-sqlite-database-files-in-net-core
            }
            connection = new SqliteConnection("Data Source=../../data/chirp.db");
            connection.Open();
        }

        public static DB GetInstance() {
            return instance;
        }

        public Task AddCheepAsync(CheepViewModel cheep, int authorID)
        {
            return Task.Run(() => AddCheep(cheep, authorID));
        }

        public Task<List<CheepViewModel>> GetCheepsAsync()
        {
            return Task.Run(() => GetCheeps());
        }

        public Task<List<CheepViewModel>> GetCheepsByAuthorAsync(int authorID)
        {
            return Task.Run(() => GetCheepsByAuthor(authorID));
        }

        public void AddCheep(CheepViewModel cheep, int authorID)
        {
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

        public List<CheepViewModel> GetCheeps()
        {
            List<CheepViewModel> returnList = new List<CheepViewModel>();
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
                        returnList.Add(new CheepViewModel(reader.GetString(0), reader.GetString(1), reader.GetString(2)));
                    }
                }
            }
            return returnList;
        }

        public List<CheepViewModel> GetCheepsByAuthor(int authorID)
        {
            List<CheepViewModel> returnList = new List<CheepViewModel>();
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
                        returnList.Add(new CheepViewModel(reader.GetString(0), reader.GetString(1), reader.GetString(2)));
                    }
                }
            }
            return returnList;
        }
    }
}
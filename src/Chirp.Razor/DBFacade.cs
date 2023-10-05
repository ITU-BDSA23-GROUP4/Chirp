using Microsoft.Data.Sqlite;
using CheepRecord;
using Microsoft.Extensions.FileProviders;
using System.Reflection;

namespace SQLDB
{
    public class DB
    {

        string sqlDBFilePath;
        private static readonly DB instance = new DB();
        private static SqliteConnection? connection;

        

        private DB()
        {
            if(String.IsNullOrEmpty(Environment.GetEnvironmentVariable("CHIRPDBPATH"))){
                sqlDBFilePath = Path.GetTempPath() + "chirp.db";
            } else 
            {
                sqlDBFilePath = Environment.GetEnvironmentVariable("CHIRPDBPATH");
            }
            Console.WriteLine(sqlDBFilePath);
            if (!File.Exists(sqlDBFilePath))
            {
                // string schemaScirpt = File.ReadAllText("../../data/schema.sql");
                // string dumpScirpt = File.ReadAllText("../../data/dump.sql");
                var embeddedProvider = new EmbeddedFileProvider(Assembly.GetExecutingAssembly());
                using var reader = embeddedProvider.GetFileInfo("schema.sql").CreateReadStream();
                using var sr = new StreamReader(reader);
                var schemaScript = sr.ReadToEnd();
                using var reader2 = embeddedProvider.GetFileInfo("dump.sql").CreateReadStream();
                using var sr2 = new StreamReader(reader2);
                var dumpScript = sr2.ReadToEnd();
                
                Console.WriteLine(schemaScript);
                // https://stackoverflow.com/questions/46084560/how-do-i-create-sqlite-database-files-in-net-core
                connection = new SqliteConnection($"Data Source={sqlDBFilePath}");
                
                    connection.Open();

                    SqliteCommand command = connection.CreateCommand();
                    command.CommandText = schemaScript;
                    command.ExecuteNonQuery();
                    
                    command.CommandText = dumpScript;
                    command.ExecuteNonQuery();


                
            }
            else
            {
                connection = new SqliteConnection($"Data Source={sqlDBFilePath}");
                connection.Open();
            }

        }

        public static DB GetInstance()
        {
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

        public Task<List<CheepViewModel>> GetCheepsByAuthorAsync(string authorID)
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
                    ORDER BY M.pub_date DESC
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

        public List<CheepViewModel> GetCheepsByAuthor(string authorID)
        {
            List<CheepViewModel> returnList = new List<CheepViewModel>();
            if (connection != null)
            {
                SqliteCommand command = connection.CreateCommand();
                command.CommandText = @"
                    SELECT U.username, M.text, M.pub_date
                    FROM user U, message M
                    WHERE M.author_id = (
                        SELECT user_id FROM User
                        WHERE username = $authorID
                    ) AND U.username = $authorID
                    ORDER BY M.pub_date DESC
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
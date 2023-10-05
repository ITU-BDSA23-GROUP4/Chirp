using Microsoft.Data.Sqlite;
using CheepRecord;
using Microsoft.Extensions.FileProviders;
using System.Reflection;

namespace SQLDB
{
    public class DB
    {

        string? sqlDBFilePath;
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
            if (!File.Exists(sqlDBFilePath))
            {
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

        public SqliteDataReader? Query(string sql, Dictionary<String, String>? parameters = null) {
            if (connection != null) {
                SqliteCommand command = connection.CreateCommand();

                command.CommandText = sql;

                if (parameters != null) {
                    foreach (KeyValuePair<string, String> parameter in parameters) {
                        command.Parameters.AddWithValue(parameter.Key, parameter.Value);
                        Console.WriteLine($"key {parameter.Key}, value: {parameter.Value}");
                    }
                }

                return command.ExecuteReader();
            }
            return null;
        }
    }
}
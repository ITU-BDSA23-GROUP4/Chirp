using Microsoft.Data.Sqlite;

namespace SQLDB {
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
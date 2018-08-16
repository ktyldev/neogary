using System;
using MySql.Data.MySqlClient;

namespace neogary
{
    public class DatabaseService 
    {
        private ILogService _log;

        public DatabaseService(string connectionString, ILogService log)
        {
            _log = log;

            var conn = new MySqlConnection();
            conn.ConnectionString = connectionString;
            conn.Open();

            _log.Log("Database connection OK");

            conn.Close();
        }
    }
}

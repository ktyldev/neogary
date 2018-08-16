using MySql.Data.MySqlClient;

namespace neogary
{
    public class DataAccess
    {
        private ILogger _log;

        public DataAccess(string connectionString, ILogger log)
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

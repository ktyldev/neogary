using System;
using System.Data;
using MySql.Data.MySqlClient;

namespace neogary
{
    public class DatabaseService : IDataService
    {
        private string _connectionString;
        private ILogService _log;

        public DatabaseService(string connectionString, ILogService log)
        {
            _log = log;
            _connectionString = connectionString;

            // test the connection
            OpenConnection(_ => _log.Log("DB connection OK"));
        }

        private void OpenConnection(Action<MySqlConnection> useConn)
        {
            var conn = new MySqlConnection();
            conn.ConnectionString = _connectionString;

            try
            {
                conn.Open();
                useConn(conn);
            }
            catch (Exception ex)
            {
                _log.Log(ex.Message); 
            }

            conn.Close();
        }

        public void Find(string table, string condition, Action<IDataRecord> readRecord)
        {
            var query = String.Format(
                "select * from {0} where {1}", 
                table, 
                condition);

            OpenConnection(c =>
            {
                var command = new MySqlCommand(query, c);
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    readRecord((IDataRecord)reader);
                }
            });
        }

        public int Insert(string table, string columns, string values)
        {
            throw new Exception();
        }

        public int Update(string table, string setValues, string condition)
        {
            throw new Exception();
        }
    }
}

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
            bool connectionOk = false;
            OpenConnection(_ =>
            {
                _log.Log("DB connection OK");
                connectionOk = true;    
            });

            if (!connectionOk)
            {
                _log.Log("Unable to connect to database, exiting...");
                Environment.Exit(1);
            }
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

        public int Find(string table, string condition, Action<IDataRecord> readRecord)
        {
            var query = String.Format(
                "select * from {0} where {1}", 
                table, 
                condition);

            int found = 0;
            OpenConnection(c =>
            {
                var command = new MySqlCommand(query, c);
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    found++;
                    readRecord((IDataRecord)reader);
                }
            });

            return found;
        }

        public int Insert(string table, string columns, string values)
        {
            var sql = String.Format(
                "insert into {0} ({1}) values ({2})",
                table,
                columns,
                values);

            int result = -1;
            OpenConnection(c =>
            {
                var command = new MySqlCommand(sql, c);
                result = command.ExecuteNonQuery(); 
            });

            if (result == -1)
                throw new Exception();

            return result;
        }

        public int Update(string table, string setValues, string condition)
        {
            var sql = String.Format(
                "update {0} set {1} where {2}",
                table,
                setValues,
                condition);

            int result = -1;
            OpenConnection(c => 
                result = new MySqlCommand(sql, c)
                    .ExecuteNonQuery());

            return result;
        }

        public int Remove(string table, string condition)
        {
            var sql = String.Format(
                "delete from {0} where {1}",
                table,
                condition);

            int result = -1;
            OpenConnection(c =>
            {
                var command = new MySqlCommand(sql, c);
                result = command.ExecuteNonQuery();
            });

            if (result == -1)
                throw new Exception();

            return result;
        }
    }
}

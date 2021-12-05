using System.Data;
using Mint.API.Interfaces;
using System.Data.SqlClient;

namespace Mint.API.Services
{
    public class DBConnection
    {

        private static readonly string DNS =
            $"Data Source={Environment.GetEnvironmentVariable("SERVER")};"
            + $"Initial Catalog={Environment.GetEnvironmentVariable("DATABASE")};"
            + $"User Id={Environment.GetEnvironmentVariable("USER")};"
            + $"Password={Environment.GetEnvironmentVariable("PASSWORD")};";

        private List<SqlParameter> Parameters = new List<SqlParameter>();
        private SqlConnection Connection = new SqlConnection(DNS);
        public DBConnection() => Connection.Open();

        public void AddParameter(string pParameterName, SqlDbType pType, object? pValue)
        {
            Parameters.Add(
                new SqlParameter(pParameterName, pType)
                {
                    Value = pValue ?? DBNull.Value,
                    Direction = ParameterDirection.Input
                }
            );
        }
        public void AddParameter(string pParameterName, SqlDbType pType, int pSize, object? pValue)
        {
            Parameters.Add(
                new SqlParameter(pParameterName, pType)
                {
                    Size = pSize,
                    Value = pValue ?? DBNull.Value,
                    Direction = ParameterDirection.Input
                }
            );
        }
        public void AddParameter(string pParameterName, string pCustomType, DataTable pValue)
        {
            Parameters.Add(
                new SqlParameter(pParameterName, SqlDbType.Structured)
                {
                    Value = pValue,
                    Direction = ParameterDirection.Input
                }
            );
        }

        public List<T> ExecuteQuery<T>(string pQueryText) where T : Model
        {
            SqlCommand vCommand = new SqlCommand(pQueryText, Connection)
            {
                CommandType = CommandType.Text
            };

            foreach (SqlParameter vParameter in Parameters)
            {
                vCommand.Parameters.Add(vParameter);
            }

            SqlDataReader vReader = vCommand.ExecuteReader();

            List<T> vRows = new List<T>();
            T vRow = Activator.CreateInstance<T>();
            while (vRow.Read(vReader))
            {
                vRows.Add(vRow);
                vRow = Activator.CreateInstance<T>();
            };

            vReader.Close();

            return vRows;
        }
        public List<T> ExecuteQuery<T>(string pQueryText, out int pReturn) where T : Model
        {
            SqlCommand vCommand = new SqlCommand(pQueryText, Connection)
            {
                CommandType = CommandType.Text
            };

            foreach (SqlParameter vParameter in Parameters)
            {
                vCommand.Parameters.Add(vParameter);
            }

            vCommand.Parameters.Add(
                new SqlParameter("ReturnValue", SqlDbType.Int, 4)
                {
                    IsNullable = false,
                    Direction = ParameterDirection.ReturnValue
                }
            );

            SqlDataReader vReader = vCommand.ExecuteReader();

            List<T> vRows = new List<T>();
            T vRow = Activator.CreateInstance<T>();
            while (vRow.Read(vReader))
            {
                vRows.Add(vRow);
                vRow = Activator.CreateInstance<T>();
            };

            vReader.Close();
            pReturn = (int) vCommand.Parameters["ReturnValue"].Value;

            return vRows;
        }

        public List<T> ExecuteProcedure<T>(string pProcedureName) where T : Model
        {
            SqlCommand vCommand = new SqlCommand(pProcedureName, Connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            foreach (SqlParameter vParameter in Parameters)
            {
                vCommand.Parameters.Add(vParameter);
            }

            SqlDataReader vReader = vCommand.ExecuteReader();

            List<T> vRows = new List<T>();
            T vRow = Activator.CreateInstance<T>();
            while (vRow.Read(vReader))
            {
                vRows.Add(vRow);
                vRow = Activator.CreateInstance<T>();
            };

            vReader.Close();

            return vRows;
        }

        public List<T> ExecuteProcedure<T>(string pProcedureName, out int pReturn) where T : Model
        {
            SqlCommand vCommand = new SqlCommand(pProcedureName, Connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            foreach (SqlParameter vParameter in Parameters)
            {
                vCommand.Parameters.Add(vParameter);
            }

            vCommand.Parameters.Add(
                new SqlParameter("ReturnValue", SqlDbType.Int, 4)
                {
                    IsNullable = false,
                    Direction = ParameterDirection.ReturnValue
                }
            );

            SqlDataReader vReader = vCommand.ExecuteReader();

            List<T> vRows = new List<T>();
            T vRow = Activator.CreateInstance<T>();
            while (vRow.Read(vReader))
            {
                vRows.Add(vRow);
                vRow = Activator.CreateInstance<T>();
            };

            vReader.Close();
            pReturn = (int) vCommand.Parameters["ReturnValue"].Value;

            return vRows;
        }
        
        public void Close()
        {
            if (Connection.State == ConnectionState.Closed)
                Connection.Close();
            Connection.Dispose();
        }

    }
}
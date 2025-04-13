using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanCafe.Data
{
    public class ConnectDB
    {
        private readonly string _connectionString;

        public ConnectDB()
        {
            _connectionString = ConfigurationManager.AppSettings["ConfigurationString"];
        }

        public SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }

    }
}

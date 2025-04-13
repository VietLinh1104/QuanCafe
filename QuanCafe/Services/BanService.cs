using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuanCafe.Data;

namespace QuanCafe.Services
{
    class BanService
    {
        
        private readonly ConnectDB _db;

        public BanService()
        {
            _db = new ConnectDB();
        }
        
        public DataTable GetAllBan()
        {
            using (SqlConnection conn = _db.GetConnection())
            {
                string query = "SELECT * FROM Ban";
                SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                return dt;
            }
        }
    }
}
        


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
    class KhachHangService
    {
        private readonly ConnectDB _db;
        public KhachHangService()
        {
            _db = new ConnectDB();
        }

        public DataTable GetAllKhachHang()
        {
            using (SqlConnection conn = _db.GetConnection())
            {
                string query = "SELECT * FROM KhachHang";
                SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                return dt;
            }
        }
    }
}

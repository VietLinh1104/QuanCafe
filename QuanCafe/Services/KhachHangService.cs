using System;
using System.Data;
using System.Data.SqlClient;
using QuanCafe.Data;

namespace QuanCafe.Services
{
    public class KhachHangService
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

        public DataTable GetAllKhachHang(string keyword = "")
        {
            using (SqlConnection conn = _db.GetConnection())
            {
                string query = "SELECT * FROM KhachHang";

                if (!string.IsNullOrWhiteSpace(keyword))
                {
                    query += " WHERE ten_khach_hang LIKE @keyword OR so_dien_thoai LIKE @keyword OR email LIKE @keyword";
                }

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    if (!string.IsNullOrWhiteSpace(keyword))
                    {
                        cmd.Parameters.AddWithValue("@keyword", "%" + keyword + "%");
                    }

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    return dt;
                }
            }
        }
        public bool AddKhachHang(string ten, string sdt, string email, string ghichu)
        {
            using (SqlConnection conn = _db.GetConnection())
            {
                string query = @"INSERT INTO KhachHang (ten_khach_hang, so_dien_thoai, email, ghi_chu)
                                 VALUES (@Ten, @SDT, @Email, @GhiChu)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Ten", ten);
                cmd.Parameters.AddWithValue("@SDT", sdt);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@GhiChu", (object)ghichu ?? DBNull.Value);

                conn.Open();
                int rows = cmd.ExecuteNonQuery();
                return rows > 0;
            }
        }

        public bool UpdateKhachHang(int id, string ten, string sdt, string email, string ghichu)
        {
            using (SqlConnection conn = _db.GetConnection())
            {
                string query = @"UPDATE KhachHang SET ten_khach_hang = @Ten, so_dien_thoai = @SDT, 
                                 email = @Email, ghi_chu = @GhiChu WHERE id_khach_hang = @Id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.Parameters.AddWithValue("@Ten", ten);
                cmd.Parameters.AddWithValue("@SDT", sdt);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@GhiChu", (object)ghichu ?? DBNull.Value);

                conn.Open();
                int rows = cmd.ExecuteNonQuery();
                return rows > 0;
            }
        }

        public bool DeleteKhachHang(int id)
        {
            using (SqlConnection conn = _db.GetConnection())
            {
                string query = "DELETE FROM KhachHang WHERE id_khach_hang = @Id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", id);

                conn.Open();
                int rows = cmd.ExecuteNonQuery();
                return rows > 0;
            }
        }
    }
}

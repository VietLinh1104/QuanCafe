using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using QuanCafe.Data;

namespace QuanCafe.Repositories
{
    public class BanService
    {
        private readonly ConnectDB _db;

        public BanService()
        {
            _db = new ConnectDB();
        }

        // Lấy toàn bộ danh sách bàn
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

        // Thêm bàn mới
        public bool AddBan(string tenBan, string trangThai)
        {
            using (SqlConnection conn = _db.GetConnection())
            {
                string query = "INSERT INTO Ban (ten_ban, trang_thai) VALUES (@tenBan, @trangThai)";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@tenBan", tenBan);
                    cmd.Parameters.AddWithValue("@trangThai", trangThai);

                    try
                    {
                        conn.Open();
                        int result = cmd.ExecuteNonQuery();
                        return result > 0;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi khi thêm bàn: " + ex.Message);
                        return false;
                    }
                }
            }
        }

        // Lấy ID tiếp theo của bàn (tự động tăng)
        public int GetNextBanId()
        {
            using (SqlConnection conn = _db.GetConnection())
            {
                string query = "SELECT ISNULL(MAX(id_ban), 0) + 1 FROM Ban";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    int nextId = (int)cmd.ExecuteScalar();
                    return nextId;
                }
            }
        }

        // Cập nhật thông tin bàn
        public bool UpdateBan(int idBan, string tenBan, string trangThai)
        {
            try
            {
                using (SqlConnection conn = _db.GetConnection())
                {
                    string query = "UPDATE Ban SET ten_ban = @tenBan, trang_thai = @trangThai WHERE id_ban = @idBan";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@tenBan", tenBan);
                        cmd.Parameters.AddWithValue("@trangThai", trangThai);
                        cmd.Parameters.AddWithValue("@idBan", idBan);

                        conn.Open();
                        int result = cmd.ExecuteNonQuery();
                        return result > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi cập nhật bàn: " + ex.Message);
                return false;
            }
        }

        // Xóa bàn theo ID
        public bool DeleteBan(int idBan)
        {
            try
            {
                using (SqlConnection conn = _db.GetConnection())
                {
                    string query = "DELETE FROM Ban WHERE id_ban = @idBan";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@idBan", idBan);

                        conn.Open();
                        int result = cmd.ExecuteNonQuery();
                        return result > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xóa bàn: " + ex.Message);
                return false;
            }
        }
    }
}

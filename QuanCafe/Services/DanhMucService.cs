using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using QuanCafe.Data;

namespace QuanCafe.Repositories
{
    public class DanhMucService
    {
        private readonly ConnectDB _db;

        public DanhMucService()
        {
            _db = new ConnectDB();
        }

        // Lấy toàn bộ danh mục sản phẩm
        public DataTable GetAllDanhMuc()
        {
            using (SqlConnection conn = _db.GetConnection())
            {
                string query = "SELECT id_danh_muc, ten_danh_muc FROM DanhMucSanPham"; // Đảm bảo cột đúng
                SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                return dt;
            }
        }

        // Thêm danh mục mới
        public bool AddDanhMuc(string tenDanhMuc, string moTa)
        {
            using (SqlConnection conn = _db.GetConnection())
            {
                string query = "INSERT INTO DanhMucSanPham (ten_danh_muc, mo_ta) VALUES (@ten, @moTa)";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ten", tenDanhMuc);
                    cmd.Parameters.AddWithValue("@moTa", moTa ?? (object)DBNull.Value);

                    try
                    {
                        conn.Open();
                        int result = cmd.ExecuteNonQuery();
                        return result > 0;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi khi thêm danh mục: " + ex.Message);
                        return false;
                    }
                }
            }
        }

        public int GetNextDanhMucId()
        {
            using (SqlConnection conn = _db.GetConnection())
            {
                string query = "SELECT ISNULL(MAX(id_danh_muc), 0) + 1 FROM DanhMucSanPham";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    int nextId = (int)cmd.ExecuteScalar();
                    return nextId;
                }
            }
        }

        // Cập nhật danh mục theo ID
        public bool UpdateDanhMuc(int id, string tenDanhMuc, string moTa)
        {
            try
            {
                using (SqlConnection conn = _db.GetConnection())
                {
                    string query = "UPDATE DanhMucSanPham SET ten_danh_muc = @ten, mo_ta = @moTa WHERE id_danh_muc = @id";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ten", tenDanhMuc);
                        cmd.Parameters.AddWithValue("@moTa", moTa ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@id", id);

                        conn.Open();
                        int result = cmd.ExecuteNonQuery();
                        return result > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi cập nhật danh mục: " + ex.Message);
                return false;
            }
        }

        // Xóa danh mục theo ID
        public bool DeleteDanhMuc(int id)
        {
            try
            {
                using (SqlConnection conn = _db.GetConnection())
                {
                    string query = "DELETE FROM DanhMucSanPham WHERE id_danh_muc = @id";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", id);

                        conn.Open();
                        int result = cmd.ExecuteNonQuery();
                        return result > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xóa danh mục: " + ex.Message);
                return false;
            }
        }

        //thêm 
        
    }
}

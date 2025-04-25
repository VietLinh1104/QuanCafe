using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuanCafe.Data;
using System.Windows.Forms;

namespace QuanCafe.Services
{
    class BanService
    {
        
        private readonly ConnectDB _db;

        public BanService()
        {
            _db = new ConnectDB();
        }

        public bool IsValidTrangThai(string trangThai)//Hàm này kiểm tra xem trạng thái bàn có hợp lệ không (chỉ cho phép 3 giá trị: "Trống", "Có người", "Đặt trước").
        {
            string[] validStatuses = { "Trống", "Có người", "Đặt trước" };
            return validStatuses.Contains(trangThai);
        }

        public DataTable GetAllBan()    //Trả về toàn bộ dữ liệu từ bảng Ban trong SQL Server.
        {
            try
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
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải bàn: " + ex.Message);
                return null;  // Trả về null nếu có lỗi
            }
        }

      

        public bool InsertBan(string tenBan, string trangThai)
        {
            if (!IsValidTrangThai(trangThai)) // Kiểm tra giá trị hợp lệ
            {
                MessageBox.Show("Trạng thái bàn không hợp lệ! Chỉ chấp nhận các giá trị: 'Trống', 'Có người', 'Đặt trước'.");
                return false;
            }

            const string sql = @"
        INSERT INTO Ban (ten_ban, trang_thai)
        VALUES (@ten_ban, @trang_thai)";
            using (SqlConnection conn = _db.GetConnection())
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@ten_ban", tenBan);
                cmd.Parameters.AddWithValue("@trang_thai", trangThai);
                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

      
        public bool UpdateBan(int id, string tenBan, string trangThai)
        {
            if (!IsValidTrangThai(trangThai)) // Kiểm tra giá trị hợp lệ
            {
                MessageBox.Show("Trạng thái bàn không hợp lệ! Chỉ chấp nhận các giá trị: 'Trống', 'Có người', 'Đặt trước'.");
                return false;
            }

            const string sql = @"
        UPDATE Ban
        SET ten_ban = @ten_ban,
            trang_thai = @trang_thai
        WHERE id_ban = @id";

            using (SqlConnection conn = _db.GetConnection())
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@ten_ban", tenBan);
                cmd.Parameters.AddWithValue("@trang_thai", trangThai);
                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool DeleteBan(int id)
        {
            const string sql = "DELETE FROM Ban WHERE id_ban = @id";
            using (SqlConnection conn = _db.GetConnection())
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            { 
                cmd.Parameters.AddWithValue("@id", id);
                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        //internal bool InsertBan(string soBan, string trangThai)
        //{
        //    throw new NotImplementedException();
        //}
    }

}
        


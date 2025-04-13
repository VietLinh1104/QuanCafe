using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuanCafe.Data;

namespace QuanCafe.Services
{
    class HoaDonService
    {
        private readonly ConnectDB _db;

        public HoaDonService()
        {
            _db = new ConnectDB();
        }
        public int InsertHoaDon(int idBan, int idNhanVien, int? idKhachHang, int trangThai)
        {
            using (SqlConnection conn = _db.GetConnection())
            {
                conn.Open();
                string query = @"
            INSERT INTO HoaDon (id_ban, id_nhan_vien, id_khach_hang, trang_thai)
            VALUES (@id_ban, @id_nhan_vien, @id_khach_hang, @trang_thai);
            SELECT CAST(SCOPE_IDENTITY() AS INT)";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id_ban", idBan);
                    cmd.Parameters.AddWithValue("@id_nhan_vien", idNhanVien);
                    cmd.Parameters.AddWithValue("@id_khach_hang", (object)idKhachHang ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@trang_thai", trangThai);

                    object result = cmd.ExecuteScalar();
                    return result != null ? Convert.ToInt32(result) : -1;
                }
            }
        }
    }
}

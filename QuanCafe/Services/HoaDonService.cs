using System;
using System.Data;
using System.Data.SqlClient;
using QuanCafe.Data;
using QuanCafe.Models;

namespace QuanCafe.Services
{
    public class HoaDonService
    {
        private readonly ConnectDB _db;

        public HoaDonService()
        {
            _db = new ConnectDB();
        }

        public int InsertHoaDon(HoaDon hoaDon)
        {
            using (SqlConnection conn = _db.GetConnection())
            {
                conn.Open();
                string query = @"
            INSERT INTO HoaDon (id_ban, id_nhan_vien, id_khach_hang, trang_thai)
            VALUES (@idBan, @idNhanVien, @idKhachHang, @trangThai);
            SELECT CAST(SCOPE_IDENTITY() as int);";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@idBan", hoaDon.IdBan);
                    cmd.Parameters.AddWithValue("@idNhanVien", hoaDon.IdNhanVien);
                    cmd.Parameters.AddWithValue("@idKhachHang", (object)hoaDon.IdKhachHang ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@trangThai", hoaDon.TrangThai);
                                        return (int)cmd.ExecuteScalar(); // Lấy id vừa insert
                }
            }
        }


    }
}

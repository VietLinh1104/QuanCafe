using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuanCafe.Data;
using QuanCafe.Models;

namespace QuanCafe.Services
{
    class ChiTietHoaDonService
    {
        private readonly ConnectDB _db;

        public ChiTietHoaDonService()
        {
            _db = new ConnectDB();
        }

        public void InsertChiTietHoaDon(ChiTietHoaDon cthd)
        {
            using (SqlConnection conn = _db.GetConnection())
            {
                conn.Open();
                string query = @"INSERT INTO ChiTietHoaDon (id_hoa_don, id_san_pham, so_luong, don_gia)
                         VALUES (@idHoaDon, @idSanPham, @soLuong, @donGia)";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@idHoaDon", cthd.IdHoaDon);
                    cmd.Parameters.AddWithValue("@idSanPham", cthd.IdSanPham);
                    cmd.Parameters.AddWithValue("@soLuong", cthd.SoLuong);
                    cmd.Parameters.AddWithValue("@donGia", cthd.DonGia);
                    cmd.ExecuteNonQuery();
                }
            }
        }


    }
}


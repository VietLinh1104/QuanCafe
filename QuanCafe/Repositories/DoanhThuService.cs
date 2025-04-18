using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using QuanCafe.Data;

namespace QuanCafe.Repositories
{
    public class DoanhThuService
    {
        private readonly ConnectDB _db;

        public DoanhThuService()
        {
            _db = new ConnectDB();
        }

        // Lấy doanh thu theo khoảng thời gian
        public DataTable GetDoanhThuTheoNgay(DateTime fromDate, DateTime toDate)
        {
            using (SqlConnection conn = _db.GetConnection())
            {
                string query = @"
                 SELECT 
                     HD.id_hoa_don,
                     HD.thoi_gian,
                     NV.ten_nhan_vien,
                     KH.ten_khach_hang,
                     HD.tong_tien
                        FROM HoaDon HD
                        LEFT JOIN NhanVien NV ON HD.id_nhan_vien = NV.id_nhan_vien
                        LEFT JOIN KhachHang KH ON HD.id_khach_hang = KH.id_khach_hang
                        WHERE HD.trang_thai = 1 
                     AND HD.thoi_gian >= @fromDate AND HD.thoi_gian < @toDate";


                SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                adapter.SelectCommand.Parameters.AddWithValue("@fromDate", fromDate);
                adapter.SelectCommand.Parameters.AddWithValue("@toDate", toDate);

                DataTable dt = new DataTable();
                adapter.Fill(dt);
                return dt;
            }
        }
    }
}

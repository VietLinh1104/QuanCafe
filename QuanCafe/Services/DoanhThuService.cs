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
            SUM(CTHD.don_gia) AS tong_tien
        FROM HoaDon HD
        LEFT JOIN NhanVien NV ON HD.id_nhan_vien = NV.id_nhan_vien
        LEFT JOIN KhachHang KH ON HD.id_khach_hang = KH.id_khach_hang
        JOIN ChiTietHoaDon CTHD ON HD.id_hoa_don = CTHD.id_hoa_don
        WHERE HD.trang_thai = 1 
            AND HD.thoi_gian >= @fromDate AND HD.thoi_gian < @toDate
        GROUP BY 
            HD.id_hoa_don, HD.thoi_gian, NV.ten_nhan_vien, KH.ten_khach_hang";

                SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                adapter.SelectCommand.Parameters.AddWithValue("@fromDate", fromDate);
                adapter.SelectCommand.Parameters.AddWithValue("@toDate", toDate);

                DataTable dt = new DataTable();
                adapter.Fill(dt);
                return dt;
            }
        }





        // Lấy danh sách khách hàng thân thiết (tích điểm và xếp hạng)
        public DataTable GetDanhSachKhachHangThanThiet()
        {
            using (SqlConnection conn = _db.GetConnection())
            {
                string query = @"
        WITH HoaDonDaThanhToan AS (
            SELECT 
                hd.id_hoa_don,
                hd.id_khach_hang,
                SUM(ct.don_gia) AS tong_tien
            FROM HoaDon hd
            JOIN ChiTietHoaDon ct ON hd.id_hoa_don = ct.id_hoa_don
            WHERE hd.trang_thai = 1
            GROUP BY hd.id_hoa_don, hd.id_khach_hang
        ),
        KhachHangTichDiem AS (
            SELECT 
                kh.id_khach_hang,
                kh.ten_khach_hang,
                kh.so_dien_thoai,
                kh.email,
                ISNULL(SUM(hdtt.tong_tien), 0) AS tong_tien_da_tra,
                FLOOR(ISNULL(SUM(hdtt.tong_tien), 0) / 30000) AS diem
            FROM KhachHang kh
            LEFT JOIN HoaDonDaThanhToan hdtt ON kh.id_khach_hang = hdtt.id_khach_hang
            GROUP BY kh.id_khach_hang, kh.ten_khach_hang, kh.so_dien_thoai, kh.email
        )
        SELECT 
            *,
            RANK() OVER (ORDER BY diem DESC) AS thu_hang
        FROM KhachHangTichDiem
        ORDER BY diem DESC";

                SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                return dt;
            }
        }








    }
}

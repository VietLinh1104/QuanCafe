using System;
using System.Data;
using System.Data.SqlClient;
using QuanCafe.Data;

namespace QuanCafe.Repositories
{
    public class ThongKeService
    {
        private readonly ConnectDB _db;

        public ThongKeService()
        {
            _db = new ConnectDB();
        }

        public DataTable GetDoanhThuTheoNgay(DateTime fromDate, DateTime toDate)
        {
            using (SqlConnection conn = _db.GetConnection())
            {
                string query = @"
                    SELECT 
                        CAST(HD.thoi_gian AS DATE) AS Ngay,
                        SUM(HD.tong_tien) AS TongDoanhThu
                    FROM HoaDon HD
                    WHERE HD.trang_thai = 1 
                        AND HD.thoi_gian BETWEEN @fromDate AND @toDate
                    GROUP BY CAST(HD.thoi_gian AS DATE)
                    ORDER BY Ngay ASC";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@fromDate", fromDate);
                cmd.Parameters.AddWithValue("@toDate", toDate);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                return dt;
            }
        }
    }
}

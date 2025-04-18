using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QuanCafe.Repositories;
using System.Windows.Forms.DataVisualization.Charting;

namespace QuanCafe.Forms.ManagerTab.Tab
{
    public partial class ThongKe: UserControl
    {
        public ThongKe()
        {
            InitializeComponent();
        }

        private ThongKeService thongKeService = new ThongKeService();


        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }
        // Hàm gọi từ DoanhThu để cập nhật biểu đồ
        public void LoadDataFromDateRange(DateTime fromDate, DateTime toDate)
        {
            TimeSpan khoangNgay = toDate.Date - fromDate.Date;
            if (khoangNgay.TotalDays < 7)
            {
                fromDate = toDate.AddDays(-6); // luôn đủ 7 ngày
            }

            DataTable dt = thongKeService.GetDoanhThuTheoNgay(fromDate, toDate);

            // Xóa dữ liệu cũ trong biểu đồ
            chart1.Series.Clear();
            chart1.ChartAreas.Clear();

            chart1.ChartAreas.Add("DoanhThuArea");

            Series series = new Series("Doanh Thu");
            series.ChartType = SeriesChartType.Column;
            series.XValueType = ChartValueType.Date;
            series.YValueType = ChartValueType.Double;
            series.IsValueShownAsLabel = true;

            // Thêm 0 cho những ngày không có dữ liệu
            DateTime currentDate = fromDate.Date;
            while (currentDate <= toDate.Date)
            {
                double doanhThu = 0;

                // Kiểm tra doanh thu cho từng ngày trong DataTable
                foreach (DataRow row in dt.Rows)
                {
                    DateTime ngay = Convert.ToDateTime(row["Ngay"]);
                    if (ngay.Date == currentDate)
                    {
                        doanhThu = Convert.ToDouble(row["TongDoanhThu"]);
                        break;
                    }
                }

                // Thêm điểm vào biểu đồ với ngày tháng cụ thể
                series.Points.AddXY(currentDate.ToString("dd/MM/yyyy"), doanhThu);
                currentDate = currentDate.AddDays(1);
            }

            chart1.Series.Add(series);
        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }
    }
}

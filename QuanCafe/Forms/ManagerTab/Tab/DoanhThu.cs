using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QuanCafe.Repositories; // để gọi ThongKeService (doanhthuService)
using QuanCafe.Forms.ManagerTab.Tab;

namespace QuanCafe.Forms.ManagerTab.Tab
{
    public partial class DoanhThu: UserControl
    {
        public DoanhThu(ThongKe thongKe)
        {
            InitializeComponent();


            this.thongKeTab = thongKe;
        }
        private DoanhThuService doanhthuService = new DoanhThuService();
        //-----------------------------------------------------------------------------------------
        private ThongKe thongKeTab;


        private void DoanhThu_Load(object sender, EventArgs e)
        {
            listView1.View = View.Details;
            listView1.Columns.Clear();
            listView1.Columns.Add("Mã Hóa Đơn", 80);
            listView1.Columns.Add("Thời gian", 150);
            listView1.Columns.Add("Nhân viên", 120);
            listView1.Columns.Add("Khách hàng", 120);
            listView1.Columns.Add("Tổng tiền", 100);
            listView1.FullRowSelect = true;

            // Gán mặc định ngày hôm nay
            dateTimePickerFrom.Value = DateTime.Today;
            dateTimePickerTo.Value = DateTime.Today;
        }

        private void label3_Click(object sender, EventArgs e)
        {
           
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DateTime from = dateTimePickerFrom.Value.Date;
            DateTime to = dateTimePickerTo.Value.Date.AddDays(1); // lấy đến trước 00:00 ngày hôm sau

            // Kiểm tra nếu ngày bắt đầu lớn hơn hoặc bằng ngày kết thúc
            if (from >= to)
            {
                MessageBox.Show("Ngày bắt đầu phải nhỏ hơn ngày kết thúc.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Lấy dữ liệu hóa đơn
            DataTable dt = doanhthuService.GetDoanhThuTheoNgay(from, to);

            // Nếu không có hóa đơn trong khoảng thời gian này
            if (dt.Rows.Count == 0)
            {
                MessageBox.Show("Không có hóa đơn nào trong thời gian này.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            listView1.Items.Clear();
            decimal tongTien = 0;

            foreach (DataRow row in dt.Rows)
            {
                ListViewItem item = new ListViewItem(row["id_hoa_don"].ToString());
                item.SubItems.Add(Convert.ToDateTime(row["thoi_gian"]).ToString("dd/MM/yyyy HH:mm"));
                item.SubItems.Add(row["ten_nhan_vien"].ToString());
                item.SubItems.Add(row["ten_khach_hang"].ToString());
                item.SubItems.Add(string.Format("{0:N0} VNĐ", row["tong_tien"]));

                tongTien += Convert.ToDecimal(row["tong_tien"]);
                listView1.Items.Add(item);
            }

            txtTong.Text = string.Format("{0:N0} VNĐ", tongTien);

            // Gọi cập nhật biểu đồ thống kê
            thongKeTab.LoadDataFromDateRange(from, to);

        }
    }
}

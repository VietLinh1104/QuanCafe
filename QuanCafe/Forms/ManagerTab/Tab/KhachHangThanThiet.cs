using System;
using System.Data;
using System.Windows.Forms;
using QuanCafe.Repositories; // Để gọi DoanhThuService

using System.Drawing;




namespace QuanCafe.Forms.ManagerTab.Tab
{
    public partial class KhachHangThanThiet : UserControl
    {
        private DoanhThuService doanhthuService = new DoanhThuService(); // Khởi tạo đối tượng service

        public KhachHangThanThiet()
        {
            InitializeComponent();
            LoadListViewFromList();
        }

        // Phương thức gọi khi form load
        private void LoadListViewFromList()
        {
            listView1.Items.Clear();
            listView1.View = View.Details;
            listView1.Columns.Clear();
            listView1.FullRowSelect = true;

            listView1.Columns.Add("ID", 50);
            listView1.Columns.Add("Tên khách hàng", 200);
            listView1.Columns.Add("SĐT", 150);
            listView1.Columns.Add("Email", 180);
            listView1.Columns.Add("Tổng tiền đã trả", 130);
            listView1.Columns.Add("Tích điểm", 100);
            listView1.Columns.Add("Thứ hạng", 100);

            DataTable dt = doanhthuService.GetDanhSachKhachHangThanThiet();

            foreach (DataRow row in dt.Rows)
            {
                ListViewItem lvi = new ListViewItem(row["id_khach_hang"].ToString());
                lvi.SubItems.Add(row["ten_khach_hang"].ToString());
                lvi.SubItems.Add(row["so_dien_thoai"].ToString());
                lvi.SubItems.Add(row["email"].ToString());
                lvi.SubItems.Add(string.Format("{0:N0} VNĐ", row["tong_tien_da_tra"]));
                lvi.SubItems.Add(row["diem"].ToString());
                lvi.SubItems.Add(row["thu_hang"].ToString());

                listView1.Items.Add(lvi);
                ColorListViewRows(listView1);
            }
        }

        private void ColorListViewRows(ListView listView)
        {
            for (int i = 0; i < listView.Items.Count; i++)
            {
                if (i % 2 == 0)
                    listView.Items[i].BackColor = Color.White;
                else
                    listView.Items[i].BackColor = Color.LightGray; // hoặc Color.AliceBlue, LightYellow...
            }
        }


        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Xử lý khi người dùng chọn một mục trong ListView (nếu cần)
        }
    }
}

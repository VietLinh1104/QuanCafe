using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QuanCafe.Forms;
using QuanCafe.Forms.ManagerTab.Tab;
using QuanCafe.Helpers;
using QuanCafe.Models;
using QuanCafe.Repositories;
using QuanCafe.Services;
//using System.Drawing.Printing;


namespace QuanCafe
{
    public partial class QuanLyBan : Form
    {
        string[] table = { "01", "02", "03", "04" };
        List<ChiTietHoaDon> listSanPham;

        public QuanLyBan()
        {
            InitializeComponent();
            LoadTableButtons();
            LoadComboBox1();
            LoadComboBox2();
            LoadComboBox3();
            LoadComboBox4();
            LoadListViewFromList();
        }

        private void LoadTableButtons()
        {
            foreach (string ban in table)
            {
                Button btn = new Button();
                btn.Text = $"Bàn {ban}";
                btn.Width = 100;
                btn.Height = 100;
                btn.BackColor = Color.LightGreen;
                btn.Tag = ban; // dùng để lưu ID bàn nếu cần

                btn.Click += TableButton_Click; // Gán sự kiện click cho từng button

                flowLayoutPanel1.Controls.Add(btn);
            }
        }

        private void TableButton_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            string maBan = btn.Tag.ToString();

            MessageBox.Show($"Bạn đã chọn bàn: {maBan}");
            // Ở đây bạn có thể mở form gọi món, load hóa đơn, v.v...
        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void QuanLyBan_Load(object sender, EventArgs e)
        {

        }

        private void homeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var formManager = new Manager();
            formManager.Show();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {


        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void tàiKhoảnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string token = Session.JwtToken;

            var (username, role, expiration) = JwtHelper.DecodeToken(token);
            if (role == "Quản lý")
            {
                var formTaiKhoan = new Forms.TaiKhoan();
                formTaiKhoan.Show();

            }
            else
            {
                MessageBox.Show("Yêu cầu Quền Quản Lý");
            }

        }

        private void LoadComboBox1()
        {
            DanhMucService service = new DanhMucService();
            DataTable dt = service.GetAllDanhMuc();

            comboBox1.DataSource = dt;
            comboBox1.DisplayMember = "ten_danh_muc"; // Tên cột hiển thị
            comboBox1.ValueMember = "id_danh_muc";     // Giá trị thực sự (ví dụ ID)
        }
        private void LoadComboBox2()
        {
            SanPhamService service = new SanPhamService(); // hoặc tên class chứa GetAllSanPham()
            DataTable dt = service.GetAllSanPham();

            comboBox2.DataSource = dt;
            comboBox2.DisplayMember = "ten_san_pham";  // tên cột hiển thị
            comboBox2.ValueMember = "id_san_pham";     // giá trị thực sự
        }
        private void LoadComboBox4()
        {
            KhachHangService service = new KhachHangService(); // hoặc class chứa GetAllKhachHang()
            DataTable dt = service.GetAllKhachHang();

            comboBox4.DataSource = dt;
            comboBox4.DisplayMember = "ten_khach_hang";  // Cột hiển thị trong ComboBox
            comboBox4.ValueMember = "id_khach_hang";     // Giá trị thực tế (thường là ID)
        }
        private void LoadComboBox3()
        {
            BanService service = new BanService(); // hoặc class chứa GetAllBan()
            DataTable dt = service.GetAllBan();
            comboBox3.DataSource = dt;
            comboBox3.DisplayMember = "ten_ban";  // tên cột hiển thị trong combobox
            comboBox3.ValueMember = "id_ban";     // giá trị thực sự
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        

        private void button1_Click(object sender, EventArgs e)
        {
            ChiTietHoaDon sanPham = new ChiTietHoaDon(0, 1, 1, 1, 30000);
            listSanPham.Add(sanPham);
            LoadListViewFromList();
        }

        private void LoadListViewFromList()
        {
            listView1.Items.Clear();
            int stt = 1;

            foreach (var item in listSanPham)
            {
                ListViewItem lvi = new ListViewItem(stt.ToString());
                lvi.SubItems.Add(item.IdSanPham.ToString());
                lvi.SubItems.Add(item.SoLuong.ToString());
                lvi.SubItems.Add(item.DonGia.ToString("N0"));

                listView1.Items.Add(lvi);
                stt++;
            }
        }


        private void listView2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}

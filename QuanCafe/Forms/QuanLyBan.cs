using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QuanCafe.Forms;
using QuanCafe.Helpers;
using QuanCafe.Models;
//using System.Drawing.Printing;


namespace QuanCafe
{
    public partial class QuanLyBan : Form
    {
        string[] table = { "01", "02", "03", "04" };

        public QuanLyBan()
        {
            InitializeComponent();
            LoadTableButtons();

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
                var formTaiKhoan = new TaiKhoan();
                formTaiKhoan.Show();

            }
            else
            {
                MessageBox.Show("Yêu cầu Quền Quản Lý");
            }


        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QuanCafe.Helpers;
using QuanCafe.Models;
using QuanCafe.Repositories;

namespace QuanCafe.Forms
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
            JwtHelper jwt = new JwtHelper();
        }

        private void Login_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            NhanVienRepository repo = new NhanVienRepository();
            string genToken = repo.Login(textBox1.Text, textBox2.Text);
            Session.JwtToken = genToken;
            string token = Session.JwtToken;

            if (JwtHelper.ValidateToken(token))
            {
                //MessageBox.Show("Đăng nhập thành công.");
                this.Hide();

                // Mở form Quản lý bàn
                QuanLyBan frm = new QuanLyBan();
                frm.FormClosed += (s, args) => this.Close(); // Khi form quản lý đóng thì thoát app
                frm.Show();
            }
            else
            {
                MessageBox.Show("Sai số điện thoại hoặc mật khẩu.");
            }
        }

    }
}

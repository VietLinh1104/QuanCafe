using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QuanCafe.Models;
using QuanCafe.Services;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using QuanCafe.Helpers;

namespace QuanCafe.Forms
{
    public partial class TaiKhoan : Form
    {
        private int dataSelected;
        NhanVienService nhanVienService = new NhanVienService();
        public TaiKhoan()
        {
            
            InitializeComponent();
            Load_List();
            LoadChucVuComboBox();
        }

        // load table function
        public void Load_List()
        {
            try
            {
                DataTable dt = nhanVienService.GetAll();

                listView1.Items.Clear();
                listView1.View = View.Details;
                listView1.Columns.Clear();
                listView1.FullRowSelect = true;


                listView1.Columns.Add("id", 50);
                listView1.Columns.Add("Tên nhân viên", 150);
                listView1.Columns.Add("Chức vụ", 100);
                listView1.Columns.Add("Số điện thoại", 100);
                listView1.Columns.Add("email", 200);

                textBox2.Text = "";
                textBox5.Text = "";
                textBox4.Text = "";
                textBox6.Text = " ";
                textBox7.Text = "";
                comboBox1.Text = "";
                textBox6.Enabled = true;
                textBox7.Enabled = true;
                button3.Enabled = true;
                button2.Enabled = false;
                button5.Enabled = false;


                foreach (DataRow row in dt.Rows)
                {
                    ListViewItem item = new ListViewItem(row[0].ToString());

                    for (int i = 1; i < dt.Columns.Count - 1; i++)
                    {
                        item.SubItems.Add(row[i].ToString());
                    }

                    listView1.Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        // click load
        private void button4_Click(object sender, EventArgs e)
        {
            Load_List();
        }

        // click list_table
        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (listView1.SelectedItems.Count > 0)
            {
                dataSelected = int.Parse(listView1.SelectedItems[0].SubItems[0].Text);
                string tenNhanVienSelected = listView1.SelectedItems[0].SubItems[1].Text;
                string chucVuSelected = listView1.SelectedItems[0].SubItems[2].Text;
                string soDienThoaiSelected = listView1.SelectedItems[0].SubItems[3].Text;
                string emailSelected = listView1.SelectedItems[0].SubItems[4].Text;

                textBox2.Text = tenNhanVienSelected;
                textBox5.Text = soDienThoaiSelected;
                textBox4.Text = emailSelected;
                comboBox1.Text = chucVuSelected;
                textBox6.Text = "Password";
                textBox7.Text = "Password";
                textBox6.Enabled = false;
                textBox7.Enabled = false;
                button3.Enabled = false;
                button2.Enabled = true;
                button5.Enabled = true;
            }
        }

        //click add
        private void button3_Click(object sender, EventArgs e)
        {
            if (!IsValidForm(out string tenNhanVien, out string chucVu, out string soDienThoai, out string email, out string password))
            {
                return;
            }

            var (emailExists, phoneExists) = nhanVienService.CheckEmailAndPhone(email, soDienThoai);

            if (emailExists || phoneExists)
            {
                string message = "";
                if (emailExists) message += "Email đã tồn tại.\n";
                if (phoneExists) message += "Số điện thoại đã tồn tại.";
                MessageBox.Show(message);
                return;
            }

            NhanVien nv = new NhanVien(0, tenNhanVien, chucVu, soDienThoai, email, password);
            nhanVienService.Register(nv);
            MessageBox.Show("Thêm tài khoản thành công");
            Load_List();
        }

        // click delete  
        private void button5_Click(object sender, EventArgs e)
        {
            var confirmResult = MessageBox.Show(
                "Bạn có chắc chắn muốn xóa nhân viên này?",
                "Xác nhận xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (confirmResult == DialogResult.Yes)
            {
                // Thực hiện xóa ở đây
                nhanVienService.DeleteById(dataSelected);
                Load_List();
            }

        }

        //click edit
        private void button2_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn một nhân viên để sửa.");
                return;
            }

            if (!IsValidForm(out string tenNhanVien, out string chucVu, out string soDienThoai, out string email, out string _))
            {
                return;
            }

            // Kiểm tra trùng lặp email và sđt, bỏ qua chính bản ghi đang sửa
            foreach (ListViewItem item in listView1.Items)
            {
                int id = int.Parse(item.SubItems[0].Text);
                string emailInList = item.SubItems[4].Text;
                string phoneInList = item.SubItems[3].Text;

                if (id != dataSelected && (emailInList == email || phoneInList == soDienThoai))
                {
                    MessageBox.Show("Email hoặc số điện thoại đã tồn tại ở nhân viên khác.");
                    return;
                }
            }

            // Gọi hàm UpdateWithoutPassword
            NhanVien nv = new NhanVien(dataSelected, tenNhanVien, chucVu, soDienThoai, email, password: "");
            bool updated = nhanVienService.UpdateWithoutPassword(nv);

            if (updated)
            {
                MessageBox.Show("Cập nhật thành công!");
                Load_List();
            }
            else
            {
                MessageBox.Show("Cập nhật thất bại.");
            }
        }

        //check valid function 
        private bool IsValidForm(out string tenNhanVien, out string chucVu, out string soDienThoai, out string email, out string password)
        {
            tenNhanVien = textBox2.Text.Trim();
            chucVu = comboBox1.Text.Trim();
            soDienThoai = textBox5.Text.Trim();
            email = textBox4.Text.Trim();
            password = textBox6.Text.Trim();
            string rePassword = textBox7.Text.Trim();

            if (string.IsNullOrEmpty(tenNhanVien) || string.IsNullOrEmpty(chucVu) ||
                string.IsNullOrEmpty(soDienThoai) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin.");
                return false;
            }

            if (!ValidateHelper.IsValidEmail(email))
            {
                MessageBox.Show("Email không hợp lệ");
                return false;
            }

            if (!ValidateHelper.IsValidPhoneNumber(soDienThoai))
            {
                MessageBox.Show("Số điện thoại không hợp lệ");
                return false;
            }

            if (password != rePassword)
            {
                MessageBox.Show("Password không giống nhau");
                return false;
            }

            return true;
        }


        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        public void LoadChucVuComboBox()
        {
            string[] chucVus = { "Quản lý", "Nhân viên" };
            comboBox1.Items.AddRange(chucVus);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        //click search
        private void button1_Click(object sender, EventArgs e)
        {
            string keyword = textBox1.Text.Trim();  // Lấy giá trị từ ô tìm kiếm

            if (string.IsNullOrEmpty(keyword))
            {
                MessageBox.Show("Vui lòng nhập từ khóa tìm kiếm.");
                return;
            }

            // Gọi phương thức tìm kiếm trong nhanVienService
            DataTable result = nhanVienService.SearchNhanVien(keyword);

            // Cập nhật lại danh sách nhân viên
            listView1.Items.Clear();
            foreach (DataRow row in result.Rows)
            {
                ListViewItem item = new ListViewItem(row[0].ToString());
                for (int i = 1; i < row.Table.Columns.Count; i++)
                {
                    item.SubItems.Add(row[i].ToString());
                }
                listView1.Items.Add(item);
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            DataTable danhSachNhanVien = nhanVienService.GetAll();
            if (danhSachNhanVien != null && danhSachNhanVien.Rows.Count > 0)
            {

                ExportHelper.ExportExcel(danhSachNhanVien, "danhSachNhanVien", "danhSachNhanVien.xlsx");
            }
            else
            {
                MessageBox.Show("Không có dữ liệu để xuất!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}

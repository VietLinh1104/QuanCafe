using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QuanCafe.Services;
using System.IO;
using QuanCafe.Helpers;
namespace QuanCafe.Forms.ManagerTab.Tab
{
    public partial class KhachHang : UserControl
    {
        private KhachHangService khachHangService;
        private int selectedId = -1;
        public KhachHang()
        {
            InitializeComponent();
            // Cách gọi đầy đủ namespace để tránh xung đột
            //OfficeOpenXml.ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
            khachHangService = new KhachHangService();
            LoadKhachHang();
        }
        private int pageSize = 10; // số khách hàng mỗi trang
        private int currentPage = 1; // trang hiện tại
        private int totalRecords = 0; // tổng số bản ghi
        private int totalPages = 0;   // tổng số trang

        private void ClearForm()
        {
            txtID.Clear();
            txtTen.Clear();
            txtSDT.Clear();
            txtEmail.Clear();
            txtGhiChu.Clear();
            selectedId = -1;
            listView1.SelectedItems.Clear(); // <- THÊM dòng này nếu chưa có
        }
        private void LoadKhachHang(string keyword = "")
        {
            try
            {
                DataTable dt = khachHangService.GetAllKhachHang(keyword);

                // Tính toán phân trang
                totalRecords = dt.Rows.Count;
                totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

                // Lấy dữ liệu của trang hiện tại
                var pageRows = dt.AsEnumerable()
                    .Skip((currentPage - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                // Hiển thị lên ListView
                listView1.Items.Clear();
                listView1.View = View.Details;
                listView1.FullRowSelect = true;
                listView1.Columns.Clear();

                listView1.Columns.Add("ID", 50);
                listView1.Columns.Add("Tên Khách Hàng", 120);
                listView1.Columns.Add("Số Điện Thoại", 100);
                listView1.Columns.Add("Email", 150);
                listView1.Columns.Add("Ghi Chú", 200);

                foreach (var row in pageRows)
                {
                    ListViewItem item = new ListViewItem(row["id_khach_hang"].ToString());
                    item.SubItems.Add(row["ten_khach_hang"].ToString());
                    item.SubItems.Add(row["so_dien_thoai"].ToString());
                    item.SubItems.Add(row["email"].ToString());
                    item.SubItems.Add(row["ghi_chu"].ToString());
                    listView1.Items.Add(item);
                }
                ColorListViewRows(listView1);
                lblPage.Text = $"Trang {currentPage}/{totalPages}";
                lblTotal.Text = $"Tổng cộng: {totalRecords} khách hàng";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
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

        private void button3_Click(object sender, EventArgs e)
        {
            bool success = khachHangService.AddKhachHang(
            txtTen.Text,
            txtSDT.Text,
            txtEmail.Text,
            txtGhiChu.Text
        );

            MessageBox.Show(success ? "Thêm thành công" : "Thêm thất bại");
            LoadKhachHang();
            ClearForm();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                // lấy thông tin
                var item = listView1.SelectedItems[0];
                selectedId = int.Parse(item.SubItems[0].Text);
                txtID.Text = item.SubItems[0].Text;
                txtTen.Text = item.SubItems[1].Text;
                txtSDT.Text = item.SubItems[2].Text;
                txtEmail.Text = item.SubItems[3].Text;
                txtGhiChu.Text = item.SubItems[4].Text;

                // bật nút sửa/xóa
                btnSua.Enabled = true;
                btnXoa.Enabled = true;
                btnHuy.Enabled = true;
            }
            else
            {
                btnSua.Enabled = false;
                btnXoa.Enabled = false;
                btnHuy.Enabled = false;
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (selectedId == -1)
            {
                MessageBox.Show("Vui lòng chọn khách hàng cần sửa.");
                return;
            }

            bool success = khachHangService.UpdateKhachHang(
                selectedId,
                txtTen.Text,
                txtSDT.Text,
                txtEmail.Text,
                txtGhiChu.Text
            );

            MessageBox.Show(success ? "Cập nhật thành công" : "Cập nhật thất bại");
            LoadKhachHang();
            ClearForm();
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (selectedId == -1)
            {
                MessageBox.Show("Vui lòng chọn khách hàng cần xóa.");
                return;
            }

            DialogResult confirm = MessageBox.Show("Xác nhận xóa?", "Xác nhận", MessageBoxButtons.YesNo);
            if (confirm == DialogResult.Yes)
            {
                bool success = khachHangService.DeleteKhachHang(selectedId);
                MessageBox.Show(success ? "Xóa thành công" : "Xóa thất bại");
                LoadKhachHang();
                ClearForm();
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            txtTimKiem.Text = "";
            currentPage = 1;
            LoadKhachHang();
        }

        private void btnTimKiem_Click_1(object sender, EventArgs e)
        {
            string keyword = txtTimKiem.Text.Trim();
            currentPage = 1; // reset về trang đầu
            LoadKhachHang(keyword);
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                LoadKhachHang(txtTimKiem.Text.Trim());
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (currentPage < totalPages)
            {
                currentPage++;
                LoadKhachHang(txtTimKiem.Text.Trim());
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ClearForm();
            listView1.SelectedItems.Clear(); // bỏ chọn trên ListView nếu có
        }

        private void btnXuatExcel_Click(object sender, EventArgs e)
        {
            DataTable danhSachKhachHang = khachHangService.GetAllKhachHang();
            if (danhSachKhachHang != null && danhSachKhachHang.Rows.Count > 0)
            {

                ExportHelper.ExportExcel(danhSachKhachHang, "danhSachKhachHang", "danhSachKhachHang.xlsx");
            }
            else
            {
                MessageBox.Show("Không có dữ liệu để xuất!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}

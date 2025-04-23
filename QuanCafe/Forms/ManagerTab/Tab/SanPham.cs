using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using QuanCafe.Services;
using QuanCafe.Repositories;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;


namespace QuanCafe.Forms.ManagerTab.Tab
{
    public partial class SanPham : UserControl
    {
        private readonly SanPhamService _sanPhamService;
        private readonly DanhMucService _danhMucService;
        private bool isAdding;

        public SanPham()
        {
            InitializeComponent();
            _sanPhamService = new SanPhamService();
            _danhMucService = new DanhMucService(); // Khởi tạo DanhMucService
            LoadDanhMuc();
            LoadDanhMucChiTietSanPham(); // Gọi phương thức load danh mục chi tiết
            LoadData();
            SetControlStateDefault();
            listBoxDanhMuc.SelectedIndexChanged += listBoxDanhMuc_SelectedIndexChanged;   
        }

        private void SetControlStateDefault()
        {
            btnThem.Enabled = true;
            btnSua.Enabled = listViewSanPham.SelectedItems.Count > 0;
            btnXoa.Enabled = listViewSanPham.SelectedItems.Count > 0;
            btnGhi.Enabled = false;
            btnHuy.Enabled = false;

            txtID.Enabled = false;
            txtTenSanPham.Enabled = false;
            txtDonGia.Enabled = false;
            comboBoxDanhMucSanPham.Enabled = false;
            txtMoTa.Enabled = false;
        }

        private void SetControlStateForEdit(bool isAdding)
        {
            this.isAdding = isAdding;
            // Disable các nút chức năng
            btnThem.Enabled = false;
            btnSua.Enabled = false;
            btnXoa.Enabled = false;
            // Enable nút Ghi và Hủy
            btnGhi.Enabled = true;
            btnHuy.Enabled = true;

            // Enable các control nhập liệu
            txtTenSanPham.Enabled = true;
            txtDonGia.Enabled = true;
            comboBoxDanhMucSanPham.Enabled = true;
            txtMoTa.Enabled = true;

            // Xử lý trường ID
            txtID.Enabled = false; // Luôn disable
            if (isAdding) txtID.Clear();

            txtTenSanPham.Focus();
        }

        // Load danh mục từ CSDL vào ListBox
        private void LoadDanhMuc()
        {
            DataTable danhMucData = _danhMucService.GetAllDanhMuc();
            // Thêm dòng "Tất cả" vào DataTable
            DataRow rowAll = danhMucData.NewRow();
            rowAll["id_danh_muc"] = -1;
            rowAll["ten_danh_muc"] = "Tất cả";
            danhMucData.Rows.InsertAt(rowAll, 0);

            // Load vào ComboBox Danh mục (Thêm/Sửa)
            comboBoxDanhMucSanPham.DataSource = danhMucData;
            comboBoxDanhMucSanPham.DisplayMember = "ten_danh_muc";
            comboBoxDanhMucSanPham.ValueMember = "id_danh_muc";
            comboBoxDanhMucSanPham.SelectedIndex = -1;

            // Load vào ListBox Danh mục để lọc
            listBoxDanhMuc.DataSource = danhMucData.Copy();
            listBoxDanhMuc.DisplayMember = "ten_danh_muc";
            listBoxDanhMuc.ValueMember = "id_danh_muc";
        }



        private void LoadData()
        {
            try
            {
                DataTable dt = _sanPhamService.GetAllSanPham();

                listViewSanPham.Items.Clear();
                listViewSanPham.View = View.Details;
                listViewSanPham.Columns.Clear();
                listViewSanPham.FullRowSelect = true;


                listViewSanPham.Columns.Add("id", 50);
                listViewSanPham.Columns.Add("Tên sản phẩm", 100);
                listViewSanPham.Columns.Add("Đơn giá", 50);
                listViewSanPham.Columns.Add("Danh Mục", 50);
                listViewSanPham.Columns.Add("Mô tả", 200);

                foreach (DataRow row in dt.Rows)
                {
                    ListViewItem item = new ListViewItem(row[0].ToString());

                    for (int i = 1; i < dt.Columns.Count; i++)
                    {
                        item.SubItems.Add(row[i].ToString());
                    }

                    listViewSanPham.Items.Add(item);
                }
                // Tự động vô hiệu hóa nút nếu không có item
                btnXoa.Enabled = listViewSanPham.Items.Count > 0 && listViewSanPham.SelectedItems.Count > 0;
                btnSua.Enabled = listViewSanPham.Items.Count > 0 && listViewSanPham.SelectedItems.Count > 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }


        }

        private void SanPham_Load(object sender, EventArgs e)
        {

        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtTenSanPham.Text))
            {
                MessageBox.Show("Vui lòng nhập tên sản phẩm.");
                return false;
            }

            if (!decimal.TryParse(txtDonGia.Text, out decimal gia) || gia <= 0)
            {
                MessageBox.Show("Đơn giá không hợp lệ.");
                return false;
            }

            if(comboBoxDanhMucSanPham.SelectedValue == null ||
    !int.TryParse(comboBoxDanhMucSanPham.SelectedValue.ToString(), out int dmId))
            {
                MessageBox.Show("Vui lòng chọn danh mục.");
                return false;
            }
            return true; // Thêm dòng này
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            ClearForm();
            SetControlStateForEdit(true); // Trạng thái Thêm mới
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (listViewSanPham.SelectedItems.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn sản phẩm cần xóa.");
                return;
            }

            int id = int.Parse(listViewSanPham.SelectedItems[0].SubItems[0].Text);
            var confirm = MessageBox.Show("Bạn có chắc chắn muốn xóa sản phẩm này?", "Xác nhận", MessageBoxButtons.YesNo);
            if (confirm == DialogResult.Yes)
            {
                bool success = _sanPhamService.XoaSanPham(id);
                if (success)
                {
                    MessageBox.Show("Xóa sản phẩm thành công!");
                    LoadData();
                    ClearForm();
                    SetControlStateDefault();
                }
                else
                {
                    MessageBox.Show("Xóa sản phẩm thất bại!");
                }
            }
        }

        private void ClearForm()
        {
            txtID.Clear();
            txtTenSanPham.Clear();
            txtDonGia.Clear();
            txtMoTa.Clear();
            comboBoxDanhMucSanPham.SelectedIndex = -1;
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (listViewSanPham.SelectedItems.Count > 0)
            {
                SetControlStateForEdit(false); // Trạng thái Sửa
            }
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            try
            {
                string keyword = txtTimKiem.Text.Trim();
                decimal? giaTu = null;
                decimal? giaDen = null;              

                // Parse giá từ/đến
                if (decimal.TryParse(txtGiaTu.Text, out decimal giaTuValue))
                    giaTu = giaTuValue;

                if (decimal.TryParse(txtGiaDen.Text, out decimal giaDenValue))
                    giaDen = giaDenValue;

                // Xử lý khi chọn "Tất cả"              
                // Gọi service tìm kiếm
                DataTable dt = _sanPhamService.TimKiemSanPham(keyword, giaTu, giaDen);

                // Hiển thị kết quả
                listViewSanPham.Items.Clear();
                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("Không tìm thấy sản phẩm phù hợp!");
                    return;
                }

                foreach (DataRow row in dt.Rows)
                {
                    ListViewItem item = new ListViewItem(row["id_san_pham"].ToString());
                    item.SubItems.Add(row["ten_san_pham"].ToString());
                    decimal gia = Convert.ToDecimal(row["gia"]);
                    item.SubItems.Add(gia.ToString("N0") + " VND");
                    item.SubItems.Add(row["ten_danh_muc"].ToString());
                    item.SubItems.Add(row["mo_ta"].ToString());
                    listViewSanPham.Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}");
            }

        }

        private void btnGhi_Click(object sender, EventArgs e)
        {
            if (!ValidateInput()) return;

            string ten = txtTenSanPham.Text;
            decimal gia = decimal.Parse(txtDonGia.Text);
            int dmId = (int)comboBoxDanhMucSanPham.SelectedValue;
            string moTa = txtMoTa.Text;

            bool success;
            if (isAdding)
            {
                success = _sanPhamService.ThemSanPham(ten, gia, dmId, moTa);
            }
            else
            {
                int id = int.Parse(txtID.Text);
                success = _sanPhamService.SuaSanPham(id, ten, gia, dmId, moTa);
            }

            if (success)
            {
                MessageBox.Show(isAdding ? "Thêm sản phẩm thành công!" : "Sửa sản phẩm thành công!");
                LoadData();
                SetControlStateDefault();
                ClearForm();
            }      
            else
            {
                MessageBox.Show("Sửa sản phẩm thất bại!");
            }
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            // Reset trạng thái về mặc định
            SetControlStateDefault();

            // Xóa dữ liệu nhập nếu đang ở chế độ thêm mới
            if (isAdding)
            {
                ClearForm();
            }
            else
            {
                // Nếu đang sửa, load lại dữ liệu từ item được chọn
                if (listViewSanPham.SelectedItems.Count > 0)
                {
                    ListViewItem selectedItem = listViewSanPham.SelectedItems[0];
                    txtID.Text = selectedItem.SubItems[0].Text;
                    txtTenSanPham.Text = selectedItem.SubItems[1].Text;

                    string giaText = selectedItem.SubItems[2].Text
                        .Replace(" VND", "")
                        .Replace(",", "");
                    txtDonGia.Text = giaText;

                    comboBoxDanhMucSanPham.SelectedValue = GetDanhMucIdByName(selectedItem.SubItems[3].Text);
                    txtMoTa.Text = selectedItem.SubItems[4].Text;
                }
            }

            // Đặt lại trạng thái nút
            btnThem.Enabled = true;
            btnSua.Enabled = listViewSanPham.SelectedItems.Count > 0;
            btnXoa.Enabled = listViewSanPham.SelectedItems.Count > 0;
        }

        private void listBoxDanhMuc_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxDanhMuc.SelectedValue != null &&
        int.TryParse(listBoxDanhMuc.SelectedValue.ToString(), out int danhMucId))
            {
                DataTable dt;

                // Kiểm tra nếu chọn "Tất cả" (id = -1)
                if (danhMucId == -1)
                {
                    dt = _sanPhamService.GetAllSanPham(); // Lấy tất cả sản phẩm
                }
                else
                {
                    dt = _sanPhamService.GetSanPhamByDanhMuc(danhMucId); // Lấy theo danh mục
                }

                // Hiển thị kết quả lên ListView
                listViewSanPham.Items.Clear();
                foreach (DataRow row in dt.Rows)
                {
                    ListViewItem item = new ListViewItem(row["id_san_pham"].ToString());
                    item.SubItems.Add(row["ten_san_pham"].ToString());
                    decimal gia = Convert.ToDecimal(row["gia"]);
                    item.SubItems.Add(gia.ToString("N0") + " VND");
                    item.SubItems.Add(row["ten_danh_muc"].ToString());
                    item.SubItems.Add(row["mo_ta"].ToString());
                    listViewSanPham.Items.Add(item);
                }
            }
        }

        private void listViewSanPham_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewSanPham.SelectedItems.Count > 0)
            {
                ListViewItem selectedItem = listViewSanPham.SelectedItems[0];
                txtID.Text = selectedItem.SubItems[0].Text;
                txtTenSanPham.Text = selectedItem.SubItems[1].Text;
                // Xử lý giá tiền: "10,000 VND" → "10000"
                string giaText = selectedItem.SubItems[2].Text
                    .Replace(" VND", "")
                    .Replace(",", "");
                txtDonGia.Text = giaText;
                // Hiển thị danh mục theo ID (thay vì tên)
                comboBoxDanhMucSanPham.SelectedValue = GetDanhMucIdByName(selectedItem.SubItems[3].Text);
                txtMoTa.Text = selectedItem.SubItems[4].Text;
            
            btnXoa.Enabled = true;
            btnSua.Enabled = true;
        }
    else
    {
        btnXoa.Enabled = false;
        btnSua.Enabled = false;
    }
}

        // Hàm hỗ trợ lấy ID danh mục từ tên
        private int GetDanhMucIdByName(string tenDanhMuc)
        {
            foreach (DataRowView item in comboBoxDanhMucSanPham.Items)
            {
                if (item["ten_danh_muc"].ToString() == tenDanhMuc)
                    return Convert.ToInt32(item["id_danh_muc"]);
            }
            return -1;
        }

        private void LoadSanPham()
        {
            DataTable dt = _sanPhamService.GetAllSanPham(); // Sửa _sanPhamService → sanPhamService
            //LoadSanPhamToListView(dt);
        }

        // Trong phương thức khởi tạo hoặc Load form
        private void LoadDanhMucChiTietSanPham()
        {
            DataTable danhMucData = _danhMucService.GetAllDanhMuc();
            // Trong constructor, thêm dòng này:
            comboBoxDanhMucSanPham.DataSource = danhMucData;
            comboBoxDanhMucSanPham.DisplayMember = "ten_danh_muc";
            comboBoxDanhMucSanPham.ValueMember = "id_danh_muc";
            comboBoxDanhMucSanPham.SelectedIndex = -1;
        }
        private void comboBoxDanhMuc_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
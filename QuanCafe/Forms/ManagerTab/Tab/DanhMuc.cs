using QuanCafe.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanCafe.Forms.ManagerTab.Tab
{

    public partial class DanhMuc : UserControl
    {
        private DanhMucService _service;
        public DanhMuc()
        {
            InitializeComponent();
            _service = new DanhMucService();
            LoadDanhMuc();
            this.txtTimKiem.TextChanged += new System.EventHandler(this.txtTimKiem_TextChanged);

        }

        private DataTable danhMucDataTable;

        private bool isAdding = false;
        private bool isEditing = false;


        public void LoadDanhMuc()
        {
            try
            {
                danhMucDataTable = _service.GetAllDanhMuc(); // ← lưu danh sách gốc

                listView1.Items.Clear();
                listView1.View = View.Details;
                listView1.Columns.Clear();
                listView1.FullRowSelect = true;

                listView1.Columns.Add("ID", 50);
                listView1.Columns.Add("Tên danh mục", 150);
                listView1.Columns.Add("Mô tả", 250);

                foreach (DataRow row in danhMucDataTable.Rows)
                {
                    ListViewItem item = new ListViewItem(row["id_danh_muc"].ToString());
                    item.SubItems.Add(row["ten_danh_muc"].ToString());
                    item.SubItems.Add(row["mo_ta"]?.ToString() ?? "");
                    listView1.Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi load danh mục: " + ex.Message);
            }
        }


        private void UpdateButtonStates()
        {
            bool hasSelection = listView1.SelectedItems.Count > 0;

            // Chỉ bật "Thêm" khi không đang sửa/thêm và không có dòng nào được chọn
            btnThem.Enabled = !isEditing && !isAdding && !hasSelection;

            // "Sửa" và "Xóa" chỉ bật khi có dòng được chọn và không đang thêm
            btnSua.Enabled = hasSelection && !isAdding;
            btnXoa.Enabled = hasSelection && !isAdding;

            // "Ghi" bật khi đang sửa hoặc thêm
            btnGhi.Enabled = isEditing || isAdding;

            // "Hoàn tác" bật nếu đang sửa, đang thêm, hoặc có dòng được chọn
            btnHoanTac.Enabled = isEditing || isAdding || hasSelection;
        }

        private void ClearInputFields()
        {
            txtID.Text = "";
            txtTenDanhMuc.Text = "";
            txtMoTa.Text = "";
        }

        private void ResetFormState()
        {
            isAdding = false;
            isEditing = false;
            UpdateButtonStates();
            ClearInputFields();
        }


        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0) return;

            var item = listView1.SelectedItems[0];

            // Gán dữ liệu lên TextBox
            txtID.Text = item.SubItems[0].Text;
            txtTenDanhMuc.Text = item.SubItems[1].Text;
            txtMoTa.Text = item.SubItems[2].Text;

            // Đặt chế độ chỉ đọc
            txtID.ReadOnly = true;
            txtTenDanhMuc.ReadOnly = true;
            txtMoTa.ReadOnly = true;

            // Reset trạng thái đang thêm/sửa
            isAdding = false;
            isEditing = false;

            // Cập nhật trạng thái các nút
            UpdateButtonStates();
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            ClearInputFields();

            // Lấy ID mới và hiển thị
            int nextId = _service.GetNextDanhMucId();
            txtID.Text = nextId.ToString();
            txtID.ReadOnly = true;

            // Cho phép nhập các trường còn lại
            txtTenDanhMuc.ReadOnly = false;
            txtMoTa.ReadOnly = false;

            isAdding = true;
            isEditing = false;

            listView1.SelectedItems.Clear();
            UpdateButtonStates();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            txtTenDanhMuc.ReadOnly = false;
            txtMoTa.ReadOnly = false;

            isEditing = true;
            isAdding = false;

            UpdateButtonStates();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                int id = int.Parse(listView1.SelectedItems[0].SubItems[0].Text);
                var confirm = MessageBox.Show("Bạn có chắc muốn xóa danh mục này?", "Xác nhận", MessageBoxButtons.YesNo);
                if (confirm == DialogResult.Yes)
                {
                    bool result = _service.DeleteDanhMuc(id);
                    if (result)
                    {
                        MessageBox.Show("Xóa thành công!");
                        LoadDanhMuc();
                        ResetFormState();
                    }
                    else
                    {
                        MessageBox.Show("Xóa thất bại!");
                    }
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string tenDanhMuc = txtTenDanhMuc.Text.Trim();
            string moTa = txtMoTa.Text.Trim();

            if (string.IsNullOrEmpty(tenDanhMuc))
            {
                MessageBox.Show("Tên danh mục không được để trống!");
                return;
            }

            bool result = false;

            if (isAdding)
            {
                result = _service.AddDanhMuc(tenDanhMuc, moTa);
            }
            else if (isEditing)
            {
                result = _service.UpdateDanhMuc(int.Parse(txtID.Text), tenDanhMuc, moTa);
            }

            if (result)
            {
                MessageBox.Show("Lưu thành công!");
                LoadDanhMuc();  // Cập nhật lại danh sách danh mục sau khi thêm hoặc sửa
                ResetFormState();
            }
            else
            {
                MessageBox.Show("Lưu thất bại! Vui lòng kiểm tra lại kết nối cơ sở dữ liệu.");
            }
        }

        private void txtTimKiem_TextChanged(object sender, EventArgs e)
        {
            string keyword = txtTimKiem.Text.Trim().ToLower();

            if (string.IsNullOrEmpty(keyword))
            {
                // Nếu không có từ khóa, hiển thị lại toàn bộ dữ liệu
                LoadDanhMuc();
                return;
            }

            // Nếu có từ khóa, thực hiện tìm kiếm như btnTimKiem_Click
            listView1.Items.Clear();

            foreach (DataRow row in danhMucDataTable.Rows)
            {
                string id = row["id_danh_muc"].ToString().ToLower();
                string ten = row["ten_danh_muc"].ToString().ToLower();
                string moTa = row["mo_ta"]?.ToString().ToLower() ?? "";

                if (id.Contains(keyword) || ten.Contains(keyword) || moTa.Contains(keyword))
                {
                    ListViewItem item = new ListViewItem(row["id_danh_muc"].ToString());
                    item.SubItems.Add(row["ten_danh_muc"].ToString());
                    item.SubItems.Add(row["mo_ta"]?.ToString() ?? "");
                    listView1.Items.Add(item);
                }
            }
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            string keyword = txtTimKiem.Text.Trim().ToLower();

            if (danhMucDataTable == null || keyword == "")
            {
                LoadDanhMuc(); // Hiện lại toàn bộ nếu không nhập gì
                return;
            }

            listView1.Items.Clear();

            foreach (DataRow row in danhMucDataTable.Rows)
            {
                string id = row["id_danh_muc"].ToString().ToLower();
                string ten = row["ten_danh_muc"].ToString().ToLower();
                string moTa = row["mo_ta"]?.ToString().ToLower() ?? "";

                if (id.Contains(keyword) || ten.Contains(keyword) || moTa.Contains(keyword))
                {
                    ListViewItem item = new ListViewItem(row["id_danh_muc"].ToString());
                    item.SubItems.Add(row["ten_danh_muc"].ToString());
                    item.SubItems.Add(row["mo_ta"]?.ToString() ?? "");
                    listView1.Items.Add(item);
                }
            }
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            ResetFormState();
            LoadDanhMuc();
            listView1.SelectedItems.Clear();

            txtTenDanhMuc.ReadOnly = true;
            txtMoTa.ReadOnly = true;
        }
    }
}

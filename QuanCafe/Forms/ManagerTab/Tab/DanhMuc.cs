using QuanCafe.Models;
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
using ClosedXML.Excel;
using QuanCafe.Helpers;



namespace QuanCafe.Forms.ManagerTab.Tab
{

    public partial class DanhMuc : UserControl
    {
        private DanhMucService _service;
        private DataTable danhMucDataTable;
        public DanhMuc()
        {
            InitializeComponent();
            _service = new DanhMucService();
            LoadComboBoxFilter();
            LoadDanhMuc();
            this.txtTimKiem.TextChanged += new System.EventHandler(this.txtTimKiem_TextChanged);
            this.cboDanhMuc.SelectedIndexChanged += new System.EventHandler(this.cboDanhMuc_SelectedIndexChanged);
            cboDanhMuc.SelectedIndexChanged += cboDanhMuc_SelectedIndexChanged;

            /*🌟 Bo góc cho TextBox
            RoundedControl.ApplyRoundedBorder(txtTimKiem, 5);
            RoundedControl.ApplyRoundedBorder(txtID, 5);
            RoundedControl.ApplyRoundedBorder(txtTenDanhMuc, 5);
            RoundedControl.ApplyRoundedBorder(txtMoTa, 5);

            // 🌟 Bo góc cho Button
            RoundedControl.ApplyRoundedBorder(btnThem, 10);
            RoundedControl.ApplyRoundedBorder(btnSua, 10);
            RoundedControl.ApplyRoundedBorder(btnXoa, 10);
            RoundedControl.ApplyRoundedBorder(btnGhi, 10);
            RoundedControl.ApplyRoundedBorder(btnHoanTac, 10);
            RoundedControl.ApplyRoundedBorder(btnTimKiem, 10);
            RoundedControl.ApplyRoundedBorder(button1, 10);
            RoundedControl.ApplyRoundedBorder(panel1, 10);
           */
            
        }



        private bool isAdding = false;
        private bool isEditing = false;
        private int pageSize = 22;  // Số bản ghi mỗi trang
        private int currentPage = 1; // Trang hiện tại
        private int totalRecords = 0;  // Tổng số bản ghi
        private int totalPages = 0;   // Tổng số trang






        public void LoadDanhMuc()
        {
            try
            {
                // Lấy toàn bộ danh mục từ cơ sở dữ liệu
                danhMucDataTable = _service.GetAllDanhMuc();

                // Lọc theo ComboBox nếu không phải "Tất cả"
                string selectedFilter = cboDanhMuc.SelectedItem?.ToString();
                IEnumerable<DataRow> filteredRows = danhMucDataTable.AsEnumerable();

                if (!string.IsNullOrEmpty(selectedFilter) && selectedFilter != "Tất cả")
                {
                    filteredRows = filteredRows
                        .Where(row => row.Field<string>("ten_danh_muc") == selectedFilter);
                }

                // Cập nhật lại danh sách sau khi lọc
                var filteredList = filteredRows.ToList();

                // Phân trang
                totalRecords = filteredList.Count;
                totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

                // Lấy dữ liệu trang hiện tại
                var pageData = filteredList
                    .Skip((currentPage - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();


                // Cập nhật ListView
                listView1.Items.Clear();
                listView1.View = View.Details;
                listView1.Columns.Clear();
                listView1.FullRowSelect = true;

                listView1.Columns.Add("ID", 50);
                listView1.Columns.Add("Tên danh mục", 280);
                listView1.Columns.Add("Mô tả", 355);

                foreach (DataRow row in pageData)
                {
                    ListViewItem item = new ListViewItem(row["id_danh_muc"].ToString());
                    item.SubItems.Add(row["ten_danh_muc"].ToString());
                    item.SubItems.Add(row["mo_ta"]?.ToString() ?? "");
                    listView1.Items.Add(item);
                }

               
                // Gọi tô màu dòng sau khi load xong
                ColorListViewRows(listView1);
                // Cập nhật số trang
                lblPage.Text = $"Trang {currentPage}/{totalPages}";
                lblTotal.Text = $"Tổng cộng: {totalRecords} danh mục";
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
            lblTotal.Text = $"Tổng cộng: {totalRecords} danh mục";

        }


        private void LoadComboBoxFilter()
        {
            try
            {
                cboDanhMuc.Items.Clear();

                // Lấy dữ liệu danh mục từ DB (hoặc từ danhMucDataTable nếu đã có)
                DataTable table = _service.GetAllDanhMuc(); // hoặc dùng: danhMucDataTable

                // Lấy danh sách tên danh mục không trùng lặp
                var tenDanhMucList = table.AsEnumerable()
                    .Select(row => row.Field<string>("ten_danh_muc"))
                    .Distinct()
                    .OrderBy(name => name)
                    .ToList();

                // Thêm mục "Tất cả"
                cboDanhMuc.Items.Add("Tất cả");

                // Thêm các mục danh mục thực tế
                foreach (var ten in tenDanhMucList)
                {
                    cboDanhMuc.Items.Add(ten);
                }

                // Chọn mặc định là "Tất cả"
                cboDanhMuc.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải ComboBox lọc: " + ex.Message);
            }
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
                        LoadComboBoxFilter();
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

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            if (currentPage < totalPages)
            {
                currentPage++;
                LoadDanhMuc();
            }
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                LoadDanhMuc();
            }
        }

        private void cboDanhMuc_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selected = cboDanhMuc.SelectedItem?.ToString() ?? "Tất cả";
            LoadDanhMuc();
        }

        private void txtID_TextChanged(object sender, EventArgs e)
        {

        }


        private void ExportExcel(DataTable dataTable)
        {
            try
            {
                using (XLWorkbook wb = new XLWorkbook())
                {
                    // Thêm DataTable vào sheet "DanhMuc"
                    wb.Worksheets.Add(dataTable, "DanhMuc");

                    // Tạo hộp thoại lưu file
                    SaveFileDialog saveFileDialog = new SaveFileDialog();
                    saveFileDialog.Filter = "Excel Workbook|*.xlsx";
                    saveFileDialog.Title = "Lưu file Excel";
                    saveFileDialog.FileName = "DanhMucSanPham.xlsx";

                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        // Lưu file tại vị trí người dùng chọn
                        wb.SaveAs(saveFileDialog.FileName);
                        MessageBox.Show("Xuất file Excel thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xuất Excel:\n" + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void button1_Click_1(object sender, EventArgs e)
        {
            if (danhMucDataTable != null && danhMucDataTable.Rows.Count > 0)
            {
 
                ExportHelper.ExportExcel(danhMucDataTable, "DanhMucSanPham", "DanhMucSanPham.xlsx");
            }
            else
            {
                MessageBox.Show("Không có dữ liệu để xuất!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void txtTenDanhMuc_TextChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }
    }
}

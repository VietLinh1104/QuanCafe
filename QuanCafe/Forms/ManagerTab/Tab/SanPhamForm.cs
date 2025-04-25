using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using QuanCafe.Services;
using QuanCafe.Repositories;
using System.Drawing.Printing;
using ClosedXML.Excel;


namespace QuanCafe.Forms.ManagerTab.Tab
{
    public partial class SanPhamForm : UserControl
    {
        // Khai báo các service và biến thành viên
        private readonly SanPhamService _sanPhamService;
        private readonly DanhMucService _danhMucService;
        private bool isAdding;          // Trạng thái thêm/sửa
        private int currentPage = 1;    // Trang hiện tại
        private int pageSize = 25;      // Số item mỗi trang
        private int totalRecords = 0;   // Tổng số bản ghi
        private int totalPages = 1;     // Tổng số trang

        public SanPhamForm()
        {
            InitializeComponent(); // Khởi tạo các control từ Designer

            _sanPhamService = new SanPhamService();
            _danhMucService = new DanhMucService();

            // Thiết lập ListView
            listViewSanPham.View = View.Details;    // Hiển thị dạng bảng
            listViewSanPham.FullRowSelect = true;   // Chọn cả dòng
            listViewSanPham.HideSelection = false;  // Giữ highlight khi mất focus

            // Gắn sự kiện
            listViewSanPham.SelectedIndexChanged += listViewSanPham_SelectedIndexChanged;
            listBoxDanhMuc.SelectedIndexChanged += listBoxDanhMuc_SelectedIndexChanged;
            btnPrev.Click += btnPrev_Click;
            btnNext.Click += btnNext_Click;

            // Khởi tạo phân trang
            currentPage = 1;
            pageSize = 25;
            totalRecords = 0;
            totalPages = 1;

            // Tải dữ liệu ban đầu
            LoadDanhMuc();
            LoadDanhMucChiTietSanPham();
            LoadData();
            SetControlStateDefault();

            // Thiết lập tham số tìm kiếm mặc định
            _currentKeyword = "";
            _currentGiaTu = null;
            _currentGiaDen = null;
            _currentDanhMucId = null;
        }
        //Các phương thức quản lý trạng thái control
        private void SetControlStateDefault()
        {
            // Thiết lập trạng thái mặc định của các control
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
            this.isAdding = isAdding;   // Lưu trạng thái hiện tại
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
            txtID.Enabled = false;          // Luôn disable
            if (isAdding) txtID.Clear();    // Xóa ID khi thêm mới

            txtTenSanPham.Focus();          // Focus vào ô tên sản phẩm
        }

        
        // Load danh mục từ CSDL vào ListBox
        private void LoadDanhMuc()
        {
            // Lấy danh sách danh mục từ service
            DataTable danhMucData = _danhMucService.GetAllDanhMuc();

            // Thêm dòng "Tất cả" vào DataTable
            DataRow rowAll = danhMucData.NewRow();
            rowAll["id_danh_muc"] = -1;
            rowAll["ten_danh_muc"] = "Tất cả";
            danhMucData.Rows.InsertAt(rowAll, 0);

            // Lưu lại selected value nếu có
            object selectedValue = comboBoxDanhMucSanPham.SelectedValue;

            // Load vào ComboBox Danh mục (Thêm/Sửa)
            comboBoxDanhMucSanPham.DataSource = danhMucData;
            comboBoxDanhMucSanPham.DisplayMember = "ten_danh_muc";
            comboBoxDanhMucSanPham.ValueMember = "id_danh_muc";

            // Khôi phục selected value nếu có
            if (selectedValue != null)
            {
                comboBoxDanhMucSanPham.SelectedValue = selectedValue;
            }
            else
            {
                comboBoxDanhMucSanPham.SelectedIndex = -1;
            }

            // Load vào ListBox Danh mục để lọc
            listBoxDanhMuc.DataSource = danhMucData.Copy();
            listBoxDanhMuc.DisplayMember = "ten_danh_muc";
            listBoxDanhMuc.ValueMember = "id_danh_muc";

        }

        // Cập nhật phương thức LoadData
        private void LoadData()
        {
            try
            {
                // Lấy dữ liệu phân trang từ service
                var dt = _sanPhamService.GetAllSanPhamPaged(currentPage, pageSize, out totalRecords);
                totalPages = Math.Max(1, (int)Math.Ceiling((double)totalRecords / pageSize));

                // ==== hiển thị lên ListView ====
                listViewSanPham.View = View.Details;
                listViewSanPham.Columns.Clear();
                listViewSanPham.Columns.Add("ID", 40);
                listViewSanPham.Columns.Add("Tên sản phẩm", 120);
                listViewSanPham.Columns.Add("Đơn giá", 100);
                listViewSanPham.Columns.Add("Danh mục", 100);
                listViewSanPham.Columns.Add("Mô tả", 180);

                // Đổ dữ liệu vào ListView
                listViewSanPham.Items.Clear();
                foreach (DataRow row in dt.Rows)
                {
                    var item = new ListViewItem(row["id_san_pham"].ToString());
                    item.SubItems.Add(row["ten_san_pham"].ToString());
                    item.SubItems.Add(Convert.ToDecimal(row["gia"]).ToString("N0") + " VND");
                    item.SubItems.Add(row["ten_danh_muc"].ToString());
                    item.SubItems.Add(row["mo_ta"].ToString());
                    listViewSanPham.Items.Add(item);
                }
                ColorListViewRows(listViewSanPham);
                UpdatePageInfo();   // Cập nhật thông tin trang
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu: " + ex.Message);
            }
        }

        private void ColorListViewRows(System.Windows.Forms.ListView listView)
        {
            for (int i = 0; i < listView.Items.Count; i++)
            {
                listView.Items[i].BackColor = (i % 2 == 0) ? Color.White : Color.LightGray;
                listView.Items[i].UseItemStyleForSubItems = true;
            }
        }

        //Các phương thức phân trang
        private void UpdatePageInfo()
        {
            lblPageInfo.Text = $"Trang {currentPage}/{totalPages}";
        }

        private void SetPaginationButtonsState()
        {
            btnPrev.Enabled = currentPage > 1;          // Disable nút Prev nếu ở trang đầu
            btnNext.Enabled = currentPage < totalPages; // Disable nút Next nếu ở trang cuối
        }

        // Xử lý sự kiện
        private void btnPrev_Click(object sender, EventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;          // Giảm số trang
                LoadDataWithSearch();   // Tải lại dữ liệu
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (currentPage < totalPages)
            {
                currentPage++;          // Tăng số trang
                LoadDataWithSearch();   // Tải lại dữ liệu
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

            if (comboBoxDanhMucSanPham.SelectedValue == null ||
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
        private string _currentKeyword = "";
        private decimal? _currentGiaTu = null;
        private decimal? _currentGiaDen = null;
        private int? _currentDanhMucId = null;

        //Các phương thức tìm kiếm
        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            currentPage = 1;    // Reset về trang đầu khi tìm kiếm

            // Lưu trữ tham số tìm kiếm
            _currentKeyword = txtTimKiem.Text.Trim();
            _currentGiaTu = decimal.TryParse(txtGiaTu.Text, out decimal giaTuValue) ? giaTuValue : (decimal?)null;
            _currentGiaDen = decimal.TryParse(txtGiaDen.Text, out decimal giaDenValue) ? giaDenValue : (decimal?)null;
            _currentDanhMucId = listBoxDanhMuc.SelectedValue != null &&
                                int.TryParse(listBoxDanhMuc.SelectedValue.ToString(), out int selectedDanhMucId) ?
                                selectedDanhMucId : (int?)null;

            // Gọi phương thức tìm kiếm
            LoadDataWithSearch();   // Thực hiện tìm kiếm
        }

        private void LoadDataWithSearch()
        {
            try
            {
                // Lấy 1 trang dữ liệu, đồng thời trả totalRecords
                DataTable dt = _sanPhamService.TimKiemSanPhamPaged(
                                    _currentKeyword,
                                    _currentGiaTu,
                                    _currentGiaDen,
                                    _currentDanhMucId,
                                    currentPage,
                                    pageSize,
                                    out totalRecords);

                // Tính tổng số trang (ít nhất =1)
                totalPages = Math.Max(1, (int)Math.Ceiling((double)totalRecords / pageSize));

                // Hiển thị
                DisplayData(dt);
                UpdatePageInfo();            // “Trang x/y”
                SetPaginationButtonsState(); // Bật/tắt Prev/Next
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu: " + ex.Message);
            }
        }




        private void DisplayData(DataTable dt)
        {
            listViewSanPham.Items.Clear();
            foreach (DataRow row in dt.Rows)
            {
                var item = new ListViewItem(row["id_san_pham"].ToString());
                item.SubItems.Add(row["ten_san_pham"].ToString());
                item.SubItems.Add(Convert.ToDecimal(row["gia"]).ToString("N0") + " VND");
                item.SubItems.Add(row["ten_danh_muc"].ToString());
                item.SubItems.Add(row["mo_ta"].ToString());
                listViewSanPham.Items.Add(item);
            }
            ColorListViewRows(listViewSanPham);
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
            if (int.TryParse(listBoxDanhMuc.SelectedValue?.ToString(), out int id))
            {
                _currentDanhMucId = id == -1 ? (int?)null : id;
                currentPage = 1;            // ← reset
                LoadDataWithSearch();
            }
        }

        private void listViewSanPham_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Đảm bảo màu nền khi chọn hiển thị rõ
            foreach (ListViewItem item in listViewSanPham.Items)
            {
                if (item.Selected)
                {
                    item.BackColor = SystemColors.Highlight;
                    item.ForeColor = SystemColors.HighlightText;
                }
                else
                {
                    // Giữ màu xen kẽ khi không được chọn
                    item.BackColor = (item.Index % 2 == 0) ? Color.White : Color.LightGray;
                    item.ForeColor = SystemColors.ControlText;
                }
            }
            // Đảm bảo có item được chọn
            if (listViewSanPham.SelectedItems.Count == 0)
            {
                btnSua.Enabled = false;
                btnXoa.Enabled = false;
                return;
            }
            ListViewItem selectedItem = listViewSanPham.SelectedItems[0];
            try
            {
                // Hiển thị thông tin cơ bản
                txtID.Text = selectedItem.SubItems[0].Text;
                txtTenSanPham.Text = selectedItem.SubItems[1].Text;

                // Xử lý giá - loại bỏ định dạng tiền tệ
                string giaText = selectedItem.SubItems[2].Text
                    .Replace("VND", "")
                    .Replace(",", "")
                    .Replace(".", "")
                    .Trim();
                txtDonGia.Text = giaText;

                // Xử lý danh mục
                string tenDanhMuc = selectedItem.SubItems[3].Text;
                foreach (DataRowView item in comboBoxDanhMucSanPham.Items)
                {
                    if (item["ten_danh_muc"].ToString().Equals(tenDanhMuc, StringComparison.OrdinalIgnoreCase))
                    {
                        comboBoxDanhMucSanPham.SelectedValue = item["id_danh_muc"];
                        break;
                    }
                }

                // Xử lý mô tả
                txtMoTa.Text = selectedItem.SubItems.Count > 4 ? selectedItem.SubItems[4].Text : "";

                // Cập nhật trạng thái nút
                btnSua.Enabled = true;
                btnXoa.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi hiển thị chi tiết: {ex.Message}");
            }
        }
        // Hàm hỗ trợ lấy ID danh mục từ tên
        private int GetDanhMucIdByName(string tenDanhMuc)
        {
            if (comboBoxDanhMucSanPham.Items.Count == 0) return -1;

            foreach (DataRowView item in comboBoxDanhMucSanPham.Items)
            {
                if (item["ten_danh_muc"].ToString() == tenDanhMuc)
                    return Convert.ToInt32(item["id_danh_muc"]);
            }
            return -1;
        }

        private void LoadSanPham()
        {
            DataTable dt = _sanPhamService.GetAllSanPham(); 
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
        //Phương thức xuất Excel 
        private void btnExportExcel_Click(object sender, EventArgs e)
        {
            try
            {
                // Lấy TOÀN BỘ dữ liệu từ service (không phân trang)
                DataTable dtAllData = _sanPhamService.GetAllSanPham(); // Thêm phương thức này trong SanPhamService

                if (dtAllData.Rows.Count == 0)
                {
                    MessageBox.Show("Không có dữ liệu để xuất!", "Cảnh báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("DanhSachSanPham");

                    // ===== TIÊU ĐỀ =====
                    worksheet.Range("A1:F1").Merge();
                    worksheet.Cell("A1").Value = "DANH SÁCH SẢN PHẨM - " + DateTime.Now.ToString("dd/MM/yyyy");
                    worksheet.Cell("A1").Style.Font.Bold = true;
                    worksheet.Cell("A1").Style.Font.FontSize = 16;
                    worksheet.Cell("A1").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    worksheet.Cell("A1").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    worksheet.Cell("A1").Style.Fill.BackgroundColor = XLColor.LightBlue;

                    // ===== HEADER =====
                    string[] headers = { "STT", "ID", "Tên sản phẩm", "Đơn giá (VND)", "Danh mục", "Mô tả" };

                    // Độ rộng các cột
                    worksheet.Column(1).Width = 8;
                    worksheet.Column(2).Width = 10;
                    worksheet.Column(3).Width = 25;
                    worksheet.Column(4).Width = 15;
                    worksheet.Column(5).Width = 20;
                    worksheet.Column(6).Width = 40;

                    // Định dạng header
                    var headerRange = worksheet.Range("A3:F3");
                    headerRange.Style.Font.Bold = true;
                    headerRange.Style.Fill.BackgroundColor = XLColor.LightGray;
                    headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    headerRange.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    headerRange.Style.Border.BottomBorder = XLBorderStyleValues.Thin;

                    // Gán giá trị header
                    for (int col = 0; col < headers.Length; col++)
                    {
                        worksheet.Cell(3, col + 1).Value = headers[col];
                    }

                    // ===== DỮ LIỆU =====
                    for (int i = 0; i < dtAllData.Rows.Count; i++)
                    {
                        DataRow row = dtAllData.Rows[i];
                        int excelRow = i + 4; // Bắt đầu từ dòng 4

                        // STT
                        worksheet.Cell(excelRow, 1).Value = i + 1;
                        worksheet.Cell(excelRow, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                        // ID
                        worksheet.Cell(excelRow, 2).Value = row["id_san_pham"].ToString();

                        // Tên sản phẩm
                        worksheet.Cell(excelRow, 3).Value = row["ten_san_pham"].ToString();

                        // Đơn giá (định dạng số)
                        if (decimal.TryParse(row["gia"].ToString(), out decimal gia))
                        {
                            worksheet.Cell(excelRow, 4).Value = gia;
                            worksheet.Cell(excelRow, 4).Style.NumberFormat.Format = "#,##0";
                        }

                        // Danh mục
                        worksheet.Cell(excelRow, 5).Value = row["ten_danh_muc"].ToString();

                        // Mô tả
                        worksheet.Cell(excelRow, 6).Value = row["mo_ta"].ToString();
                    }

                    // ===== ĐỊNH DẠNG BORDER =====
                    var dataRange = worksheet.Range(3, 1, dtAllData.Rows.Count + 3, headers.Length);
                    dataRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    dataRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

                    // ===== LƯU FILE =====
                    SaveFileDialog saveFile = new SaveFileDialog
                    {
                        Filter = "Excel files (*.xlsx)|*.xlsx",
                        FileName = $"DanhSachSanPham_Full_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx"
                    };

                    if (saveFile.ShowDialog() == DialogResult.OK)
                    {
                        workbook.SaveAs(saveFile.FileName);
                        MessageBox.Show($"Xuất thành công {dtAllData.Rows.Count} sản phẩm!", "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xuất Excel: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtTimKiem_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnResetSearch_Click(object sender, EventArgs e)
        {
            // Reset các control tìm kiếm
            txtTimKiem.Text = string.Empty;
            txtGiaTu.Text = string.Empty;
            txtGiaDen.Text = string.Empty;

            // Reset danh mục về "Tất cả"
            if (listBoxDanhMuc.Items.Count > 0)
            {
                listBoxDanhMuc.SelectedIndex = 0; // Chọn "Tất cả" (dòng đầu tiên)
            }

            // Reset các biến lưu trữ điều kiện tìm kiếm
            _currentKeyword = string.Empty;
            _currentGiaTu = null;
            _currentGiaDen = null;
            _currentDanhMucId = null;

            // Reset về trang đầu tiên
            currentPage = 1;

            // Load lại dữ liệu với điều kiện tìm kiếm đã reset
            LoadDataWithSearch();

            // Focus lại ô tìm kiếm để tiện nhập liệu
            txtTimKiem.Focus();
        }
        // In
        private void btnPrint_Click(object sender, EventArgs e)
        {
            // Tạo PrintDocument
            PrintDocument printDoc = new PrintDocument();
            printDoc.DocumentName = "Danh sách sản phẩm";
            printDoc.PrintPage += new PrintPageEventHandler(PrintPageHandler);

            // Thiết lập PrintDialog
            PrintDialog printDialog = new PrintDialog
            {
                Document = printDoc,
                AllowSomePages = true,
                AllowSelection = false,
                UseEXDialog = true
            };

            // Hiển thị hộp thoại in
            if (printDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // Bắt đầu quá trình in
                    printDoc.Print();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi in: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        // in từng trang
        private void PrintPageHandler(object sender, PrintPageEventArgs e)
        {
            // Font và màu sắc
            Font headerFont = new Font("Arial", 14, FontStyle.Bold);
            Font bodyFont = new Font("Arial", 10);
            Font footerFont = new Font("Arial", 8, FontStyle.Italic);
            Brush brush = Brushes.Black;

            // Căn lề và vị trí
            float leftMargin = e.MarginBounds.Left;
            float topMargin = e.MarginBounds.Top;
            float yPos = topMargin;
            float lineHeight = bodyFont.GetHeight();

            // Tiêu đề
            string title = "DANH SÁCH SẢN PHẨM";
            e.Graphics.DrawString(title, headerFont, brush,
                leftMargin + (e.MarginBounds.Width - e.Graphics.MeasureString(title, headerFont).Width) / 2,
                yPos);
            yPos += lineHeight * 2;

            // Ngày in
            string printDate = $"Ngày in: {DateTime.Now.ToString("dd/MM/yyyy HH:mm")}";
            e.Graphics.DrawString(printDate, footerFont, brush, leftMargin, yPos);
            yPos += lineHeight * 1.5f;

            // Header cột
            string[] headers = { "STT", "Mã SP", "Tên sản phẩm", "Đơn giá", "Danh mục" };
            float[] columnWidths = { 40, 60, 200, 80, 120 };

            // Vẽ header
            for (int i = 0; i < headers.Length; i++)
            {
                e.Graphics.DrawString(headers[i], bodyFont, brush,
                    leftMargin + columnWidths.Take(i).Sum(),
                    yPos);
            }
            yPos += lineHeight * 1.2f;

            // Vẽ đường kẻ ngang dưới header
            e.Graphics.DrawLine(Pens.Black, leftMargin, yPos,
                leftMargin + columnWidths.Sum(), yPos);
            yPos += lineHeight * 0.5f;

            // Dữ liệu sản phẩm
            int startIndex = (int)e.PageSettings.PrinterSettings.FromPage * listViewSanPham.Items.Count /
                            (e.PageSettings.PrinterSettings.ToPage - e.PageSettings.PrinterSettings.FromPage + 1);
            int endIndex = Math.Min(startIndex + 30, listViewSanPham.Items.Count); // ~30 dòng/trang

            for (int i = startIndex; i < endIndex; i++)
            {
                ListViewItem item = listViewSanPham.Items[i];

                // STT
                e.Graphics.DrawString((i + 1).ToString(), bodyFont, brush,
                    leftMargin, yPos);

                // Mã SP
                e.Graphics.DrawString(item.SubItems[0].Text, bodyFont, brush,
                    leftMargin + columnWidths[0], yPos);

                // Tên SP
                e.Graphics.DrawString(item.SubItems[1].Text, bodyFont, brush,
                    leftMargin + columnWidths[0] + columnWidths[1], yPos);

                // Đơn giá (bỏ định dạng tiền tệ)
                string gia = item.SubItems[2].Text.Replace(" VND", "").Replace(",", "");
                e.Graphics.DrawString(gia, bodyFont, brush,
                    leftMargin + columnWidths[0] + columnWidths[1] + columnWidths[2], yPos,
                    new StringFormat { Alignment = StringAlignment.Far });

                // Danh mục
                e.Graphics.DrawString(item.SubItems[3].Text, bodyFont, brush,
                    leftMargin + columnWidths[0] + columnWidths[1] + columnWidths[2] + columnWidths[3], yPos);

                yPos += lineHeight * 1.1f;

                // Kiểm tra nếu hết trang
                if (yPos >= e.MarginBounds.Bottom - lineHeight * 3)
                {
                    e.HasMorePages = true;
                    return;
                }
            }

            // Footer
            string footer = $"Tổng số: {listViewSanPham.Items.Count} sản phẩm";
            e.Graphics.DrawString(footer, footerFont, brush,
                leftMargin, e.MarginBounds.Bottom - lineHeight * 2);

            // Ký tên
            string signature = "Người in: " + Environment.UserName;
            e.Graphics.DrawString(signature, footerFont, brush,
                leftMargin + columnWidths.Sum() - e.Graphics.MeasureString(signature, footerFont).Width,
                e.MarginBounds.Bottom - lineHeight * 2);

            e.HasMorePages = false;
        }

        private void btnPrintPreview_Click(object sender, EventArgs e)
        {
            PrintDocument printDoc = new PrintDocument();
            printDoc.DocumentName = "Danh sách sản phẩm";
            printDoc.PrintPage += new PrintPageEventHandler(PrintPageHandler);

            PrintPreviewDialog previewDialog = new PrintPreviewDialog
            {
                Document = printDoc,
                WindowState = FormWindowState.Maximized
            };

            previewDialog.ShowDialog();
        }

        private void btnPageSetup_Click(object sender, EventArgs e)
        {
            PageSetupDialog pageSetupDialog = new PageSetupDialog
            {
                PageSettings = new PageSettings
                {
                    Margins = new Margins(50, 50, 50, 50), // Lề: trái, phải, trên, dưới
                    Landscape = false // Chế độ ngang/dọc
                },
                PrinterSettings = new PrinterSettings(),
                AllowMargins = true,
                AllowOrientation = true,
                AllowPaper = true,
                AllowPrinter = true
            };

            if (pageSetupDialog.ShowDialog() == DialogResult.OK)
            {
                // Lưu thiết lập trang in
            }
        }
    }
}
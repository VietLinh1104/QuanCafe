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
using ClosedXML.Excel;
using QuanCafe.Services;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace QuanCafe.Forms
{
    public partial class Ban : UserControl
    {
        private readonly BanService banService;
        private bool isAdding = false;
        private bool isEditing = false;
        private int pageSize = 10;  // Số bản ghi mỗi trang
        private int currentPage = 1; // Trang hiện tại
        private int totalRecords = 0;  // Tổng số bản ghi
        private int totalPages = 0;   // Tổng số trang
        private DataTable banDataTable;
        public Ban()
        {
            InitializeComponent(); // Phải gọi đầu tiên

            banService = new BanService();
            InitTrangThaiComboBox(); // Gọi sau khi InitializeComponent

            LoadData();
            SetControlStateDefault();

            listView1.SelectedIndexChanged += listView1_SelectedIndexChanged;
            
        }
        private void InitTrangThaiComboBox()
        {
            // Tạo ComboBox và thêm vào form (nếu cần)
            
            cboTrangThaiLoc.Items.Clear();
            cboTrangThaiLoc.Items.AddRange(new string[] { "Tất cả", "Trống", "Có người", "Đặt trước" });
            cboTrangThaiLoc.SelectedIndex = 0; // Chọn mặc định "Tất cả"

            // Đăng ký sự kiện khi giá trị trong ComboBox thay đổi
            cboTrangThaiLoc.SelectedIndexChanged += cboTrangThaiLoc_SelectedIndexChanged_1;
        }
        
        public void LoadData()
        {
           
            try
            {
                // Lấy dữ liệu từ dịch vụ
                banDataTable = banService.GetAllBan();

                // Kiểm tra nếu không có dữ liệu trả về
                if (banDataTable == null || banDataTable.Rows.Count == 0)
                {
                    MessageBox.Show("Không có bàn nào trong hệ thống.");
                    return;
                }

                // Tính tổng bản ghi và trang
                totalRecords = banDataTable.Rows.Count;
                totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

                // Đảm bảo currentPage hợp lệ
                if (currentPage < 1) currentPage = 1;
                if (currentPage > totalPages) currentPage = totalPages;

                // Phân trang dữ liệu
                var pageData = banDataTable.AsEnumerable()
                    .Skip((currentPage - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                // Hiển thị vào ListView
                listView1.Items.Clear();
                listView1.View = View.Details;
                listView1.Columns.Clear();
                listView1.FullRowSelect = true;

                // Thêm cột vào ListView
                listView1.Columns.Add("ID", 50);
                listView1.Columns.Add("Số bàn", 100);
                listView1.Columns.Add("Trạng thái", 100);

                // Thêm các dòng dữ liệu vào ListView
                foreach (var row in pageData)
                {
                    ListViewItem item = new ListViewItem(row["id_ban"].ToString());
                    item.SubItems.Add(row["ten_ban"].ToString());
                    item.SubItems.Add(row["trang_thai"].ToString());
                    listView1.Items.Add(item);
                }

                // Cập nhật label phân trang
                lblTrang.Text = $"Trang {currentPage}/{totalPages}";
                lblTongsotrang.Text = $"Tổng cộng: {totalRecords} bàn";
               //thống kê số bàn trống
                int soBanTrong = banDataTable.AsEnumerable()
                .Count(row => row["trang_thai"].ToString() == "Trống");

                
                // Hiển thị lên Label
                lblBanTrong.Text = $"Số bàn trống: {soBanTrong}";
            }

            catch (Exception ex)
            {
                // Xử lý lỗi và thông báo cho người dùng
                MessageBox.Show("Lỗi khi tải danh sách bàn: " + ex.Message);
            }
        }
        //đặt lại trạng thái mặc định cho các điều khiển trong giao diện khi không thực hiện thao tác thêm
        //hay sửa (khi bạn quay lại giao diện ban đầu sau khi thêm/xóa/sửa bàn).
        private void SetControlStateDefault()
        {
            btnThemban.Enabled = true;  
            btnSuaban.Enabled = listView1.SelectedItems.Count > 0;//vô hiệu hóa khi không có dòng nào trong listview được chọn
            btnXoaban.Enabled = listView1.SelectedItems.Count > 0;
            btnGhidulieuban.Enabled = false;
            //btnHuy.Enabled = false;

          
            txtTenBan.Enabled = false;
            txtTrangThai.Enabled = false;
        }
        //Phương thức này sẽ điều chỉnh các trạng thái của các điều khiển khi người dùng bắt đầu chỉnh sửa thông tin của bàn, như khi thêm bàn mới hoặc sửa bàn đã tồn tại
        //. Phương thức nhận tham số isAdding, giúp xác định liệu người dùng đang thêm bàn mới hay sửa một bàn đã có.
        private void SetControlStateForEdit(bool isAdding)
        {
            this.isAdding = isAdding;
            btnThemban.Enabled = false; //vô hiệu hóa
            btnSuaban.Enabled = false;
            btnXoaban.Enabled = false;
            btnGhidulieuban.Enabled = true;
            //btnHuy.Enabled = true;

            txtTenBan.Enabled = true;
            txtTrangThai.Enabled = true;
         
            

            txtTenBan.Focus();//đặt con trỏ
        }
        private void ClearForm()
        {
           
            txtTenBan.Clear();
            txtTrangThai.Clear();
        }
        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void btnThemban_Click(object sender, EventArgs e)
        {
            ClearForm();
            SetControlStateForEdit(true);
        }

        private void btnSuaban_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
                SetControlStateForEdit(false);
        }

        private void btnXoaban_Click(object sender, EventArgs e)
        {
            //ktra xem đã chọn bàn nào chưa
            if (listView1.SelectedItems.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn bàn cần xóa.");
                return;
            }   

            int id = int.Parse(listView1.SelectedItems[0].SubItems[0].Text);// Lấy ID của bàn đã chọn từ ListView
            var confirm = MessageBox.Show("Bạn có chắc chắn muốn xóa bàn này?", "Xác nhận", MessageBoxButtons.YesNo);
            if (confirm == DialogResult.Yes)
            {
                bool success = banService.DeleteBan(id); // Thực hiện xóa bàn thông qua service và trả về kết quả
                if (success)
                {
                    MessageBox.Show("Xóa bàn thành công!");
                    LoadData();
                    ClearForm();
                    SetControlStateDefault();
                }
                else
                {
                    MessageBox.Show("Xóa bàn thất bại!");
                }
            }
        }

        private void listView1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                string SoBanSelected = listView1.SelectedItems[0].SubItems[1].Text;
                string TrangThaiSelected = listView1.SelectedItems[0].SubItems[2].Text;
                // // Hiển thị thông tin tên bàn và trạng thái bàn vào các TextBox
                txtTenBan.Text = SoBanSelected;
               txtTrangThai.Text = TrangThaiSelected;
                // Kích hoạt các nút (buttons) thêm, sửa, xóa bàn
                btnThemban.Enabled = true;
                btnXoaban.Enabled =true;
                btnSuaban.Enabled = true;

            }
        }

        private void txtSoBan_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnGhidulieuban_Click(object sender, EventArgs e)
        {
            if (!ValidateInput()) return;//kiểm tra dữ liệu
            //lấy thông tin từ textbox
            string tenBan = txtTenBan.Text;
            string trangThai = txtTrangThai.Text;

            bool success;
            if (isAdding)
            {
                success = banService.InsertBan(tenBan, trangThai);
            }
            else
            {
                //  lấy id từ mục được chọn trong lits view
                int id =int.Parse( listView1.SelectedItems[0].SubItems[0].Text);
                success = banService.UpdateBan(id, tenBan, trangThai);
            }

            if (success)
            {
                MessageBox.Show(isAdding ? "Thêm bàn thành công!" : "Sửa bàn thành công!");
                LoadData();
                SetControlStateDefault();
                ClearForm();
            }
            else
            {
                MessageBox.Show("Cập nhật bàn thất bại!");
            }
        }
        private bool ValidateInput() //kiểm tra người dùng đã nhập đầy đủ thông tin cần thiết trước khi thực hiện các thao tác thêm/sửa.
        {
            if (string.IsNullOrWhiteSpace(txtTenBan.Text))
            {
                MessageBox.Show("Vui lòng nhập tên bàn.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtTrangThai.Text))
            {
                MessageBox.Show("Vui lòng nhập trạng thái bàn.");
                return false;
            }

            return true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string keyword = txtTimkiem.Text.Trim().ToLower();//loại bỏ các khoảng trắng thừa

            if (string.IsNullOrEmpty(keyword))
            {
                LoadData(); // Hiển thị lại toàn bộ nếu ô tìm kiếm trống
                return;
            }

            try
            {
                DataTable dt = banService.GetAllBan(); // Hoặc tạo phương thức tìm kiếm từ DB
                var filtered = dt.AsEnumerable()// Lọc dữ liệu trong DataTable dựa trên từ khóa tìm kiếm
                                 .Where(row => row["ten_ban"].ToString().ToLower().Contains(keyword))
                                 .CopyToDataTable();

                listView1.Items.Clear();

                foreach (DataRow row in filtered.Rows)
                {// Tạo một ListViewItem mới cho mỗi dòng trong DataTable
                    ListViewItem item = new ListViewItem(row[0].ToString());
                    for (int i = 1; i < dt.Columns.Count; i++)// Thêm các giá trị của các cột khác vào subitems của ListViewItem
                    {
                        item.SubItems.Add(row[i].ToString());// Các cột còn lại (tên bàn, trạng thái)
                    }

                    listView1.Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tìm kiếm: " + ex.Message);
            }
        }

        private void button1_Click_1(object sender, EventArgs e)//nút hoàn tác
        {
            isAdding = false;
            isEditing = false;

            // 2. Xóa nội dung các TextBox
            txtTenBan.Text = "";
            txtTrangThai.Text = "";

            // 4. Bỏ chọn trong ListView
            listView1.SelectedItems.Clear();

            // 5. Tải lại danh sách bàn từ DB
            LoadData();

            // 6. Tùy chọn: tắt các nút Lưu, Xóa (nếu bạn quản lý như vậy)
            btnGhidulieuban.Enabled = false;
            btnXoaban.Enabled = false;
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
          
            if (currentPage > 1)
            {
                currentPage--;
                LoadData();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (currentPage < totalPages)
            {
                currentPage++;
                LoadData();
            }
        }

        private void cboTrangThaiLoc_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            // Kiểm tra nếu không có giá trị nào được chọn
            if (cboTrangThaiLoc.SelectedItem == null) return;

            // Lấy giá trị đã chọn trong ComboBox
            string selectedStatus = cboTrangThaiLoc.SelectedItem.ToString();

            // Kiểm tra nếu banDataTable có dữ liệu
            if (banDataTable == null) return;

            // Lọc dữ liệu dựa trên trạng thái đã chọn
            var filteredData = banDataTable.AsEnumerable();

            // Nếu chọn "Tất cả", không lọc, ngược lại chỉ lọc theo trạng thái
            if (selectedStatus != "Tất cả")
            {
                filteredData = filteredData.Where(row => row["trang_thai"].ToString() == selectedStatus);
            }

            // Cập nhật lại ListView
            listView1.Items.Clear();
            foreach (var row in filteredData)
            {
                ListViewItem item = new ListViewItem(row["id_ban"].ToString());
                item.SubItems.Add(row["ten_ban"].ToString());
                item.SubItems.Add(row["trang_thai"].ToString());
                listView1.Items.Add(item);
            }
        }

        private void ExportExcel(DataTable dataTable)
        {
            try
            {
                using (XLWorkbook wb = new XLWorkbook())//tạo một workbook mới sử dụng thư viện ClosedXML
                {
                    var ws = wb.Worksheets.Add(dataTable, "DanhMuc");// Thêm dữ liệu từ DataTable vào worksheet mới, với tên "DanhMuc"

                    // Định dạng
                    ws.Row(1).Style.Font.Bold = true;// Định dạng chữ trong dòng đầu tiên (tiêu đề) thành đậm
                    ws.Columns().AdjustToContents();// Điều chỉnh độ rộng các cột tự động để vừa với nội dung

                    SaveFileDialog saveFileDialog = new SaveFileDialog
                    {
                        Filter = "Excel Workbook|*.xlsx",
                        Title = "Lưu file Excel",
                        FileName = "Ban.xlsx"
                    };

                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        wb.SaveAs(saveFileDialog.FileName);// Lưu workbook vào file người dùng đã chọn
                        MessageBox.Show("Xuất file Excel thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xuất Excel:\n" + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (banDataTable != null && banDataTable.Rows.Count > 0)
            {
                ExportExcel(banDataTable);
            }
            else
            {
                MessageBox.Show("Không có dữ liệu để xuất!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
    
    
    


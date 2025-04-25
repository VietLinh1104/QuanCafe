using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using QuanCafe.Forms;
using QuanCafe.Helpers;
using QuanCafe.Models;
using QuanCafe.Repositories;
using QuanCafe.Services;
//using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace QuanCafe
{
    public partial class QuanLyBan : Form
    {
        string[] table = { "01", "02", "03", "04" };
        List<ChiTietHoaDon> listSanPham;
        DataTable sanPhamList;
        int idSanPhamAdd;
        int idKhachHangComboBoxSelect;
        decimal tongTien = 0;

        SanPhamService sanPhamService;
        DoanhThuService doanhThuService ;
        HoaDonService hoaDonService ;
        ChiTietHoaDonService chiTietHoaDonService ;

        HoaDon HoaDon ;

        int itemSelected;
        int currentChiTietId = 1;

        public QuanLyBan()
        {
            InitializeComponent();
            listSanPham = new List<ChiTietHoaDon>();
            sanPhamList = new DataTable();

            sanPhamService = new SanPhamService();
            hoaDonService = new HoaDonService();
            chiTietHoaDonService = new ChiTietHoaDonService();

            numericUpDown1.Value = 1;
            button4.Enabled = false;

            LoadTableButtons();
            LoadComboBox1();
            LoadComboBox3();
            LoadComboBox4();
            LoadListViewFromList();
        }

        private void LoadTableButtons()
        {
            flowLayoutPanel1.Controls.Clear(); // Xóa các button cũ nếu có

            BanService banService = new BanService();
            DataTable table = banService.GetAllBan();

            if (table != null)
            {
                foreach (DataRow row in table.Rows)
                {
                    string tenBan = row["ten_ban"].ToString();
                    string trangThai = row["trang_thai"].ToString();

                    Button btn = new Button
                    {
                        Text = $"Bàn {tenBan}\n({trangThai})",
                        Width = 100,
                        Height = 100,
                        Tag = row["id_ban"], // lưu ID để xử lý khi click
                        BackColor = GetColorByStatus(trangThai)
                    };

                    btn.Click += TableButton_Click;
                    flowLayoutPanel1.Controls.Add(btn);
                }
            }
        }

        private Color GetColorByStatus(string trangThai)
        {
            switch (trangThai)
            {
                case "Trống":
                    return Color.LightGreen;
                case "Có người":
                    return Color.LightCoral;
                case "Đặt trước":
                    return Color.Khaki;
                default:
                    return Color.Gray;
            }
        }



        private void TableButton_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            int idBan = Convert.ToInt32(btn.Tag);

            // Ví dụ xử lý click: hiển thị thông tin chi tiết bàn
            MessageBox.Show($"Bạn đã chọn {btn.Text} - ID: {idBan}");
        }


        private void homeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var formManager = new Manager();
            formManager.Show();
        }

        //Mở mục quản lý tài khoản
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
                MessageBox.Show("Yêu cầu quyền Quản Lý");
            }
        }

        //khởi tạo comboBox Danh Mục
        private void LoadComboBox1()
        {
            DanhMucService service = new DanhMucService();
            DataTable dt = service.GetAllDanhMuc();

            comboBox1.DataSource = dt;
            comboBox1.DisplayMember = "ten_danh_muc";
            comboBox1.ValueMember = "id_danh_muc";

            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
        }

        //HandleSelect comboBox Danh Mục
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedValue != null && int.TryParse(comboBox1.SelectedValue.ToString(), out int idDanhMuc))
            {
                LoadComboBox2(idDanhMuc);
            }
        }

        //HandleSelect comboBox Sản Phẩm
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox2.SelectedValue != null && int.TryParse(comboBox2.SelectedValue.ToString(), out int idSanPham))
            {
                idSanPhamAdd = idSanPham;
            }
        }

        //khởi tạo comboBox Sản Phẩm
        private void LoadComboBox2(int idDanhMuc)
        {
            SanPhamService service = new SanPhamService();
            DataTable dt = service.GetSanPhamByDanhMuc(idDanhMuc);

            comboBox2.DataSource = dt;
            comboBox2.DisplayMember = "ten_san_pham";
            comboBox2.ValueMember = "id_san_pham";

            comboBox2.SelectedIndexChanged += comboBox2_SelectedIndexChanged;
        }

        //khởi tạo comboBox Bàn
        private void LoadComboBox3()
        {
            BanService service = new BanService();
            DataTable dt = service.GetAllBan();

            comboBox3.DataSource = dt;
            comboBox3.DisplayMember = "ten_ban";
            comboBox3.ValueMember = "id_ban";
        }

        //khởi tạo comboBox Khách Hàng
        private void LoadComboBox4()
        {
            KhachHangService service = new KhachHangService();
            DataTable dt = service.GetAllKhachHang();

            comboBox4.DataSource = dt;
            comboBox4.DisplayMember = "ten_khach_hang";
            comboBox4.ValueMember = "id_khach_hang";
        }

        //HandleSelect comboBox Khách Hàng
        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Kiểm tra giá trị được chọn có tồn tại không
            if (comboBox4.SelectedValue != null && int.TryParse(comboBox4.SelectedValue.ToString(), out int idKhachHang))
            {

                idKhachHangComboBoxSelect = idKhachHang;
                //DataRow khachHangDuocChon = doanhThuService.GetKhachHangThanThietById(1);

                //if (khachHangDuocChon != null)
                //{
                //    Console.WriteLine($"Tên: {khachHangDuocChon["ten_khach_hang"]}, Điểm: {khachHangDuocChon["diem"]}");
                //    int thuHang = Convert.ToInt32(khachHangDuocChon["thu_hang"]);
                //}
                //else
                //{
                //    Console.WriteLine("❌ Không có dữ liệu khách hàng thân thiết.");
                //}

            }
        }


        //Click thêm vào giỏ hàng 
        private void button1_Click(object sender, EventArgs e)
        {
            // Kiểm tra xem đã chọn sản phẩm chưa
            if (idSanPhamAdd == null)
            {
                MessageBox.Show("Vui lòng chọn sản phẩm trước khi thêm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int soLuong = (int)numericUpDown1.Value;
            SanPham addSanPham = sanPhamService.GetSanPhamById(idSanPhamAdd);
            string tenSanPham = addSanPham.TenSanPham;
            decimal gia = soLuong * (decimal)addSanPham.Gia;

            // Tạo ID tự tăng cho chi tiết
            int chiTietId = currentChiTietId++;

            ChiTietHoaDon sanPham = new ChiTietHoaDon(chiTietId, 0, idSanPhamAdd, soLuong, gia);
            listSanPham.Add(sanPham);

            // Cập nhật lại danh sách hiển thị
            sanPhamList = DataConvertHelper.ConvertCTHDListToDataTable(listSanPham);
            LoadListViewFromList();

            // Tính tổng giá
            tongTien = listSanPham.Sum(item => item.DonGia);

            // Hiển thị tổng giá (ví dụ gán vào label)
            textBox1.Text = $"{tongTien:N0} đ"; // hoặc $"{tongGia:C}" nếu muốn định dạng tiền tệ
        }




        // Xóa khỏi sanPhamList
        private void button4_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                string idText = listView1.SelectedItems[0].SubItems[0].Text;

                if (int.TryParse(idText, out int idCanXoa))
                {
                    // Tìm chi tiết cần xóa trong list
                    ChiTietHoaDon chiTietXoa = listSanPham.FirstOrDefault(ct => ct.IdChiTietHoaDon == idCanXoa);

                    if (chiTietXoa != null)
                    {
                        listSanPham.Remove(chiTietXoa);

                        // Cập nhật lại DataTable và giao diện
                        sanPhamList = DataConvertHelper.ConvertCTHDListToDataTable(listSanPham);
                        LoadListViewFromList();
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy sản phẩm để xóa!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn sản phẩm cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        //Khởi Tạo table
        private void LoadListViewFromList()
        {
            listView1.Items.Clear();
            listView1.View = View.Details;
            listView1.Columns.Clear();
            listView1.FullRowSelect = true;
            listView1.Columns.Add("ID", 20);
            listView1.Columns.Add("Sản phẩm", 220);
            listView1.Columns.Add("Số lượng", 70);
            listView1.Columns.Add("Giá", 120);
            

            foreach (DataRow row in sanPhamList.Rows)
            {
                SanPham addSanPham = sanPhamService.GetSanPhamById((int)row[2]);
                string tenSanPham = addSanPham.TenSanPham;

                ListViewItem lvi = new ListViewItem(row[0].ToString());
                //lvi.SubItems.Add(row[1].ToString());
                lvi.SubItems.Add(tenSanPham);
                lvi.SubItems.Add(row[3].ToString());
                lvi.SubItems.Add(row[4].ToString());
                listView1.Items.Add(lvi);
            }
        }

        //Click gio hang
        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                button4.Enabled = true;
            }
            else
            {
                button4.Enabled = false;
            }

        }



        private void listView2_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void QuanLyBan_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

            HoaDon hoadon = new HoaDon(0, 1, 1, idKhachHangComboBoxSelect, 1, tongTien);

            // Insert HoaDon and get the returned idHoaDon
            int idHoaDon = hoaDonService.InsertHoaDon(hoadon);


            foreach (var sp in listSanPham)
            {
                
                sp.IdHoaDon = idHoaDon; 
                chiTietHoaDonService.InsertChiTietHoaDon(sp);
            }

            sanPhamList.Clear();
            LoadListViewFromList();

            MessageBox.Show("Tạo Hóa Đơn Thành Công !!!!");

        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}

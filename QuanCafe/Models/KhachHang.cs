using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanCafe.Models
{
    public class KhachHang
    {
        private int idKhachHang;
        private string tenKhachHang;
        private string soDienThoai;
        private string email;
        private string ghiChu;

        public KhachHang()
        {
        }

        public KhachHang(int idKhachHang, string tenKhachHang, string soDienThoai, string email, string ghiChu)
        {
            this.idKhachHang = idKhachHang;
            this.tenKhachHang = tenKhachHang;
            this.soDienThoai = soDienThoai;
            this.email = email;
            this.ghiChu = ghiChu;
        }

        public int IdKhachHang { get => idKhachHang; set => idKhachHang = value; }
        public string TenKhachHang { get => tenKhachHang; set => tenKhachHang = value; }
        public string SoDienThoai { get => soDienThoai; set => soDienThoai = value; }
        public string Email { get => email; set => email = value; }
        public string GhiChu { get => ghiChu; set => ghiChu = value; }
    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanCafe.Models
{
    public class HoaDon
    {
        private int idHoaDon;
        private int idBan;
        private int idNhanVien;
        private int? idKhachHang;
        private DateTime thoiGian;
        private int trangThai;
        private decimal tongTien;

        public HoaDon()
        {
        }

        public HoaDon(int idHoaDon, int idBan, int idNhanVien, int? idKhachHang, int trangThai, decimal tongTien)
        {
            this.idHoaDon = idHoaDon;
            this.idBan = idBan;
            this.idNhanVien = idNhanVien;
            this.idKhachHang = idKhachHang;
            this.trangThai = trangThai;
            this.tongTien = tongTien;
        }

        public int IdHoaDon { get => idHoaDon; set => idHoaDon = value; }
        public int IdBan { get => idBan; set => idBan = value; }
        public int IdNhanVien { get => idNhanVien; set => idNhanVien = value; }
        public int? IdKhachHang { get => idKhachHang; set => idKhachHang = value; }
        public DateTime ThoiGian { get => thoiGian; set => thoiGian = value; }
        public int TrangThai { get => trangThai; set => trangThai = value; }
        public decimal TongTien { get => tongTien; set => tongTien = value; }
    }

}

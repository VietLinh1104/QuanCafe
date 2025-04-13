using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanCafe.Models
{
    public class ChiTietHoaDon
    {
        private int idChiTietHoaDon;
        private int idHoaDon;
        private int idSanPham;
        private int soLuong;
        private decimal donGia;

        public ChiTietHoaDon()
        {
        }

        public ChiTietHoaDon(int idChiTietHoaDon, int idHoaDon, int idSanPham, int soLuong, decimal donGia)
        {
            this.idChiTietHoaDon = idChiTietHoaDon;
            this.idHoaDon = idHoaDon;
            this.idSanPham = idSanPham;
            this.soLuong = soLuong;
            this.donGia = donGia;
        }

        public int IdChiTietHoaDon { get => idChiTietHoaDon; set => idChiTietHoaDon = value; }
        public int IdHoaDon { get => idHoaDon; set => idHoaDon = value; }
        public int IdSanPham { get => idSanPham; set => idSanPham = value; }
        public int SoLuong { get => soLuong; set => soLuong = value; }
        public decimal DonGia { get => donGia; set => donGia = value; }
    }

}

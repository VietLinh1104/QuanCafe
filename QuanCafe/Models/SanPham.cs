using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanCafe.Models
{
    public class SanPham
    {
        private int idSanPham;
        private string tenSanPham;
        private double gia;
        private int idDanhMuc;
        private string moTa;


        public SanPham()
        {
        }

        public SanPham(int idSanPham, string tenSanPham, double gia, int idDanhMuc, string moTa)
        {
            this.idSanPham = idSanPham;
            this.tenSanPham = tenSanPham;
            this.gia = gia;
            this.idDanhMuc = idDanhMuc;
            this.moTa = moTa;
        }

        public int IdSanPham { get => idSanPham; set => idSanPham = value; }
        public string TenSanPham { get => tenSanPham; set => tenSanPham = value; }
        public double Gia { get => gia; set => gia = value; }
        public int IdDanhMuc { get => idDanhMuc; set => idDanhMuc = value; }
        public string MoTa { get => moTa; set => moTa = value; }

    }

}

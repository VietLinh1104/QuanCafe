using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanCafe.Models
{
    public class DanhMucSanPham
    {
        private int idDanhMuc;
        private string tenDanhMuc;
        private string moTa;

        public DanhMucSanPham()
        {
        }

        public DanhMucSanPham(int idDanhMuc, string tenDanhMuc, string moTa)
        {
            this.idDanhMuc = idDanhMuc;
            this.tenDanhMuc = tenDanhMuc;
            this.moTa = moTa;
        }

        public int IdDanhMuc { get => idDanhMuc; set => idDanhMuc = value; }
        public string TenDanhMuc { get => tenDanhMuc; set => tenDanhMuc = value; }
        public string MoTa { get => moTa; set => moTa = value; }
    }
}

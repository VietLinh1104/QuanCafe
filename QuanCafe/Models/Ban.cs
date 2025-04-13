using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanCafe.Models
{
    public class Ban
    {
        private int idBan;
        private string tenBan;
        private string trangThai;

        public Ban()
        {
        }

        public Ban(int idBan, string tenBan, string trangThai)
        {
            this.idBan = idBan;
            this.tenBan = tenBan;
            this.trangThai = trangThai;
        }

        public int IdBan { get => idBan; set => idBan = value; }
        public string TenBan { get => tenBan; set => tenBan = value; }
        public string TrangThai { get => trangThai; set => trangThai = value; }
    }

}

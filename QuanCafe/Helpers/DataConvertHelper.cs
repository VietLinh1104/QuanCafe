using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuanCafe.Models;

namespace QuanCafe.Helpers
{
    public static class DataConvertHelper
    {
        public static DataTable ConvertCTHDListToDataTable(List<ChiTietHoaDon> list)
        {
            DataTable dt = new DataTable();

            dt.Columns.Add("IdChiTietHoaDon", typeof(int));
            dt.Columns.Add("IdHoaDon", typeof(int));
            dt.Columns.Add("IdSanPham", typeof(int));
            dt.Columns.Add("SoLuong", typeof(int));
            dt.Columns.Add("DonGia", typeof(decimal));

            foreach (var item in list)
            {
                dt.Rows.Add(item.IdChiTietHoaDon, item.IdHoaDon, item.IdSanPham, item.SoLuong, item.DonGia);
            }

            return dt;
        }
    }
}

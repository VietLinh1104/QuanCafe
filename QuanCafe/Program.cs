using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using QuanCafe.Forms;
using QuanCafe.Helpers;
using QuanCafe.Models;
using QuanCafe.Services;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace QuanCafe
{


    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new QuanLyBan());

            //NhanVien nvc = new NhanVien(0, "admin", "Quản lý", "admin", "admin", "admin");
            //NhanVienService nv = new NhanVienService();
            //nv.Register(nvc);
        }
    }
}

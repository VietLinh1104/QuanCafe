﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QuanCafe.Forms.ManagerTab.Tab;

namespace QuanCafe.Forms
{
    public partial class Manager : Form
    {
        public Manager()
        {
            InitializeComponent();
            LoadTabs();
        }

        private void LoadTabs()
        {
            var banTab = new Ban();
            banTab.Dock = DockStyle.Fill;
            tabPage2.Controls.Add(banTab);
            tabPage2.Text = "Bàn";

            var danhMucTab = new DanhMuc();
            banTab.Dock = DockStyle.Fill;
            tabPage3.Controls.Add(danhMucTab);
            tabPage3.Text = "Danh Mục";

            //var doanhThuTab = new DoanhThu();
            //banTab.Dock = DockStyle.Fill;

            var sanPhamTab = new SanPham();
            banTab.Dock = DockStyle.Fill;
            tabPage4.Controls.Add(sanPhamTab);
            tabPage4.Text = "Sản Phẩm";

            var taiKhoanTab = new ManagerTab.Tab.TaiKhoan();
            banTab.Dock = DockStyle.Fill;
            tabPage5.Controls.Add(taiKhoanTab);
            tabPage5.Text = "Tài Khoản";
        }

        private void Manager_Load(object sender, EventArgs e)
        {

        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void Manager_Load_1(object sender, EventArgs e)
        {

        }
    }
}

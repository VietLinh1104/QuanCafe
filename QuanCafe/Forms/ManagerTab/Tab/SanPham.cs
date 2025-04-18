using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using QuanCafe.Repositories;


namespace QuanCafe.Forms.ManagerTab.Tab
{
    public partial class SanPham : UserControl
    {
        private SanPhamRepository _repository;

        public SanPham()
        {
            InitializeComponent();
            _repository = new SanPhamRepository();
            LoadData();
        }

        public void LoadData()
        {
            try
            {
                DataTable dt = _repository.GetAllSanPham();

                listView1.Items.Clear();
                listView1.View = View.Details;
                listView1.Columns.Clear();
                listView1.FullRowSelect = true;

                
                listView1.Columns.Add("id", 50);
                listView1.Columns.Add("Tên sản phẩm", 100);
                listView1.Columns.Add("Đơn giá", 50);
                listView1.Columns.Add("Danh Mục", 50);
                listView1.Columns.Add("Mô tả", 200);

                foreach (DataRow row in dt.Rows)
                {
                    ListViewItem item = new ListViewItem(row[0].ToString());

                    for (int i = 1; i < dt.Columns.Count; i++)
                    {
                        item.SubItems.Add(row[i].ToString());
                    }

                    listView1.Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }



        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}

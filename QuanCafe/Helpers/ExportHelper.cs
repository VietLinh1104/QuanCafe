using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data;
using System.Windows.Forms;
using ClosedXML.Excel;


namespace QuanCafe.Helpers
{
    class ExportHelper
    {
        public static void ExportExcel(DataTable dataTable, string sheetName = "Sheet1", string defaultFileName = "ExportedFile.xlsx")
        {
            try
            {
                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(dataTable, sheetName);

                    SaveFileDialog saveFileDialog = new SaveFileDialog
                    {
                        Filter = "Excel Workbook|*.xlsx",
                        Title = "Lưu file Excel",
                        FileName = defaultFileName
                    };

                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        wb.SaveAs(saveFileDialog.FileName);
                        MessageBox.Show("Xuất file Excel thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xuất Excel:\n" + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}

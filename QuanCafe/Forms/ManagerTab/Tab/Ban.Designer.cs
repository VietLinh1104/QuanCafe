namespace QuanCafe.Forms
{
    partial class Ban
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnXuatexcel = new System.Windows.Forms.Button();
            this.btnHoanTac = new System.Windows.Forms.Button();
            this.btnGhidulieuban = new System.Windows.Forms.Button();
            this.btnXoaban = new System.Windows.Forms.Button();
            this.btnSuaban = new System.Windows.Forms.Button();
            this.btnThemban = new System.Windows.Forms.Button();
            this.txtTrangThai = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtTenBan = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.listView1 = new System.Windows.Forms.ListView();
            this.btnTimKiem = new System.Windows.Forms.Button();
            this.txtTimkiem = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.lblTrang = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.lblTongsotrang = new System.Windows.Forms.Label();
            this.cboTrangThaiLoc = new System.Windows.Forms.ComboBox();
            this.lblBanTrong = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.lblBanTrong);
            this.groupBox1.Controls.Add(this.btnXuatexcel);
            this.groupBox1.Controls.Add(this.btnHoanTac);
            this.groupBox1.Controls.Add(this.btnGhidulieuban);
            this.groupBox1.Controls.Add(this.btnXoaban);
            this.groupBox1.Controls.Add(this.btnSuaban);
            this.groupBox1.Controls.Add(this.btnThemban);
            this.groupBox1.Controls.Add(this.txtTrangThai);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtTenBan);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(928, 39);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox1.Size = new System.Drawing.Size(465, 689);
            this.groupBox1.TabIndex = 15;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "groupBox1";
            this.groupBox1.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // btnXuatexcel
            // 
            this.btnXuatexcel.Location = new System.Drawing.Point(224, 283);
            this.btnXuatexcel.Name = "btnXuatexcel";
            this.btnXuatexcel.Size = new System.Drawing.Size(209, 23);
            this.btnXuatexcel.TabIndex = 14;
            this.btnXuatexcel.Text = "Xuất excel";
            this.btnXuatexcel.UseVisualStyleBackColor = true;
            this.btnXuatexcel.Click += new System.EventHandler(this.button4_Click);
            // 
            // btnHoanTac
            // 
            this.btnHoanTac.Location = new System.Drawing.Point(7, 283);
            this.btnHoanTac.Name = "btnHoanTac";
            this.btnHoanTac.Size = new System.Drawing.Size(209, 23);
            this.btnHoanTac.TabIndex = 13;
            this.btnHoanTac.Text = "Hoàn tác";
            this.btnHoanTac.UseVisualStyleBackColor = true;
            this.btnHoanTac.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // btnGhidulieuban
            // 
            this.btnGhidulieuban.Location = new System.Drawing.Point(224, 228);
            this.btnGhidulieuban.Margin = new System.Windows.Forms.Padding(4);
            this.btnGhidulieuban.Name = "btnGhidulieuban";
            this.btnGhidulieuban.Size = new System.Drawing.Size(209, 28);
            this.btnGhidulieuban.TabIndex = 12;
            this.btnGhidulieuban.Text = "Ghi dữ liệu";
            this.btnGhidulieuban.UseVisualStyleBackColor = true;
            this.btnGhidulieuban.Click += new System.EventHandler(this.btnGhidulieuban_Click);
            // 
            // btnXoaban
            // 
            this.btnXoaban.Location = new System.Drawing.Point(7, 228);
            this.btnXoaban.Margin = new System.Windows.Forms.Padding(4);
            this.btnXoaban.Name = "btnXoaban";
            this.btnXoaban.Size = new System.Drawing.Size(209, 28);
            this.btnXoaban.TabIndex = 11;
            this.btnXoaban.Text = "Xóa";
            this.btnXoaban.UseVisualStyleBackColor = true;
            this.btnXoaban.Click += new System.EventHandler(this.btnXoaban_Click);
            // 
            // btnSuaban
            // 
            this.btnSuaban.Location = new System.Drawing.Point(224, 176);
            this.btnSuaban.Margin = new System.Windows.Forms.Padding(4);
            this.btnSuaban.Name = "btnSuaban";
            this.btnSuaban.Size = new System.Drawing.Size(209, 28);
            this.btnSuaban.TabIndex = 10;
            this.btnSuaban.Text = "Sửa";
            this.btnSuaban.UseVisualStyleBackColor = true;
            this.btnSuaban.Click += new System.EventHandler(this.btnSuaban_Click);
            // 
            // btnThemban
            // 
            this.btnThemban.Location = new System.Drawing.Point(7, 176);
            this.btnThemban.Margin = new System.Windows.Forms.Padding(4);
            this.btnThemban.Name = "btnThemban";
            this.btnThemban.Size = new System.Drawing.Size(209, 28);
            this.btnThemban.TabIndex = 9;
            this.btnThemban.Text = "Thêm";
            this.btnThemban.UseVisualStyleBackColor = true;
            this.btnThemban.Click += new System.EventHandler(this.btnThemban_Click);
            // 
            // txtTrangThai
            // 
            this.txtTrangThai.Location = new System.Drawing.Point(127, 97);
            this.txtTrangThai.Margin = new System.Windows.Forms.Padding(4);
            this.txtTrangThai.Name = "txtTrangThai";
            this.txtTrangThai.Size = new System.Drawing.Size(329, 22);
            this.txtTrangThai.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 101);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 16);
            this.label2.TabIndex = 5;
            this.label2.Text = "Trạng Thái";
            // 
            // txtTenBan
            // 
            this.txtTenBan.Location = new System.Drawing.Point(127, 53);
            this.txtTenBan.Margin = new System.Windows.Forms.Padding(4);
            this.txtTenBan.Name = "txtTenBan";
            this.txtTenBan.Size = new System.Drawing.Size(329, 22);
            this.txtTenBan.TabIndex = 4;
            this.txtTenBan.TextChanged += new System.EventHandler(this.txtSoBan_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 57);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Tên Bàn";
            // 
            // listView1
            // 
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(8, 39);
            this.listView1.Margin = new System.Windows.Forms.Padding(4);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(911, 599);
            this.listView1.TabIndex = 14;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged_1);
            // 
            // btnTimKiem
            // 
            this.btnTimKiem.AutoSize = true;
            this.btnTimKiem.Location = new System.Drawing.Point(820, 7);
            this.btnTimKiem.Margin = new System.Windows.Forms.Padding(4);
            this.btnTimKiem.Name = "btnTimKiem";
            this.btnTimKiem.Size = new System.Drawing.Size(100, 28);
            this.btnTimKiem.TabIndex = 13;
            this.btnTimKiem.Text = "Tìm Kiếm";
            this.btnTimKiem.UseVisualStyleBackColor = true;
            this.btnTimKiem.Click += new System.EventHandler(this.button1_Click);
            // 
            // txtTimkiem
            // 
            this.txtTimkiem.Location = new System.Drawing.Point(8, 7);
            this.txtTimkiem.Margin = new System.Windows.Forms.Padding(4);
            this.txtTimkiem.Name = "txtTimkiem";
            this.txtTimkiem.Size = new System.Drawing.Size(803, 22);
            this.txtTimkiem.TabIndex = 12;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.lblTrang);
            this.panel1.Controls.Add(this.button3);
            this.panel1.Controls.Add(this.button2);
            this.panel1.Location = new System.Drawing.Point(8, 636);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(911, 37);
            this.panel1.TabIndex = 16;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(187, 10);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(43, 16);
            this.label4.TabIndex = 20;
            this.label4.Text = "Trang";
            // 
            // lblTrang
            // 
            this.lblTrang.AutoSize = true;
            this.lblTrang.Location = new System.Drawing.Point(389, 14);
            this.lblTrang.Name = "lblTrang";
            this.lblTrang.Size = new System.Drawing.Size(44, 16);
            this.lblTrang.TabIndex = 19;
            this.lblTrang.Text = "label3";
            this.lblTrang.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.lblTrang.Click += new System.EventHandler(this.label3_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(276, 9);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 18;
            this.button3.Text = "<";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(497, 10);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 17;
            this.button2.Text = ">";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // lblTongsotrang
            // 
            this.lblTongsotrang.AutoSize = true;
            this.lblTongsotrang.Location = new System.Drawing.Point(45, 688);
            this.lblTongsotrang.Name = "lblTongsotrang";
            this.lblTongsotrang.Size = new System.Drawing.Size(90, 16);
            this.lblTongsotrang.TabIndex = 21;
            this.lblTongsotrang.Text = "Tổng số trang";
            this.lblTongsotrang.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.lblTongsotrang.UseWaitCursor = true;
            // 
            // cboTrangThaiLoc
            // 
            this.cboTrangThaiLoc.FormattingEnabled = true;
            this.cboTrangThaiLoc.Location = new System.Drawing.Point(928, 8);
            this.cboTrangThaiLoc.Name = "cboTrangThaiLoc";
            this.cboTrangThaiLoc.Size = new System.Drawing.Size(121, 24);
            this.cboTrangThaiLoc.TabIndex = 21;
            this.cboTrangThaiLoc.SelectedIndexChanged += new System.EventHandler(this.cboTrangThaiLoc_SelectedIndexChanged_1);
            // 
            // lblBanTrong
            // 
            this.lblBanTrong.AutoSize = true;
            this.lblBanTrong.Location = new System.Drawing.Point(12, 144);
            this.lblBanTrong.Name = "lblBanTrong";
            this.lblBanTrong.Size = new System.Drawing.Size(90, 16);
            this.lblBanTrong.TabIndex = 22;
            this.lblBanTrong.Text = "Số Bàn Trống";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 176);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(0, 16);
            this.label3.TabIndex = 23;
            // 
            // Ban
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.cboTrangThaiLoc);
            this.Controls.Add(this.lblTongsotrang);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.btnTimKiem);
            this.Controls.Add(this.txtTimkiem);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Ban";
            this.Size = new System.Drawing.Size(1401, 736);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnGhidulieuban;
        private System.Windows.Forms.Button btnXoaban;
        private System.Windows.Forms.Button btnSuaban;
        private System.Windows.Forms.Button btnThemban;
        private System.Windows.Forms.TextBox txtTrangThai;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtTenBan;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.Button btnTimKiem;
        private System.Windows.Forms.TextBox txtTimkiem;
        private System.Windows.Forms.Button btnHoanTac;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblTrang;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label lblTongsotrang;
        private System.Windows.Forms.ComboBox cboTrangThaiLoc;
        private System.Windows.Forms.Button btnXuatexcel;
        private System.Windows.Forms.Label lblBanTrong;
        private System.Windows.Forms.Label label3;
    }
}

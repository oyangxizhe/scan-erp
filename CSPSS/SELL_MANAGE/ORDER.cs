using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using XizheC;
using System.IO;
namespace CSPSS.SELL_MANAGE
{
    public partial class ORDER : Form
    {
        DataTable dt = new DataTable();
        basec bc = new basec();
        CORDER corder = new CORDER();
        private bool _IFExecutionSUCCESS;
        public bool IFExecution_SUCCESS
        {
            set { _IFExecutionSUCCESS = value; }
            get { return _IFExecutionSUCCESS; }
        }
        private  bool _IF_DOUBLE_CLICK;
        public  bool IF_DOUBLE_CLICK
        {
            set { _IF_DOUBLE_CLICK = value; }
            get { return _IF_DOUBLE_CLICK; }
        }
        private string _ORID;
        public string ORID
        {
            set { _ORID = value; }
            get { return _ORID; }
        }
        private string _CNAME;
        public string CNAME
        {
            set { _CNAME = value; }
            get { return _CNAME; }
        }
        private string _ADDRESS;
        public string ADDRESS
        {
            set { _ADDRESS = value; }
            get { return _ADDRESS; }
        }
        private string _CONTACT;
        public string CONTACT
        {
            set { _CONTACT = value; }
            get { return _CONTACT; }
        }
        private string _PHONE;
        public string PHONE
        {
            set { _PHONE = value; }
            get { return _PHONE; }
        }
        int select=0;
        private string _BARCODE;
        public string BARCODE
        {
            set { _BARCODE = value; }
            get { return _BARCODE; }
        }
        private string _ORKEY;
        public string ORKEY
        {
            set { _ORKEY = value; }
            get { return _ORKEY; }
        }
        private int _SELECT;
        public int SELECT
        {
            set { _SELECT = value; }
            get { return _SELECT; }
        }
        public List<string> selectOrid { set; get; }
        public string getSelectOrid { set; get; }
        public string ErrowInfo { set; get; }
        public ORDER()
        {
            InitializeComponent();
        }
        #region init
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ORDER));
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label13 = new System.Windows.Forms.Label();
            this.textBox6 = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.dateTimePicker2 = new System.Windows.Forms.DateTimePicker();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.btnToExcel = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.pictureBox5 = new System.Windows.Forms.PictureBox();
            this.label17 = new System.Windows.Forms.Label();
            this.btnAdd = new System.Windows.Forms.PictureBox();
            this.btnExit = new System.Windows.Forms.PictureBox();
            this.btnSearch = new System.Windows.Forms.PictureBox();
            this.hint = new System.Windows.Forms.Label();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label14 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnAdd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnExit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnSearch)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(245)))), ((int)(((byte)(255)))));
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dataGridView1.Location = new System.Drawing.Point(0, 315);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(943, 301);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.DataSourceChanged += new System.EventHandler(this.dataGridView1_DataSourceChanged);
            this.dataGridView1.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellClick);
            this.dataGridView1.DoubleClick += new System.EventHandler(this.dataGridView1_DoubleClick);
            this.dataGridView1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.dataGridView1_MouseUp);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.label13);
            this.groupBox1.Controls.Add(this.textBox6);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.textBox5);
            this.groupBox1.Controls.Add(this.comboBox2);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.comboBox1);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.textBox4);
            this.groupBox1.Controls.Add(this.textBox3);
            this.groupBox1.Controls.Add(this.textBox2);
            this.groupBox1.Controls.Add(this.checkBox1);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.textBox1);
            this.groupBox1.Controls.Add(this.dateTimePicker2);
            this.groupBox1.Controls.Add(this.dateTimePicker1);
            this.groupBox1.Controls.Add(this.btnToExcel);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(3, 151);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(936, 130);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "查询条件";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(563, 106);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(29, 12);
            this.label13.TabIndex = 138;
            this.label13.Text = "材料";
            // 
            // textBox6
            // 
            this.textBox6.Location = new System.Drawing.Point(598, 103);
            this.textBox6.Name = "textBox6";
            this.textBox6.Size = new System.Drawing.Size(141, 21);
            this.textBox6.TabIndex = 137;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(563, 78);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(29, 12);
            this.label10.TabIndex = 136;
            this.label10.Text = "品名";
            // 
            // textBox5
            // 
            this.textBox5.Location = new System.Drawing.Point(598, 75);
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new System.Drawing.Size(141, 21);
            this.textBox5.TabIndex = 135;
            // 
            // comboBox2
            // 
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Items.AddRange(new object[] {
            "",
            "已打印",
            "未打印"});
            this.comboBox2.Location = new System.Drawing.Point(355, 80);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(141, 20);
            this.comboBox2.TabIndex = 133;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(296, 83);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(53, 12);
            this.label9.TabIndex = 134;
            this.label9.Text = "打印状态";
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "",
            "已发货",
            "部分发货",
            "未发货"});
            this.comboBox1.Location = new System.Drawing.Point(115, 80);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(141, 20);
            this.comboBox1.TabIndex = 131;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(56, 83);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(53, 12);
            this.label8.TabIndex = 132;
            this.label8.Text = "订单状态";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(527, 54);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(65, 12);
            this.label7.TabIndex = 130;
            this.label7.Text = "客户订单号";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(563, 22);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(29, 12);
            this.label6.TabIndex = 129;
            this.label6.Text = "型号";
            // 
            // textBox4
            // 
            this.textBox4.Location = new System.Drawing.Point(598, 47);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(141, 21);
            this.textBox4.TabIndex = 125;
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(598, 19);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(141, 21);
            this.textBox3.TabIndex = 128;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(115, 20);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(141, 21);
            this.textBox2.TabIndex = 127;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(35, 54);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(15, 14);
            this.checkBox1.TabIndex = 126;
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(296, 23);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 125;
            this.label4.Text = "客户名称";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(355, 20);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(141, 21);
            this.textBox1.TabIndex = 124;
            // 
            // dateTimePicker2
            // 
            this.dateTimePicker2.Cursor = System.Windows.Forms.Cursors.Default;
            this.dateTimePicker2.Location = new System.Drawing.Point(355, 50);
            this.dateTimePicker2.Name = "dateTimePicker2";
            this.dateTimePicker2.Size = new System.Drawing.Size(141, 21);
            this.dateTimePicker2.TabIndex = 4;
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Cursor = System.Windows.Forms.Cursors.Default;
            this.dateTimePicker1.Location = new System.Drawing.Point(114, 50);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(141, 21);
            this.dateTimePicker1.TabIndex = 3;
            // 
            // btnToExcel
            // 
            this.btnToExcel.FlatAppearance.BorderSize = 0;
            this.btnToExcel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnToExcel.Font = new System.Drawing.Font("宋体", 9F);
            this.btnToExcel.Image = ((System.Drawing.Image)(resources.GetObject("btnToExcel.Image")));
            this.btnToExcel.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnToExcel.Location = new System.Drawing.Point(847, 14);
            this.btnToExcel.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnToExcel.Name = "btnToExcel";
            this.btnToExcel.Size = new System.Drawing.Size(50, 64);
            this.btnToExcel.TabIndex = 5;
            this.btnToExcel.Text = "导出";
            this.btnToExcel.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnToExcel.UseVisualStyleBackColor = false;
            this.btnToExcel.Click += new System.EventHandler(this.btnToExcel_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(68, 28);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 12);
            this.label3.TabIndex = 8;
            this.label3.Text = "订单号";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(299, 56);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(11, 12);
            this.label2.TabIndex = 6;
            this.label2.Text = "~";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(56, 54);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "日期期间";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(862, 95);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(29, 12);
            this.label11.TabIndex = 29;
            this.label11.Text = "退出";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(771, 95);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(29, 12);
            this.label12.TabIndex = 28;
            this.label12.Text = "搜索";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.label14);
            this.groupBox2.Controls.Add(this.pictureBox1);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.pictureBox5);
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.Controls.Add(this.label12);
            this.groupBox2.Controls.Add(this.label17);
            this.groupBox2.Controls.Add(this.btnAdd);
            this.groupBox2.Controls.Add(this.btnExit);
            this.groupBox2.Controls.Add(this.btnSearch);
            this.groupBox2.Location = new System.Drawing.Point(3, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(936, 121);
            this.groupBox2.TabIndex = 34;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "菜单栏";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(684, 95);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(29, 12);
            this.label5.TabIndex = 114;
            this.label5.Text = "打印";
            // 
            // pictureBox5
            // 
            this.pictureBox5.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox5.Image")));
            this.pictureBox5.InitialImage = null;
            this.pictureBox5.Location = new System.Drawing.Point(667, 20);
            this.pictureBox5.Name = "pictureBox5";
            this.pictureBox5.Size = new System.Drawing.Size(60, 60);
            this.pictureBox5.TabIndex = 113;
            this.pictureBox5.TabStop = false;
            this.pictureBox5.Click += new System.EventHandler(this.pictureBox5_Click);
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(28, 95);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(29, 12);
            this.label17.TabIndex = 24;
            this.label17.Text = "新增";
            // 
            // btnAdd
            // 
            this.btnAdd.Image = ((System.Drawing.Image)(resources.GetObject("btnAdd.Image")));
            this.btnAdd.InitialImage = null;
            this.btnAdd.Location = new System.Drawing.Point(12, 20);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(60, 60);
            this.btnAdd.TabIndex = 16;
            this.btnAdd.TabStop = false;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnExit
            // 
            this.btnExit.Image = ((System.Drawing.Image)(resources.GetObject("btnExit.Image")));
            this.btnExit.InitialImage = null;
            this.btnExit.Location = new System.Drawing.Point(847, 20);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(60, 60);
            this.btnExit.TabIndex = 19;
            this.btnExit.TabStop = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.Image = ((System.Drawing.Image)(resources.GetObject("btnSearch.Image")));
            this.btnSearch.InitialImage = null;
            this.btnSearch.Location = new System.Drawing.Point(757, 20);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(60, 60);
            this.btnSearch.TabIndex = 18;
            this.btnSearch.TabStop = false;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // hint
            // 
            this.hint.AutoSize = true;
            this.hint.Location = new System.Drawing.Point(204, 136);
            this.hint.Name = "hint";
            this.hint.Size = new System.Drawing.Size(29, 12);
            this.hint.TabIndex = 105;
            this.hint.Text = "hint";
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Location = new System.Drawing.Point(52, 291);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(48, 16);
            this.checkBox2.TabIndex = 106;
            this.checkBox2.Text = "全选";
            this.checkBox2.UseVisualStyleBackColor = true;
            this.checkBox2.CheckedChanged += new System.EventHandler(this.CheckBox2_CheckedChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(117, 287);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 107;
            this.button1.Text = "确认";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Button1_Click);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(596, 95);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(29, 12);
            this.label14.TabIndex = 116;
            this.label14.Text = "补印";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.InitialImage = null;
            this.pictureBox1.Location = new System.Drawing.Point(579, 20);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(60, 60);
            this.pictureBox1.TabIndex = 115;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // ORDER
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(245)))), ((int)(((byte)(255)))));
            this.ClientSize = new System.Drawing.Size(942, 616);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.checkBox2);
            this.Controls.Add(this.hint);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.dataGridView1);
            this.Name = "ORDER";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "订单查询";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.FrmWorkGroup_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnAdd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnExit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnSearch)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion
  
        private void FrmWorkGroup_Load(object sender, EventArgs e)
        {
      
            try
            {
                ORID = "";
                CNAME = "";
                 this.Icon = Resource1.xz_200X200;
                hint.Location = new Point(400, 100);
                hint.ForeColor = Color.Red;
                dateTimePicker1.CustomFormat = "yyyy/MM/dd";
                dateTimePicker2.CustomFormat = "yyyy/MM/dd";
                dateTimePicker1.Format = DateTimePickerFormat.Custom;
                dateTimePicker2.Format = DateTimePickerFormat.Custom;
                hint.Text = "";
                textBox2.Focus();
                hint.Text = "";
                checkBox1.Checked = true;
                //bind();
            }
            catch (Exception)
            {
                MessageBox.Show("网络连接中断");
            }
        }
        #region bind
        public void bind()
        {
            hint.Text = "";
            StringBuilder stb = new StringBuilder();
            stb.Append(corder.sql);
            stb.Append("  WHERE  客户名称 LIKE '%" + textBox1.Text + "%'");
           
            if (textBox2.Text != "")
            {
                stb.Append(" AND 订单号 LIKE '%" + textBox2.Text + "%'");
            }
         
            if (textBox4.Text != "")
            { 
                stb.Append(" AND 客户订单号 LIKE '%" + textBox4.Text + "%'");
            }
            stb.Append(" AND 订单状态 LIKE '%" + comboBox1.Text + "%'");
            stb.Append(" AND 打印状态 LIKE '%" + comboBox2.Text + "%'");
            if (textBox3.Text != "")
            {
                stb.Append(" AND 型号 LIKE '%" + textBox3.Text + "%'");
            }
            if (textBox5.Text != "")
            {
                stb.Append(" AND 品名 LIKE '%" + textBox5.Text + "%'");
            }
            if (textBox6.Text != "")
            {
                stb.Append(" AND 材料 LIKE '%" + textBox6.Text + "%'");
            }
            string v1 = dateTimePicker1.Text + " 0:00:00";
            string v2 = dateTimePicker2.Text + " 23:59:59";
            if (checkBox1.Checked)
            {
                stb.Append(" AND DATE  BETWEEN  '" + v1 + "' AND '" + v2 + "'");
                //MessageBox.Show(" AND B.DATE  '" + v1 + "' AND '" + v2 + "'");
            }
       
       
            dataGridView1.AllowUserToAddRows = false;
            //dataGridView1.ContextMenuStrip = contextMenuStrip1;

            hint.Location = new Point(400, 100);
            hint.ForeColor = Color.Red;

            if (bc.GET_IFExecutionSUCCESS_HINT_INFO(IFExecution_SUCCESS) != "")
            {

                hint.Text = bc.GET_IFExecutionSUCCESS_HINT_INFO(IFExecution_SUCCESS);
            }
            else
            {
                hint.Text = "";
            }
        
            search_o(stb.ToString());
            try
            {
        
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }
        #endregion
        #region search_o()
        public void search_o(string sql)
        {
       
            //string v7 = bc.getOnlyString("SELECT SCOPE FROM SCOPE_OF_AUTHORIZATION WHERE USID='" + LOGIN.USID + "'");
            string v7 = "Y";
             if (v7 == "Y")
            {

                dt = bc.getdt(sql+" ORDER BY ORID ASC");

            }
            else if (v7 == "GROUP")
            {

                dt = bc.getdt(sql + @" AND MAKERID IN (SELECT EMID FROM USERINFO A WHERE UGID IN 
 (SELECT UGID FROM USERINFO WHERE USID='" + LOGIN.USID + "'))" );
            }
            else
            {
                dt = bc.getdt(sql + " AND MAKERID='" + LOGIN.EMID + "'" );

            }
            dt = corder.RETURN_DT(dt);
            if (dt.Rows.Count > 0)
            {
            
                dataGridView1.DataSource = dt;

                dgvStateControl();
            }
            else
            {
                hint.Text = "找不到所要搜索项！";
                dataGridView1.DataSource = null;

            }
        }
        #endregion
        #region dgvStateControl
        private void dgvStateControl()
        {
            int i;
            dataGridView1.RowHeadersDefaultCellStyle.BackColor = Color.Lavender;
            int numCols1 = dataGridView1.Columns.Count;
            //自动调整列宽将严重影响查询显示速度
            //dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;/*自动调整DATAGRIDVIEW的列宽*/
            for (i = 0; i < numCols1; i++)
            {
                dataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                //this.dataGridView1.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                //this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
                dataGridView1.EnableHeadersVisualStyles = false;
                dataGridView1.Columns[i].HeaderCell.Style.BackColor = Color.Lavender;
                dataGridView1.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                if (i == 0)
                    dataGridView1.Columns[i].ReadOnly = false;
                else
                dataGridView1.Columns[i].ReadOnly = true;
            }
            for (i = 0; i < dataGridView1.Rows.Count; i++)
            {
                dataGridView1.Rows[i].Height = 18;
            }
            for (i = 0; i < dataGridView1.Rows.Count - 1; i++)
            {
                dataGridView1.Rows[i].DefaultCellStyle.BackColor = CCOLOR.GLS;
                dataGridView1.Rows[i + 1].DefaultCellStyle.BackColor = CCOLOR.YG;
                i = i + 1;
            }
        }
        #endregion
        #region add

        #endregion
        #region override enter
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Enter &&
             (
             (
              !(ActiveControl is System.Windows.Forms.TextBox) ||
              !((System.Windows.Forms.TextBox)ActiveControl).AcceptsReturn)
             )
             )
            {
                SendKeys.SendWait("{Tab}");
                return true;
            }
            if (keyData == (Keys.Enter | Keys.Shift))
            {
                SendKeys.SendWait("+{Tab}");
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
        #endregion
        #region doubleclick
        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            if (select != 0)
            {
           
            }
            else
            {
                ORDERT FRM = new ORDERT(this);
                string v1 = dt.Rows[dataGridView1.CurrentCell.RowIndex]["订单号"].ToString();
                FRM.IDO = v1;
                FRM.ADD_OR_UPDATE = "UPDATE";
                FRM.Show();
            }
        }
        #endregion
        private void btnToExcel_Click(object sender, EventArgs e)
        {
            if (dt.Rows.Count > 0)
            {
                bc.dgvtoExcel(dataGridView1, "订单信息");
            }
            else
            {
                MessageBox.Show("没有数据可导出！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void dataGridView1_DataSourceChanged(object sender, EventArgs e)
        {
            int i;
            for (i = 0; i < dataGridView1.Columns.Count; i++)
            {
                if (dataGridView1.Columns[i].ValueType.ToString() == "System.Decimal")
                {
                    dataGridView1.Columns[i].DefaultCellStyle.Format = "N";
                    dataGridView1.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomRight;
                }

            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            CSPSS.SELL_MANAGE.ORDERT FRM = new SELL_MANAGE.ORDERT(this);
            FRM.IDO = corder.GETID();
            FRM.Show();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            bind();
            try
            {
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);

            }

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dataGridView1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right) //判断是不是右键
            {
                Control control = new Control();
                Point ClickPoint = new Point(e.X, e.Y);
                control.GetChildAtPoint(ClickPoint);
                if (dataGridView1.HitTest(e.X, e.Y).RowIndex >= 0 && dataGridView1.HitTest(e.X, e.Y).ColumnIndex >= 0)//判断你点的是不是一个信息行里
                {
                    dataGridView1.CurrentCell = dataGridView1.Rows[dataGridView1.HitTest(e.X, e.Y).RowIndex].Cells[dataGridView1.HitTest(e.X, e.Y).ColumnIndex];
                    ContextMenu con = new ContextMenu();
                    MenuItem menuDeleteknowledge = new MenuItem("复制");
                    menuDeleteknowledge.Click += new EventHandler(btndgvInfoCopy_Click);
                    con.MenuItems.Add(menuDeleteknowledge);
                    this.dataGridView1.ContextMenu = con;
                    con.Show(dataGridView1, new Point(e.X + 10, e.Y));
                }
            }
        }
        private void btndgvInfoCopy_Click(object sender, EventArgs e)
        {

            dgvCopy(ref dataGridView1);
        }
        private void dgvCopy(ref DataGridView  dgv)
        {
            if (dgv.GetCellCount(DataGridViewElementStates.Selected) > 0)
            {
                try
                {
                    Clipboard.SetDataObject(dgv.GetClipboardContent());
                }
                catch (Exception MyEx)
                {
                    MessageBox.Show(MyEx.Message, "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            corder.MAKERID = "";
            if (!File.Exists("D:\\barcode\\barcode.btw"))
            {
                MessageBox.Show("D:\\barcode\\barcode.btw下不存在打印模版文件");
                return;
            }
            if (File.Exists("D:\\barcode\\barcode_data.txt"))
                File.Delete("D:\\barcode\\barcode_data.txt");
            if (File.Exists("D:\\barcode\\barcode_data1.txt"))
                File.Delete("D:\\barcode\\barcode_data1.txt");
            Print(dataGridView1, "D:\\barcode\\barcode_data1.txt");
            //corder.ExcelPrint(dataGridView1, "订单", System.IO.Path.GetFullPath("订单.xlsx"));
            //corder.ExcelPrint_40X30(dataGridView1, "订单", System.IO.Path.GetFullPath("订单40X30.xlsx"));
            //hint.Text = corder.ErrowInfo;
            try
            {

            }
            catch (Exception MyEx)
            {
                MessageBox.Show(MyEx.Message, "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        #region Print
        public void Print(DataGridView dv,string path)
        {
            if (dv.Rows.Count > 0)
            {
                if (bc.JUAGE_IF_EXISTS_SELECT_DV(dv))
                {
               
                    int j = 0;
                    StringBuilder sqb = new StringBuilder();
                    for (int i = 0; i < dv.Rows.Count; i++)
                    {
                        if (dv["序号", i].Selected == true)
                        {
                            BARCODE = bc.numYMD(20, 12, "000000000001", "select * from ORDER_BARCODE", "BARCODE", "BA");
                            ORKEY = bc.getOnlyString(string.Format("SELECT ORKEY FROM ORDER_DET WHERE ORID='{0}' AND SN='{1}'",
                                dt.Rows[i]["订单号"].ToString(), dt.Rows[i]["项次"].ToString()));
                            corder.BARCODE = BARCODE;
                            corder.ORKEY = ORKEY;
                            corder.SQlcommandE();
                            j = j + 1;
                            sqb.AppendFormat(BARCODE+",",BARCODE);
                            sqb.AppendFormat("{0}"+",", dt.Rows[i]["客户名称"].ToString());
                            sqb.AppendFormat("{0}" + ",", dt.Rows[i]["型号"].ToString());
                            sqb.AppendFormat("{0}" + ",", dt.Rows[i]["品名"].ToString());
                            sqb.AppendFormat("{0}" + ",", dt.Rows[i]["材料"].ToString());
                            sqb.AppendFormat("{0}" + ",", dt.Rows[i]["数量"].ToString());
                            sqb.AppendFormat("{0}" + ",", dt.Rows[i]["下单日期"].ToString());

                            sqb.AppendFormat("{0}" + ",", dt.Rows[i]["重量"].ToString());
                            sqb.AppendFormat("{0}" + ",", dt.Rows[i]["订单交期"].ToString());
                            sqb.AppendFormat("{0}" + ",", dt.Rows[i]["模具编号"].ToString());
                            sqb.AppendFormat("\r\n");
                        }
                    }
                    ErrowInfo = "打印数据已发送至打印机";
                    hint.Text = ErrowInfo;

                    string v1 = "";
                    v1 = sqb.ToString().Replace(",", ",");
                    new CFileInfo().data_to_txt(path, v1);
                    hint.Text = "打印数据已写入txt文件";
                    System.Diagnostics.Process.Start("D:\\barcode\\barcode_bat.bat");
                    hint.Text = "已打印";
                }
                else
                {
                    ErrowInfo = "没有选中要打印的项";
                    return;
                }
            }
            else
            {
                ErrowInfo = "没有数据可打印";
                return;
            }



        }
        public void rePrint(DataGridView dv, string path)
        {
            if (dv.Rows.Count > 0)
            {
                if (bc.JUAGE_IF_EXISTS_SELECT_DV(dv))
                {

                    int j = 0;
                    StringBuilder sqb = new StringBuilder();
                    for (int i = 0; i < dv.Rows.Count; i++)
                    {
                        if (dv["序号", i].Selected == true)
                        {
                            DataTable dtTemp = bc.getdt(string.Format("select * from ORDER_BARCODE where" +
                                " ORKEY in (SELECT ORKEY FROM Order_DET WHERE ORID='{0}' AND SN='{1}') order by DATE asc ",
                                dt.Rows[i]["订单号"].ToString(), dt.Rows[i]["项次"].ToString()));
                            if (dtTemp.Rows.Count > 0)
                            {
                                BARCODE = dtTemp.Rows[0]["barcode"].ToString();
                                ORKEY = dtTemp.Rows[0]["orkey"].ToString();
                            }
                            j = j + 1;
                            sqb.AppendFormat(BARCODE + ",", BARCODE);
                            sqb.AppendFormat("{0}" + ",", dt.Rows[i]["客户名称"].ToString());
                            sqb.AppendFormat("{0}" + ",", dt.Rows[i]["型号"].ToString());
                            sqb.AppendFormat("{0}" + ",", dt.Rows[i]["品名"].ToString());
                            sqb.AppendFormat("{0}" + ",", dt.Rows[i]["材料"].ToString());
                            sqb.AppendFormat("{0}" + ",", dt.Rows[i]["数量"].ToString());
                            sqb.AppendFormat("{0}" + ",", dt.Rows[i]["下单日期"].ToString());

                            sqb.AppendFormat("{0}" + ",", dt.Rows[i]["重量"].ToString());
                            sqb.AppendFormat("{0}" + ",", dt.Rows[i]["订单交期"].ToString());
                            sqb.AppendFormat("{0}" + ",", dt.Rows[i]["模具编号"].ToString());
                            sqb.AppendFormat("\r\n");
                        }
                    }
                    ErrowInfo = "打印数据已发送至打印机";
                    hint.Text = ErrowInfo;

                    string v1 = "";
                    v1 = sqb.ToString().Replace(",", ",");
                    new CFileInfo().data_to_txt(path, v1);
                    hint.Text = "打印数据已写入txt文件";
                    System.Diagnostics.Process.Start("D:\\barcode\\barcode_bat.bat");
                    hint.Text = "已补印";
                }
                else
                {
                    ErrowInfo = "没有选中要打印的项";
                    return;
                }
            }
            else
            {
                ErrowInfo = "没有数据可打印";
                return;
            }



        }
        #endregion
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            /*if (SELECT == 1)
            {
                ORID = dt.Rows[dataGridView1.CurrentCell.RowIndex]["订单号"].ToString();
                DataTable dtx = bc.getdt(corder.sql + " WHERE A.ORID='"+ORID +"'");
                if(dtx.Rows.Count >0)
                {
                    ADDRESS = dtx.Rows[0]["公司地址"].ToString();
                    CONTACT = dtx.Rows[0]["联系人"].ToString();
                    PHONE = dtx.Rows[0]["联系电话"].ToString();
                    CNAME = dtx.Rows[0]["客户名称"].ToString();
                }
                this.Close();
            }*/
        }

        private void CheckBox2_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBox2.Checked)
            {
                foreach (DataRow dr in dt.Rows )
                {
                    dr["选取"] = true;
                }
            }
            else
            {
                foreach (DataRow dr in dt.Rows)
                {
                    dr["选取"] = false;
                }
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            getSelectOrid = "";
            List<string> list = new List<string>();
            List<string> list2 = new List<string>();//判断所选取的订单客户是不是同一家
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    if (Convert.ToBoolean(dr["选取"]))
                    {

                        if (!list.Contains(dr["订单号"].ToString()))
                        {
                            list.Add(dr["订单号"].ToString());
                        }
                        if (!list2.Contains(dr["客户名称"].ToString()))
                        {
                            list2.Add(dr["客户名称"].ToString());
                        }
                        //MessageBox.Show(dr["订单号"].ToString());
                    }

                }
                foreach (string listitem in list)
                {
                    if (getSelectOrid =="")
                    {
                        getSelectOrid = "'" + listitem + "'";
                    }
                    else
                    {
                        getSelectOrid = getSelectOrid + "," + "'" + listitem + "'";

                    }
                }
                if (list2.Count > 1)
                {
                    MessageBox.Show("一个销货单只能选择同一个客户的订单");
                    return;
                }
                //MessageBox.Show(getSelectOrid);
                this.Close();
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            corder.MAKERID = "";
            if (!File.Exists("D:\\barcode\\barcode.btw"))
            {
                MessageBox.Show("D:\\barcode\\barcode.btw下不存在打印模版文件");
                return;
            }
            if (File.Exists("D:\\barcode\\barcode_data.txt"))
                File.Delete("D:\\barcode\\barcode_data.txt");
            if (File.Exists("D:\\barcode\\barcode_data1.txt"))
                File.Delete("D:\\barcode\\barcode_data1.txt");
            rePrint(dataGridView1, "D:\\barcode\\barcode_data1.txt");
            //corder.ExcelPrint(dataGridView1, "订单", System.IO.Path.GetFullPath("订单.xlsx"));
            //corder.ExcelPrint_40X30(dataGridView1, "订单", System.IO.Path.GetFullPath("订单40X30.xlsx"));
            //hint.Text = corder.ErrowInfo;
            try
            {

            }
            catch (Exception MyEx)
            {
                MessageBox.Show(MyEx.Message, "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}

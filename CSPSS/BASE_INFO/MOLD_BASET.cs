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
using System.Net;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using Newtonsoft.Json.Linq;
using System.Configuration;

namespace CSPSS.BASE_INFO
{
    public partial class MOLD_BASET : Form
    {
        DataTable dt = new DataTable();
        basec bc=new basec ();
        CFileInfo cfileinfo = new CFileInfo();
        StringBuilder sqb = new StringBuilder();
        private string _IDO;
        public string IDO
        {
            set { _IDO = value; }
            get { return _IDO; }

        }
        private static string _MATERIAL;
        public static string MATERIAL
        {
            set { _MATERIAL = value; }
            get { return _MATERIAL; }
        }
        public bool IF_IMPORT_SUCCESS { set; get; }
        private string _MAID;
        public string MAID
        {
            set { _MAID = value; }
            get { return _MAID; }
        }
        private string _INITIAL_OR_OTHER;
        public string INITIAL_OR_OTHER
        {
            set { _INITIAL_OR_OTHER = value; }
            get { return _INITIAL_OR_OTHER; }
        }
        private string _CUID;
        public string CUID
        {
            set { _CUID = value; }
            get { return _CUID; }
        }
        private string _WEIGHT;
        public string WEIGHT
        {
            set { _WEIGHT = value; }
            get { return _WEIGHT; }
        }
        public bool if_import { set; get; }
        private bool _IFExecutionSUCCESS;
        public bool IFExecution_SUCCESS
        {
            set { _IFExecutionSUCCESS = value; }
            get { return _IFExecutionSUCCESS; }

        }
        private static bool _IF_DOUBLE_CLICK;
        public static bool IF_DOUBLE_CLICK
        {
            set { _IF_DOUBLE_CLICK = value; }
            get { return _IF_DOUBLE_CLICK; }

        }
        private string _WATER_MARK_CONTENT;
        public string WATER_MARK_CONTENT
        {
            set { _WATER_MARK_CONTENT = value; }
            get { return _WATER_MARK_CONTENT; }

        }
        private string _OLD_FILE_NAME;
        public string OLD_FILE_NAME
        {
            set { _OLD_FILE_NAME = value; }
            get { return _OLD_FILE_NAME; }

        }
        private string _NEW_FILE_NAME;
        public string NEW_FILE_NAME
        {
            set { _NEW_FILE_NAME = value; }
            get { return _NEW_FILE_NAME; }

        }

        private CBOM cBOM;
        private DataTable dtx;
        protected int M_int_judge, i;
        protected int select;
        CMOLD_BASE cMOLD_BASE = new CMOLD_BASE();
        MOLD_BASE mOLD_BASE = new MOLD_BASE();
        DataTable dt3 = new DataTable();
        private class CUID_AND_MATERIAL
        {
            public string CUID { set; get; }
            public string CNAME { set; get; }
            public string MAID { set; get; }
            public string MATERIAL { set; get; }
            public int totalCount { set; get; }

        }
        private List<CUID_AND_MATERIAL> list;

        public MOLD_BASET()
        {
            InitializeComponent();
        }
        public MOLD_BASET(MOLD_BASE frm)
        {
            mOLD_BASE = frm;
            InitializeComponent();
        }
        private void MOLD_BASET_Load(object sender, EventArgs e)
        {

             this.Icon = Resource1.xz_200X200;
            hint.Location = new Point(256, 136);
            hint.ForeColor = Color.Red;
            //comboBox1.BackColor = CCOLOR.YELLOW;
            //textBox2.BackColor = CCOLOR.YELLOW;
            //comboBox2.BackColor = CCOLOR.YELLOW;
            hint.Text = "";
            DataGridViewCheckBoxColumn dgvc1 = new DataGridViewCheckBoxColumn();
            dgvc1.Name = "复选框";
            dataGridView2.Columns.Add(dgvc1);
            DataGridViewTextBoxColumn dgvc2 = new DataGridViewTextBoxColumn();
            dgvc2.Name = "文件名";
            dataGridView2.Columns.Add(dgvc2);
            DataGridViewImageColumn dgvc3 = new DataGridViewImageColumn();

            dgvc3.Name = "缩略图";
            dataGridView2.Columns.Add(dgvc3);
            DataGridViewTextBoxColumn dgvc4 = new DataGridViewTextBoxColumn();
            dgvc4.Name = "索引";
            dgvc4.Visible = false;
            dataGridView2.Columns.Add(dgvc4);
            DataGridViewTextBoxColumn dgvc5 = new DataGridViewTextBoxColumn();
            dgvc5.Name = "新文件名";
            dgvc5.Visible = false;
            dataGridView2.Columns.Add(dgvc5);
            label52.Text = "";
            label53.Visible = false;
            label55.Visible = false;
            label56.Visible = false;
            label57.Visible = false;
            progressBar1.Visible = false;
            dataGridView1.EditMode = DataGridViewEditMode.EditOnEnter;


            bind();
            bind2();
            try
            {
               
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
        }

  
        public void a1()
        {
            dataGridView1.ReadOnly = true;
            select = 0;
        }
        public void a2()
        {
            dataGridView1.ReadOnly = true;
            select = 1;
        }
        public void ClearText()
        {
            hint.Text = "";
            label52.Text = "";

        }

        #region bind
        private void bind()
        {
       
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.ContextMenuStrip = contextMenuStrip1;
            dtx = basec.getdts(cMOLD_BASE.sql + " where a.mbid='" + IDO + "' ORDER BY  A.mbid ASC ");
            if (dtx.Rows.Count > 0)
            {

                dt = total();//继承表结构
                             /*foreach (DataRow dr1 in dtx.Rows)
                              {
                                  DataRow dr = dt.NewRow();
                                  dr["项次"] = dr1["项次"].ToString();
                                  dr["型号"] = dr1["型号"].ToString();
                                  dr["材料"] = dr1["材料"].ToString();
                                  dr["重量"] = dr1["重量"].ToString();
                                  dr["模具编号"] = dr1["模具编号"].ToString();
                                  dr["mbid"] = dr1["mbid"].ToString();
                                  dr["cuid"] = dr1["cuid"].ToString();
                                  dr["maid"] = dr1["maid"].ToString();
                                  dt.Rows.Add(dr);
                              }*/
                dt = dtx;
                /*if (dt.Rows.Count > 0 && dt.Rows.Count < 6)
                {
                    int n = 1;
                    for (int i = 0; i < n; i++)
                    {
                        DataRow dr = dt.NewRow();
                        int b1 = Convert.ToInt32(dt.Rows[dt.Rows.Count - 1]["项次"].ToString());
                        dr["项次"] = Convert.ToString(b1 + 1);
                        dt.Rows.Add(dr);
                    }
                }*/

            }
            else
            {
                dt = total1();//先继承表结构再添加默认输入的6行
            }

            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.DataSource = dt;
          
        }
        #endregion
        private void btnAdd_Click(object sender, EventArgs e)
        {
            ClearText();
            IDO = cMOLD_BASE.GETID();
            dt = total1();
            dataGridView1.DataSource = dt;
            bind2();

        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            btnSave.Enabled = true;
            M_int_judge = 1;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            btnSave.Focus();//将焦点移到BTNSAVE主是为了在DATAGRIDVIEW编辑后使其失去焦点，得到更新后的DT
            if (juage2())
            {
                
            }
            else
            {
              
                Microsoft.Practices.EnterpriseLibrary.Data.Database database = bc.getdb();
                using (System.Data.Common.DbConnection dbconnection = database.CreateConnection())
                {
                    dbconnection.Open();
                    System.Data.Common.DbTransaction dbTransaction = dbconnection.BeginTransaction();
                    cMOLD_BASE.EMID = LOGIN.EMID;
                    cMOLD_BASE.save(dt, database, dbTransaction);
                    dbTransaction.Commit();
                    try
                    {
                   
                

                    }
                    catch (Exception ex)
                    {

                        dbTransaction.Rollback();
                        MessageBox.Show(ex.Message);
                    }
                    finally
                    {
                        dbconnection.Close();//不管执行成功与失败均要释放数据库连接，避免影响数据库资源，使数据查询相关资源变慢
                      
                    }

                    IFExecution_SUCCESS = cMOLD_BASE.IFExecution_SUCCESS;
                    hint.Text = cMOLD_BASE.ErrowInfo;
                    if (IFExecution_SUCCESS)
                    {
                        if (bc.GET_IFExecutionSUCCESS_HINT_INFO(IFExecution_SUCCESS) != "")
                        {

                            hint.Text = bc.GET_IFExecutionSUCCESS_HINT_INFO(IFExecution_SUCCESS);
                        }
                        else
                        {
                            hint.Text = "";
                        }
                        mOLD_BASE.bind();
                    }
                }

            }
       

        }
  

               
        private bool juage()
        {
            bool b = false;
            /*if (comboBox1.Text == "")
          {
              hint.Text = "客户名称不能为空";
              b = true;
          }
          else if (!bc.exists("SELECT * FROM CUSTOMERINFO_MST WHERE CNAME='"+comboBox1 .Text +"'"))
          {
              hint.Text = "系统不存在该客户名称";
              b = true;
          }
         else if (textBox1 .Text  == "")
           {
               hint.Text = "型号不能为空";
               b = true;
           }
          else if (comboBox2.Text == "")
          {
              hint.Text = "材料不能为空";
              b = true;
          }
          else if (comboBox2 .Text !="" && !bc.exists("SELECT * FROM MATERIAL WHERE MATERIAL='" + comboBox2.Text + "'"))
          {
              hint.Text = "系统不存在该材料";
              b = true;
          }
          else if (textBox2 .Text  == "")
          {
              hint.Text = "重量不能为空";
              b = true;
          }
          else if (textBox2 .Text !="" &&  bc.yesno (textBox2 .Text ) ==0)
          {
              hint.Text = "重量只能输入数字";
              b = true;
          }*/
            return b;
        }
        #region juage2()
        private bool juage2()
        {
            bool b = false;
            dt = (DataTable)dataGridView1.DataSource;
            DataTable dtx = bc.GET_NOEXISTS_EMPTY_ROW_DT(dt, "", "客户名称 IS NOT NULL");
            dt = dtx;
            list = new List<CUID_AND_MATERIAL>();
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dtx.Rows)
                {
                    CUID_AND_MATERIAL cUID_AND_MATERIAL = new CUID_AND_MATERIAL();

                    CUID = bc.getOnlyString("SELECT CUID FROM CUSTOMERINFO_MST WHERE CNAME='" + dr["客户名称"].ToString() + "'");
                    MAID = bc.getOnlyString("SELECT MAID FROM MATERIAL WHERE MATERIAL='" + dr["材料"].ToString() + "'");
                    dr["CUID"] = CUID;
                    dr["MAID"] = MAID;


                    string v1 = dr["型号"].ToString();
                    string v2 = dr["材料"].ToString();
                    string v3 = dr["重量"].ToString();
                    string v4 = dr["模具编号"].ToString();
                    if (v2 == "")
                    {
                        b = true;
                        hint.Text = "项次" + dr["项次"].ToString() + " 材料不能为空！";
                        break;

                    }
                    else if (!bc.exists("SELECT * FROM MATERIAL WHERE MATERIAL='" + v2 + "'"))
                    {
                        b = true;
                        hint.Text = "项次" + dr["项次"].ToString() + " 系统不存在该材料！";
                        break;
                    }
                    else if (v3 != "" && bc.checkphone(v3) == false)
                    {
                        b = true;
                        hint.Text = "项次" + dr["项次"].ToString() + " 重量只能输入数字！";
                        break;
                    }
                }
                if (b == true)
                    return b;
                /*判断提交的数据是否有重复的订单ID与材料ID START*/
                DataTable dtx1 = basec.getGroupBydt(dt);
                foreach (DataRow dr in dtx1.Rows)
                {
                    if (Convert.ToInt32(dr["totalCount"].ToString()) > 1)
                    {
                        hint.Text = "项次：" + dr["项次"].ToString()
                            + " 客户名称：" + dr["客户名称"].ToString() + " + " + dr["材料"].ToString() + "出现相同的项";
                        b = true;
                        break;
                    }
                }
                //MessageBox.Show(dt.Rows.Count.ToString() + "," + dtx.Rows.Count.ToString());
                /*判断提交的数据是否重复的订单ID与材料ID END*/

            }

            else
            {
                b = true;

                hint.Text = "至少有一项客户名称不为空的项才能保存";
            }
       
        
            return b;
        }
        #endregion
        #region juage3()
        private int juage3()
        {
            DataTable dtx = bc.GET_NOEXISTS_EMPTY_ROW_DT(dt, "", "联系人 IS NOT NULL");
            int n = 0;
            foreach (DataRow dr in dtx.Rows)
            {
                string v1 = dr["默认联系人"].ToString();
                if (v1=="True")
                {
                    n = n + 1;

                }
            }
            return n;
        }
        #endregion
        private DataTable total()
        {
            DataTable dtt2 = new DataTable();
            dtt2.Columns.Add("项次", typeof(string));
            dtt2.Columns.Add("客户名称",typeof(string));
            dtt2.Columns.Add("型号", typeof(string));
            dtt2.Columns.Add("材料", typeof(string));
            dtt2.Columns.Add("重量", typeof(double));
            dtt2.Columns.Add("模具编号", typeof(string));
            dtt2.Columns.Add("cuid", typeof(string));
            dtt2.Columns.Add("maid", typeof(string));
            dtt2.Columns.Add("totalcount", typeof(int));
            dtt2.Columns.Add("ID", typeof(string));
            dtt2.Columns.Add("MBID", typeof(string));
            dtt2.Columns.Add("wname", typeof(string));
            dtt2.Columns.Add("remark", typeof(string));
            return dtt2;
        }
        private DataTable total1()
        {
            DataTable dtt2 = new DataTable();
            dtt2 = total();
            for (i = 1; i <= 1; i++)
            {
                DataRow dr = dtt2.NewRow();
                dr["项次"] = i;
                dtt2.Rows.Add(dr);
            }
            return dtt2;
        }
        private void btnDel_Click(object sender, EventArgs e)
        {
            try
            {
                 if (MessageBox.Show("确定要删除该条凭证吗？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    basec.getcoms("DELETE MOLD_BASE WHERE MBID='" + IDO + "';delete warefile where wareid='"+IDO +"'");
                    ClearText();
                    IDO = cMOLD_BASE.GETID();//重新产生IDO供图片上传使用
                    bind();
                    bind2();
                    mOLD_BASE.bind();//刷新查询窗体数据
                    
                }
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

        #region override enter
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Enter && ((!(ActiveControl is System.Windows.Forms.TextBox) ||
                !((System.Windows.Forms.TextBox)ActiveControl).AcceptsReturn)))
            {


                    SendKeys.SendWait("{Tab}");
                
                return true;
            }
            if (keyData == (Keys.Enter | Keys.Shift))
            {
                SendKeys.SendWait("+{Tab}");

                return true;
            }
            if (keyData == (Keys.F7))
            {

                //double_info();

                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
        #endregion
        #region dgvStateControl
        private void dgvStateControl()
        {
            int i;
            dataGridView1.RowHeadersDefaultCellStyle.BackColor = Color.Lavender;
            int numCols1 = dataGridView1.Columns.Count;
            dataGridView1.Columns["项次"].ReadOnly = true;
            dataGridView1.Columns["客户名称"].ReadOnly = true;
            dataGridView1.Columns["材料"].ReadOnly = true;
            dataGridView1.Columns["项次"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomCenter;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;/*自动调整DATAGRIDVIEW的列宽*/
            for (i = 0; i < numCols1; i++)
            {

                dataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                this.dataGridView1.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                //this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
                dataGridView1.EnableHeadersVisualStyles = false;
                dataGridView1.Columns[i].HeaderCell.Style.BackColor = Color.Lavender;

            }
            for (i = 0; i < dataGridView1.Columns.Count; i++)
            {
                dataGridView1.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridView1.Columns[i].DefaultCellStyle.BackColor = Color.OldLace;
                i = i + 1;
            }
            dataGridView2.AllowUserToAddRows = false;
            if (dataGridView1.Rows.Count > 0)
            {

                dataGridView2.RowHeadersDefaultCellStyle.BackColor = Color.Lavender;
                int numCols2 = dataGridView2.Columns.Count;
                dataGridView2.Columns["复选框"].Width = 50;
                dataGridView2.Columns["文件名"].Width = 130;
                dataGridView2.Columns["索引"].Width = 130;

                for (i = 0; i < numCols2; i++)
                {

                    dataGridView2.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    this.dataGridView1.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                    //this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
                    dataGridView2.EnableHeadersVisualStyles = false;
                    dataGridView2.Columns[i].HeaderCell.Style.BackColor = Color.Lavender;
                }
                for (i = 0; i < dataGridView2.Columns.Count; i++)
                {
                    dataGridView2.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                    dataGridView2.Columns[i].DefaultCellStyle.BackColor = Color.OldLace;
                    i = i + 1;
                }
                dataGridView2.Columns["文件名"].ReadOnly = true;
                dataGridView2.Columns["索引"].ReadOnly = true;
            }
       
       
       
         
        }
        #endregion


        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

   

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
         
            /*string varDate = DateTime.Now.ToString("yyy/MM/dd").Replace("-", "/");
             a = dataGridView1.CurrentCell.ColumnIndex;
            int b = dataGridView1.CurrentCell.RowIndex;
            int c = dataGridView1.Columns.Count - 1;
            int d = dataGridView1.Rows.Count - 1;
            if (a == c && b == d)
            {
                if (dt.Rows.Count >= 1)
                {
                    DataRow dr = dt.NewRow();
                    int b1 = Convert.ToInt32(dt.Rows[dt.Rows.Count - 1]["项次"].ToString());
                    dr["项次"] = Convert.ToString(b1 + 1);
                    dt.Rows.Add(dr);

                }
            }
            try
            {
              
            }
            catch (Exception)
            {
                //MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);

            }*/

        }

        private void 删除此项ToolStripMenuItem_Click(object sender, EventArgs e)
        {
    
       
        }

        private void dataGridView1_Click(object sender, EventArgs e)
        {

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
        private void dgvCopy(ref DataGridView dgv)
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

   




        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            hint.Text = "";
            string v1 = Convert.ToString(dataGridView1["编号", dataGridView1.CurrentCell.RowIndex].Value).Trim();
          
            if(v1!="")//V1有值才替换否则新增时要有ID才能上传图片
            { IDO = v1; }
            if (v1 != "")
            {
                bind2();
            }
            else
            {

                dataGridView2.Rows.Clear();//清除图片显示
            }
        
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            if (dt.Rows.Count > 0)
            {

                bc.dgvtoExcel(dataGridView1, this.Text);
            }
            else
            {
                MessageBox.Show("没有数据可导出！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnupload_Click(object sender, EventArgs e)
        {
            DataTable dty = bc.getdt("SELECT * FROM WAREFILE WHERE WAREID='" + IDO + "'");
            if (IDO == "")
            {
                hint.Text = "编号不能为空";
            }
            else if (dty.Rows.Count.ToString() == "6")
            {

                hint.Text = "最多只能上传三张图片";
            }
            else
            {
                uploadfile();
            }
            try
            {
          

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);

            }
        }
        #region uploadfile
        private void uploadfile()
        {
            int i = 0;
            label53.Visible = false;
            label55.Visible = false;
            label56.Visible = false;
            label57.Visible = false;
            progressBar1.Visible = false;
            /*  string v2 = bc.getOnlyString("SELECT EDIT FROM RIGHTLIST WHERE USID='" + LOGIN.USID + "' AND NODE_NAME='传单作业'");
              if (v2 != "Y" && ADD_OR_UPDATE == "UPDATE")
              {
                  hint.Text = "您没有修改权限不能修改上传";
              }
              else*/
            label52.Text = "";
            if (bc.RETURN_SERVER_IP_OR_DOMAIN() == "")
            {
                hint.Text = "未设置服务器IP或域名";
            }

            else
            {
                OpenFileDialog openf = new OpenFileDialog();
                if (openf.ShowDialog() == DialogResult.OK)
                {
                    Random ro = new Random();
                    string stro = ro.Next(80, 10000000).ToString() + "-";
                    string NeWAREID = DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + DateTime.Now.Millisecond.ToString() + stro;

                    cfileinfo.SERVER_IP_OR_DOMAIN = bc.RETURN_SERVER_IP_OR_DOMAIN();
                    WATER_MARK_CONTENT = "";//水印内容
                    //cfileinfo.UploadImage(openf.FileName, Path.GetFileName(openf.FileName), textBox1 .Text );
                    //this.UploadFile(openf.FileName, System.IO.Path.GetFileName(openf.FileName), "File/", textBox1.Text);

                    string v21 = bc.FROM_RIGHT_UNTIL_CHAR(Path.GetFileName(openf.FileName), 46);
                    OLD_FILE_NAME = Path.GetFileName(openf.FileName);
                    NEW_FILE_NAME = NeWAREID + Path.GetFileName(openf.FileName);
                    //如果上传的是图片文件
                    if (v21 == "jpeg" || v21 == "jpg" || v21 == "JPG" || v21 == "png" || v21 == "bmp" || v21 == "gif")
                    {
                        //裁切小图
                        cfileinfo.MakeThumbnail(openf.FileName, "d:\\" + Path.GetFileName(openf.FileName), 80, 80, "Cut");
                        //小图加水印
                        cfileinfo.ADD_WATER_MARK("d:\\" + Path.GetFileName(openf.FileName), "d:\\80X80" + NeWAREID + Path.GetFileName(openf.FileName), WATER_MARK_CONTENT);
                        //原图加水印
                        cfileinfo.ADD_WATER_MARK(openf.FileName, "d:\\INITIAL" + NeWAREID + Path.GetFileName(openf.FileName), WATER_MARK_CONTENT);
                        INITIAL_OR_OTHER = "INITIAL";
                      
                        //上传原图
                        i = Upload_Request(ConfigurationManager.AppSettings["api-url"]  + "/webuploadfile/default.aspx", "D:\\INITIAL" + NeWAREID + System.IO.Path.GetFileName(openf.FileName),
                                "INITIAL" + NeWAREID + System.IO.Path.GetFileName(openf.FileName), progressBar1, IDO );

                        //上传80X80的缩略图
                        INITIAL_OR_OTHER = "80X80";
                        i = Upload_Request(ConfigurationManager.AppSettings["api-url"]  + "/webuploadfile/default.aspx", "D:\\80X80" + NeWAREID + System.IO.Path.GetFileName(openf.FileName),
                                "80X80" + NeWAREID + System.IO.Path.GetFileName(openf.FileName), progressBar1, IDO );


                        //删除本地临时水印图及剪切图
                        if (File.Exists("d:\\80X80" + NeWAREID + Path.GetFileName(openf.FileName)))
                        {
                            File.Delete("d:\\80X80" + NeWAREID + Path.GetFileName(openf.FileName));
                            File.Delete("d:\\" + Path.GetFileName(openf.FileName));
                            File.Delete("d:\\INITIAL" + NeWAREID + Path.GetFileName(openf.FileName));
                        }
                    }
                    else
                    {
                        label53.Visible = true;
                        label55.Visible = true;
                        label56.Visible = true;
                        label57.Visible = true;
                        progressBar1.Visible = true;
                        //上传的是非图片文件
                        INITIAL_OR_OTHER = "INITIAL";
                        i = Upload_Request(ConfigurationManager.AppSettings["api-url"]  + "/webuploadfile/default.aspx", openf.FileName,
                                                      "INITIAL" + NeWAREID + System.IO.Path.GetFileName(openf.FileName), progressBar1, IDO );
                    }
                    if (i == 1)
                    {
                        label52.Text = "成功上传";
                    }
                    else
                    {
                        label52.Text = "上传失败";
                    }

                    bind2();
                }
            }

        }
        #endregion
        #region HttpWebRequst_uploadfile
        /// <summary>
        /// 将本地文件上传到指定的服务器(HttpWebRequest方法)
        /// </summary>
        /// <param name="address">文件上传到的服务器</param>
        /// <param name="fileNamePath">要上传的本地文件（全路径）</param>
        /// <param name="saveName">文件上传后的名称</param>
        /// <param name="progressBar">上传进度条</param>
        /// <returns>成功返回1，失败返回0</returns>
        /// 
        #region Upload_Request
        public int Upload_Request(string address, string fileNamePath, string saveName, ProgressBar progressBar, string WAREID)
        {
            int returnValue = 0;
            // 要上传的文件

            FileStream fs = new FileStream(fileNamePath, FileMode.Open, FileAccess.Read);
            BinaryReader r = new BinaryReader(fs);
            //时间戳
            string strBoundary = "----------" + DateTime.Now.Ticks.ToString("x");
            byte[] boundaryBytes = Encoding.ASCII.GetBytes("\r\n--" + strBoundary + "\r\n");
            //请求头部信息
            StringBuilder sb = new StringBuilder();
            sb.Append("--");
            sb.Append(strBoundary);
            sb.Append("\r\n");
            sb.Append("Content-Disposition: form-data; name=\"");
            sb.Append("file");
            sb.Append("\"; filename=\"");
            sb.Append(saveName);
            sb.Append("\"");
            sb.Append("\r\n");
            sb.Append("Content-Type: ");
            sb.Append("application/octet-stream");
            sb.Append("\r\n");
            sb.Append("\r\n");
            string strPostHeader = sb.ToString();


            byte[] postHeaderBytes = Encoding.UTF8.GetBytes(strPostHeader);
            // 根据uri创建HttpWebRequest对象
            HttpWebRequest httpReq = (HttpWebRequest)WebRequest.Create(new Uri(address));
            httpReq.Method = "POST";
            //对发送的数据不使用缓存
            httpReq.AllowWriteStreamBuffering = false;
            //设置获得响应的超时时间（300秒）
            httpReq.Timeout = 300000;
            httpReq.ContentType = "multipart/form-data; boundary=" + strBoundary;
            long length = fs.Length + postHeaderBytes.Length + boundaryBytes.Length;
            long fileLength = fs.Length;
            httpReq.ContentLength = length;
            if (fileLength / 1048576.0 > 2.5)
            {

                label52.Visible = false;
                label53.Visible = false;
                label55.Visible = false;
                label56.Visible = false;
                label57.Visible = false;
                progressBar1.Visible = false;
                MessageBox.Show("上传的图片长度为:" + (fileLength / 1048576.0).ToString("F2") + "M" + " 已经大于允许上传的2.5M");
            }
            else
            {
                try
                {
                    progressBar.Maximum = int.MaxValue;
                    progressBar.Minimum = 0;
                    progressBar.Value = 0;
                    //每次上传4k
                    int bufferLength = 4096;
                    byte[] buffer = new byte[bufferLength];
                    //已上传的字节数
                    long offset = 0;
                    //开始上传时间
                    DateTime startTime = DateTime.Now;
                    int size = r.Read(buffer, 0, bufferLength);

                    Stream postStream = httpReq.GetRequestStream();
                    //发送请求头部消息
                    postStream.Write(postHeaderBytes, 0, postHeaderBytes.Length);
                    while (size > 0)
                    {
                        postStream.Write(buffer, 0, size);
                        offset += size;
                        progressBar.Value = (int)(offset * (int.MaxValue / length));
                        TimeSpan span = DateTime.Now - startTime;
                        double second = span.TotalSeconds;
                        label53.Text = "已用时：" + second.ToString("F2") + "秒";

                        if (second > 0.001)
                        {
                            label55.Text = "平均速度：" + (offset / 1024 / second).ToString("0.00") + "KB/秒";
                        }
                        else
                        {
                            label55.Text = "正在连接…";
                        }
                        label56.Text = "已上传：" + (offset * 100.0 / length).ToString("F2") + "%";
                        label57.Text = (offset / 1048576.0).ToString("F2") + "M/" + (fileLength / 1048576.0).ToString("F2") + "M";
                        Application.DoEvents();
                        size = r.Read(buffer, 0, bufferLength);
                    }
                    //添加尾部的时间戳
                    postStream.Write(boundaryBytes, 0, boundaryBytes.Length);
                    postStream.Close();

                    string year = DateTime.Now.ToString("yy");
                    string month = DateTime.Now.ToString("MM");
                    string day = DateTime.Now.ToString("dd");
                    string varDate = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                    string v1 = bc.numYMD(20, 12, "000000000001", "SELECT * FROM WAREFILE", "FLKEY", "FL");
                    string newFileName, uriString;
                    newFileName = System.IO.Path.GetFileName(saveName);
                    uriString = ConfigurationManager.AppSettings["api-url"]  + "/uploadfile/" + newFileName;


                    String sql = @"
INSERT INTO  WAREFILE 
(
FLKEY,
WAREID,
OLD_FILE_NAME,
NEW_FILE_NAME,
PATH,
INITIAL_OR_OTHER,
DATE,
YEAR,
MONTH,
DAY
) 
VALUES
(
@FLKEY,
@WAREID,
@OLD_FILE_NAME,
@NEW_FILE_NAME,
@PATH,
@INITIAL_OR_OTHER,
@DATE,
@YEAR,
@MONTH,
@DAY

)";
                    SqlConnection sqlcon = bc.getcon();
                    SqlCommand sqlcom = new SqlCommand(sql, sqlcon);
                    sqlcom.Parameters.Add("@FLKEY", SqlDbType.VarChar, 20).Value = v1;
                    sqlcom.Parameters.Add("@WAREID", SqlDbType.VarChar, 20).Value = IDO;
                    sqlcom.Parameters.Add("@OLD_FILE_NAME", SqlDbType.VarChar, 100).Value = OLD_FILE_NAME;
                    sqlcom.Parameters.Add("@NEW_FILE_NAME", SqlDbType.VarChar, 100).Value = NEW_FILE_NAME;
                    sqlcom.Parameters.Add("@PATH", SqlDbType.VarChar, 100).Value = uriString;
                    sqlcom.Parameters.Add("@INITIAL_OR_OTHER", SqlDbType.VarChar, 100).Value = INITIAL_OR_OTHER;
                    sqlcom.Parameters.Add("@DATE", SqlDbType.VarChar, 20).Value = varDate;
                    sqlcom.Parameters.Add("@YEAR", SqlDbType.VarChar, 20).Value = year;
                    sqlcom.Parameters.Add("@MONTH", SqlDbType.VarChar, 20).Value = month;
                    sqlcom.Parameters.Add("@DAY", SqlDbType.VarChar, 20).Value = day;
                    sqlcon.Open();
                    sqlcom.ExecuteNonQuery();
                    sqlcon.Close();


                    //获取服务器端的响应
                    WebResponse webRespon = httpReq.GetResponse();
                    Stream s = webRespon.GetResponseStream();
                    StreamReader sr = new StreamReader(s);
                    //读取服务器端返回的消息
                    String sReturnString = sr.ReadLine();
                    s.Close();
                    sr.Close();
                    if (sReturnString == "Success")
                    {
                        returnValue = 1;
                    }
                    else if (sReturnString == "Error")
                    {
                        returnValue = 0;
                    }
                }
                catch
                {
                    returnValue = 0;
                }
                finally
                {
                    fs.Close();
                    r.Close();
                }
            }
            return returnValue;
        }
        #endregion
        #endregion
        #region bind2
        private void bind2()
        {
            
            dt3 = bc.getdt(@"
SELECT cast(0   as   bit)   as   复选框,
OLD_FILE_NAME AS 文件名,NEW_FILE_NAME AS 新文件名,FLKEY AS 索引,
PATH FROM WAREFILE WHERE WAREID='" + IDO + "'  AND INITIAL_OR_OTHER='80X80'");


            dataGridView2.Rows.Clear();//在下一次增加行前需清空上一次产生的行，否则显示行数不正常
            for (int i = 0; i < dt3.Rows.Count; i++)
            {

                DataGridViewRow dgr = new DataGridViewRow();
                dataGridView2.Rows.Add(dgr);
                dataGridView2["复选框", i].Value = false;
                dataGridView2["文件名", i].Value = dt3.Rows[i]["文件名"].ToString();
                dataGridView2["缩略图", i].Value = Image.FromStream(System.Net.WebRequest.Create(dt3.Rows[i]["PATH"].ToString()).GetResponse().GetResponseStream());
                dataGridView2["索引", i].Value = dt3.Rows[i]["索引"].ToString();

            }
            for (i = 0; i < dataGridView2.Rows.Count; i++)
            {
                dataGridView2.Rows[i].Height = 80;
            }
            this.WindowState = FormWindowState.Maximized;
            Color c = System.Drawing.ColorTranslator.FromHtml("#efdaec");

            dgvStateControl();
        }
        #endregion
    

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                int i = dataGridView2.CurrentCell.RowIndex;

                if (dataGridView2.CurrentCell.ColumnIndex == 1)
                {
                    SaveFileDialog sfl = new SaveFileDialog();
                    sfl.FileName = dt3.Rows[dataGridView2.CurrentCell.RowIndex]["文件名"].ToString();
                    sfl.DefaultExt = "jpg";
                    sfl.Filter = "(*.jpg)|*.jpg";
                    if (sfl.ShowDialog() == DialogResult.OK)
                    {
                        sqb = new StringBuilder();
                        sqb.AppendFormat("SELECT PATH FROM WAREFILE WHERE ");
                        sqb.AppendFormat(" NEW_FILE_NAME='{0}'", dt3.Rows[i]["新文件名"].ToString());
                        sqb.AppendFormat(" AND INITIAL_OR_OTHER='INITIAL'");
                        WebClient wclient = new WebClient();
                        string v1 = bc.getOnlyString(sqb.ToString());
                        wclient.DownloadFile(v1, sfl.FileName);

                        /*DataTable dt3x = bc.getdt("SELECT * FROM WAREFILE WHERE FLKEY='" + dt3.Rows[dataGridView1.CurrentCell.RowIndex]["索引"].ToString() + "'");
                        Byte[] byte2 = (byte[])dt3x.Rows[0]["IMAGE_DATA"];
                        System.IO.File.WriteAllBytes(sfl.FileName, byte2);*/
                        hint.Text = "已下载";
                    }
                }

            }
            catch (Exception)
            {
                //MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);

            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            IF_IMPORT_SUCCESS = false;
          
            OpenFileDialog opfv = new OpenFileDialog();
            if (opfv.ShowDialog() == DialogResult.OK)
            {
                string path = opfv.FileName;

                /*cinventory.IF_IMPORT = true;
                cinventory.showdata(path);
                if (cinventory.IFExecution_SUCCESS)
                {
                    IF_IMPORT_SUCCESS = true;
                    hint.Text = "导入成功";

                }*/

                EXCEL_IMPORT(path);
              
            }
            try
            {


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        protected void EXCEL_IMPORT(string path)
        {

            hint.Text = "";
            DataTable dt1 = cfileinfo.importExcelToDataSet(path, ExcelToCSHARP.GetExcelFirstTableName(path)).Tables[0];
            DataView dv = new DataView(dt1);
            dv.RowFilter = "F1 IS NOT NULL";
            dtx = dv.ToTable();
            if (dtx.Rows.Count >= 2)
            {
                if (juage(dtx))
                {

                }
                else
                {
                    EXCEL_IMPORT(dtx);
                }
            }
            else
            {
                hint.Text = "无导入的数据！";
            }
        }
        protected void EXCEL_IMPORT(DataTable dtx)
        {
            dt = null;
            dt = new DataTable();
            dt = total();
            int j = 1;
            for (i = 1; i < dtx.Rows.Count; i++)
            {
                      

                DataRow dr = dt.NewRow();
                dr["项次"] = Convert.ToString(j);
                dr["客户名称"] = dtx.Rows[i]["F1"].ToString();
                dr["型号"] = dtx.Rows[i]["F2"].ToString();
                dr["材料"] = dtx.Rows[i]["F3"].ToString();
                dr["重量"] = dtx.Rows[i]["F4"].ToString();
                dr["模具编号"] = dtx.Rows[i]["F5"].ToString();
                dt.Rows.Add(dr);
                j = j + 1;
            }
            if (dt.Rows.Count > 0 && dt.Rows.Count < 6)
            {
                int n = 6 - dt.Rows.Count;
                for (int i = 0; i < n; i++)
                {
                    DataRow dr = dt.NewRow();
                    int b1 = Convert.ToInt32(dt.Rows[dt.Rows.Count - 1]["项次"].ToString());
                    dr["项次"] = Convert.ToString(b1 + 1);
                    dt.Rows.Add(dr);
                }
            }
            dataGridView1.DataSource = dt;
            dgvStateControl();
        }
        #region JUAGE()
        private bool juage(DataTable dt)
        {

            bool b = false;
            /*for (i = 2; i < dt.Rows.Count; i++)
            {

                string v1 = dt.Rows[i]["F1"].ToString();
                string v2 = dt.Rows[i]["F2"].ToString();
                string v3 = dt.Rows[i]["F3"].ToString();
                string v4 = dt.Rows[i]["F4"].ToString().Trim();
                string v5 = dt.Rows[i]["F5"].ToString();
                DateTime temp = DateTime.MinValue;
                if (v1 == "")
                {

                }
                else if (!DateTime.TryParse(v3, out temp))
                {
                    b = true;
                    hint.Text = "日期格式不正确，需为：yyyy/MM/dd！";
                    MessageBox.Show("日期格式不正确，需为：yyyy/MM/dd！");
                    break;

                }
                else if (v3.Length != 10 || v3.Substring(4, 1) != "/" || v3.Substring(7, 1) != "/")
                {
                    b = true;
                    hint.Text = "";
                    MessageBox.Show("日期长度需10位且格式：yyyy/MM/dd！");
                    break;
                }
                else if (!bc.exists("SELECT * FROM WAREINFO WHERE CO_WAREID='" + v1 + "' AND ACTIVE='Y'"))
                {
                    b = true;
                    hint.Text = "成品料号：" + v1 + "不存在于系统中或状态不为正常！";
                    MessageBox.Show("成品料号：" + v1 + "不存在于系统中或状态不为正常！");
                    break;
                }
                else if (v4 != "Y" && v4 != "N")
                {

                    b = true;
                    hint.Text = "生效否只能输入Y OR N！";
                    MessageBox.Show(string.Format("第 {0} 行生效否只能输入Y OR N！", (i + 1).ToString()));
                    break;
                }
                else if (v5 == "")
                {

                    b = true;
                    MessageBox.Show(string.Format("第 {0} 行料号不能为空！", (i + 1).ToString()));
                    break;
                }
                else if (!bc.exists("SELECT * FROM WAREINFO WHERE CO_WAREID='" + v5 + "' AND ACTIVE='Y'"))
                {
                    b = true;
                    hint.Text = "料号：" + v5 + " 不存在于系统中或状态不为正常！";
                    MessageBox.Show("料号：" + v5 + " 不存在于系统中或状态不为正常！");
                    break;
                }


            }*/
            return b;

        }
        #endregion
        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            IF_DOUBLE_CLICK = false;
            int rows = dataGridView1.CurrentCell.RowIndex;
            int columns = dataGridView1.CurrentCell.ColumnIndex;
            dt = (DataTable)dataGridView1.DataSource;
            if (dataGridView1.Columns[columns].Name.ToString() == "客户名称")
            {

                BASE_INFO.CUSTOMER_INFO FRM = new CSPSS.BASE_INFO.CUSTOMER_INFO();
                FRM.SELECT = 1;
                FRM.ShowDialog();
                if (FRM.IF_DOUBLE_CLICK)
                {
                    dataGridView1.Rows[rows].Cells["客户名称"].Value = FRM.CNAME;
                    dataGridView1.CurrentCell = dataGridView1["型号", rows];
                }
            }
            else if (dataGridView1.Columns[columns].Name.ToString() == "材料")
            {

                BASE_INFO.MATERIAL FRM = new BASE_INFO.MATERIAL();
                FRM.SELECT = 1;
                FRM.ShowDialog();
                if (FRM.MATERIAL_VALUE != "")
                {
                    dataGridView1.Rows[rows].Cells["材料"].Value = FRM.MATERIAL_VALUE;
                    dataGridView1.CurrentCell = dataGridView1["重量", rows];
                }
            }
        }

        private void btndelfile_Click(object sender, EventArgs e)
        {

            try
            {
                /*string v21 = bc.getOnlyString("SELECT EDIT FROM RIGHTLIST WHERE USID='" + LOGIN.USID + "' AND NODE_NAME='传单作业'");
                if (v21 != "Y" && ADD_OR_UPDATE == "UPDATE")
                {
                    hint.Text = "您没有修改权限不能删除文件";
                }
                else if (vou.CheckIfALLOW_SAVEOR_DELETE(textBox1.Text, LOGIN.USID))
                {
                    hint.Text = vou.ErrowInfo;
                }
                else
                {
                

                }*/
                if (MessageBox.Show("确定要删除该文件吗？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    if (dt3.Rows.Count > 0)
                    {

                        for (int i = 0; i < dt3.Rows.Count; i++)
                        {
                            if (dataGridView2.Rows[i].Cells[0].EditedFormattedValue.ToString() == "True")
                            {

                                string v2 = dt3.Rows[i]["索引"].ToString();
                                string v4 = dt3.Rows[i]["新文件名"].ToString();
                                bc.getcom(@"INSERT INTO SERVER_DELETE_FILE(FLKEY,NEW_FILE_NAME) VALUES ('" + v2 + "','" + v4 + "')");
                                bc.getcom("DELETE WAREFILE WHERE NEW_FILE_NAME='" + v4 + "'");

                            }
                        }
                        bind2();

                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);

            }
        }
    }
}

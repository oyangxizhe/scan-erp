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
using System.Linq;
namespace CSPSS.BASE_INFO
{
    public partial class MOLD_BASE : Form
    {
        DataTable dt = new DataTable();
        basec bc=new basec ();
        CFileInfo cfileinfo = new CFileInfo();
        StringBuilder sqb;
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
        public MOLD_BASE()
        {
            InitializeComponent();
        }

        private void MOLD_BASE_Load(object sender, EventArgs e)
        {

             this.Icon = Resource1.xz_200X200;
            hint.Location = new Point(256, 136);
            hint.ForeColor = Color.Red;
            //comboBox1.BackColor = CCOLOR.YELLOW;
            //textBox2.BackColor = CCOLOR.YELLOW;
            //comboBox2.BackColor = CCOLOR.YELLOW;
            hint.Text = "";
 
            dataGridView1.EditMode = DataGridViewEditMode.EditOnEnter;
            dataGridView1.AutoGenerateColumns = false;
            //bind();
         
            dataGridView1.AllowUserToAddRows = false;
         
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

            comboBox1.Text = "";
            textBox1.Text = "";
            comboBox2.Text = "";
            textBox2.Text = "";
        }

        #region bind
        public  void bind()
        {
       
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.ContextMenuStrip = contextMenuStrip1;
     
         
            /*
            textBox1.BackColor = CCOLOR.YELLOW;
            textBox2.BackColor = CCOLOR.YELLOW;*/
            if (bc.GET_IFExecutionSUCCESS_HINT_INFO(IFExecution_SUCCESS) != "")
            {

                hint.Text = bc.GET_IFExecutionSUCCESS_HINT_INFO(IFExecution_SUCCESS);
            }
            else
            {
                hint.Text = "";
            }
            StringBuilder sqb = new StringBuilder();
            sqb.Append(cMOLD_BASE. sql);
       
            sqb.Append(" WHERE B.CNAME LIKE '%" + comboBox1.Text + "%' ");
            sqb.Append(" AND A.WAREID LIKE '%" + textBox1.Text + "%'");
            sqb.Append(" AND C.MATERIAL LIKE '%" + comboBox2.Text  + "%'");
            sqb.Append(" AND A.WEIGHT LIKE '%" + textBox2 .Text  + "%'");
            sqb.Append(" AND A.WNAME LIKE '%" + textBox3.Text + "%'");
            sqb.Append(" AND A.MOLDNO LIKE '%" + textBox4.Text + "%'");
            sqb.Append(" AND A.REMARK LIKE '%" + textBox5.Text + "%'");
            dt = bc.getdt(sqb.ToString());
            dataGridView1.DataSource = dt;
            bind2();
            dgvStateControl();
        }
        #endregion
        private void btnAdd_Click(object sender, EventArgs e)
        {
            MOLD_BASET mOLD_BASET = new MOLD_BASET(this );
            mOLD_BASET.IDO = cMOLD_BASE.GETID();
            mOLD_BASET.Show();

        }

        private void btnEdit_Click(object sender, EventArgs e)
        {

            M_int_judge = 1;
        }

     
  

               
        private bool juage()
        {
            bool b = false;
            if (comboBox1.Text == "")
            {
                hint.Text = "客户名称不能为空";
                b = true;
            }
            else if (!bc.exists("SELECT * FROM CUSTOMERINFO_MST WHERE CNAME='"+comboBox1 .Text +"'"))
            {
                hint.Text = "系统不存在该客户名称";
                b = true;
            }
            /* else if (textBox1 .Text  == "")
             {
                 hint.Text = "型号不能为空";
                 b = true;
             }*/
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
            }
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
            dtt2.Columns.Add("WNAME", typeof(string));
            dtt2.Columns.Add("remark", typeof(string));
            return dtt2;
        }
        private DataTable total1()
        {
            DataTable dtt2 = new DataTable();
            dtt2 = total();
            for (i = 1; i <= 6; i++)
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
                    bind(); 
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

         
            dataGridView1.Columns["项次"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomCenter;
            for (i = 0; i < numCols1; i++)
            {
                dataGridView1.Columns[i].ReadOnly = true;
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
    
       
       
       
         
        }
        #endregion


        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

   

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {

            string varDate = DateTime.Now.ToString("yyy/MM/dd").Replace("-", "/");
            try
            {
                int a = dataGridView1.CurrentCell.ColumnIndex;
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
            }
            catch (Exception)
            {
                //MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);

            }

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

        private void comboBox1_DropDown(object sender, EventArgs e)
        {
      
            BASE_INFO.CUSTOMER_INFO FRM = new CSPSS.BASE_INFO.CUSTOMER_INFO();
            FRM.SELECT = 1;
            FRM.ShowDialog();
            this.comboBox1.IntegralHeight = false;//使组合框不调整大小以显示其所有项
            this.comboBox1.DroppedDown = false;//使组合框不显示其下拉部分
            this.comboBox1.IntegralHeight = true;//恢复默认值
            if (FRM.IF_DOUBLE_CLICK)
            {
                comboBox1.Text = FRM.CNAME;
                textBox1.Focus();
            }
          
        }

        private void comboBox2_DropDown(object sender, EventArgs e)
        {
            BASE_INFO.MATERIAL FRM = new BASE_INFO.MATERIAL();
            FRM.ORDER_USE();
            FRM.ShowDialog();
            if (FRM.MATERIAL_VALUE != "")
            {
              comboBox2 .Text = FRM.MATERIAL_VALUE;
              textBox2.Focus();
            }
            this.comboBox2.IntegralHeight = false;//使组合框不调整大小以显示其所有项
            this.comboBox2.DroppedDown = false;//使组合框不显示其下拉部分
            this.comboBox2.IntegralHeight = true;//恢复默认值
           
         
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            IFExecution_SUCCESS = false;
            hint.Text = "";
            bind();
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


    
        #region bind2
        private void bind2()
        {
            



      
            this.WindowState = FormWindowState.Maximized;
            Color c = System.Drawing.ColorTranslator.FromHtml("#efdaec");

            dgvStateControl();
        }
        #endregion
    


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
          
            bool b = false;
            string year, month, day;
            year = DateTime.Now.ToString("yy");
            month = DateTime.Now.ToString("MM");
            day = DateTime.Now.ToString("dd");
            string varDate = DateTime.Now.ToString("yyy/MM/dd HH:mm:ss").Replace("-", "/");
            //取得库存中所有有模具信息，用于判断已经存的要覆盖
            DataTable dtCache = new DataTable();

            //取得客户信息
            DataTable dtCustomer = new DataTable();

            //取得材料信息
            DataTable dtMaterial = new DataTable();

            //取得模具表当前最后一个ID，用于写入时自动生成ID,避免多次调数据库连接获取MBID
            DataTable dtGenerateId = new DataTable();

            sqb = new StringBuilder();
            sqb.AppendFormat(@"select * from mold_base a left join customerinfo_mst b on a.cuid=b.cuid
left join material c on a.maid = c.maid;");
            sqb.AppendFormat("select * from customerinfo_mst;");
            sqb.AppendFormat("select * from material;");
            sqb.AppendFormat("SELECT TOP 1 MBID FROM mold_base ");
            sqb.AppendFormat(" WHERE SUBSTRING (MBID ,3,2)=SUBSTRING (convert(varchar(4),DATEPART (YY,getdate()),111),3,2)");
            sqb.AppendFormat(" and SUBSTRING (MBID,5,2)=SUBSTRING (convert(varchar(10),getdate(),111),6,2) ");
            sqb.AppendFormat(" and SUBSTRING (MBID,7,2)=SUBSTRING (convert(varchar(10),getdate(),111),9,2) ORDER BY MBID DESC");
            SqlConnection sqlConnection = bc.getcon();
            SqlCommand sqlcom = new SqlCommand(sqb.ToString(), sqlConnection);
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlcom);
            DataSet dataSet = new DataSet();
            sqlDataAdapter.Fill(dataSet);
            dtCache = dataSet.Tables[0];
            dtCustomer = dataSet.Tables[1];
            dtMaterial = dataSet.Tables[2];
            dtGenerateId = dataSet.Tables[3];
            sqlConnection.Close();

            //写入前数据校验
            for(int i=1;i<dtx.Rows.Count;i++)
            {
                DataRow[] drArrayCustomer = dtCustomer.Select("CNAME='" + dtx.Rows[i]["F1"].ToString() + "'");
                DataRow[] drArrayMaterial = dtMaterial.Select("MATERIAL='" + dtx.Rows[i]["F3"].ToString() + "'");
                if (drArrayCustomer.Length <= 0)
                {
                    hint.Text = string.Format("客户名称：{0} 不存在系统中", dtx.Rows[i]["F1"].ToString());
                    b = true;
                    break;
                }
                if (drArrayMaterial.Length <= 0)
                {
                    hint.Text = string.Format("材料：{0} 不存在系统中", dtx.Rows[i]["F3"].ToString());
                    b = true;
                    break;
                }
            }
            if (b == true)
                return;
            List<String> listWaitDel = new List<string>();

            //取得要删除的旧数据等待删除
            for (int i = 1; i < dtx.Rows.Count; i++)
            {
                //根据客户名称找到CUID
                DataRow[] dataRowsCustomer = dtCustomer.Select("CNAME='" + dtx.Rows[i]["F1"].ToString() + "'");
                if (dataRowsCustomer.Length > 0)
                    CUID = dataRowsCustomer[0]["CUID"].ToString();

                //根据材料名称找到MAID
                DataRow[] dataRowsMaterial = dtMaterial.Select("MATERIAL='" + dtx.Rows[i]["F3"].ToString() + "'");
                if (dataRowsCustomer.Length > 0)
                    MAID = dataRowsMaterial[0]["MAID"].ToString();

                DataRow[] drWaitDel = dtCache.Select(String.Format("CUID='{0}' AND WAREID='{1}' AND  MAID='{2}' AND WNAME='{3}' ", 
                    CUID, dtx.Rows[i]["F2"].ToString(), MAID, dtx.Rows[i]["F5"].ToString()));
                if (drWaitDel.Length > 0)
                {
                    listWaitDel.Add(drWaitDel[0]["MBID"].ToString());//存下待删除的单号
                }

            }
            sqb = new StringBuilder();
            //删除旧数据

            if(listWaitDel.Count>0)//有旧数据才带出删除字串
            sqb = new StringBuilder(String.Format("delete MOLD_BASE where mbid in ({0});", basec.InConvert(listWaitDel)));
            //写入新数据 拼接insert语句批量执行]
            string ID;
            for (int i = 1; i < dtx.Rows.Count; i++)
            {

                //根据客户名称找到CUID
                DataRow[] dataRowsCustomer = dtCustomer.Select("CNAME='" + dtx.Rows[i]["F1"].ToString() + "'");
                if (dataRowsCustomer.Length > 0)
                    CUID = dataRowsCustomer[0]["CUID"].ToString();

                //根据材料名称找到MAID
                DataRow[] dataRowsMaterial = dtMaterial.Select("MATERIAL='" + dtx.Rows[i]["F3"].ToString() + "'");
                if (dataRowsCustomer.Length > 0)
                    MAID = dataRowsMaterial[0]["MAID"].ToString();
                if (dtGenerateId.Rows.Count > 0)
                    ID = dtGenerateId.Rows[0]["MBID"].ToString().Substring(0, 6) + (Convert.ToInt32(dtGenerateId.Rows[0]["MBID"].ToString().Substring(6, 4)) + i).ToString().PadLeft(4, '0');
                else
                    ID = "MB" + year + month + day + i.ToString().PadLeft(4, '0');
                sqb.AppendFormat(" INSERT INTO MOLD_BASE(MBID,CUID,WAREID,MAID,WEIGHT,MAKERID,DATE,MOLDNO,WNAME,REMARK) VALUES(");
                sqb.AppendFormat("'{0}',", ID);
                sqb.AppendFormat("'{0}',", CUID);
                sqb.AppendFormat("'{0}',", dtx.Rows[i]["F2"].ToString());
                sqb.AppendFormat("'{0}',", MAID);
                sqb.AppendFormat("'{0}',", dtx.Rows[i]["F4"].ToString());
                sqb.AppendFormat("'{0}',", LOGIN.EMID);
                sqb.AppendFormat("'{0}',", varDate);
                sqb.AppendFormat("'{0}',", dtx.Rows[i]["F6"].ToString());
                sqb.AppendFormat("'{0}',", dtx.Rows[i]["F5"].ToString());
                sqb.AppendFormat("'{0}'", dtx.Rows[i]["F7"].ToString());
                sqb.AppendFormat(");");
            }
            try
            {
                basec.getcoms(sqb.ToString());
                IFExecution_SUCCESS = true;
            }
            catch(Exception)
            {
                IFExecution_SUCCESS = false;
            }
            if (IFExecution_SUCCESS == true)
                hint.Text = "数据成功导入";
            sqb = new StringBuilder();
            sqb.Append(cMOLD_BASE.sql);
            sqb.Append(" WHERE B.CNAME LIKE '%" + comboBox1.Text + "%' ");
            sqb.Append(" AND A.WAREID LIKE '%" + textBox1.Text + "%'");
            sqb.Append(" AND C.MATERIAL LIKE '%" + comboBox2.Text + "%'");
            sqb.Append(" AND A.WEIGHT LIKE '%" + textBox2.Text + "%'");
            dt = bc.getdt(sqb.ToString());
            dataGridView1.DataSource = dt;
            bind2();
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


                try
                {


                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            IF_DOUBLE_CLICK = false;
            int rows = dataGridView1.CurrentCell.RowIndex;
            int columns = dataGridView1.CurrentCell.ColumnIndex;
            MOLD_BASET mOLD_BASET = new MOLD_BASET(this );
            mOLD_BASET.IDO = dataGridView1.Rows[rows].Cells["MBID"].Value.ToString();
            mOLD_BASET.Show();
        }

    }
}

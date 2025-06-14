using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Web;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data.SqlClient;
using XizheC;
using System.Collections;
using System.Xml;
using System.Configuration;

namespace CSPSS.BASE_INFO
{
    public partial class EMPLOYEE_INFO : Form
    {
        private string _EMID;
        public string EMID
        {
            set { _EMID = value; }
            get { return _EMID; }
        }
        private string _EMPLOYEE_ID;
        public string EMPLOYEE_ID
        {
            set { _EMPLOYEE_ID = value; }
            get { return _EMPLOYEE_ID; }
        }
        private string _ENAME;
        public string ENAME
        {
            set { _ENAME = value; }
            get { return _ENAME; }
        }
        private  string _DEPART;
        public  string DEPART
        {
            set { _DEPART = value; }
            get { return _DEPART; }
        }
        private  bool _IF_DOUBLE_CLICK;
        public  bool IF_DOUBLE_CLICK
        {
            set { _IF_DOUBLE_CLICK = value; }
            get { return _IF_DOUBLE_CLICK; }
        }
        DataTable dt = new DataTable();
        DataTable dt1 = new DataTable();
        StringBuilder sqb = new StringBuilder();
        private string _IDO;
        public string IDO
        {
            set { _IDO = value; }
            get { return _IDO; }

        }
        private string _ADD_OR_UPDATE;
        public string ADD_OR_UPDATE
        {
            set { _ADD_OR_UPDATE = value; }
            get { return _ADD_OR_UPDATE; }
        }

        private string _POSITION;
        public string POSITION
        {
            set { _POSITION = value; }
            get { return _POSITION; }
        }
        private string _GROUP;
        public string GROUP
        {
            set { _GROUP = value; }
            get { return _GROUP; }
        }
        private int _SELECT;
        public int SELECT
        {
            set { _SELECT = value; }
            get { return _SELECT; }
        }
        private bool _IFExecutionSUCCESS;
        public bool IFExecution_SUCCESS
        {
            set { _IFExecutionSUCCESS = value; }
            get { return _IFExecutionSUCCESS; }

        }
        basec bc = new basec();
        CEMPLOYEE_INFO cemployee_info = new CEMPLOYEE_INFO();

        protected int M_int_judge, i;
        protected int select;
        public EMPLOYEE_INFO()
        {
            InitializeComponent();
        }
     
 
        private void EMPLOYEE_INFO_Load(object sender, EventArgs e)
        {
         
            try
            {
                EMID = null;
                EMPLOYEE_ID = null;
                ENAME = null;
                DEPART = null;
                this.Icon = Resource1.xz_200X200;
                Bind();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }
        private void Bind()
        {
  
            textBox1.BackColor = Color.Yellow;
            textBox6.BackColor  = Color.Yellow;
            sqb = new StringBuilder();
          
 
            if (POSITION != null)
            {
                sqb.AppendFormat("POSITION={0}",POSITION );
            }
            else if (GROUP != null)
            {
                sqb.AppendFormat("DEPART={0}",GROUP);
            }
            else
            {
               sqb.AppendFormat("ALL={0}", '*');
            }
            if (POSITION != null || GROUP != null)
            {
              
                btnAdd.Visible = false;
                btnSave.Visible = false;
                btnDel.Visible = false;
                label11.Visible = false;
                label7.Visible = false;
                label8.Visible = false;
                btnToExcel.Visible = false;
            }
            //earch("http://" + RETURN_SERVER_IP_OR_DOMAIN() + "/webserver_ys/s_employeeinfo.aspx", sqb.ToString());
            string url = ConfigurationManager.AppSettings["api-url"].ToString () + "s_employeeinfo.aspx";
            search(url, sqb.ToString());
           
            //dataGridView1.DataSource = dt;
            dataGridView1.AllowUserToAddRows = false;
            textBox1.Focus();
            textBox2.BackColor = Color.Yellow;

            dgvStateControl();
            hint.Location = new Point(256, 136);
            hint.ForeColor = Color.Red;
            if (bc.GET_IFExecutionSUCCESS_HINT_INFO(IFExecution_SUCCESS) != "")
            {
                hint.Text = bc.GET_IFExecutionSUCCESS_HINT_INFO(IFExecution_SUCCESS);
            }
            else
            {
                hint.Text = "";
            }

            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox1.DataSource = bc.RETURN_ADD_EMPTY_COLUMN("DEPART", "DEPART");
            comboBox1.DisplayMember = "DEPART";

            comboBox2.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox2.DataSource = bc.RETURN_ADD_EMPTY_COLUMN("POSITION", "POSITION");
            comboBox2.DisplayMember = "POSITION";

        }
        #region RETURN_SERVER_IP_OR_DOMAIN
        public string RETURN_SERVER_IP_OR_DOMAIN()
        {
            string v = "";
            if (File.Exists(Resource1.Configuration))
            {
                //MessageBox.Show(GetSERVER_IP(System.IO.Path.GetFullPath("项目管理系统客户端.exe.config")));
                v = RETURN_APPOINT_UNTIL_CHAR(GetSERVER_IP(Resource1.Configuration), 8, '/');
            }
            else
            {
                MessageBox.Show("不存在指定的配置文件");
            }
            return v;
        }
        #endregion
        #region RETURN_APPOINT_UNTIL_CHAR
        public string RETURN_APPOINT_UNTIL_CHAR(string HAVE_NAME_STRING, int START, char C1)
        {
            string v = "";
            if (HAVE_NAME_STRING.Length > 0 && HAVE_NAME_STRING.Length >= START)
            {
                int q = Convert.ToInt32(C1);
                for (int i = START - 1; i < HAVE_NAME_STRING.Length; i++)
                {
                    int p = Convert.ToInt32(HAVE_NAME_STRING[i]);

                    if (p != q)
                    {
                        v = v + HAVE_NAME_STRING[i];
                    }
                    else
                    {
                        break;
                    }
                }
            }

            return v;
        }
        #endregion
        #region GetSERVER_IP
        public string GetSERVER_IP(string Dir)
        {
            //获取客户端应用程序及服务器端升级程序的最近一次更新版本
            string LastUpdateVersion = "";
            string AutoUpdaterFileName = Dir;
            if (!File.Exists(AutoUpdaterFileName))
                return LastUpdateVersion;
            //打开xml文件  
            FileStream myFile = new FileStream(AutoUpdaterFileName, FileMode.Open);
            //xml文件阅读器  
            XmlTextReader xml = new XmlTextReader(myFile);
            while (xml.Read())
            {
                if (xml.Name == "endpoint")
                {  //获取升级文档的最后一次更新版本
                    LastUpdateVersion = xml.GetAttribute("address");
                    break;
                }
            }
            xml.Close();
            myFile.Close();
            return LastUpdateVersion;
        }
        #endregion
        #region dgvStateControl
        private void dgvStateControl()
        {
            int i;
            dataGridView1.RowHeadersDefaultCellStyle.BackColor = Color.Lavender;
            int numCols1 = dataGridView1.Columns.Count;
            for (i = 0; i < numCols1; i++)
            {
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;/*自动调整DATAGRIDVIEW的列宽*/
                dataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                this.dataGridView1.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridView1.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                if (i == 1)
                {
                    dataGridView1.Columns[i].Width = 70;

                }
                else if (i == 6)
                {
                    dataGridView1.Columns[i].Width = 120;

                }
                else if (i == 4)
                {
                    dataGridView1.Columns[i].Width = 90;

                }
                else
                {
                    dataGridView1.Columns[i].Width = 60;

                }
            
                dataGridView1.EnableHeadersVisualStyles = false;
                dataGridView1.Columns[i].HeaderCell.Style.BackColor = Color.Lavender;

            }
            for (i = 0; i < dataGridView1.Columns.Count; i++)
            {
                dataGridView1.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridView1.Columns[i].DefaultCellStyle.BackColor = Color.OldLace;
                i = i + 1;
            }
            for (i = 0; i < dataGridView1.Columns.Count; i++)
            {
                dataGridView1.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridView1.Columns[i].ReadOnly = true;

            }
            dataGridView1.Columns["制单人"].Width = 70;
        }
        #endregion
    
        #region save
        private void save()
        {
            string year = DateTime.Now.ToString("yy");
            string month = DateTime.Now.ToString("MM");
            string day = DateTime.Now.ToString("dd");
            string varDate = DateTime.Now.ToString("yyy/MM/dd HH:mm:ss");
            string varMakerID = LOGIN.EMID;
            string GET_EMPLOYEE_ID = bc.getOnlyString("SELECT EMPLOYEE_ID FROM EMPLOYEEINFO WHERE EMID='"+IDO +"'");
            sqb = new StringBuilder();
            sqb.AppendFormat("LOGIN_EMID={0}",LOGIN .EMID );
            sqb.AppendFormat("&EMPLOYEE_ID={0}", textBox1.Text);
            sqb.AppendFormat("&IDO={0}", IDO );
            sqb.AppendFormat("&ENAME={0}", textBox2.Text);
            sqb.AppendFormat("&DEPART={0}", comboBox1 .Text );
            sqb.AppendFormat("&POSITION={0}", comboBox2 .Text );
            sqb.AppendFormat("&PHONE={0}", textBox3.Text);
            sqb.AppendFormat("&SAMPLE_CODE={0}", textBox6.Text);
            sqb.AppendFormat("&UPDATE={0}", '*');
            string url = ConfigurationManager.AppSettings["api-url"]  + "s_update_employeeinfo.aspx";
            JArray jar = bc.RETURN_JARRAY(url,sqb.ToString ());
            if (jar[0]["IFExecution_SUCCESS"].ToString() == "True")
            {
                IFExecution_SUCCESS = true;
                Bind();
            }
            else
            {
                IFExecution_SUCCESS = false;
                hint.Text = jar[0]["ErrowInfo"].ToString();
            }
        }
        #endregion
        #region juage()
        private bool juage()
        {
            bool b = false;
            if (string.IsNullOrEmpty (IDO))
            {
                b = true;
                hint.Text = "编号不能为空！";
            }
            else if (textBox1.Text == "")
            {
                b = true;
                hint.Text = "员工工号不能为空！";

            }
            else    if (textBox2.Text == "")
            {
                b = true;

                hint.Text = "姓名不能为空！";
             
            }
            else if (textBox6.Text == "")
            {
                b = true;

                hint.Text = "简码不能为空！";

            }
            else if (bc.checkphone(textBox3 .Text ) == false)
            {
                b = true;
                hint.Text = "电话号码只能输入数字！";

            }
            return b;

        }
        #endregion
        public void ClearText()
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            comboBox1.Text = "";
            comboBox2.Text = "";
            textBox6.Text = "";
        }
        #region only read

        public void PROJECT_INFO_USE()
        {
            dataGridView1.Enabled = true;
            select = 1;

        }
        public void SAMPLE_REAL_LIST()
        {
            dataGridView1.Enabled = true;
            select = 2;

        }
        public void SAMPLE_REAL_LIST_1920()
        {
            dataGridView1.Enabled = true;
            select = 3;
        }
        public void NOTICE_LIST_USE()
        {
            dataGridView1.Enabled = true;
            select = 4;
        }
        public void PN_PRODUCTION_INSTRUCTIONST_USE()
        {
            dataGridView1.Enabled = true;
            select = 5;
        }
        public void AUDIT_LIST_USE()
        {
            dataGridView1.Enabled = true;
            select = 6;
        }
        public void USER_INFO_USE()
        {
            dataGridView1.Enabled = true;
            select = 16;
        }
        public void CUSTOMERINFO_USE()
        {
            dataGridView1.Enabled = true;
            select = 17;
        }

        public void a3()
        {
            dataGridView1.Enabled = true;
            select = 19;

        }

        #endregion
 

        private void btnAdd_Click(object sender, EventArgs e)
        {

            add();
        }
        private void add()
        {
            string url = ConfigurationManager.AppSettings["api-url"]  + "s_employeeinfo.aspx";
            JArray jar = bc.RETURN_JARRAY(url, "GETID=*");
            IDO = jar[0]["IDO"].ToString();
            ClearText();
            textBox1.Focus();

        }
      

        private void btnSave_Click(object sender, EventArgs e)
        {
            
            if (juage())
            {

            }
            else
            {
                save();
                if (IFExecution_SUCCESS)
                {
                    add();
                }
                
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            sqb = new StringBuilder();

            sqb.AppendFormat("EMPLOYEE_ID={0}", textBox4 .Text );
            sqb.AppendFormat("&ENAME={0}", textBox5.Text);
            if (textBox4.Text == "" && textBox5.Text == "")
            {
                sqb.AppendFormat("&ALL={0}", '*');
            }
           
            //search("http://"+bc.RETURN_SERVER_IP_OR_DOMAIN()+"/webserver_ys/s_employeeinfo.aspx",sqb.ToString ());
           string url=ConfigurationManager.AppSettings["api-url"]+"s_employeeinfo.aspx";
           search(url, sqb.ToString());
           
        }
        private void b()
        {
           
        }
        private void search(string url,string parameter)
        {
            hint.Text = "";
            try
            {
                JArray jar = RETURN_JARRAY(url,parameter);
                dt=cemployee_info .emptydatatable_T ();
                if (jar.Count > 0)
                {
                    for (int i = 0; i < jar.Count; i++)
                    {
                        DataRow dr = dt.NewRow();
                        dr["序号"] = jar[i]["序号"].ToString();
                        dr["员工工号"] = jar[i]["员工工号"].ToString();
                        dr["员工姓名"] = jar[i]["员工姓名"].ToString();
                        dr["部门"] = jar[i]["部门"].ToString();
                        dr["职务"] = jar[i]["职务"].ToString();
                        dr["电话"] = jar[i]["电话"].ToString();
                        dr["简码"] = jar[i]["简码"].ToString();
                        dr["制单人"] = jar[i]["制单人"].ToString();
                        dr["制单日期"] = jar[i]["制单日期"].ToString();
                        dt.Rows.Add(dr);
                    }
                }
         
                if (dt.Rows .Count  > 0)
                {
                    dataGridView1.DataSource = dt;
                    dgvStateControl();
                }
                else
                {
                   
                    hint.Text = "没有找到相关信息！";
                    dataGridView1.DataSource = null;
                }
             
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Information);
           
            }
        }

        #region RETURN_JARRAY
        public JArray RETURN_JARRAY(string url, string parameter)
        {
            string urlPage = url;
            Stream outstream = null;
            Stream instream = null;
            StreamReader sr = null;
            HttpWebResponse response = null;
            HttpWebRequest request = null;
            JArray jar = new JArray();
            try
            {
                Encoding encoding = Encoding.GetEncoding("UTF-8");
                byte[] data = encoding.GetBytes(parameter);
                request = WebRequest.Create(urlPage) as HttpWebRequest;
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = data.Length;
                outstream = request.GetRequestStream();
                outstream.Write(data, 0, data.Length);
                outstream.Flush();
                outstream.Close();
                response = request.GetResponse() as HttpWebResponse;
                instream = response.GetResponseStream();
                sr = new StreamReader(instream, encoding);
                string a = sr.ReadToEnd();
                string b = "";
                // 方案1：直接搜索 JSON 起始标志
                int jsonStart = a.IndexOfAny(new[] { '[', '{' });
                if (jsonStart >= 0)
                {
                    int jsonEnd = a.LastIndexOfAny(new[] { ']', '}' });
                    if (jsonEnd > jsonStart)
                    {
                        b = a.Substring(jsonStart, jsonEnd - jsonStart + 1);
                    }
                }
                jar = JArray.Parse(b);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return jar;
        }
        #endregion
        private void btnDel_Click(object sender, EventArgs e)
        {
            string id = dt.Rows[dataGridView1.CurrentCell.RowIndex]["员工工号"].ToString();
            IDO = bc.getOnlyString("SELECT EMID FROM EMPLOYEEINFO WHERE EMPLOYEE_ID='" + id  + "'");
            if (bc.JuageIfAllowDeleteEMID(IDO))
            {
                hint.Text = bc.ErrowInfo;
            }
            else
            {
                IFExecution_SUCCESS = false;
                string strSql = "DELETE FROM EMPLOYEEINFO WHERE EMPLOYEE_ID='" + id + "'";
                basec.getcoms(strSql);

                Bind();
                ClearText();
            }
          
            try
            {
            
            }
            catch (Exception)
            {


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

                dataGridView1.Focus();

                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
        #endregion

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            sqb = new StringBuilder();
            sqb.AppendFormat("EMPLOYEE_ID={0}", textBox4.Text);
            sqb.AppendFormat("&ENAME={0}", textBox5.Text);
            if (textBox4.Text == "" && textBox5.Text == "")
            {
                sqb.AppendFormat("&ALL={0}", '*');
            }
            string url = ConfigurationManager.AppSettings["api-url"] + "s_employeeinfo.aspx";
            search(url, sqb.ToString());
       
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            sqb = new StringBuilder();
            sqb.AppendFormat("EMPLOYEE_ID={0}", textBox4.Text);
            sqb.AppendFormat("&ENAME={0}", textBox5.Text);
            if (textBox4.Text == "" && textBox5.Text == "")
            {
                sqb.AppendFormat("&ALL={0}", '*');
            }

            search(ConfigurationManager.AppSettings["api-url"]  + "s_employeeinfo.aspx", sqb.ToString());
        
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

            if (SELECT  != 0 || select !=0)
            {
               int indexNumber = dataGridView1.CurrentCell.RowIndex;
               EMPLOYEE_ID = dt.Rows[dataGridView1.CurrentCell.RowIndex]["员工工号"].ToString();
               ENAME = dt.Rows[dataGridView1.CurrentCell.RowIndex]["员工姓名"].ToString();
               DEPART  = dt.Rows[dataGridView1.CurrentCell.RowIndex]["部门"].ToString();

                if (select == 0)
                {
                    //CSPSS.SellManage.FrmOrders.inputgetOEName[0] = inputarry[0]; 
                }
     
                else if (select == 16)
                {
                    CSPSS.USER_MANAGE.USER_INFO.EMID = bc.getOnlyString("SELECT EMID FROM EMPLOYEEINFO WHERE EMPLOYEE_ID='" + EMPLOYEE_ID + "'");
                    USER_MANAGE.USER_INFO.EMPLOYEE_ID = EMPLOYEE_ID;
                    CSPSS.USER_MANAGE.USER_INFO.ENAME = ENAME;
                    CSPSS.USER_MANAGE.USER_INFO.DEPART = DEPART;
                    CSPSS.USER_MANAGE.USER_INFO.IF_DOUBLE_CLICK = true;
                }
                else if (select == 17)
                {
                    BASE_INFO.CUSTOMER_INFOT.EMPLOYEE_ID = EMPLOYEE_ID;
                    BASE_INFO.CUSTOMER_INFOT.ENAME = ENAME;
                    BASE_INFO.CUSTOMER_INFOT.IF_DOUBLE_CLICK = true;
                }
                this.Close();

            }
            else
            {
                string v1 = dt.Rows[dataGridView1.CurrentCell.RowIndex]["员工工号"].ToString();
                IDO = bc.getOnlyString("SELECT EMID FROM EMPLOYEEINFO WHERE EMPLOYEE_ID='" + v1 + "'");
                textBox1.Text = Convert.ToString(dataGridView1[1, dataGridView1.CurrentCell.RowIndex].Value).Trim();
                textBox2.Text = Convert.ToString(dataGridView1[2, dataGridView1.CurrentCell.RowIndex].Value).Trim();
                comboBox1.Text = Convert.ToString(dataGridView1[3, dataGridView1.CurrentCell.RowIndex].Value).Trim();
                comboBox2.Text = Convert.ToString(dataGridView1[4, dataGridView1.CurrentCell.RowIndex].Value).Trim();
                textBox3.Text = Convert.ToString(dataGridView1[5, dataGridView1.CurrentCell.RowIndex].Value).Trim();
                textBox6.Text = Convert.ToString(dataGridView1[6, dataGridView1.CurrentCell.RowIndex].Value).Trim();
            }
        }

        private void btnToExcel_Click(object sender, EventArgs e)
        {
            if (dt.Rows.Count > 0)
            {

                bc.dgvtoExcel(dataGridView1,this.Text );
            }
            else
            {
                MessageBox.Show("没有数据可导出！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
    class EMLOYEEINFO_O
    {
        public EMLOYEEINFO_O()
        {


        }
        private string _IDO;
        public string IDO
        {
            set { _IDO = value; }
            get { return _IDO; }

        }
        private string _NO;
        public string NO
        {
            set { _NO = value; }
            get { return _NO; }

        }
        private string _EMID;
        public string EMID
        {
            set { _EMID = value; }
            get { return _EMID; }

        }
        private string _EMPLOYEE_ID;
        public string EMPLOYEE_ID
        {
            set { _EMPLOYEE_ID = value; }
            get { return _EMPLOYEE_ID; }

        }
        private string _ENAME;
        public string ENAME
        {
            set { _ENAME = value; }
            get { return _ENAME; }

        }
        private string _POSITION;
        public string POSITION
        {
            set { _POSITION = value; }
            get { return _POSITION; }

        }
        private string _DEPART;
        public string DEPART
        {
            set { _DEPART = value; }
            get { return _DEPART; }

        }
        private string _PHONE;
        public string PHONE
        {
            set { _PHONE = value; }
            get { return _PHONE; }

        }
        private string _SAMPLE_CODE;
        public string SAMPLE_CODE
        {
            set { _SAMPLE_CODE = value; }
            get { return _SAMPLE_CODE; }

        }
        private string _MAKER;
        public string MAKER
        {
            set { _MAKER = value; }
            get { return _MAKER; }

        }
        private string _DATE;
        public string DATE
        {
            set { _DATE = value; }
            get { return _DATE; }

        }
        private string _ErrowInfo;
        public string ErrowInfo
        {

            set { _ErrowInfo = value; }
            get { return _ErrowInfo; }

        }
        private bool _IFExecutionSUCCESS;
        public bool IFExecution_SUCCESS
        {
            set { _IFExecutionSUCCESS = value; }
            get { return _IFExecutionSUCCESS; }

        }


    }  
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using XizheC;


namespace CSPSS.SELL_MANAGE
{
    public partial class ORDERT : Form
    {

        private  string _CUID;
        public  string CUID
        {
            set { _CUID = value; }
            get { return _CUID; }
        }
        private  string _MATERIAL;
        public  string MATERIAL
        {
            set { _MATERIAL = value; }
            get { return _MATERIAL; }
        }
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
        private bool _IF_IMPORT_SUCCESS;
        public bool IF_IMPORT_SUCCESS
        {
            set { _IF_IMPORT_SUCCESS = value; }
            get { return _IF_IMPORT_SUCCESS; }
        }
        private string _IDO;
        public string IDO
        {
            set { _IDO = value; }
            get { return _IDO; }
        }
        private string _MGKEY;
        public string MGKEY
        {
            set { _MGKEY = value; }
            get { return _MGKEY; }
        }
        private string _COUNT;
        public string COUNT
        {
            set { _COUNT = value; }
            get { return _COUNT; }
        }
        private string _ADD_OR_UPDATE;
        public string ADD_OR_UPDATE
        {
            set { _ADD_OR_UPDATE = value; }
            get { return _ADD_OR_UPDATE; }
        }
        basec bc = new basec();
        CCUSTOMER_INFO ccustomer_info = new CCUSTOMER_INFO();
        CMISC_STORAGE cmisc_storage = new CMISC_STORAGE();
        CORDER corder = new CORDER();
        DataTable dt = new DataTable();
        DataTable dtx = new DataTable();
        string varDate = DateTime.Now.ToString("yyy/MM/dd").Replace("-", "/");
        ORDER F1= new ORDER();
        CFileInfo cfileinfo = new CFileInfo();
        CBOM cBOM = new CBOM();
        protected int i, j;
        public ORDERT()
        {
            InitializeComponent();
        }
        public ORDERT(ORDER FRM)
        {
            InitializeComponent();
            F1 = FRM;
        }
        private void ORDERT_Load(object sender, EventArgs e)
        {
            this.Icon = Resource1.xz_200X200;
            textBox1.Text = IDO;
            bind();
            try
            {
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            }
        }
        #region bind
        private void bind()
        {
            comboBox1.Focus();
            comboBox1.BackColor = CCOLOR.YELLOW;
            dataGridView1.EditMode = DataGridViewEditMode.EditOnEnter;
            dataGridView1.ContextMenuStrip = contextMenuStrip1;
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
            dtx = basec.getdts(corder.sql + " where 订单号='" + textBox1.Text + "' ORDER BY  订单号 ASC ");
            if (dtx.Rows.Count > 0)
            {
                comboBox1.Text= dtx.Rows[0]["客户编号"].ToString();
                textBox2.Text = dtx.Rows[0]["客户名称"].ToString();
                dateTimePicker1.Text = dtx.Rows[0]["下单日期"].ToString();
                textBox3.Text = dtx.Rows[0]["联系人"].ToString();
                textBox4.Text = dtx.Rows[0]["公司地址"].ToString();
                textBox5.Text = dtx.Rows[0]["联系电话"].ToString();
                //textBox6.Text = dtx.Rows[0]["客户订单号"].ToString();
                dt = corder.GetTableInfo();
                foreach (DataRow dr1 in dtx.Rows)
                {
                    DataRow dr = dt.NewRow();
                    dr["项次"] = dr1["项次"].ToString();
                    dr["型号"] = dr1["型号"].ToString();
                    dr["模具编号"] = dr1["模具编号"].ToString();
                    dr["品名"] = dr1["品名"].ToString();
                    dr["材料"] = dr1["材料"].ToString();
                    dr["数量"] = dr1["数量"].ToString();
                    dr["单位"] = dr1["单位"].ToString();
                    dr["订单交期"] = dr1["订单交期"].ToString();
                    dr["客户订单号"] = dr1["客户订单号"].ToString();
                    dt.Rows.Add(dr);
                }
                if (dt.Rows.Count > 0 && dt.Rows.Count < 6)
                {
                    int n = 6 - dt.Rows.Count;
                    for (int i = 0; i < n; i++)
                    {
                        DataRow dr = dt.NewRow();
                        int b1 = Convert.ToInt32(dt.Rows[dt.Rows.Count - 1]["项次"].ToString());
                        dr["项次"] = Convert.ToString(b1 + 1);
                        dr["订单交期"] = varDate;
                        dt.Rows.Add(dr);
                    }
                }
               
            }
            else
            {
                dt = total();
            }
            dataGridView1.DataSource = dt;
            dgvStateControl();
        }
        #endregion
        #region total1
        private DataTable total()
        {
            DataTable dtt2 = corder.GetTableInfo();
            for (i = 1; i <= 6; i++)
            {
                DataRow dr = dtt2.NewRow();
                dr["项次"] = i;
                dr["订单交期"] = varDate;
                dtt2.Rows.Add(dr);
            }
            return dtt2;
        }
        #endregion
        #region dgvStateControl
        private void dgvStateControl()
        {
            int i;
            dataGridView1.RowHeadersDefaultCellStyle.BackColor = Color.Lavender;
            int numCols1 = dataGridView1.Columns.Count;
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
            dataGridView1.Columns["项次"].ReadOnly = true;
            dataGridView1.Columns["项次"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView1.Columns["型号"].DefaultCellStyle.BackColor = CCOLOR.CUSTOMER_YELLOW;
            dataGridView1.Columns["材料"].DefaultCellStyle.BackColor = CCOLOR.CUSTOMER_YELLOW;
            dataGridView1.Columns["数量"].DefaultCellStyle.BackColor = CCOLOR.CUSTOMER_YELLOW;
            dataGridView1.Columns["订单交期"].DefaultCellStyle.BackColor = CCOLOR.CUSTOMER_YELLOW;
        }
        #endregion
        #region save

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
        #region dgvcellclick
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            IF_DOUBLE_CLICK = false;
            int rows = dataGridView1.CurrentCell.RowIndex;
            int columns = dataGridView1.CurrentCell.ColumnIndex;
            if (dataGridView1.Columns[columns].DataPropertyName.ToString() == "材料")
            {
             
                BASE_INFO.MATERIAL FRM = new BASE_INFO.MATERIAL();
                FRM.ORDER_USE();
                FRM.ShowDialog();
                if (FRM.MATERIAL_VALUE !="")
                {
                    dt.Rows[rows]["材料"] = FRM.MATERIAL_VALUE;
                }
                dataGridView1.CurrentCell = dataGridView1["数量", rows];
            }

        }
        #endregion

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
                        dr["订单交期"] = varDate;
                        dt.Rows.Add(dr);
                    }
                }
            }
            catch (Exception)
            {
                //MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);

            }

        }
        private void dataGridView1_DataSourceChanged(object sender, EventArgs e)
        {
           
        }

        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            

        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);

            }
        }

        private void add()
        {
            ClearText();
            IDO = corder.GETID();
            textBox1.Text = IDO;
            bind();
            comboBox1.Focus();
        }
        public void ClearText()
        {
            comboBox1.Text = "";
            textBox2.Text = "";
            dateTimePicker1.Text = DateTime.Now.ToString("yyyy/MM/dd").Replace("-", "/");
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            textBox6.Text = "";
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            add();
        }
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
  
        private void comboBox1_DropDown(object sender, EventArgs e)
        {
            IF_DOUBLE_CLICK = false;
            BASE_INFO.CUSTOMER_INFO FRM = new CSPSS.BASE_INFO.CUSTOMER_INFO();
            FRM.ORDER_USE();
            FRM.ShowDialog();
            this.comboBox1.IntegralHeight = false;//使组合框不调整大小以显示其所有项
            this.comboBox1.DroppedDown = false;//使组合框不显示其下拉部分
            this.comboBox1.IntegralHeight = true;//恢复默认值
            CUID = FRM.CUID;
            dtx = bc.getdt(ccustomer_info.sql + " WHERE A.CUID='" + CUID + "'");
            if (dtx.Rows.Count > 0)
            {
                comboBox1.Text = dtx.Rows[0]["客户编号"].ToString();
                textBox2.Text = dtx.Rows[0]["客户名称"].ToString();
                textBox3.Text = dtx.Rows[0]["联系人"].ToString();
                textBox4.Text = dtx.Rows[0]["公司地址"].ToString();
                textBox5.Text = dtx.Rows[0]["联系电话"].ToString();
            }

            textBox6.Focus();
        }
        #region juage
        private bool juage()
        {
            bool b = false;
            if (IDO == "")
            {
                hint.Text = "编号不能为空！";
                b = true;
            }
            else if(bc.exists (cmisc_storage .sql +string .Format (" WHERE D.ORID='{0}'",IDO )))
            {
                hint.Text = string.Format("订单号 {0} 已经有入库记录不允许修改", IDO);
                b = true;
            }
            else if (comboBox1.Text == "")
            {
                hint.Text = "客户编号不能为空！";
                b = true;
            }
            else if (!bc.exists(ccustomer_info .sql  + " WHERE A.CUID='" + comboBox1.Text + "'"))
            {
                hint.Text = "客户编号不存在系统！";
                b = true;
            }
            else if (JUAGE_WNAME_IF_ABOVE_ONE(dataGridView1, "型号") == false)
            {
                hint.Text = string.Format("至少有一项型号才能保存");
                b = true;
            }
            else if (juage2())
            {
                b = true;
            }
            return b;
        }
        #endregion
        #region juage2()
        private bool juage2()
        {
            bool b = false;
            for (i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (dataGridView1["型号", i].FormattedValue.ToString() == "")
                {
                    continue;
                }
                /*else if (dataGridView1["型号", i].FormattedValue.ToString() == "")
                {
                    hint.Text = string.Format("项次 {0} 型号不能为空", dataGridView1["项次", i].FormattedValue.ToString());
                    b = true;
                    break;
                }*/
                else   if (dataGridView1["材料", i].FormattedValue.ToString() == "")
                {
                    hint.Text = string.Format("项次 {0} 材料不能为空", dataGridView1["项次", i].FormattedValue.ToString());
                    b = true;
                    break;
                }
                else if (!bc.exists(@"
SELECT * FROM MATERIAL 
WHERE MATERIAL='" + dataGridView1["材料", i].FormattedValue.ToString() + "'"))
                {
                    hint.Text = string.Format("项次 {0} 材料不存在系统", dataGridView1["项次", i].FormattedValue.ToString());
                    b = true;
                    break;
                }
                else if (dataGridView1["数量", i].FormattedValue.ToString() == "")
                {
                    hint.Text = string.Format("项次 {0} 数量不能为空", dataGridView1["项次", i].FormattedValue.ToString());
                    b = true;
                    break;
                }
        
                else if (bc.yesno(dataGridView1["数量", i].FormattedValue.ToString()) == 0)
                {
                    hint.Text = string.Format("项次 {0} 数量只能输入数字", dataGridView1["项次", i].FormattedValue.ToString());
                    b = true;
                    break;
                }
                else if (dataGridView1["数量", i].FormattedValue.ToString() == "0")
                {
                    hint.Text = string.Format("项次 {0} 数量不能为0", dataGridView1["项次", i].FormattedValue.ToString());
                    b = true;
                    break;
                }
                else if (dataGridView1["订单交期", i].FormattedValue.ToString() == "")
                {
                    hint.Text = string.Format("项次 {0} 订单交期不能为空", dataGridView1["项次", i].FormattedValue.ToString());
                    b = true;
                    break;
                }
                DateTime temp = DateTime.MinValue;
                if (!DateTime.TryParse(dataGridView1["订单交期", i].FormattedValue.ToString(), out temp))
                {
                    hint.Text = string.Format("第 {0} 行订单交期 {1} 格式不正确 需为格式yyyy/MM/dd", dataGridView1["项次", i].FormattedValue.ToString(), dataGridView1["订单交期", i].FormattedValue.ToString());
                    b = true;
                    break;
                }
            }
            return b;
        }
        #endregion
        #region JUAGE_WNAME_IF_ABOVE_ONE
        private bool JUAGE_WNAME_IF_ABOVE_ONE(DataGridView dgv, string COLUMN_NAME)
        {
            bool b = false;
            for (int i = 0; i < dgv.Rows.Count; i++)
            {
                if (dgv[COLUMN_NAME, i].FormattedValue.ToString() != "")
                {
                    b = true;
                }
            }
            return b;
        }
        #endregion
        private void btnSave_Click(object sender, EventArgs e)
        {
            IFExecution_SUCCESS = false;
            hint.Text = "";
            btnSave.Focus();
            if (juage())
            {

            }
            else
            {
                save();
            }
            try
            {
            
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            }
        }
        #region save
        private void save()
        {

            btnSave.Focus();
            corder.MAKERID = LOGIN.EMID;
            corder.ORID = IDO;
            corder.CUID = comboBox1.Text;
            corder.ORDER_DATE = dateTimePicker1.Text;
            corder.CUSTOMER_ORID = textBox6.Text;
            corder.save(dataGridView1);
            IFExecution_SUCCESS = corder.IFExecution_SUCCESS;
            hint.Text = corder.ErrowInfo;
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
                F1.bind();
            }
        }
        #endregion
        private void btnDel_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("确定要删除该条凭证吗？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    if (bc.exists(cmisc_storage.sql + " WHERE D.ORID='"+textBox1.Text +"'"))
                    {
                        hint.Text = string.Format("订单号 {0} 已经有入库记录不允许删除", textBox1.Text);
                    }
                    else
                    {
                        basec.getcoms("DELETE ORDER_MST WHERE ORID='" + textBox1.Text + "'");
                        basec.getcoms("DELETE ORDER_DET WHERE ORID='" + textBox1.Text + "'");
                        bind();
                        ClearText();
                        textBox1.Text = "";
                        F1.bind();
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);

            }
        }

        private void dataGridView1_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
       
        }

        private void PictureBox1_Click(object sender, EventArgs e)
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
                if(textBox2.Text =="")
                {
                    hint.Text = "客户名称不能为空";//因为要根据客户名称与材料带出模具编号，所以要求客户名称不能为空
                }
                else if (juage(dtx))
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

            dt = cBOM.GetTableInfo();
            int j = 1;
            for (i = 1; i < dtx.Rows.Count; i++)
            {
                DataRow dr = dt.NewRow();
                dr["项次"] = Convert.ToString(j);
                dr["型号"] = dtx.Rows[i]["F2"].ToString();
                dr["模具编号"] = bc.getOnlyString(@"select moldno from mold_base a 
inner join customerinfo_mst b on a.cuid=b.cuid inner join material c on a.maid=c.maid 
where cname='"+textBox2.Text +"' and c.material='"+ dtx.Rows[i]["F4"].ToString() + "'");
                dr["品名"] = dtx.Rows[i]["F3"].ToString();
                dr["材料"] = dtx.Rows[i]["F4"].ToString();
                dr["数量"] = Convert.ToDouble(dtx.Rows[i]["F5"].ToString());
                dr["单位"] = dtx.Rows[i]["F6"].ToString();
                dr["订单交期"] = dtx.Rows[i]["F7"].ToString();
                dr["客户订单号"] = dtx.Rows[i]["F8"].ToString();
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
        protected DataTable emptydatatable()
        {
            DataTable dtt = new DataTable();
            dtt.Columns.Add("项次", typeof(string));
            dtt.Columns.Add("型号", typeof(string));
            dtt.Columns.Add("元件品名", typeof(string));
            dtt.Columns.Add("规格", typeof(string));
            return dtt;
        }

        private void TSMI_Click(object sender, EventArgs e)
        {
            dt.Rows.RemoveAt(dataGridView1.CurrentCell.RowIndex);
            //重新更新项次顺序
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt.Rows[i]["项次"] = i + 1;
            }
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

    }
}

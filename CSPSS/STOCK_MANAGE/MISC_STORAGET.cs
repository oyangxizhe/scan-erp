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


namespace CSPSS.STOCK_MANAGE
{
    public partial class MISC_STORAGET : Form
    {

        private string _ORKEY;
        public string ORKEY
        {
            set { _ORKEY = value; }
            get { return _ORKEY; }
        }
        private static string _MATERIAL;
        public static string MATERIAL
        {
            set { _MATERIAL = value; }
            get { return _MATERIAL; }
        }
        private string _WAREID;
        public string WAREID
        {
            set { _WAREID = value; }
            get { return _WAREID; }
        }
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
        private string _BARCODE;
        public string BARCODE
        {
            set { _BARCODE = value; }
            get { return _BARCODE; }
        }
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

        basec bc = new basec();
        CCUSTOMER_INFO ccustomer_info = new CCUSTOMER_INFO();
        CORDER corder = new CORDER();
        CMISC_STORAGE cMISC_STORAGE = new CMISC_STORAGE();
        DataTable dt = new DataTable();
        DataTable dtx = new DataTable();
        string varDate = DateTime.Now.ToString("yyy/MM/dd").Replace("-", "/");
        MISC_STORAGE F1= new MISC_STORAGE();
        protected int i, j;
        public MISC_STORAGET()
        {
            InitializeComponent();
        }
        public MISC_STORAGET(MISC_STORAGE FRM)
        {
            InitializeComponent();
            F1 = FRM;
        }
        private void MISC_STORAGET_Load(object sender, EventArgs e)
        {
            label9.Text = "（说明：批号有销货的不允许整笔单据删除，批号没有销货记录，则可以鼠标右击选中该批号单项删除）";
            label9.ForeColor = CCOLOR.lylf1;
             this.Icon = Resource1.xz_200X200;
            textBox1.Text = IDO;
            label2.Text = "";
            textBox3.Font = new Font("黑体", 45, FontStyle.Regular);
            textBox3.BackColor = CCOLOR.lylfnp;
            textBox3.ForeColor = Color.White;
            textBox3.Focus();
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
            comboBox1.Text = LOGIN.EMPLOYEE_ID;
            label2.Text = LOGIN.ENAME;
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
            dtx = basec.getdts(cMISC_STORAGE.sql + " where A.MGID='" + textBox1.Text + "' ORDER BY  A.MGKEY ASC ");
            if (dtx.Rows.Count > 0)
            {
              
                dateTimePicker1.Text = dtx.Rows[0]["入库日期"].ToString();
                comboBox1.Text = dtx.Rows[0]["入库员工号"].ToString();
                label2.Text  = dtx.Rows[0]["入库员"].ToString();
                textBox2.Text = dtx.Rows[0]["客户名称"].ToString();
                textBox4.Text = dtx.Rows[0]["下单日期"].ToString();
                dt = cMISC_STORAGE.GetTableInfo();
                foreach (DataRow dr1 in dtx.Rows)
                {
                    DataRow dr = dt.NewRow();
                    dr["项次"] = dr1["项次"].ToString();
                    dr["型号"] = dr1["型号"].ToString();
                    dr["品名"] = dr1["品名"].ToString();
                    dr["材料"] = dr1["材料"].ToString();
                    dr["数量"] = dr1["数量"].ToString();
                    dr["单位"] = dr1["单位"].ToString();
                    dr["批号"] = dr1["批号"].ToString();
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
            DataTable dtt2 = cMISC_STORAGE.GetTableInfo();
            for (i = 1; i <= 6; i++)
            {
                DataRow dr = dtt2.NewRow();
                dr["项次"] = i;
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
              !((System.Windows.Forms.TextBox)ActiveControl).AcceptsReturn && ActiveControl.TabIndex != 5)
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
                FRM.SELECT = 1;
                FRM.ShowDialog();
                if (FRM.MATERIAL_VALUE != "")
                {
                    dt.Rows[rows]["材料"] = FRM.MATERIAL_VALUE;
                    dataGridView1.CurrentCell = dataGridView1["数量", rows];
                }
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
            IDO = cMISC_STORAGE.GETID();
            textBox1.Text = IDO;
            bind();
          
        }
        public void ClearText()
        {

            dateTimePicker1.Text = DateTime.Now.ToString("yyyy/MM/dd").Replace("-", "/");
            comboBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            add();
        }
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
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
            else if (JUAGE_WNAME_IF_ABOVE_ONE(dataGridView1, "型号") == false)
            {
                hint.Text = string.Format("至少有一项型号才能保存");
                b = true;
            }
            else if (cMISC_STORAGE.JUAGE_CURRENT_STORAGECOUNT_IF_LESSTHAN_DELETE_COUNT(textBox1.Text))
            {
                b = true;
                hint.Text = cMISC_STORAGE.ErrowInfo;
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
                    
                }
                else   if (dataGridView1["型号", i].FormattedValue.ToString() == "")
                {
                    hint.Text = string.Format("项次 {0} 型号不能为空", dataGridView1["项次", i].FormattedValue.ToString());
                    b = true;
                    break;
                }
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
                else if (dataGridView1["批号", i].FormattedValue.ToString() == "")
                {
                    hint.Text = string.Format("项次 {0} 批号不能为空", dataGridView1["项次", i].FormattedValue.ToString());
                    b = true;
                    break;
                }
                else if (!bc.exists(@"
SELECT * FROM ORDER_BARCODE
WHERE BARCODE='" + dataGridView1["批号", i].FormattedValue.ToString() + "'"))
                {
                    hint.Text = string.Format("项次 {0} 批号不存在系统", dataGridView1["项次", i].FormattedValue.ToString());
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

           
            IFExecution_SUCCESS = cMISC_STORAGE.IFExecution_SUCCESS;
            hint.Text = cMISC_STORAGE.ErrowInfo;
            cMISC_STORAGE.MGID = IDO;
            cMISC_STORAGE.GODE_DATE = dateTimePicker1.Text;
            cMISC_STORAGE.GODE_MAKERID = bc.getOnlyString("SELECT EMID FROM EMPLOYEEINFO WHERE EMPLOYEE_ID='" + comboBox1.Text + "'");
            cMISC_STORAGE.MAKERID = "";
            cMISC_STORAGE.REMARK = "";
            cMISC_STORAGE.save(dt, true);
            IFExecution_SUCCESS = cMISC_STORAGE.IFExecution_SUCCESS;
            F1.bind();
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
              
          
            }
        }
        #endregion
        private void btnDel_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("确定要删除该条凭证吗？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    if (cMISC_STORAGE .JUAGE_CURRENT_STORAGECOUNT_IF_LESSTHAN_DELETE_COUNT (textBox1 .Text ))
                    {
                        hint.Text = cMISC_STORAGE.ErrowInfo;
                    }
                    else
                    {
                        basec.getcoms("DELETE MISC_GODE_MST WHERE MGID='" + textBox1.Text + "'");
                        basec.getcoms("DELETE MISC_GODE_DET WHERE MGID='" + textBox1.Text + "'");
                        basec.getcoms("DELETE GODE WHERE GODEID='" + textBox1.Text + "'");
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

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void comboBox1_DropDown(object sender, EventArgs e)
        {
            IF_DOUBLE_CLICK = false;
            BASE_INFO.EMPLOYEE_INFO FRM = new CSPSS.BASE_INFO.EMPLOYEE_INFO();
            FRM.SELECT = 1;
            FRM.ShowDialog();
            this.comboBox1.IntegralHeight = false;//使组合框不调整大小以显示其所有项
            this.comboBox1.DroppedDown = false;//使组合框不显示其下拉部分
            this.comboBox1.IntegralHeight = true;//恢复默认值
            if (FRM.EMPLOYEE_ID != null)
            {
                comboBox1.Text = FRM.EMPLOYEE_ID;
                label2.Text = FRM.ENAME;
            }
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                actioin();
            }
        }
        private int yesno(string vars)
        {
            int k = 1;
            int i;
            for (i = 0; i < vars.Length; i++)
            {
                int p = Convert.ToInt32(vars[i]);
                if (p >= 48 && p <= 57 || p >= 65 && p <= 90 || p >= 97 && p <= 122)
                {
                    k = 1;
                }
                else
                {
                    k = 0; break;
                }

            }
            return k;
        }
        private void actioin()
        {
            
            ORKEY = "";
            DataTable dtt = bc.getdt("SELECT * FROM ORDER_BARCODE WHERE BARCODE='" + textBox3.Text.Trim() + "'");
            if (dtt.Rows.Count > 0)
            {

                ORKEY = dtt.Rows[0]["ORKEY"].ToString();
            }
            else
            {
                MessageBox.Show("条码："+textBox3.Text.Trim() + " 不存在系统", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox3.Text = "";
            }
            if (yesno(textBox3.Text.Trim()) == 0)
            {
                MessageBox.Show("条码："+textBox3.Text.Trim()+" 输入的字符不合法", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox3.Text = "";
            }
            else  if (bc.exists("SELECT * FROM GODE WHERE BATCHID='" + textBox3 .Text .Trim () + "'"))
            {
                MessageBox.Show(string.Format("条码：{0} 已经存在入库记录", textBox3.Text.Trim ()), "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox3.Text = "";

            }
            else
            {
                
                dtx = bc.getdt(corder.sql + string.Format(" WHERE 索引='{0}'", ORKEY));
                if (dtx.Rows.Count > 0)
                {
                    textBox2.Text = dtx.Rows[0]["客户名称"].ToString();
                    textBox4.Text = dtx.Rows[0]["下单日期"].ToString();
                    cMISC_STORAGE.MGID = IDO;
                    cMISC_STORAGE.WAREID = dtx.Rows[0]["型号"].ToString();
                    cMISC_STORAGE.MGCOUNT = dtx.Rows[0]["数量"].ToString();
                    cMISC_STORAGE.SKU = dtx.Rows[0]["单位"].ToString();
                    cMISC_STORAGE.BARCODE = textBox3.Text.Trim();
                    cMISC_STORAGE.GODE_DATE = dateTimePicker1.Text;
                    cMISC_STORAGE.GODE_MAKERID = bc.getOnlyString("SELECT EMID FROM EMPLOYEEINFO WHERE EMPLOYEE_ID='" + comboBox1.Text + "'");
                    cMISC_STORAGE.MAKERID = "";
                    cMISC_STORAGE.REMARK = "";
                    cMISC_STORAGE.ORKEY = ORKEY;
                    cMISC_STORAGE.save_BARCODE();
                    textBox3.Text = "";
                    bind();
                }
            }
            try
            {
              

     
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            }

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
           
        }

        private void TSMI_Click(object sender, EventArgs e)
        {
         
            int i = dataGridView1.CurrentCell.RowIndex;
            if (bc.exists("SELECT * FROM MATERE WHERE BATCHID='" + dt.Rows[i]["批号"].ToString() + "'"))
            {
                hint.Text = string.Format("此批号：{0} 已经有销货记录，不允许删除", dt.Rows[i]["批号"].ToString());
            }
            else
            {
                if (bc.juageOne("SELECT * FROM MISC_GODE_DET WHERE MGID='" + textBox1.Text + "'"))
                {
                    basec.getcoms("DELETE MISC_GODE_MST WHERE MGID='" + textBox1.Text + "'");
                    basec.getcoms("DELETE MISC_GODE_DET WHERE MGID='" + textBox1.Text + "'");
                    basec.getcoms("DELETE GODE WHERE GODEID='" + textBox1.Text + "'");
                    bind();
                }
                else
                {
                    basec.getcoms("DELETE MISC_GODE_DET WHERE MGKEY=(SELECT GEKEY FROM Gode WHERE BatchID='" + dt.Rows[i]["批号"].ToString() + "')");
                    basec.getcoms("DELETE GODE WHERE BATCHID='" + dt.Rows[i]["批号"].ToString() + "'");
                    bind();
                }
            }
        }
    }
}

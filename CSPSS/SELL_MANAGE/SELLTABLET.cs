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
    public partial class SELLTABLET : Form
    {

        basec bc = new basec();
        CCUSTOMER_INFO ccustomer_info = new CCUSTOMER_INFO();
        CMISC_STORAGE cmisc_storage = new CMISC_STORAGE();
        CSELLTABLE cselltable = new CSELLTABLE();
        DataTable dt = new DataTable();
        DataTable dt1 = new DataTable();
        DataTable dtx = new DataTable();
        string varDate = DateTime.Now.ToString("yyy/MM/dd").Replace("-", "/");
        SELLTABLE F1= new SELLTABLE();
        CORDER corder = new CORDER();
        protected int i, j;
        #region nature
        private  string _CUID;
        public string CUID
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
        private string _SKU;
        public string SKU
        {
            set { _SKU = value; }
            get { return _SKU; }
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
        private string _WEIGHT;
        public string WEIGHT
        {
            set { _WEIGHT = value; }
            get { return _WEIGHT; }
        }
        private string _ORID;
        public string ORID
        {
            set { _ORID = value; }
            get { return _ORID; }
        }
        private string _ORKEY;
        public string ORKEY
        {
            set { _ORKEY = value; }
            get { return _ORKEY; }
        }
        private string _NOSECOUNT;
        public string NOSECOUNT
        {
            set { _NOSECOUNT = value; }
            get { return _NOSECOUNT; }
        }
#endregion
        DataTable dtx1= new DataTable();
        DataTable dtx2 = new DataTable();
        CMOLD_BASE cmold_base = new CMOLD_BASE();
        public SELLTABLET()
        {
            InitializeComponent();
        }
        public SELLTABLET(SELLTABLE FRM)
        {
            InitializeComponent();
            F1 = FRM;
        }
        private void SELLTABLET_Load(object sender, EventArgs e)
        {
            //textBox1.Text = "SE16110001";
            //comboBox2.Text = "OR16100004";
            label10.Text = "";
            textBox50.BackColor = CCOLOR.lylfnp;
             this.Icon = Resource1.xz_200X200;
            textBox1.Text = IDO;
            comboBox2.Text = ORID;
            comboBox3.Text = LOGIN.EMPLOYEE_ID;
            label10.Text = LOGIN.ENAME;
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
        protected void bind()
        {
            comboBox2.BackColor = CCOLOR.YELLOW;
            hint.ForeColor = Color.Red;
            if (bc.GET_IFExecutionSUCCESS_HINT_INFO(IFExecution_SUCCESS) != "")
            {
                hint.Text = bc.GET_IFExecutionSUCCESS_HINT_INFO(IFExecution_SUCCESS);
            }
            else
            {
                hint.Text = "";
            }
            if (!string.IsNullOrEmpty(comboBox2.Text))
            {
                dt = ask(textBox1.Text, comboBox2.Text);
            }
            comboBox3.BackColor = CCOLOR.YELLOW;
            //MessageBox.Show("1");
            if (comboBox2.Text != null && comboBox2.Text != "")
            {
                DataTable dtx4 = basec.getdts(string.Format(@"
SELECT 
A.SEID,
A.ORID,
SUM(C.MRcount*A.unitprice)
FROM SELLTABLE_DET A 
LEFT JOIN ORDER_DET B ON A.ORID=B.ORID AND A.SN=B.SN
LEFT JOIN MATERE C ON A.SEKEY=C.MRKEY 
WHERE A.SEID='" + textBox1.Text + "' AND A.ORID IN ({0}) GROUP BY A.ORID,A.SEID ", comboBox2.Text));

                if (dtx4.Rows.Count > 0)
                {
                    string v8 = dtx4.Rows[0][2].ToString();
                    textBox50.Text = string.Format("{0:F2}", Convert.ToDouble(v8));

                }
                else
                {
                    textBox50.Text = "";

                }
                if (dtx4.Rows.Count > 0)
                {
                    string v8 = dtx4.Rows[0][2].ToString();
                    textBox50.Text = string.Format("{0:F2}", Convert.ToDouble(v8));

                }
                else
                {
                    textBox50.Text = "";

                }
            }


            if (!string.IsNullOrEmpty(comboBox2.Text))
            {
                if (dt.Rows.Count > 0)
                {
                    dataGridView1.DataSource = dt;
                    dgvStateControl();
                }
                else
                {
                    dataGridView1.DataSource = null;
                }
            }
            else
            {

                dataGridView1.DataSource = null;
            }
            dt1 = bc.getdt(cselltable.sql + " WHERE A.SEID='" + IDO + "'");
            dataGridView2.DataSource = dt1;
            dgvStateControl_2();
        }
        #endregion
        #region total1
        private DataTable total()
        {
            DataTable dtt2 = cselltable.GetTableInfo();
            for (i = 1; i <= 6; i++)
            {
                DataRow dr = dtt2.NewRow();
                dr["项次"] = i;
                dr["交货日期"] = varDate;
                dtt2.Rows.Add(dr);
            }
            return dtt2;
        }
        #endregion
        #region ask
        private DataTable ask(string SEID, string ORID)
        {
           
            DataTable dtt = new DataTable();
            dtt.Columns.Add("订单号", typeof(string));
            dtt.Columns.Add("项次", typeof(string));
            dtt.Columns.Add("型号", typeof(string));
            dtt.Columns.Add("品名", typeof(string));
            dtt.Columns.Add("材料", typeof(string));
            dtt.Columns.Add("单位", typeof(string));
            dtt.Columns.Add("下单日期", typeof(string));
            dtt.Columns.Add("订单交期", typeof(string));
            dtt.Columns.Add("单价", typeof(decimal));
            //dtt.Columns.Add("单价", typeof(decimal), "基数 * 重量");
            dtt.Columns.Add("基数", typeof(decimal));
            dtt.Columns.Add("订单数量", typeof(decimal));
            dtt.Columns.Add("重量", typeof(decimal));
            dtt.Columns.Add("累计销货数量", typeof(decimal));
            /*dtt.Columns.Add("累计销退数量", typeof(decimal));*/
            dtt.Columns.Add("未销货数量", typeof(decimal), "订单数量-累计销货数量");
            dtt.Columns.Add("批号", typeof(string));
            dtt.Columns.Add("库存数量", typeof(decimal));
            dtt.Columns.Add("销货数量", typeof(decimal));
            dtt.Columns.Add("本销货单累计销货数量", typeof(decimal));
            //dtt.Columns.Add("金额", typeof(decimal), "单价*基数*销货数量");
            dtt.Columns.Add("金额", typeof(decimal));
            //客户需求只调出未发货或是部分发货的订单数据，已经发货的订单项不显示 20191219

            /*使用数据集方式来组合sql语句以便只调一次sql server连接就获得多个数据表*/
            DataSet ds = new DataSet();
            StringBuilder sqb = new StringBuilder();
            sqb.AppendFormat(string.Format(corder.sql + " WHERE ORID in ({0}) and 未销数量<>0 ORDER BY ORID,SN ASC;", ORID));

            /*累计销货数量数据*/
            sqb.AppendFormat(@"SELECT
A.ORID AS ORID,
A.SN AS SN,
CAST(ROUND(SUM(B.MRCOUNT), 2) AS DECIMAL(18, 2)) AS MRCOUNT
FROM SELLTABLE_DET A
LEFT JOIN MATERE
B ON A.SEKEY = B.MRKEY
WHERE  A.ORID in ({0}) GROUP BY A.ORID,A.SN;", ORID);

            /*本销货单累计销货数量*/
            sqb.AppendFormat(@"
SELECT 
A.ORID AS ORID,
A.SEID AS SEID,
A.SN AS SN,
CAST(ROUND(SUM(B.MRCOUNT),2) AS DECIMAL(18,2)) AS MRCOUNT
FROM SELLTABLE_DET A 
LEFT JOIN  MATERE B ON A.SEKEY=B.MRKEY 
WHERE  A.ORID in ({0}) AND A.SEID='" + SEID + "' GROUP BY A.ORID,A.SEID,A.SN;", ORID);

            sqb.AppendFormat(@"SELECT 
A.ORID AS ORID,
A.SN AS SN,
SUM(B.GECOUNT) AS GECOUNT
FROM SELLRETURN_DET A
LEFT JOIN GODE B ON A.SRKEY = B.GEKEY
GROUP BY
A.ORID,
A.SN");
            //去摸具库拉取订单号对应的摸具数据用于得到相关的重量数据

            sqb.AppendFormat(";SELECT *, WEIGHT AS 重量 FROM MOLD_BASE WHERE CUID IN (SELECT CUID FROM Order_MST WHERE ORID IN  ({0}))", ORID);

            SqlConnection sqlConnection = bc.getcon();
            sqlConnection.Open();
            SqlCommand sqlCommand = new SqlCommand(sqb.ToString(), sqlConnection);
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
            sqlDataAdapter.Fill(ds);
            sqlConnection.Close();

            /*订单相关数据*/
            dtx1 = ds.Tables[0];
            /*累计销货数量数据*/
            DataTable dtx2 = ds.Tables[1];
            /*本销货单累计销货数量*/
            DataTable dtx3 = ds.Tables[2];
            /*累计销退数量*/
            DataTable dtx4 = ds.Tables[3];
            /*取得库存数量表*/
            DataTable dtx5 = bc.getstoragecountNew();
            //取得该客户的模具库数据
            DataTable dtx7 = ds.Tables[4];

            if (dtx1.Rows.Count > 0)
            {
                for (i = 0; i < dtx1.Rows.Count; i++)
                {
                    decimal d1 = 0;decimal d2 = 0;
                    DataRow dr = dtt.NewRow();
                    dr["订单号"] = dtx1.Rows[i]["订单号"].ToString();
                    dr["项次"] = dtx1.Rows[i]["项次"].ToString();
                    dr["型号"] = dtx1.Rows[i]["型号"].ToString();
                    dtx = bc.GET_DT_TO_DV_TO_DT(dtx7, "", @"CUID='"
+dtx1.Rows[i]["CUID"].ToString()+"' AND WAREID='"+dtx1.Rows[i]["WAREID"].ToString() +"' AND MAID='"+dtx1.Rows[i]["MAID"].ToString() + "' AND WNAME='" + dtx1.Rows[i]["WNAME"].ToString() + "'");

                    //重量直接带模具库不带订单里的目的是获得最新的模具库重量数据，可能做订单时没有重量，订单做完后又维护了模具库数据 20201026
                    if (dtx.Rows .Count >0)
                    {
                        d2 = decimal.Parse(dtx.Rows[0]["WEIGHT"].ToString());
                        dr["重量"] = d2;
                    }
                    else
                    {
                        dr["重量"] = DBNull.Value;

                    }
                    if (!string.IsNullOrEmpty(dtx1.Rows[i]["基数"].ToString()))
                    {
                        dr["基数"] = dtx1.Rows[i]["基数"].ToString();
                        d1 = decimal.Parse(dtx1.Rows[i]["基数"].ToString());
                    }
                    dr["单价"] = (d1 * d2).ToString("0.0000");
                    dr["订单数量"] = dtx1.Rows[i]["数量"].ToString();
                    dr["型号"] = dtx1.Rows[i]["型号"].ToString();
                    dr["品名"] = dtx1.Rows[i]["品名"].ToString();
                    dr["材料"] = dtx1.Rows[i]["材料"].ToString();
                    dr["单位"] = dtx1.Rows[i]["单位"].ToString();
                    dr["下单日期"] = dtx1.Rows[i]["下单日期"].ToString();
                    dr["订单交期"] = dtx1.Rows[i]["订单交期"].ToString();
                    dr["累计销货数量"] = 0;
                    /*dr["累计销退数量"] = 0;*/
                    dr["本销货单累计销货数量"] = 0;
                    dtt.Rows.Add(dr);
                    SKU = dtx1.Rows[i]["单位"].ToString();
                    //取得订单项次的库存数量
                    DataTable dtx6 = bc.getmaxstoragecountNew(dtx5,dtx1.Rows[i]["索引"].ToString(), SKU);
                    if (dtx6.Rows.Count > 0)
                    {
                        dr["批号"] = dtx6.Rows[0]["批号"].ToString();
                        dr["库存数量"] = dtx6.Rows[0]["库存数量"].ToString();
                    }
         
                }
            }
            DataView dv = dtt.DefaultView;
            dv.RowFilter = "批号 is not null";//过滤没有库存的订单项以减少显示项，客户需求 191219
            dtt = dv.ToTable();
            if (dtx2.Rows.Count > 0)
            {
                for (i = 0; i < dtx2.Rows.Count; i++)
                {
                    for (j = 0; j < dtt.Rows.Count; j++)
                    {
                        if (dtt.Rows[j]["订单号"].ToString() == dtx2.Rows[i]["ORID"].ToString() && dtt.Rows[j]["项次"].ToString() == dtx2.Rows[i]["SN"].ToString())
                        {
                            dtt.Rows[j]["累计销货数量"] = dtx2.Rows[i]["MRCOUNT"].ToString();
                            break;
                        }

                    }
                }

            }
        
            if (dtx3.Rows.Count > 0)
            {
                for (i = 0; i < dtx3.Rows.Count; i++)
                {
                    for (j = 0; j < dtt.Rows.Count; j++)
                    {
                        if (dtt.Rows[j]["订单号"].ToString() == dtx3.Rows[i]["ORID"].ToString() && dtt.Rows[j]["项次"].ToString() == dtx3.Rows[i]["SN"].ToString())
                        {
                            dtt.Rows[j]["本销货单累计销货数量"] = dtx3.Rows[i]["MRCOUNT"].ToString();
                            break;
                        }

                    }
                }

            }
            if (dtx4.Rows.Count > 0)
            {
                for (i = 0; i < dtx4.Rows.Count; i++)
                {
                    for (j = 0; j < dtt.Rows.Count; j++)
                    {
                        if (dtt.Rows[j]["订单号"].ToString() == dtx4.Rows[i]["ORID"].ToString() && dtt.Rows[j]["项次"].ToString() == dtx4.Rows[i]["SN"].ToString())
                        {
                            dtt.Rows[j]["累计销退数量"] = dtx4.Rows[i]["GECOUNT"].ToString();
                            break;
                        }

                    }
                }

            }
            foreach (DataRow dr in dtt.Rows )
            {
                decimal d1 = 0, d2 = 0, d3 = 0,d4=0;
                if (!string.IsNullOrEmpty(dr["单价"].ToString()))
                {
                    dr["单价"] = dr["单价"].ToString();
                    d1 = decimal.Parse(dr["单价"].ToString());
                }
                else
                {
                    dr["单价"] = DBNull.Value;
                }
           
                if (!string.IsNullOrEmpty(dr["基数"].ToString()))
                {
                    dr["基数"] = dr["基数"].ToString();
                    d2 = decimal.Parse(dr["基数"].ToString());
                 
                }
                else
                {
                    dr["基数"] = DBNull.Value;
                }
                if (!string.IsNullOrEmpty(dr["重量"].ToString()))
                {
                    dr["重量"] = dr["重量"].ToString();
                    d3 = decimal.Parse(dr["重量"].ToString());
                }
                else
                {
                    dr["重量"] = DBNull.Value;
                }
                dr["销货数量"] = dr["未销货数量"].ToString();
                if (!string.IsNullOrEmpty(dr["销货数量"].ToString()))
                {
                    dr["销货数量"] = dr["销货数量"].ToString();
                    d4 = decimal.Parse(dr["销货数量"].ToString());
                }
                else
                {
                    dr["销货数量"] = DBNull.Value;
                }

                dr["金额"] = (d1 * d2 * d3*d4).ToString("0.00");
            }
            return dtt;
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
                dataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                this.dataGridView1.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                //this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
                dataGridView1.EnableHeadersVisualStyles = false;
                dataGridView1.Columns[i].HeaderCell.Style.BackColor = Color.Lavender;
                dataGridView1.Columns[i].ReadOnly = true;
            }

            for (i = 0; i < dataGridView1.Columns.Count; i++)
            {
                dataGridView1.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridView1.Columns[i].DefaultCellStyle.BackColor = Color.OldLace;
              
                i = i + 1;

            }
   
            dataGridView1.Columns["项次"].ReadOnly = true;
            dataGridView1.Columns["单价"].ReadOnly = false;
            dataGridView1.Columns["重量"].ReadOnly = false;
            dataGridView1.Columns["批号"].ReadOnly = false;
            dataGridView1.Columns["销货数量"].ReadOnly = false;
            dataGridView1.Columns["项次"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView1.Columns["单价"].DefaultCellStyle.BackColor = CCOLOR.YELLOW;
            dataGridView1.Columns["重量"].DefaultCellStyle.BackColor = CCOLOR.YELLOW;
            dataGridView1.Columns["销货数量"].DefaultCellStyle.BackColor = CCOLOR.YELLOW;
            dataGridView1.Columns["批号"].DefaultCellStyle.BackColor = CCOLOR.YELLOW;  
        }
        #endregion
        #region dgvStateControl_2
        private void dgvStateControl_2()
        {
            int i;
            dataGridView2.RowHeadersDefaultCellStyle.BackColor = Color.Lavender;
            int numCols1 = dataGridView2.Columns.Count;
     
            for (i = 0; i < numCols1; i++)
            {
                dataGridView2.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                this.dataGridView2.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                //this.dataGridView2.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
                dataGridView2.EnableHeadersVisualStyles = false;
                dataGridView2.Columns[i].HeaderCell.Style.BackColor = Color.Lavender;
                dataGridView2.Columns[i].ReadOnly = true;
            }
            for (i = 0; i < dataGridView2.Columns.Count; i++)
            {
                dataGridView2.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridView2.Columns[i].DefaultCellStyle.BackColor = Color.OldLace;
                i = i + 1;
            }
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
            ORKEY = bc.getOnlyString("SELECT ORKEY FROM ORDER_DET WHERE ORID='" + dt.Rows[rows]["订单号"].ToString() +
                "' AND SN='" + dt.Rows[rows]["项次"].ToString() + "'");
            if (dataGridView1.Columns[columns].DataPropertyName.ToString() == "批号")
            {
                
                STOCK_MANAGE.MISC_STORAGE FRM = new STOCK_MANAGE.MISC_STORAGE();
                FRM.SELECT = 1;
                FRM.ORKEY = ORKEY;
                FRM.ShowDialog();
                if (FRM.BATCHID  != "")
                {
                    dt.Rows[rows]["批号"] = FRM.BATCHID;
                }
                //dataGridView1.CurrentCell = dataGridView1["基数", rows];
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
                        dr["交货日期"] = varDate;
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

            MessageBox.Show("数值型数据只能输入数字", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
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
            IDO = cselltable.GETID();
            textBox1.Text = IDO;
           
            bind();
            dataGridView1.DataSource = null;
            comboBox1.Focus();
        }
        public void ClearText()
        {
            comboBox1.Text = "";
            comboBox2.Text = "";
            comboBox3.Text = LOGIN.EMPLOYEE_ID;
            label10.Text = LOGIN.ENAME;
            textBox2.Text = "";
            dateTimePicker1.Text = DateTime.Now.ToString("yyyy/MM/dd").Replace("-", "/");
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox50.Text = "";
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
            FRM.SELECT = 1;
            FRM.ShowDialog();
            this.comboBox1.IntegralHeight = false;//使组合框不调整大小以显示其所有项
            this.comboBox1.DroppedDown = false;//使组合框不显示其下拉部分
            this.comboBox1.IntegralHeight = true;//恢复默认值
            if (FRM.CNAME != "")
            {
                comboBox1.Text = FRM.CNAME;
            }
            comboBox2.Focus();
        }
   
        private void btnSave_Click(object sender, EventArgs e)
        {
            IFExecution_SUCCESS = false;
            hint.Text = "";
            btnSave.Focus();
            StringBuilder sqb = new StringBuilder();
            //校验一个销货单下只能有一个客户名称
            sqb.AppendFormat(@"
select B.CUID  from SellTable_DET A
INNER JOIN Order_MST B ON A.ORID = B.ORID
INNER JOIN CustomerInfo_MST C ON B.CUID = C.CUID
WHERE A.SEID = '" + IDO+ "';");

            //判断订单号是否存在系统及获取ORKEY
            sqb.AppendFormat("SELECT * FROM ORDER_MST A LEFT JOIN ORDER_DET B ON  A.ORID=B.ORID WHERE A.ORID IN ({0});",comboBox2.Text);

            //判断发货员工号是否存在系统
            sqb.AppendFormat("SELECT* FROM EMPLOYEEINFO WHERE EMPLOYEE_ID = '{0}';", comboBox3.Text);

            //取得订单累计销货数量
            sqb.AppendFormat(@"SELECT
A.ORID AS ORID,
A.SN AS SN,
B.WAREID AS WAREID,
CAST(ROUND(SUM(B.MRCOUNT), 2) AS DECIMAL(18, 2)) AS MRCOUNT
FROM SELLTABLE_DET A
LEFT JOIN MATERE
B ON A.SEKEY = B.MRKEY
WHERE  A.ORID IN ({0}) GROUP BY A.ORID, A.SN, B.WAREID;",comboBox2.Text);

            //去库存表中查询批号对应的ORKY
            sqb.AppendFormat("SELECT * FROM Gode WHERE ORKEY IN (SELECT ORKEY FROM Order_DET WHERE ORID IN ({0}))",comboBox2.Text);

            SqlConnection sqlConnection = bc.getcon();
            sqlConnection.Open();
            SqlCommand sqlCommand = new SqlCommand(sqb.ToString(), sqlConnection);
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
            DataSet ds = new DataSet();
            sqlDataAdapter.Fill(ds);
            sqlConnection.Close();
            //校验一个销货单下只能有一个客户名称
            DataTable dtx1 = ds.Tables[0];

            //判断订单号是否存在系统及获取ORKEY
            DataTable dtx2 = ds.Tables[1];

            //判断发货员工号是否存在系统
            DataTable dtx3 = ds.Tables[2];

            //获取库存表
            DataTable dtx4 = bc.getstoragecountNew();

            //取得订单累计销货数量
            DataTable dtx5 = ds.Tables[3];

            //去库存表中查询批号对应的ORKY
            DataTable dtx6 = ds.Tables[4];
            if (juage(dtx1,dtx2,dtx3,dtx4,dtx5,dtx6))
            {

            }
            else
            {

                btnSave.Focus();
                cselltable.MAKERID = LOGIN.EMID;
                cselltable.ORID = comboBox2.Text;
                cselltable.SEID = IDO;
                cselltable.CNAME = comboBox1.Text;
                cselltable.SELLDATE = dateTimePicker1.Text;
                if(dtx3.Rows.Count>0)
                { 
                    cselltable.SELLERID = dtx3.Rows[0]["EMID"].ToString();
                }
                cselltable.SEND_ADDRESS = textBox4.Text;
                cselltable.CONTACT = textBox2.Text;
                cselltable.PHONE = textBox3.Text;
                dt = cselltable.save(dt,dtx1,dtx2,dtx4);
                IFExecution_SUCCESS = cselltable.IFExecution_SUCCESS;
                hint.Text = cselltable.ErrowInfo;
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
                    bind();
                    F1.bind();
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
        #region juage
        private bool juage(DataTable dtx1, DataTable dtx2, DataTable dtx3,DataTable dtx4,DataTable dtx5,DataTable dtx6)
        {
            bool b = false;
            bool c = false;
            if (IDO == "")
            {
                hint.Text = "编号不能为空！";
                b = true;
            }
            else if (dtx1.Rows.Count > 0 && dtx1.Rows[0]["CUID"].ToString() != CUID)
            {
                hint.Text = "同一个发货单下面只能出现一个客户名称!";
                b = true;
            }
            else if (dtx2.Rows.Count == 0)
            {
                hint.Text = "订单号为空或不存在于系统中！";
                b = true;
            }
            /*if (cselltable.JUAGE_RESIDUE_SECOUNT_IF_LESSTHAN_SR_COUNT(comboBox2.Text ))
            {

                hint.Text  = cselltable.ErrowInfo;
                b = true;
            }*/
            else if (comboBox3.Text == "")
            {
                hint.Text = "发货工号不能为空！";
                b = true;
            }
            else if (dtx3.Rows.Count == 0)
            {
                hint.Text = "发货员工号不存在于系统中！";
                b = true;
            }
            else if (juage2(dtx2,dtx4,dtx5,dtx6))
            {
                b = true;
            }
            return b;
        }
        #endregion
        #region juage2()
        private bool juage2(DataTable dtx2,DataTable dtx4,DataTable dtx5,DataTable dtx6)
        {
            bool b = false;
            foreach (DataRow dr in dt.Rows)
            {
                if (dr["销货数量"].ToString() == "0")//销货数量为0不做处理，即此项不用销货
                {

                }
                else
                {
                    string SECOUNT = dr["销货数量"].ToString();
                    string BATCHID = dr["批号"].ToString();
                    string NOSECOUNT = "";
                    /*避免单据未保存前，此入库单在别的电脑修改过库存数量,销过货，引起库存数量，未销货数量不对,再次更新数据为最新 start 161120*/
                    SKU = dr["单位"].ToString();

                    if (dtx2.Rows.Count > 0)
                    {
                        ORKEY = bc.GET_DT_TO_DV_TO_DT(dtx2, "", " ORID='" + dr["订单号"].ToString() + "' AND SN='" + dr["项次"].ToString() + "'").Rows[0].ToString();
                    }
                    dtx = bc.GET_DT_TO_DV_TO_DT(dtx4, "", "批号='" + dr["批号"].ToString() + "'");
                    string STORAGECOUNT = "";
                    if (dtx.Rows.Count > 0)
                    {
                        STORAGECOUNT = dtx.Rows[0]["库存数量"].ToString();
                        dr["库存数量"] = dtx.Rows[0]["库存数量"].ToString();
                    }
                    else
                    {
                        dr["库存数量"] = "0";
                        STORAGECOUNT = "0";
                    }

                    if (dtx5.Rows.Count > 0)
                    {
                        dr["累计销货数量"] = dtx5.Rows[0]["MRCOUNT"].ToString();

                    }
                    else
                    {
                        dr["累计销货数量"] = "0";

                    }
                    NOSECOUNT = dr["未销货数量"].ToString();
                    /*避免单据未保存前，此入库单在别的电脑修改过库存数量,销过货，引起库存数量，未销货数量不对,再次更新数据为最新 end 161120*/
                    string GET_ORKEY = null;
                    if(dtx6.Rows.Count>0)
                    {
                        GET_ORKEY = bc.GET_DT_TO_DV_TO_DT(dtx6, "", "BATCHID='" + BATCHID + "'").Rows[0].ToString();
                    }
                    if (dr["单价"].ToString() == "")
                    {
                        hint.Text = string.Format("订单号：{0} " + "项次：{1} 单价不能为空！", dr["订单号"].ToString(), dr["项次"].ToString());
                        b = true;
                        break;
                    }
                    else if (dr["基数"].ToString() == "")
                    {
                        hint.Text = string.Format("订单号：{0} " + "项次：{1} 基数不能为空！", dr["订单号"].ToString(), dr["项次"].ToString());
                        b = true;
                        break;
                    }
                    else if (dr["重量"].ToString() == "")
                    {
                        hint.Text = string.Format("订单号：{0} " + "项次：{1} 重量不能为空！", dr["订单号"].ToString(), dr["项次"].ToString());
                        b = true;
                        break;
                    }
                    else if (SECOUNT == "")
                    {
                        hint.Text = string.Format("订单号：{0} 与项次：{1} 销货数量不能为空！",
                         dr["订单号"].ToString(), dr["项次"].ToString());
                        b = true;
                        break;
                    }
                    else if (bc.yesno(SECOUNT) == 0)
                    {
                        hint.Text = "数量只能输入数字！";
                        b = true;
                        break;
                    }
                    else if (decimal.Parse(SECOUNT) == 0)
                    {
                        hint.Text = "销货数量不能为0！";
                        b = true;
                        break;
                    }

                    else if (BATCHID == "")
                    {
                        hint.Text = string.Format("项次：{0} 批号不能为空！", dr["项次"].ToString());
                        b = true;
                        break;
                    }
                    else if (!bc.exists("SELECT * FROM GODE WHERE BATCHID='" + dr["批号"].ToString() + "'"))
                    {
                        hint.Text = string.Format("项次：{0} 批号不存在系统！", dr["项次"].ToString());
                        b = true;
                        break;
                    }
                    else if (ORKEY != GET_ORKEY)
                    {
                        hint.Text = string.Format("选择的批号：{0} 不属于订单号：{1} 与项次：{2} ",
                            BATCHID, dr["订单号"].ToString(), dr["项次"].ToString());
                        b = true;
                        break;
                    }
                    else if (decimal.Parse(SECOUNT) > decimal.Parse(NOSECOUNT))
                    {
                        hint.Text = string.Format("订单号：{0} 与项次：{1} 销货数量不能大于未销货数量！",
                            dr["订单号"].ToString(), dr["项次"].ToString());
                        b = true;
                        break;
                    }

                    else if (decimal.Parse(SECOUNT) > decimal.Parse(STORAGECOUNT))
                    {
                        hint.Text = string.Format("订单号：{0} 与项次：{1} 销货数量不能大于批号：{2} 的库存数量: {3} ！",
                           dr["订单号"].ToString(), dr["项次"].ToString(), BATCHID, STORAGECOUNT);
                        b = true;
                        break;
                    }
                }
            }
            return b;
        }
        #endregion

        private void btnDel_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("确定要删除该条凭证吗？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    basec.getcoms("DELETE SELLTABLE_MST WHERE SEID='" + textBox1.Text + "'");
                    basec.getcoms("DELETE SELLTABLE_DET WHERE SEID='" + textBox1.Text + "'");
                    basec.getcoms("DELETE MATERE WHERE MATEREID='" + textBox1.Text + "'");
                    ClearText();
                    bind();
                    F1.bind();  
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
            int i=dataGridView1 .CurrentCell .RowIndex ;
            if (dataGridView1["单价", i].FormattedValue.ToString() != "" && 
                dataGridView1["重量", i].FormattedValue.ToString()!="" &&
                  dataGridView1["基数", i].FormattedValue.ToString() != "" &&
                 dataGridView1["销货数量", i].FormattedValue.ToString() != "")
            {
                dataGridView1["金额", i].Value = 
                    (decimal.Parse(dataGridView1["单价", i].FormattedValue.ToString()) *
                    decimal.Parse(dataGridView1["基数", i].FormattedValue.ToString()) *
                    decimal.Parse(dataGridView1["重量", i].FormattedValue.ToString()) *
                    decimal.Parse(dataGridView1["销货数量", i].FormattedValue.ToString())).ToString ("0.00");
               
            }
            textBox50.Text = dt.Compute("SUM(金额)", "").ToString();
        }

        private void comboBox2_DropDown(object sender, EventArgs e)
        {
            IF_DOUBLE_CLICK = false;
            SELL_MANAGE.ORDER FRM = new ORDER();
            FRM.CNAME = comboBox1.Text;
            FRM.SELECT = 1;
            FRM.ShowDialog();
            this.comboBox2.IntegralHeight = false;//使组合框不调整大小以显示其所有项
            this.comboBox2.DroppedDown = false;//使组合框不显示其下拉部分
            this.comboBox2.IntegralHeight = true;//恢复默认值
            if (FRM.getSelectOrid != null)
            {
                comboBox2.Text = FRM.getSelectOrid;
                this.ActiveControl.TabIndex = 5;

            }
            bind();
            try
            {
      
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);

            }
 
        }
        private void comboBox3_DropDown(object sender, EventArgs e)
        {
            IF_DOUBLE_CLICK = false;
            BASE_INFO.EMPLOYEE_INFO FRM = new CSPSS.BASE_INFO.EMPLOYEE_INFO();
            FRM.SELECT = 1;
            FRM.ShowDialog();
            this.comboBox3.IntegralHeight = false;//使组合框不调整大小以显示其所有项
            this.comboBox3.DroppedDown = false;//使组合框不显示其下拉部分
            this.comboBox3.IntegralHeight = true;//恢复默认值
            if (FRM.EMPLOYEE_ID != null)
            {
                comboBox3.Text = FRM.EMPLOYEE_ID;
                label10.Text = FRM.ENAME;
            }

        }

        private void comboBox4_DropDown(object sender, EventArgs e)
        {
          
        }

        private void comboBox2_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(comboBox2.Text))
            {
                dtx = bc.getdt(string.Format(corder.sql + " WHERE ORID in ({0})", comboBox2.Text));
                if (dtx.Rows.Count > 0)
                {
                    textBox4.Text = dtx.Rows[0]["公司地址"].ToString();
                    textBox2.Text = dtx.Rows[0]["联系人"].ToString();
                    textBox3.Text = dtx.Rows[0]["联系电话"].ToString();
                    comboBox1.Text = dtx.Rows[0]["客户名称"].ToString();
                    CUID= dtx.Rows[0]["客户编号"].ToString();
                }
                else
                {
                    textBox4.Text = "";
                    textBox2.Text = "";
                    textBox3.Text = "";
                    comboBox1.Text = "";
                    CUID = "";
                }
                bind();

            }
            try
            {
           
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);

            }
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            cselltable.MAKERID = "";
           
            cselltable.ExcelPrint(dt1, "销货单", System.IO.Path.GetFullPath("销货单.xls"));
            //corder.ExcelPrint_40X30(dataGridView1, "订单", System.IO.Path.GetFullPath("订单40X30.xlsx"));
            hint.Text = cselltable.ErrowInfo;
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

using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using System.Data.SqlClient;
using System.Windows.Forms;
using XizheC;

namespace XizheC
{
    public class CMISC_STORAGE
    {
        basec bc = new basec();
        DataTable dt = new DataTable();
        #region nature
        private string _WAREID;
        public string WAREID
        {
            set { _WAREID = value; }
            get { return _WAREID; }
        }
        private string _ORKEY;
        public string ORKEY
        {
            set { _ORKEY = value; }
            get { return _ORKEY; }
        }
        private string _GODE_MAKERID;
        public string GODE_MAKERID
        {
            set { _GODE_MAKERID = value; }
            get { return _GODE_MAKERID; }
        }
        private string _REMARK;
        public string REMARK
        {
            set { _REMARK = value; }
            get { return _REMARK; }
        }
        private string _GODE_DATE;
        public string GODE_DATE
        {
            set { _GODE_DATE = value; }
            get { return _GODE_DATE; }
        }
        private string _BARCODE;
        public string BARCODE
        {
            set { _BARCODE = value; }
            get { return _BARCODE; }
        }
        private string _SN;
        public string SN
        {
            set { _SN = value; }
            get { return _SN; }
        }
        private string _MGKEY;
        public string MGKEY
        {
            set { _MGKEY = value; }
            get { return _MGKEY; }
        }
        private string _MGCOUNT;
        public string MGCOUNT
        {
            set { _MGCOUNT = value; }
            get { return _MGCOUNT; }
        }
        private string _SKU;
        public string SKU
        {
            set { _SKU = value; }
            get { return _SKU; }

        }
        private string _sql;
        public string sql
        {
            set { _sql = value; }
            get { return _sql; }

        }
        private string _sqlo;
        public string sqlo
        {
            set { _sqlo = value; }
            get { return _sqlo; }

        }
        private string _sqlt;
        public string sqlt
        {
            set { _sqlt = value; }
            get { return _sqlt; }

        }
        private string _sqlth;
        public string sqlth
        {
            set { _sqlth = value; }
            get { return _sqlth; }

        }
        private string _sqlf;
        public string sqlf
        {
            set { _sqlf = value; }
            get { return _sqlf; }

        }
        private string _sqlfi;
        public string sqlfi
        {
            set { _sqlfi = value; }
            get { return _sqlfi; }

        }
        private string _MAKERID;
        public string MAKERID
        {
            set { _MAKERID = value; }
            get { return _MAKERID; }

        }
        private  bool _IFExecutionSUCCESS;
        public bool IFExecution_SUCCESS
        {
            set { _IFExecutionSUCCESS = value; }
            get { return _IFExecutionSUCCESS; }

        }
        private string _ErrowInfo;
        public string ErrowInfo
        {

            set { _ErrowInfo = value; }
            get { return _ErrowInfo; }

        }
        private string _WP_COUNT;
        public string WP_COUNT
        {
            set { _WP_COUNT = value; }
            get { return _WP_COUNT; }
        }
        private string _MGID;
        public string MGID
        {
            set { _MGID = value; }
            get { return _MGID; }
        }
        #endregion
        #region sql
        string setsql = @"
SELECT
ROW_NUMBER() OVER (ORDER BY A.MGKEY ASC)  AS 序号, 
A.MGID AS 入库单号, 
A.SN AS 项次,
D.ORID AS 订单号,
D.SN AS 订单项次,
D.WareID AS 型号,
D.WNAME AS 品名,
D.MATERIAL AS 材料,
G.ORDER_DATE AS 下单日期,
c.GECount as 数量,
D.SKU AS 单位,
D.WEIGHT AS 重量,
E.CNAME  AS 客户名称,
F.GODE_DATE AS 入库日期,
(SELECT EMPLOYEE_ID FROM EMPLOYEEINFO WHERE EMID=F.GODE_MAKERID )  AS 入库员工号,
(SELECT ENAME FROM EMPLOYEEINFO WHERE EMID=F.GODE_MAKERID )  AS 入库员,
(SELECT ENAME FROM EMPLOYEEINFO WHERE EMID=F.MAKERID )  AS 制单人,
F.DATE AS 制单日期,
C.BATCHID AS 批号
FROM MISC_GODE_DET A 
LEFT JOIN Gode  C ON A.MGKEY=C.GEKEY
LEFT JOIN ORDER_BARCODE B ON B.BARCODE =C.BatchID 
LEFT JOIN Order_DET D ON D.ORKEY=B.ORKEY 
LEFT JOIN Order_MST G ON D.ORID =G.ORID 
LEFT JOIN CUSTOMERINFO_MST E ON G.CUID=E.CUID
LEFT JOIN MISC_GODE_MST F ON A.MGID=F.MGID
";
        string setsqlo = @"
INSERT INTO 
MISC_GODE_DET
(
MGKEY,
MGID,
SN,
REMARK,
YEAR,
MONTH,
DAY
)
VALUES
(
@MGKEY,
@MGID,
@SN,
@REMARK,
@YEAR,
@MONTH,
@DAY

)
";
        string setsqlt = @"
INSERT INTO 
MISC_GODE_MST
(
MGID,
GODE_DATE,
GODE_MAKERID,
DATE,
MAKERID,
YEAR,
MONTH,
DAY
)
VALUES
(
@MGID,
@GODE_DATE,
@GODE_MAKERID,
@DATE,
@MAKERID,
@YEAR,
@MONTH,
@DAY
)
";
        string setsqlth = @"
UPDATE MISC_GODE_MST SET 
GODE_DATE=@GODE_DATE,
GODE_MAKERID=@GODE_MAKERID,
DATE=@DATE,
MAKERID=@MAKERID,
YEAR=@YEAR,
MONTH=@MONTH,
DAY=@DAY

";
        string setsqlf = @"
INSERT INTO GODE
(
GEKEY,
GODEID,
SN,
WAREID,
GECOUNT,
SKU,
STORAGEID,
SLID,
ORKEY,
BATCHID,
DATE,
MAKERID,
YEAR,
MONTH,
DAY
)
VALUES
(
@GEKEY,
@GODEID,
@SN,
@WAREID,
@GECOUNT,
@SKU,
@STORAGEID,
@SLID,
@ORKEY,
@BATCHID,
@DATE,
@MAKERID,
@YEAR,
@MONTH,
@DAY
)
";
        string setsqlfi = @"


";
        #endregion
        int i;
        public CMISC_STORAGE()
        {
            sql = setsql;
            sqlo = setsqlo;
            sqlt = setsqlt;
            sqlth = setsqlth;
            sqlf = setsqlf;
            sqlfi = setsqlfi;
        }
        public string GETID()
        {
            string v1 = bc.numYM(10, 4, "0001", "SELECT * FROM MISC_GODE_MST", "MGID", "MG");
            string GETID = "";
            if (v1 != "Exceed Limited")
            {
                GETID = v1;
            }
            return GETID;
        }
        #region GetTableInfo
        public DataTable GetTableInfo()
        {
            dt = new DataTable();
            dt.Columns.Add("项次", typeof(string));
            dt.Columns.Add("型号", typeof(string));
            dt.Columns.Add("品名", typeof(string));
            dt.Columns.Add("材料", typeof(string));
            dt.Columns.Add("数量", typeof(string));
            dt.Columns.Add("单位", typeof(string));
            dt.Columns.Add("批号", typeof(string));
            return dt;
        }
        #endregion
        #region ask
        public DataTable ask(string MGID)
        {
            string sql1 = sqlo;
            DataTable dtt = bc.getdt(sqlfi + " WHERE A.MGID='" + MGID + "' ORDER BY A.MGKEY ASC");
            return dtt;
        }
        #endregion
        #region  JUAGE_CURRENT_STORAGECOUNT_IF_LESSTHAN_DELETE_COUNT
        public bool JUAGE_CURRENT_STORAGECOUNT_IF_LESSTHAN_DELETE_COUNT(string MGID)
        {
            bool b = false;
            DataTable dt = bc.getdt(sql+ " WHERE A.MGID='" + MGID + "'");
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    WAREID  = dr["批号"].ToString();
        
                    decimal d= decimal.Parse(dr["数量"].ToString());
                    decimal d1 = 0;
                    DataView dv = new DataView(bc.getstoragecountNew());
                    dv.RowFilter = "批号='" + WAREID  + "'";
                   
                    DataTable dtx = dv.ToTable();
                    if (dtx.Rows.Count > 0)
                    {
                        d1 = decimal.Parse(dtx.Rows[0]["库存数量"].ToString());
                        if (d1 < d)
                        {
                            b = true;
                            ErrowInfo = "批号："+WAREID +" 库存数量：" + d1.ToString("#0.00") 
                                +"小于该批号要删除的入库数量：" + d.ToString("0.00") + "，不允许编辑或删除该单据";
                            break;
                        }
                        
                    }
                    else
                    {

                        b = true;
                        ErrowInfo = "批号："+WAREID +" 库存数量为0："+"不允许编辑或删除该单据";
                        break;
                    }
                }
            }
            return b;
        }
        #endregion
        #region save_BARCODE
        public void save_BARCODE()
        {
            string year = DateTime.Now.ToString("yy");
            string month = DateTime.Now.ToString("MM");
            string day = DateTime.Now.ToString("dd");
            string varDate = DateTime.Now.ToString("yyy/MM/dd HH:mm:ss").Replace("-", "/");
        
                int s1, s2;
                DataTable dty = bc.getdt("SELECT * FROM MISC_GODE_DET WHERE MGID='" + MGID + "'");
                if (dty.Rows.Count > 0)
                {
                    s1 = Convert.ToInt32(dty.Rows[dty.Rows.Count - 1]["SN"].ToString());
                    s2 = Convert.ToInt32(s1) + 1;
                }
                else
                {
                    s2 = 1;
                }
                SN = Convert.ToString(s2);
            
            
            if (!bc.exists("SELECT MGID FROM MISC_GODE_DET WHERE MGID='" + MGID + "'"))
            {
               
                    SQlcommandE_DET(sqlo);
                    SQlcommandE_GODE(sqlf);
                
                SQlcommandE_MST(sqlt);
                IFExecution_SUCCESS = true;
            }
            else
            {

               
                    SQlcommandE_DET(sqlo);
                    SQlcommandE_GODE(sqlf);
                
                SQlcommandE_MST(sqlth + " WHERE MGID='" + MGID + "'");
                IFExecution_SUCCESS = true;
            }
            
        }
        #endregion
        #region save
        public void save(DataTable dt, bool COME_FROM_DGV_OR_BARCODE)
        {
            string year = DateTime.Now.ToString("yy");
            string month = DateTime.Now.ToString("MM");
            string day = DateTime.Now.ToString("dd");
            string varDate = DateTime.Now.ToString("yyy/MM/dd HH:mm:ss").Replace("-", "/");
            if (COME_FROM_DGV_OR_BARCODE)//来自入库单DGV输入数据
            {
                basec.getcoms("DELETE MISC_GODE_DET WHERE MGID='" + MGID + "'");
                basec.getcoms("DELETE GODE WHERE GODEID='" + MGID + "'");
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["型号"].ToString() == "")
                    {

                    }
                    else
                    {
                        int s1, s2;
                        DataTable dty = bc.getdt("SELECT * FROM MISC_GODE_DET WHERE MGID='" + MGID + "'");
                        if (dty.Rows.Count > 0)
                        {
                            s1 = Convert.ToInt32(dty.Rows[dty.Rows.Count - 1]["SN"].ToString());
                            s2 = Convert.ToInt32(s1) + 1;
                        }
                        else
                        {
                            s2 = 1;
                        }
                        SN = Convert.ToString(s2);
                        WAREID = dt.Rows[i]["型号"].ToString();
                        MGCOUNT = dt.Rows[i]["数量"].ToString();
                        SKU = dt.Rows[i]["单位"].ToString();
                        BARCODE = dt.Rows[i]["批号"].ToString();
                        ORKEY = bc.getOnlyString("SELECT ORKEY FROM ORDER_BARCODE WHERE BARCODE='" + BARCODE + "'");
                        SQlcommandE_DET(sqlo);
                        SQlcommandE_GODE(sqlf);
                    }
                }
                if (!bc.exists("SELECT MGID FROM MISC_GODE_MST WHERE MGID='" + MGID + "'"))
                {
                    SQlcommandE_MST(sqlt);
                    IFExecution_SUCCESS = true;
                }
                else
                {
                    SQlcommandE_MST(sqlth + " WHERE MGID='" + MGID + "'");
                    IFExecution_SUCCESS = true;
                }
            }
            else//来自条码扫入时保存161031
            {
                int s1, s2;
                DataTable dty = bc.getdt("SELECT * FROM MISC_GODE_DET WHERE MGID='" + MGID + "'");
                if (dty.Rows.Count > 0)
                {
                    s1 = Convert.ToInt32(dty.Rows[dty.Rows.Count - 1]["SN"].ToString());
                    s2 = Convert.ToInt32(s1) + 1;
                }
                else
                {
                    s2 = 1;
                }
                SN = Convert.ToString(s2);
            }

            if (!bc.exists("SELECT MGID FROM MISC_GODE_DET WHERE MGID='" + MGID + "'"))
            {
                if (COME_FROM_DGV_OR_BARCODE == false)
                {
                    SQlcommandE_DET(sqlo);
                    SQlcommandE_GODE(sqlf);
                }
                SQlcommandE_MST(sqlt);
                IFExecution_SUCCESS = true;
            }
            else
            {

                if (COME_FROM_DGV_OR_BARCODE == false)
                {
                    SQlcommandE_DET(sqlo);
                    SQlcommandE_GODE(sqlf);
                }
                SQlcommandE_MST(sqlth + " WHERE MGID='" + MGID + "'");
                IFExecution_SUCCESS = true;
            }

        }
        #endregion
        #region SQlcommandE_DET
        protected void SQlcommandE_DET(string sql)
        {
            string year = DateTime.Now.ToString("yy");
            string month = DateTime.Now.ToString("MM");
            string day = DateTime.Now.ToString("dd");
            string varDate = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss").Replace("-", "/");
            SqlConnection sqlcon = bc.getcon();
            sqlcon.Open();
            SqlCommand sqlcom = new SqlCommand(sql, sqlcon);
            MGKEY = bc.numYMD(20, 12, "000000000001", "select * from MISC_GODE_DET", "MGKEY", "MG");
            sqlcom.Parameters.Add("@MGKEY", SqlDbType.VarChar, 20).Value = MGKEY;
            sqlcom.Parameters.Add("@MGID", SqlDbType.VarChar, 20).Value = MGID;
            sqlcom.Parameters.Add("@SN", SqlDbType.VarChar, 20).Value = SN;
            sqlcom.Parameters.Add("@REMARK", SqlDbType.VarChar, 20).Value = REMARK;
            sqlcom.Parameters.Add("@MAKERID", SqlDbType.VarChar, 20).Value = MAKERID;
            sqlcom.Parameters.Add("@DATE", SqlDbType.VarChar, 20).Value = varDate;
            sqlcom.Parameters.Add("@YEAR", SqlDbType.VarChar, 20).Value = year;
            sqlcom.Parameters.Add("@MONTH", SqlDbType.VarChar, 20).Value = month;
            sqlcom.Parameters.Add("@DAY", SqlDbType.VarChar, 20).Value = day;
            sqlcom.ExecuteNonQuery();
            sqlcon.Close();
        }
        #endregion
        #region SQlcommandE_MST
        protected void SQlcommandE_MST(string sql)
        {
            string year = DateTime.Now.ToString("yy");
            string month = DateTime.Now.ToString("MM");
            string day = DateTime.Now.ToString("dd");
            string varDate = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss").Replace("-", "/");
            SqlConnection sqlcon = bc.getcon();
            SqlCommand sqlcom = new SqlCommand(sql, sqlcon);
            sqlcon.Open();
            sqlcom.Parameters.Add("@MGID", SqlDbType.VarChar, 20).Value = MGID;
            sqlcom.Parameters.Add("@GODE_DATE", SqlDbType.VarChar, 20).Value = GODE_DATE;
            sqlcom.Parameters.Add("@GODE_MAKERID", SqlDbType.VarChar, 20).Value = GODE_MAKERID;
            sqlcom.Parameters.Add("@DATE", SqlDbType.VarChar, 20).Value = varDate;
            sqlcom.Parameters.Add("@MAKERID", SqlDbType.VarChar, 20).Value = MAKERID;
            sqlcom.Parameters.Add("@YEAR", SqlDbType.VarChar, 20).Value = year;
            sqlcom.Parameters.Add("@MONTH", SqlDbType.VarChar, 20).Value = month;
            sqlcom.Parameters.Add("@DAY", SqlDbType.VarChar, 20).Value = day;
            sqlcom.ExecuteNonQuery();
            sqlcon.Close();
        }
        #endregion
        #region SQlcommandE_GODE
        protected void SQlcommandE_GODE(string sql)
        {
            string year = DateTime.Now.ToString("yy");
            string month = DateTime.Now.ToString("MM");
            string day = DateTime.Now.ToString("dd");
            string varDate = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss").Replace("-", "/");
            SqlConnection sqlcon = bc.getcon();
            SqlCommand sqlcom = new SqlCommand(sql, sqlcon);
            sqlcon.Open();
            sqlcom.Parameters.Add("@GEKEY", SqlDbType.VarChar, 20).Value = MGKEY ;
            sqlcom.Parameters.Add("@GODEID", SqlDbType.VarChar, 20).Value = MGID;
            sqlcom.Parameters.Add("@SN", SqlDbType.VarChar, 20).Value = SN;
            sqlcom.Parameters.Add("@WAREID", SqlDbType.VarChar, 20).Value = WAREID;
            sqlcom.Parameters.Add("@GECOUNT", SqlDbType.VarChar, 20).Value = MGCOUNT;
            sqlcom.Parameters.Add("@SKU", SqlDbType.VarChar, 20).Value = SKU;
            sqlcom.Parameters.Add("@STORAGEID", SqlDbType.VarChar, 20).Value = "ST01";
            sqlcom.Parameters.Add("@SLID", SqlDbType.VarChar, 20).Value = "SL01";
            sqlcom.Parameters.Add("@ORKEY", SqlDbType.VarChar, 20).Value = ORKEY;
            sqlcom.Parameters.Add("@BATCHID", SqlDbType.VarChar, 20).Value = BARCODE;
            sqlcom.Parameters.Add("@DATE", SqlDbType.VarChar, 20).Value = varDate;
            sqlcom.Parameters.Add("@MAKERID", SqlDbType.VarChar, 20).Value = MAKERID;
            sqlcom.Parameters.Add("@YEAR", SqlDbType.VarChar, 20).Value = year;
            sqlcom.Parameters.Add("@MONTH", SqlDbType.VarChar, 20).Value = month;
            sqlcom.Parameters.Add("@DAY", SqlDbType.VarChar, 20).Value = day;
            sqlcom.ExecuteNonQuery();
            sqlcon.Close();
        }
        #endregion
    }
}

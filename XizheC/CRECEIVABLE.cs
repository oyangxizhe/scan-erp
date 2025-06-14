using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using System.Data.SqlClient;
using System.IO;
using System.Data.OleDb;
using XizheC;

namespace XizheC
{
    public class CRECEIVABLE
    {
        #region nature
        private string _ErrowInfo;
        public string ErrowInfo
        {

            set { _ErrowInfo = value; }
            get { return _ErrowInfo; }

        }
        private string _sql;
        public string sql
        {
            set { _sql = value; }
            get { return _sql; }
        }
        private string _RECEIVABLE_DATE;
        public string RECEIVABLE_DATE
        {
            set { _RECEIVABLE_DATE = value; }
            get { return _RECEIVABLE_DATE; }
        }
        private string _NO_TAX_UNIT_PRICE;
        public string NO_TAX_UNIT_PRICE
        {
            set { _NO_TAX_UNIT_PRICE = value; }
            get { return _NO_TAX_UNIT_PRICE; }
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
        private bool _IFExecutionSUCCESS;
        public bool IFExecution_SUCCESS
        {
            set { _IFExecutionSUCCESS = value; }
            get { return _IFExecutionSUCCESS; }
        }
        private bool _IF_IMPORT;
        public bool IF_IMPORT
        {
            set { _IF_IMPORT = value; }
            get { return _IF_IMPORT; }

        }
        private string _PICK_RECEIVABLE_MAKER;
        public string PICK_RECEIVABLE_MAKER
        {

            set { _PICK_RECEIVABLE_MAKER = value; }
            get { return _PICK_RECEIVABLE_MAKER; }

        }
        private decimal _BILL_ID;
        public decimal BILL_ID
        {
            set { _BILL_ID = value; }
            get { return _BILL_ID; }
        }
        private string _EMID;
        public string EMID
        {
            set { _EMID = value; }
            get { return _EMID; }
        }
        private string _REMARK;
        public string REMARK
        {
            set { _REMARK = value; }
            get { return _REMARK; }
        }
        private string _RCID;
        public string RCID
        {
            set { _RCID = value; }
            get { return _RCID; }
        }
        private string _PNID;
        public string PNID
        {
            set { _PNID = value; }
            get { return _PNID; }
        }
        #endregion
        #region sql
        string INKEY;
        string setsql = @"

SELECT 
C.ORDER_ID AS 订单编号,
E.CName AS 客户名称,
C.WNAME AS 品名,
C.PRODUCTION_COUNT AS 订单数量,
C.HAVE_TAX_UNIT_PRICE AS 含税单价,
CASE WHEN C.HAVE_TAX_UNIT_PRICE IS NOT NULL AND C.HAVE_TAX_UNIT_PRICE<>'' THEN C.HAVE_TAX_UNIT_PRICE*C.PRODUCTION_COUNT
ELSE 0
END  AS 订单金额,
B.RECEIVABLE_DATE  AS 收款日期,
B.NO_TAX_UNIT_PRICE AS 未税单价,
B.COUNT AS 数量,
B.TAX_RATE AS 税率,
B.NO_TAX_UNIT_PRICE*B.COUNT AS 未税金额,
B.NO_TAX_UNIT_PRICE*B.COUNT*(B.TAX_RATE/100) AS 税额,
B.NO_TAX_UNIT_PRICE*B.COUNT*(1+B.TAX_RATE/100) AS 含税金额,
A.DATE AS 制单日期,
F.ENAME AS 制单人
FROM RECEIVABLE_MST A
LEFT JOIN RECEIVABLE_DET B ON A.RCID=B.RCID 
LEFT JOIN PN_PRODUCTION_INSTRUCTIONS C ON A.PNID=C.PNID
LEFT JOIN PROJECT_INFO D ON C.PIID=D.PIID
LEFT JOIN CustomerInfo_MST E ON D.CUID=E.CUID
LEFT JOIN EMPLOYEEINFO F ON A.MAKERID=F.EMID


";
        string setsqlo = @"
SELECT 
C.ORDER_ID AS 订单编号,
E.CName AS 客户名称,
C.WNAME AS 品名,
C.PRODUCTION_COUNT AS 订单数量,
C.HAVE_TAX_UNIT_PRICE AS 含税单价,
CASE WHEN C.HAVE_TAX_UNIT_PRICE IS NOT NULL  AND C.HAVE_TAX_UNIT_PRICE<>'' THEN C.HAVE_TAX_UNIT_PRICE*C.PRODUCTION_COUNT
ELSE 0
END  AS 应收金额,
RTRIM(CONVERT(DECIMAL(18,2),SUM(B.NO_TAX_UNIT_PRICE*(1+B.TAX_RATE/100))))
 AS 实收金额,
CASE WHEN C.HAVE_TAX_UNIT_PRICE IS NOT NULL  AND C.HAVE_TAX_UNIT_PRICE<>'' THEN C.HAVE_TAX_UNIT_PRICE*C.PRODUCTION_COUNT-SUM(B.NO_TAX_UNIT_PRICE*(1+B.TAX_RATE/100))
ELSE 0
END  AS 待收金额,
CONVERT(varchar(12) , getdate(), 111 )  AS 截止日期
FROM RECEIVABLE_MST A
LEFT JOIN RECEIVABLE_DET B ON A.RCID=B.RCID 
LEFT JOIN PN_PRODUCTION_INSTRUCTIONS C ON A.PNID=C.PNID
LEFT JOIN PROJECT_INFO D ON C.PIID=D.PIID
LEFT JOIN CustomerInfo_MST E ON D.CUID=E.CUID
";


        string setsqlt = @"INSERT INTO RECEIVABLE_MST(

RCID,
PNID,
MAKERID,
DATE,
YEAR,
MONTH,
DAY
) VALUES 

(
@RCID,
@PNID,
@MAKERID,
@DATE,
@YEAR,
@MONTH,
@DAY

)

";
        string setsqlth = @"UPDATE RECEIVABLE_MST SET 
RCID=@RCID,
PNID=@PNID,
MAKERID=@MAKERID,
DATE=@DATE,
YEAR=@YEAR,
MONTH=@MONTH,
DAY=@DAY

";
        string setsqlf = @"INSERT INTO RECEIVABLE_DET(
RCKEY,
RCID,
SN,
RECEIVABLE_DATE,
NO_TAX_UNIT_PRICE,
TAX_RATE,
COUNT,
YEAR,
MONTH,
DAY
)
VALUES (
@RCKEY,
@RCID,
@SN,
@RECEIVABLE_DATE,
@NO_TAX_UNIT_PRICE,
@TAX_RATE,
@COUNT,
@YEAR,
@MONTH,
@DAY
)

";


        #endregion
        basec bc = new basec();
        DataTable dt = new DataTable();
        DataTable dto = new DataTable();
        ExcelToCSHARP etc = new ExcelToCSHARP();
        StringBuilder sqb = new StringBuilder();
        public CRECEIVABLE()
        {
            IFExecution_SUCCESS = true;
            sql = setsql;
            sqlo = setsqlo;
            sqlt = setsqlt;
            sqlth = setsqlth;
            sqlf = setsqlf;
        }
        public string GETID()
        {
            string year = DateTime.Now.ToString("yy");
            string month = DateTime.Now.ToString("MM");
            string day = DateTime.Now.ToString("dd");
            string varDate = DateTime.Now.ToString("yyy/MM/dd HH:mm:ss").Replace("-", "/");
            string v1 = bc.numYMD(12, 4, "0001", "select * from RECEIVABLE_NO", "RCID", "RC");
            string GETID = "";
            if (v1 != "Exceed Limited")
            {
                GETID = v1;
                bc.getcom("INSERT INTO RECEIVABLE_NO(RCID,DATE,YEAR,MONTH,DAY) VALUES ('" + v1 + "','" + varDate + "','" + year + "','" + month + "','" + day + "')");
            }
            return GETID;
        }
        #region GetTableInfo
        public DataTable GetTableInfo()
        {
            dt = new DataTable();
            dt.Columns.Add("项次", typeof(string));
            dt.Columns.Add("收款日期", typeof(string));
            dt.Columns.Add("未税单价", typeof(decimal));
            dt.Columns.Add("数量", typeof(decimal));
            dt.Columns.Add("税率", typeof(decimal));
            dt.Columns.Add("未税金额", typeof(decimal));
            dt.Columns.Add("税额", typeof(decimal));
            dt.Columns.Add("含税金额", typeof(decimal));
            dt.Columns.Add("待收金额", typeof(string));
            dt.Columns.Add("制单日期", typeof(string));
            dt.Columns.Add("制单人", typeof(string));
            return dt;
        }
        #endregion
        #region GetTableInfo_2
        public DataTable GetTableInfo_2()
        {
            dt = new DataTable();
            dt.Columns.Add("序号", typeof(string));
            dt.Columns.Add("订单编号", typeof(string));
            dt.Columns.Add("客户名称", typeof(string));
            dt.Columns.Add("品名", typeof(string));
            dt.Columns.Add("订单数量", typeof(decimal));
            dt.Columns.Add("含税单价", typeof(decimal));
            dt.Columns.Add("应收金额", typeof(decimal));
            dt.Columns.Add("实收金额", typeof(decimal));
            dt.Columns.Add("待收金额", typeof(decimal));
            dt.Columns.Add("截止日期", typeof(string));
            return dt;
        }
        #endregion
        #region GetTableInfo_3
        public DataTable GetTableInfo_3()
        {
            dt = new DataTable();
            dt.Columns.Add("项次", typeof(string));
            dt.Columns.Add("品号", typeof(string));
            dt.Columns.Add("品名", typeof(string));
            dt.Columns.Add("收款日期", typeof(decimal));
            dt.Columns.Add("未税金额", typeof(decimal));
            dt.Columns.Add("税率", typeof(string));
            dt.Columns.Add("日期", typeof(string));
            dt.Columns.Add("税金", typeof(string));
            dt.Columns.Add("含税金额", typeof(string));
            dt.Columns.Add("备注", typeof(string));
            return dt;
        }
        #endregion
        #region GetTableInfo_4
        public DataTable GetTableInfo_4()
        {
            dt = new DataTable();
            dt.Columns.Add("序号", typeof(string));
            dt.Columns.Add("编号", typeof(string));
            dt.Columns.Add("订单编号", typeof(string));
            dt.Columns.Add("客户名称", typeof(string));
            dt.Columns.Add("品名", typeof(string));
            dt.Columns.Add("订单数量", typeof(decimal));
            dt.Columns.Add("已收款日期", typeof(decimal));
            dt.Columns.Add("已出货数量", typeof(decimal));
            dt.Columns.Add("待出货数量", typeof(decimal));
            dt.Columns.Add("税率", typeof(string));
            dt.Columns.Add("截止日期", typeof(string));
            return dt;
        }
        #endregion
        #region save
        public void save(string TABLENAME_MST, string TABLENAME_DET, string COLUMNID,
           string IDVALUE, DataTable dt)
        {
            string year = DateTime.Now.ToString("yy");
            string month = DateTime.Now.ToString("MM");
            string day = DateTime.Now.ToString("dd");
            string varDate = DateTime.Now.ToString("yyy/MM/dd HH:mm:ss").Replace("-", "/");
            //string varMakerID;
            basec.getcoms("DELETE " + TABLENAME_DET + " WHERE " + COLUMNID + "='" + IDVALUE + "'");
            SQlcommandE(sqlf, dt);
            if (!bc.exists("SELECT " + COLUMNID + " FROM " + TABLENAME_DET + " WHERE " + COLUMNID + "='" + IDVALUE + "'"))
            {
                return;
            }
            else if (!bc.exists("SELECT " + COLUMNID + " FROM " + TABLENAME_MST + " WHERE " + COLUMNID + "='" + IDVALUE + "'"))
            {
                SQlcommandE(
                    sqlt,
                    IDVALUE);
            }
            else
            {
                SQlcommandE(sqlth + " WHERE " + COLUMNID + "='" + IDVALUE + "'", IDVALUE);
            }
        }
        #endregion
        #region SQlcommandE
        protected void SQlcommandE(string sql,DataTable dt)
        {
            string year = DateTime.Now.ToString("yy");
            string month = DateTime.Now.ToString("MM");
            string day = DateTime.Now.ToString("dd");
            string varDate = DateTime.Now.ToString("yyy/MM/dd HH:mm:ss").Replace("-", "/");
            int i=1;
            foreach (DataRow dr in dt.Rows)
            {
            
                SqlConnection sqlcon = bc.getcon();
                SqlCommand sqlcom = new SqlCommand(sql, sqlcon);
                INKEY = bc.numYMD(20, 12, "000000000001", "select * from RECEIVABLE_DET", "RCKEY", "RC");
                sqlcom.Parameters.Add("@RCKEY", SqlDbType.VarChar, 20).Value = INKEY;
                sqlcom.Parameters.Add("@RCID", SqlDbType.VarChar, 20).Value = RCID;
                sqlcom.Parameters.Add("@SN", SqlDbType.VarChar, 20).Value = i.ToString();
                sqlcom.Parameters.Add("@RECEIVABLE_DATE", SqlDbType.VarChar, 20).Value = dr["收款日期"].ToString();
                if (!string.IsNullOrEmpty(dr["未税单价"].ToString()))
                {
                    sqlcom.Parameters.Add("@NO_TAX_UNIT_PRICE", SqlDbType.VarChar, 20).Value = dr["未税单价"].ToString();
                }
                else
                {
                    sqlcom.Parameters.Add("@NO_TAX_UNIT_PRICE", SqlDbType.VarChar, 20).Value = DBNull.Value;
                }
                if (!string.IsNullOrEmpty(dr["税率"].ToString()))
                {
                    sqlcom.Parameters.Add("@TAX_RATE", SqlDbType.VarChar, 20).Value = dr["税率"].ToString();
                }
                else
                {
                    sqlcom.Parameters.Add("@TAX_RATE", SqlDbType.VarChar, 20).Value = DBNull.Value;
                }
                if (!string.IsNullOrEmpty(dr["数量"].ToString()))
                {
                    sqlcom.Parameters.Add("@COUNT", SqlDbType.VarChar, 20).Value = dr["数量"].ToString();
                }
                else
                {
                    sqlcom.Parameters.Add("@COUNT", SqlDbType.VarChar, 20).Value = DBNull.Value;
                }
                sqlcom.Parameters.Add("@YEAR", SqlDbType.VarChar, 20).Value = year;
                sqlcom.Parameters.Add("@MONTH", SqlDbType.VarChar, 20).Value = month;
                sqlcom.Parameters.Add("@DAY", SqlDbType.VarChar, 20).Value = day;
                sqlcon.Open();
                sqlcom.ExecuteNonQuery();
                sqlcon.Close();
                i = i + 1;
            }
        
        }
        #endregion
        #region SQlcommandE
        protected void SQlcommandE(string sql, string v1)
        {
            string year = DateTime.Now.ToString("yy");
            string month = DateTime.Now.ToString("MM");
            string day = DateTime.Now.ToString("dd");
            string varDate = DateTime.Now.ToString("yyy/MM/dd HH:mm:ss").Replace("-", "/");
            SqlConnection sqlcon = bc.getcon();
            SqlCommand sqlcom = new SqlCommand(sql, sqlcon);
            sqlcom.Parameters.Add("@RCID", SqlDbType.VarChar, 20).Value = v1;
            sqlcom.Parameters.Add("@PNID", SqlDbType.VarChar, 20).Value = PNID;
            sqlcom.Parameters.Add("@MAKERID", SqlDbType.VarChar, 20).Value = EMID;
            sqlcom.Parameters.Add("@DATE", SqlDbType.VarChar, 20).Value = varDate;
            sqlcom.Parameters.Add("@YEAR", SqlDbType.VarChar, 20).Value = year;
            sqlcom.Parameters.Add("@MONTH", SqlDbType.VarChar, 20).Value = month;
            sqlcom.Parameters.Add("@DAY", SqlDbType.VarChar, 20).Value = day;
            sqlcon.Open();
            sqlcom.ExecuteNonQuery();
            sqlcon.Close();
        }
        #endregion
        #region  GET_CALCULATE
        public DataTable GET_CALCULATE(DataTable dt) /*流水账余额TABLE*/
        {
            DataTable dtt = GetTableInfo();
            if (dt.Rows.Count > 0)
            {
                decimal SUM = 0;
                int i = 1;
                decimal d3 = 0;

                foreach (DataRow dr1 in dt.Rows)
                {
                    decimal d1 = 0, d2 = 0;
                    if (!string.IsNullOrEmpty(dr1["未税金额"].ToString()))
                    {
                        d1 = decimal.Parse(dr1["未税金额"].ToString());
                    }
                    if (!string.IsNullOrEmpty(dr1["税率"].ToString()))
                    {
                        d2 = decimal.Parse(dr1["税率"].ToString());
                    }
                    d3 = d3+ d1*(1+d2/100);
                    SUM = decimal.Parse(dr1["订单金额"].ToString()) - d3;
                    DataRow dr = dtt.NewRow();
                    dr["项次"] = i.ToString();
                    dr["收款日期"] = dr1["收款日期"].ToString();
                    if (!string.IsNullOrEmpty(dr1["未税单价"].ToString()))
                    {
                        dr["未税单价"] = dr1["未税单价"].ToString();
                    }
                    else
                    {
                        dr["未税单价"] = DBNull.Value;
                    }
                    if (!string.IsNullOrEmpty(dr1["税率"].ToString()))
                    {
                        dr["税率"] = dr1["税率"].ToString();
                    }
                    else
                    {
                        dr["税率"] = DBNull.Value;
                    }
                    if (!string.IsNullOrEmpty(dr1["数量"].ToString()))
                    {
                        dr["数量"] = dr1["数量"].ToString();
                    }
                    else
                    {
                        dr["数量"] = DBNull.Value;
                    }
                    dr["未税金额"] = (decimal.Parse(dr1["未税单价"].ToString()) * decimal.Parse(dr1["数量"].ToString())).ToString ("0.00");
                    dr["税额"] = (decimal.Parse(dr1["未税单价"].ToString()) * decimal.Parse(dr1["数量"].ToString())*decimal.Parse(dr1["税率"].ToString()) / 100).ToString ("0.00");
                    dr["含税金额"] = (decimal.Parse(dr1["未税单价"].ToString()) * decimal.Parse(dr1["数量"].ToString()) * (1 + decimal.Parse(dr1["税率"].ToString()) / 100)).ToString ("0.00");
                    dr["制单人"] = dr1["制单人"].ToString();
                    dr["制单日期"] = dr1["制单日期"].ToString();
                    dr["待收金额"] = SUM.ToString ("0.00");
                    dtt.Rows.Add(dr);
                    i = i + 1;
                }
            }
            return dtt;
        }
        #endregion
        #region RETURN_HAVE_ID_DT
        public DataTable RETURN_HAVE_ID_DT(DataTable dtx,bool IF_HAVE_ID)
        {
          DataTable dt = GetTableInfo_2();
          int i = 1;
          foreach (DataRow dr1 in dtx.Rows)
          {
              DataRow dr = dt.NewRow();
              dr["序号"] = i.ToString();
              if (IF_HAVE_ID)
              {
                  dr["编号"] = dr1["编号"].ToString();
              }
              dr["订单编号"] = dr1["订单编号"].ToString();
              dr["客户名称"] = dr1["客户名称"].ToString();
              dr["品名"] = dr1["品名"].ToString();
              dr["订单数量"] = dr1["订单数量"].ToString();
              if (!string.IsNullOrEmpty(dr1["含税单价"].ToString()))
              {
                  dr["含税单价"] = dr1["含税单价"].ToString();
              }
              else
              {
                  dr["含税单价"] = DBNull.Value;
              }
              dr["应收金额"] = dr1["应收金额"].ToString();
              dr["实收金额"] = dr1["实收金额"].ToString();
              dr["待收金额"] = dr1["待收金额"].ToString();
              dr["截止日期"] = dr1["截止日期"].ToString();
              dt.Rows.Add(dr);
              i = i + 1;
          }
            return dt;
        }
        #endregion

    
    }

}

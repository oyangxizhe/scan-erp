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
using Excel = Microsoft.Office.Interop.Excel;

namespace XizheC
{
    public class CORDER
    {
        basec bc = new basec();
        #region nature
        private string _ORID;
        public string ORID
        {
            set { _ORID = value; }
            get { return _ORID; }
        }
        private string _CUID;
        public string CUID
        {
            set { _CUID = value; }
            get { return _CUID; }
        }
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
        private string _MAKERID;
        public string MAKERID
        {
            set { _MAKERID = value; }
            get { return _MAKERID; }
        }
        private string _ORDER_DATE;
        public string ORDER_DATE
        {
            set { _ORDER_DATE = value; }
            get { return _ORDER_DATE; }
        }
        private string _CUSTOMER_ORID;
        public string CUSTOMER_ORID
        {
            set { _CUSTOMER_ORID = value; }
            get { return _CUSTOMER_ORID; }
        }
        private string _CO_COUNT;
        public string CO_COUNT
        {
            set { _CO_COUNT = value; }
            get { return _CO_COUNT; }

        }
        private bool _IFExecutionSUCCESS;
        public bool IFExecution_SUCCESS
        {
            set { _IFExecutionSUCCESS = value; }
            get { return _IFExecutionSUCCESS; }
        }
        private string _WO_COUNT;
        public string WO_COUNT
        {
            set { _WO_COUNT = value; }
            get { return _WO_COUNT; }

        }
        private string _STORAGE_COUNT;
        public string STORAGE_COUNT
        {
            set { _STORAGE_COUNT = value; }
            get { return _STORAGE_COUNT; }
        }
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
        private string _UNITPRICE;
        public string UNITPRICE
        {
            set { _UNITPRICE = value; }
            get { return _UNITPRICE; }

        }

        private string _WEIGHT;
        public string WEIGHT
        {
            set { _WEIGHT = value; }
            get { return _WEIGHT; }

        }

        #endregion
        #region sql
        string setsql = @"

with x1 as (SELECT 
A.ORKEY AS 索引,
A.SN ,
A.ORID AS ORID,
A.ORID AS 订单号,
C.CUID AS 客户编号,
C.CUID AS CUID,
C.CUSTOMER_ID AS 客户代码,
C.CNAME AS 客户名称,
A.CUSTOMER_ORID AS 客户订单号,
A.SN AS 项次,
A.WAREID AS 型号,
A.WareID AS WAREID,
A.MOLDNO AS 模具编号,
A.WNAME AS 品名,
A.WNAME AS WNAME,
A.OCOUNT AS 数量 ,
A.UNITPRICE AS 单价 ,
A.CURRENCY AS 币别,
A.TAXRATE AS 税率,
A.UNITPRICE*A.OCOUNT AS 未税金额,
A.TAXRATE/100*A.UNITPRICE*OCOUNT AS 税额,
A.UNITPRICE*(1+(A.TAXRATE)/100)*OCOUNT AS 含税金额,
A.MATERIAL AS 材料,
(SELECT MAID FROM MATERIAL  WHERE MATERIAL=A.MATERIAL) AS MAID,
(SELECT BASE FROM CustomerInfo_DET WHERE CUID=D.CUID AND MAID=(SELECT MAID FROM MATERIAL WHERE MATERIAL =A.MATERIAL )) AS 基数,
A.SKU AS 单位,
A.WEIGHT AS 重量,   
D.ORDER_DATE AS 下单日期,
A.DELIVERY_DATE AS  订单交期,
D.SALEID AS 业务员工号,
(SELECT ENAME FROM EMPLOYEEINFO WHERE EMID=D.SALEID ) AS 业务员,
A.LEADDAYS AS 前置天数,
A.NEEDDATE AS 需求日期 ,
A.STOCK_PREPOSITION AS 备料前置,
A.REMARK AS 备注,
E.ADDRESS AS 公司地址,
E.CONTACT AS 联系人,
E.PHONE AS 联系电话,
D.Date,
D.MAKERID AS MAKERID,
CASE WHEN (select ISNULL(COUNT(*),0) from ORDER_BARCODE WHERE ORKEY =A.ORKEY)>0 THEN '已打印'
ELSE '未打印'
END 打印状态,
ocount as 订单数量,

ISNULL((
SELECT RTRIM(CONVERT(DECIMAL(18,2),SUM(A2.MRCOUNT))) FROM SELLTABLE_DET A1 
LEFT JOIN MateRe A2 ON A1.SEKEY=A2.MRKEY WHERE A1.ORID=A.ORID AND A1.SN=A.SN  GROUP BY A1.ORID,A1.SN 
),0) AS 累计销货,
OCOUNT-
ISNULL((
SELECT 
RTRIM(CONVERT(DECIMAL(18,2),SUM(A2.MRCOUNT))) FROM SELLTABLE_DET A1 
LEFT JOIN MateRe A2 ON A1.SEKEY=A2.MRKEY WHERE A1.ORID=A.ORID AND A1.SN=A.SN  GROUP BY A1.ORID,A1.SN 
),0) AS 未销数量

FROM ORDER_DET A 
LEFT JOIN ORDER_MST D ON A.ORID=D.ORID
LEFT JOIN CUSTOMERINFO_MST C ON D.CUID=C.CUID
LEFT JOIN (SELECT * FROM CustomerInfo_DET A1 WHERE CUKEY IN (SELECT MIN(CUKEY) FROM CUSTOMERINFO_DET WHERE CUID=A1.CUID) ) E 
ON E.CUID=C.CUID)

,x2 as (
select *,case when x1.未销数量=0 then '已发货'
when x1.未销数量>0 and x1.未销数量<x1.订单数量 then '部分发货'
else '未发货' end as 订单状态 from x1  )
select * from x2 

";
        string setsqlo = @"
INSERT INTO ORDER_DET
(
ORKEY,
ORID,
SN,
WAREID,
MOLDNO,
WEIGHT,
WNAME,
MATERIAL,
OCOUNT,
SKU,
DELIVERY_DATE,
CUSTOMER_ORID,
YEAR,
MONTH,
DAY
)
VALUES
(
@ORKEY,
@ORID,
@SN,
@WAREID,
@MOLDNO,
@WEIGHT,
@WNAME,
@MATERIAL,
@OCOUNT,
@SKU,
@DELIVERY_DATE,
@CUSTOMER_ORID,
@YEAR,
@MONTH,
@DAY

)

";

        string setsqlt = @"

INSERT INTO ORDER_MST
(
ORID,
CUID,
ORDER_DATE,
CUSTOMER_ORID,
DATE,
MAKERID,
YEAR,
MONTH,
DAY
)
VALUES
(
@ORID,
@CUID,
@ORDER_DATE,
@CUSTOMER_ORID,
@DATE,
@MAKERID,
@YEAR,
@MONTH,
@DAY
)
";
        string setsqlth = @"
UPDATE ORDER_MST SET 
CUID=@CUID,
ORDER_DATE=@ORDER_DATE,
CUSTOMER_ORID=@CUSTOMER_ORID,
DATE=@DATE
";
        string setsqlf = @"
INSERT INTO ORDER_BARCODE
(
BARCODE,
ORKEY,
MAKERID,
DATE,
YEAR,
MONTH,
DAY
)
VALUES
(
@BARCODE,
@ORKEY,
@MAKERID,
@DATE,
@YEAR,
@MONTH,
@DAY
)
";
        string setsqlfi = @"

";
        #endregion
        DataTable dtx2 = new DataTable();
        DataTable dt4 = new DataTable();
        DataTable dt = new DataTable();
        CFileInfo cfileinfo = new CFileInfo();
        int i,j;
        public CORDER()
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
            string v1 = bc.numYM(10, 4, "0001", "SELECT * FROM ORDER_MST", "ORID", "OR");
            string GETID = "";
            if (v1 != "Exceed Limited")
            {
                GETID = v1;
            }
            return GETID;
        }
        public bool IFNOALLOW_DELETE_ORID(string ORID)
        {
            bool b = false;
            if (bc.exists("SELECT * FROM CO_ORDER WHERE ORID='" + ORID + "'"))
            {
                b = true;
                ErrowInfo = "该订单号已经存在厂内订单中，不允许修改与删除！";
            }
            return b;
        }
        #region GET_TOTAL_ORDER
        public  DataTable GET_TOTAL_ORDER()
        {
            DataTable dtt = new DataTable();
            dtt.Columns.Add("索引", typeof(string));
            dtt.Columns.Add("订单号", typeof(string));
            dtt.Columns.Add("项次", typeof(string));
            dtt.Columns.Add("ID", typeof(string));
            dtt.Columns.Add("料号", typeof(string));
            dtt.Columns.Add("品名", typeof(string));
            dtt.Columns.Add("规格", typeof(string));
            dtt.Columns.Add("客户料号", typeof(string));
            dtt.Columns.Add("订单数量", typeof(decimal));
            dtt.Columns.Add("累计销货数量", typeof(decimal));
            dtt.Columns.Add("累计销退数量", typeof(decimal));
            dtt.Columns.Add("订单未结数量", typeof(decimal), "订单数量-累计销货数量+累计销退数量");
            dtt.Columns.Add("状态", typeof(string));
            dtt.Columns.Add("订单交期", typeof(string));

            DataTable dtx1 = bc.getdt("SELECT * FROM ORDER_DET ");
            if (dtx1.Rows.Count > 0)
            {
                for (i = 0; i < dtx1.Rows.Count; i++)
                {
                    DataRow dr = dtt.NewRow();
                    dr["索引"] = dtx1.Rows[i]["ORKEY"].ToString();
                    dr["订单号"] = dtx1.Rows[i]["ORID"].ToString();
                    dr["项次"] = dtx1.Rows[i]["SN"].ToString();
                    dr["ID"] = dtx1.Rows[i]["WAREID"].ToString();
                    dtx2 = bc.getdt("select * from wareinfo where wareid='" + dtx1.Rows[i]["WAREID"].ToString() + "'");
                    dr["料号"] = dtx2.Rows[0]["CO_WAREID"].ToString();
                    dr["品名"] = dtx2.Rows[0]["WNAME"].ToString();
                    dr["规格"] = dtx2.Rows[0]["SPEC"].ToString();
                    dr["客户料号"] = dtx2.Rows[0]["CWAREID"].ToString();
                    dr["订单数量"] = dtx1.Rows[i]["OCOUNT"].ToString();
                    dr["累计销货数量"] = 0;
                    dr["累计销退数量"] = 0;
                    dr["订单交期"] = dtx1.Rows[i]["DELIVERYDATE"].ToString();
                    if (dtx1.Rows[i]["ORDERSTATUS_DET"].ToString() == "OPEN")
                    {
                        dr["状态"] = "OPEN";
                    }
                    else if (dtx1.Rows[i]["ORDERSTATUS_DET"].ToString() == "PROGRESS")
                    {
                        dr["状态"] = "部分出货";
                    }
                    else if (dtx1.Rows[i]["ORDERSTATUS_DET"].ToString() == "DELAY")
                    {
                        dr["状态"] = "DELAY";
                    }
                    else
                    {
                        dr["状态"] = "已出货";
                    }

                    dtt.Rows.Add(dr);
                }

            }

            DataTable dtx4 = bc.getdt(@"
SELECT
A.ORID AS ORID,
A.SN AS SN,
B.WAREID AS WAREID,
SUM(B.MRCOUNT) AS MRCOUNT 
FROM SELLTABLE_DET A 
LEFT JOIN MATERE B ON A.SEKEY=B.MRKEY 
GROUP BY A.ORID,A.SN,B.WAREID
");
            if (dtx4.Rows.Count > 0)
            {
                for (i = 0; i < dtx4.Rows.Count; i++)
                {
                    for (j = 0; j < dtt.Rows.Count; j++)
                    {
                        if (dtt.Rows[j]["订单号"].ToString() == dtx4.Rows[i]["ORID"].ToString() && dtt.Rows[j]["项次"].ToString() == dtx4.Rows[i]["SN"].ToString())
                        {
                            dtt.Rows[j]["累计销货数量"] = dtx4.Rows[i]["MRCOUNT"].ToString();
                            break;
                        }

                    }
                }

            }
            DataTable dtx6 = bc.getdt(@"
SELECT 
A.ORID AS ORID,
A.SN AS SN,
B.WAREID AS WAREID,
SUM(B.GECOUNT) AS GECOUNT
FROM SELLRETURN_DET A 
LEFT JOIN GODE B ON A.SRKEY=B.GEKEY  
GROUP BY 
A.ORID,
A.SN,
B.WAREID

");
            if (dtx6.Rows.Count > 0)
            {
                for (i = 0; i < dtx6.Rows.Count; i++)
                {
                    for (j = 0; j < dtt.Rows.Count; j++)
                    {
                        if (dtt.Rows[j]["订单号"].ToString() == dtx6.Rows[i]["ORID"].ToString() && dtt.Rows[j]["项次"].ToString() == dtx6.Rows[i]["SN"].ToString())
                        {
                            dtt.Rows[j]["累计销退数量"] = dtx6.Rows[i]["GECOUNT"].ToString();
                            break;
                        }

                    }
                }

            }

            return dtt;
        }
        #endregion
        #region GET_ORDER_PROGRESS_COUNT
        public string GET_ORDER_PROGRESS_COUNT(string WAREID,string ORKEY)
        {
            string v = "0";
            DataView dv = new DataView(GET_TOTAL_ORDER());
            dv.RowFilter = "状态 NOT IN ('已出货') AND ID='" + WAREID + "' AND 索引 NOT IN ('"+ORKEY +"')";
            DataTable dt = dv.ToTable();
            if (dt.Rows.Count > 0)
            {

                v = dt.Compute("SUM(订单未结数量)", "").ToString();

            }
            return v;
        }
        #endregion
        #region UPDATE_ORDER_STATUS
        public void UPDATE_ORDER_STATUS(string ORID)
        {
            DataView dv = new DataView(GET_TOTAL_ORDER());
            dv.RowFilter = "订单号='" + ORID + "'";
            DataTable dt = dv.ToTable();
            if (dt.Rows.Count > 0)
            {

                foreach (DataRow dr in dt.Rows)
                {
                    decimal d0 = decimal.Parse(dr["订单数量"].ToString());
                    decimal d1 = decimal.Parse(dr["累计销货数量"].ToString());
                    decimal d2 = decimal.Parse(dr["累计销退数量"].ToString());

                   if (decimal.Parse (dr["订单未结数量"].ToString()) ==0)
                    {
                        basec.getcoms("UPDATE ORDER_DET SET ORDERSTATUS_DET='CLOSE' WHERE ORID='" + ORID + "' AND SN='" +dr["项次"].ToString () + "'");
                    }
                    else if (bc.JuageCurrentDateIFAboveDeliveryDate(DateTime.Now.ToString(), dr["订单交期"].ToString()))
                    {
                        basec.getcoms("UPDATE ORDER_DET SET ORDERSTATUS_DET='DELAY' WHERE ORID='" + ORID + "' AND SN='" + dr["项次"].ToString() + "'");
                    }
                    else if (d1 - d2 > 0)
                    {
                        basec.getcoms("UPDATE ORDER_DET SET ORDERSTATUS_DET='PROGRESS' WHERE ORID='" + ORID + "' AND SN='" + dr["项次"].ToString() + "'");
                    }
                    else
                    {
                        basec.getcoms("UPDATE ORDER_DET SET ORDERSTATUS_DET='OPEN' WHERE ORID='" + ORID + "' AND SN='" + dr["项次"].ToString() + "'");
                    }
                }
                if (bc.JuageOrderOrPurchaseStatus(ORID, 0))
                {
                    basec.getcoms("UPDATE ORDER_MST SET ORDERSTATUS_MST='CLOSE' WHERE ORID='" + ORID + "'");

                }
                else if (bc.JuageCurrentDateIFAboveDeliveryDate(ORID, 0))
                {
                    basec.getcoms("UPDATE ORDER_MST SET ORDERSTATUS_MST='DELAY' WHERE ORID='" + ORID + "'");
                }
                else if (JUAGE_REALTY_IFHAVE_SELLCOUNT(ORID))
                {

                    basec.getcoms("UPDATE ORDER_MST SET ORDERSTATUS_MST='PROGRESS' WHERE ORID='" + ORID + "'");
                }
                else
                {
                    basec.getcoms("UPDATE ORDER_MST SET ORDERSTATUS_MST='OPEN' WHERE ORID='" + ORID + "'");

                }
            }
        }
        #endregion
        #region JUAGE_REALTY_IFHAVE_SELLCOUNT
        public bool  JUAGE_REALTY_IFHAVE_SELLCOUNT(string ORID)
        {
            bool b = false;
            DataView dv = new DataView(GET_TOTAL_ORDER());
            dv.RowFilter = "订单号='" + ORID + "'";
            DataTable dt = dv.ToTable();
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {

                    decimal d1 = decimal.Parse(dr["累计销货数量"].ToString());
                    decimal d2 = decimal.Parse(dr["累计销退数量"].ToString());
                    if (d1 - d2 > 0)
                    {
                        b = true;
                        break;
                    }

                }
            }
            return b;
        }
        #endregion
        #region JUAGE_ORDER_IF_HAVE_NO_AUDIT
        public bool JUAGE_ORDER_IF_HAVE_NO_AUDIT(string ORID)
        {
            bool b = false;
            string s2 = bc.getOnlyString("SELECT IF_AUDIT FROM ORDER_MST WHERE ORID='" +ORID  + "'");
            if (s2 != "Y")
            {
                b = true;
                ErrowInfo = "此订单未审核，不能进行相关操作！";
            }
            return b;
        }
        #endregion
  
        #region save
        public void save(DataGridView dgv)
        {
            string year = DateTime.Now.ToString("yy");
            string month = DateTime.Now.ToString("MM");
            string day = DateTime.Now.ToString("dd");
            string varDate = DateTime.Now.ToString("yyy/MM/dd HH:mm:ss").Replace("-", "/");
            if (!bc.exists("SELECT ORID FROM ORDER_DET WHERE ORID='" + ORID + "'"))
            {
                SQlcommandE_MST(sqlt);
                SQlcommandE_DET(dgv, sqlo);
              
                IFExecution_SUCCESS = true;
            }
            else
            {
                SQlcommandE_DET(dgv, sqlo);
                SQlcommandE_MST(sqlth+" WHERE ORID='"+ORID+"'");
                IFExecution_SUCCESS = true;
            }
        }
        #endregion
        #region SQlcommandE_DET
        protected void SQlcommandE_DET(DataGridView dgv, string sql)
        {
            string year = DateTime.Now.ToString("yy");
            string month = DateTime.Now.ToString("MM");
            string day = DateTime.Now.ToString("dd");
            string varDate = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss").Replace("-", "/");
            basec.getcoms("DELETE ORDER_DET WHERE ORID='" + ORID + "'");
            for (i = 0; i < dgv.Rows.Count; i++)
            {
                if (dgv["型号", i].FormattedValue.ToString() == "")
                {

                }
                else
                {
                    //返回含重量与模具编号的数据
                    DataTable dtx = new getWeight(ORID, dgv["型号", i].FormattedValue.ToString(), dgv["材料", i].FormattedValue.ToString(), dgv["品名", i].FormattedValue.ToString()).ReturnWeight();
                    SqlConnection sqlcon = bc.getcon();
                    sqlcon.Open();
                    SqlCommand sqlcom = new SqlCommand(sql, sqlcon);
                    ORKEY = bc.numYMD(20, 12, "000000000001", "SELECT * FROM ORDER_DET", "ORKEY", "OR");
                    sqlcom.Parameters.Add("@ORKEY", SqlDbType.VarChar, 20).Value = ORKEY;
                    sqlcom.Parameters.Add("@ORID", SqlDbType.VarChar, 20).Value = ORID;
                    sqlcom.Parameters.Add("@SN", SqlDbType.VarChar, 20).Value = dgv["项次", i].FormattedValue.ToString();
                    sqlcom.Parameters.Add("@WAREID", SqlDbType.VarChar, 20).Value = dgv["型号", i].FormattedValue.ToString();

                    if(dtx.Rows.Count>0)
                    {
                        sqlcom.Parameters.Add("@MOLDNO", SqlDbType.VarChar, 20).Value = dtx.Rows[0]["moldno"].ToString();
                        sqlcom.Parameters.Add("@WEIGHT", SqlDbType.VarChar, 20).Value = dtx.Rows[0]["weight"].ToString();
                    }
                    else
                    {
                        sqlcom.Parameters.Add("@MOLDNO", SqlDbType.VarChar, 20).Value = "";
                        sqlcom.Parameters.Add("@WEIGHT", SqlDbType.VarChar, 20).Value = "";
                    }
                  

                    sqlcom.Parameters.Add("@WNAME", SqlDbType.VarChar, 50).Value = dgv["品名", i].FormattedValue.ToString();
                    sqlcom.Parameters.Add("@MATERIAL", SqlDbType.VarChar, 20).Value = dgv["材料", i].FormattedValue.ToString();
                    sqlcom.Parameters.Add("@OCOUNT", SqlDbType.VarChar, 20).Value = dgv["数量", i].FormattedValue.ToString();
                    sqlcom.Parameters.Add("@SKU", SqlDbType.VarChar, 20).Value = dgv["单位", i].FormattedValue.ToString();
                    sqlcom.Parameters.Add("@DELIVERY_DATE", SqlDbType.VarChar, 20).Value = dgv["订单交期", i].FormattedValue.ToString();
                    sqlcom.Parameters.Add("@CUSTOMER_ORID", SqlDbType.VarChar, 20).Value = dgv["客户订单号", i].FormattedValue.ToString();
                    sqlcom.Parameters.Add("@MAKERID", SqlDbType.VarChar, 20).Value = MAKERID;
                    sqlcom.Parameters.Add("@DATE", SqlDbType.VarChar, 20).Value = varDate;
                    sqlcom.Parameters.Add("@YEAR", SqlDbType.VarChar, 20).Value = year;
                    sqlcom.Parameters.Add("@MONTH", SqlDbType.VarChar, 20).Value = month;
                    sqlcom.Parameters.Add("@DAY", SqlDbType.VarChar, 20).Value = day;
                    sqlcom.ExecuteNonQuery();
                    sqlcon.Close();
                }
            }
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
            sqlcom.Parameters.Add("ORID", SqlDbType.VarChar, 20).Value = ORID;
            sqlcom.Parameters.Add("CUID", SqlDbType.VarChar, 20).Value = CUID;
            sqlcom.Parameters.Add("ORDER_DATE", SqlDbType.VarChar, 20).Value = ORDER_DATE;
            sqlcom.Parameters.Add("CUSTOMER_ORID", SqlDbType.VarChar, 20).Value =CUSTOMER_ORID;
            sqlcom.Parameters.Add("@DATE", SqlDbType.VarChar, 20).Value = varDate;
            sqlcom.Parameters.Add("@MAKERID", SqlDbType.VarChar, 20).Value = MAKERID;
            sqlcom.Parameters.Add("@YEAR", SqlDbType.VarChar, 20).Value = year;
            sqlcom.Parameters.Add("@MONTH", SqlDbType.VarChar, 20).Value = month;
            sqlcom.Parameters.Add("@DAY", SqlDbType.VarChar, 20).Value = day;
            sqlcom.ExecuteNonQuery();
            sqlcon.Close();
        }
        #endregion
        #region GetTableInfo
        public DataTable GetTableInfo()
        {
            dt = new DataTable();
            dt.Columns.Add("项次", typeof(string));
            dt.Columns.Add("型号", typeof(string));
            dt.Columns.Add("模具编号", typeof(string));
            dt.Columns.Add("品名", typeof(string));
            dt.Columns.Add("材料", typeof(string));
            dt.Columns.Add("数量", typeof(string));
            dt.Columns.Add("单位", typeof(string));
            dt.Columns.Add("订单交期", typeof(string));
            dt.Columns.Add("客户订单号", typeof(string));
            return dt;
        }
        #endregion
        #region GetTableInfo_SEARCH
        public DataTable GetTableInfo_SEARCH()
        {
            dt = new DataTable();
            dt.Columns.Add("选取", typeof(bool));
            dt.Columns.Add("序号", typeof(string));
            dt.Columns.Add("订单号", typeof(string));
            dt.Columns.Add("项次", typeof(string));
            dt.Columns.Add("客户名称", typeof(string));
            dt.Columns.Add("下单日期", typeof(string));
            dt.Columns.Add("客户订单号", typeof(string));
            dt.Columns.Add("型号", typeof(string));
            dt.Columns.Add("模具编号", typeof(string));
            dt.Columns.Add("品名", typeof(string));
            dt.Columns.Add("材料", typeof(string));
            dt.Columns.Add("数量", typeof(string));
            dt.Columns.Add("单位", typeof(string));
            dt.Columns.Add("订单交期", typeof(string));
            dt.Columns.Add("打印状态", typeof(string));
            dt.Columns.Add("重量", typeof(string));
            dt.Columns.Add("订单状态", typeof(string));
            return dt;
        }
        #endregion
        #region RETURN_DT
        public DataTable RETURN_DT(DataTable dtt)
        {
            int i = 1;
            DataTable dt = GetTableInfo_SEARCH();
            foreach (DataRow dr1 in dtt.Rows)
            {
                DataRow dr = dt.NewRow();
                dr["选取"] = false;
                dr["序号"] = i.ToString();
                dr["订单号"] = dr1["订单号"].ToString();
                dr["项次"] = dr1["项次"].ToString();
                dr["客户名称"] = dr1["客户名称"].ToString();
                dr["下单日期"] = dr1["下单日期"].ToString();
                dr["客户订单号"] = dr1["客户订单号"].ToString();
                dr["型号"] = dr1["型号"].ToString();
                dr["模具编号"] = dr1["模具编号"].ToString();
                dr["品名"] = dr1["品名"].ToString();
                dr["材料"] = dr1["材料"].ToString();
                dr["数量"] = dr1["数量"].ToString();
                dr["单位"] = dr1["单位"].ToString();
                dr["订单交期"] = dr1["订单交期"].ToString();
                dr["打印状态"] = dr1["打印状态"].ToString();
                dr["重量"] = dr1["重量"].ToString();
                dr["订单状态"] = dr1["订单状态"].ToString();
                dt.Rows.Add(dr);
                i = i + 1;
            }
            return dt;
        }
        #endregion
        #region ExcelPrint
  
        #endregion
        #region ExcelPrint_40X30
      
        #endregion
        #region SQlcommandE
        public void SQlcommandE()
        {
            string year = DateTime.Now.ToString("yy");
            string month = DateTime.Now.ToString("MM");
            string day = DateTime.Now.ToString("dd");
            string varDate = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss").Replace("-", "/");
            SqlConnection sqlcon = bc.getcon();
            SqlCommand sqlcom = new SqlCommand(sqlf, sqlcon);
            sqlcon.Open();
            sqlcom.Parameters.Add("@BARCODE", SqlDbType.VarChar, 20).Value = BARCODE;
            sqlcom.Parameters.Add("@ORKEY", SqlDbType.VarChar, 20).Value = ORKEY;
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

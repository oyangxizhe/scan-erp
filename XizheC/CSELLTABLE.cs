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
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;

namespace XizheC
{
    public class CSELLTABLE
    {
        basec bc = new basec();
        #region nature
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
        private string _SN;
        public string SN
        {
            set { _SN = value; }
            get { return _SN; }
        }
        private string _SEID;
        public string SEID
        {
            set { _SEID = value; }
            get { return _SEID; }
        }
        private string _CUID;
        public string CUID
        {
            set { _CUID = value; }
            get { return _CUID; }
        }
        private string _CNAME;
        public string CNAME
        {
            set { _CNAME = value; }
            get { return _CNAME; }
        }
        private string _BARCODE;
        public string BARCODE
        {
            set { _BARCODE = value; }
            get { return _BARCODE; }
        }
        private string _SEKEY;
        public string SEKEY
        {
            set { _SEKEY = value; }
            get { return _SEKEY; }
        }
        private string _UNITRPICE;
        public string UNITPRICE
        {
            set { _UNITRPICE = value; }
            get { return _UNITRPICE; }

        }
        private string _MAKERID;
        public string MAKERID
        {
            set { _MAKERID = value; }
            get { return _MAKERID; }
        }
        private string _SELLTABLE_DATE;
        public string SELLTABLE_DATE
        {
            set { _SELLTABLE_DATE = value; }
            get { return _SELLTABLE_DATE; }
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
        private string _SELLDATE;
        public string SELLDATE
        {
            set { _SELLDATE = value; }
            get { return _SELLDATE; }
        }
        private string _SELLERID;
        public string SELLERID
        {
            set { _SELLERID = value; }
            get { return _SELLERID; }
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
        private string _PHONE;
        public string PHONE
        {
            set { _PHONE = value; }
            get { return _PHONE; }
        }
        private string _CONTACT;
        public string CONTACT
        {
            set { _CONTACT = value; }
            get { return _CONTACT; }

        }
        private string _SEND_ADDRESS;
        public string SEND_ADDRESS
        {
            set { _SEND_ADDRESS = value; }
            get { return _SEND_ADDRESS; }

        }
        private string _BASE;
        public string BASE
        {
            set { _BASE = value; }
            get  { return _BASE; }

        }
        private string _WEIGHT;
        public string WEIGHT
        {
            set { _WEIGHT = value; }
            get { return _WEIGHT; }

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

        #endregion
        #region sql
        string setsql = @"

SELECT
ROW_NUMBER() OVER (ORDER BY A.SEKEY ASC)  AS  序号,
A.SEID AS 销货单号,
A.ORID as 订单号, 
A.SN as 项次,
C.Customer_ORID AS 客户订单号,
C.WareID as 型号,
C.WNAME AS 品名,
C.MATERIAL AS 材料,
C.SKU AS 单位,
C.OCOUNT AS 订单数量,
C.UNITPRICE AS 单价,
A.BASE AS 基数,
A.WEIGHT AS 重量,
CAST(ROUND(E.MRCount,2) AS DECIMAL(18,2)) as 销货数量 ,
CAST(ROUND(E.MRCOUNT*A.UNITPRICE,4) AS DECIMAL(18,2)) AS 金额,
(
SELECT 
RTRIM(CONVERT(DECIMAL(18,2),SUM(A2.MRCOUNT*A1.UNITPRICE))) FROM SELLTABLE_DET A1 
LEFT JOIN MateRe A2 ON A1.SEKEY=A2.MRKEY WHERE A1.SEID=A.SEID GROUP BY A1.SEID
) AS 销货单销货金额,
(
SELECT 
RTRIM(CONVERT(DECIMAL(18,2),SUM(A2.MRCOUNT*A1.UNITPRICE))) FROM SELLTABLE_DET A1 
LEFT JOIN MateRe A2 ON A1.SEKEY=A2.MRKEY WHERE A1.ORID=A.ORID GROUP BY A1.ORID
) AS 订单销货金额,
(
SELECT 
RTRIM(CONVERT(DECIMAL(18,2),SUM(A2.MRCOUNT))) FROM SELLTABLE_DET A1 
LEFT JOIN MateRe A2 ON A1.SEKEY=A2.MRKEY WHERE A1.ORID=A.ORID AND A1.SN=A.SN  GROUP BY A1.ORID,A1.SN 
) AS 累计销货,
C.OCOUNT-
(
SELECT 
RTRIM(CONVERT(DECIMAL(18,2),SUM(A2.MRCOUNT))) FROM SELLTABLE_DET A1 
LEFT JOIN MateRe A2 ON A1.SEKEY=A2.MRKEY WHERE A1.ORID=A.ORID AND A1.SN=A.SN  GROUP BY A1.ORID,A1.SN 
) AS 未销数量,
D.CName as 客户名称, 
F.CONTACT AS 联系人,
F.PHONE AS 联系电话,
F.SEND_ADDRESS AS 送货地址,
E.BatchID AS 批号,
(SELECT ENAME FROM EMPLOYEEINFO WHERE EMID=F.SELLERID )  AS 销货员,
F.SELLDATE AS 销货日期
from SELLTABLE_DET A 
LEFT JOIN ORDER_DET C ON A.ORID=C.ORID AND A.SN=C.SN
LEFT JOIN MATERE E ON A.SEKEY=E.MRKEY
LEFT JOIN WAREINFO B ON E.WAREID=B.WAREID
LEFT JOIN SELLTABLE_MST F ON A.SEID=F.SEID
LEFT JOIN Order_MST H ON C.ORID =H.ORID 
LEFT JOIN CUSTOMERINFO_MST D ON H.CUID=D.CUID
";
        string setsqlo = @"
INSERT INTO SELLTABLE_DET
(
SEKEY,
SEID,
ORID,
SN,
UNITPRICE,
BASE,
WEIGHT,
YEAR,
MONTH,
DAY
)
VALUES
(
@SEKEY,
@SEID,
@ORID,
@SN,
@UNITPRICE,
@BASE,
@WEIGHT,
@YEAR,
@MONTH,
@DAY

)

";

        string setsqlt = @"

INSERT INTO SELLTABLE_MST
(
SEID,
SELLDATE,
SELLERID,
SEND_ADDRESS,
CONTACT,
PHONE,
DATE,
MAKERID,
YEAR,
MONTH,
DAY
)
VALUES
(
@SEID,
@SELLDATE,
@SELLERID,
@SEND_ADDRESS,
@CONTACT,
@PHONE,
@DATE,
@MAKERID,
@YEAR,
@MONTH,
@DAY
)
";
        string setsqlth = @"
UPDATE SELLTABLE_MST SET 
SELLDATE=@SELLDATE,
SELLERID=@SELLERID,
SEND_ADDRESS=@SEND_ADDRESS,
CONTACT=@CONTACT,
PHONE=@PHONE,
DATE=@DATE
";
        string setsqlf = @"
INSERT INTO 
MATERE
(
MRKEY,
MATEREID,
SN,
MRCOUNT,
ORKEY,
BATCHID,
Date,
MakerID,
Year,
Month,
Day
)
VALUES
(
@MRKEY,
@MATEREID,
@SN,
@MRCOUNT,
@ORKEY,
@BATCHID,
@Date,
@MakerID,
@Year,
@Month,
@Day
)
";
        string setsqlfi = @"

";
        #endregion
        DataTable dtx2 = new DataTable();
        DataTable dt4 = new DataTable();
        DataTable dt = new DataTable();
        DataTable dtx = new DataTable();
        CFileInfo cfileinfo = new CFileInfo();
        CORDER corder = new CORDER();
        CMOLD_BASE cmold_base = new CMOLD_BASE();
        int i,j;
        public CSELLTABLE()
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
            string v1 = bc.numYM_NEW(10, 4, "0001", "SELLTABLE_MST", "SEID", "SE");

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
            if (bc.exists("SELECT * FROM CO_SELLTABLE WHERE ORID='" + ORID + "'"))
            {
                b = true;
                ErrowInfo = "该订单号已经存在厂内订单中，不允许修改与删除！";
            }
            return b;
        }
        #region GET_TOTAL_SELLTABLE
        public  DataTable GET_TOTAL_SELLTABLE()
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
            dtt.Columns.Add("交货日期", typeof(string));

            DataTable dtx1 = bc.getdt("SELECT * FROM SELLTABLE_DET ");
            if (dtx1.Rows.Count > 0)
            {
                for (i = 0; i < dtx1.Rows.Count; i++)
                {
                    DataRow dr = dtt.NewRow();
                    dr["索引"] = dtx1.Rows[i]["SEKEY"].ToString();
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
                    dr["交货日期"] = dtx1.Rows[i]["DELIVERYDATE"].ToString();
                    if (dtx1.Rows[i]["SELLTABLESTATUS_DET"].ToString() == "OPEN")
                    {
                        dr["状态"] = "OPEN";
                    }
                    else if (dtx1.Rows[i]["SELLTABLESTATUS_DET"].ToString() == "PROGRESS")
                    {
                        dr["状态"] = "部分出货";
                    }
                    else if (dtx1.Rows[i]["SELLTABLESTATUS_DET"].ToString() == "DELAY")
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
        #region GET_SELLTABLE_PROGRESS_COUNT
        public string GET_SELLTABLE_PROGRESS_COUNT(string WAREID,string SEKEY)
        {
            string v = "0";
            DataView dv = new DataView(GET_TOTAL_SELLTABLE());
            dv.RowFilter = "状态 NOT IN ('已出货') AND ID='" + WAREID + "' AND 索引 NOT IN ('"+SEKEY +"')";
            DataTable dt = dv.ToTable();
            if (dt.Rows.Count > 0)
            {

                v = dt.Compute("SUM(订单未结数量)", "").ToString();

            }
            return v;
        }
        #endregion
    
        #region JUAGE_REALTY_IFHAVE_SELLCOUNT
        public bool  JUAGE_REALTY_IFHAVE_SELLCOUNT(string ORID)
        {
            bool b = false;
            DataView dv = new DataView(GET_TOTAL_SELLTABLE());
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
        #region JUAGE_SELLTABLE_IF_HAVE_NO_AUDIT
        public bool JUAGE_SELLTABLE_IF_HAVE_NO_AUDIT(string ORID)
        {
            bool b = false;
            string s2 = bc.getOnlyString("SELECT IF_AUDIT FROM SELLTABLE_MST WHERE ORID='" +ORID  + "'");
            if (s2 != "Y")
            {
                b = true;
                ErrowInfo = "此订单未审核，不能进行相关操作！";
            }
            return b;
        }
        #endregion
        #region  JUAGE_RESIDUE_SECOUNT_IF_LESSTHAN_SR_COUNT
        public bool JUAGE_RESIDUE_SECOUNT_IF_LESSTHAN_SR_COUNT(string SEID)
        {
            bool b = false;
            DataTable dt = bc.getdt(sqlo + " WHERE A.SEID='" + SEID + "'");
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    ORID = dr["订单号"].ToString();
                    SN = dr["项次"].ToString();
                    decimal d1 = decimal.Parse(dr["销货数量"].ToString());
                    decimal d = 0;
                    decimal d2 = 0;
                    DataView dv = new DataView(corder.GET_TOTAL_ORDER());
                    dv.RowFilter = "订单号='" + ORID + "' AND 项次='" + SN + "'";
                    DataTable dtx = dv.ToTable();
                    if (dtx.Rows.Count > 0)
                    {

                        d = decimal.Parse(dtx.Rows[0]["累计销货数量"].ToString());
                        d2 = decimal.Parse(dtx.Rows[0]["累计销退数量"].ToString());
                        if (d - d1 < d2)
                        {
                            b = true;
                            ErrowInfo = "项次:" + SN + " 累计销货数量：" + d.ToString("#0.00") +
                                "与删除的销货数量：" + d1.ToString("#0.00") + "差值：" + (d - d1).ToString("#0.00") +
                                "小于该项次的累计销退数量：" + d2.ToString("0.00") + "，不允许编辑或删除该单据";
                            break;
                        }
                    }
                }
            }
            return b;
        }
        #endregion
        #region save
        public DataTable save(DataTable dt,DataTable dtx1,DataTable dtx2,DataTable dtx4)//返回第一次执行后订单的项次最新库存数量与累计销货数量值
        {
            string year = DateTime.Now.ToString("yy");
            string month = DateTime.Now.ToString("MM");
            string day = DateTime.Now.ToString("dd");
            string varDate = DateTime.Now.ToString("yyy/MM/dd HH:mm:ss").Replace("-", "/");

            //循环体外取号调一次数据库连接取出多个号来
            SEKEY = bc.numYMD_NEW(20, 12, "000000000001", "SELLTABLE_DET", "SEKEY", "SE");

            //list里存了取出来的号,有多少行数据写入DB就取出多少个号
            List<String> list = new List<string>();


            for (int i = 0; i < dt.Rows.Count; i++)
            {
                int j = Convert.ToInt32(SEKEY.Substring(8, 12)) + i;
                list.Add(SEKEY.Substring(0, 8) + j.ToString().PadLeft(12, '0'));//在流水码的前面补0直到长度达到指定长度);
            }


            //写入销货单明细表的多行数据
            List<CSELLTABLE> listCSELLTABLE = new List<CSELLTABLE>();

            //写入销货扣减库存表的多行数据matere
            List<MateRe> listMateRe = new List<MateRe>();

            //更新订单明细表单价与重量数据
            List<CORDER> listCorder = new List<CORDER>();

            for (i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["销货数量"].ToString() == "0")
                {

                }
                else
                {
                    CSELLTABLE cSELLTABLE = new CSELLTABLE();
                    cSELLTABLE._SEKEY = list[i];
                    cSELLTABLE._SEID = SEID;
                    cSELLTABLE._ORID = dt.Rows[i]["订单号"].ToString();
                    cSELLTABLE._SN = dt.Rows[i]["项次"].ToString();
                    if (!string.IsNullOrEmpty(dt.Rows[i]["单价"].ToString()))
                    {
                        cSELLTABLE._UNITRPICE = dt.Rows[i]["单价"].ToString();
                    }
                    if (!string.IsNullOrEmpty(dt.Rows[i]["基数"].ToString()))
                    {
                        cSELLTABLE._BASE = dt.Rows[i]["基数"].ToString();
                    }
                    if (!string.IsNullOrEmpty(dt.Rows[i]["重量"].ToString()))
                    {
                        cSELLTABLE._WEIGHT = dt.Rows[i]["重量"].ToString();
                    }
                    listCSELLTABLE.Add(cSELLTABLE);

                    if (dtx2.Rows.Count > 0)
                    {
                        ORKEY = bc.GET_DT_TO_DV_TO_DT(dtx2, "", "ORID='" + dt.Rows[i]["订单号"].ToString() + "' AND SN='" + dt.Rows[i]["项次"].ToString() + "'").Rows[0]["ORKEY"].ToString();
                    }
                    dtx = bc.getmaxstoragecountNew(dtx4, ORKEY, "");
                    if (dtx.Rows.Count > 0)
                    {
                        dt.Rows[i]["批号"] = dtx.Rows[0]["批号"].ToString();
                        dt.Rows[i]["库存数量"] = dtx.Rows[0]["库存数量"].ToString();
                    }
                    //写入matere数据
                    MateRe mateRe = new MateRe();
                    mateRe.MRKEY = list[i];
                    mateRe.MATEREID = SEID;
                    mateRe.SN = dt.Rows[i]["项次"].ToString();
                    mateRe.MRCOUNT = dt.Rows[i]["销货数量"].ToString();
                    mateRe.ORKEY = ORKEY;
                    mateRe.BATCHID = dt.Rows[i]["批号"].ToString();
                    mateRe.MAKERID = MAKERID;
                    mateRe.DATE = varDate;
                    listMateRe.Add(mateRe);


                    //更新订单明细表单价与重量数据 //将销货单的单价,重量更新到订单，以便下次销同一订单时带出单价
                    CORDER cORDER = new CORDER();
                    cORDER.UNITPRICE = dt.Rows[i]["单价"].ToString();
                    cORDER.WEIGHT = dt.Rows[i]["重量"].ToString();
                    cORDER.ORKEY = ORKEY;
                    listCorder.Add(cORDER);
                }
            }
            //拼接销货单明细表Insert语句
            StringBuilder sqb1 = new StringBuilder();
            foreach (CSELLTABLE cSELLTABLE1 in listCSELLTABLE)
            {

                sqb1.AppendFormat("insert into selltable_det(sekey,seid,orid,sn,unitprice,base,weight) values ('{0}'", cSELLTABLE1.SEKEY);
                sqb1.AppendFormat(",'{0}'", cSELLTABLE1.SEID);
                sqb1.AppendFormat(",'{0}'", cSELLTABLE1.ORID);
                sqb1.AppendFormat(",'{0}'", cSELLTABLE1.SN);
                sqb1.AppendFormat(",'{0}'", cSELLTABLE1.UNITPRICE);
                sqb1.AppendFormat(",'{0}'", cSELLTABLE1.BASE);
                sqb1.AppendFormat(",'{0}'", cSELLTABLE1.WEIGHT);
                sqb1.AppendFormat(");");
            }

            //拼接matere数据Insert语句
            StringBuilder sqb2 = new StringBuilder();
            foreach (MateRe mateRe1 in listMateRe)
            {
                sqb2.AppendFormat("insert into matere(mrkey,matereid,sn,mrcount,orkey,batchid,makerid,date) values (");
                sqb2.AppendFormat("'{0}',", mateRe1.MRKEY);
                sqb2.AppendFormat("'{0}',", mateRe1.MATEREID);
                sqb2.AppendFormat("'{0}',", mateRe1.SN);
                sqb2.AppendFormat("'{0}',", mateRe1.MRCOUNT);
                sqb2.AppendFormat("'{0}',", mateRe1.ORKEY);
                sqb2.AppendFormat("'{0}',", mateRe1.BATCHID);
                sqb2.AppendFormat("'{0}',", mateRe1.MAKERID);
                sqb2.AppendFormat("'{0}'", mateRe1.DATE);
                sqb2.AppendFormat(");");
            }

            //拼接order_det数据Update语句
            StringBuilder sqb3 = new StringBuilder();
            foreach (CORDER cORDER1 in listCorder)
            {
                sqb3.AppendFormat("update order_det set unitprice='{0}',", cORDER1.UNITPRICE);
                sqb3.AppendFormat(" weight='{0}'", cORDER1.WEIGHT);
                sqb3.AppendFormat(" where orkey='{0}';", cORDER1.ORKEY);
            }

            //写入销货单主表数据
            StringBuilder sqb4 = new StringBuilder();
            sqb4.AppendFormat("insert into selltable_mst(seid,selldate,sellerid,send_address,contact,phone,date,makerid) values (");
            sqb4.AppendFormat("'{0}',", SEID);
            sqb4.AppendFormat("'{0}',", SELLDATE);
            sqb4.AppendFormat("'{0}',", SELLERID);
            sqb4.AppendFormat("'{0}',", SEND_ADDRESS);
            sqb4.AppendFormat("'{0}',", CONTACT);
            sqb4.AppendFormat("'{0}',", PHONE);
            sqb4.AppendFormat("'{0}',", varDate);
            sqb4.AppendFormat("'{0}'", MAKERID);
            sqb4.AppendFormat(")");

            //修改销货单主表数据
            StringBuilder sqb5 = new StringBuilder();
            sqb5.AppendFormat("update selltable_mst set ");
            sqb5.AppendFormat("selldate='{0}',", SELLDATE);
            sqb5.AppendFormat("sellerid='{0}',", SELLERID);
            sqb5.AppendFormat("send_address='{0}',",SEND_ADDRESS);
            sqb5.AppendFormat("contact='{0}',", CONTACT);
            sqb5.AppendFormat("phone='{0}',", PHONE);
            sqb5.AppendFormat("date='{0}',", varDate);
            sqb5.AppendFormat("makerid='{0}'", MAKERID);
            sqb5.AppendFormat(" WHERE SEID = '" + SEID + "'");

            //拼接所有sql语句一次提交数据库执行减少对数据库的连接,缩小程序执行时间
            StringBuilder sqb = new StringBuilder();
            sqb.Append(sqb1);
            sqb.Append(sqb2);
            sqb.Append(sqb3);
            if (dtx1.Rows.Count == 0)//主表新增
            {
                sqb.Append(sqb4);
            }
            //主表修改
            else
            {
                sqb.Append(sqb5);
            }
            string str = sqb.ToString();
            string str1 = null;
            Database database = bc.getdb();
            using (DbConnection dbConnection = database.CreateConnection())
            {
                dbConnection.Open();
                DbTransaction dbTransaction = dbConnection.BeginTransaction();
                System.Data.Common.DbCommand dbCommand = database.GetSqlStringCommand(sqb.ToString());
                database.ExecuteNonQuery(dbCommand, dbTransaction);
                try
                {
                    dbTransaction.Commit();//执行成功就提交SQL,执行失败就回滚
                    IFExecution_SUCCESS = true;
                }
                catch (Exception ex)
                {
                    dbTransaction.Rollback();//执行失败就回滚
                    IFExecution_SUCCESS = false;
                    MessageBox.Show(ex.Message);
                }
            }
           
            return dt;
        }
        #endregion
    





    
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
            dt.Columns.Add("交货日期", typeof(string));
            return dt;
        }
        #endregion
        #region GetTableInfo_SEARCH
        public DataTable GetTableInfo_SEARCH()
        {
            dt = new DataTable();
            dt.Columns.Add("序号", typeof(string));
            dt.Columns.Add("订单号", typeof(string));
            dt.Columns.Add("项次", typeof(string));
            dt.Columns.Add("客户名称", typeof(string));
            dt.Columns.Add("订单日期", typeof(string));
            dt.Columns.Add("客户订单号", typeof(string));
            dt.Columns.Add("型号", typeof(string));
            dt.Columns.Add("品名", typeof(string));
            dt.Columns.Add("材料", typeof(string));
            dt.Columns.Add("数量", typeof(string));
            dt.Columns.Add("单位", typeof(string));
            dt.Columns.Add("交货日期", typeof(string));
            return dt;
        }
        #endregion
    
        #region ExcelPrint
        public void ExcelPrint(DataTable dt, string BillName, string Printpath)
        {
            if (dt.Rows.Count > 0)
            {
                //根据要打印的行数求出一共要几张A4纸，每张A4纸打印5个项
                    decimal totalcount = Math.Ceiling(decimal.Parse(dt.Rows.Count.ToString()) / 40);
                    int i = 0;
                    int i1=0;
                    for (int z = 0; z < totalcount; z++)
                    {
                        SaveFileDialog sfdg = new SaveFileDialog();
                        //sfdg.DefaultExt = @"D:\xls";
                        sfdg.Filter = "Excel(*.xls)|*.xls";
                        sfdg.RestoreDirectory = true;
                        sfdg.FileName = Printpath;
                        sfdg.CreatePrompt = true;
                        Excel.Application application = new Microsoft.Office.Interop.Excel.Application();
                        Excel.Workbook workbook;
                        Excel.Worksheet worksheet;
                        workbook = application.Workbooks._Open(sfdg.FileName, Type.Missing, Type.Missing,
                        Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                        Type.Missing, Type.Missing, Type.Missing);
                        worksheet = (Excel.Worksheet)workbook.Worksheets[1];
                        application.Visible = true;
                        application.ExtendList = false;
                        application.DisplayAlerts = false;
                        application.AlertBeforeOverwriting = false;
                        int count = 0;
                        int j = 0;
                        for (i =i1 ; i < dt.Rows.Count; i++)
                        {
                          
                            if (count == 40)
                            {
                                i1 = count;
                                break;
                            }
                            else
                            {
                            //MessageBox.Show(dt.Rows[i]["序号"].ToString()+","+ dt.Rows[i]["型号"].ToString());
                            j = i;
                            worksheet.Cells[6, "C"] = dt.Rows[i]["客户名称"].ToString ();
                                worksheet.Cells[6, "H"] = dt.Rows[i]["销货单号"].ToString ();
                                worksheet.Cells[7, "C"] = dt.Rows[i]["送货地址"].ToString ();
                                worksheet.Cells[7, "H"] = dt.Rows[i]["联系人"].ToString ();
                                worksheet.Cells[8, "C"] = dt.Rows[i]["联系电话"].ToString ();
                                worksheet.Cells[8, "H"] = dt.Rows[i]["销货日期"].ToString ();
                                //worksheet.Cells[9, "C"] = dt.Rows[i]["订单号"].ToString();
                            
                                worksheet.Cells[12 + 1 * j, "A"] = dt.Rows[i]["序号"].ToString ();
                                worksheet.Cells[12 + 1 * j, "B"] = dt.Rows[i]["客户订单号"].ToString();
                                worksheet.Cells[12 + 1 * j, "D"] = dt.Rows[i]["型号"].ToString();
                                worksheet.Cells[12 + 1 * j, "E"] = dt.Rows[i]["品名"].ToString();
                                worksheet.Cells[12 + 1 * j, "G"] = dt.Rows[i]["材料"].ToString();
                                worksheet.Cells[12 + 1 * j, "H"] = dt.Rows[i]["销货数量"].ToString ();
                                worksheet.Cells[12 + 1 * j, "I"] = dt.Rows[i]["单位"].ToString();
                                worksheet.Cells[12 + 1 * j, "K"] = dt.Rows[i]["单价"].ToString();
                                worksheet.Cells[12 + 1 * j, "L"] = dt.Rows[i]["金额"].ToString();
                          
                                worksheet.Cells[30, "C"] = dt.Rows[i]["销货员"].ToString();
                          
                            count = count + 1;
                            }

                        }
                        //worksheet.PrintOut(Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                        /*workbook.SaveAs(System.IO.Path.GetFullPath("PRINT_TEMP/"+BARCODE+".xlsx"), Excel.XlFileFormat.xlExcel7, Type.Missing, 
                            Type.Missing, Type.Missing, Type.Missing, Excel.XlSaveAsAccessMode.xlNoChange, Type.Missing, 
                            Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                 
                        application.Quit();
                        worksheet = null;
                        workbook = null;
                        application = null;
                        GC.Collect();
                        Excel.Application application1 = new Microsoft.Office.Interop.Excel.Application();
                        Excel.Workbook workbook1;
                        Excel.Worksheet worksheet1=null ;
                        workbook1 = application1.Workbooks._Open(System.IO.Path.GetFullPath("PRINT_TEMP/" + BARCODE + ".xls"), Type.Missing, Type.Missing,
                        Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                        Type.Missing, Type.Missing, Type.Missing); ;
                        worksheet1 = (Excel.Worksheet)workbook1.Worksheets[1];
                        worksheet1.PrintOut(Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                        application1.Quit();
                        worksheet1 = null;
                        workbook1 = null;
                        application1 = null;
                        GC.Collect();
                        ErrowInfo = "打印数据已发出";*/
                      
                    }
            }
            else
            {
                ErrowInfo = "没有数据可打印";
                return;
            }
     
         
          
        }
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
            sqlcom.Parameters.Add("BARCODE", SqlDbType.VarChar, 20).Value = BARCODE;
            sqlcom.Parameters.Add("SEKEY", SqlDbType.VarChar, 20).Value = SEKEY;
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

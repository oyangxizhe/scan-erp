using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using System.Data.SqlClient;
using XizheC;
using System.Windows.Forms;

namespace XizheC
{
    public class CBOM
    {
        basec bc = new basec();
        #region nature
        private string _EMID;
        public string EMID
        {
            set { _EMID = value; }
            get { return _EMID; }

        }
        private string _TEL;
        public string TEL
        {
            set { _TEL = value; }
            get { return _TEL; }

        }
        private string _CURRENT_WAREID;
        public string CURRENT_WAREID
        {
            set { _CURRENT_WAREID = value; }
            get { return _CURRENT_WAREID; }

        }
        private string _LAST_WAREID;
        public string LAST_WAREID
        {
            set { _LAST_WAREID = value; }
            get { return _LAST_WAREID; }

        }
        private string _NEXT_WAREID;
        public string NEXT_WAREID
        {
            set { _NEXT_WAREID = value; }
            get { return _NEXT_WAREID; }

        }
        private string _RMKEY;
        public string RMKEY
        {
            set { _RMKEY = value; }
            get { return _RMKEY; }

        }

        private string _PHONE;
        public string PHONE
        {
            set { _PHONE = value; }
            get { return _PHONE; }

        }
 
        private string _BOM_EDITION;
        public string BOM_EDITION
        {
            set { _BOM_EDITION = value; }
            get { return _BOM_EDITION; }

        }
       
        private string _BOID;
        public string BOID
        {
            set { _BOID = value; }
            get { return _BOID; }

        }
        private string _WAREID;
        public string WAREID
        {
            set { _WAREID = value; }
            get { return _WAREID; }
        }
        private string _DET_WAREID;
        public string DET_WAREID
        {
            set { _DET_WAREID = value; }
            get { return _DET_WAREID; }
        }
        private string _BOM_ID;
        public string BOM_ID
        {
            set { _BOM_ID = value; }
            get { return _BOM_ID; }

        }
        private string _BOM;
        public string BOM
        {
            set { _BOM = value; }
            get { return _BOM; }

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
        private string _POSTCODE;
        public string POSTCODE
        {
            set { _POSTCODE = value; }
            get { return _POSTCODE; }

        }
        private string _ADDRESS;
        public string ADDRESS
        {
            set { _ADDRESS = value; }
            get { return _ADDRESS; }

        }
        private string _sqlsi;
        public string sqlsi
        {
            set { _sqlsi = value; }
            get { return _sqlsi; }

        }
        private string _MAKERID;
        public string MAKERID
        {
            set { _MAKERID = value; }
            get { return _MAKERID; }

        }
        private string _BOKEY;
        public string BOKEY
        {
            set { _BOKEY = value; }
            get { return _BOKEY; }

        }
        private  bool _IFExecutionSUCCESS;
        public  bool IFExecution_SUCCESS
        {
            set { _IFExecutionSUCCESS = value; }
            get { return _IFExecutionSUCCESS; }

        }
        private string _PAYMENT;
        public string PAYMENT
        {
            set { _PAYMENT = value; }
            get { return _PAYMENT; }

        }

        private string _SN;
        public string SN
        {
            set { _SN = value; }
            get { return _SN; }

        }
        private string _ACTIVE;
        public string ACTIVE
        {
            set { _ACTIVE = value; }
            get { return _ACTIVE; }

        }
        private string _ErrowInfo;
        public string ErrowInfo
        {

            set { _ErrowInfo = value; }
            get { return _ErrowInfo; }

        }
        private string _PROVINCE;
        public string PROVINCE
        {
            set { _PROVINCE = value; }
            get { return _PROVINCE; }

        }
        private string _REMARK;
        public string REMARK
        {
            set { _REMARK = value; }
            get { return _REMARK; }

        }
        #endregion
        DataTable dt = new DataTable();
        #region sql
        string setsql = @"

SELECT 
B.BOID AS BOM编号,
B.BOM AS BOM名称,
B.BOM_ID AS BOM代码,
B.BOM_EDITION AS 版本号,
A.SN AS 项次,
D.WareID AS 主件编号,
D.CO_WAREID AS 主件料号,
D.WNAME AS 主件品名,
C.WAREID AS 元件编号,
C.CO_WAREID  AS 元件料号,
C.WName  AS 元件品名,
CASE WHEN B.ACTIVE='Y' THEN '已生效'
ELSE '未生效'
END 
AS 生效否,
(SELECT ENAME FROM EMPLOYEEINFO WHERE EMID=B.MAKERID) AS 制单人,
B.DATE AS 制单日期,
A.REMARK AS 备注
FROM BOM_DET A 
LEFT JOIN BOM_MST B ON A.BOID=B.BOID
LEFT JOIN WAREINFO C ON A.DET_WAREID=C.WAREID
LEFT JOIN WAREINFO D ON B.WAREID=D.WAREID

";


        string setsqlo = @"
INSERT INTO BOM_DET
(
BOKEY,
BOID,
SN,
DET_WAREID,
MAKERID,
DATE,
YEAR,
MONTH,
DAY
)
VALUES
(
@BOKEY,
@BOID,
@SN,
@DET_WAREID,
@MAKERID,
@DATE,
@YEAR,
@MONTH,
@DAY

)


";

        string setsqlt = @"

INSERT INTO BOM_MST
(
BOID,
BOM_ID,
BOM,
WAREID,
BOM_EDITION,
ACTIVE,
DATE,
MAKERID,
YEAR,
MONTH,
DAY
)
VALUES
(
@BOID,
@BOM_ID,
@BOM,
@WAREID,
@BOM_EDITION,
@ACTIVE,
@DATE,
@MAKERID,
@YEAR,
@MONTH,
@DAY
)
";
        string setsqlth = @"
UPDATE BOM_MST SET 
BOM_ID=@BOM_ID,
BOM=@BOM,
WAREID=@WAREID,
BOM_EDITION=@BOM_EDITION,
ACTIVE=@ACTIVE,
DATE=@DATE,
YEAR=@YEAR,
MONTH=@MONTH,
DAY=@DAY
";

        string setsqlf = @"

";
        string setsqlfi = @"

";
        string setsqlsi = @"


";
        #endregion
        public CBOM()
        {
            string year, month, day;
            year = DateTime.Now.ToString("yy");
            month = DateTime.Now.ToString("MM");
            day = DateTime.Now.ToString("dd");
            //GETID =bc.numYM(10, 4, "0001", "SELECT * FROM WORKORDER_PICKING_MST", "WPID", "WP");

            sql = setsql;
            sqlo = setsqlo;
            sqlt = setsqlt;
            sqlth = setsqlth;
            sqlf = setsqlf;
            sqlfi = setsqlfi;
            sqlsi = setsqlsi;
        }
        #region GetTableInfo
        public DataTable GetTableInfo()
        {
            dt = new DataTable();
            dt.Columns.Add("项次", typeof(string));
            dt.Columns.Add("型号", typeof(string));
            dt.Columns.Add("模具编号", typeof(string));
            dt.Columns.Add("品名", typeof(string));
            dt.Columns.Add("材料", typeof(string));
            dt.Columns.Add("数量", typeof(double));
            dt.Columns.Add("单位", typeof(string));
            dt.Columns.Add("订单交期", typeof(string));
            dt.Columns.Add("客户订单号", typeof(string));
            return dt;
        }
    
        #endregion
        public string GETID()
        {
            string year = DateTime.Now.ToString("yy");
            string month = DateTime.Now.ToString("MM");
            string day = DateTime.Now.ToString("dd");
            string varDate = DateTime.Now.ToString("yyy/MM/dd HH:mm:ss").Replace("-", "/");
            string v1 = bc.numYM(10, 4, "0001", "select * from BOM_MST", "BOID", "BO");
            string GETID = "";
            if (v1 != "Exceed Limited")
            {
                GETID = v1;
              
            }
            return GETID;
        }
        #region save
        public void save(DataTable dt)
        {
          
            string year = DateTime.Now.ToString("yy");
            string month = DateTime.Now.ToString("MM");
            string day = DateTime.Now.ToString("dd");
            string varDate = DateTime.Now.ToString("yyy/MM/dd HH:mm:ss").Replace("-", "/");
            string GET_WAREID= bc.getOnlyString("SELECT WAREID FROM BOM_MST WHERE  WAREID='" +WAREID  + "'");
            string GET_BOM_EDITION = bc.getOnlyString("SELECT BOM_EDITION  FROM BOM_MST WHERE BOID='" + BOID + "'");
            if (!bc.exists("SELECT BOID FROM BOM_DET WHERE BOID='" + BOID + "'"))
            {
                if (bc.exists("SELECT * FROM BOM_MST where WAREID='" + WAREID  + "' AND BOM_EDITION='"+BOM_EDITION +"'"))
                {
                    ErrowInfo = string.Format("物料编号：{0}"+"+"+"版本号：{1}"+"已经存在系统", WAREID,BOM_EDITION );
                    IFExecution_SUCCESS = false;
                }
                else
                {
                    if (ACTIVE =="Y" && bc.exists("SELECT * FROM BOM_MST where WAREID='" + WAREID + "'"))
                    {

                        bc.getcom("UPDATE BOM_MST SET ACTIVE='N' WHERE WAREID='"+WAREID +"'");
                    }
                    ACTION_DET(dt);
                    SQlcommandE_MST(sqlt);
                    IFExecution_SUCCESS = true;
                }
            }
            else if (WAREID != GET_WAREID || BOM_EDITION != GET_BOM_EDITION)
            {
                if (bc.exists("SELECT * FROM BOM_MST where WAREID='" + WAREID + "' AND BOM_EDITION='" + BOM_EDITION + "'"))
                {

                    ErrowInfo = string.Format("物料编号：{0}" + "+" + "版本号：{1}" + "已经存在系统", WAREID, BOM_EDITION);
                    IFExecution_SUCCESS = false;
                }
                else
                {
                    if (ACTIVE == "Y" && bc.exists("SELECT * FROM BOM_MST where WAREID='" + WAREID + "'"))
                    {
                        bc.getcom("UPDATE BOM_MST SET ACTIVE='N' WHERE WAREID='" + WAREID + "'");
                    }
                    ACTION_DET(dt);
                    SQlcommandE_MST(sqlth + " WHERE BOID='" + BOID + "'");
                    IFExecution_SUCCESS = true;
                }
            }
            else
            {

                if (ACTIVE == "Y" && bc.exists("SELECT * FROM BOM_MST where WAREID='" + WAREID + "'"))
                {

                    bc.getcom("UPDATE BOM_MST SET ACTIVE='N' WHERE WAREID='" + WAREID + "'");

                }
                    ACTION_DET(dt);
                    SQlcommandE_MST(sqlth + " WHERE BOID='" + BOID + "'");
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
            string varDate = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss").Replace ("-","/");
            SqlConnection sqlcon = bc.getcon();
            sqlcon.Open();
            SqlCommand sqlcom = new SqlCommand(sql, sqlcon);
            sqlcom.Parameters.Add("@BOKEY", SqlDbType.VarChar, 20).Value = BOKEY;
            sqlcom.Parameters.Add("@SN", SqlDbType.VarChar, 20).Value = SN;
            sqlcom.Parameters.Add("@BOID", SqlDbType.VarChar, 20).Value = BOID;
            sqlcom.Parameters.Add("@DET_WAREID", SqlDbType.VarChar, 20).Value = DET_WAREID;
            sqlcom.Parameters.Add("@MAKERID", SqlDbType.VarChar, 20).Value = EMID;
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
            sqlcom.Parameters.Add("@BOID", SqlDbType.VarChar, 20).Value = BOID;
            sqlcom.Parameters.Add("@BOM", SqlDbType.VarChar, 20).Value = BOM;
            sqlcom.Parameters.Add("@BOM_ID", SqlDbType.VarChar, 20).Value = BOM_ID;
            sqlcom.Parameters.Add("@BOM_EDITION", SqlDbType.VarChar, 20).Value = BOM_EDITION;
            sqlcom.Parameters.Add("@ACTIVE", SqlDbType.VarChar, 20).Value = ACTIVE;
            sqlcom.Parameters.Add("@WAREID", SqlDbType.VarChar, 20).Value = WAREID;
            sqlcom.Parameters.Add("@DATE", SqlDbType.VarChar, 20).Value = varDate;
            sqlcom.Parameters.Add("@MAKERID", SqlDbType.VarChar, 20).Value = EMID;
            sqlcom.Parameters.Add("@YEAR", SqlDbType.VarChar, 20).Value = year;
            sqlcom.Parameters.Add("@MONTH", SqlDbType.VarChar, 20).Value = month;
            sqlcom.Parameters.Add("@DAY", SqlDbType.VarChar, 20).Value = day;
            sqlcom.ExecuteNonQuery();
            sqlcon.Close();
        }
        #endregion
        private void ACTION_DET(DataTable dt)
        {
           
            basec.getcoms("DELETE BOM_DET WHERE BOID='" + BOID + "'");
            foreach (DataRow dr in dt.Rows)
            {
                BOKEY = bc.numYMD(20, 12, "000000000001", "SELECT * FROM BOM_DET", "BOKEY", "BO");
                DET_WAREID = bc.getOnlyString(string.Format("SELECT WAREID FROM WAREINFO WHERE CO_WAREID='{0}'", dr["元件料号"].ToString()));
                SN = dr["项次"].ToString();
                SQlcommandE_DET(sqlo);
            }
        }
        public void RETURN_LAST_AND_NEXT_WAREID(string WAREID, string BOID, string BOM_EDITION)
        {
            dt = bc.GET_DT_TO_DV_TO_DT(bc.getdt(sql), "", string.Format("BOM编号='{0}' AND 版本号='{1}'", BOID, BOM_EDITION));
            DataTable dtx = bc.GET_DT_TO_DV_TO_DT(dt, "", string.Format ("物料编号='{0}'",WAREID));
            if (dt.Rows.Count > 0)
            {
                if (dtx.Rows.Count > 0)
                {

                    int i = Convert.ToInt32(dtx.Rows[0]["项次"].ToString());
                    if (i < dt.Rows.Count)
                    {
                        dtx = bc.GET_DT_TO_DV_TO_DT(dt, "",string.Format ("项次='{0}'",(i+1).ToString ()));
                        NEXT_WAREID = dtx.Rows[0]["物料编号"].ToString();
                    }

                }

            }
        }
    
    }
}

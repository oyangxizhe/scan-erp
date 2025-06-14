﻿using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using System.Data.SqlClient;
using XizheC;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace XizheC
{
    public class CUSER
    {
        basec bc = new basec();
        CEDIT_RIGHT cedit_right = new CEDIT_RIGHT();
        #region nature
        private string _ErrowInfo;
        public string ErrowInfo
        {

            set { _ErrowInfo = value; }
            get { return _ErrowInfo; }

        }
        private string _USID;
        public string USID
        {
            set { _USID = value; }
            get { return _USID; }

        }
        private string _UGID;
        public string UGID
        {
            set { _UGID = value; }
            get { return _UGID; }

        }
        private string _UNAME;
        public string UNAME
        {
            set { _UNAME = value; }
            get { return _UNAME; }

        }
        private string _EMID;
        public string EMID
        {
            set { _EMID = value; }
            get { return _EMID; }

        }
        private string _USER_GROUP;
        public string USER_GROUP
        {
            set { _USER_GROUP = value; }
            get { return _USER_GROUP; }

        }
        private string _MAKERID;
        public string MAKERID
        {
            set { _MAKERID = value; }
            get { return _MAKERID; }

        }
        private string _USER_TYPE;
        public string USER_TYPE
        {
            set { _USER_TYPE = value; }
            get { return _USER_TYPE; }

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
        private string _AUID;
        public string AUID
        {
            set { _AUID = value; }
            get { return _AUID; }
        }
        private string _DEPART;
        public string DEPART
        {
            set { _DEPART = value; }
            get { return _DEPART; }
        }
        private string _PWD;
        public string PWD
        {
            set { _PWD = value; }
            get { return _PWD; }

        }
        private bool _IFExecutionSUCCESS;
        public bool IFExecution_SUCCESS
        {
            set { _IFExecutionSUCCESS = value; }
            get { return _IFExecutionSUCCESS; }

        }
        private string _sql;
        public string sql
        {
            set { _sql = value; }
            get { return _sql; ; }

        }
        private string _sqlo;
        public string sqlo
        {
            set { _sqlo = value; }
            get { return _sqlo; ; }

        }
        private string _sqlt;
        public string sqlt
        {
            set { _sqlt = value; }
            get { return _sqlt; ; }
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
        #endregion
        #region sql

        string setsql = @"
SELECT
A.USID AS 用户编号,
A.UNAME AS 用户名,
(SELECT EMPLOYEE_ID FROM EMPLOYEEINFO  WHERE EMID=A.EMID) AS 员工工号,
B.ENAME AS 姓名,
C.USER_GROUP AS 用户组,
(SELECT ENAME FROM EMPLOYEEINFO  WHERE EMID=A.MAKERID) AS 制单人,
A.DATE AS 制单日期 
FROM   USERINFO  A 
LEFT JOIN EMPLOYEEINFO B ON A.EMID=B.EMID
LEFT JOIN USER_GROUP C ON A.UGID=C.UGID


";
        string setsqlo = @"
INSERT INTO USERINFO(
USID, 
UNAME, 
PWD, 
EMID, 
UGID,
USER_GROUP,
MAKERID,
DATE,
YEAR,
MONTH

) VALUES 

(
@USID, 
@UNAME, 
@PWD, 
@EMID, 
@UGID,
@USER_GROUP,
@MAKERID,
@DATE,
@YEAR,
@MONTH


)

";


        string setsqlt = @"
UPDATE USERINFO SET 
USID=@USID,
UNAME=@UNAME,
PWD=@PWD,
EMID=@EMID,
UGID=@UGID,
USER_GROUP=@USER_GROUP,
MAKERID=@MAKERID,
DATE=@DATE,
YEAR=@YEAR,
MONTH=@MONTH

";

        string setsqlth = @"
INSERT INTO 
AUTHORIZATION_USER
(
AUID,
USID,
STATUS,
LOGIN_DATE,
LEAVE_DATE,
CLIENT_IP ,
COMPUTER_NAME,
YEAR,
MONTH
)  
VALUES 
(
@AUID,
@USID,
@STATUS,
@LOGIN_DATE,
@LEAVE_DATE,
@CLIENT_IP ,
@COMPUTER_NAME,
@YEAR,
@MONTH
)"
 ;
        string setsqlf = @"
SELECT
A.AUID AS 编号, 
B.UNAME AS 用户名,
C.ENAME AS 姓名,
A.COMPUTER_NAME AS 计算机名,
A.CLIENT_IP AS IP地址,
CASE WHEN A.STATUS='Y' THEN '在线'
ELSE '离线'
END  AS 状态,
A.LOGIN_DATE AS 登录时间,
CASE WHEN CONVERT(VARCHAR(10),A.LEAVE_DATE,120)='1900-01-01' THEN ''
ELSE A.LEAVE_DATE 
END
AS 离开时间,
CASE WHEN  CONVERT(VARCHAR(10),LEAVE_DATE,120)='1900-01-01' THEN DATEDIFF(N,LOGIN_DATE,GETDATE())
ELSE DATEDIFF(N,LOGIN_DATE,LEAVE_DATE)
END AS 在线分钟,
CASE WHEN A.IF_COMPUTER_UPDATE='Y' THEN '是'
ELSE '否'
END  AS 状态是否来自更新,
A.COMPUTER_UPDATE_DATE AS 更新时间
FROM OFFER.DBO.AUTHORIZATION_USER A 
LEFT JOIN USERINFO B ON A.USID=B.USID 
LEFT JOIN EMPLOYEEINFO C ON B.EMID=C.EMID
WHERE CONVERT(VARCHAR(10),A.LOGIN_DATE,111)=CONVERT(VARCHAR(10),GETDATE(),111)
ORDER BY LOGIN_DATE DESC
";



        #endregion
        DataTable dt = new DataTable();
        public CUSER()
        {
            sql = setsql;
            sqlo = setsqlo;
            sqlt = setsqlt;
            sqlth = setsqlth;
            sqlf = setsqlf;
        }
        public CUSER(string USID)
        {
            UNAME = bc.getOnlyString("SELECT UNAME FROM USERINFO WHERE USID='" + USID + "'");
        }
        public string GETID_AUID()
        {
            string v1 = bc.numYM(10, 4, "0001", "SELECT * FROM AUTHORIZATION_USER", "AUID", "AU");
            string GETID = "";
            if (v1 != "Exceed Limited")
            {
                GETID = v1;
            }
            return GETID;
        }
        public static DataTable SqlDTM(string TableName, string ColumnName)
        {

            return basec.getdts("SELECT " + ColumnName + " FROM " + TableName);
        }
        #region EMPTY_DT()
        public DataTable EMPTY_DT()
        {

            DataTable dtt = new DataTable();
            dtt.Columns.Add("USID", typeof(string));
            dtt.Columns.Add("UNAME", typeof(string));
            dtt.Columns.Add("FREE_REGISTRATION", typeof(string));
            dtt.Columns.Add("MY_ORDER", typeof(string));
            dtt.Columns.Add("CONTACT_CUSTOMER_SERVICE", typeof(string));
            return dtt;
        }
        #endregion
        #region GET_LOGIN_INFO()
        public DataTable GET_LOGIN_INFO(string USID)
        {
            DataTable dtt = this.EMPTY_DT();
            dt = bc.getdt("SELECT * FROM USERINFO WHERE USID='" + USID + "'");
            DataRow dr1 = dtt.NewRow();
            dr1["USID"] = dt.Rows[0]["USID"].ToString();
            dr1["UNAME"] = dt.Rows[0]["UNAME"].ToString();
            dr1["FREE_REGISTRATION"] = "退出";
            dr1["MY_ORDER"] = "我的订单";
            dr1["CONTACT_CUSTOMER_SERVICE"] = "联系客服";
            dtt.Rows.Add(dr1);
            return dtt;
        }
        #endregion
        #region PLEASE_LOGIN()
        public DataTable PLEASE_LOGIN()
        {
            DataTable dtt = this.EMPTY_DT();
            DataRow dr1 = dtt.NewRow();
            dr1["UNAME"] = "请登录";
            dr1["FREE_REGISTRATION"] = "免费注册";
            dr1["MY_ORDER"] = "我的订单";
            dr1["CONTACT_CUSTOMER_SERVICE"] = "联系客服";
            dtt.Rows.Add(dr1);
            return dtt;
        }
        #endregion

        #region GET_NODEID
        public int GET_NODEID(string NODE_NAME)
        {
            string v1 = bc.getOnlyString("SELECT NODEID FROM RIGHTNAME WHERE NODE_NAME='" + NODE_NAME + "'");
            int NODE_ID = Convert.ToInt32(bc.getOnlyString("SELECT NODEID FROM RIGHTNAME WHERE NODE_NAME='" + NODE_NAME + "'"));
            return NODE_ID;
        }
        #endregion
        public string GETID()
        {
            string v1 = bc.numYM(10, 4, "0001", "SELECT * FROM USERINFO", "USID", "US");
            string GETID = "";
            if (v1 != "Exceed Limited")
            {
                GETID = v1;
            }
            return GETID;
        }
        #region JUAGE_LOGIN_IF_SUCCESS
        public bool JUAGE_LOGIN_IF_SUCCESS(string UNAME, string PWD)
        {
            bool b = false;
            try
            {
                byte[] B = bc.GetMD5(PWD);
                SqlConnection sqlcon = bc.getcon();
                string sql1 = "SELECT * FROM USERINFO WHERE PWD=@PWD and UNAME=@UNAME";
                SqlCommand sqlcom = new SqlCommand(sql1, sqlcon);
                sqlcom.Parameters.Add("@PWD", SqlDbType.Binary, 50).Value = B;
                sqlcom.Parameters.Add("@UNAME", SqlDbType.VarChar, 50).Value = UNAME;
                sqlcon.Open();
                sqlcom.ExecuteNonQuery();
                if (sqlcom.ExecuteScalar().ToString() != "")
                {
                    string sql = @"
SELECT
B.DEPART,
B.EMID,
B.ENAME,
B.EMPLOYEE_ID,
A.USID AS USID,
A.UNAME FROM USERINFO A 
LEFT JOIN EMPLOYEEINFO B ON A.EMID =B.EMID WHERE A.UNAME='" + UNAME + "'";
                    DataTable dt = basec.getdts(sql);
                    if (dt.Rows.Count > 0)
                    {
                        DEPART = dt.Rows[0]["DEPART"].ToString();
                        ENAME = dt.Rows[0]["ENAME"].ToString();
                        EMID = dt.Rows[0]["EMID"].ToString();
                        USID = dt.Rows[0]["USID"].ToString();
                        EMPLOYEE_ID = dt.Rows[0]["EMPLOYEE_ID"].ToString();
                    }
                    b = true;
                }
                sqlcon.Close();
            }
            catch (Exception)
            {

            }
            return b;
        }
        #endregion

        #region save IDVALUE
        public void save(string TABLENAME, string COLUMNID, string COLUMNNAME, string IDVALUE,
            string NAMEVALUE, string INFOID, string INFONAME, string COLUMNID_o, string COLUMNNAME_o,
            string IDVALUE_o, string NAMEVALUE_o,string INFOID_o)
        {

            string year = DateTime.Now.ToString("yy");
            string month = DateTime.Now.ToString("MM");
            string day = DateTime.Now.ToString("dd");
            string varDate = DateTime.Now.ToString("yyy/MM/dd HH:mm:ss").Replace("-", "/");
            USID = IDVALUE;
            string v1 = bc.getOnlyString("SELECT " + COLUMNNAME + " FROM " + TABLENAME + " WHERE " + COLUMNID + "='" + IDVALUE + "'");
            string v2 = bc.getOnlyString("SELECT " + COLUMNID_o   + " FROM " + TABLENAME + " WHERE " + COLUMNID + "='" + IDVALUE  + "'");
            //string varMakerID;
            if (!bc.exists("SELECT " + COLUMNID + " FROM " + TABLENAME + " WHERE " + COLUMNID + "='" + IDVALUE + "'"))
            {
                if (bc.exists("SELECT " + COLUMNID + " FROM " + TABLENAME + " WHERE " + COLUMNNAME + "='" + NAMEVALUE + "'"))
                {
                    ErrowInfo = INFONAME + "已经存在于系统！";
                    IFExecution_SUCCESS = false;
                }
                else if (bc.exists("SELECT " + COLUMNID_o + " FROM " + TABLENAME + " WHERE " + COLUMNID_o  + "='" + IDVALUE_o + "'"))
                {
                    ErrowInfo =INFOID_o + "已经存在于系统！";
                    IFExecution_SUCCESS = false;
                }
                else
                {
                    SQlcommandE(sqlo , IDVALUE, NAMEVALUE);
                    IFExecution_SUCCESS = true;
                }

            }
            else if (v1 != NAMEVALUE && v2==IDVALUE_o )
            {

                if (bc.exists("SELECT " + COLUMNID + " FROM " + TABLENAME + " WHERE " + COLUMNNAME + "='" + NAMEVALUE + "'"))
                {
                    ErrowInfo = INFONAME + "已经存在于系统！";
                    IFExecution_SUCCESS = false;
                }
                else
                {
                    SQlcommandE(sqlt + " WHERE " + COLUMNID + "='" + IDVALUE + "'", IDVALUE, NAMEVALUE);
                    IFExecution_SUCCESS = true;
                }
            }
            else if (v1 == NAMEVALUE && v2 != IDVALUE_o)
            {
                if (bc.exists("SELECT " + COLUMNID + " FROM " + TABLENAME + " WHERE " + COLUMNID_o  + "='" + IDVALUE_o + "'"))
                {
                    ErrowInfo = INFOID_o  + "已经存在于系统！";
                    IFExecution_SUCCESS = false;
                }
                else
                {
                    SQlcommandE(sqlt + " WHERE " + COLUMNID + "='" + IDVALUE + "'", IDVALUE, NAMEVALUE);
                    IFExecution_SUCCESS = true;
                }
            }
            else if (v1 != NAMEVALUE && v2 != IDVALUE_o)
            {

                if (bc.exists("SELECT " + COLUMNID + " FROM " + TABLENAME + " WHERE " + COLUMNNAME + "='" + NAMEVALUE + "'"))
                {
                    ErrowInfo =INFONAME  + "已经存在于系统！";
                    IFExecution_SUCCESS = false;

                }
                else if (bc.exists("SELECT " + COLUMNID_o + " FROM " + TABLENAME + " WHERE " + COLUMNID_o + "='" +IDVALUE_o  + "'"))
                {
                    ErrowInfo = INFOID_o + "已经存在于系统！";
                    IFExecution_SUCCESS = false;
                }
                else
                {
                    SQlcommandE(sqlt + " WHERE " + COLUMNID + "='" + IDVALUE + "'", IDVALUE, NAMEVALUE);
                    IFExecution_SUCCESS = true;
                }

            }
            else
            {
                    SQlcommandE(sqlt + " WHERE " + COLUMNID + "='" + IDVALUE + "'", IDVALUE, NAMEVALUE);
                    IFExecution_SUCCESS = true;
            }

        }
        #endregion

        #region SQlcommandE
        protected void SQlcommandE(string sql, string IDVALUE, string NAMEVALUE)
        {
            string year = DateTime.Now.ToString("yy");
            string month = DateTime.Now.ToString("MM");
            string day = DateTime.Now.ToString("dd");
            string varDate = DateTime.Now.ToString("yyy/MM/dd HH:mm:ss").Replace("-", "/");
            Byte[] B = bc.GetMD5(PWD);
            SqlConnection sqlcon = bc.getcon();
            SqlCommand sqlcom = new SqlCommand(sql, sqlcon);
            sqlcom.Parameters.Add("@USID", SqlDbType.VarChar, 20).Value = IDVALUE;
            sqlcom.Parameters.Add("@UNAME", SqlDbType.VarChar, 20).Value = NAMEVALUE;
            sqlcom.Parameters.Add("@PWD", SqlDbType.Binary, 50).Value = B;
            sqlcom.Parameters.Add("@EMID", SqlDbType.VarChar, 20).Value = EMID;
            sqlcom.Parameters.Add("@UGID", SqlDbType.VarChar, 20).Value = UGID;
            sqlcom.Parameters.Add("@USER_GROUP", SqlDbType.VarChar, 20).Value = USER_GROUP;
            sqlcom.Parameters.Add("@MAKERID", SqlDbType.VarChar, 20).Value = MAKERID;
            sqlcom.Parameters.Add("@DATE", SqlDbType.VarChar, 20).Value = varDate;
            sqlcom.Parameters.Add("@YEAR", SqlDbType.VarChar, 20).Value = year;
            sqlcom.Parameters.Add("@MONTH", SqlDbType.VarChar, 20).Value = month;
            sqlcon.Open();
            sqlcom.ExecuteNonQuery();
            sqlcon.Close();
            if (bc.exists("SELECT * FROM RIGHTLIST WHERE USID='"+USER_GROUP +"'"))
            {
                dt=bc.getdt("SELECT * FROM RIGHTLIST WHERE USID='"+USER_GROUP +"'");
                cedit_right.SQlcommandE_USER_GROUP_USERD(dt, USID,EMID,USER_GROUP );
            }
        }
        #endregion
        #region SQlcommandE
        public void SQlcommandE(string sql)
        {
            string year = DateTime.Now.ToString("yy");
            string month = DateTime.Now.ToString("MM");
            string day = DateTime.Now.ToString("dd");
            string varDate = DateTime.Now.ToString("yyy/MM/dd HH:mm:ss").Replace("-", "/");
            string varMakerID = bc.getOnlyString("SELECT EMID FROM USERINFO WHERE USID='" + USID + "'");
            SqlConnection sqlcon = bc.getcon();
            SqlCommand sqlcom = new SqlCommand(sql, sqlcon);
            sqlcom.Parameters.Add("@AUID", SqlDbType.VarChar, 20).Value = AUID;
            sqlcom.Parameters.Add("@USID", SqlDbType.VarChar, 20).Value = USID;
            sqlcom.Parameters.Add("@STATUS", SqlDbType.VarChar, 20).Value = "Y";
            sqlcom.Parameters.Add("@LOGIN_DATE", SqlDbType.VarChar, 20).Value = varDate;
            sqlcom.Parameters.Add("@LEAVE_DATE", SqlDbType.VarChar, 20).Value = "";
            sqlcom.Parameters.Add("@CLIENT_IP", SqlDbType.VarChar, 20).Value = bc.GetIP4Address();
            sqlcom.Parameters.Add("@COMPUTER_NAME", SqlDbType.VarChar, 20).Value = bc.GetComputerName();
            sqlcom.Parameters.Add("@YEAR", SqlDbType.VarChar, 20).Value = year;
            sqlcom.Parameters.Add("@MONTH", SqlDbType.VarChar, 20).Value = month;
            sqlcon.Open();
            sqlcom.ExecuteNonQuery();
            sqlcon.Close();
        }
        #endregion
        #region UPDATE_EXCEL_LOGIN_INFO
        public void UPDATE_EXCEL_LOGIN_INFO(DataTable dt, string BillName, string Printpath)
        {
            SaveFileDialog sfdg = new SaveFileDialog();
            //sfdg.DefaultExt = @"D:\xls";
            sfdg.Filter = "Excel(*.xls)|*.xls";
            sfdg.RestoreDirectory = true;
            sfdg.FileName = Printpath;
            sfdg.CreatePrompt = true;
            Microsoft.Office.Interop.Excel.Application application = new Microsoft.Office.Interop.Excel.Application();
            Excel.Workbook workbook;
            Excel.Worksheet worksheet;
            workbook = application.Workbooks._Open(sfdg.FileName, Type.Missing, Type.Missing,
            Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
            Type.Missing, Type.Missing, Type.Missing);
            worksheet = (Excel.Worksheet)workbook.Worksheets[1];
            application.Visible = false;
            application.ExtendList = false;
            application.DisplayAlerts = false;
            application.AlertBeforeOverwriting = false;
           int i;
            for (i = 0; i < dt.Rows.Count; i++)
            {
                worksheet.Cells[i + 2, "A"] = dt.Rows[i]["F1"].ToString();//从第二行开始，第一行为标题/16/01/27
                worksheet.Cells[i + 2, "B"] = dt.Rows[i]["F2"].ToString();
                worksheet.Cells[i + 2, "C"] = dt.Rows[i]["F3"].ToString();
            }
             //worksheet.get_Range(worksheet.Cells[1, 1], worksheet.Cells[65536, 256]).Columns.AutoFit();
            worksheet.get_Range(worksheet.Cells [1, "A"], worksheet.Cells [i + 1, "C"]).Columns.AutoFit();
            workbook.Save();//保存数据
            application.Quit();//退出EXCEL,结束进程
            GC.Collect();//回收资源
        }
        #endregion
    }
}

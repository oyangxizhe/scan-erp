﻿using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Drawing;
using System.Xml.Linq;
using System.Data.SqlClient;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Net;
using System.Data.OleDb;
using System.Xml;
namespace UpdateDeliveryDate
{
    public class basec
    {
        #region  建立数据库连接
        /// <summary>
        /// 建立数据库连接.
        /// </summary>
        /// <returns>返回SqlConnection对象</returns>
        /// 
        // 摘要:
        public SqlConnection getcon()
        {
           
            //string M_str_sqlcon = ConfigurationManager.AppSettings["ConnectionDB"].ToString();
            //string M_str_sqlcon = "Data Source=localhost;Database=OFFER;User id=sa;PWD=0";
            string M_str_sqlcon = "Data Source=localhost;Database=CSERP;User id=cserp;PWD=cserp161214";

            SqlConnection myCon = new SqlConnection(M_str_sqlcon);
        
            return myCon;
        }
        #endregion
        private string _ErrowInfo;
        public string ErrowInfo
        {

            set { _ErrowInfo = value; }
            get { return _ErrowInfo; }

        }
        private static  bool _IFExecutionSUCCESS;
        public static bool IFExecution_SUCCESS
        {
            set { _IFExecutionSUCCESS = value; }
            get { return _IFExecutionSUCCESS; }

        }
        StringBuilder sqb = new StringBuilder();
        int i, j;
        #region delegate
        public delegate bool USE_DELEGATE(string v1, string v2);
        public delegate bool USE_DELEGATE_T(string JUAGE_VALUE);
        public bool JUAGE_BATCHID(string v1)
        {
            bool b = false;
            if (v1 == "")
            {
                b = true;
                ErrowInfo = string.Format("批号不能为空！");

            }
            else if (!this.exists("SELECT * FROM BATCH_DET WHERE BATCHID='" + v1 + "'"))
            {
                b = true;
                ErrowInfo = string.Format("批号" + " {0} " + " 不存在系统中！", v1);

            }
            return b;

        }

   

        public bool JUDGE_STEP_IF_NEED_MACHINE(string CURRENT_STEP_ID)
        {
            bool b = false;
            StringBuilder sqb = new StringBuilder();
            sqb.AppendFormat(@"
SELECT
A.IF_NEED_MACHINE
FROM
STEP A 
WHERE A.STEP_ID='{0}' ", CURRENT_STEP_ID);
            if (getOnlyString (sqb.ToString ())=="Y")
            {
                b = true;
                ErrowInfo = string.Format("站别代码 {0} 过账需指定机台",CURRENT_STEP_ID );
            }
            return b;
        }
        public bool JUAGE_WAREID(string v1)
        {
            bool b = false;
            if (v1 == "")
            {
                b = true;
                ErrowInfo = string.Format("物料编号不能为空！");

            }
            else if (!this.exists("SELECT * FROM WAREINFO WHERE WAREID='" + v1 + "'"))
            {
                b = true;
                ErrowInfo = string.Format("物料编号" + "{0}" + " 不存在系统中！", v1);

            }
            return b;

        }
        public bool JUAGE_WOID(string v1)
        {
            bool b = false;
            if (v1 == "")
            {
                b = true;
                ErrowInfo = string.Format("工单号不能为空！");

            }
            else if (!this.exists("SELECT * FROM WORKORDER_MST WHERE WOID='" + v1 + "'"))
            {
                b = true;
                ErrowInfo = string.Format("工单号" + "{0}" + " 不存在系统中！", v1);

            }
            return b;

        }
        public bool DELEGATE_JUAGE(string VALUE, string SN, USE_DELEGATE dele)
        {
            bool b = false;
            b = dele(VALUE, SN);
            return b;

        }
        public bool DELEGATE_JUAGE_T(string VALUE,USE_DELEGATE_T dele)
        {
            bool b = false;
            b = dele(VALUE);
            return b;

        }
        public bool JUAGE_STID(string v1, string v2)
        {
            bool b = false;
            if (!this.exists ("SELECT * FROM STEP WHERE STID='"+v1+"'"))
            {
                b = true;
                ErrowInfo = string.Format("项次" + "{0}" + " 站别编号不存在系统中！", v2);

            }
            return b;

        }
        public bool JUDGE_STEP_ID(string v1, string v2)
        {
            bool b = false;
            if (!this.exists("SELECT * FROM STEP WHERE STEP_ID='" + v1 + "'"))
            {
                b = true;
                ErrowInfo = string.Format("项次" + "{0}" + " 站别代码不存在系统中！", v2);

            }
            return b;

        }
        public bool JUAGE_BAD_PHENOMENON_GROUP(string VALUES, string SN)
        {
            bool b = false;
            if (!this.exists("SELECT * FROM BAD_PHENOMENON_GROUP WHERE BAD_PHENOMENON_GROUP_ID='" + VALUES + "'"))
            {
                b = true;
                ErrowInfo = string.Format("项次" + "{0}" + " 不良现象群组代码不存在系统中！", SN);

            }
            return b;

        }
        public bool JUAGE_BAD_PHENOMENON(string VALUES, string SN)
        {
            bool b = false;
            if (!this.exists("SELECT * FROM BAD_PHENOMENON WHERE BAD_PHENOMENON_ID='" + VALUES + "'"))
            {
                b = true;
                ErrowInfo = string.Format("项次" + "{0}" + " 不良现象代码不存在系统中！", SN);

            }
            return b;

        }
        public bool JUAGE_BAD_PHENOMENON_BECAUSE(string VALUES, string SN)
        {
            bool b = false;
            if (VALUES !="" && !this.exists("SELECT * FROM BAD_PHENOMENON_BECAUSE WHERE BAD_PHENOMENON_BECAUSE_ID='" + VALUES + "'"))
            {
                b = true;
                ErrowInfo = string.Format("项次" + "{0}" + " 不良现象原因不存在系统中！", SN);

            }
            return b;

        }
        public bool JUAGE_PHONE(string v1, string v2)
        {
            bool b = false;
            if (checkphone(v1) == false)
            {
                b = true;
                ErrowInfo = "项次" + v2 + " 电话号码只能输入数字！";

            }
            return b;

        }
        public bool JUAGE_FAX(string v1, string v2)
        {
            bool b = false;
            if (checkphone(v1) == false)
            {
                b = true;

                ErrowInfo = string.Format("项次" + "{0}" + " 传真号码只能输入数字！", v2);

            }
            return b;

        }
        public bool JUAGE_POSTCODE(string v1, string v2)
        {
            bool b = false;
            if (checkphone(v1) == false)
            {
                b = true;
                ErrowInfo = string.Format("项次" + "{0}" + " 邮政编码只能输入数字！", v2);

            }
            return b;

        }
        public bool JUAGE_QQ(string v1, string v2)
        {
            bool b = false;
            if (this.checkphone(v1) == false)
            {
                b = true;
                ErrowInfo = string.Format("项次" + "{0}" + " QQ号码只能输入数字！", v2);

            }
            return b;

        }
        #endregion
        #region  执行SqlCommand命令
        /// <summary>
        /// 执行SqlCommand
        /// </summary>
        /// <param name="M_str_sqlstr">SQL语句</param>
        /// 
        public void getcom(string M_str_sqlstr)
        {
            SqlConnection sqlcon = this.getcon();
            sqlcon.Open();
            SqlCommand sqlcom = new SqlCommand(M_str_sqlstr, sqlcon);
            sqlcom.ExecuteNonQuery();
            sqlcom.Dispose();
            sqlcon.Close();
            sqlcon.Dispose();
        }
        #endregion
        #region getcoms
        public static void getcoms(string M_str_sqlstr)
        {
            basec bc = new basec();
            SqlConnection sqlcon = bc.getcon();
            sqlcon.Open();
            SqlCommand sqlcom = new SqlCommand(M_str_sqlstr, sqlcon);
            sqlcom.ExecuteNonQuery();
            sqlcom.Dispose();
            sqlcon.Close();
            sqlcon.Dispose();
        }
        #endregion

        #region  创建DataSet对象
        /// <summary>
        /// 创建一个DataSet对象
        /// </summary>
        /// <param name="M_str_sqlstr">SQL语句</param>
        /// <param name="M_str_table">表名</param>
        /// <returns>返回DataSet对象</returns>
        public DataSet getds(string M_str_sqlstr, string M_str_table)
        {
            SqlConnection sqlcon = this.getcon();
            SqlDataAdapter sqlda = new SqlDataAdapter(M_str_sqlstr, sqlcon);
            DataSet myds = new DataSet();
            sqlda.Fill(myds, M_str_table);
            return myds;
        }
        #endregion

        #region  创建SqlDataReader对象
        /// <summary>
        /// 创建一个SqlDataReader对象
        /// </summary>
        /// <param name="M_str_sqlstr">SQL语句</param>
        /// <returns>返回SqlDataReader对象</returns>
        public SqlDataReader getread(string M_str_sqlstr)
        {
            SqlConnection sqlcon = this.getcon();
            SqlCommand sqlcom = new SqlCommand(M_str_sqlstr, sqlcon);
            sqlcon.Open();
            SqlDataReader sqlread = sqlcom.ExecuteReader(CommandBehavior.CloseConnection);
            return sqlread;
        }
        #endregion

        public DataTable table(string M_str_sql)
        {
            SqlConnection sqlcon = this.getcon();
            SqlCommand sqlcmd = new SqlCommand(M_str_sql, sqlcon);
            sqlcon.Open();
            SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            sqlcon.Close();
            GC.Collect();
            return dt;
        }
  
        public DataTable getdt(string M_str_sql)
        {
            SqlConnection sqlcon = this.getcon();
            SqlCommand sqlcom = new SqlCommand(M_str_sql, sqlcon);
            SqlDataAdapter da = new SqlDataAdapter(sqlcom);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;

        }
        public static DataTable getdts(string M_str_sql)
        {
            basec bc = new basec();
            SqlConnection sqlcon = bc.getcon();
            SqlCommand sqlcom = new SqlCommand(M_str_sql, sqlcon);
            SqlDataAdapter da = new SqlDataAdapter(sqlcom);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;

        }
        public SqlDataAdapter getda(string M_str_sql)
        {
            SqlConnection sqlcon = this.getcon();
            SqlCommand sqlcom = new SqlCommand(M_str_sql, sqlcon);
            SqlDataAdapter da = new SqlDataAdapter(sqlcom);
            return da;


        }
   
        #region FROM_RIGHT_UNTIL_CHAR
        public string FROM_RIGHT_UNTIL_CHAR(string STRING_VALUE, int CHAR_VALUE)
        {
            string v = "";
            if (STRING_VALUE.Length > 0)
            {

                for (int i = STRING_VALUE.Length - 1; i > 0; i--)
                {
                    int p = Convert.ToInt32(STRING_VALUE[i]);
                    if (p != CHAR_VALUE)
                    {
                        v = STRING_VALUE[i] + v;
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
        #region JuageCurrentDateIFAboveDeliveryDate
        public bool JuageCurrentDateIFAboveDeliveryDate(string CurrentDate, string DeliveryDate)
        {
            bool b = false;
            if (CurrentDate != "" && DeliveryDate != "")
            {
                if (Convert.ToDateTime(CurrentDate) >= Convert.ToDateTime(DeliveryDate).AddDays(+1))
                {
                    return true;
                }

            }
            return b;
        }
        #endregion
        #region 编号YM
        public string numYM(int digit, int wcodedigit, string wcode, string sql, string tbColumns, string prifix)
        {
            string year, month;
            year = DateTime.Now.ToString("yy");
            month = DateTime.Now.ToString("MM");
            string P_str_Code, t, r, sql1, q = "";
            int P_int_Code, w, w1;

            sql1 = sql + " WHERE YEAR='" + year + "' AND  MONTH='" + month + "'";
            SqlDataReader sqlread = this.getread(sql1);
            DataTable dt = this.getdt(sql1);
            sqlread.Read();
            if (sqlread.HasRows)
            {
                P_str_Code = Convert.ToString(dt.Rows[(dt.Rows.Count - 1)][tbColumns]);
                w1 = digit - wcodedigit;
                P_int_Code = Convert.ToInt32(P_str_Code.Substring(w1, wcodedigit)) + 1;
                t = Convert.ToString(P_int_Code);
                w = wcodedigit - t.Length;
                if (w >= 0)
                {
                    while (w >= 1)
                    {
                        q = q + "0";
                        w = w - 1;

                    }
                    r = prifix + year + month + q + P_int_Code;
                }
                else
                {
                    r = "Exceed Limited";

                }

            }
            else
            {
                r = prifix + year + month + wcode;
            }
            sqlread.Close();
            return r;
        }
        #endregion
        #region 编号YMD
        public string numYMD(int digit, int wcodedigit, string wcode, string sql, string tbColumns, string prifix)
        {
            string year, month, day;
            year = DateTime.Now.ToString("yy");
            month = DateTime.Now.ToString("MM");
            day = DateTime.Now.ToString("dd");
            string P_str_Code, t, r, sql1, q = "";
            int P_int_Code, w, w1;

            sql1 = sql + " WHERE YEAR='" + year + "' AND  MONTH='" + month + "' AND DAY='" + day + "'";
            SqlDataReader sqlread = this.getread(sql1);
            DataTable dt = this.getdt(sql1);
            sqlread.Read();
            if (sqlread.HasRows)
            {
                P_str_Code = Convert.ToString(dt.Rows[(dt.Rows.Count - 1)][tbColumns]);
                w1 = digit - wcodedigit;
                P_int_Code = Convert.ToInt32(P_str_Code.Substring(w1, wcodedigit)) + 1;
                t = Convert.ToString(P_int_Code);
                w = wcodedigit - t.Length;
                if (w >= 0)
                {
                    while (w >= 1)
                    {
                        q = q + "0";
                        w = w - 1;

                    }
                    r = prifix + year + month + day + q + P_int_Code;
                }
                else
                {
                    r = "Exceed Limited";

                }

            }
            else
            {
                r = prifix + year + month + day + wcode;
            }
            sqlread.Close();
            return r;
        }
        #endregion
        #region 编号NOYMD
        public string numNOYMD(int digit, int wcodedigit, string wcode, string sql, string tbColumns, string prifix)
        {

            string year, month, day;
            year = DateTime.Now.ToString("yy");
            month = DateTime.Now.ToString("MM");
            day = DateTime.Now.ToString("dd");
            string P_str_Code, t, r, sql1, q = "";
            int P_int_Code, w, w1;
            sql1 = sql;
            DataTable dt = this.getdt(sql1);
            if (dt.Rows.Count > 0)
            {
                P_str_Code = Convert.ToString(dt.Rows[(dt.Rows.Count - 1)][tbColumns]);
                w1 = digit - wcodedigit;
                P_int_Code = Convert.ToInt32(P_str_Code.Substring(w1, wcodedigit)) + 1;
                t = Convert.ToString(P_int_Code);
                w = wcodedigit - t.Length;
                if (w >= 0)
                {
                    while (w >= 1)
                    {
                        q = q + "0";
                        w = w - 1;

                    }
                    r = prifix + q + P_int_Code;
                }
                else
                {
                    r = "Exceed Limited";

                }

            }
            else
            {
                r = prifix  + wcode;
            }
            return r;
        }
        #endregion
        #region 编号YMFREE
        public string numYMFREE(int digit, int wcodedigit, string wcode, string sql, string tbColumns, string prifix)
        {
            string year, month, day;
            year = DateTime.Now.ToString("yy");
            month = DateTime.Now.ToString("MM");
            day = DateTime.Now.ToString("dd");
            string P_str_Code, t, r, sql1, q = "";
            int P_int_Code, w, w1;

            sql1 = sql;
            DataTable dt = this.getdt(sql1);
            if (dt.Rows.Count > 0)
            {
                P_str_Code = Convert.ToString(dt.Rows[(dt.Rows.Count - 1)][tbColumns]);
                w1 = digit - wcodedigit;
                P_int_Code = Convert.ToInt32(P_str_Code.Substring(w1, wcodedigit)) + 1;
                t = Convert.ToString(P_int_Code);
                w = wcodedigit - t.Length;
                if (w >= 0)
                {
                    while (w >= 1)
                    {
                        q = q + "0";
                        w = w - 1;

                    }
                    r = prifix + year + month + q + P_int_Code;
                }
                else
                {
                    r = "Exceed Limited";

                }

            }
            else
            {
                r = prifix + year + month + wcode;
            }
            return r;
        }
        #endregion
    
        #region RETURN_MONTH
        public string RETURN_MONTH(string MONTH_VALUE)
        {
            string GETID = "";
            if (MONTH_VALUE == "01")
            {
                GETID = "A";
            }
            else if (MONTH_VALUE == "02")
            {
                GETID = "B";
            }
            else if (MONTH_VALUE == "03")
            {
                GETID = "C";
            }
            else if (MONTH_VALUE == "04")
            {
                GETID = "D";
            }
            else if (MONTH_VALUE == "05")
            {
                GETID = "E";
            }
            else if (MONTH_VALUE == "06")
            {
                GETID = "F";
            }
            else if (MONTH_VALUE == "07")
            {
                GETID = "G";
            }
            else if (MONTH_VALUE == "08")
            {
                GETID = "H";
            }
            else if (MONTH_VALUE == "09")
            {
                GETID = "I";
            }
            else if (MONTH_VALUE == "10")
            {
                GETID = "J";
            }
            else if (MONTH_VALUE == "11")
            {
                GETID = "K";
            }
            else if (MONTH_VALUE == "12")
            {
                GETID = "L";
            }

            return GETID;
        }
        #endregion
        #region 编号Restriction
        public string Restriction(int digit, int wcodedigit, string wcode, string sql, string tbColumns, string prifix)
        {
            string year, month, day;
            year = DateTime.Now.ToString("yy");
            month = DateTime.Now.ToString("MM");
            day = DateTime.Now.ToString("dd");
            string P_str_Code, t, r, sql1, q = "";
            int P_int_Code, w, w1;

            sql1 = sql;
            SqlDataReader sqlread = this.getread(sql1);
            DataTable dt = this.getdt(sql1);
            sqlread.Read();
            if (sqlread.HasRows)
            {
                P_str_Code = Convert.ToString(dt.Rows[(dt.Rows.Count - 1)][tbColumns]);
                w1 = digit - wcodedigit;
                P_int_Code = Convert.ToInt32(P_str_Code.Substring(w1, wcodedigit)) + 1;
                t = Convert.ToString(P_int_Code);
                w = wcodedigit - t.Length;
                if (w >= 0)
                {
                    while (w >= 1)
                    {
                        q = q + "0";
                        w = w - 1;

                    }
                    r = prifix + year + month + day + q + P_int_Code;
                }
                else
                {
                    r = "Exceed Limited";

                }

            }
            else
            {
                r = prifix + year + month + day + wcode;
            }
            sqlread.Close();
            return r;
        }
        #endregion
        #region  GET_IFExecutionSUCCESS_HINT_INFO
        public string  GET_IFExecutionSUCCESS_HINT_INFO(bool SET_IFExecutionSUCCESS)
        {
            string v = "";
            if (SET_IFExecutionSUCCESS == true)
            {

                v = "已保存成功!";
            }
            return v;
        }
            #endregion
        #region yesno
        public int yesno(string vars)
        {
            int k = 1;
            int i;
            for (i = 0; i < vars.Length; i++)
            {
                int p = Convert.ToInt32(vars[i]);
                if (p >= 48 && p <= 57 || p == 46)
                {
                    k = 1;
                }
                else
                {
                    k = 0;
                    ErrowInfo = vars + " 只能输入数字";
                    break;
                }

            }
            return k;
        }
        #endregion
        #region yesno_HAVE_PERCENT
        public int yesno_HAVE_PERCENT(string vars)
        {
            int k = 1;
            int i;
            for (i = 0; i < vars.Length; i++)
            {
                int p = Convert.ToInt32(vars[i]);
                if (p >= 48 && p <= 57 || p == 46 || p==37)
                {
                    k = 1;
                }
                else
                {
                    k = 0;
                    ErrowInfo = vars + " 只能输入数字";
                    break;
                }

            }
            return k;
        }
        #endregion
        #region yesno_have_diagonal
        public int yesno_have_diagonal(string vars)
        {
            int k = 1;
            int i;
            for (i = 0; i < vars.Length; i++)
            {
                int p = Convert.ToInt32(vars[i]);
                if (p >= 48 && p <= 57 || p == 46|| p==47)
                {
                    k = 1;
                }
                else
                {
                    k = 0;
                    ErrowInfo = vars + " 只能输入数字";
                    break;
                }

            }

            return k;

        }
        #endregion
        #region JUAGE_IF_EXISTS_DIAGONAL
        public bool  JUAGE_IF_EXISTS_DIAGONAL(string vars)
        {
            bool b = false;
            int i;
            for (i = 0; i < vars.Length; i++)
            {
                int p = Convert.ToInt32(vars[i]);
                if (p == 47)
                {
                    b = true;
                    break;
                }
            }
            return b;
        }
        #endregion
        #region yesno1
        public int yesno1(string vars)
        {
            int k = 1;
            int i;
            for (i = 0; i < vars.Length; i++)
            {
                int p = Convert.ToInt32(vars[i]);
                if (p >= 48 && p <= 57)
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
        #endregion
        #region checkphone
        public bool checkphone(string vars)
        {
            bool k = true;
            int i;
            for (i = 0; i < vars.Length; i++)
            {
                int p = Convert.ToInt32(vars[i]);
                if (p >= 48 && p <= 57 || p == 46 || p == 45)
                {

                }
                else
                {
                    k = false;
                    break;
                }

            }

            return k;

        }
        #endregion
        #region checkEMAIL
        public bool checkEmail(string vars)
        {
            bool k = true;
            int i;
            for (i = 0; i < vars.Length; i++)
            {
                int p = Convert.ToInt32(vars[i]);
                if (p >= 48 && p <= 57 || p == 46 || p >= 64 && p <= 90 || p >= 97 && p <= 122)
                {

                }
                else
                {
                    k = false;
                    break;
                }

            }
            return k;
        }
        #endregion
        #region checkNumber
        public bool checkNumber(string vars)
        {
            bool k = false;
            int i;
            for (i = 0; i < vars.Length; i++)
            {
                int p = Convert.ToInt32(vars[i]);
                if (p >= 48 && p <= 57)
                {
                    k = true;
                    break;
                }
            }
            return k;
        }
        #endregion
        #region checkLetter
        public bool checkLetter(string vars)
        {
            bool k = false;
            int i;
            for (i = 0; i < vars.Length; i++)
            {
                int p = Convert.ToInt32(vars[i]);
                if (p >= 65 && p <= 90 || p >= 97 && p <= 122)
                {
                    k = true;
                    break;
                }
            }
            return k;
        }
        #endregion

        #region getstoragetable
        public DataTable getstoragetable()
        {
            DataTable dtk = new DataTable();
            dtk.Columns.Add("品号", typeof(string));
            dtk.Columns.Add("EUJ料号", typeof(string));
            dtk.Columns.Add("品名", typeof(string));
            dtk.Columns.Add("客户料号", typeof(string));
            dtk.Columns.Add("规格", typeof(string));
            dtk.Columns.Add("单位", typeof(string));
            dtk.Columns.Add("仓库", typeof(string));
            dtk.Columns.Add("批号", typeof(string));
            dtk.Columns.Add("库存数量", typeof(string));
            return dtk;
        }
        #endregion

        #region getstoragecount
        public DataTable getstoragecount()
        {
            int s1, s2;
            DataTable dtk = this.getstoragetable();
            DataTable dtk1 = new DataTable();
            DataTable dtk2 = new DataTable();
            string sqlk1 = @"
select a.wareid AS WAREID,b.wname AS WNAME,a.storageID AS STORAGEID,
c.storageNAME AS STORAGENAME,A.BATCHID AS BATCHID,sum(a.gecount)
AS GECOUNT from gode a
left join wareinfo b on a.wareid=b.wareid left join storageinfo c
 on c.storageid=a.storageid group 
 by a.wareid,b.wname,A.STORAGEID,A.BATCHID,C.STORAGENAME order by a.wareid,A.storageid,a.batchid";

            string sqlk2 = @"select WAREID AS WAREID,STORAGEID AS STORAGEID,BATCHID AS BATCHID,sum(MRcount) AS MRCOUNT from MATERE
GROUP BY WAREID,STORAGEID,BATCHID order by wareid,storageid, BATCHID";

            dtk1 = this.getdt(sqlk1);
            dtk2 = this.getdt(sqlk2);

            for (s1 = 0; s1 < dtk1.Rows.Count; s1++)
            {
                decimal d1 = 0;
                string z = "";
                decimal dec1 = 0;
                for (s2 = 0; s2 < dtk2.Rows.Count; s2++)
                {
                    string v1 = dtk1.Rows[s1]["WAREID"].ToString();
                    string v2 = dtk1.Rows[s1]["STORAGEID"].ToString();
                    string v3 = dtk1.Rows[s1]["BATCHID"].ToString();
                    string v4 = dtk2.Rows[s2]["WAREID"].ToString();
                    string v5 = dtk2.Rows[s2]["STORAGEID"].ToString();
                    string v6 = dtk2.Rows[s2]["BATCHID"].ToString();
                    if (v1 == v4 && v2 == v5 && v3 == v6)
                    {

                        dec1 = (decimal.Parse(dtk1.Rows[s1]["GECOUNT"].ToString())) - (decimal.Parse(dtk2.Rows[s2]["MRCOUNT"].ToString()));
                        z = Convert.ToString(dec1);
                        break;
                    }

                }

                if (z != "")
                {

                    d1 = decimal.Parse(z);

                }
                else
                {

                    d1 = decimal.Parse(dtk1.Rows[s1]["GECOUNT"].ToString());

                }
                if (d1 != 0)
                {
                    DataRow dr = dtk.NewRow();
                    dr["品号"] = dtk1.Rows[s1]["WAREID"].ToString();
                    dr["品名"] = dtk1.Rows[s1]["WNAME"].ToString();
                    DataTable dtx2 = this.getdt("select * from wareinfo where wareid='" + dtk1.Rows[s1]["WAREID"].ToString() + "'");
                    dr["规格"] = dtx2.Rows[0]["Spec"].ToString();
                    dr["单位"] = dtx2.Rows[0]["UNIT"].ToString();
                    dr["仓库"] = dtk1.Rows[s1]["STORAGENAME"].ToString();
                    dr["批号"] = dtk1.Rows[s1]["BATCHID"].ToString();
                    dr["库存数量"] = d1;
                    dtk.Rows.Add(dr);

                }


            }

            return dtk;

        }
        #endregion

        #region juagestoragecount
        public bool JuageDeleteCount_MoreThanStorageCount(string GodEID)
        {
            int i;
            bool z = false;
            DataTable dt6 = this.getdt(@"
select A.WAREID AS WAREID,A.STORAGEID AS STORAGEID,B.STORAGENAME AS STORAGENAME,A.BATCHID AS BATCHID,
SUM(A.GECOUNT) as GECOUNT FROM GODE A LEFT JOIN STORAGEINFO B ON 
A.STORAGEID=B.STORAGEID  WHERE A.GODEID='" + GodEID + "' GROUP BY A.WAREID,A.STORAGEID,B.STORAGENAME,"
           + " A.BATCHID ORDER BY A.WAREID,A.STORAGEID,A.BATCHID ASC");
            if (dt6.Rows.Count > 0)
            {
                for (i = 0; i < dt6.Rows.Count; i++)
                {
                    string c1, c2, c3;
                    c1 = dt6.Rows[i]["WAREID"].ToString();
                    c2 = dt6.Rows[i]["STORAGENAME"].ToString();
                    c3 = dt6.Rows[i]["BATCHID"].ToString();
                    DataRow[] dr = this.getstoragecount().Select("品号='" + c1 + "' and 仓库='" + c2 + "'AND 批号='" + c3 + "'");
                    if (dr.Length > 0)
                    {
                        if (decimal.Parse(dr[0]["库存数量"].ToString()) < decimal.Parse(dt6.Rows[i]["GECOUNT"].ToString()))
                        {

                            ErrowInfo = "品号:" + dt6.Rows[i][0].ToString() + " 库存不足，不允许编辑或删除该单据";
                            z = true;
                            break;
                        }
                    }
                    else
                    {
                        ErrowInfo = "品号:" + dt6.Rows[i][0].ToString() + " 库存不足，不允许编辑或删除该单据";
                        z = true;
                        break;
                    }

                }
            }
            return z;
        }
        #endregion

        #region juagedate
        public bool juagedate(string StartDate, string EndDate)
        {
            bool b = true;
            if (StartDate != "" && EndDate != "")
            {
                if (Convert.ToDateTime(StartDate) <= Convert.ToDateTime(EndDate))
                {

                }
                else
                {
                    ErrowInfo = "截止日期需大于起始日期！";
                    return false;

                }
            }
            return b;
        }
        #endregion
        #region juagedate
        public bool juageStartDateAndEndDatedate(string StartDate, string EndDate)
        {
            bool b = false ;
            if (StartDate != "" && EndDate != "")
            {
                if (Convert.ToDateTime(StartDate) > Convert.ToDateTime(EndDate))
                {
                    ErrowInfo = "截止日期需大于起始日期！";
                    b = true;
                }
            }
            return b;
        }
        #endregion
        #region exists
        public bool exists(string sql)
        {
            DataTable dtx1 = this.getdt(sql);
            if (dtx1.Rows.Count > 0)
                return true;
            else
                return false;
        }
        #endregion
        #region Exists
        public bool exists(string TableName, string ColumnName, string ColumnValue,string REMARK)
        {
           DataTable dt = new DataTable();
           dt = this.getdt("SELECT *  FROM " + TableName + " WHERE "+ColumnName +"='"+ColumnValue +"'");
            if (dt.Rows.Count > 0)
            {
                MessageBox.Show(REMARK +"", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return true;
                
            }
            return false;
        }
        #endregion
        #region maxstoragecount
        public DataTable getmaxstoragecount(string wareid)
        {
            DataTable dt = this.getstoragecount();
            DataTable dtu1 = new DataTable();
            DataRow[] dr = dt.Select("品号='" + wareid + "'");
            if (dr.Length > 0)
            {
                DataTable dtu = this.getstoragetable();
                for (i = 0; i < dr.Length; i++)
                {
                    DataRow dr1 = dtu.NewRow();
                    dr1["品号"] = dr[i]["品号"].ToString();
                    dr1["品名"] = dr[i]["品名"].ToString();
                    dr1["仓库"] = dr[i]["仓库"].ToString();
                    dr1["批号"] = dr[i]["批号"].ToString();
                    dr1["库存数量"] = dr[i]["库存数量"].ToString();
                    dtu.Rows.Add(dr1);

                }
                string s1 = "";
                string s2 = "";/*13111501*/
                decimal c1 = 0;
                decimal n = 0;
                string n1 = "";
                string n2 = "";/*13111501*/
                if (dtu.Rows.Count == 1)
                {

                    s1 = dtu.Rows[0]["仓库"].ToString();
                    s2 = dtu.Rows[0]["批号"].ToString();
                    c1 = decimal.Parse(dtu.Rows[0]["库存数量"].ToString());
                }
                else
                {
                    for (int j = 0; j < dtu.Rows.Count; j++)
                    {

                        decimal c2 = decimal.Parse(dtu.Rows[j]["库存数量"].ToString());

                        if (n > c2)
                        {

                        }
                        else if (n == c2)
                        {


                        }
                        else
                        {
                            n = c2;
                            n1 = dtu.Rows[j]["仓库"].ToString();
                            n2 = dtu.Rows[j]["批号"].ToString();

                        }
                    }
                    s1 = n1;
                    s2 = n2;/*13111501*/
                    c1 = n;
                }
                dtu1 = this.getstoragetable();
                DataRow dr2 = dtu1.NewRow();
                dr2["品号"] = dtu.Rows[0]["品号"].ToString();
                dr2["品名"] = dtu.Rows[0]["品名"].ToString();
                dr2["仓库"] = s1;
                dr2["批号"] = s2;
                dr2["库存数量"] = c1;
                dtu1.Rows.Add(dr2);
            }
            return dtu1;
        }
        #endregion
        #region getstorageid
        public string getstorageid(string storagetype)
        {
            string storageid = "";
            DataTable dtx3 = this.getdt("select * from tb_storageinfo where storagetype='" + storagetype + "'");
            if (dtx3.Rows.Count > 0)
            {
                storageid = dtx3.Rows[0][0].ToString();
            }

            return storageid;
        }
        #endregion
        #region checkingWareidAndstorage
        public string CheckingWareidAndStorage(string wareid, string storageType, string batchID)
        {
            string storagecount = "A";
            DataTable dt = this.getstoragecount();
            DataTable dtu1 = new DataTable();
            DataRow[] dr = dt.Select("品号= '" + wareid + "' and 仓库='" + storageType + "' and 批号='" + batchID + "'");
            if (dr.Length > 0)
            {
                storagecount = dr[0]["库存数量"].ToString();
            }
            return storagecount;
        }
        #endregion
        #region getOnlyString
        public string getOnlyString(string sql)
        {
            string s2 = "";
            DataTable dtu2 = this.getdt(sql);
            if (dtu2.Rows.Count > 0)
            {
                s2 = dtu2.Rows[0][0].ToString();

            }

            return s2;
        }
        #endregion
        #region getOnlyString
        public string getOnlyString(string ColumnName, string TableName, string WAREID)
        {
            string s2 = "";
            DataTable dtu2 = basec.getdts("SELECT " + ColumnName + " FROM " + TableName + " WHERE WAREID=" + WAREID);
            if (dtu2.Rows.Count > 0)
            {
                s2 = dtu2.Rows[0][0].ToString();

            }

            return s2;
        }
        #endregion
        #region getOnlyString
        public string getOnlyStringO(string TableName, string SelectColumn, string ColumnName, string ColumnValue)
        {
            string s2 = "";
            DataTable dtu2 = basec.getdts("SELECT " + SelectColumn + " FROM " + TableName + " WHERE " + ColumnName + "='" + ColumnValue + "'");
            if (dtu2.Rows.Count > 0)
            {
                s2 = dtu2.Rows[0][0].ToString();

            }
            return s2;
        }
        #endregion
        #region getprintinfo
        public DataTable getPrintInfo()
        {

            DataTable dtt = new DataTable();
            dtt.Columns.Add("销货单号", typeof(string));
            dtt.Columns.Add("订单号", typeof(string));
            dtt.Columns.Add("项次", typeof(string));
            dtt.Columns.Add("品号", typeof(string));
            dtt.Columns.Add("品名", typeof(string));
            dtt.Columns.Add("套件", typeof(string));
            dtt.Columns.Add("型号", typeof(string));
            dtt.Columns.Add("细节", typeof(string));
            dtt.Columns.Add("皮种", typeof(string));
            dtt.Columns.Add("颜色", typeof(string));
            dtt.Columns.Add("线色", typeof(string));
            dtt.Columns.Add("海棉厚度", typeof(string));
            dtt.Columns.Add("销售单价", typeof(decimal));
            dtt.Columns.Add("折扣率", typeof(decimal));
            dtt.Columns.Add("税率", typeof(decimal));
            dtt.Columns.Add("订单数量", typeof(decimal));
            dtt.Columns.Add("销货数量", typeof(decimal));
            dtt.Columns.Add("未税金额", typeof(decimal), "销售单价*折扣率*销货数量");
            dtt.Columns.Add("税额", typeof(decimal), "销售单价*折扣率*销货数量*税率/100");
            dtt.Columns.Add("含税金额", typeof(decimal), "销售单价*折扣率*销货数量*(1+税率/100)");
            dtt.Columns.Add("客户代码", typeof(string));
            dtt.Columns.Add("客户", typeof(string));
            dtt.Columns.Add("电话", typeof(string));
            dtt.Columns.Add("地址", typeof(string));
            dtt.Columns.Add("订货日期", typeof(string));
            dtt.Columns.Add("交货日期", typeof(string));
            dtt.Columns.Add("加急否", typeof(string));
            dtt.Columns.Add("制单人", typeof(string));
            dtt.Columns.Add("制单日期", typeof(string));
            return dtt;


        }
        #endregion
        #region ask
        public DataTable ask(string sqlcondition, int GROUP, int printselltable)
        {

            string M_str_sql1 = @"select ORID ,SN ,WareID ,OCOUNT ,ORDERDATE,DELIVERYDATE,URGENT,SELLUNITPRICE,DISCOUNTRATE,TAXRATE,CUID FROM TB_ORDER";

            DataTable dtt = this.getPrintInfo();
            DataTable dtx6 = this.getdt(sqlcondition);
            if (dtx6.Rows.Count > 0)
            {
                for (int i1 = 0; i1 < dtx6.Rows.Count; i1++)
                {
                    DataRow dr = dtt.NewRow();
                    dr["销货单号"] = dtx6.Rows[i1]["SEID"].ToString();
                    dr["订单号"] = dtx6.Rows[i1]["ORID"].ToString();
                    if (printselltable == 1)
                    {

                    }
                    else
                    {
                        dr["项次"] = dtx6.Rows[i1]["SN"].ToString();
                    }
                    dr["品号"] = dtx6.Rows[i1]["WAREID"].ToString();
                    DataTable dtx2 = this.getdt("select * from tb_wareinfo where wareid='" + dtx6.Rows[i1]["WAREID"].ToString() + "'");
                    dr["品名"] = dtx2.Rows[0]["WNAME"].ToString();
                    dr["套件"] = dtx2.Rows[0]["ExternalM"].ToString();
                    dr["型号"] = dtx2.Rows[0]["TYPE"].ToString();
                    dr["细节"] = dtx2.Rows[0]["DETAIL"].ToString();
                    dr["皮种"] = dtx2.Rows[0]["Leather"].ToString();
                    dr["颜色"] = dtx2.Rows[0]["COLOR"].ToString();
                    dr["线色"] = dtx2.Rows[0]["StitchingC"].ToString();
                    dr["海棉厚度"] = dtx2.Rows[0]["Thickness"].ToString();
                    if (GROUP == 0)
                    {
                        dr["销货数量"] = dtx6.Rows[i1]["SECOUNT"].ToString();
                        dr["制单人"] = dtx6.Rows[i1]["MAKER"].ToString();
                        dr["制单日期"] = dtx6.Rows[i1]["DATE"].ToString();
                    }
                    else
                    {
                        dr["销货数量"] = dtx6.Rows[i1][3].ToString();

                    }

                    dtt.Rows.Add(dr);

                }
            }

            DataTable dtx4 = this.getdt(M_str_sql1);
            if (dtx4.Rows.Count > 0)
            {
                for (i = 0; i < dtt.Rows.Count; i++)
                {
                    for (int j = 0; j < dtx4.Rows.Count; j++)
                    {
                        if (printselltable == 1)
                        {
                            if (dtt.Rows[i]["订单号"].ToString() == dtx4.Rows[j]["ORID"].ToString() && dtt.Rows[i]["品号"].ToString() == dtx4.Rows[j]["WAREID"].ToString())
                            {
                                dtt.Rows[i]["订单数量"] = dtx4.Rows[j]["OCOUNT"].ToString();
                                dtt.Rows[i]["订货日期"] = dtx4.Rows[j]["ORDERDATE"].ToString();
                                dtt.Rows[i]["交货日期"] = dtx4.Rows[j]["DELIVERYDATE"].ToString();
                                dtt.Rows[i]["加急否"] = dtx4.Rows[j]["URGENT"].ToString();
                                dtt.Rows[i]["销售单价"] = dtx4.Rows[j]["SELLUNITPRICE"].ToString();
                                dtt.Rows[i]["折扣率"] = dtx4.Rows[j]["DISCOUNTRATE"].ToString();
                                dtt.Rows[i]["税率"] = dtx4.Rows[j]["TAXRATE"].ToString();
                                dtt.Rows[i]["客户代码"] = dtx4.Rows[j]["CUID"].ToString();
                                DataTable dtx7 = this.getdt("select * from tb_customerinfo where cuid='" + dtx4.Rows[j]["CUID"].ToString() + "'");
                                dtt.Rows[i]["客户"] = dtx7.Rows[0]["CNAME"].ToString();
                                dtt.Rows[i]["电话"] = dtx7.Rows[0]["PHONE"].ToString();
                                dtt.Rows[i]["地址"] = dtx7.Rows[0]["ADDRESS"].ToString();
                                break;
                            }
                        }
                        else
                        {

                            if (dtt.Rows[i]["订单号"].ToString() == dtx4.Rows[j]["ORID"].ToString() && dtt.Rows[i]["项次"].ToString() == dtx4.Rows[j]["SN"].ToString())
                            {
                                dtt.Rows[i]["订单数量"] = dtx4.Rows[j]["OCOUNT"].ToString();
                                dtt.Rows[i]["订货日期"] = dtx4.Rows[j]["ORDERDATE"].ToString();
                                dtt.Rows[i]["交货日期"] = dtx4.Rows[j]["DELIVERYDATE"].ToString();
                                dtt.Rows[i]["加急否"] = dtx4.Rows[j]["URGENT"].ToString();
                                dtt.Rows[i]["销售单价"] = dtx4.Rows[j]["SELLUNITPRICE"].ToString();
                                dtt.Rows[i]["折扣率"] = dtx4.Rows[j]["DISCOUNTRATE"].ToString();
                                dtt.Rows[i]["税率"] = dtx4.Rows[j]["TAXRATE"].ToString();
                                dtt.Rows[i]["客户代码"] = dtx4.Rows[j]["CUID"].ToString();
                                DataTable dtx7 = this.getdt("select * from tb_customerinfo where cuid='" + dtx4.Rows[j]["CUID"].ToString() + "'");
                                dtt.Rows[i]["客户"] = dtx7.Rows[0]["CNAME"].ToString();
                                dtt.Rows[i]["电话"] = dtx7.Rows[0]["PHONE"].ToString();
                                dtt.Rows[i]["地址"] = dtx7.Rows[0]["ADDRESS"].ToString();
                                break;
                            }

                        }

                    }
                }
            }
            return dtt;
        }
        #endregion
        public DataTable asko(string sql, int Need)
        {
            DataTable dt = new DataTable();

            /*销货单打印数据含项次*/
            string s31 = @"
select A.SEID AS 销货单号,A.ORID AS 订单号,G.ORDERDATE AS 订货日期,C.DELIVERYDATE AS 交货日期,C.URGENT AS 加急否,
A.SN as 项次,E.WareID as 品号,
B.WNAME AS 品名,B.SPEC as 规格,B.UNIT as 单位,B.CWAREID AS 客户料号,
C.SELLUNITPRICE AS 销售单价,C.TAXRATE AS 税率,
SUM(E.MRCount) as 销货数量 ,SUM(E.MRCOUNT*C.SELLUNITPRICE) AS 未税金额,SUM(E.MRCOUNT*C.SELLUNITPRICE*C.TAXRATE/100) 
AS 税额,SUM(E.MRCOUNT*C.SELLUNITPRICE*(1+C.TAXRATE/100)) AS 含税金额,C.CUID as 客户代码,
D.CName as 客户 ,H.PHONE AS 电话,H.ADDRESS AS 地址,F.SELLDATE AS 销货日期,(SELECT ENAME FROM EMPLOYEEINFO WHERE EMID=F.SELLERID )  AS 销货员
from SELLTABLE_DET A 
LEFT JOIN ORDER_DET C ON A.ORID=C.ORID AND A.SN=C.SN
LEFT JOIN CUSTOMERINFO_MST D ON C.CUID=D.CUID
LEFT JOIN MATERE E ON A.SEKEY=E.MRKEY
LEFT JOIN WAREINFO B ON E.WAREID=B.WAREID
LEFT JOIN SELLTABLE_MST F ON A.SEID=F.SEID
LEFT JOIN ORDER_MST G ON A.ORID=G.ORID
LEFT JOIN CUSTOMERINFO_DET H ON D.CUKEY=H.CUKEY";

            string s32 = @" GROUP BY A.SEID,A.ORID,A.SN,E.WAREID,B.WNAME,B.SPEC,B.UNIT,B.CWAREID,
C.SELLUNITPRICE,C.TAXRATE,C.CUID,D.CNAME,F.SELLDATE,F.SELLERID,G.ORDERDATE,C.DELIVERYDATE,C.URGENT,H.PHONE,H.ADDRESS ORDER BY A.SEID,A.SN";

            if (Need == 2)
            {
                dt = this.getdt(s31 + sql + s32);


            }
            return dt;
        }
        #region PrintOrder
        public DataTable PrintOrder(string sqlcondition)
        {

            string M_str_sql1 = @"select * FROM TB_ORDER";
            DataTable dtt = this.getPrintInfo();
            DataTable dtx6 = this.getdt(M_str_sql1 + sqlcondition);
            if (dtx6.Rows.Count > 0)
            {
                for (i = 0; i < dtx6.Rows.Count; i++)
                {
                    DataRow dr = dtt.NewRow();

                    dr["订单号"] = dtx6.Rows[i]["ORID"].ToString();
                    dr["项次"] = dtx6.Rows[i]["SN"].ToString();
                    dr["品号"] = dtx6.Rows[i]["WAREID"].ToString();
                    DataTable dtx2 = this.getdt("select * from tb_wareinfo where wareid='" + dtx6.Rows[i]["WAREID"].ToString() + "'");
                    dr["品名"] = dtx2.Rows[0]["WNAME"].ToString();
                    dr["套件"] = dtx2.Rows[0]["ExternalM"].ToString();
                    dr["型号"] = dtx2.Rows[0]["TYPE"].ToString();
                    dr["细节"] = dtx2.Rows[0]["DETAIL"].ToString();
                    dr["皮种"] = dtx2.Rows[0]["Leather"].ToString();
                    dr["颜色"] = dtx2.Rows[0]["COLOR"].ToString();
                    dr["线色"] = dtx2.Rows[0]["StitchingC"].ToString();
                    dr["海棉厚度"] = dtx2.Rows[0]["Thickness"].ToString();
                    dr["订单数量"] = dtx6.Rows[i]["OCOUNT"].ToString();
                    dr["订货日期"] = dtx6.Rows[i]["ORDERDATE"].ToString();
                    dr["交货日期"] = dtx6.Rows[i]["DELIVERYDATE"].ToString();
                    dr["加急否"] = dtx6.Rows[i]["URGENT"].ToString();
                    dr["客户代码"] = dtx6.Rows[i]["CUID"].ToString();
                    DataTable dtx7 = this.getdt("select * from tb_customerinfo where cuid='" + dtx6.Rows[i]["CUID"].ToString() + "'");
                    dr["客户"] = dtx7.Rows[0]["CNAME"].ToString();
                    dr["电话"] = dtx7.Rows[0]["PHONE"].ToString();
                    dr["地址"] = dtx7.Rows[0]["ADDRESS"].ToString();
                    dtt.Rows.Add(dr);

                }
            }
            return dtt;
        }
        #endregion
        public bool JuageCoxBoxValueNoExists(string [] arr, string b,string RemarkInfo)
        {
            DataTable dtzz = new DataTable();
            dtzz.Columns.Add("X", typeof(string));
            bool b1 = false;
            for (i = 0; i < arr.Length; i++)
            {
                DataRow dr = dtzz.NewRow();
                dr["X"] = arr[i];
                dtzz.Rows.Add(dr);
            }
            DataRow[] dr1 = dtzz.Select("X='" + b + "'");
            if (dr1.Length > 0)
            {

            }
            else if(b=="")
            {
                b1 = true;
                MessageBox.Show(RemarkInfo + " 不能为空！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                b1 = true;
                MessageBox.Show(b+ " 不是预设值！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            return b1;
        }
        public bool JuageIfAllowKEYIN(List<string> list, string b, string RemarkInfo)
        {
            bool b1 = false ;
            if(b=="")
            {
                b1 = true;
                MessageBox.Show(RemarkInfo + " 不能为空！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (JuageIfAllowKEYIN_O(list, b, RemarkInfo))
            {

                b1 = true;
                MessageBox.Show(b + " 不是预设值！"+RemarkInfo, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            return b1;
        }
        public bool JuageIfAllowKEYIN_O(List<string> list, string b, string RemarkInfo)
        {
            bool b1 = true;
            for (i = 0; i < list.Count; i++)
            {
                if (list[i] == b)
                {
                    b1 = false;
                    break;
                }
            }
            return b1;
        }
        public bool juageValueLimits(string sql, string b)
        {
            DataTable dt = this.getdt(sql);
            DataTable dtzz = new DataTable();
            dtzz.Columns.Add("X", typeof(string));
            if (dt.Rows.Count > 0)
            {
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr = dtzz.NewRow();
                    dr["X"] = dt.Rows[i][0].ToString();
                    dtzz.Rows.Add(dr);
                }
            }
            DataRow dr1 = dtzz.NewRow();
            dr1["X"] = "";
            dtzz.Rows.Add(dr1);
            DataRow[] dr2 = dtzz.Select("X='" + b + "'");
            bool b1 = true;
            if (dr2.Length > 0)
            {

            }
            else
            {
                b1 = false;


            }
            return b1;
        }
        #region GetFileName
        public string GetFileName(string sql, string field)
        {
            string v2 = "";
            DataTable dt1 = this.getdt(sql);
            if (dt1.Rows.Count > 0)
            {
                string v1 = dt1.Rows[0][field].ToString();
                for (int j = v1.Length - 1; j >= 0; j--)
                {
                    if (v1[j] != '-')
                    {
                        v2 = v1[j] + v2;
                    }
                    else
                    {
                        break;

                    }
                }
            }
            return v2;
        }
        #endregion
        #region DelImagesFile
        public void DelImagesFile(string path, string sql, string field)
        {
            DataTable dt1 = this.getdt(sql);
            if (dt1.Rows.Count > 0)
            {
                for (i = 0; i < dt1.Rows.Count; i++)
                {
                    string v1 = dt1.Rows[i][field].ToString();
                    string v2 = "";
                    for (int j = v1.Length - 1; j >= 0; j--)
                    {
                        if (v1[j] != '-')
                        {
                            v2 = v1[j] + v2;
                        }
                        else
                        {
                            break;

                        }
                    }
                    string path2 = path + v2;
                    string path3 = path + "50x50-" + v2;
                    string path4 = path + "150x150-" + v2;
                    if (File.Exists(path2))
                    {
                        File.Delete(path2);
                    }
                    if (File.Exists(path3))
                    {
                        File.Delete(path3);
                    }
                    if (File.Exists(path4))
                    {
                        File.Delete(path4);
                    }
                }
            }
        }
        #endregion

        #region GetStorageCOID
        public DataTable GetStorageCOID(string v1)
        {
            DataTable dtk = new DataTable();
            dtk.Columns.Add("WAREID", typeof(string));
            dtk.Columns.Add("COID", typeof(string));
            dtk.Columns.Add("COLOR", typeof(string));
            dtk.Columns.Add("COLORIMAS", typeof(string));

            string sql = @"
SELECT A.WAREID,A.COID,B.COLOR,SUM(A.GECOUNT) ,SUM(A.MRCOUNT),
SUM(A.GECOUNT)-SUM(A.MRCOUNT)  FROM TB_SIZEMANAGE A
LEFT JOIN TB_COLOR B ON A.COID=B.COID
LEFT JOIN TB_SIZE C ON A.SIID=C.SIID
WHERE A.WAREID='" + v1 + "' GROUP BY A.WAREID,A.COID,B.COLOR HAVING SUM(A.GECOUNT)-SUM(A.MRCOUNT)>0 ORDER BY A.WAREID,A.COID,B.COLOR";

            string sql2 = @"SELECT A.WAREID,A.COID,B.COLOR,A.COLORIMAS FROM TB_SELECTCOLOR A
LEFT JOIN TB_COLOR B ON A.COID=B.COID
WHERE A.WAREID='" + v1 + "'";
            DataTable dt = this.getdt(sql);
            DataTable dt2 = this.getdt(sql2);
            if (dt.Rows.Count > 0)
            {
                for (i = 0; i < dt.Rows.Count; i++)
                {

                    DataRow dr = dtk.NewRow();
                    dr["Wareid"] = dt.Rows[i][0].ToString();
                    dr["COID"] = dt.Rows[i][1].ToString();
                    dr["COLOR"] = dt.Rows[i][2].ToString();
                    dtk.Rows.Add(dr);
                }
                for (int i1 = 0; i1 < dtk.Rows.Count; i1++)
                {

                    if (dt2.Rows.Count > 0)
                    {
                        for (j = 0; j < dt2.Rows.Count; j++)
                        {
                            if (dtk.Rows[i1]["wareid"].ToString() == dt2.Rows[j][0].ToString() &&
                                dtk.Rows[i1]["COID"].ToString() == dt2.Rows[j][1].ToString())
                            {

                                dtk.Rows[i1]["COLORIMAS"] = dt2.Rows[j][3].ToString();
                                break;
                            }


                        }

                    }

                }
            }
            return dtk;
        }
        #endregion

        #region GetStorage
        public DataTable GetStorage(string wareid, string coid)
        {
            DataTable dtk = new DataTable();
            dtk.Columns.Add("WAREID", typeof(string));
            dtk.Columns.Add("COID", typeof(string));
            dtk.Columns.Add("COLOR", typeof(string));
            dtk.Columns.Add("SIID", typeof(string));
            dtk.Columns.Add("SIZE", typeof(string));
            dtk.Columns.Add("STORAGECOUNT", typeof(string));

            string sql = @"
SELECT A.WAREID,A.COID,B.COLOR,A.SIID,C.SIZE,SUM(A.GECOUNT) ,SUM(A.MRCOUNT) ,
SUM(A.GECOUNT)-SUM(A.MRCOUNT)  FROM TB_SIZEMANAGE A
LEFT JOIN TB_COLOR B ON A.COID=B.COID
LEFT JOIN TB_SIZE C ON A.SIID=C.SIID
WHERE A.WAREID='" + wareid + "' AND A.COID='" + coid + "' GROUP BY A.WAREID,A.COID,A.SIID,B.COLOR,C.SIZE HAVING SUM(A.GECOUNT)-SUM(A.MRCOUNT)>0 ORDER BY A.WAREID,A.COID,A.SIID";
            DataTable dt = this.getdt(sql);
            if (dt.Rows.Count > 0)
            {
                for (i = 0; i < dt.Rows.Count; i++)
                {

                    DataRow dr = dtk.NewRow();
                    dr["Wareid"] = dt.Rows[i][0].ToString();
                    dr["COID"] = dt.Rows[i][1].ToString();
                    dr["COLOR"] = dt.Rows[i][2].ToString();
                    dr["SIID"] = dt.Rows[i][3].ToString();
                    dr["SIZE"] = dt.Rows[i][4].ToString();
                    dr["STORAGECOUNT"] = dt.Rows[i][7].ToString();
                    dtk.Rows.Add(dr);
                }
            }
            return dtk;
        }
        #endregion

        #region addwater_nm
        /**/
        /// <summary>
        /// 在图片上增加文字水印
        /// </summary>
        /// <param name="Path">原服务器图片路径</param>
        /// <param name="Path_sy">生成的带文字水印的图片路径</param>
        protected void AddWater(string Path, string Path_sy)
        {
            string addText = "1";
            System.Drawing.Image image = System.Drawing.Image.FromFile(Path);
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(image);
            g.DrawImage(image, 0, 0, image.Width, image.Height);
            System.Drawing.Font f = new System.Drawing.Font("Verdana", 60);
            System.Drawing.Brush b = new System.Drawing.SolidBrush(System.Drawing.Color.Green);

            g.DrawString(addText, f, b, 35, 35);
            g.Dispose();

            image.Save(Path_sy);
            image.Dispose();
        }
        #endregion

        #region addwaterpic_nm
        /**/
        /// <summary>
        /// 在图片上生成图片水印
        /// </summary>
        /// <param name="Path">原服务器图片路径</param>
        /// <param name="Path_syp">生成的带图片水印的图片路径</param>
        /// <param name="Path_sypf">水印图片路径</param>
        protected void AddWaterPic(string Path, string Path_syp, string Path_sypf)
        {
            System.Drawing.Image image = System.Drawing.Image.FromFile(Path);
            System.Drawing.Image copyImage = System.Drawing.Image.FromFile(Path_sypf);
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(image);
            g.DrawImage(copyImage, new System.Drawing.Rectangle(image.Width - copyImage.Width, image.Height - copyImage.Height, copyImage.Width, copyImage.Height), 0, 0, copyImage.Width, copyImage.Height, System.Drawing.GraphicsUnit.Pixel);
            g.Dispose();

            image.Save(Path_syp);
            image.Dispose();
        }
        #endregion

        #region makethumbnail_nm
        /**/
        /// <summary>
        /// 生成缩略图
        /// </summary>
        /// <param name="originalImagePath">源图路径（物理路径）</param>
        /// <param name="thumbnailPath">缩略图路径（物理路径）</param>
        /// <param name="width">缩略图宽度</param>
        /// <param name="height">缩略图高度</param>
        /// <param name="mode">生成缩略图的方式</param>    
        /// 

        public void MakeThumbnail(string originalImagePath, string thumbnailPath, int width, int height, string mode)
        {
            System.Drawing.Image originalImage = System.Drawing.Image.FromFile(originalImagePath);

            int towidth = width;
            int toheight = height;

            int x = 0;
            int y = 0;
            int ow = originalImage.Width;
            int oh = originalImage.Height;

            switch (mode)
            {
                case "HW"://指定高宽缩放（可能变形）                
                    break;
                case "W"://指定宽，高按比例                    
                    toheight = originalImage.Height * width / originalImage.Width;
                    break;
                case "H"://指定高，宽按比例
                    towidth = originalImage.Width * height / originalImage.Height;
                    break;
                case "Cut"://指定高宽裁减（不变形）                
                    if ((double)originalImage.Width / (double)originalImage.Height > (double)towidth / (double)toheight)
                    {
                        oh = originalImage.Height;
                        ow = originalImage.Height * towidth / toheight;
                        y = 0;
                        x = (originalImage.Width - ow) / 2;
                    }
                    else
                    {
                        ow = originalImage.Width;
                        oh = originalImage.Width * height / towidth;
                        x = 0;
                        y = (originalImage.Height - oh) / 2;
                    }
                    break;
                default:
                    break;
            }

            //新建一个bmp图片
            System.Drawing.Image bitmap = new System.Drawing.Bitmap(towidth, toheight);

            //新建一个画板
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bitmap);

            //设置高质量插值法
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;

            //设置高质量,低速度呈现平滑程度
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            //清空画布并以透明背景色填充
            g.Clear(System.Drawing.Color.Transparent);

            //在指定位置并且按指定大小绘制原图片的指定部分
            g.DrawImage(originalImage, new System.Drawing.Rectangle(0, 0, towidth, toheight),
                new System.Drawing.Rectangle(x, y, ow, oh),
                System.Drawing.GraphicsUnit.Pixel);

            try
            {
                //以jpg格式保存缩略图
                bitmap.Save(thumbnailPath, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
            catch (System.Exception e)
            {
                throw e;
            }
            finally
            {
                originalImage.Dispose();
                bitmap.Dispose();
                g.Dispose();
            }
        }
        #endregion
   
        public string RETURN_EMPLOYEE_ID(string EMID)
        {
            StringBuilder sqb = new StringBuilder();
            sqb.AppendFormat("SELECT EMPLOYEE_ID FROM EMPLOYEEINFO WHERE EMID='{0}'",EMID );
            string v1 = this.getOnlyString(sqb.ToString());
            return v1;
        }
        public string RETURN_ENMAE_USE_EMID(string EMID)
        {
            StringBuilder sqb = new StringBuilder();
            sqb.AppendFormat("SELECT ENAME FROM EMPLOYEEINFO WHERE EMID='{0}'", EMID);
            string v1 = this.getOnlyString(sqb.ToString());
            return v1;
        }
        public string RETURN_EMID(string EMPLOYEE_ID)
        {
            StringBuilder sqb = new StringBuilder();
            sqb.AppendFormat("SELECT EMID FROM EMPLOYEEINFO WHERE EMPLOYEE_ID='{0}'", EMPLOYEE_ID);
            string v1 = this.getOnlyString(sqb.ToString());
            return v1;
        }
        public string RETURN_CNAME(string PROJECT_ID)
        {
            StringBuilder sqb = new StringBuilder();
            sqb.AppendFormat(@"SELECT B.CNAME FROM PROJECT_INFO A LEFT JOIN CUSTOMERINFO_MST B ON A.CUID=B.CUID");
            sqb.AppendFormat(@" WHERE A.PROJECT_ID='{0}'", PROJECT_ID);
            string v1 = this.getOnlyString(sqb.ToString());
            return v1;
        }
        public string RETURN_CUID_TO_CNAME(string CUID)
        {
            StringBuilder sqb = new StringBuilder();
            sqb.AppendFormat(@"SELECT CNAME FROM CUSTOMERINFO_MST  ");
            sqb.AppendFormat(@" WHERE CUID='{0}'", CUID);
            string v1 = this.getOnlyString(sqb.ToString());
            return v1;
        }
        public string RETURN_CUSTOMER_TYPE(string PROJECT_ID)
        {
            DataTable dt = new DataTable();
            string v1 ="";
            dt =getdt(string.Format(@"
SELECT
A.PROJECT_ID ,
B.CName AS 客户名称,
c.BRAND AS 品牌,
SUBSTRING(C.CUSTOMER_TYPE,1,1) 客户类别 
FROM  CUSTOMERINFO_MST B 
LEFT JOIN CustomerInfo_DET C ON B.CUID =C.CUID 
LEFT JOIN PROJECT_INFO A ON B.CUID=A.CUID AND A.BRAND=C.BRAND
WHERE A.PROJECT_ID='{0}'", PROJECT_ID ));
            if (dt.Rows.Count > 0)
            {

                v1 = dt.Rows[0]["客户类别"].ToString();

            }
            return v1;
        }
        public string RETURN_ENAME(string EMPLOYEE_ID)
        {
            StringBuilder sqb = new StringBuilder();
            sqb.AppendFormat("SELECT ENAME FROM EMPLOYEEINFO WHERE EMPLOYEE_ID='{0}'", EMPLOYEE_ID);
            string v1 = this.getOnlyString(sqb.ToString());
            return v1;
        }
        public string RETURN_SRID(string SAMPLE_ID)
        {
            StringBuilder sqb = new StringBuilder();
            sqb.AppendFormat("SELECT SRID FROM SAMPLE_RELY_LIST WHERE SAMPLE_ID='{0}'", SAMPLE_ID );
            string v1 = this.getOnlyString(sqb.ToString());
            return v1;
        }

        public bool juageOne(string sql)
        {
            bool b = false;
            DataTable dt = this.getdt(sql);
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows.Count == 1)
                {
                    b = true;
                }

            }
            return b;


        }
     
 
  
        public bool JuageSourceStatus(string ORID)
        {

            bool b1 = false;
            string v11 = this.getOnlyString(@"SELECT A.SOURCESTATUS FROM PURCHASE_MST A LEFT JOIN ORDER_MST B 
            ON A.PUID=B.PUID WHERE B.ORID='" + ORID + "'");
            if (!string.IsNullOrEmpty(v11))
            {

                b1 = true;

            }
            return b1;

        }
        public byte[] GetMD5(string Password)
        {
            byte[] Encrypt = HashAlgorithm.Create().ComputeHash(Encoding.Unicode.GetBytes(Password));
            return Encrypt;
        }
        #region JuageOrderPurchaseStatus
        public bool JuageOrderOrPurchaseStatus(string ORIDorPUID, int OrderOrPurchase)
        {
            bool b = true;
            DataTable dt = new DataTable();
            if (OrderOrPurchase == 0)
            {
                dt = this.getdt("SELECT * FROM ORDER_DET WHERE ORID='" + ORIDorPUID + "'");
                foreach (DataRow dr in dt.Rows)
                {
                    if (dr["ORDERSTATUS_DET"].ToString() != "CLOSE")
                    {
                        b = false;
                        break;

                    }
                }

            }
            else
            {
                dt = this.getdt("SELECT * FROM PURCHASE_DET WHERE PUID='" + ORIDorPUID + "'");
                foreach (DataRow dr in dt.Rows)
                {

                    if (dr["PURCHASESTATUS_DET"].ToString() != "CLOSE")
                    {
                        b = false;
                        break;

                    }

                }

            }

            return b;

        }
        #endregion

        #region JuageIfAllowDeleteEMID
        public bool JuageIfAllowDeleteEMID(string EMID)
        {
            bool b = false;
            if (this.exists("SELECT * FROM USERINFO WHERE MAKERID='" + EMID + "' OR EMID='" + EMID + "'"))
            {
                b = true;
                ErrowInfo = "该工号已经在用户信息中使用了，不允许删除！";

            }
            else if (this.exists("SELECT * FROM WAREFILE WHERE MAKERID='" + EMID + "'"))
            {
                b = true;
                ErrowInfo = "该工号已经在上传文件信息中使用了，不允许删除！";

            }
            else if (this.exists("SELECT * FROM PRINTING_OFFER_MST WHERE MAKERID='" + EMID + "'"))
            {
                b = true;
                ErrowInfo = "该工号已经在报价信息中使用了，不允许删除！";

            }
            else if (this.exists("SELECT * FROM PRINTING_OFFER_MST WHERE MAKERID='" + EMID + "'"))
            {
                b = true;
                ErrowInfo = "该工号已经在报价信息中使用了，不允许删除！";
            }
            else if (this.exists("SELECT * FROM PROJECT_INFO WHERE AE_MAKERID_1='" + EMID + "'"))
            {
                b = true;
                ErrowInfo = "该工号已经在项目信息中使用了，不允许删除！";
            }
            else if (this.exists("SELECT * FROM PROJECT_INFO WHERE AE_MAKERID_2='" + EMID + "'"))
            {
                b = true;
                ErrowInfo = "该工号已经在项目信息中使用了，不允许删除！";
            }
            else if (this.exists("SELECT * FROM PROJECT_INFO WHERE AE_MAKERID_3='" + EMID + "'"))
            {
                b = true;
                ErrowInfo = "该工号已经在项目信息中使用了，不允许删除！";
            }
            else if (this.exists("SELECT * FROM PROJECT_INFO WHERE PLANE_MAKERID_1='" + EMID + "'"))
            {
                b = true;
                ErrowInfo = "该工号已经在项目信息中使用了，不允许删除！";
            }
            else if (this.exists("SELECT * FROM PROJECT_INFO WHERE PLANE_MAKERID_2='" + EMID + "'"))
            {
                b = true;
                ErrowInfo = "该工号已经在项目信息中使用了，不允许删除！";
            }
            else if (this.exists("SELECT * FROM PROJECT_INFO WHERE PLANE_MAKERID_3='" + EMID + "'"))
            {
                b = true;
                ErrowInfo = "该工号已经在项目信息中使用了，不允许删除！";
            }
            else if (this.exists("SELECT * FROM PROJECT_INFO WHERE STRUCTURE_MAKERID_1='" + EMID + "'"))
            {
                b = true;
                ErrowInfo = "该工号已经在项目信息中使用了，不允许删除！";
            }
            else if (this.exists("SELECT * FROM PROJECT_INFO WHERE STRUCTURE_MAKERID_2='" + EMID + "'"))
            {
                b = true;
                ErrowInfo = "该工号已经在项目信息中使用了，不允许删除！";
            }
            else if (this.exists("SELECT * FROM PROJECT_INFO WHERE STRUCTURE_MAKERID_3='" + EMID + "'"))
            {
                b = true;
                ErrowInfo = "该工号已经在项目信息中使用了，不允许删除！";
            }
            else if (this.exists("SELECT * FROM PROJECT_INFO WHERE OFFER_MAKERID='" + EMID + "'"))
            {
                b = true;
                ErrowInfo = "该工号已经在项目信息中使用了，不允许删除！";
            }
            else if (this.exists("SELECT * FROM PROJECT_INFO WHERE AUDIT_MAKERID='" + EMID + "'"))
            {
                b = true;
                ErrowInfo = "该工号已经在项目信息中使用了，不允许删除！";
            }
            else if (this.exists("SELECT * FROM PROJECT_INFO WHERE MAKERID='" + EMID + "'"))
            {
                b = true;
                ErrowInfo = "该工号已经在项目信息中使用了，不允许删除！";
            }

            else if (this.exists("SELECT * FROM SAMPLE_RELY_LIST WHERE OWN_MAKERID='" + EMID + "'"))
            {
                b = true;
                ErrowInfo = "该工号已经在样板依赖单中使用了，不允许删除！";
            }
            else if (this.exists("SELECT * FROM SAMPLE_RELY_LIST WHERE PURCHASE_MAKERID='" + EMID + "'"))
            {
                b = true;
                ErrowInfo = "该工号已经在样板依赖单中使用了，不允许删除！";
            }
         
            else if (this.exists("SELECT * FROM SAMPLE_RELY_LIST WHERE PAPER_MAKERID='" + EMID + "'"))
            {
                b = true;
                ErrowInfo = "该工号已经在样板依赖单中使用了，不允许删除！";
            }
            else if (this.exists("SELECT * FROM SAMPLE_RELY_LIST WHERE ACRYLIC_MAKERID='" + EMID + "'"))
            {
                b = true;
                ErrowInfo = "该工号已经在样板依赖单中使用了，不允许删除！";
            }
            else if (this.exists("SELECT * FROM SAMPLE_RELY_LIST WHERE WOOD_IRON_MAKERID='" + EMID + "'"))
            {
                b = true;
                ErrowInfo = "该工号已经在样板依赖单中使用了，不允许删除！";
            }
            else if (this.exists("SELECT * FROM SAMPLE_RELY_LIST WHERE PURCHASE_AUDIT_MAKERID='" + EMID + "'"))
            {
                b = true;
                ErrowInfo = "该工号已经在样板依赖单中使用了，不允许删除！";
            }
            else if (this.exists("SELECT * FROM SAMPLE_RELY_LIST WHERE MAKERID='" + EMID + "'"))
            {
                b = true;
                ErrowInfo = "该工号已经在样板依赖单中使用了，不允许删除！";
            }
            return b;
        }
        #endregion
 
        #region JuageIfAllowDeleteWareID
        public bool JuageIfAllowDeleteWareID(string WareID)
        {
            bool b = false;
            if (this.exists("SELECT * FROM WORKORDER_MST WHERE WareID='" + WareID + "' "))
            {
                b = true;
                ErrowInfo = "该品号已经在工单信息中使用了，不允许删除！";

            }
            return b;
        }
        #endregion

 
        #region JuageIfAllowDeleteCAR_CAID
        public bool JuageIfAllowDeleteCAR_CAID(string CAID)
        {
            bool b = false;
            if (this.exists("SELECT * FROM AUDIT WHERE CAID='" + CAID + "' "))
            {
                b = true;
                ErrowInfo = "该车牌号码已经在年审信息中使用了，不允许删除！";

            }
            else if (this.exists("SELECT * FROM  GAS WHERE CAID='" + CAID + "' "))
            {
                b = true;
                ErrowInfo = "该车牌号码已经在加油信息中使用了，不允许删除！";

            }
            else if (this.exists("SELECT * FROM  GasCardAddFunds  WHERE CAID='" + CAID + "' "))
            {
                b = true;
                ErrowInfo = "该车牌号码已经在加油卡冲值信息中使用了，不允许删除！";

            }
            else if (this.exists("SELECT * FROM  GasCardInfo WHERE CAID='" + CAID + "' "))
            {
                b = true;
                ErrowInfo = "该车牌号码已经在加油卡信息中使用了，不允许删除！";

            }
            else if (this.exists("SELECT * FROM  GODE WHERE CAID='" + CAID + "'"))
            {
                b = true;
                ErrowInfo = "该车牌号码已经在交易数量表信息中使用了，不允许删除！";

            }
            else if (this.exists("SELECT * FROM  INSURE  WHERE CAID='" + CAID + "' "))
            {
                b = true;
                ErrowInfo = "该车牌号码已经在保险费用信息中使用了，不允许删除！";

            }
            else if (this.exists("SELECT * FROM  OTHER WHERE CAID='" + CAID + "' "))
            {
                b = true;
                ErrowInfo = "该车牌号码已经在其它费用信息中使用了，不允许删除！";

            }

            else if (this.exists("SELECT * FROM  REPAIR WHERE CAID='" + CAID + "' "))
            {
                b = true;
                ErrowInfo = "该车牌号码已经在维修费用信息中使用了，不允许删除！";

            }

            else if (this.exists("SELECT * FROM  TOLL  WHERE CAID='" + CAID + "' "))
            {
                b = true;
                ErrowInfo = "该车牌号码已经在路费信息中使用了，不允许删除！";

            }

            else if (this.exists("SELECT * FROM  TollCardInfo  WHERE CAID='" + CAID + "' "))
            {
                b = true;
                ErrowInfo = "该车牌号码已经在路卡信息中使用了，不允许删除！";

            }

            else if (this.exists(@"SELECT * FROM  UCAPPLY_MST  WHERE CAID='" + CAID + "' "))
            {
                b = true;
                ErrowInfo = "该车牌号码已经在用车申请主表信息中使用了，不允许删除！";

            }
            else if (this.exists("SELECT * FROM  UPKEEP  WHERE CAID='" + CAID + "' "))
            {
                b = true;
                ErrowInfo = "该车牌号码已经在保养费用信息中使用了，不允许删除！";

            }

            else if (this.exists("SELECT * FROM  WASH  WHERE CAID='" + CAID + "' "))
            {
                b = true;
                ErrowInfo = "该车牌号码已经在洗车费用信息中使用了，不允许删除！";

            }
            return b;
        }
        #endregion

        #region JuageIfAllowReductionReconcileCAR_APPROVE
        public bool JuageIfAllowReductionReconcileCAR_APPROVE(string UCID)
        {
            bool b = false;
            if (this.exists("SELECT * FROM  GAS WHERE UCID='" + UCID + "' "))
            {
                b = true;
                ErrowInfo = "该用车单号已经在加油信息中使用了，不允许撤审！";

            }

            else if (this.exists("SELECT * FROM  OTHER WHERE UCID='" + UCID + "' "))
            {
                b = true;
                ErrowInfo = "该用车单号已经在其它费用信息中使用了，不允许撤审！";

            }

            else if (this.exists("SELECT * FROM  REPAIR WHERE UCID='" + UCID + "' "))
            {
                b = true;
                ErrowInfo = "该用车单号已经在维修费用信息中使用了，不允许撤审！";

            }

            else if (this.exists("SELECT * FROM  TOLL  WHERE UCID='" + UCID + "' "))
            {
                b = true;
                ErrowInfo = "该用车单号已经在路费信息中使用了，不允许撤审！";

            }

            else if (this.exists("SELECT * FROM  WASH  WHERE UCID='" + UCID + "' "))
            {
                b = true;
                ErrowInfo = "该用车单号已经在洗车费用信息中使用了，不允许撤审！";

            }
            return b;
        }
        #endregion
        public bool checkEMID(string EMID)
        {
            bool ju = true;
            if (!this.exists("select * from EMPLOYEEINFO where EMID='" + EMID + "'"))
            {
                ju = false;
                ErrowInfo = "工号为空或不存在于系统中！";

            }
            return ju;
        }
        public bool checkPLATENUM(string PLATENUM)
        {
            bool ju = true;
            if (!this.exists("SELECT PLATENUM FROM CARINFO WHERE PLATENUM='" + PLATENUM + "'"))
            {
                ju = false;
                ErrowInfo = "车牌号码为空或不存在于系统中！";

            }
            return ju;
        }
        public bool checkUCID(string UCID)
        {
            bool ju = true;
            if (!this.exists("SELECT * FROM UCAPPLY_MST WHERE UCID='" +UCID  + "' AND UCAPPLY_STATUS='APPROVE'"))
            {
                ju = false;
                ErrowInfo = "用车单号为空或不存在于系统中或未审核！";

            }
            return ju;
        }
        public bool checkGASCARDID(string GASCARDID)
        {
            bool ju = true;
            if (!this.exists("SELECT * FROM GASCARDINFO WHERE GASCARDID='" + GASCARDID + "'"))
            {
                ju = false;
                ErrowInfo = "油卡号为空或不存在于系统中！";

            }
            return ju;
        }
        public bool checkTOLLCARDID(string CARDID)
        {
            bool ju = true;
            if (!this.exists("SELECT * FROM TOLLCARDINFO WHERE TOLLCARDID='" + CARDID + "'"))
            {
                ju = false;
                ErrowInfo = "路卡号为空或不存在于系统中！";

            }
            return ju;
        }
        public bool checkOriginalMakerID(string tablename, string ColumnName, string billID, string MAKERID)
        {
            bool ju = true;
            if (this.exists("SELECT MAKERID FROM " + tablename + " WHERE " + ColumnName + "='" + billID + "'"))
            {
                string originalMakerid = this.getOnlyString("SELECT MAKERID FROM " + tablename + " WHERE " + ColumnName + "='" + billID + "'");
                if (MAKERID != originalMakerid)
                {
                    ju = false;
                    ErrowInfo = "只有原始制单人才能修改此单据！";
                }
            }
            return ju;
        }

        #region getstoragetable_toll
        public DataTable getstoragetable_toll()
        {
            DataTable dtk = new DataTable();
            dtk.Columns.Add("路卡编号", typeof(string));
            dtk.Columns.Add("路卡号", typeof(string));
            dtk.Columns.Add("车辆编号", typeof(string));
            dtk.Columns.Add("车牌号码", typeof(string));
            dtk.Columns.Add("余额", typeof(decimal));
            return dtk;
        }
        #endregion

        #region getstoragecount_toll
        public DataTable getstoragecount_toll()
        {
            string sqlk1 = @"
SELECT B.TCID AS 路卡编号,
B.TOLLCARDID AS 路卡号,
A.CAID AS 车辆编号,
C.PlateNum  AS 车牌号码,
CASE WHEN SUM(A.TOLLCARD_GECOUNT) IS NULL THEN 0
ELSE SUM(A.TOLLCARD_GECOUNT)
END 
AS 累计冲值金额,
CASE WHEN SUM(A.TOLLCARD_MRCOUNT) IS NULL THEN 0
ELSE SUM(A.TOLLCARD_MRCOUNT)
END 
AS 累计消费金额,
(
CASE WHEN SUM(A.TOLLCARD_GECOUNT) IS NULL THEN 0
ELSE SUM(A.TOLLCARD_GECOUNT)
END 
-
CASE WHEN SUM(A.TOLLCARD_MRCOUNT) IS NULL THEN 0
 ELSE SUM(A.TOLLCARD_MRCOUNT)
 END 
)
 AS 余额
FROM GODE A
LEFT JOIN TOLLCARDINFO B ON A.CAID=B.CAID  
LEFT JOIN CarInfo C ON A.CAID =C.CAID 
WHERE A.CAID IS NOT NULL  
 GROUP BY B.TCID,B.TOLLCARDID,A.CAID,C.PlateNum
 HAVING 
 (
CASE WHEN SUM(A.TOLLCARD_GECOUNT) IS NULL THEN 0
ELSE SUM(A.TOLLCARD_GECOUNT)
END 
-
CASE WHEN SUM(A.TOLLCARD_MRCOUNT) IS NULL THEN 0
 ELSE SUM(A.TOLLCARD_MRCOUNT)
 END )>0
ORDER BY A.CAID
";
            DataTable dtk1 = this.getdt(sqlk1);
            DataRow dr2 = dtk1.NewRow();
            dr2["路卡号"] = "合计";
            dr2["余额"] = dtk1.Compute("SUM(余额)", "");
            dtk1.Rows.Add(dr2);
            return dtk1;
        }
        #endregion

        #region juagestoragecount_toll
        public bool JuageDeleteCount_MoreThanStorageCount_toll(string TCID, string DELETECOUNT)
        {

            bool z = false;
            string v1 = this.getOnlyString("SELECT TOLLCARDID FROM TOLLCARDINFO WHERE TCID='" + TCID + "'");
            DataRow[] dr = this.getstoragecount_toll().Select("路卡编号='" + TCID + "'");
            if (dr.Length > 0)
            {
                if (decimal.Parse(dr[0]["余额"].ToString()) < decimal.Parse(DELETECOUNT))
                {

                    ErrowInfo = "卡号:" + v1 + " 余额不足，不允许删除该单据";
                    z = true;

                }
            }
            else
            {
                ErrowInfo = "卡号:" + v1 + " 余额不足，不允许删除该单据";
                z = true;

            }
            return z;
        }
        #endregion

        #region juagestoragecount_toll
        public string gettollAmount(string TALLCARDID)
        {

            string z = "";
            DataRow[] dr = this.getstoragecount_toll().Select("路卡号='" + TALLCARDID + "'");
            if (dr.Length > 0)
            {

                z = dr[0]["余额"].ToString();
            }
            return z;
        }
        #endregion

   
        public bool CheckKeyInValueIfNoExistsOrEmpty(string TABLENAME,string COLUMN_NAME,string COLUMN_VALUE,string REMARK)
        {
            bool ju = false;
            if (COLUMN_VALUE == "")
            {

                ju = true;
                MessageBox.Show(REMARK + "为空!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (!this.exists("SELECT *  FROM " + TABLENAME + " WHERE " + COLUMN_NAME + "='" + COLUMN_VALUE + "'"))
            {
                ju = true;
                MessageBox.Show(REMARK+" "+COLUMN_VALUE+ "不存在于系统中！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            return ju;
        }
        public bool CheckKeyInValueIfNoExists(string TABLENAME, string COLUMN_NAME, string COLUMN_VALUE, string REMARK)
        {
            bool ju = false;
            if (COLUMN_VALUE == "")
            {

            }
            else if (!this.exists("SELECT *  FROM " + TABLENAME + " WHERE " + COLUMN_NAME + "='" + COLUMN_VALUE + "'"))
            {
                ju = true;
                MessageBox.Show(REMARK + " " + COLUMN_VALUE + "不存在于系统中！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            return ju;
        }

        public bool CheckKeyInValueIfNoDigitOrEmpty(string Value, string REMARK)
        {
            bool ju = false;
            if (Value == "")
            {

                ju = true;
                MessageBox.Show(REMARK + "为空", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (this.yesno(Value) == 0)
            {

                ju = true;
                MessageBox.Show(REMARK + "不为数字！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
      
            return ju;
        }
        public void  dgvTAB_MOVE(DataGridView dataGridView1)
        {
            int currentcolumnindex = dataGridView1.CurrentCell.ColumnIndex;
            int currentrowindex = dataGridView1.CurrentCell.RowIndex;
            int columncount = dataGridView1.Columns.Count - 1;
            int rowcount = dataGridView1.Rows.Count - 1;
            int i1 = dataGridView1.Columns.Count - 1;
            if (currentcolumnindex != columncount && currentrowindex != rowcount)
            {
                dataGridView1.CurrentCell = dataGridView1[dataGridView1.Columns.Count - 1, dataGridView1.Rows.Count - 1];
            }
            else
            {
                dataGridView1.CurrentCell = dataGridView1[0, 0];
            }

        }
        #region LOWERCASE_TO_CAPITAL
        public string  LOWERCASE_TO_CAPITAL(string v)
        {
            int i;
            string v1 = "";
            if (v == "")
            {
            //MessageBox.Show("不能为空！", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                for (i = 0; i < v.Length; i++)
                {
                    int p = Convert.ToInt32(v[i]);
                    if (p >= 97 && p <= 122)
                    {
                        p = p - 32;
                        v1 = v1 + Convert.ToChar(p);
                    }
                    else
                    {
                        v1 = v1 + Convert.ToChar(p);
                    }

                }
            }
            return v1;
        }
        #endregion
        #region IFEXISTS_LOWERCASE
        public bool IFEXISTS_LOWERCASE(string v)
        {
            bool b = false;
            if (v == "")
            {
                //MessageBox.Show("不能为空！", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                for (i = 0; i < v.Length; i++)
                {
                    int p = Convert.ToInt32(v[i]);
                    if (p >= 97 && p <= 122)
                    {
                        b = true;
                        break;
                    }
                    else
                    {
                     
                    }

                }
            }
            return b;
        }
        #endregion
        #region GET_NOEXISTS_EMPTY_ROW_DT
        public DataTable  GET_NOEXISTS_EMPTY_ROW_DT(DataTable  dt,string Sort,string RowFilter)
        {
            DataView dv = new DataView(dt);
            dv.RowFilter = RowFilter;
            dv.Sort = Sort;
            dt = dv.ToTable();
            return dt;
        }
        #endregion
        #region GET_DT_TO_DV_TO_DT
        public DataTable GET_DT_TO_DV_TO_DT(DataTable dt, string Sort, string RowFilter)
        {
            DataView dv = new DataView(dt);
            dv.RowFilter = RowFilter;
            dv.Sort = Sort;
            dt = dv.ToTable();
            return dt;
        }
        #endregion
        #region RETURN_NOHAVE_REPEAT_DT
        public DataTable RETURN_NOHAVE_REPEAT_DT(DataTable dt, string COLUMN_NAME)
        {
            DataTable dtt = new DataTable();
            dtt.Columns.Add("VALUE", typeof(string));
            if (dt.Rows.Count > 0)
            {
                dt =GET_DT_TO_DV_TO_DT(dt, "", COLUMN_NAME + " IS NOT NULL");
                foreach (DataRow dr1 in dt.Rows)
                {
                    if (dr1[COLUMN_NAME].ToString() == "")
                    {

                    }
                    else
                    {
                        DataTable dtt1 = GET_DT_TO_DV_TO_DT(dtt, "", string.Format("VALUE='{0}'", dr1[COLUMN_NAME].ToString()));
                        if (dtt1.Rows.Count > 0)
                        {

                        }
                        else
                        {
                            DataRow dr = dtt.NewRow();
                            dr["VALUE"] = dr1[COLUMN_NAME].ToString();
                            dtt.Rows.Add(dr);
                        }
                    }
                }
            }

            return dtt;
        }
        #endregion
        #region RETURN_NOHAVE_REPEAT_DT
        public DataTable RETURN_NOHAVE_REPEAT_DT(DataTable dt, string COLUMN_NAME1,string COLUMN_NAME2)
        {
            DataTable dtt = new DataTable();
            dtt.Columns.Add("VALUE1", typeof(string));
            dtt.Columns.Add("VALUE2", typeof(string));
            if (dt.Rows.Count > 0)
            {
                dt = GET_DT_TO_DV_TO_DT(dt, "", COLUMN_NAME1 + " IS NOT NULL AND "+COLUMN_NAME2 +" IS NOT NULL");
                foreach (DataRow dr1 in dt.Rows)
                {
                    if (dr1[COLUMN_NAME1].ToString() == "" || dr1[COLUMN_NAME2].ToString() == "")
                    {

                    }
                    else
                    {
                        sqb = new StringBuilder();
                        sqb.AppendFormat("VALUE1='{0}'", dr1[COLUMN_NAME1].ToString());
                        sqb.AppendFormat(" AND VALUE2='{0}'", dr1[COLUMN_NAME2].ToString());
                        DataTable dtt1 = GET_DT_TO_DV_TO_DT(dtt, "",sqb.ToString ());
                        if (dtt1.Rows.Count > 0)
                        {

                        }
                        else
                        {
                            DataRow dr = dtt.NewRow();
                            dr["VALUE1"] = dr1[COLUMN_NAME1].ToString();
                            dr["VALUE2"] = dr1[COLUMN_NAME2].ToString();
                            dtt.Rows.Add(dr);
                        }
                    }
                }
            }

            return dtt;
        }
        #endregion
        #region GET_NOEMPTY_ROW_COURSE_DT
        public DataTable GET_NOEMPTY_ROW_COURSE_DT(DataTable dt)
        {
            dt=this.GET_NOEXISTS_EMPTY_ROW_DT(dt, "", "科目 IS NOT NULL ");
            return dt;
        }
        #endregion
        #region REMOVE_NAME
        public string REMOVE_NAME(string HAVE_NAME_STRING)
        {
            string v= "";
            if (HAVE_NAME_STRING.Length > 0)
            {

                for (int i = 0; i < HAVE_NAME_STRING.Length; i++)
                {
                    int p = Convert.ToInt32(HAVE_NAME_STRING[i]);
                    if (p != 32)
                    {
                        v = v+ HAVE_NAME_STRING[i];
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
        #region RETURN_APPOINT_UNTIL_CHAR
        public string RETURN_APPOINT_UNTIL_CHAR(string HAVE_NAME_STRING,int START, char C1)
        {
            string v = "";
            if (HAVE_NAME_STRING.Length > 0 && HAVE_NAME_STRING .Length >=START )
            {
                int q = Convert.ToInt32(C1);
                for (int i = START -1; i < HAVE_NAME_STRING.Length; i++)
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
        #region RETURN_UNTIL_CHAR
        public string RETURN_UNTIL_CHAR(string HAVE_NAME_STRING, char C1)
        {
            string v = "";
            if (HAVE_NAME_STRING.Length > 0)
            {
                int q = Convert.ToInt32(C1);
                for (int i = 0; i < HAVE_NAME_STRING.Length; i++)
                {
                    int p = Convert.ToInt32(HAVE_NAME_STRING[i]);
                    
                    if (p !=q)
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
        #region RETURN_FROM_RIGHT_UNTIL_CHAR
        public string RETURN_FROM_RIGHT_UNTIL_CHAR(string STRING_VALUE, char c)
        {
            string v = "";
            if (STRING_VALUE.Length > 0)
            {
            
                for (int i = STRING_VALUE.Length - 1; i > 0; i--)
                {
                    
                    if (STRING_VALUE[i] != c)
                    {
                        v = STRING_VALUE[i] + v;
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
    
        #region GET_NO_ZERO_MONTH
        public string GET_NO_ZERO_MONTH(string MM)
        {
            string v = "";
            if (MM.Length ==2)
            {
                if (MM.Substring(0, 1) == "0")
                {
                    v = MM.Substring(1, 1);
                }
                else
                {
                    v = MM;
                }
            }
            return v;
        }
        #endregion
        #region RETURN_ADD_EMPTY_COLUMN
        public DataTable RETURN_ADD_EMPTY_COLUMN(string TABLE_NAME, string COLUMN_NAME)
        {
            DataTable dtx = this.getdt("SELECT "+COLUMN_NAME+" FROM "+TABLE_NAME );
            DataTable dt = new DataTable();
            dt.Columns.Add(COLUMN_NAME, typeof(string));
            if (dtx.Rows.Count > 0)
            {
                DataRow drx = dt.NewRow();
                drx[COLUMN_NAME] = "";
                dt.Rows.Add(drx);

                foreach (DataRow dr1 in dtx.Rows)
                {
                    DataRow dr = dt.NewRow();
                    dr[COLUMN_NAME] = dr1[COLUMN_NAME];
                    dt.Rows.Add(dr);

                }
            }
        
            return dt;
        }
        #endregion
        #region RETURN_NO_HAVE_REPEA_DT//获取不含重复行值的数据表
        public DataTable RETURN_ADD_EMPTY_COLUMN(DataTable dtt,string COLUMNS)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("VALUE", typeof(string));
            if (dtt.Rows.Count > 0)
            {
                foreach (DataRow dr1 in dtt.Rows)
                {
                    DataTable dtx = this.GET_DT_TO_DV_TO_DT(dt, "", string.Format ("VALUE='{0}'",dr1 [COLUMNS ].ToString ()));
                    if (dtx.Rows.Count > 0)
                    {

                    }
                    else
                    {
                        DataRow dr = dt.NewRow();
                        dr["VALUE"] = dr1[COLUMNS].ToString();
                        dt.Rows.Add(dr);
                    }
                }
            }

            return dt;
        }
        #endregion
        #region RETURN_NATURE_AND_NOW_DT//1/2
        public DataTable RETURN_NATURE_AND_NOW_DT(DataTable NATURE_DT,DataTable NOW_DT,string COLUMNS,int start_now_dt_for,int end_now_dt_for)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("VALUE", typeof(string));
            if (NATURE_DT.Rows.Count > 0)
            {
                foreach (DataRow dr1 in NATURE_DT.Rows)
                {
                    DataRow dr = dt.NewRow();
                    dr["VALUE"] = dr1[COLUMNS].ToString();
                    dt.Rows.Add(dr);
                }
            }
            NOW_DT = GET_DT_TO_DV_TO_DT(NOW_DT, "", "'"+COLUMNS +"' IS NOT NULL ");
            if (NOW_DT.Rows.Count > 0)
            {
                for (i = start_now_dt_for; i < end_now_dt_for; i++)
                {
                    DataTable dtx = this.GET_DT_TO_DV_TO_DT(dt, "", string.Format("VALUE='{0}'", NOW_DT .Rows [i][COLUMNS].ToString()));
                    if (dtx.Rows.Count > 0)
                    {

                    }
                    else
                    {
                        if (NOW_DT.Rows[i][COLUMNS].ToString() == "")
                        {

                        }
                        else
                        {
                            DataRow dr = dt.NewRow();
                            dr["VALUE"] = NOW_DT.Rows[i][COLUMNS].ToString();
                            dt.Rows.Add(dr);
                        }
                    }
                }
            }
            return dt;
        }
        #endregion
        #region RETURN_NATURE_AND_NOW_DT//2/2
        public DataTable RETURN_NATURE_AND_NOW_DT(DataTable NATURE_DT, DataTable NOW_DT, string COLUMNS_NATURE, string COLUMNS_NOW,int start_now_dt_for,
            int end_now_dt_for)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("VALUE", typeof(string));
            if (NATURE_DT.Rows.Count > 0)
            {
                foreach (DataRow dr1 in NATURE_DT.Rows)
                {
                    DataRow dr = dt.NewRow();
                    dr["VALUE"] = dr1[COLUMNS_NATURE].ToString();
                    dt.Rows.Add(dr);
                }
            }
            NOW_DT = GET_DT_TO_DV_TO_DT(NOW_DT, "", "'" + COLUMNS_NOW + "' IS NOT NULL ");
            if (NOW_DT.Rows.Count > 0)
            {
                for (i = start_now_dt_for; i < end_now_dt_for; i++)
                {
                    DataTable dtx = this.GET_DT_TO_DV_TO_DT(dt, "", string.Format("VALUE='{0}'", NOW_DT.Rows[i][COLUMNS_NOW].ToString()));
                    if (dtx.Rows.Count > 0)
                    {

                    }
                    else
                    {
                        if (NOW_DT.Rows[i][COLUMNS_NOW].ToString() == "")
                        {

                        }
                        else
                        {
                            //MessageBox.Show(NOW_DT.Rows[i][COLUMNS_NOW].ToString());
                            DataRow dr = dt.NewRow();
                            dr["VALUE"] = NOW_DT.Rows[i][COLUMNS_NOW].ToString();
                            dt.Rows.Add(dr);
                        }
                    }
                }
            }
            return dt;
        }
        #endregion
        #region load()
        public string load(string HAVE_NAME_STRING, int START, char C1)
        {
            string v = "";
            MessageBox.Show("ok");

            return v;
        }
        #endregion
    }

}

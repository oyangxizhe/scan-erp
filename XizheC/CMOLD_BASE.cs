using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using System.Data.SqlClient;
using XizheC;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Text;
namespace XizheC
{
    public class CMOLD_BASE
    {
        basec bc = new basec();
        #region nature
        private string _MBID;
        public string MBID
        {
            set { _MBID = value; }
            get { return _MBID; }

        }
        private string _ErrowInfo;
        public string ErrowInfo
        {
            set { _ErrowInfo = value; }
            get { return _ErrowInfo; }
        }
        private string _EMID;
        public string EMID
        {
            set { _EMID = value; }
            get { return _EMID; }

        }
        private string _ENAME;
        public string ENAME
        {
            set { _ENAME = value; }
            get { return _ENAME; }

        }
        private string _MAID;
        public string MAID
        {
            set { _MAID = value; }
            get { return _MAID; }
        }
        private string _CUID;
        public string CUID
        {
            set { _CUID = value; }
            get { return _CUID; }
        }
        private string _WEIGHT;
        public string WEIGHT
        {
            set { _WEIGHT = value; }
            get { return _WEIGHT; }
        }
        private string _WAREID;
        public string WAREID
        {
            set { _WAREID = value; }
            get { return _WAREID; }

        }
        private string _CNAME;
        public string CNAME
        {
            set { _CNAME = value; }
            get { return _CNAME; }

        }
        private string _MATERIAL;
        public string MATERIAL
        {
            set { _MATERIAL = value; }
            get { return _MATERIAL; }

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
        private bool _IFExecutionSUCCESS;
        public bool IFExecution_SUCCESS
        {
            set { _IFExecutionSUCCESS = value; }
            get { return _IFExecutionSUCCESS; }

        }
       #endregion
        DataTable dt = new DataTable();
        public string moldNo { set; get; }
        public string wName { set; get; }
        public string remark { set; get; }
        string setsql = @"
SELECT 
ROW_NUMBER() OVER (ORDER BY A.MAID ASC)  AS  项次,
A.CUID,
A.MBID,
A.MAID,
A.MOLDNO  AS 模具编号,
'' AS TOTALCOUNT,
A.MBID AS 编号,
B.CNAME AS 客户名称,
A.WAREID AS 型号,
C.MATERIAL AS 材料,
A.WEIGHT AS 重量,
A.WNAME,
A.REMARK
FROM MOLD_BASE A 
LEFT JOIN CUSTOMERINFO_MST B ON A.CUID=B.CUID
LEFT JOIN MATERIAL C ON A.MAID=C.MAID

";
        string setsqlo = @"
INSERT INTO MOLD_BASE
(
MBID,
CUID,
WAREID,
MAID,
WEIGHT,
MAKERID,
DATE,
MOLDNO,
WNAME,
REMARK
)
VALUES
(
@MBID,
@CUID,
@WAREID,
@MAID,
@WEIGHT,
@MAKERID,
@DATE,
@MOLDNO,
@WNAME,
@REMARK
)
";



        string setsqlt = @"
UPDATE MOLD_BASE SET
CUID=@CUID,
WAREID=@WAREID,
MAID=@MAID,
WEIGHT=@WEIGHT,
MAKERID=@MAKERID,
DATE=@DATE,
MOLDNO=@MOLDNO

";
        string setsqlth = @"

";

        public CMOLD_BASE()
        {
            sql = setsql;
            sqlo = setsqlo;
            sqlt = setsqlt;
            sqlth = setsqlth;
       
        }
        public string GETID(Database database,DbTransaction dbTransaction)
        {
            string v1 = bc.numYM(10, 4, "0001","MOLD_BASE", "MBID", "MB",database,dbTransaction );
            string GETID = "";
            if (v1 != "Exceed Limited")
            {
                GETID = v1;
            }
            return GETID;
        }
        public string GETID()
        {
            string v1 = bc.numYM_NEW(10, 4, "0001", "MOLD_BASE", "MBID", "MB");
            string GETID = "";
            if (v1 != "Exceed Limited")
            {
                GETID = v1;
            }
            return GETID;
        }
        #region save
        public void save(DataTable dt, Database database, DbTransaction dbTransaction)
        {
            string year = DateTime.Now.ToString("yy");
            string month = DateTime.Now.ToString("MM");
            string day = DateTime.Now.ToString("dd");
            string varDate = DateTime.Now.ToString("yyy/MM/dd HH:mm:ss").Replace("-", "/");
            foreach (DataRow dr in dt.Rows)
            {

                CNAME = dr["客户名称"].ToString();
                WAREID = dr["型号"].ToString();
                MATERIAL = dr["材料"].ToString().Trim();
                //MessageBox.Show(dr["客户名称"].ToString()+","+dr["材料"].ToString()+","+ dr["型号"].ToString());
                WEIGHT = dr["重量"].ToString();
                moldNo = dr["模具编号"].ToString();
                wName = dr["wname"].ToString();
                remark = dr["remark"].ToString();
                MBID = dr["MBID"].ToString();//存在数据时的更新
                CUID = bc.getOnlyString("SELECT CUID FROM CUSTOMERINFO_MST WHERE CNAME='" + CNAME + "'", database, dbTransaction);
                MAID = bc.getOnlyString("SELECT MAID FROM MATERIAL WHERE MATERIAL='" + MATERIAL + "'", database, dbTransaction);
                string get_CUID = bc.getOnlyString("SELECT cuid from mold_base WHERE  mbid='" + MBID + "'", database, dbTransaction);
                string get_MAID = bc.getOnlyString("SELECT maid from mold_base WHERE  mbid='" + MBID + "'", database, dbTransaction);
              
                if (!bc.exists("SELECT MBID FROM MOLD_BASE WHERE MBID='" + MBID + "'", database, dbTransaction))
                {

                    //判断数据库是否存在客户名称+材料的项 start

                    /* DataTable dtx1 = basec.getdts(@"
 select * from mold_base where cuid='" + CUID + "' and maid='" + MAID + "'");
                     if (dtx1.Rows.Count > 0)
                     {
                         ErrowInfo = " 客户名称：" + CNAME + " + " + MATERIAL + "已经存在数据库1" + "," + MBID;


                     }
                     else
                     {
                         MBID = GETID(database, dbTransaction);
                         SQlcommandE_MST(sqlo, database, dbTransaction);
                         IFExecution_SUCCESS = true;
                     }*/
                    //判断数据库是否存在客户名称+材料的项，保证此两项的组合唯一 end
                    MBID = GETID(database, dbTransaction);
                    SQlcommandE_MST(sqlo, database, dbTransaction);
                    IFExecution_SUCCESS = true;
                }
                else
                {

                    /*if (CUID != get_CUID || MAID != get_MAID)//修改过CUID或MAID的其中一项目或是两项后要判断是否数据库已经存在相同的内容
                    {
                        ErrowInfo = " 客户名称：" + CNAME + " + " + MATERIAL + "已经存在数据库2 " + " MBID=" + MBID + ",CUID=" + CUID + ",GETCUID=" + get_CUID + "MAID=" + MAID + ",GETMAID=" + get_MAID;


                    }
                    else
                    {
                        SQlcommandE_MST(sqlt + " WHERE MBID='" + MBID + "'", database, dbTransaction);
                        IFExecution_SUCCESS = true;
                    }*/
                    SQlcommandE_MST(sqlt + " WHERE MBID='" + MBID + "'", database, dbTransaction);
                    IFExecution_SUCCESS = true;
                }
            }
  
        }
        public void save( Database database, DbTransaction dbTransaction)
        {
            string year = DateTime.Now.ToString("yy");
            string month = DateTime.Now.ToString("MM");
            string day = DateTime.Now.ToString("dd");
            string varDate = DateTime.Now.ToString("yyy/MM/dd HH:mm:ss").Replace("-", "/");
        


                CUID = bc.getOnlyString("SELECT CUID FROM CUSTOMERINFO_MST WHERE CNAME='" + CNAME + "'", database, dbTransaction);
                MAID = bc.getOnlyString("SELECT MAID FROM MATERIAL WHERE MATERIAL='" + MATERIAL + "'", database, dbTransaction);
                string get_CUID = bc.getOnlyString("SELECT cuid from mold_base WHERE  mbid='" + MBID + "'", database, dbTransaction);
                string get_MAID = bc.getOnlyString("SELECT maid from mold_base WHERE  mbid='" + MBID + "'", database, dbTransaction);
          
                if (!bc.exists("SELECT MBID FROM MOLD_BASE WHERE MBID='" + MBID + "'", database, dbTransaction))
                {

                    //判断数据库是否存在客户名称+材料的项 start

                    DataTable dtx1 = basec.getdts(@"
select * from mold_base where cuid='" + CUID + "' and maid='" + MAID + "'");
                    if (dtx1.Rows.Count > 0)
                    {
                        ErrowInfo = " 客户名称：" + CNAME + " + " + MATERIAL + "已经存在数据库1" + "," + MBID;


                    }
                    else
                    {
                        SQlcommandE_MST(sqlo, database, dbTransaction);
                        IFExecution_SUCCESS = true;
                    }
                    //判断数据库是否存在客户名称+材料的项，保证此两项的组合唯一 end

                }
                else
                {

                    if (CUID != get_CUID || MAID != get_MAID)//修改过CUID或MAID的其中一项目或是两项后要判断是否数据库已经存在相同的内容
                    {
                    ErrowInfo = " 客户名称：" + CNAME + " + " + MATERIAL + "已经存在数据库2" +"MBID="+MBID +",CUID="+CUID+",GETCUID="+get_CUID;



                    }
                    else
                    {
                    ErrowInfo = " 客户名称：" + CNAME + " + " + MATERIAL + "已经存在数据库3" + "MBID=" + MBID + ",CUID=" + CUID + ",GETCUID=" + get_CUID;
                    SQlcommandE_MST(sqlt + " WHERE MBID='" + MBID + "'", database, dbTransaction);
                        IFExecution_SUCCESS = true;
                    }

                }
            

        }
        #endregion
        protected void SQlcommandE_MST(string sql,Database db,DbTransaction dbTransaction)
        {
           string varDate = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss").Replace("-", "/");
           System.Data.Common.DbCommand dbCommand = db.GetSqlStringCommand(sql);
           db.AddParameter(dbCommand, "MBID", DbType.String, ParameterDirection.Input, "MBID", DataRowVersion.Default, MBID );
           db.AddParameter(dbCommand, "CUID", DbType.String, ParameterDirection.Input, "CUID", DataRowVersion.Default, CUID);
           db.AddParameter(dbCommand, "WAREID", DbType.String, ParameterDirection.Input, "WAREID", DataRowVersion.Default,WAREID );
           db.AddParameter(dbCommand, "MAID", DbType.String, ParameterDirection.Input, "MAID", DataRowVersion.Default, MAID);
           db.AddParameter(dbCommand, "WEIGHT", DbType.String, ParameterDirection.Input, "WEIGHT", DataRowVersion.Default, WEIGHT);
           db.AddParameter(dbCommand, "DATE", DbType.String,ParameterDirection.Input, "DATE", DataRowVersion.Default,varDate);
           db.AddParameter(dbCommand, "MAKERID", DbType.String, ParameterDirection.Input, "MAKERID", DataRowVersion.Default,EMID );
           db.AddParameter(dbCommand, "MOLDNO", DbType.String, ParameterDirection.Input, "MOLDNO", DataRowVersion.Default, moldNo );
           db.AddParameter(dbCommand, "wName", DbType.String, ParameterDirection.Input, "wName", DataRowVersion.Default, wName);
           db.AddParameter(dbCommand, "remark", DbType.String, ParameterDirection.Input, "remark", DataRowVersion.Default, remark );
           db.ExecuteNonQuery(dbCommand ,dbTransaction );
        }
    }
}

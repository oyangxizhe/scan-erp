﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Windows.Forms;
using XizheC_SERVER;
using System.Text;
using System.Net;
using System.IO;
using System.Configuration;

namespace WebServer
{
    public partial class S_EMPLOYEEINFO : System.Web.UI.Page
    {
        basec bc = new basec();
        DataTable dt = new DataTable();
        CEMPLOYEE_INFO cemployee_info = new CEMPLOYEE_INFO();
        StringBuilder sqb = new StringBuilder();
        private string _ErrowInfo;
        public string ErrowInfo
        {

            set { _ErrowInfo = value; }
            get { return _ErrowInfo; }

        }
        private bool _IFExecutionSUCCESS;
        public bool IFExecution_SUCCESS
        {
            set { _IFExecutionSUCCESS = value; }
            get { return _IFExecutionSUCCESS; }

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            StringBuilder sqb = new StringBuilder();
            string log_save_path = ConfigurationManager.AppSettings["log_save_path"].ToString();

            string varDate = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss").Replace("-", "/");
             try
             {
                 /* Response.Clear();
                  Response.ContentType = "application/json";

                  // 1. 确保 HttpContext 可用
                  if (HttpContext.Current == null)
                  {
                      Response.Write(JsonConvert.SerializeObject(new { error = "HttpContext not available" }));
                      return;
                  }*/
                 List<EMLOYEEINFO_O> list1 = new List<EMLOYEEINFO_O>();
                 if (Request.Form["GETID"] != "" && Request.Form["GETID"] != null)
                 {
                     EMLOYEEINFO_O employeeinfo1 = new EMLOYEEINFO_O();
                     employeeinfo1.IDO = cemployee_info.GETID();
                     list1.Add(employeeinfo1);
                     Response.Write(JsonConvert.SerializeObject(list1));

                 }
                 else if (Request.Form["EMPLOYEE_ID"] != "" && Request.Form["EMPLOYEE_ID"] != null || Request.Form["ENAME"] != "" &&
                     Request.Form["ENAME"] != null)
                 {

                     dt = bc.getdt(cemployee_info.sql + " WHERE EMPLOYEE_ID LIKE '%" + Request.Form["EMPLOYEE_ID"].ToString() + "%' AND  ENAME LIKE '%" + Request.Form["ENAME"].ToString() + "%'");
                     dt = cemployee_info.RETURN_HAVE_ID_DT(dt);
                     Response.Write(JsonConvert.SerializeObject(dt));

                 }
                 else if (Request.Form["POSITION"] != "" && Request.Form["POSITION"] != null)
                 {

                     dt = bc.getdt(cemployee_info.sql + " WHERE POSITION LIKE '%" + Request.Form["POSITION"].ToString() + "%'");
                     dt = cemployee_info.RETURN_HAVE_ID_DT(dt);
                     Response.Write(JsonConvert.SerializeObject(dt));

                 }
                 else if (Request.Form["DEPART"] != "" && Request.Form["DEPART"] != null)
                 {

                     dt = bc.getdt(cemployee_info.sql + " WHERE DEPART LIKE '%" + Request.Form["DEPART"].ToString() + "%'");
                     dt = cemployee_info.RETURN_HAVE_ID_DT(dt);
                     Response.Write(JsonConvert.SerializeObject(dt));

                 }
                 else if (Request.Form["ALL"] != null && Request.Form["ALL"] != "")
                 {
                     dt = bc.getdt(cemployee_info.sql);
                     dt = cemployee_info.RETURN_HAVE_ID_DT(dt);
                     Response.Write(JsonConvert.SerializeObject(dt));
                     //HttpContext.Current.ApplicationInstance.CompleteRequest(); // 正确调用
                 }
                 sqb.AppendFormat("Success"+dt.ToString()+"-"+varDate );
                 sqb.AppendFormat("\r\n");
                 // 4. 正常结束
                 HttpContext.Current.ApplicationInstance.CompleteRequest();
                
             }
             catch (Exception ex)
             {
                 Response.Clear();
                 Response.Write(JsonConvert.SerializeObject(new { error = ex.Message }));
                 HttpContext.Current.ApplicationInstance.CompleteRequest();
                 sqb.Append(ex.Message + "\r\n");

             }
             finally
             {
                 data_to_txt(log_save_path + "record-" + DateTime.Now.ToString("yyMMdd") + ".txt", sqb.ToString());//執行記錄

             }
      

        }
        #region data_to_txt
        public void data_to_txt(string path, string data)
        {
            if (!File.Exists(path)) //記錄上傳成功與否 若當日日志文件不存在那么新增一個日志文件，若存在就在日志文件末尾追加日志
            {
                System.IO.File.WriteAllText(path, data);
            }
            else
            {
                FileStream fs = null;
                string filePath = path;
                //将待写的入数据从字符串转换为字节数组  
                Encoding encoder = Encoding.UTF8;
                byte[] bytes = encoder.GetBytes(data);
                try
                {
                    fs = File.OpenWrite(filePath);
                    //设定书写的开始位置为文件的末尾  
                    fs.Position = fs.Length;
                    //将待写入内容追加到文件末尾  
                    fs.Write(bytes, 0, bytes.Length);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("文件打开失败{0}", ex.ToString());
                }
                finally
                {
                    fs.Close();
                }
                Console.ReadLine();
            }
        }
        #endregion
        #region save
        private void save(string LOGIN_EMID,string EMPLOYEE_ID,string IDO,string ENAME,string DEPART,string POSITION,string PHONE,string SAMPLE_CODE)
        {

            string year = DateTime.Now.ToString("yy");
            string month = DateTime.Now.ToString("MM");
            string day = DateTime.Now.ToString("dd");
            string varDate = DateTime.Now.ToString("yyy/MM/dd HH:mm:ss");
            string varMakerID = LOGIN_EMID;
            string GET_EMPLOYEE_ID = bc.getOnlyString("SELECT EMPLOYEE_ID FROM EMPLOYEEINFO WHERE EMID='" + IDO + "'");

            if (!bc.exists("SELECT EMID FROM EMPLOYEEINFO WHERE EMID='" + IDO + "'"))
            {
                if (bc.exists("SELECT * FROM EMPLOYEEINFO WHERE EMPLOYEE_ID='" + EMPLOYEE_ID + "'"))
                {
                    ErrowInfo = string.Format("员工工号 {0} 已经存在", EMPLOYEE_ID);
                    IFExecution_SUCCESS = false;

                }
                else
                {
                    basec.getcoms(@"INSERT INTO EMPLOYEEINFO(EMID,EMPLOYEE_ID,ENAME,DEPART,POSITION,MAKERID,DATE,YEAR,
                                   MONTH,PHONE,SAMPLE_CODE) VALUES ('" + IDO + "','" + EMPLOYEE_ID + "','" + ENAME  +
                     "','" + DEPART  + "','" + POSITION + "','" + varMakerID + "','" + varDate +
                     "','" + year + "','" + month + "','" + PHONE + "','" + SAMPLE_CODE + "')");
                    IFExecution_SUCCESS = true;
                    //Bind();
                }

            }
            else if (EMPLOYEE_ID != GET_EMPLOYEE_ID)
            {
                //MessageBox.Show(IDO + "," + textBox1.Text + "," + GET_EMPLOYEE_ID);
                if (bc.exists("SELECT * FROM EMPLOYEEINFO WHERE EMPLOYEE_ID='" + EMPLOYEE_ID + "'"))
                {
                    ErrowInfo = string.Format("员工工号 {0} 已经存在", EMPLOYEE_ID);
                    IFExecution_SUCCESS = false;


                }
                else
                {
                    basec.getcoms(@"UPDATE EMPLOYEEINFO SET EMPLOYEE_ID='" + EMPLOYEE_ID + "' ,ENAME='" +ENAME  + "',DEPART='" + DEPART  +
                         "',POSITION='" + POSITION + "',MAKERID='" + varMakerID +
                         "',DATE='" + varDate + "',PHONE='" + PHONE + "',SAMPLE_CODE='" + SAMPLE_CODE + "' WHERE EMID='" + IDO + "'");
                    IFExecution_SUCCESS = true;
                    //Bind();
                }
            }
            else
            {
                basec.getcoms(@"UPDATE EMPLOYEEINFO SET EMPLOYEE_ID='" + EMPLOYEE_ID + "' ,ENAME='" + ENAME + "',DEPART='" + DEPART +
                         "',POSITION='" + POSITION + "',MAKERID='" + varMakerID +
                         "',DATE='" + varDate + "',PHONE='" + PHONE + "',SAMPLE_CODE='" + SAMPLE_CODE + "' WHERE EMID='" + IDO + "'");
                IFExecution_SUCCESS = true;
                //Bind();
            }

        }
        #endregion
           class EMLOYEEINFO_O
           {
              public EMLOYEEINFO_O ()
              {


              }
              private string _IDO;
              public string IDO
              {
                  set { _IDO = value; }
                  get { return _IDO; }

              }
              private string _NO;
              public string NO
              {
                  set { _NO = value; }
                  get { return _NO; }

              }
        private string _EMID;
        public string EMID
        {
            set { _EMID = value; }
            get { return _EMID; }

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
        private string _POSITION;
        public string POSITION
        {
            set { _POSITION = value; }
            get { return _POSITION; }

        }
        private string _DEPART;
        public string DEPART
        {
            set { _DEPART = value; }
            get { return _DEPART; }

        }
        private string _PHONE;
        public string PHONE
        {
            set { _PHONE = value; }
            get { return _PHONE; }

        }
        private string _SAMPLE_CODE;
        public string SAMPLE_CODE
        {
            set { _SAMPLE_CODE = value; }
            get { return _SAMPLE_CODE; }

        }
        private string _MAKER;
        public string MAKER
        {
            set { _MAKER = value; }
            get { return _MAKER; }

        }
        private string _DATE;
        public string DATE
        {
            set { _DATE = value; }
            get { return _DATE; }

        }
        private string _ErrowInfo;
        public string ErrowInfo
        {

            set { _ErrowInfo = value; }
            get { return _ErrowInfo; }

        }
        private  bool _IFExecutionSUCCESS;
        public  bool IFExecution_SUCCESS
        {
            set { _IFExecutionSUCCESS = value; }
            get { return _IFExecutionSUCCESS; }

        }


           }  
           
        
    }
}
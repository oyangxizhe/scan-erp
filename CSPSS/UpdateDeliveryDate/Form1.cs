using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace UpdateDeliveryDate
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }



        basec bc = new basec();
        
        DataTable dt = new DataTable();
        DataTable dt1 = new DataTable();
        DataTable dt2 = new DataTable();
        DataTable dt3 = new DataTable();
        private void Form1_Load(object sender, EventArgs e)
        {
            notifyIcon1.Icon = new Icon(System.IO.Path.GetFullPath("update.ico"));
            notifyIcon1.Text = "文件删除程序";
            timer1.Enabled = true;
          

        }
        private void bind()
        {
            string v1=@"
SELECT 
A.FLKEY AS FLKEY,
A.NEW_FILE_NAME AS NEW_FILE_NAME
FROM SERVER_DELETE_FILE A
";
            dt = bc.getdt(v1);
       
            try
            {
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        string v2 = "d:/uploadfile/" + "INITIAL"+dr["NEW_FILE_NAME"].ToString();
                        if (File.Exists(v2))
                        {
                            File.Delete(v2);
                        }
                        string v3 = "d:/uploadfile/" + "80X80" + dr["NEW_FILE_NAME"].ToString();
                        if (File.Exists(v3))
                        {
                            File.Delete(v3);
                            bc.getcom("DELETE SERVER_DELETE_FILE WHERE NEW_FILE_NAME='" + dr["NEW_FILE_NAME"].ToString() + "'");
                        }

                    }

                }
            }
            catch (Exception)
            {

            }
            //dataGridView1.DataSource = bc.getdt(sqlo);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
           
            timer1.Interval = 1000;
            bind();
        }

        private void notifyIcon1_Click(object sender, EventArgs e)
        {
            click();//托盘单击事件
        }
        private void click()
        {

            //basec.getcoms("UPDATE REMIND SET RECEIVE_STATUS='Y' WHERE RIID='" + ID + "' AND NOTICE_MAKERID='" + LOGIN.EMID + "'");
      

         
            ContextMenu c = new ContextMenu();
            MenuItem s = new MenuItem("退出");
            c.MenuItems.Add(s);
            notifyIcon1.ContextMenu = c;
            notifyIcon1.Icon = new Icon(System.IO.Path.GetFullPath("Update.ico"));
            s.Click += new EventHandler(notify_Click);
            this.Show();

        }
        private void notify_Click(object sender, EventArgs e)
        {
            EXIT();
        }
        private void EXIT()
        {
            this.Dispose();
            notifyIcon1.Dispose();
            Application.Exit();

        }

        private void notifyIcon1_BalloonTipClicked(object sender, EventArgs e)
        {
            click();//气泡单击事件
          
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }
   
    }
}

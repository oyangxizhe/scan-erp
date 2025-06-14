using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using XizheC_SERVER;
using System.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
namespace Webserver
{
    public partial class S_CONNECTIONSTRING : System.Web.UI.Page
    {
        basec bc = new basec();
        private string _CONNECTIONSTRING;
        public string CONNECTIONSTRING
        {

            set { _CONNECTIONSTRING = value; }
            get { return _CONNECTIONSTRING; }

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Form["S_CONNECTIONSTRING"] != "" && Request.Form["S_CONNECTIONSTRING"] != null)
            {
                string M_str_sqlcon = ConfigurationManager.AppSettings["ConnectionDB"].ToString();
                List<string> list1 = new List<string>();
                CONNECTIONSTRING = M_str_sqlcon;
                list1.Add(CONNECTIONSTRING);
                Response.Write(JsonConvert.SerializeObject(list1));

            }
        }
    }
}
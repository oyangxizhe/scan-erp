using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace webuploadfile
{
    public partial class _default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (Request.Files.Count > 0)
            {
                try
                {
                    HttpPostedFile file = Request.Files[0];
                    string filePath = @"c:\uploadfile\" + file.FileName;
                    file.SaveAs(filePath);
                    Response.Write("Success\r\n");
                }
                catch
                {
                    Response.Write("Error\r\n");
                }
            }
        }
        
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CSPSS
{
    static class Program
    {/// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new  BASE_INFO.MOLD_BASE());
            Application.Run(new LOGIN());
           // Application.Run(new BASE_INFO.EMPLOYEE_INFO());
            //Application.Run(new SELL_MANAGE.SELLTABLET());
            //Application.Run(new SELL_MANAGE.ORDER());
        }
    }
}

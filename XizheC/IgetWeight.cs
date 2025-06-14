using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace XizheC
{
    interface IgetWeight
    {

        //返回含重量与模具编号的数据
        DataTable getWeight(string orid, string wareid, string material, string wname);
    }
}

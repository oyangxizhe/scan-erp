using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace XizheC
{
  public class getWeight : IgetWeight
    {
        private DataTable dt;
        
        public getWeight(string orid, string wareid, string material, string wname)
        {
            StringBuilder sqb = new StringBuilder();
            sqb.AppendFormat("SELECT *,WEIGHT AS 重量 FROM MOLD_BASE WHERE CUID = (SELECT CUID FROM Order_MST WHERE ORID = '{0}') AND WAREID = '{1}'", orid, wareid);
            sqb.AppendFormat("AND MAID = (SELECT MAID FROM MATERIAL WHERE MATERIAL = '{0}') AND WNAME = '{1}'", material, wname);
            dt = new basec().getdt(sqb.ToString());
       
        }

        DataTable IgetWeight.getWeight(string orid, string wareid, string material, string wname)
        {
            DataTable dt = new DataTable();

            return dt;
        }
        public DataTable ReturnWeight()
        {
            return dt;
        }
        public static DataTable ReturnWeightNew(DataTable dtx,string cuid, string wareid, string maid, string wname)
        {
           DataTable  dt = new basec().GET_DT_TO_DV_TO_DT(dtx, "", "cuid='"+cuid+"' and wareid='"+wareid+"' and maid='"+maid+"' and wname='"+wname+"'");
           return dt;
        }


    }
}

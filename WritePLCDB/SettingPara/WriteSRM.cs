using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SettingPara
{
    public class WriteSRM
    {
        public static void WriteSrmData(string inOut,int stand,int list,int blank,int floor)
        {
            foreach (var s in PLCSystem_SRM.Dic_SRM_Request)
            {
                SRM_Respone cr = null;

                PLCSystem_SRM.Dic_SRM_Res.TryGetValue(s.Key, out cr);

                //if (inOut == "入库")
                //{
                //    cr.s_FrPattern = 2;
                //    cr.s_ToStand = 0;
                //    cr.s_FrLine = 0;
                //    cr.s_FrGrid = 0;
                //    cr.s_FrTier = 0;

                //    cr.s_FrStand = (ushort)stand;
                //    cr.s_ToLine = (ushort)list;
                //    cr.s_ToGrid = (ushort)blank;
                //    cr.s_ToTier = (ushort)floor;

                //    cr.s_Execute = 1;
                //}
                //if (inOut == "出库")
                //{
                //    cr.s_FrPattern = 1;
                //    cr.s_FrStand = 0;
                //    cr.s_FrLine = (ushort)list;
                //    cr.s_FrGrid = (ushort)blank;
                //    cr.s_FrTier = (ushort)floor;

                //    cr.s_ToStand = (ushort)stand;
                //    cr.s_ToLine = 0;
                //    cr.s_ToGrid = 0;
                //    cr.s_ToTier = 0;

                //    cr.s_Execute = 1;
                //}
                //cr.s_Fault = 1;
                break;
            }
        }
    }
}

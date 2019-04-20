using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using LIB_Communation;
using System.Configuration;
namespace SettingPara
{
    public class SettingPLC
    {
        private static SettingPLC _instance=null;

        public static SettingPLC GetInstance()
        {
            if(_instance==null)
               _instance=new SettingPLC();
            return _instance;
        }
        protected SettingPLC()
        {

        }
        public DataTable GetParaTableFromCfg()
        {
            string SechemeCFG_PLC = "";
            string DefaultData_PLC = "";
            if (ConfigurationManager.AppSettings.AllKeys.Contains("Path_SechemeCFG_PLC"))
            {
                SechemeCFG_PLC = ConfigurationManager.AppSettings["Path_SechemeCFG_PLC"].ToString().Trim();
            }
            if (ConfigurationManager.AppSettings.AllKeys.Contains("Patch_DefaultData_PLC"))
            {
                DefaultData_PLC = ConfigurationManager.AppSettings["Patch_DefaultData_PLC"].ToString().Trim();
            }
            DataTable dt = new DataTable();
            dt.ReadXmlSchema(@SechemeCFG_PLC);
            dt.ReadXml(@DefaultData_PLC);
            return dt;
        }
        public bool WritePara(CallDave.daveConnection dvc, string key, string value, string ParaType, string ParaScope, string plcAddress, out string outMsg)
        {
            int res = 0;
            outMsg = "";
            bool result = false;

            if (value == null || value == string.Empty || value == "")
                value = "0";
            byte[] b_data = { 0, 0 };
            try
            {
                result = CheckValueScope(ParaScope, value);
                if (!result)
                {
                    outMsg = string.Format("[{0}]验证范围失败", key);
                    throw new ApplicationException(outMsg);
                }
                int db = 0;
                int start = 0;
                result = SplitplcAddress(plcAddress, out db, out start, out outMsg);
                if (!result)
                {
                    outMsg = string.Format("[{0}]PLC地址不合法", key);
                    throw new ApplicationException(outMsg);
                }

                b_data = BitConverter.GetBytes(ushort.Parse(value));
                b_data = b_data.Reverse().ToArray();
                res = dvc.writeBytes(CallDave.daveDB, db, start, b_data.Length, b_data);

                if (res != 0)
                {
                    outMsg = string.Format("[{0}]写入PLC失败-{1}", key, res);
                    throw new ApplicationException(outMsg);
                }
                result = ServiceREF.SetParameterByCode(key, value, out outMsg);
                if (!result)
                {
                    outMsg = string.Format("[{0}]保存数据至服务器失败-{1}", key, outMsg);
                    throw new ApplicationException(outMsg);
                }

                return result;
            }
            catch (Exception ex)
            {
                outMsg = ex.Message;
                return false;
            }
        }
        private bool SplitplcAddress(string plcAddress, out int db, out int start, out string OutMsg)
        {
            db = 0;
            start = 0;
            OutMsg = "";
            try
            {
                string[] temp = plcAddress.Split('.');

                if (temp.Length == 2)
                {
                    if (temp[0].Substring(0, 2).ToUpper() == "DB".ToUpper())
                    {
                        db = int.Parse(temp[0].Substring(2, temp[0].Length - 2));
                    }
                    else
                    {
                        return false;
                    }
                    if (temp[1].Substring(0, 3).ToUpper() == "DBW".ToUpper())
                    {
                        start = int.Parse(temp[1].Substring(3, temp[1].Length - 3));
                    }
                    else
                    {
                        return false;
                    }
                    return true;
                }
                else
                {
                    return false; ;
                }
            }
            catch (Exception ex)
            {
                OutMsg = ex.Message;
                return false;
            }

        }
        private bool CheckValueScope(string paraScope, string value)
        {
            string[] tmp1 = paraScope.Split(',');

            string[] tmp2 = tmp1[0].Split('~');
            int min = 0;
            int max = 0;
            int iValue = int.Parse(value);


            if (tmp2.Length == 2)
            {
                int.TryParse(tmp2[0], out min);
                int.TryParse(tmp2[1], out max);
                if (iValue >= min && iValue <= max)
                {
                    return true;
                }
                else
                {

                    if (tmp1.Length > 1)
                    {
                        return value == tmp1[1];
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            return true;

        }
        
    }
}

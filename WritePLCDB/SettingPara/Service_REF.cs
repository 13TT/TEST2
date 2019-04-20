using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.ServiceModel;
using LIB_Log;
using LIB_WCF;
namespace SettingPara
{
    public class ServiceREF
    {
        public const string MSGSTS_CREATE = "10";
        public const string MSGSTS_REACH = "30";
        public const string MSGSTS_Complete = "50";

        public const int MaxTryConnectTimes = 1;
        public static int CurrentTryTime = 0;
        private static IService_SCS_OP _service = null;

        public static IService_SCS_OP GetService()
        {
            try
            {
                string outMsg;
                //////return _RFservice;
                //if (_service == null || _service.GetSSSStatus(out outMsg) == -1)
                if (_service == null)
                {
                    _service = CretaeService();
                }
                _service.GetSSSStatus(out outMsg);
                CurrentTryTime = 0;
                return _service;
            }
            catch (Exception ex)
            {
                _service = null;
                CurrentTryTime++;
                if (CurrentTryTime < MaxTryConnectTimes)
                {
                    return GetService();
                }
                throw new ApplicationException(string.Format("The WCF Connect Closed.{0}", ex.Message));
            }
        }
        private static IService_SCS_OP CretaeService()
        {
            //NetTcpBinding WShb = new NetTcpBinding();            
            //WShb.Security.Mode = SecurityMode.None;
            //EndpointAddress epo = new EndpointAddress("net.tcp://192.168.1.154:20029/WAPService/RFService");
            //ChannelFactory<IServiceRF> cf = new ChannelFactory<IServiceRF>(WShb, epo);//创建客户端频道

            ChannelFactory<IService_SCS_OP> cf = new ChannelFactory<IService_SCS_OP>("NetTcpBinding_IService_SCS_OP");
            IService_SCS_OP sv1 = cf.CreateChannel();               //创建服务接口实例并连接到服务端
            return sv1;
        }

        public static DataTable GetChuteList(out string outMsg)
        {
            outMsg = "";
            try
            {
                IService_SCS_OP Service = GetService();
                DataSet ds = Service.GetChuteList(out outMsg);
                if (ds != null && ds.Tables.Count > 0)
                    return ds.Tables[0];
                else
                    return new DataTable();
            }
            catch (Exception ex)
            {
                outMsg = ex.StackTrace;
                return null;
            }
        }
        public static DataTable GetPrintList(out string outMsg)
        {
            outMsg = "";
            try
            {
                IService_SCS_OP Service = GetService();
                DataSet ds = Service.GetPrintList(out outMsg);
                if (ds != null && ds.Tables.Count > 0)
                    return ds.Tables[0];
                else
                    return new DataTable();
            }
            catch (Exception ex)
            {
                outMsg = ex.StackTrace;
                return null;
            }
        }

        public static DataTable GetHistoryLableByLaneCode(string chute_code, out string outMsg)
        {
            outMsg = "";
            try
            {
                IService_SCS_OP Service = GetService();
                DataSet ds = Service.GetHistoryLabelByLanecode(chute_code, out outMsg);
                if (ds != null && ds.Tables.Count > 0)
                    return ds.Tables[0];
                else
                    return new DataTable();
            }
            catch (Exception ex)
            {
                outMsg = ex.StackTrace;
                return null;
            }
        }

        public static int GetHostStatus()
        {
            try
            {
                string outMsg;
                IService_SCS_OP Service = GetService();
                int sts = Service.GetSSSStatus(out outMsg);
                return sts;
            }
            catch
            {
                return -1;
            }
        }
        public static PrintLibData GetPrintLibByLaneCode(string chute_code, out string outMsg)
        {
            PrintLibData pld = new PrintLibData();
            outMsg = "";
            try
            {
                IService_SCS_OP Service = GetService();
                pld = Service.GetPrintLibByLaneCode(chute_code, out outMsg);
                return pld;
            }
            catch (Exception ex)
            {
                outMsg = ex.StackTrace;
                return pld;
            }
        }
        public static PrintLibData GetPrintLibByID(int ID, out string outMsg)
        {
            PrintLibData pld = new PrintLibData();
            outMsg = "";
            try
            {
                IService_SCS_OP Service = GetService();
                pld = Service.GetPrintLibByID(ID, out outMsg);
                return pld;
            }
            catch (Exception ex)
            {
                outMsg = ex.StackTrace;
                return pld;
            }
        }
        public static bool ChangePacket(string chute_code, out string outMsg)
        {
            outMsg = "";
            try
            {
                IService_SCS_OP Service = GetService();
                int i = Service.ChangePacket(chute_code, out outMsg);
                return true;
            }
            catch (Exception ex)
            {
                outMsg = ex.StackTrace;
                return false;
            }
        }

        public static bool SetParameterByCode(string key, string value, out string outMsg)
        {
            bool result = false;
            outMsg = "";
            try
            {
                IService_SCS_OP Service = GetService();
                result = Service.SetParameterByCode(key, value, out outMsg) != -1;
                return result;
            }
            catch (Exception ex)
            {
                outMsg = ex.StackTrace;
                return result;
            }
        }

        public static bool SetParameterByTable(DataTable dt, string value, out string outMsg)
        {
            DataSet ds = new DataSet();
            ds.Tables.Add(dt);
            bool result = false;
            outMsg = "";
            try
            {
                IService_SCS_OP Service = GetService();
                result = Service.SetParameterByTable(ds, out outMsg) != -1;
                return result;
            }
            catch (Exception ex)
            {
                outMsg = ex.StackTrace;
                return result;
            }
        }

        public static DataTable GetParameterList(out string  outMsg)
        {
            DataTable dt = new DataTable();
            outMsg = "";
            try
            {
                IService_SCS_OP Service = GetService();
               DataSet ds = Service.GetParameterList(out outMsg);
               if (ds != null && ds.Tables.Count >= 1)
               {
                   return ds.Tables[0];
               }
               else
               {
                   return null;
               }
            }
            catch (Exception ex)
            {
                outMsg = ex.StackTrace;
                return null;
            }
        }

    }
}

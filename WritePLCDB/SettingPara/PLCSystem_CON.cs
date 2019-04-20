using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using LIB_Communation;
using LIB_Log;
using System.Threading;
using System.Data;

namespace SettingPara
{
    /// <summary>
    /// PLC系统类
    /// 管理所有PLC连接
    /// </summary>
    public class PLCSystem_CON
    {
        public static bool IsScanCONSTS = true;
        public static int ScanSleepTime = 500;
        /// <summary>
        /// 当前条码信息（紧用于界面显示，无实际意义）
        /// </summary>
        public static string CurentBarcode = "";
        /// <summary>
        /// 预设路径（紧用于测试）
        /// </summary>
        public static Dictionary<string, string> Dic_setPath = new Dictionary<string, string>();
        /// <summary>
        /// 输送机状态字典，储存输送机状态DB块对应类
        /// </summary>
        public static Dictionary<string, CON_Status> Dic_CON_STS = new Dictionary<string, CON_Status>();
        /// <summary>
        /// 输送机请求字典，储存输送机请求DB块对应类
        /// </summary>
        public static Dictionary<string, CON_Request> Dic_CON_Req = new Dictionary<string, CON_Request>();
        /// <summary>
        /// 输送机应答字典，储存输送机应答DB块对应类
        /// </summary>
        public static Dictionary<string, CON_Response> Dic_CON_Res = new Dictionary<string, CON_Response>();
        /// <summary>
        /// 输送机连接字典，储存输送机PLC连接数据处理类
        /// </summary>
        public static Dictionary<string, PLCProcesser> Dic_PlcProc = new Dictionary<string, PLCProcesser>();

        public static Dictionary<string, SRM_PlatformStatus> Dic_SRM_PS = new Dictionary<string, SRM_PlatformStatus>();

        public static Dictionary<string, SRM_PlatStatusWrite> Dic_SRM_SPW = new Dictionary<string, SRM_PlatStatusWrite>();

        public static Dictionary<string, string> SIMPLCRequest = new Dictionary<string, string>();

        /// <summary>
        /// 初始化PLC连接
        /// </summary>
        public static void InitConn()
        {
            string CC01_IPStr = ConfigurationManager.AppSettings["CC01"].ToString();
            string CC02_IPStr = ConfigurationManager.AppSettings["CC02"].ToString();
            string ip1, ip2;
            int port1, port2;
            int slot1, slot2;
            ip1 = CC01_IPStr.Split(':')[0]; port1 = int.Parse(CC01_IPStr.Split(':')[1]); slot1 = int.Parse(CC01_IPStr.Split(':')[2]);
            ip2 = CC02_IPStr.Split(':')[0]; port2 = int.Parse(CC02_IPStr.Split(':')[1]); slot2 = int.Parse(CC02_IPStr.Split(':')[2]);
            Dic_PlcProc.Add("CC01", new PLCProcesser() { Name = "CC01", IP = ip1, Port = port1, slot = slot1, WLog = Program.Log_Agent_CON });
            Dic_PlcProc.Add("CC02", new PLCProcesser() { Name = "CC02", IP = ip2, Port = port2, slot = slot2, WLog = Program.Log_Agent_CON });
        }
        /// <summary>
        /// 初始化PLC数据项
        /// </summary>
        public static void InitItem()
        {
            try
            {
                //Dic_CON_STS.Add("101", new CON_Status() { address = new PLCAddress("CC01.DB64.DBW0") });
                //Dic_CON_STS.Add("102", new CON_Status() { address = new PLCAddress("CC01.DB60.DBW0") });
                //Dic_CON_STS.Add("103", new CON_Status() { address = new PLCAddress("CC01.DB61.DBW0") });
                //Dic_CON_STS.Add("104", new CON_Status() { address = new PLCAddress("CC01.DB62.DBW0") });
                //Dic_CON_STS.Add("105", new CON_Status() { address = new PLCAddress("CC01.DB63.DBW0") });
                //Dic_CON_STS.Add("106", new CON_Status() { address = new PLCAddress("CC01.DB65.DBW0") });
                //Dic_CON_STS.Add("107", new CON_Status() { address = new PLCAddress("CC01.DB66.DBW0") });
                //Dic_CON_STS.Add("112", new CON_Status() { address = new PLCAddress("CC01.DB67.DBW0") });
                //Dic_CON_STS.Add("113", new CON_Status() { address = new PLCAddress("CC01.DB68.DBW0") });
                //Dic_CON_STS.Add("115", new CON_Status() { address = new PLCAddress("CC01.DB69.DBW0") });
                //Dic_CON_STS.Add("116", new CON_Status() { address = new PLCAddress("CC01.DB40.DBW0") });
                //Dic_CON_STS.Add("117", new CON_Status() { address = new PLCAddress("CC01.DB41.DBW0") });
                //Dic_CON_STS.Add("118", new CON_Status() { address = new PLCAddress("CC01.DB42.DBW0") });
                //Dic_CON_STS.Add("111", new CON_Status() { address = new PLCAddress("CC01.DB43.DBW0") });
                //Dic_CON_STS.Add("114", new CON_Status() { address = new PLCAddress("CC01.DB44.DBW0") });
                //Dic_CON_STS.Add("152", new CON_Status() { address = new PLCAddress("CC01.DB45.DBW0") });
                //Dic_CON_STS.Add("153", new CON_Status() { address = new PLCAddress("CC01.DB46.DBW0") });
                //Dic_CON_STS.Add("154", new CON_Status() { address = new PLCAddress("CC01.DB47.DBW0") });
                //Dic_CON_STS.Add("155", new CON_Status() { address = new PLCAddress("CC01.DB48.DBW0") });

                //Dic_CON_STS.Add("201", new CON_Status() { address = new PLCAddress("CC02.DB62.DBW0") });
                //Dic_CON_STS.Add("202", new CON_Status() { address = new PLCAddress("CC02.DB63.DBW0") });
                //Dic_CON_STS.Add("203", new CON_Status() { address = new PLCAddress("CC02.DB64.DBW0") });
                //Dic_CON_STS.Add("204", new CON_Status() { address = new PLCAddress("CC02.DB65.DBW0") });
                //Dic_CON_STS.Add("205", new CON_Status() { address = new PLCAddress("CC02.DB60.DBW0") });
                //Dic_CON_STS.Add("206", new CON_Status() { address = new PLCAddress("CC02.DB61.DBW0") });

                //Dic_CON_STS.Add("207", new CON_Status() { address = new PLCAddress("CC02.DB66.DBW0") });
                //Dic_CON_STS.Add("208", new CON_Status() { address = new PLCAddress("CC02.DB67.DBW0") });
                //Dic_CON_STS.Add("212", new CON_Status() { address = new PLCAddress("CC02.DB68.DBW0") });
                //Dic_CON_STS.Add("213", new CON_Status() { address = new PLCAddress("CC02.DB69.DBW0") });
                //Dic_CON_STS.Add("215", new CON_Status() { address = new PLCAddress("CC02.DB40.DBW0") });
                //Dic_CON_STS.Add("216", new CON_Status() { address = new PLCAddress("CC02.DB41.DBW0") });
                //Dic_CON_STS.Add("217", new CON_Status() { address = new PLCAddress("CC02.DB42.DBW0") });
                //Dic_CON_STS.Add("218", new CON_Status() { address = new PLCAddress("CC02.DB43.DBW0") });
                //Dic_CON_STS.Add("211", new CON_Status() { address = new PLCAddress("CC02.DB44.DBW0") });
                //Dic_CON_STS.Add("214", new CON_Status() { address = new PLCAddress("CC02.DB45.DBW0") });

                //Dic_CON_Req.Add("101", new CON_Request() { address = new PLCAddress("CC01.DB50.DBW0"), StnType = eStationType.SP });
                //Dic_CON_Res.Add("101", new CON_Response() { address = new PLCAddress("CC01.DB51.DBW0") });

                //Dic_CON_Req.Add("201", new CON_Request() { address = new PLCAddress("CC02.DB50.DBW0"), StnType = eStationType.AP });
                //Dic_CON_Res.Add("201", new CON_Response() { address = new PLCAddress("CC02.DB51.DBW0") });

                //Dic_CON_Req.Add("202", new CON_Request() { address = new PLCAddress("CC02.DB52.DBW0"), StnType = eStationType.AP });
                //Dic_CON_Res.Add("202", new CON_Response() { address = new PLCAddress("CC02.DB53.DBW0") });

                //Dic_CON_Req.Add("203", new CON_Request() { address = new PLCAddress("CC02.DB54.DBW0"), StnType = eStationType.AP });
                //Dic_CON_Res.Add("203", new CON_Response() { address = new PLCAddress("CC02.DB55.DBW0") });

                //Dic_CON_Req.Add("204", new CON_Request() { address = new PLCAddress("CC02.DB56.DBW0"), StnType = eStationType.AP });
                //Dic_CON_Res.Add("204", new CON_Response() { address = new PLCAddress("CC02.DB57.DBW0") });

                ////Dic_setPath.Add("101", "102");
                ////Dic_setPath.Add("106", "103");

                //Dic_setPath.Add("101", "103");
                //Dic_setPath.Add("101", "104");
                //Dic_setPath.Add("101", "105");
                //Dic_setPath.Add("101", "205");

                Dic_SRM_PS.Add("10", new SRM_PlatformStatus() { address = new PLCAddress("SRM02.DB10.DBW0"), StnType = eStationType.EP });
                Dic_SRM_SPW.Add("11", new SRM_PlatStatusWrite() { address = new PLCAddress("SRM02.DB11.DBW0") });
            }
            catch (Exception ex)
            {
                Program.Log_Agent_CON.WriteLog(Enum_LogType.LogType_StartStop,
                       Enum_LogGrade.LogGrade_Nin,
                       Enum_LogMessageType.LogMsgType_Exception,
                       "InitItem",
                       ex.Message,
                       ex.StackTrace);
            }
        }
        /// <summary>
        /// 开始启动
        /// </summary>
        public static void Start()
        {
            foreach (var v in Dic_PlcProc.Values)
            {
                v.IsBeginProcess = true;
                v.LoopWrite += V_LoopWrite;
                v.Start();
            }
            //开始检测状态的线程
            PLCSystem_CON.IsScanCONSTS = true;
            Thread th = new Thread(ScanConSts);
            th.IsBackground = true;
            th.Start();
        }
        public static void ScanConSts()
        {
            while (PLCSystem_CON.IsScanCONSTS)
            {
                try
                {
                    string outString = "";
                    var vs = PLCSystem_SRM.SysSRMMaping.Where((e) => e.StnType == eStationType.EP);
                    foreach (var item in vs)
                    {
                        CON_Status conSts;
                        var exists = Dic_CON_STS.TryGetValue(item.ConNodeID, out conSts);
                        if (exists && conSts.s_OccupyStats == 1)
                        {
                            WMBusiness wmsbus = new WMBusiness();
                            var b = wmsbus.ActivateOrderInfo(item.LinkLine, item.StnNo, conSts.s_TUID, out outString);
                            if (b)
                            {
                                //写日志
                                Program.Log_Agent_CON.WriteLog(Enum_LogType.LogType_Evnet,
                                                    Enum_LogGrade.LogGrade_Nin,
                                                    Enum_LogMessageType.LogMsgType_Exception,
                                                    "ActivateOrderInfo",
                                                    vs.ToString(),
                                                    conSts.ToString());
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Program.Log_Agent_CON.WriteLog(Enum_LogType.LogType_Error,
                                Enum_LogGrade.LogGrade_Nin,
                                Enum_LogMessageType.LogMsgType_Exception,
                                "InitItem",
                                ex.Message,
                                ex.StackTrace);
                }
                finally
                {
                    Thread.Sleep(ScanSleepTime);
                }
            }
        }
        /// <summary>
        /// 输送机PLC连接的处理逻辑
        /// </summary>
        /// <param name="Sender"></param>
        /// <param name="e"></param>
        private static void V_LoopWrite(object Sender, EArgOnHandle_Process e)
        {
            string plcID = ((PLCProcesser)Sender).Name;
            string currentConNo = "";
            try
            {
                var ps = Dic_SRM_PS.Where(item => item.Value.address.PLC == plcID).ToList();
                var spw = Dic_SRM_SPW.Where(item => item.Value.address.PLC == plcID).ToList();

                foreach(var p in ps)
                {
                    //p.Value.LoadFromPLC(e.PLCConn);
                }
                foreach(var s in spw)
                {
                    s.Value.LoadFromPLC(e.PLCConn);
                }

                var arry_sts = Dic_CON_STS.Where(item => item.Value.address.PLC == plcID).ToList();

                foreach (var kv in arry_sts)
                {
                    currentConNo = kv.Key;
                    kv.Value.LoadFromPLC(e.PLCConn);

                    #region 写日志
                    Program.Log_Agent_CON.WriteLog(Enum_LogType.LogType_Debug,
                                               Enum_LogGrade.LogGrade_Nin,
                                               Enum_LogMessageType.LogMsgType_Event,
                                               "V_LoopWrite-ReadConnStatus",
                                               string.Format("[{0}]-[{1}]", plcID, currentConNo),
                                               ByteHelper.ToMessage(kv.Value.Tobytes(), CON_Status.cGroupLen));
                    #endregion
                }
                var arry_req = Dic_CON_Req.Where(item => item.Value.address.PLC == plcID).ToList();
                foreach (var kv in arry_req)
                {
                    currentConNo = kv.Key;
                    kv.Value.LoadFromPLC(e.PLCConn);
                    #region 写日志
                    Program.Log_Agent_CON.WriteLog(Enum_LogType.LogType_Debug,
                                               Enum_LogGrade.LogGrade_Nin,
                                               Enum_LogMessageType.LogMsgType_Event,
                                               "V_LoopWrite-ReadRequest",
                                                   string.Format("[{0}]-[{1}]-{2}", plcID, currentConNo, kv.Value.s_ScanBarCode),
                                               ByteHelper.ToMessage(kv.Value.Tobytes(), CON_Request.cGroupLen));
                    #endregion
                    #region
                    //if (kv.Value.s_OverRead == 1)
                    //{
                    //    string sql = string.Format(ConfigurationManager.AppSettings["Sql_CodeSel"].Replace('\n', ' '));
                    //    string outString = "";
                    //    var db = new DBAccess_MySql("MySql");
                    //    DBAccess_MySql dby = new DBAccess_MySql();
                    //    dby = db.ReturnSQL_String(sql, out outString);
                    //    string a = null;
                    //    while (dby.rec.Read())
                    //    {
                    //        a = dby.rec.GetString(0);
                    //    }
                    //    if (a != "")
                    //    {
                    //        string outBC = "";
                    //        if (!PLCSystem_CON.SIMPLCRequest.TryGetValue(currentConNo, out outBC))
                    //        {
                    //            kv.Value.s_ScanBarCode = a;// outBC;
                    //            kv.Value.s_OverRead = 0;
                    //            PLCSystem_CON.SIMPLCRequest.Remove(currentConNo);

                    //        }
                    //    }
                    //string sql3 = string.Format(ConfigurationManager.AppSettings["Sql_CodeDel"].Replace('\n', ' '));
                    //var rlt = db.ExecSql(sql, out outString);
                    //kv.Value.s_ScanBarCode = ff.txt_BarCode.Text; //"1121712-BN76:S1Q0893:24:011:S22333:";// outBC;
                    //kv.Value.s_OverRead = 0;
                    //PLCSystem_CON.SIMPLCRequest.Remove(currentConNo);
                    ////ff.txt_BarCode.Text = "";
                    //}
                    #endregion
                    if (kv.Value.s_OverRead == 1)
                    {
                        string outBC = "";
                        if (PLCSystem_CON.SIMPLCRequest.TryGetValue("102", out outBC))//(currentConNo, out outBC))
                        {
                            kv.Value.s_ScanBarCode = outBC;
                            kv.Value.s_OverRead = 0;
                            PLCSystem_CON.SIMPLCRequest.Remove("102");
                        }
                        if (PLCSystem_CON.SIMPLCRequest.TryGetValue(currentConNo, out outBC))
                        {
                            kv.Value.s_ScanBarCode = outBC;
                            kv.Value.s_OverRead = 1;
                            PLCSystem_CON.SIMPLCRequest.Remove(currentConNo);
                        }
                    }
                    if (kv.Value.s_OverRead == 0)
                    {
                        string outString = "";
                        //插入输入库，分配货位，分配目的地
                        //应答消息
                        CON_Response cr = null;
                        Dic_CON_Res.TryGetValue(kv.Key, out cr);
                        if (cr == null) continue;
                        cr.s_TUID = kv.Value.s_ScanBarCode;
                        cr.s_TaskID = kv.Value.s_TaskID;
                        cr.s_FromLoc = kv.Value.s_RequestLoc;

                        BarcodeInfo bi = new BarcodeInfo(cr.s_TUID);
                        
                        #region
                        //Program.Log_Agent_CON.WriteLog(Enum_LogType.LogType_Debug,
                        //    Enum_LogGrade.LogGrade_Nin,
                        //    Enum_LogMessageType.LogMsgType_Event,
                        //    "V_LoopWrite-WriteResponse",
                        //    "条码分解",
                        //    bi.ToString());
                        #endregion

                        CurentBarcode = bi.ToString();
                        if (kv.Value.StnType == eStationType.SP)
                        {
                            //匹配路线更新目的地，(紧测试用，将来会用WMBusiness里业务类代替）
                            string toLoc = "0";
                            //Dic_setPath.TryGetValue(cr.s_FromLoc.ToString(), out toLoc);

                            WMBusiness wmsbus = new WMBusiness();
                            var b = wmsbus.RequestInStock(cr.s_TUID, cr.s_FromLoc.ToString(), out toLoc, out outString);
                            if (b)
                            {
                                byte tb_toLoc = 0;
                                if (!byte.TryParse(toLoc, out tb_toLoc)) continue;
                                cr.s_ToLoc = tb_toLoc;
                                //将应答任务目的地写入PLC
                                cr.WirteToPLC(e.PLCConn);
                                #region
                                Program.Log_Agent_CON.WriteLog(Enum_LogType.LogType_Debug,
                                                           Enum_LogGrade.LogGrade_Nin,
                                                           Enum_LogMessageType.LogMsgType_Event,
                                                           "V_LoopWrite-WriteResponse",
                                                           string.Format("PLC:[{0}]-CON:[{1}]-TUID:[{2}]-FROMLOC:[{3}]-TOLOC:{4}-Resoponse:", plcID, currentConNo, cr.s_TUID, cr.s_FromLoc.ToString(), cr.s_ToLoc),
                                                           ByteHelper.ToMessage(cr.Tobytes(), CON_Response.cGroupLen));
                                #endregion
                            }
                            else
                            {
                                Program.Log_Agent_CON.WriteLog(Enum_LogType.LogType_Error,
                                 Enum_LogGrade.LogGrade_Nin,
                                 Enum_LogMessageType.LogMsgType_Event,
                                 "V_LoopWrite-WriteResponse", "入库请求失败.", outString);
                            }
                        }
                        else if (kv.Value.StnType == eStationType.AP)
                        {
                            string toLoc = "0";
                            Dic_setPath.TryGetValue(kv.Value.s_RequestLoc.ToString(), out toLoc);
                            cr.s_ToLoc = byte.Parse(toLoc);
                            //将应答任务目的地写入PLC
                            cr.WirteToPLC(e.PLCConn);
                        }
                        else
                        {
                            //异常处理20181206
                        }
                        //更新请求任务WCS已写入状态
                        var r = kv.Value.SetOverRead(e.PLCConn);
                        Program.Log_Agent_CON.WriteLog(Enum_LogType.LogType_Debug,
                           Enum_LogGrade.LogGrade_Nin,
                           Enum_LogMessageType.LogMsgType_Event,
                           "V_LoopWrite-UpdateRequestStats",
                            string.Format("PLC:[{0}]-CON:[{1}]-TUID:[{2}]-FROMLOC:[{3}]-WriteStatus:[{4}]", plcID, currentConNo, cr.s_FromLoc.ToString(), cr.s_TUID, r.ToString()), "");
                    }
                }
                e.result = true;
            }
            catch (Exception ex)
            {
                Program.Log_Agent_CON.WriteLog(Enum_LogType.LogType_Error,
                                       Enum_LogGrade.LogGrade_Nin,
                                       Enum_LogMessageType.LogMsgType_Exception,
                                       string.Format("V_LoopWrite-[{0}]-[{1}]", plcID, currentConNo),
                                       ex.Message,
                                       ex.StackTrace);
                e.result = false;
            }
        }
        /// <summary>
        /// 关闭
        /// </summary>
        public static void stop()
        {
            foreach (var v in Dic_PlcProc.Values)
            {
                v.IsBeginProcess = false;
                v.LoopWrite -= V_LoopWrite;
                v.Close();
            }
        }
        /// <summary>
        /// 获取PLC状态为特殊字符串格式
        /// </summary>
        /// <returns></returns>
        public static string GetPLCSts()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var v in Dic_PlcProc.Values)
            {
                sb.AppendFormat(" {0}[{1}]:{2} ", v.Name, v.IP, v.Status);
            }
            return sb.ToString();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using LIB_Communation;
using LIB_Log;
using System.Data;
using SettingPara.Wdata;

namespace SettingPara
{
    public class PLCSystem_SRM
    {
        public const string InStnNo = "1";

        public static Dictionary<string, PLCProcesser> Dic_PlcProc = new Dictionary<string, PLCProcesser>();

        //public static Dictionary<string, SRM_T_Status> Dic_SRM_STS = new Dictionary<string, SRM_T_Status>();
        //public static Dictionary<string, SRM_Allow> Dic_SRM_Allow = new Dictionary<string, SRM_Allow>();
        //堆垛机请求字典，储存堆垛机请求DB块对应类

        public static Dictionary<string, SRM_Request> Dic_SRM_Request = new Dictionary<string, SRM_Request>();
        //堆垛机应答字典，储存输送机应答DB块对应类
        public static Dictionary<string, SRM_Respone> Dic_SRM_Res = new Dictionary<string, SRM_Respone>();

        public static Dictionary<string, SRM_StatusClass> Dic_SRM_STSClass = new Dictionary<string, SRM_StatusClass>();

        public static Dictionary<string, SRM_PlatformStatus> Dic_SRM_PS = new Dictionary<string, SRM_PlatformStatus>();

        public static Dictionary<string, SRM_PlatStatusWrite> Dic_SRM_SPW = new Dictionary<string, SRM_PlatStatusWrite>();

        public static SRMapingList SysSRMMaping = new SRMapingList();

        /// <summary>
        /// 初始化PLC连接
        /// </summary>
        public static void InitConn()
        {
            string SRM01_IPStr = ConfigurationManager.AppSettings["SRM01"].ToString();
            string ip1, ip2;
            int port1, port2;
            int slot1, slot2;
            ip1 = SRM01_IPStr.Split(':')[0]; port1 = int.Parse(SRM01_IPStr.Split(':')[1]); slot1 = int.Parse(SRM01_IPStr.Split(':')[2]);
            Dic_PlcProc.Add("SRM01", new PLCProcesser() { Name = "SRM01", IP = ip1, Port = port1, slot = slot1, WLog = Program.Log_Agent_SRM });
        }

        /// <summary>
        /// 初始化PLC数据项
        /// </summary>
        public static void InitItem()
        {
            try
            {
                Dic_SRM_STSClass.Add("106", new SRM_StatusClass() { address = new PLCAddress("SRM01.DB6.DBW0") });
                //Dic_SRM_STSClass.Add("107", new SRM_StatusClass() { address = new PLCAddress("SRM01.DB7.DBW0") });

                Dic_SRM_Request.Add("6", new SRM_Request() { address = new PLCAddress("SRM01.DB7.DBW0")});
                Dic_SRM_Res.Add("6", new SRM_Respone() { address = new PLCAddress("SRM01.DB6.DBW0") });
                //Dic_SRM_Request.Add("10", new SRM_Request() { address = new PLCAddress("SRM01.DB10.DBW0"), StnType = eStationType.EP });
                Dic_SRM_PS.Add("10", new SRM_PlatformStatus() { address = new PLCAddress("SRM01.DB1.DBW0") });
                Dic_SRM_SPW.Add("11", new SRM_PlatStatusWrite() { address = new PLCAddress("SRM01.DB2.DBW0") });

                SysSRMMaping.Add(new SRMStnMaping { ConNodeID = "102", LinkLine = "1", StnNo = PLCSystem_SRM.InStnNo, StnType = eStationType.EP });
            }
            catch (Exception ex)
            {
                Program.Log_Agent_SRM.WriteLog(Enum_LogType.LogType_Error,
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
        }

        /// <summary>
        /// 堆垛机PLC连接的处理逻辑
        /// </summary>
        /// <param name="Sender"></param>
        /// <param name="e"></param>
        private static void V_LoopWrite(object Sender, EArgOnHandle_Process e)
        {
            string plcID = ((PLCProcesser)Sender).Name;
            string currentSRMNo = "";
            try
            {
                var arry_sts = Dic_SRM_STSClass.Where(item => item.Value.address.PLC == plcID).ToList();

                var arry_pc = Dic_SRM_PS.Where(item => item.Value.address.PLC == plcID).ToList();
                bool f = false;
                int r = 0;
                int c = 0;
                foreach (var pc in arry_pc)
                {
                    f = pc.Value.LoadFromPLC(e.PLCConn,4);
                }

                int s_finish = 0;
                int s_fault = 1;
                int s_runMaintenance = 1;
                int s_run = 1;
                int s_FrGrid = 0;
                int s_FrTier = 0;
                int s_pattern=0;

                int s_FrStand=0;//站
                int s_FrLine=0;
                int s_FrGrids=0;
                int s_FrTiers=0;

                int s_ToStand=0;//站
                int s_ToLine=0;
                int s_ToGrid=0;
                int s_ToTier=0;
                string str;

                var arry_res = Dic_SRM_Res.Where(item => item.Value.address.PLC == plcID).ToList();
                foreach(var res in arry_res)
                {
                    res.Value.LoadFromPLC(e.PLCConn);

                    //完成信号
                    s_finish = res.Value.s_finish;

                    //故障
                    s_fault = res.Value.s_fault;

                    //运行维护
                    s_runMaintenance = res.Value.s_runMaintenance;

                    //运行信号
                    s_run = res.Value.s_run;

                    //格
                    s_FrGrid = res.Value.s_FrGrid;

                    //层
                    s_FrTier = res.Value.s_FrTier;

                }

                #region 当前指令
                var arry_dd = Dic_SRM_Request.Where(item => item.Value.address.PLC == plcID).ToList();
                foreach (var s in arry_dd)
                {
                    s.Value.LoadFromPLC(e.PLCConn);
                    int s_serial = s.Value.s_serial;
                    s_pattern = s.Value.s_pattern;//模式

                    s_FrStand = s.Value.s_FrStand;//站
                    s_FrLine = s.Value.s_FrLine;
                    s_FrGrids = s.Value.s_FrGrid;
                    s_FrTiers = s.Value.s_FrTier;

                    s_ToStand = s.Value.s_ToStand;//站
                    s_ToLine = s.Value.s_ToLine;
                    s_ToGrid = s.Value.s_ToGrid;
                    s_ToTier = s.Value.s_ToTier;
                }
                #endregion

                var arry_req = Dic_SRM_Request.Where(item => item.Value.address.PLC == plcID).ToList();
                WOrderInfo w = new WOrderInfo();
                foreach (var s in arry_req)
                {
                    //currentSRMNo = s.Key;
                    //s.Value.LoadFromPLC(e.PLCConn);
                    r = s.Value.s_FrStand;
                    c = s.Value.s_ToStand;

                    #region 写入数据
                    if (s_finish == 1 && s_fault == 0 && s_runMaintenance == 0)
                    {
                        //判断当前指令是否有值
                        str = w.OrderInfo(s_pattern);
                        //更新数据库
                        if (str == "出库")
                        {
                            w.OutAccount(s_FrLine, s_FrGrids, s_FrTiers, s_ToStand);
                        }
                        else if (str == "入库")
                        {
                            w.InAccount(s_FrStand, s_ToLine, s_ToGrid, s_ToTier);
                        }

                        //判断指令排队序列
                        string strs = w.Write();

                        if (strs == "S")
                        {
                            w.ClearAccount();
                            //写入数据成功后  完成信号改为0
                            if (s.Value.WirteToPLC(e.PLCConn))
                            {
                                foreach (var h in PLCSystem_SRM.Dic_SRM_Res)
                                {
                                    h.Value.s_finish = 0;
                                    h.Value.WirteToPLC(e.PLCConn);
                                    int aaa = h.Value.s_finish;
                                }
                                //系统过账
                                w.Account();
                            }
                        }
                        if (strs == "N")
                        {
                            #region 清除数据
                            s.Value.s_serial = 0;
                            s.Value.s_pattern = 0;
                            s.Value.s_FrStand = 0;
                            s.Value.s_FrLine = 0;
                            s.Value.s_FrGrid = 0;
                            s.Value.s_FrTier = 0;

                            s.Value.s_ToStand = 0;
                            s.Value.s_ToLine = 0;
                            s.Value.s_ToGrid = 0;
                            s.Value.s_ToTier = 0;
                            s.Value.WirteToPLC(e.PLCConn);
                            w.ClearAccount();
                            #endregion
                        }
                        #region 判断当前指令  格 层是否有值
                        //if (str == "写入")
                        //{
                        //    //执行order_info 中排队指令
                        //    w.Write();

                        //    //写入数据成功后  完成信号改为0
                        //    if (s.Value.WirteToPLC(e.PLCConn))
                        //    {
                        //        foreach (var h in PLCSystem_SRM.Dic_SRM_Res)
                        //        {
                        //            h.Value.s_finish = 0;
                        //            h.Value.WirteToPLC(e.PLCConn);
                        //            int aaa = h.Value.s_finish;
                        //        }
                        //        #region 清除数据
                        //        //s.Value.s_serial = 0;
                        //        //s.Value.s_pattern = 0;
                        //        //s.Value.s_FrStand = 0;
                        //        //s.Value.s_FrLine = 0;
                        //        //s.Value.s_FrGrid = 0;
                        //        //s.Value.s_FrTier = 0;

                        //        //s.Value.s_ToStand = 0;
                        //        //s.Value.s_ToLine = 0;
                        //        //s.Value.s_ToGrid = 0;
                        //        //s.Value.s_ToTier = 0;
                        //        //s.Value.WirteToPLC(e.PLCConn);
                        //        #endregion
                        //        //系统过账
                        //        w.Account();
                        //    }
                        //}

                        //if (str == "过账")
                        //{
                        //    s.Value.s_serial = 0;
                        //    s.Value.s_pattern = 0;
                        //    s.Value.s_FrStand = 0;
                        //    s.Value.s_FrLine = 0;
                        //    s.Value.s_FrGrid = 0;
                        //    s.Value.s_FrTier = 0;

                        //    s.Value.s_ToStand = 0;
                        //    s.Value.s_ToLine = 0;
                        //    s.Value.s_ToGrid = 0;
                        //    s.Value.s_ToTier = 0;

                        //    if (s.Value.WirteToPLC(e.PLCConn))
                        //    {
                        //        foreach (var h in PLCSystem_SRM.Dic_SRM_Res)
                        //        {
                        //            h.Value.s_finish = 0;
                        //            h.Value.WirteToPLC(e.PLCConn);
                        //            int aaa = h.Value.s_finish;
                        //        }
                        //        #region 完成后指令过账
                        //        w.ClearAccount();
                        //        #endregion
                        //    }
                        //}

                        #region 清除数据
                        //if (of1!=1)
                        //{
                        //    s.Value.s_serial = 0;
                        //    s.Value.s_pattern = 0;
                        //    s.Value.s_FrStand = 0;
                        //    s.Value.s_FrLine = 0;
                        //    s.Value.s_FrGrid = 0;
                        //    s.Value.s_FrTier = 0;

                        //    s.Value.s_ToStand = 0;
                        //    s.Value.s_ToLine = 0;
                        //    s.Value.s_ToGrid = 0;
                        //    s.Value.s_ToTier = 0;

                        //    if (s.Value.WirteToPLC(e.PLCConn))
                        //    {
                        //        foreach (var h in PLCSystem_SRM.Dic_SRM_Res)
                        //        {
                        //            h.Value.s_finish = 0;
                        //            h.Value.WirteToPLC(e.PLCConn);
                        //            int aaa = h.Value.s_finish;
                        //        }
                        //        #region 完成后指令过账
                        //        w.ClearAccount();
                        //        #endregion
                        //    }
                        //}
                        #endregion

                        #endregion
                    }
                    #endregion

                    #region 清除指令
                    //if (s_finish == 0)
                    //{
                    //    s.Value.s_serial = 0;
                    //    s.Value.s_pattern = 0;
                    //    s.Value.s_FrStand = 0;
                    //    s.Value.s_FrLine = 0;
                    //    s.Value.s_FrGrid = 0;
                    //    s.Value.s_FrTier = 0;

                    //    s.Value.s_ToStand = 0;
                    //    s.Value.s_ToLine = 0;
                    //    s.Value.s_ToGrid = 0;
                    //    s.Value.s_ToTier = 0;

                    //    if (s.Value.WirteToPLC(e.PLCConn))
                    //    {
                    //        foreach (var h in PLCSystem_SRM.Dic_SRM_Res)
                    //        {
                    //            h.Value.s_finish = 0;
                    //            h.Value.WirteToPLC(e.PLCConn);
                    //            int aaa = h.Value.s_finish;
                    //        }
                    //        #region 完成后指令过账
                    //        w.ClearAccount();
                    //        #endregion
                    //    }
                    //}
                    #endregion
                }

                if (s_fault == 1) //故障
                {
                    w.Fault();
                    foreach (var s in PLCSystem_SRM.Dic_SRM_Request)
                    {
                        s.Value.s_serial = 0;
                        s.Value.s_pattern = 0;
                        s.Value.s_FrStand = 0;
                        s.Value.s_FrLine = 0;
                        s.Value.s_FrGrid = 0;
                        s.Value.s_FrTier = 0;

                        s.Value.s_ToStand = 0;
                        s.Value.s_ToLine = 0;
                        s.Value.s_ToGrid = 0;
                        s.Value.s_ToTier = 0;
                        s.Value.WirteToPLC(e.PLCConn);
                    }
                }

                e.result = true;
            }
            catch (Exception ex)
            {
                Program.Log_Agent_SRM.WriteLog(Enum_LogType.LogType_Error,
                                       Enum_LogGrade.LogGrade_Nin,
                                       Enum_LogMessageType.LogMsgType_Exception,
                                       string.Format("V_LoopWrite-[{0}]-[{1}]", plcID, currentSRMNo),
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
            //foreach (var v in Dic_PlcProc.Values)
            //{
            //    v.IsBeginProcess = false;
            //    v.LoopWrite -= V_LoopWrite;
            //    v.Close();
            //}
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
            //PLCProcesser plc = new PLCProcesser();
            //string status = null;
            //if(plc.Status.ToString()== "NotOpen")
            //{
            //    status = "建立连接...";
            //}
            //if (plc.Status.ToString() == "Connected")
            //{
            //    status = "连接";
            //}
            //if (plc.Status.ToString() == "Closed")
            //{
            //    status = "断开";
            //}
            //if (plc.Status.ToString() == "Failed")
            //{
            //    status = "Failed";
            //}
            //return status;
        }

        public static int GetLineByhouseNumber(string houseNumber)
        {
            return int.Parse(houseNumber.Substring(0, 2));
        }
    }
    public enum eStationType
    {
        /// <summary>
        /// 堆垛机取货口
        /// </summary>
        EP,
        /// <summary>
        /// 堆垛机放货口
        /// </summary>
        AP,
        /// <summary>
        ///输送线入库口
        /// </summary>
        IP,
        /// <summary>
        /// 外检扫描入库申请申请点
        /// </summary>
        SP,
        /// <summary>
        ///输送线出库口
        /// </summary>
        OP
    }
    public class SRMStnMaping
    {
        public string LinkLine;
        public eStationType StnType;
        public string StnNo;
        public string ConNodeID;
        public string Area;

        public override string ToString()
        {
            return string.Format("Area:{0},ConNodeID:{1},LinkLine:{2},StnType:{3},StnNo:{4}", Area, ConNodeID, LinkLine, StnType.ToString(), StnNo);
        }
    }
    public class SRMapingList : List<SRMStnMaping>
    {
        public string GetConNodeID(string pLinkLine, string pStnNo, eStationType pStnType)
        {
            var v = this.Where((e) => e.LinkLine == pLinkLine && e.StnNo == pStnNo && e.StnType == pStnType).ToList();
            if (v == null & v.Count < 1) return "";
            return v[0].ConNodeID;
        }
    }
}

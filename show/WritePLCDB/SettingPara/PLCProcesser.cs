using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LIB_Common;
using LIB_Communation;
using System.Threading;
using LIB_Log;
namespace SettingPara
{
    /// <summary>
    /// 定义PLC数据事件数据参数
    /// </summary>
    public class EArgOnHandle_Process : EventArgs
    {
        public bool result = true;
        public CallDave.daveConnection PLCConn;
        public EArgOnHandle_Process(CallDave.daveConnection conn)
        {
            PLCConn = conn;
        }
    }
    /// <summary>
    /// 处理数代理定义
    /// </summary>
    /// <param name="Sender"></param>
    /// <param name="e"></param>
    public delegate void DlgProcessOnHandle(object Sender, EArgOnHandle_Process e);

    /// <summary>
    /// PLC连接状态
    /// </summary>
    public enum ePLCStatus
    {
        NotOpen,
        Connected,
        Closed,
        Failed
    }
    /// <summary>
    /// PLC连接处理类
    /// </summary>
    public class PLCProcesser
    {
        public ePLCStatus Status = ePLCStatus.NotOpen;
        public string Name = "";
        public  bool IsBeginProcess = false;
        public  int ProecssSleepTime = 500;
        public  int MaxReConnTime = 100;
        public  int ReConnSleep = 1000;
        public  string Err_Handle = "";
        public  int MaxProcessNum = 1;
        public  bool ProcessIsClosed = false;
        /// <summary>
        /// 默认IP
        /// </summary>
        public  string IP = "192.168.1.4";
        /// <summary>
        /// 默认端口
        /// </summary>
        public  int Port = 102;
        public int slot = 2;
        public int rack = 0;

        public  event DlgProcessOnHandle LoopWrite;
        public  WriteErrorLog WLog = new WriteErrorLog();
        private  CallDave.daveConnection _dVC;

        public  CallDave.daveConnection DVC
        {
            get { return this._dVC; }
        }
        public PLCProcesser()
        {
            WLog.SleepTime = 1000;
            WLog.logName = string.Format("PLCProcesser_{0}",this.Name);
        }
        public void Start()
        {
            WLog.Start(WLog);
            IsBeginProcess = true;
            new Thread(this.Run).Start(); 
        }
        protected void Run()
        {
            try
            {
                int plcMPI = 2;
                CallDave.daveInterface di = CreateDaveInterface_Define(IP, Port);
                while (IsBeginProcess && di!=null)
                {
                    int res = di.initAdapter();
                    if (res == 0)
                    {
                        _dVC = new CallDave.daveConnection(di, plcMPI, rack, slot);
                        if (0 == _dVC.connectPLC())
                        {
                            Status = ePLCStatus.Connected; ;
                            int times = 1;
                            if (LoopWrite != null)
                            {
                                do
                                {
                                    EArgOnHandle_Process e = new EArgOnHandle_Process(_dVC);
                                    LoopWrite(this, e);
                                    if (e.result)
                                    {
                                        times = 0;
                                    }
                                    else
                                    {
                                        times++;
                                    }
                                    Thread.Sleep(this.ProecssSleepTime);
                                } while (times <= MaxReConnTime);
                            }
                            //Log PLCProcesser.ReConnSleep 后重新连接
                            WLog.WriteLog(Enum_LogType.LogType_Communication, Enum_LogGrade.LogGrade_Nin, Enum_LogMessageType.LogMsgType_Event, "Run", "ReConnSleep", "");
                            Thread.Sleep(this.ReConnSleep);
                        }
                        Thread.Sleep(this.ReConnSleep);
                    }
                }
                if(di!=null)
                    di.disconnectAdapter();
                Status = ePLCStatus.Closed;
                WLog.WriteLog(Enum_LogType.LogType_Error, Enum_LogGrade.LogGrade_Fiv, Enum_LogMessageType.LogMsgType_Exception, "Run", "Closed", string.Format("IP:{0},Port:{1},Flag:{2}",IP,Port,IsBeginProcess));

            }
            catch (Exception ex)
            {
                Status = ePLCStatus.Failed;
                WLog.WriteLog(Enum_LogType.LogType_Error, Enum_LogGrade.LogGrade_Nin, Enum_LogMessageType.LogMsgType_Exception, "Run", ex.Message, ex.StackTrace);
            }
        }
        public static CallDave.daveInterface CreateDaveInterface_Define(string ip, int port)
        {
            CallDave.daveOSserialType fds;
            CallDave.daveInterface di = null;
            int localMPI = 0;
            fds.rfd = CallDave.openSocket(port, ip);
            fds.wfd = fds.rfd;
            if (fds.rfd > 0)
            {
                di = new CallDave.daveInterface(fds, "IF1", localMPI, CallDave.daveProtoISOTCP, CallDave.daveSpeed187k);
                di.setTimeout(1000000);
                //int res = di.initAdapter();
            }
            return di;
        }
        public void Close()
        {
            IsBeginProcess = false;
            if(_dVC!=null)
            _dVC.disconnectPLC();
        }

    }
}

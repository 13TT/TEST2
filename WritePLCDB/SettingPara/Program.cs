using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Configuration;
using LIB_Log;
namespace SettingPara
{
    static class Program
    {
        public static WriteErrorLog Log_Agent_CON;
        public static WriteErrorLog Log_Agent_SRM;
        public static WriteErrorLog Log_Agent_BUS;
        public static WriteErrorLog Log_Agent_DB;
        public static WriteErrorLog Log_Agent_TEMP;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            InitLog();
            Application.Run(new frmLogin());
        }
        private static void InitLog()
        {
            string logType = ConfigurationManager.AppSettings["logType"].ToString().Trim();
            string logGrade = ConfigurationManager.AppSettings["logGrade"].ToString().Trim();
            string logMsgType = ConfigurationManager.AppSettings["LogMsgType"].ToString().Trim();
            string logFunName = ConfigurationManager.AppSettings["LogFunctionName"].ToString().Trim();
            string logMessage = ConfigurationManager.AppSettings["LogMessage"].ToString().Trim();
            string logDesp = ConfigurationManager.AppSettings["LogDescription"].ToString().Trim();
            string seperator = ConfigurationManager.AppSettings["Seperator"].ToString().Trim();

            Log_Agent_CON = new WriteErrorLog();
            Log_Agent_CON.SetLogPara(logType, logGrade, logMsgType, logFunName, logMessage, logDesp, seperator);
            Log_Agent_CON.IsBeginProcess = true;
            Log_Agent_CON.SleepTime = 500;
            Log_Agent_CON.logName = "PLCDataAgent_CON";
            Log_Agent_CON.Start(Log_Agent_CON);
            Log_Agent_CON.WriteLog(Enum_LogType.LogType_StartStop,
                                   Enum_LogGrade.LogGrade_Nin,
                                   Enum_LogMessageType.LogMsgType_Event,
                                   "StartLog",
                                   "Log Thread Start",
                                   "");
            Log_Agent_SRM = new WriteErrorLog();
            Log_Agent_SRM.SetLogPara(logType, logGrade, logMsgType, logFunName, logMessage, logDesp, seperator);
            Log_Agent_SRM.IsBeginProcess = true;
            Log_Agent_SRM.SleepTime = 500;
            Log_Agent_SRM.logName = "PLCDataAgent_SRM";
            Log_Agent_SRM.Start(Log_Agent_SRM);
            Log_Agent_SRM.WriteLog(Enum_LogType.LogType_StartStop,
                                   Enum_LogGrade.LogGrade_Nin,
                                   Enum_LogMessageType.LogMsgType_Event,
                                   "StartLog",
                                   "Log Thread Start",
                                   "");
            Log_Agent_BUS = new WriteErrorLog();
            Log_Agent_BUS.SetLogPara(logType, logGrade, logMsgType, logFunName, logMessage, logDesp, seperator);
            Log_Agent_BUS.IsBeginProcess = true;
            Log_Agent_BUS.SleepTime = 500;
            Log_Agent_BUS.logName = "WMBusniess";
            Log_Agent_BUS.Start(Log_Agent_BUS);
            Log_Agent_BUS.WriteLog(Enum_LogType.LogType_StartStop,
                                   Enum_LogGrade.LogGrade_Nin,
                                   Enum_LogMessageType.LogMsgType_Event,
                                   "StartLog",
                                   "Log Thread Start",
                                   "");
            Log_Agent_DB = new WriteErrorLog();
            Log_Agent_DB.SetLogPara(logType, logGrade, logMsgType, logFunName, logMessage, logDesp, seperator);
            Log_Agent_DB.IsBeginProcess = true;
            Log_Agent_DB.SleepTime = 500;
            Log_Agent_DB.logName = "SQLDubug";
            Log_Agent_DB.Start(Log_Agent_DB);
            Log_Agent_DB.WriteLog(Enum_LogType.LogType_Debug,
                                   Enum_LogGrade.LogGrade_Nin,
                                   Enum_LogMessageType.LogMsgType_Event,
                                   "StartLog",
                                   "Log Thread Start",
                                   "");
            Log_Agent_TEMP = new WriteErrorLog();
            Log_Agent_TEMP.SetLogPara(logType, logGrade, logMsgType, logFunName, logMessage, logDesp, seperator);
            Log_Agent_TEMP.IsBeginProcess = true;
            Log_Agent_TEMP.SleepTime = 500;
            Log_Agent_TEMP.logName = "TagTemp";
            Log_Agent_TEMP.Start(Log_Agent_DB);
            Log_Agent_TEMP.WriteLog(Enum_LogType.LogType_StartStop,
                                   Enum_LogGrade.LogGrade_Nin,
                                   Enum_LogMessageType.LogMsgType_Event,
                                   "StartLog",
                                   "Log Thread Start",
                                   "");
        }
    }
}

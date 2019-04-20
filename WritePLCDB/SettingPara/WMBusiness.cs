using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;
using System.Configuration;
using LIB_Log;
namespace SettingPara
{
    /// <summary>
    /// WMS/WCS业务数据处理类(用于实现业务逻辑，读写数据库) -----------未完成-----------
    /// </summary>
    public class WMBusiness
    {
        // public DBAccess_MySql thedb=null;
        public WriteErrorLog Wlog;
        public WMBusiness()
        {
            //  thedb = new DBAccess_MySql("MySql");
        }
        /// <summary>
        /// 得到可以下发给堆垛机的任务
        /// </summary>
        /// <param name="outString"></param>
        /// <returns></returns>
        public DataTable GetCMD_NoSend(DBAccess_MySql db, string line, string tuid, string stn, out string outString)
        {
            DataTable dt = null;
            outString = "";
            string sql = string.Format(ConfigurationManager.AppSettings["SQL_GetCMD_NoSend_In"].Replace('\n', ' '), line, tuid, stn);
            var rlt = db.QuerySQL_ToTable(sql, out dt, out outString);
            return dt;
        }

        public DataTable GetCMD_Doing(DBAccess_MySql db, string line, out string outString)
        {
            DataTable dt = null;
            outString = "";
            string sql = string.Format(ConfigurationManager.AppSettings["SQL_GetCMD_Doing"].Replace('\n', ' '), line);
            var rlt = db.QuerySQL_ToTable(sql, out dt, out outString);
            return dt;
        }

        public DataTable GetCMD_Doing(string line, out string outString)
        {
            var db = new DBAccess_MySql("MySql");
            return GetCMD_Doing(db, line, out outString);
        }
        /// <summary>
        /// 取得可出库货位列表
        /// </summary>
        /// <param name="outString"></param>
        /// <returns></returns>
        public DataTable GetAllocationLoc(DBAccess_MySql db, out string outString)
        {
            DataTable dt;
            outString = "";

            string sql = ConfigurationManager.AppSettings["SQL_GetAllocationLoc"].Replace('\n', ' ');
            var rlt = db.QuerySQL_ToTable(sql, out dt, out outString);
            return dt;
        }
        /// <summary>
        /// 分配货位
        /// </summary>
        /// <param name="barcode"></param>
        /// <param name="outString"></param>
        /// <returns></returns>
        public bool AllocationLoc(DBAccess_MySql db, string barcode, out string Loc, out string outString)
        {
            Loc = "";
            BarcodeInfo bi = new BarcodeInfo(barcode);
            outString = "";
            //找到空格位最少的巷道
            //找到该巷道货位 按 层，列，排  升序排序的 的一个货位
            //预定货位-将状态设置为入库预定I 
            //返货预定储位
            DataTable dt = GetAllocationLoc(db, out outString);
            if (dt == null || dt.Rows.Count <= 0) return false;
            Loc = dt.Rows[0]["house_number"].ToString();
            string sql = ConfigurationManager.AppSettings["SQL_BookingLoc"].Replace('\n', ' ');
            sql = string.Format(sql, Loc);
            var rlt = db.ExecSql(sql, out outString);
            return rlt == DBExeResult.Successed;
        }
        /// <summary>
        /// 取消分配
        /// </summary>
        /// <param name="houseNumber"></param>
        /// <param name="outString"></param>
        /// <returns></returns>
        public bool CancelAllocationLoc(DBAccess_MySql db, string houseNumber, out string outString)
        {
            outString = "";
            //取消预定  
            string sql = string.Format(ConfigurationManager.AppSettings["SQL_UnBookingLoc"].Replace('\n', ' '), houseNumber);
            var rlt = db.ExecSql(sql, out outString);
            return rlt == DBExeResult.Successed;
        }

        /// <summary>
        /// 创建库存
        /// </summary>
        /// <param name="outString"></param>
        /// <returns></returns>
        public bool CreateStock(DBAccess_MySql db, string houseNumber, BarcodeInfo info, out string outString)
        {
            outString = "";
            string sql = string.Format(ConfigurationManager.AppSettings["SQL_CreateStock"].Replace('\n', ' '), houseNumber, info.ProductCode, info.BatchNo, info.UnitQty, info.VendorCode, info.PacketSeqNo, info.Barcode);
            var rlt = db.ExecSql(sql, out outString);
            return rlt == DBExeResult.Successed;
        }
        public bool CreateStock(string houseNumber, BarcodeInfo info, out string outString)
        {
            var db = new DBAccess_MySql("MySql");
            return CreateStock(db, houseNumber, info, out outString);
        }

        /// <summary>
        /// 创建给输送机及堆垛机的任务
        /// </summary>
        /// <param name="outString"></param>
        /// <returns></returns>
        public bool CreateOrderInfo_In(DBAccess_MySql db, string barcode, string FromLoc, string ToLoc,string Line, out string outString)
        {
            outString = "";
            string sql = ConfigurationManager.AppSettings["SQL_CreateOrder_In"].Replace('\n', ' ');
            sql = string.Format(sql, FromLoc, ToLoc, barcode,Line);
            var rlt = db.ExecSql(sql, out outString);
            return rlt == DBExeResult.Successed;
        }

        public bool CreateOrderInfo_Out(DBAccess_MySql db, string barcode, string FromLoc, string ToLoc, out string outString)
        {
            outString = "";
            string sql = ConfigurationManager.AppSettings["SQL_CreateOrder_Out"].Replace('\n', ' ');
            sql = string.Format(sql, FromLoc, ToLoc, barcode);
            var rlt = db.ExecSql(sql, out outString);
            return rlt == DBExeResult.Successed;
        }

        public bool CreateOrderInfo_Out_T(DBAccess_MySql db, string barcode, string FromLoc, string ToLoc, out string outString)
        {
            outString = "";
            string sql = ConfigurationManager.AppSettings["SQL_CreateOrder_Out_T"].Replace('\n', ' ');
            sql = string.Format(sql, FromLoc, ToLoc, barcode);
            var rlt = db.ExecSql(sql, out outString);
            return rlt == DBExeResult.Successed;
        }
        /// <summary>
        /// 指定位置出库（用于紧急出库）
        /// </summary>
        /// <param name="HouseNumber"></param>
        /// <param name="outString"></param>
        /// <returns></returns>
        public bool OutStore_AssignLoction(string HouseNumber,int j, out string outString)
        {
            bool r = false;
            outString = "";
            var db = new DBAccess_MySql("MySql");
            DBAccess_MySql dby = new DBAccess_MySql();
            try
            {
                //查找出HouseNumber内所有库存，更新其库存计划出库量（参考WMS-Client的里的出库相关业务的“确认”按钮内的代码）
                string sql = string.Format(ConfigurationManager.AppSettings["SQL_AssignGoods"].Replace('\n', ' '), HouseNumber);
                dby = db.ReturnSQL_String(sql, out outString);
                try
                {
                    string a = null;
                    string b = null;
                    string c = null;
                    int d = 0;
                    string e = null;
                    while (dby.rec.Read())
                    {
                        a = dby.rec.GetString(0);
                        b = dby.rec.GetString(1);
                        c = dby.rec.GetString(2);
                        d = dby.rec.GetInt32(3);
                        e = dby.rec.GetString(4);
                    }
                    dby.connMysql.Close();
                    dby.rec.Close();
                    if (a != "")
                    {
                        string sql1 = string.Format(ConfigurationManager.AppSettings["SQL_UpdateGoods"].Replace('\n', ' '), d, a, b);
                        var rlt1 = db.ExecSql(sql1, out outString);

                        //改变库位状态
                        string sql2 = string.Format(ConfigurationManager.AppSettings["SQL_OutStateGoods"].Replace('\n', ' '), a);
                        var rlt2 = db.ExecSql(sql2, out outString);

                        //改变储位状态
                        string sql3 = string.Format(ConfigurationManager.AppSettings["SQL_OutDataGoods"].Replace('\n', ' '), a,b);
                        var rlt3 = db.ExecSql(sql3, out outString);

                        //判断是不是全部物品出库,如果是全部都出库了,那么是出库命令2.否则是减料出库3
                        string sql4 = string.Format(ConfigurationManager.AppSettings["SQL_OutSelectNull"].Replace('\n', ' '), a);
                        var obj4 = db.QuerySQL_GetValue(sql4, out outString);
                        if(obj4 == null)
                        {
                            //创建出库指令(order_info)
                            int i = int.Parse(a.Substring(1, 1));
                            string sql5 = string.Format(ConfigurationManager.AppSettings["SQL_AddOutOrder"].Replace('\n', ' '), a,i,e,j);
                            var rlt5 = db.ExecSql(sql5, out outString);
                        }
                        else
                        {
                        }
                    }
                }
                catch
                {
                   
                }
                finally
                {
                    
                }
                return !r;
            }
            catch(Exception ex)
            {
                outString = ex.Message;
                return r;
            }
        }
        /// <summary>
        /// 判断重复指令
        /// </summary>
        /// <returns></returns>
        public bool IsRepeatPallet(string barcode, out string outString)
        {
            outString = "";
            var db = new DBAccess_MySql("MySql");
            try
            {
                string sql = string.Format(ConfigurationManager.AppSettings["SQL_IsRepeatPallet"].Replace('\n', ' '), barcode);
                var obj = db.QuerySQL_GetValue(sql, out outString);
                return obj != null;
            }
            catch (Exception ex)
            {
                outString = ex.Message;
                return false;
            }
        }
        /// <summary>
        ///入库申请
        /// </summary>
        /// <param name="barcode"></param>
        /// <param name="RequestLoc"></param>
        /// <param name="fromStn"></param>
        /// <param name="outString"></param>
        /// <returns></returns>
        public bool RequestInStock(string barcode, string RequestLoc, out string fromStn, out string outString)
        {
            string newHouseNumber = "";
            string Line = "";
            bool r = false;
            fromStn = "";
            outString = "";
            //1判断是否重复申请
            //2.创建库存
            //3.分配货位
            //4.获取巷到对应EP点
            //5.创建入库执行
            var db = new DBAccess_MySql("MySql");
            try
            {
                db.StartTran();
                r = IsRepeatPallet(barcode,out outString);
                if (r)
                    throw new ApplicationException(string.Format("[{0}]重复的入库申请!{1}", barcode, outString));
                r = AllocationLoc(db, barcode, out newHouseNumber, out outString);
                if (!r)
                    throw new ApplicationException(string.Format("[{0}]分配货位失败!{1}", barcode, outString));
                BarcodeInfo bi = new BarcodeInfo(barcode);
                r = CreateStock(db, newHouseNumber, bi, out outString);
                if (!r)
                    throw new ApplicationException(string.Format("[{0}]创建库存失败{!1}", barcode, outString));
                Line = PLCSystem_SRM.GetLineByhouseNumber(newHouseNumber).ToString();
                fromStn = PLCSystem_SRM.SysSRMMaping.GetConNodeID(Line, PLCSystem_SRM.InStnNo, eStationType.EP);
                if (Line == "" || fromStn == "")
                    throw new ApplicationException(string.Format("[{0}]获取匹配巷道及入库站点失败!{1}", barcode, outString));
                r = CreateOrderInfo_In(db, barcode, PLCSystem_SRM.InStnNo, newHouseNumber,Line, out outString);
                if (!r)
                    throw new ApplicationException(string.Format("[{0}]创建入库指令失败!{1}", barcode, outString));
                db.CommitTran();
            }
            catch (Exception ex)
            {
                outString = ex.Message;
                Program.Log_Agent_BUS.WriteLog(Enum_LogType.LogType_Error,
                        Enum_LogGrade.LogGrade_Nin,
                        Enum_LogMessageType.LogMsgType_Exception,
                        "RequestInStocke",
                        ex.Message,
                        ex.StackTrace);
                r = false;
                db.RollBack();
            }
            return r;
        }

        /// <summary>
        /// 检测输送机任务完成(查找改巷到-1的 指令改为0)
        /// </summary>
        public bool ActivateOrderInfo(string line, string stn, string tuid, out string outString)
        {
            bool r = false;
            outString = "";
            var db = new DBAccess_MySql("MySql");
            try
            {
                db.StartTran();
                DataTable dt = GetCMD_NoSend(db, line, stn, tuid, out outString);
                if (dt == null) return false;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    var dr = dt.Rows[i];
                    string sql = string.Format(ConfigurationManager.AppSettings["SQL_ActiveCMD"].Replace('\n', ' '), dr["order_id"].ToString());
                    var dbr = db.ExecSql(sql, out outString);
                    r = dbr == DBExeResult.Successed;
                }
                db.CommitTran();
                return r;
            }
            catch
            {
                r = false;
                db.RollBack();
            }
            return r;
        }
    }
        /// <summary>
        /// 条码信息
        /// </summary>
    public class BarcodeInfo
    {
        public const char cSplitShar = ':';
        public string ProductCode = "";
        public string BatchNo = "";
        public int UnitQty = 0;
        public int PacketSeqNo = 0;
        public string VendorCode = "";
        public string Other = "";
        public string Barcode="";

        /// <summary>
        /// 产品代码:生产批次号(7):每单元模组数量(2):包装单元流水号(3):供应商代码(6/7)
        /// </summary>
        /// <param name="barcode"></param>
        public BarcodeInfo(string barcode)
        { 
            this.Barcode=barcode;
            string[] temp = barcode.Split(BarcodeInfo.cSplitShar);
            if (temp.Length >= 1)  this.ProductCode = temp[0];
            if (temp.Length >= 2) this.BatchNo = temp[1];
            if (temp.Length >= 3) this.UnitQty = int.Parse(temp[2]);
            if (temp.Length >= 4) this.PacketSeqNo = int.Parse(temp[3]);
            if (temp.Length >= 5) this.VendorCode = temp[4];
            if (temp.Length >= 6) this.Other = barcode.Substring(temp[0].Length + temp[1].Length + temp[2].Length + temp[3].Length + temp[4].Length + 5);
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("[产品代码ProductCode={0}],",this.ProductCode);
            sb.AppendFormat("[生产批次号BatchNo={0}],", this.BatchNo);
            sb.AppendFormat("[每单元模组数量UnitQty={0}]", this.UnitQty);
            sb.AppendFormat("[包装单元流水号PacketSeqNo={0}]", this.PacketSeqNo);
            sb.AppendFormat("[供应商代码VendorCode={0}]", this.VendorCode);
            sb.AppendFormat("[其它Other={0}]", this.Other);
            return sb.ToString();
        }
    }
}

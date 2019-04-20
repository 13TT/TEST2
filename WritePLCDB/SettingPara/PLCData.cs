using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LIB_Common;
using LIB_Communation;

namespace SettingPara
{
    /// <summary>
    /// /PLC数据点类型
    /// </summary>
    public enum ePLCPointType
    {
        Bit,
        Byte,
        Word,
        DWord,
        String,
        Bytes
    }
    /// <summary>
    /// PLC地址
    /// </summary>
    public class PLCAddress
    {
        public string PLC;
        public int db;
        public int start;
        public int bit;

        public PLCAddress()
        { }

        public PLCAddress(string plcAddress)
        {
            string outMg;
            PLCAddress.SplitplcAddress(plcAddress,out PLC,out  db, out  start,out bit,out outMg);
        }

        public static bool SplitplcAddress(string plcAddress, out string plc ,out int db, out int start,out int bit, out string OutMsg)
        {
            plc = "";
            db = 0;
            start = 0;
            bit = 0;
            OutMsg = "";
            try
            {
                string[] temp = plcAddress.Split('.');

                if (temp.Length == 3)
                {
                    plc = temp[0];
                    if (temp[1].Substring(0, 2).ToUpper() == "DB".ToUpper())
                    {
                        db = int.Parse(temp[1].Substring(2, temp[1].Length - 2));
                    }
                    else
                    {
                        return false;
                    }
                    if (temp[2].Substring(0, 3).ToUpper() == "DBW".ToUpper())
                    {
                        start = int.Parse(temp[2].Substring(3, temp[2].Length - 3));
                    }
                    else
                    {
                        return false;
                    }
                    if (temp.Length > 2)
                    {
                        bit = int.Parse(temp[3]);
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

        public override string ToString()
        {
            return string.Format("{0}.DB{1}.DBW{2}", PLC, db, start);
        }
    }
    /// <summary>
    /// PLC数据项
    /// </summary>
    public class PLCPointItem
    {
        public string ItemCode = "";
        public string ItemName = "";
        public ePLCPointType Type;
        public object Value;
        public object OldValue;
        public PLCAddress address = new PLCAddress();
    }

    /// <summary>
    /// 输送机请求数据块映射类
    /// </summary>
    public class CON_Request
    {
        /// <summary>
        /// 填充字符
        /// </summary>
        public const string C_FillChar = " ";
        /// <summary>
        /// 左填充还是又填充
        /// </summary>
        public const int C_leftorRigh = 0;

        public eStationType StnType = eStationType.SP;

        public PLCAddress address = new PLCAddress();
        public const int cGroupLen = 58;
        public byte[] b_RequestLoc = new byte[1];
        public byte[] b_reserved = new byte[1];
        public byte[] b_TaskID = new byte[4];
        public byte[] b_ErrorCode = new byte[1];
        public byte[] b_OverRead = new byte[1];
        public byte[] b_ScanBarCode = new byte[50];

        public byte[] b_Grid = new byte[2];
        public byte[] b_Layer = new byte[2];

        public byte s_RequestLoc
        {
            get
            {
                return b_RequestLoc[0];
            }
            set
            {
                 b_RequestLoc[0] = value;
            }
        }
        public byte s_reserved
        {
            get
            {
                return b_reserved[0];
            }
            set
            {
                b_reserved[0] = value;
            }
        }
        public UInt32 s_TaskID
        {
            get
            {
                byte[] tmpbyte = new byte[b_TaskID.Length];
                Buffer.BlockCopy(b_TaskID, 0, tmpbyte, 0, b_TaskID.Length);
                Array.Reverse(tmpbyte);
                return BitConverter.ToUInt32(tmpbyte, 0);
            }
            set
            {
                byte[] tempbyte = BitConverter.GetBytes(value);
                Array.Reverse(tempbyte);
                b_TaskID = tempbyte;
            }
        }
        public byte s_ErrorCode
        {
            get
            {
                return b_ErrorCode[0];
            }
            set
            {
                b_ErrorCode[0] = value;
            }
        }
        public byte s_OverRead
        {
            get
            {
                return b_OverRead[0];
            }
            set
            {
                b_OverRead[0] = value;
            }
        }
        public string s_ScanBarCode
        {
            get
            {
                return Encoding.ASCII.GetString(b_ScanBarCode).Replace('\0', ' ').Trim();
            }
            set
            {
                ByteHelper.StringCopy(b_ScanBarCode, value, 0, b_ScanBarCode.Length, CON_Request.C_FillChar, CON_Request.C_leftorRigh);
            }
        }

        public CON_Request()
        {
        }
        public CON_Request(byte[] bytes)
        {
            this.LoadBytes(bytes);
        }

        /// <summary>
        /// 根据Byte数据填充类
        /// </summary>
        /// <param name="bytes"></param>
        private void LoadBytes(byte[] bytes)
        {

            int copy_Pionter = 0;
            try
            {
                Buffer.BlockCopy(bytes, copy_Pionter += 0, b_RequestLoc, 0, b_RequestLoc.Length);
                Buffer.BlockCopy(bytes, copy_Pionter += b_RequestLoc.Length, b_reserved, 0, b_reserved.Length);
                Buffer.BlockCopy(bytes, copy_Pionter += b_reserved.Length, b_TaskID,0 , b_TaskID.Length);
                Buffer.BlockCopy(bytes, copy_Pionter += b_TaskID.Length, b_ErrorCode,0 , b_ErrorCode.Length);
                Buffer.BlockCopy(bytes, copy_Pionter += b_ErrorCode.Length, b_OverRead, 0, b_OverRead.Length);
                Buffer.BlockCopy(bytes, copy_Pionter += b_OverRead.Length, b_ScanBarCode, 0, b_ScanBarCode.Length);
            }
            catch (Exception ex)
            {
                throw new ApplicationException(string.Format("ErrorMSG:{0},Pointer:{1},Bytes:{2}", ex.Message, copy_Pionter, ByteHelper.ToMessage(bytes, bytes.Length)));
            }
        }

        /// <summary>
        /// 转换为Byte数组
        /// </summary>
        /// <returns></returns>
        public byte[] Tobytes()
        {
            int copy_Pionter = 0;
            byte[] result = new byte[cGroupLen];
            Buffer.BlockCopy(b_RequestLoc, 0, result, copy_Pionter += 0, b_RequestLoc.Length);
            Buffer.BlockCopy(b_reserved, 0, result, copy_Pionter += b_RequestLoc.Length, b_reserved.Length);
            Buffer.BlockCopy(b_TaskID, 0, result, copy_Pionter += b_reserved.Length, b_TaskID.Length);
            Buffer.BlockCopy(b_ErrorCode, 0, result, copy_Pionter += b_TaskID.Length, b_ErrorCode.Length);
            Buffer.BlockCopy(b_OverRead, 0, result, copy_Pionter += b_ErrorCode.Length, b_OverRead.Length);
            Buffer.BlockCopy(b_ScanBarCode, 0, result, copy_Pionter += b_OverRead.Length, b_ScanBarCode.Length);
            return result;
        }

        public bool LoadFromPLC(CallDave.daveConnection _dVC)
        {
            byte[] tempbytes = new byte[CON_Request.cGroupLen];
            var rec = _dVC.readBytes(CallDave.daveDB, address.db, address.start, CON_Request.cGroupLen, tempbytes);
            if (rec != 0) return false;
            this.LoadBytes(tempbytes);
            return true;
        }

        public bool SetOverRead(CallDave.daveConnection _dVC)
        {
            var res=_dVC.writeBytes(CallDave.daveDB, address.db, address.start+ (CON_Request.cGroupLen- b_ScanBarCode.Length-1), 1, new byte[] { 1 });
            return res == 0;
        }

    }
    /// <summary>
    /// WCS相应请求数据块映射类
    /// </summary>
    public class CON_Response
    {
        /// <summary>
        /// 填充字符
        /// </summary>
        public const string C_FillChar = " ";
        /// <summary>
        /// 左填充还是又填充
        /// </summary>
        public const int C_leftorRigh = 0;

        public PLCAddress address = new PLCAddress();
        public const int cGroupLen = 58;
        public byte[] b_TaskID = new byte[4];
        public byte[] b_FromLoc = new byte[1];
        public byte[] b_ToLoc = new byte[1];
        public byte[] b_OverRead = new byte[1];
        public byte[] b_reserved = new byte[1];
        public byte[] b_TUID = new byte[50];

        public UInt32 s_TaskID
        {
            get
            {
                byte[] tmpbyte = new byte[b_TaskID.Length];
                Buffer.BlockCopy(b_TaskID, 0, tmpbyte, 0, b_TaskID.Length);
                Array.Reverse(tmpbyte);
                return BitConverter.ToUInt32(tmpbyte, 0);
            }
            set
            {

                byte[] tempbyte = BitConverter.GetBytes(value);
                Array.Reverse(tempbyte);
                b_TaskID = tempbyte;
            }
        }
        public byte s_FromLoc
        {
            get
            {
                return b_FromLoc[0];
            }
            set
            {
                b_FromLoc[0] = value;
            }
        }
        public byte s_ToLoc
        {
            get
            {
                return b_ToLoc[0];
            }
            set
            {
                b_ToLoc[0] = value;
            }
        }
        public byte s_OverRead
        {
            get
            {
                return b_OverRead[0];
            }
            set
            {
                b_OverRead[0] = value;
            }
        }
        public byte s_reserved
        {
            get
            {
                return b_reserved[0];
            }
            set
            {
                b_reserved[0] = value;
            }
        }
        public string s_TUID
        {
            get
            {
                return Encoding.ASCII.GetString(b_TUID).Replace('\0', ' ').Trim();
            }
            set
            {
                ByteHelper.StringCopy(b_TUID, value, 0, b_TUID.Length, CON_Response.C_FillChar, CON_Response.C_leftorRigh);
            }
        }

        public byte[] Tobytes()
        {
            int copy_Pionter = 0;
            byte[] result = new byte[cGroupLen];
            Buffer.BlockCopy(b_TaskID, 0, result, copy_Pionter += 0, b_TaskID.Length);
            Buffer.BlockCopy(b_FromLoc, 0, result, copy_Pionter += b_TaskID.Length, b_FromLoc.Length);
            Buffer.BlockCopy(b_ToLoc, 0, result, copy_Pionter += b_FromLoc.Length, b_ToLoc.Length);
            Buffer.BlockCopy(b_OverRead, 0, result, copy_Pionter += b_ToLoc.Length, b_OverRead.Length);
            Buffer.BlockCopy(b_reserved, 0, result, copy_Pionter += b_OverRead.Length, b_reserved.Length);
            Buffer.BlockCopy(b_TUID, 0, result, copy_Pionter += b_reserved.Length, b_TUID.Length);
            return result;
        }

        /// <summary>
        /// 将应答任务写入PLC
        /// </summary>
        /// <param name="_dVC"></param>
        /// <returns></returns>
        public bool WirteToPLC(CallDave.daveConnection _dVC)
        {
            byte[] tempbytes = this.Tobytes();
            tempbytes[CON_Response.cGroupLen - b_TUID.Length- b_reserved.Length-1] = 0; //设置和为未读
            var rec = _dVC.writeBytes(CallDave.daveDB, address.db, address.start, CON_Response.cGroupLen, tempbytes);
            if (rec != 0) return false;
            return true;
        }

        /// <summary>
        /// 判断PLC是否已经处理完成，是否可以继续写入任务
        /// </summary>
        /// <param name="_dVC"></param>
        /// <returns></returns>
        public bool CheckOverRead(CallDave.daveConnection _dVC)
        {
            byte[] flag_OverRead = new byte[1];
            var res = _dVC.readBytes(CallDave.daveDB, address.db, address.start + (CON_Response.cGroupLen - b_TUID.Length - b_reserved.Length - 1), 1, flag_OverRead);
            return res == 0 && flag_OverRead[0]==1;
        }
    }
    /// <summary>
    /// 输送机状态类
    /// </summary>
    public class CON_Status
    {
        /// <summary>
        /// 填充字符
        /// </summary>
        public const string C_FillChar = " ";
        /// <summary>
        /// 左填充还是右填充
        /// </summary>
        public const int C_leftorRigh = 0;

        public PLCAddress address = new PLCAddress();
        public const int cGroupLen =56;
        public byte[] b_RequestLoc = new byte[1];
        public byte[] b_OccupyStats = new byte[1];
        public byte[] b_RuningStatus = new byte[1];
        public byte[] b_RuningWay = new byte[1];
        public byte[] b_HavingTask = new byte[1];
        public byte[] b_ErrorCode = new byte[1];
        public byte[] b_TUID = new byte[50];

        public byte s_RequestLoc
        {
            get
            {
                return b_RequestLoc[0];
            }
            set
            {
                b_RequestLoc[0] = value;
            }
        }
        public byte s_OccupyStats
        {
            get
            {
                return b_OccupyStats[0];
            }
            set
            {
                b_OccupyStats[0] = value;
            }
        }
        public byte s_RuningStatus
        {
            get
            {
                return b_RuningStatus[0];
            }
            set
            {
                b_RuningStatus[0] = value;
            }
        }
        public byte s_RuningWay
        {
            get
            {
                return b_RuningWay[0];
            }
            set
            {
                b_RuningWay[0] = value;
            }
        }
        public byte s_HavingTask
        {
            get
            {
                return b_HavingTask[0];
            }
            set
            {
                b_HavingTask[0] = value;
            }
        }
        public byte s_ErrorCode
        {
            get
            {
                return b_ErrorCode[0];
            }
            set
            {
                b_ErrorCode[0] = value;
            }
        }
        public string s_TUID
        {
            get
            {
                return Encoding.ASCII.GetString(b_TUID).Replace('\0',' ').Trim();
            }
            set
            {
                ByteHelper.StringCopy(b_TUID, value, 0, b_TUID.Length, CON_Status.C_FillChar, CON_Status.C_leftorRigh);
            }
        }

        /// <summary>
        /// 根据Byte数据填充类
        /// </summary>
        /// <param name="bytes"></param>
        private void LoadBytes(byte[] bytes)
        {
            int copy_Pionter = 0;
            try
            {
                Buffer.BlockCopy(bytes, copy_Pionter += 0, b_RequestLoc, 0, b_RequestLoc.Length);
                Buffer.BlockCopy(bytes, copy_Pionter += b_RequestLoc.Length, b_OccupyStats, 0, b_OccupyStats.Length);
                Buffer.BlockCopy(bytes, copy_Pionter += b_OccupyStats.Length, b_RuningStatus, 0, b_RuningStatus.Length);
                Buffer.BlockCopy(bytes, copy_Pionter += b_RuningStatus.Length, b_RuningWay, 0, b_RuningWay.Length);
                Buffer.BlockCopy(bytes, copy_Pionter += b_RuningWay.Length, b_HavingTask, 0, b_HavingTask.Length);
                Buffer.BlockCopy(bytes, copy_Pionter += b_HavingTask.Length, b_ErrorCode, 0, b_ErrorCode.Length);
                Buffer.BlockCopy(bytes, copy_Pionter += b_ErrorCode.Length, b_TUID, 0, b_TUID.Length);
            }
            catch (Exception ex)
            {
                throw new ApplicationException(string.Format("ErrorMSG:{0},Pointer:{1},Bytes:{2}",ex.Message, copy_Pionter,ByteHelper.ToMessage(bytes, bytes.Length)));
            }
        }

        /// <summary>
        /// 读取PLC
        /// </summary>
        /// <param name="_dVC"></param>
        /// <returns></returns>
        public bool LoadFromPLC(CallDave.daveConnection _dVC)
        {
            byte[] tempbytes = new byte[CON_Status.cGroupLen];
            var rec = _dVC.readBytes(CallDave.daveDB, address.db, address.start, CON_Status.cGroupLen, tempbytes);
            if (rec != 0) return false;
            this.LoadBytes(tempbytes);
            return true;
        }

        /// <summary>
        /// 转换为Byte数组
        /// </summary>
        /// <returns></returns>
        public byte[] Tobytes()
        {
            int copy_Pionter = 0;
            byte[] result = new byte[cGroupLen];
            Buffer.BlockCopy(b_RequestLoc, 0, result, copy_Pionter += 0, b_RequestLoc.Length);
            Buffer.BlockCopy(b_OccupyStats, 0, result, copy_Pionter += b_RequestLoc.Length, b_OccupyStats.Length);
            Buffer.BlockCopy(b_RuningStatus, 0, result, copy_Pionter += b_OccupyStats.Length, b_RuningStatus.Length);
            Buffer.BlockCopy(b_RuningWay, 0, result, copy_Pionter += b_RuningStatus.Length, b_RuningWay.Length);
            Buffer.BlockCopy(b_HavingTask, 0, result, copy_Pionter += b_RuningWay.Length, b_HavingTask.Length);
            Buffer.BlockCopy(b_ErrorCode, 0, result, copy_Pionter += b_HavingTask.Length, b_ErrorCode.Length);
            Buffer.BlockCopy(b_TUID, 0, result, copy_Pionter += b_ErrorCode.Length, b_TUID.Length);
            return result;
        }

        public override string ToString()
        {
            return string.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}", s_RequestLoc, s_OccupyStats, s_RuningStatus, s_RuningWay, s_HavingTask, s_ErrorCode, s_TUID);
        }
    }

    public class SRM_T_Status
    {
        /// <summary>
        /// 填充字符
        /// </summary>
        public const string C_FillChar = " ";
        /// <summary>
        /// 左填充还是又填充
        /// </summary>
        public const int C_leftorRigh = 0;

        public PLCAddress address = new PLCAddress();
        public const int cGroupLen = 4;
        public byte[] bT_Left = new byte[2];
        public byte[] bT_Right = new byte[2];

        public UInt16 iT_Left
        {
            get
            {
                byte[] tmpbyte = new byte[bT_Left.Length];
                Buffer.BlockCopy(bT_Left, 0, tmpbyte, 0, bT_Left.Length);
                Array.Reverse(tmpbyte);
                return BitConverter.ToUInt16(tmpbyte, 0);
            }
            set
            {

                byte[] tempbyte = BitConverter.GetBytes(value);
                Array.Reverse(tempbyte);
                bT_Left = tempbyte;
            }
        }

        public UInt16 iT_Right
        {
            get
            {
                byte[] tmpbyte = new byte[bT_Right.Length];
                Buffer.BlockCopy(bT_Right, 0, tmpbyte, 0, bT_Right.Length);
                Array.Reverse(tmpbyte);
                return BitConverter.ToUInt16(tmpbyte, 0);
            }
            set
            {

                byte[] tempbyte = BitConverter.GetBytes(value);
                Array.Reverse(tempbyte);
                bT_Right = tempbyte;
            }
        }

        public bool LoadFromPLC(CallDave.daveConnection _dVC)
        {
            byte[] tempbytes = new byte[CON_Status.cGroupLen];
            var rec = _dVC.readBytes(CallDave.daveDB, address.db, address.start, CON_Status.cGroupLen, tempbytes);
            if (rec != 0) return false;
            Buffer.BlockCopy(tempbytes, 0, bT_Left, 0, bT_Left.Length);
            Buffer.BlockCopy(tempbytes, 2, bT_Right, 0, bT_Right.Length);
            return true;
        }

        public byte[] Tobytes()
        {
            int copy_Pionter = 0;
            byte[] result = new byte[cGroupLen];
            Buffer.BlockCopy(bT_Left, 0, result, copy_Pionter += 0, bT_Left.Length);
            Buffer.BlockCopy(bT_Right, 0, result, copy_Pionter += bT_Left.Length, bT_Right.Length);
            return result;
        }
    }

    public class SRM_Allow
    {
        public const string C_FillChar = " ";
        /// <summary>
        /// 左填充还是又填充
        /// </summary>
        public const int C_leftorRigh = 0;
        public const int cGroupLen = 1;
        public PLCAddress address = new PLCAddress();
        public bool Allow = false;

        public bool WirteToPLC(CallDave.daveConnection _dVC)
        {
            var rec = _dVC.writeBits(CallDave.daveDB, address.db, address.start,1, new byte[]{1});
            if (rec != 0) return false;
            return true;
        }

        public bool LoadFromPLC(CallDave.daveConnection _dVC)
        {
            byte[] tempbytes = new byte[CON_Status.cGroupLen];
            var rec = _dVC.readBits(CallDave.daveDB, address.db, address.start, CON_Status.cGroupLen, tempbytes);
            if (rec != 0) return false;
            Allow = (tempbytes[0] & 1) == 1;
            return true;
        }
    }


    //堆垛机请求数据块映射类
    public class SRM_Request
    {
        /// <summary>
        /// 填充字符
        /// </summary>
        public const string C_FillChar = " ";
        /// <summary>
        /// 左填充还是又填充
        /// </summary>
        public const int C_leftorRigh = 0;

        public eStationType StnType = eStationType.AP;
        
        public PLCAddress address = new PLCAddress();
        public const int cGroupLen = 26;

        public byte[] b_Serial = new byte[2];//序号
        public byte[] b_Pattern = new byte[2]; //模式

        public byte[] b_FrStand = new byte[2]; //当前执行站
        public byte[] b_FrLine = new byte[2]; //当前执行列
        public byte[] b_FrGrid = new byte[2]; //当前执行格
        public byte[] b_FrTier = new byte[2]; //当前执行层

        public byte[] b_ToStand = new byte[2]; //当前执行站
        public byte[] b_ToLine = new byte[2]; //当前执行列
        public byte[] b_ToGrid = new byte[2]; //当前执行格
        public byte[] b_ToTier = new byte[2]; //当前执行层

        public byte[] b_execute_signal = new byte[1]; //执行信号
        public byte[] b_stop_signal = new byte[1]; //急停信号
        public byte[] b_order_sum = new byte[2];//总数
        //public byte[] b_index_code = new byte[2];//索引码后四位
        //public byte[] b_year = new byte[2];//年
        //public byte[] b_month = new byte[2];//月
        //public byte[] b_day = new byte[2];//日
        //public byte[] b_hour = new byte[2];//小时
        //public byte[] b_minute = new byte[2];//分
        //public byte[] b_second = new byte[2];//秒
        //public byte[] b_wtime = new byte[1];//更新时间
        public byte[] b_fault_reset = new byte[1];//故障复位
        public byte[] b_res_Voice = new byte[1];//维护型号
        //public byte[] b_hz1s = new byte[1];//1HZ
        //public byte[] b_idr = new byte[1];//ID收

        #region
        public UInt16 s_serial
        {
            get
            {
                byte[] tmpbyte = new byte[b_Serial.Length];
                Buffer.BlockCopy(b_Serial, 0, tmpbyte, 0, b_Serial.Length);
                Array.Reverse(tmpbyte);
                return BitConverter.ToUInt16(tmpbyte, 0);
            }
            set
            {
                byte[] tmpbyte = BitConverter.GetBytes(value);
                Array.Reverse(tmpbyte);
                b_Serial = tmpbyte;
            }
        }
        public UInt16 s_pattern
        {
            get
            {
                byte[] tmpbyte = new byte[b_Pattern.Length];
                Buffer.BlockCopy(b_Pattern, 0, tmpbyte, 0, b_Pattern.Length);
                Array.Reverse(tmpbyte);
                return BitConverter.ToUInt16(tmpbyte, 0);
            }
            set
            {
                byte[] tmpbyte = BitConverter.GetBytes(value);
                Array.Reverse(tmpbyte);
                b_Pattern = tmpbyte;
            }
        }
        public UInt16 s_FrStand
        {
            get
            {
                byte[] tmpbyte = new byte[b_FrStand.Length];
                Buffer.BlockCopy(b_FrStand, 0, tmpbyte, 0, b_FrStand.Length);
                Array.Reverse(tmpbyte);
                return BitConverter.ToUInt16(tmpbyte, 0);
            }
            set
            {
                byte[] tmpbyte = BitConverter.GetBytes(value);
                Array.Reverse(tmpbyte);
                b_FrStand = tmpbyte;
            }
        }
        public UInt16 s_FrLine
        {
            get
            {
                byte[] tmpbyte = new byte[b_FrLine.Length];
                Buffer.BlockCopy(b_FrLine, 0, tmpbyte, 0, b_FrLine.Length);
                Array.Reverse(tmpbyte);
                return BitConverter.ToUInt16(tmpbyte, 0);
            }
            set
            {
                byte[] tmpbyte = BitConverter.GetBytes(value);
                Array.Reverse(tmpbyte);
                b_FrLine = tmpbyte;
            }
        }
        public UInt16 s_FrGrid
        {
            get
            {
                byte[] tmpbyte = new byte[b_FrGrid.Length];
                Buffer.BlockCopy(b_FrGrid, 0, tmpbyte, 0, b_FrGrid.Length);
                Array.Reverse(tmpbyte);
                return BitConverter.ToUInt16(tmpbyte, 0);
            }
            set
            {
                byte[] tmpbyte = BitConverter.GetBytes(value);
                Array.Reverse(tmpbyte);
                b_FrGrid = tmpbyte;
            }
        }
        public UInt16 s_FrTier
        {
            get
            {
                byte[] tmpbyte = new byte[b_FrTier.Length];
                Buffer.BlockCopy(b_FrTier, 0, tmpbyte, 0, b_FrTier.Length);
                Array.Reverse(tmpbyte);
                return BitConverter.ToUInt16(tmpbyte, 0);
            }
            set
            {
                byte[] tmpbyte = BitConverter.GetBytes(value);
                Array.Reverse(tmpbyte);
                b_FrTier = tmpbyte;
            }
        }
        public UInt16 s_ToStand
        {
            get
            {
                byte[] tmpbyte = new byte[b_ToStand.Length];
                Buffer.BlockCopy(b_ToStand, 0, tmpbyte, 0, b_ToStand.Length);
                Array.Reverse(tmpbyte);
                return BitConverter.ToUInt16(tmpbyte, 0);
            }
            set
            {
                byte[] tmpbyte = BitConverter.GetBytes(value);
                Array.Reverse(tmpbyte);
                b_ToStand = tmpbyte;
            }
        }
        public UInt16 s_ToLine
        {
            get
            {
                byte[] tmpbyte = new byte[b_ToLine.Length];
                Buffer.BlockCopy(b_ToLine, 0, tmpbyte, 0, b_ToLine.Length);
                Array.Reverse(tmpbyte);
                return BitConverter.ToUInt16(tmpbyte, 0);
            }
            set
            {
                byte[] tmpbyte = BitConverter.GetBytes(value);
                Array.Reverse(tmpbyte);
                b_ToLine = tmpbyte;
            }
        }
        public UInt16 s_ToGrid
        {
            get
            {
                byte[] tmpbyte = new byte[b_ToGrid.Length];
                Buffer.BlockCopy(b_ToGrid, 0, tmpbyte, 0, b_ToGrid.Length);
                Array.Reverse(tmpbyte);
                return BitConverter.ToUInt16(tmpbyte, 0);
            }
            set
            {
                byte[] tmpbyte = BitConverter.GetBytes(value);
                Array.Reverse(tmpbyte);
                b_ToGrid = tmpbyte;
            }
        }
        public UInt16 s_ToTier
        {
            get
            {
                byte[] tmpbyte = new byte[b_ToTier.Length];
                Buffer.BlockCopy(b_ToTier, 0, tmpbyte, 0, b_ToTier.Length);
                Array.Reverse(tmpbyte);
                return BitConverter.ToUInt16(tmpbyte, 0);
            }
            set
            {
                byte[] tmpbyte = BitConverter.GetBytes(value);
                Array.Reverse(tmpbyte);
                b_ToTier = tmpbyte;
            }
        }
        public byte s_execute_signal { get { return b_execute_signal[0]; } set { b_execute_signal[0] = value; } }
        public byte s_stop_signal { get { return b_stop_signal[0]; } set { b_stop_signal[0] = value; } }
        public UInt16 s_order_sum
        {
            get
            {
                byte[] tmpbyte = new byte[b_order_sum.Length];
                Buffer.BlockCopy(b_order_sum, 0, tmpbyte, 0, b_order_sum.Length);
                Array.Reverse(tmpbyte);
                return BitConverter.ToUInt16(tmpbyte, 0);
            }
            set
            {
                byte[] tmpbyte = BitConverter.GetBytes(value);
                Array.Reverse(tmpbyte);
                b_order_sum = tmpbyte;
            }
        }
        //public UInt16 s_index_code
        //{
        //    get
        //    {
        //        byte[] tmpbyte = new byte[b_index_code.Length];
        //        Buffer.BlockCopy(b_index_code, 0, tmpbyte, 0, b_index_code.Length);
        //        Array.Reverse(tmpbyte);
        //        return BitConverter.ToUInt16(tmpbyte, 0);
        //    }
        //    set
        //    {
        //        byte[] tmpbyte = BitConverter.GetBytes(value);
        //        Array.Reverse(tmpbyte);
        //        b_index_code = tmpbyte;
        //    }
        //}
        //public UInt16 s_year
        //{
        //    get
        //    {
        //        byte[] tmpbyte = new byte[b_year.Length];
        //        Buffer.BlockCopy(b_year, 0, tmpbyte, 0, b_year.Length);
        //        Array.Reverse(tmpbyte);
        //        return BitConverter.ToUInt16(tmpbyte, 0);
        //    }
        //    set
        //    {
        //        byte[] tmpbyte = BitConverter.GetBytes(value);
        //        Array.Reverse(tmpbyte);
        //        b_year = tmpbyte;
        //    }
        //}
        //public UInt16 s_month
        //{
        //    get
        //    {
        //        byte[] tmpbyte = new byte[b_month.Length];
        //        Buffer.BlockCopy(b_month, 0, tmpbyte, 0, b_month.Length);
        //        Array.Reverse(tmpbyte);
        //        return BitConverter.ToUInt16(tmpbyte, 0);
        //    }
        //    set
        //    {
        //        byte[] tmpbyte = BitConverter.GetBytes(value);
        //        Array.Reverse(tmpbyte);
        //        b_month = tmpbyte;
        //    }
        //}
        //public UInt16 s_day
        //{
        //    get
        //    {
        //        byte[] tmpbyte = new byte[b_day.Length];
        //        Buffer.BlockCopy(b_day, 0, tmpbyte, 0, b_day.Length);
        //        Array.Reverse(tmpbyte);
        //        return BitConverter.ToUInt16(tmpbyte, 0);
        //    }
        //    set
        //    {
        //        byte[] tmpbyte = BitConverter.GetBytes(value);
        //        Array.Reverse(tmpbyte);
        //        b_day = tmpbyte;
        //    }
        //}
        //public UInt16 s_hour
        //{
        //    get
        //    {
        //        byte[] tmpbyte = new byte[b_hour.Length];
        //        Buffer.BlockCopy(b_hour, 0, tmpbyte, 0, b_hour.Length);
        //        Array.Reverse(tmpbyte);
        //        return BitConverter.ToUInt16(tmpbyte, 0);
        //    }
        //    set
        //    {
        //        byte[] tmpbyte = BitConverter.GetBytes(value);
        //        Array.Reverse(tmpbyte);
        //        b_hour = tmpbyte;
        //    }
        //}
        //public UInt16 s_minute
        //{
        //    get
        //    {
        //        byte[] tmpbyte = new byte[b_minute.Length];
        //        Buffer.BlockCopy(b_minute, 0, tmpbyte, 0, b_minute.Length);
        //        Array.Reverse(tmpbyte);
        //        return BitConverter.ToUInt16(tmpbyte, 0);
        //    }
        //    set
        //    {
        //        byte[] tmpbyte = BitConverter.GetBytes(value);
        //        Array.Reverse(tmpbyte);
        //        b_minute = tmpbyte;
        //    }
        //}
        //public UInt16 s_second
        //{
        //    get
        //    {
        //        byte[] tmpbyte = new byte[b_second.Length];
        //        Buffer.BlockCopy(b_second, 0, tmpbyte, 0, b_second.Length);
        //        Array.Reverse(tmpbyte);
        //        return BitConverter.ToUInt16(tmpbyte, 0);
        //    }
        //    set
        //    {
        //        byte[] tmpbyte = BitConverter.GetBytes(value);
        //        Array.Reverse(tmpbyte);
        //        b_second = tmpbyte;
        //    }
        //}
        //public byte s_wtime { get { return b_wtime[0]; } set { b_wtime[0] = value; } }
        public byte s_fault_reset { get { return b_fault_reset[0]; } set { b_fault_reset[0] = value; } }
        public byte s_res_Voice { get { return b_res_Voice[0]; } set { b_res_Voice[0] = value; } }
        //public byte s_hz1s { get { return b_hz1s[0]; } set { b_hz1s[0] = value; } }
        //public byte s_idr { get { return b_idr[0]; } set { b_idr[0] = value; } }
        #endregion

        public SRM_Request()
        {

        }

        public SRM_Request(byte[] bytes)
        {
            this.LoadByte(bytes);
        }

        #region 根据Byte数据填充类
        public void LoadByte(byte[] bytes)
        {
            int copy_Pionter = 0;
            try
            {
                Buffer.BlockCopy(bytes, copy_Pionter += 0, b_Serial, 0, b_Serial.Length);
                Buffer.BlockCopy(bytes, copy_Pionter += b_Serial.Length, b_Pattern, 0, b_Pattern.Length);
                Buffer.BlockCopy(bytes, copy_Pionter += b_Pattern.Length, b_FrStand, 0, b_FrStand.Length);
                Buffer.BlockCopy(bytes, copy_Pionter += b_FrStand.Length, b_FrLine, 0, b_FrLine.Length);
                Buffer.BlockCopy(bytes, copy_Pionter += b_FrLine.Length, b_FrGrid, 0, b_FrGrid.Length);
                Buffer.BlockCopy(bytes, copy_Pionter += b_FrGrid.Length, b_FrTier, 0, b_FrTier.Length);
                Buffer.BlockCopy(bytes, copy_Pionter += b_FrTier.Length, b_ToStand, 0, b_ToStand.Length);
                Buffer.BlockCopy(bytes, copy_Pionter += b_ToStand.Length, b_ToLine, 0, b_ToLine.Length);
                Buffer.BlockCopy(bytes, copy_Pionter += b_ToLine.Length, b_ToGrid, 0, b_ToGrid.Length);
                Buffer.BlockCopy(bytes, copy_Pionter += b_ToGrid.Length, b_ToTier, 0, b_ToTier.Length);
                Buffer.BlockCopy(bytes, copy_Pionter += b_ToTier.Length, b_execute_signal, 0, b_execute_signal.Length);
                Buffer.BlockCopy(bytes, copy_Pionter += b_execute_signal.Length, b_stop_signal, 0, b_stop_signal.Length);
                Buffer.BlockCopy(bytes, copy_Pionter += b_stop_signal.Length, b_order_sum, 0, b_order_sum.Length);
                //Buffer.BlockCopy(bytes, copy_Pionter += b_order_sum.Length, b_index_code, 0, b_index_code.Length);
                //Buffer.BlockCopy(bytes, copy_Pionter += b_index_code.Length, b_year, 0, b_year.Length);
                //Buffer.BlockCopy(bytes, copy_Pionter += b_year.Length, b_month, 0, b_month.Length);
                //Buffer.BlockCopy(bytes, copy_Pionter += b_month.Length, b_day, 0, b_day.Length);
                //Buffer.BlockCopy(bytes, copy_Pionter += b_day.Length, b_hour, 0, b_hour.Length);
                //Buffer.BlockCopy(bytes, copy_Pionter += b_hour.Length, b_minute, 0, b_minute.Length);
                //Buffer.BlockCopy(bytes, copy_Pionter += b_minute.Length, b_second, 0, b_second.Length);
                //Buffer.BlockCopy(bytes, copy_Pionter += b_second.Length, b_wtime, 0, b_wtime.Length);
                Buffer.BlockCopy(bytes, copy_Pionter += b_order_sum.Length, b_fault_reset, 0, b_fault_reset.Length);
                Buffer.BlockCopy(bytes, copy_Pionter += b_fault_reset.Length, b_res_Voice, 0, b_res_Voice.Length);
                //Buffer.BlockCopy(bytes, copy_Pionter += b_res_Voice.Length, b_hz1s, 0, b_hz1s.Length);
                //Buffer.BlockCopy(bytes, copy_Pionter += b_hz1s.Length, b_idr, 0, b_idr.Length);
            }
            catch(Exception e)
            {
                throw new ApplicationException(string.Format("ErrorMSG:{0},Pointer:{1},Bytes:{2}", e.Message, copy_Pionter, ByteHelper.ToMessage(bytes, bytes.Length)));
            }
        }
        #endregion

        #region 转化为Byte[] 数组
        public byte[] ToBytes()
        {
            int copy_Pionter = 0;

            byte[] result = new byte[cGroupLen];
            Buffer.BlockCopy(b_Serial, 0, result, copy_Pionter += 0, b_Serial.Length);
            Buffer.BlockCopy(b_Pattern, 0, result, copy_Pionter += b_Serial.Length, b_Pattern.Length);
            Buffer.BlockCopy(b_FrStand, 0, result, copy_Pionter += b_Pattern.Length, b_FrStand.Length);
            Buffer.BlockCopy(b_FrLine, 0, result, copy_Pionter += b_FrStand.Length, b_FrLine.Length);
            Buffer.BlockCopy(b_FrGrid, 0, result, copy_Pionter += b_FrLine.Length, b_FrGrid.Length);
            Buffer.BlockCopy(b_FrTier, 0, result, copy_Pionter += b_FrGrid.Length, b_FrTier.Length);
            Buffer.BlockCopy(b_ToStand, 0, result, copy_Pionter += b_FrTier.Length, b_ToStand.Length);
            Buffer.BlockCopy(b_ToLine, 0, result, copy_Pionter += b_ToStand.Length, b_ToLine.Length);
            Buffer.BlockCopy(b_ToGrid, 0, result, copy_Pionter += b_ToLine.Length, b_ToGrid.Length);
            Buffer.BlockCopy(b_ToTier, 0, result, copy_Pionter += b_ToGrid.Length, b_ToTier.Length);
            Buffer.BlockCopy(b_execute_signal, 0, result, copy_Pionter += b_ToTier.Length, b_execute_signal.Length);
            Buffer.BlockCopy(b_stop_signal, 0, result, copy_Pionter += b_execute_signal.Length, b_stop_signal.Length);
            Buffer.BlockCopy(b_order_sum, 0, result, copy_Pionter += b_stop_signal.Length, b_order_sum.Length);
            //Buffer.BlockCopy(b_index_code, 0, result, copy_Pionter += b_order_sum.Length, b_index_code.Length);
            //Buffer.BlockCopy(b_year, 0, result, copy_Pionter += b_index_code.Length, b_year.Length);
            //Buffer.BlockCopy(b_month, 0, result, copy_Pionter += b_year.Length, b_month.Length);
            //Buffer.BlockCopy(b_day, 0, result, copy_Pionter += b_month.Length, b_day.Length);
            //Buffer.BlockCopy(b_hour, 0, result, copy_Pionter += b_day.Length, b_hour.Length);
            //Buffer.BlockCopy(b_minute, 0, result, copy_Pionter += b_hour.Length, b_minute.Length);
            //Buffer.BlockCopy(b_second, 0, result, copy_Pionter += b_minute.Length, b_second.Length);
            //Buffer.BlockCopy(b_wtime, 0, result, copy_Pionter += b_second.Length, b_wtime.Length);
            Buffer.BlockCopy(b_fault_reset, 0, result, copy_Pionter += b_order_sum.Length, b_fault_reset.Length);
            Buffer.BlockCopy(b_res_Voice, 0, result, copy_Pionter += b_fault_reset.Length, b_res_Voice.Length);
            //Buffer.BlockCopy(b_hz1s, 0, result, copy_Pionter += b_res_Voice.Length, b_hz1s.Length);
            //Buffer.BlockCopy(b_idr, 0, result, copy_Pionter += b_hz1s.Length, b_idr.Length);

            return result;
        }
        #endregion

        public bool LoadFromPLC(CallDave.daveConnection _dVC)
        {
            byte[] tempBytes = new byte[SRM_Request.cGroupLen];
            var rec = _dVC.readBytes(CallDave.daveDB, address.db, address.start, SRM_Request.cGroupLen, tempBytes);
            if (rec != 0) return false;
            this.LoadByte(tempBytes);
            return true;
        }

        public bool SetOverRead(CallDave.daveConnection _dVc)
        {
            byte[] tempbytes = this.ToBytes();
            //var rec = _dVc.writeBytes(CallDave.daveDB, address.db, address.start + (SRM_Request.cGroupLen), 1, new byte[] { 0 });
            var rec = _dVc.writeBytes(CallDave.daveDB, address.db, 27, 2, new byte[] { 0,0 });
            return rec == 0;
        }
        public bool WirteToPLC(CallDave.daveConnection _dVC)
        {
            byte[] tempbytes = this.ToBytes();
            tempbytes[SRM_Request.cGroupLen - 1] = 0; //设置和为未读
            var rec = _dVC.writeBytes(CallDave.daveDB, address.db, 200, SRM_Request.cGroupLen, tempbytes);
            if (rec != 0) return false;
            return true;
        }

    }
    //WCS相应请求数据块映射类
    public class SRM_Respone
    {
        /// <summary>
        /// 填充字符
        /// </summary>
        public const string C_FillChar = " ";
        /// <summary>
        /// 左填充还是又填充
        /// </summary>
        public const int C_leftorRigh = 0;

        public PLCAddress address = new PLCAddress();
        public const int cGroupLen = 20;

        public byte[] b_runMaintenance = new byte[2]; //维护/运行
        public byte[] b_finish = new byte[2]; //完成信号
        public byte[] b_FrGrid = new byte[2];//格位
        public byte[] b_FrTier = new byte[2]; //层位
        public byte[] b_arm = new byte[2];//叉臂位置
        public byte[] b_step1 = new byte[2];//step1
        public byte[] b_step2 = new byte[2];//step2
        public byte[] b_step3 = new byte[2];//step3
        public byte[] b_fault = new byte[2];//故障信号
        public byte[] b_run = new byte[2];//运行信号

        //public byte[] b_runMaintenance = new byte[1]; //运行/维护
        //public byte[] b_leftDetection = new byte[1]; //左有物检测
        //public byte[] b_rightDetection = new byte[1]; //右有物检测
        //public byte[] b_leftStretch = new byte[1]; //左伸叉到位
        //public byte[] b_rightStretch = new byte[1];//右伸叉到位

        //public byte[] b_goods = new byte[1]; //堆垛机有物
        //public byte[] b_centre = new byte[1]; //叉臂中间位
        ////public byte[] b_finish = new byte[2]; //完成信号
        ////public byte[] b_fault = new byte[1];//故障信号
        ////public byte[] b_run = new byte[1];//运行信号
        //public byte[] b_1hz = new byte[1];

        public UInt16 s_runMaintenance
        {
            get
            {
                byte[] tmpbyte = new byte[b_runMaintenance.Length];
                Buffer.BlockCopy(b_runMaintenance, 0, tmpbyte, 0, b_runMaintenance.Length);
                Array.Reverse(tmpbyte);
                return BitConverter.ToUInt16(tmpbyte, 0);
            }
            set
            {
                byte[] tmpbyte = BitConverter.GetBytes(value);
                Array.Reverse(tmpbyte);
                b_runMaintenance = tmpbyte;
            }
        }
        public UInt16 s_finish
        {
            get
            {
                byte[] tmpbyte = new byte[b_finish.Length];
                Buffer.BlockCopy(b_finish, 0, tmpbyte, 0, b_finish.Length);
                Array.Reverse(tmpbyte);
                return BitConverter.ToUInt16(tmpbyte, 0);
            }
            set
            {
                byte[] tmpbyte = BitConverter.GetBytes(value);
                Array.Reverse(tmpbyte);
                b_finish = tmpbyte;
            }
        }
        public UInt16 s_FrGrid
        {
            get
            {
                byte[] tmpbyte = new byte[b_FrGrid.Length];
                Buffer.BlockCopy(b_FrGrid, 0, tmpbyte, 0, b_FrGrid.Length);
                Array.Reverse(tmpbyte);
                return BitConverter.ToUInt16(tmpbyte, 0);
            }
            set
            {
                byte[] tmpbyte = BitConverter.GetBytes(value);
                Array.Reverse(tmpbyte);
                b_FrGrid = tmpbyte;
            }
        }
        public UInt16 s_FrTier
        {
            get
            {
                byte[] tmpbyte = new byte[b_FrTier.Length];
                Buffer.BlockCopy(b_FrTier, 0, tmpbyte, 0, b_FrTier.Length);
                Array.Reverse(tmpbyte);
                return BitConverter.ToUInt16(tmpbyte, 0);
            }
            set
            {
                byte[] tmpbyte = BitConverter.GetBytes(value);
                Array.Reverse(tmpbyte);
                b_FrTier = tmpbyte;
            }
        }
        public UInt16 s_arm
        {
            get
            {
                byte[] tmpbyte = new byte[b_arm.Length];
                Buffer.BlockCopy(b_arm, 0, tmpbyte, 0, b_arm.Length);
                Array.Reverse(tmpbyte);
                return BitConverter.ToUInt16(tmpbyte, 0);
            }
            set
            {
                byte[] tmpbyte = BitConverter.GetBytes(value);
                Array.Reverse(tmpbyte);
                b_arm = tmpbyte;
            }
        }
        public UInt16 s_step1
        {
            get
            {
                byte[] tmpbyte = new byte[b_step1.Length];
                Buffer.BlockCopy(b_step1, 0, tmpbyte, 0, b_step1.Length);
                Array.Reverse(tmpbyte);
                return BitConverter.ToUInt16(tmpbyte, 0);
            }
            set
            {
                byte[] tmpbyte = BitConverter.GetBytes(value);
                Array.Reverse(tmpbyte);
                b_step1 = tmpbyte;
            }
        }
        public UInt16 s_step2
        {
            get
            {
                byte[] tmpbyte = new byte[b_step2.Length];
                Buffer.BlockCopy(b_step2, 0, tmpbyte, 0, b_step2.Length);
                Array.Reverse(tmpbyte);
                return BitConverter.ToUInt16(tmpbyte, 0);
            }
            set
            {
                byte[] tmpbyte = BitConverter.GetBytes(value);
                Array.Reverse(tmpbyte);
                b_step2 = tmpbyte;
            }
        }
        public UInt16 s_step3
        {
            get
            {
                byte[] tmpbyte = new byte[b_step3.Length];
                Buffer.BlockCopy(b_step3, 0, tmpbyte, 0, b_step3.Length);
                Array.Reverse(tmpbyte);
                return BitConverter.ToUInt16(tmpbyte, 0);
            }
            set
            {
                byte[] tmpbyte = BitConverter.GetBytes(value);
                Array.Reverse(tmpbyte);
                b_step3 = tmpbyte;
            }
        }
        public UInt16 s_fault
        {
            get
            {
                byte[] tmpbyte = new byte[b_fault.Length];
                Buffer.BlockCopy(b_fault, 0, tmpbyte, 0, b_fault.Length);
                Array.Reverse(tmpbyte);
                return BitConverter.ToUInt16(tmpbyte, 0);
            }
            set
            {
                byte[] tmpbyte = BitConverter.GetBytes(value);
                Array.Reverse(tmpbyte);
                b_fault = tmpbyte;
            }
        }
        public UInt16 s_run
        {
            get
            {
                byte[] tmpbyte = new byte[b_run.Length];
                Buffer.BlockCopy(b_run, 0, tmpbyte, 0, b_run.Length);
                Array.Reverse(tmpbyte);
                return BitConverter.ToUInt16(tmpbyte, 0);
            }
            set
            {
                byte[] tmpbyte = BitConverter.GetBytes(value);
                Array.Reverse(tmpbyte);
                b_run = tmpbyte;
            }
        }

        public byte[] ToBytes()
        {
            int copy_Pionter = 0;

            byte[] result = new byte[cGroupLen];
            Buffer.BlockCopy(b_runMaintenance, 0, result, copy_Pionter += 0, b_runMaintenance.Length);
            Buffer.BlockCopy(b_finish, 0, result, copy_Pionter += b_runMaintenance.Length, b_finish.Length);
            Buffer.BlockCopy(b_FrGrid, 0, result, copy_Pionter += b_finish.Length, b_FrGrid.Length);
            Buffer.BlockCopy(b_FrTier, 0, result, copy_Pionter += b_FrGrid.Length, b_FrTier.Length);
            Buffer.BlockCopy(b_arm, 0, result, copy_Pionter += b_FrTier.Length, b_arm.Length);
            Buffer.BlockCopy(b_step1, 0, result, copy_Pionter += b_arm.Length, b_step1.Length);
            Buffer.BlockCopy(b_step2, 0, result, copy_Pionter += b_step1.Length, b_step2.Length);
            Buffer.BlockCopy(b_step3, 0, result, copy_Pionter += b_step2.Length, b_step3.Length);
            Buffer.BlockCopy(b_fault, 0, result, copy_Pionter += b_step3.Length, b_fault.Length);
            Buffer.BlockCopy(b_run, 0, result, copy_Pionter += b_fault.Length, b_run.Length);

            return result;
        }

        private void LoadBytes(byte[] bytes)
        {
            int copy_Pionter = 0;
            try
            {
                Buffer.BlockCopy(bytes, copy_Pionter += 0, b_runMaintenance, 0, b_runMaintenance.Length);
                Buffer.BlockCopy(bytes, copy_Pionter += b_runMaintenance.Length, b_finish, 0, b_finish.Length);
                Buffer.BlockCopy(bytes, copy_Pionter += b_finish.Length, b_FrGrid, 0, b_FrGrid.Length);
                Buffer.BlockCopy(bytes, copy_Pionter += b_FrGrid.Length, b_FrTier, 0, b_FrTier.Length);
                Buffer.BlockCopy(bytes, copy_Pionter += b_FrTier.Length, b_arm, 0, b_arm.Length);
                Buffer.BlockCopy(bytes, copy_Pionter += b_arm.Length, b_step1, 0, b_step1.Length);
                Buffer.BlockCopy(bytes, copy_Pionter += b_step1.Length, b_step2, 0, b_step2.Length);
                Buffer.BlockCopy(bytes, copy_Pionter += b_step2.Length, b_step3, 0, b_step3.Length);
                Buffer.BlockCopy(bytes, copy_Pionter += b_step3.Length, b_fault, 0, b_fault.Length);
                Buffer.BlockCopy(bytes, copy_Pionter += b_fault.Length, b_run, 0, b_run.Length);
            }
            catch (Exception ex)
            {
                throw new ApplicationException(string.Format("ErrorMSG:{0},Pointer:{1},Bytes:{2}", ex.Message, copy_Pionter, ByteHelper.ToMessage(bytes, bytes.Length)));
            }
        }

        public bool WirteToPLC(CallDave.daveConnection _dVC)
        {
            byte[] tempbytes = this.ToBytes();
            tempbytes[SRM_Respone.cGroupLen-1] = 0; //设置和为未读
            var rec = _dVC.writeBytes(CallDave.daveDB, address.db, 3, SRM_Respone.cGroupLen, new byte[] { 0,1});
            if (rec != 0) return false;
            return true;
        }

        public bool CheckOverRead(CallDave.daveConnection _dVC)
        {
            byte[] flag_OverRead = new byte[] { 0, 0 };
            var res = _dVC.readBytes(CallDave.daveDB, 6, 18, 2, flag_OverRead);
            return res == 0;
        }

        public bool LoadFromPLC(CallDave.daveConnection _dVC)
        {
            byte[] tempBytes = new byte[SRM_Respone.cGroupLen];
            //byte[] bytes = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            var rec = _dVC.readBytes(CallDave.daveDB, 6, 0, 20, tempBytes);
            //var rec = _dVC.readBytes(CallDave.daveDB, 6, address.start, 30, tempBytes);
            if (rec != 0) return false;
            this.LoadBytes(tempBytes);
            return true;
        }
    }
    //堆垛机状态类
    public class SRM_StatusClass
    {
        /// <summary>
        /// 填充字符
        /// </summary>
        public const string C_FillChar = " ";
        /// <summary>
        /// 左填充还是又填充
        /// </summary>
        public const int C_leftorRigh = 0;

        public eStationType StnType = eStationType.EP;

        public PLCAddress address = new PLCAddress();
        public const int cGroupLen = 30;
        public byte[] b_Grid = new byte[2]; //当前格位
        public byte[] b_Layer = new byte[2]; //当前层位
        public byte[] b_ExecutionStep = new byte[2]; //执行步骤
        public byte[] b_Pattern = new byte[1]; //模式
        public byte[] b_Status = new byte[1]; //状态
        public byte[] b_ID = new byte[2]; //当前执行ID

        public byte[] b_FrPattern = new byte[2]; //当前执行模式
        public byte[] b_FrStand = new byte[2]; //当前执行站
        public byte[] b_FrLine = new byte[2]; //当前执行列
        public byte[] b_FrGrid = new byte[2]; //当前执行格
        public byte[] b_FrTier = new byte[2]; //当前执行层

        public byte[] b_ToStand = new byte[2]; //当前执行站
        public byte[] b_ToLine = new byte[2]; //当前执行列
        public byte[] b_ToGrid = new byte[2]; //当前执行格
        public byte[] b_ToTier = new byte[2]; //当前执行层

        public byte[] b_Finish = new byte[1]; //完成信号
        public byte[] b_Cancel = new byte[1]; //指令取消

        public UInt16 s_Grid
        {
            get
            {
                byte[] tmpbyte = new byte[b_Grid.Length];
                Buffer.BlockCopy(b_Grid, 0, tmpbyte, 0, b_Grid.Length);
                Array.Reverse(tmpbyte);
                return BitConverter.ToUInt16(tmpbyte, 0);
            }
            set
            {
                byte[] tmpbyte = BitConverter.GetBytes(value);
                Array.Reverse(tmpbyte);
                b_Grid = tmpbyte;
            }
        }
        public UInt16 s_Layer
        {
            get
            {
                byte[] tmpbyte = new byte[b_Layer.Length];
                Buffer.BlockCopy(b_Layer, 0, tmpbyte, 0, b_Layer.Length);
                Array.Reverse(tmpbyte);
                return BitConverter.ToUInt16(tmpbyte, 0);
            }
            set
            {
                byte[] tmpbyte = BitConverter.GetBytes(value);
                Array.Reverse(tmpbyte);
                b_Layer = tmpbyte;
            }
        }
        public UInt16 s_ExecutionStep
        {
            get
            {
                byte[] tmpbyte = new byte[b_ExecutionStep.Length];
                Buffer.BlockCopy(b_ExecutionStep, 0, tmpbyte, 0, b_ExecutionStep.Length);
                Array.Reverse(tmpbyte);
                return BitConverter.ToUInt16(tmpbyte, 0);
            }
            set
            {
                byte[] tmpbyte = BitConverter.GetBytes(value);
                Array.Reverse(tmpbyte);
                b_ExecutionStep = tmpbyte;
            }
        }
        public byte s_Pattern { get { return b_Pattern[0]; } set { b_Pattern[0] = value; } }
        public byte s_Status { get { return b_Status[0]; } set { b_Status[0] = value; } }
        public UInt16 s_ID
        {
            get
            {
                byte[] tmpbyte = new byte[b_ID.Length];
                Buffer.BlockCopy(b_ID, 0, tmpbyte, 0, b_ID.Length);
                Array.Reverse(tmpbyte);
                return BitConverter.ToUInt16(tmpbyte, 0);
            }
            set
            {
                byte[] tmpbyte = BitConverter.GetBytes(value);
                Array.Reverse(tmpbyte);
                b_ID = tmpbyte;
            }
        }
        public UInt16 s_FrPattern
        {
            get
            {
                byte[] tmpbyte = new byte[b_FrPattern.Length];
                Buffer.BlockCopy(b_FrPattern, 0, tmpbyte, 0, b_FrPattern.Length);
                Array.Reverse(tmpbyte);
                return BitConverter.ToUInt16(tmpbyte, 0);
            }
            set
            {
                byte[] tmpbyte = BitConverter.GetBytes(value);
                Array.Reverse(tmpbyte);
                b_FrPattern = tmpbyte;
            }
        }
        public UInt16 s_FrStand
        {
            get
            {
                byte[] tmpbyte = new byte[b_FrStand.Length];
                Buffer.BlockCopy(b_FrStand, 0, tmpbyte, 0, b_FrStand.Length);
                Array.Reverse(tmpbyte);
                return BitConverter.ToUInt16(tmpbyte, 0);
            }
            set
            {
                byte[] tmpbyte = BitConverter.GetBytes(value);
                Array.Reverse(tmpbyte);
                b_FrStand = tmpbyte;
            }
        }
        public UInt16 s_FrLine
        {
            get
            {
                byte[] tmpbyte = new byte[b_FrLine.Length];
                Buffer.BlockCopy(b_FrLine, 0, tmpbyte, 0, b_FrLine.Length);
                Array.Reverse(tmpbyte);
                return BitConverter.ToUInt16(tmpbyte, 0);
            }
            set
            {
                byte[] tmpbyte = BitConverter.GetBytes(value);
                Array.Reverse(tmpbyte);
                b_FrLine = tmpbyte;
            }
        }
        public UInt16 s_FrGrid
        {
            get
            {
                byte[] tmpbyte = new byte[b_FrGrid.Length];
                Buffer.BlockCopy(b_FrGrid, 0, tmpbyte, 0, b_FrGrid.Length);
                Array.Reverse(tmpbyte);
                return BitConverter.ToUInt16(tmpbyte, 0);
            }
            set
            {
                byte[] tmpbyte = BitConverter.GetBytes(value);
                Array.Reverse(tmpbyte);
                b_FrGrid = tmpbyte;
            }
        }
        public UInt16 s_FrTier
        {
            get
            {
                byte[] tmpbyte = new byte[b_FrTier.Length];
                Buffer.BlockCopy(b_FrTier, 0, tmpbyte, 0, b_FrTier.Length);
                Array.Reverse(tmpbyte);
                return BitConverter.ToUInt16(tmpbyte, 0);
            }
            set
            {
                byte[] tmpbyte = BitConverter.GetBytes(value);
                Array.Reverse(tmpbyte);
                b_FrTier = tmpbyte;
            }
        }
        public UInt16 s_ToStand
        {
            get
            {
                byte[] tmpbyte = new byte[b_ToStand.Length];
                Buffer.BlockCopy(b_ToStand, 0, tmpbyte, 0, b_ToStand.Length);
                Array.Reverse(tmpbyte);
                return BitConverter.ToUInt16(tmpbyte, 0);
            }
            set
            {
                byte[] tmpbyte = BitConverter.GetBytes(value);
                Array.Reverse(tmpbyte);
                b_ToStand = tmpbyte;
            }
        }
        public UInt16 s_ToLine
        {
            get
            {
                byte[] tmpbyte = new byte[b_ToLine.Length];
                Buffer.BlockCopy(b_ToLine, 0, tmpbyte, 0, b_ToLine.Length);
                Array.Reverse(tmpbyte);
                return BitConverter.ToUInt16(tmpbyte, 0);
            }
            set
            {
                byte[] tmpbyte = BitConverter.GetBytes(value);
                Array.Reverse(tmpbyte);
                b_ToLine = tmpbyte;
            }
        }
        public UInt16 s_ToGrid
        {
            get
            {
                byte[] tmpbyte = new byte[b_ToGrid.Length];
                Buffer.BlockCopy(b_ToGrid, 0, tmpbyte, 0, b_ToGrid.Length);
                Array.Reverse(tmpbyte);
                return BitConverter.ToUInt16(tmpbyte, 0);
            }
            set
            {
                byte[] tmpbyte = BitConverter.GetBytes(value);
                Array.Reverse(tmpbyte);
                b_ToGrid = tmpbyte;
            }
        }
        public UInt16 s_ToTier
        {
            get
            {
                byte[] tmpbyte = new byte[b_ToTier.Length];
                Buffer.BlockCopy(b_ToTier, 0, tmpbyte, 0, b_ToTier.Length);
                Array.Reverse(tmpbyte);
                return BitConverter.ToUInt16(tmpbyte, 0);
            }
            set
            {
                byte[] tmpbyte = BitConverter.GetBytes(value);
                Array.Reverse(tmpbyte);
                b_ToTier = tmpbyte;
            }
        }

        public byte s_Finish { get { return b_Finish[0]; } set { b_Finish[0] = value; } }
        public byte s_Cancel { get { return b_Cancel[0]; } set { b_Cancel[0] = value; } }

        public void Loadbyte(byte[] bytes)
        {
            int copy_Pionter = 0;
            try
            {
                Buffer.BlockCopy(bytes, copy_Pionter += 0, b_Grid, 0, b_Grid.Length);
                Buffer.BlockCopy(bytes, copy_Pionter += b_Grid.Length, b_Layer, 0, b_Layer.Length);
                Buffer.BlockCopy(bytes, copy_Pionter += b_Layer.Length, b_ExecutionStep, 0, b_ExecutionStep.Length);
                Buffer.BlockCopy(bytes, copy_Pionter += b_ExecutionStep.Length, b_Pattern, 0, b_Pattern.Length);
                Buffer.BlockCopy(bytes, copy_Pionter += b_Pattern.Length, b_Status, 0, b_Status.Length);
                Buffer.BlockCopy(bytes, copy_Pionter += b_Status.Length, b_ID, 0, b_ID.Length);
                Buffer.BlockCopy(bytes, copy_Pionter += b_ID.Length, b_FrPattern, 0, b_FrPattern.Length);
                Buffer.BlockCopy(bytes, copy_Pionter += b_FrPattern.Length, b_FrStand, 0, b_FrStand.Length);
                Buffer.BlockCopy(bytes, copy_Pionter += b_FrStand.Length, b_FrLine, 0, b_FrLine.Length);
                Buffer.BlockCopy(bytes, copy_Pionter += b_FrLine.Length, b_FrGrid, 0, b_FrGrid.Length);
                Buffer.BlockCopy(bytes, copy_Pionter += b_FrGrid.Length, b_FrTier, 0, b_FrTier.Length);
                Buffer.BlockCopy(bytes, copy_Pionter += b_FrTier.Length, b_ToStand, 0, b_ToStand.Length);
                Buffer.BlockCopy(bytes, copy_Pionter += b_ToStand.Length, b_ToLine, 0, b_ToLine.Length);
                Buffer.BlockCopy(bytes, copy_Pionter += b_ToLine.Length, b_ToGrid, 0, b_ToGrid.Length);
                Buffer.BlockCopy(bytes, copy_Pionter += b_ToGrid.Length, b_ToTier, 0, b_ToTier.Length);
                Buffer.BlockCopy(bytes, copy_Pionter += b_ToTier.Length, b_Finish, 0, b_Finish.Length);
                Buffer.BlockCopy(bytes, copy_Pionter += b_Finish.Length, b_Cancel, 0, b_Cancel.Length);
            }
            catch (Exception e)
            {
                throw new ApplicationException(string.Format("ErrorMSG:{0},Pointer:{1},Bytes:{2}", e.Message, copy_Pionter, ByteHelper.ToMessage(bytes, bytes.Length)));
            }
        }

        public bool LoadFromPLC(CallDave.daveConnection _dVC)
        {
            byte[] tempBytes = new byte[SRM_StatusClass.cGroupLen];
            var rec = _dVC.readBytes(CallDave.daveDB, address.db, address.start, SRM_StatusClass.cGroupLen, tempBytes);
            if (rec != 0) return false;
            this.Loadbyte(tempBytes);
            return true;
        }

        public byte[] Tobytes()
        {
            int copy_Pionter = 0;

            byte[] result = new byte[cGroupLen];
            Buffer.BlockCopy(b_Grid, 0, result, copy_Pionter += 0, b_Grid.Length);
            Buffer.BlockCopy(b_Layer, 0, result, copy_Pionter += b_Grid.Length, b_Layer.Length);
            Buffer.BlockCopy(b_ExecutionStep, 0, result, copy_Pionter += b_Layer.Length, b_ExecutionStep.Length);
            Buffer.BlockCopy(b_Pattern, 0, result, copy_Pionter += b_ExecutionStep.Length, b_Pattern.Length);
            Buffer.BlockCopy(b_Status, 0, result, copy_Pionter += b_Pattern.Length, b_Status.Length);
            Buffer.BlockCopy(b_ID, 0, result, copy_Pionter += b_Status.Length, b_ID.Length);
            Buffer.BlockCopy(b_FrPattern, 0, result, copy_Pionter += b_ID.Length, b_FrPattern.Length);
            Buffer.BlockCopy(b_FrStand, 0, result, copy_Pionter += b_FrPattern.Length, b_FrStand.Length);
            Buffer.BlockCopy(b_FrLine, 0, result, copy_Pionter += b_FrStand.Length, b_FrLine.Length);
            Buffer.BlockCopy(b_FrGrid, 0, result, copy_Pionter += b_FrLine.Length, b_FrGrid.Length);
            Buffer.BlockCopy(b_FrTier, 0, result, copy_Pionter += b_FrGrid.Length, b_FrTier.Length);
            Buffer.BlockCopy(b_ToStand, 0, result, copy_Pionter += b_FrTier.Length, b_ToStand.Length);
            Buffer.BlockCopy(b_ToLine, 0, result, copy_Pionter += b_ToStand.Length, b_ToLine.Length);
            Buffer.BlockCopy(b_ToGrid, 0, result, copy_Pionter += b_ToLine.Length, b_ToGrid.Length);
            Buffer.BlockCopy(b_ToTier, 0, result, copy_Pionter += b_ToGrid.Length, b_ToTier.Length);
            Buffer.BlockCopy(b_Finish, 0, result, copy_Pionter += b_ToTier.Length, b_Finish.Length);
            Buffer.BlockCopy(b_Cancel, 0, result, copy_Pionter += b_Finish.Length, b_Cancel.Length);

            return result;
        }

        public override string ToString()
        {
            return string.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|{10}|{11}|{12}|{13}|{14}|{15}|{16}|{17}", s_Grid, s_Layer, s_ExecutionStep, s_Pattern, s_Status, s_ID, s_FrPattern, s_FrStand, s_FrLine, s_FrGrid, s_FrTier, s_ToStand, s_ToLine, s_ToGrid, s_ToTier, s_Finish, s_Cancel);
        }
    }

    //站台状态
    public class SRM_PlatformStatus
    {
        public const string C_FillChar = " ";
        public const int C_leftorRigh = 0;

        public eStationType StnType = eStationType.AP;

        public PLCAddress address = new PLCAddress();

        public const int cGroupLen = 6;
        public byte[] b_scram1 = new byte[1];//站台急停
        public byte[] b_sure1 = new byte[1];//站台确定
        public byte[] b_something1 = new byte[1];//站台有物

        public byte[] b_scram2 = new byte[1];//站台急停
        public byte[] b_sure2 = new byte[1];//站台确定
        public byte[] b_something2 = new byte[1];//站台有物

        public byte s_scram1 { get { return b_scram1[0]; } set { b_scram1[0] = value; } }
        public byte s_sure1 { get { return b_sure1[0]; } set { b_sure1[0] = value; } }
        
        public byte s_something1 { get { return b_something1[0]; } set { b_something1[0] = value; } }
        public byte s_scram2 { get { return b_scram2[0]; } set { b_scram2[0] = value; } }
        public byte s_sure2 { get { return b_sure2[0]; } set { b_sure2[0] = value; } }
        public byte s_something2 { get { return b_something2[0]; } set { b_something2[0] = value; } }

        public SRM_PlatformStatus()
        {

        }

        public SRM_PlatformStatus(byte[] bytes)
        {
            this.LoadByte(bytes);
        }

        public void LoadByte(byte[] bytes)
        {
            int copy_Pionter = 0;

            try
            {
                Buffer.BlockCopy(bytes, copy_Pionter += 0, b_scram1, 0, b_scram1.Length);
                Buffer.BlockCopy(bytes, copy_Pionter += b_scram1.Length, b_sure1, 0, b_sure1.Length);
                Buffer.BlockCopy(bytes, copy_Pionter += b_sure1.Length, b_something1, 0, b_something1.Length);
                Buffer.BlockCopy(bytes, copy_Pionter += b_something1.Length, b_scram2, 0, b_scram2.Length);
                Buffer.BlockCopy(bytes, copy_Pionter += b_scram2.Length, b_sure2, 0, b_sure2.Length);
                Buffer.BlockCopy(bytes, copy_Pionter += b_sure2.Length, b_something2, 0, b_something2.Length);
            }catch(Exception e)
            {
                throw new ApplicationException(string.Format("ErrorMSG:{0},Pointer:{1},Bytes:{2}", e.Message, copy_Pionter, ByteHelper.ToMessage(bytes, bytes.Length)));
            }
        }

        public byte[] ToBytes()
        {
            int copy_Pionter = 0;
            byte[] result = new byte[cGroupLen];
            Buffer.BlockCopy(b_scram1, 0, result, copy_Pionter += 0, b_scram1.Length);
            Buffer.BlockCopy(b_sure1, 0, result, copy_Pionter += b_scram1.Length, b_sure1.Length);
            Buffer.BlockCopy(b_something1, 0, result, copy_Pionter += b_sure1.Length, b_something1.Length);
            Buffer.BlockCopy(b_scram2, 0, result, copy_Pionter += b_something1.Length, b_scram2.Length);
            Buffer.BlockCopy(b_sure2, 0, result, copy_Pionter += b_scram2.Length, b_sure2.Length);
            Buffer.BlockCopy(b_something2, 0, result, copy_Pionter += b_sure2.Length, b_something2.Length);
            return result;
        }

        public bool LoadFromPLC(CallDave.daveConnection _dVC,int i)
        {
            byte[] tempBytes = new byte[] { 0 };
            var rec = _dVC.readBits(CallDave.daveDB, address.db, i, 1, tempBytes);
            if (rec != 0) return false;
            byte c = tempBytes[0];
            byte b = 1;
            if (b == tempBytes[0])
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool SetOverRead(CallDave.daveConnection _dVc)
        {
            byte[] tempbytes = this.ToBytes();
            //var rec = _dVc.writeBytes(CallDave.daveDB, address.db, SRM_PlatformStatus.cGroupLen, 0, new byte[] { 0, 0 });
            var rec = _dVc.writeBytes(CallDave.daveDB, address.db, 0, 0, new byte[] { 0, 0 });
            return rec == 0;
        }
    }
    //站台指示灯状态
    public class SRM_PlatStatusWrite
    {
        public const string C_FillChar = " ";

        public const int C_leftorRigh = 0;

        public PLCAddress address = new PLCAddress();
        public const int cGroupLen = 9;
        public byte[] b_in1_led = new byte[1];//1#站台入库指示
        public byte[] b_out1_led = new byte[1];//1#站台出库指示
        public byte[] b_wait1_led = new byte[1];//1#站台等待指示
        public byte[] b_in2_led = new byte[1];//2#站台入库指示
        public byte[] b_out2_led = new byte[1];//2#站台出库指示
        public byte[] b_wait2_led = new byte[1];//2#站台等待指示
        public byte[] b_fault_led = new byte[1];//故障指示（红）
        public byte[] b_stop_led = new byte[1];//正常停机指示（黄）;
        public byte[] b_run_led = new byte[1];//运行指示（绿）
        
        public byte s_in1_led { get { return b_in1_led[0]; } set { b_in1_led[0] = value; } }
        public byte s_out1_led { get { return b_out1_led[0]; } set { b_out1_led[0] = value; } }
        public byte s_wait1_led { get { return b_wait1_led[0]; } set { b_wait1_led[0] = value; } }
        public byte s_in2_led { get { return b_in2_led[0]; } set { b_in2_led[0] = value; } }
        public byte s_out2_led { get { return b_out2_led[0]; } set { b_out2_led[0] = value; } }
        public byte s_wait2_led { get { return b_wait2_led[0]; } set { b_wait2_led[0] = value; } }
        public byte s_fault_led { get { return b_fault_led[0]; } set { b_fault_led[0] = value; } }
        public byte s_stop_led { get { return b_stop_led[0]; } set { b_stop_led[0] = value; } }
        public byte s_run_led { get { return b_run_led[0]; } set { b_run_led[0]= value; } }

        public byte[] ToBytes()
        {
            int copy_Pionter = 0;

            byte[] result = new byte[cGroupLen];
            Buffer.BlockCopy(b_in1_led, 0, result, copy_Pionter += 0, b_in1_led.Length);
            Buffer.BlockCopy(b_out1_led, 0, result, copy_Pionter += b_in1_led.Length, b_out1_led.Length);
            Buffer.BlockCopy(b_wait1_led, 0, result, copy_Pionter += b_out1_led.Length, b_wait1_led.Length);
            Buffer.BlockCopy(b_in2_led, 0, result, copy_Pionter += b_wait1_led.Length, b_in2_led.Length);
            Buffer.BlockCopy(b_out2_led, 0, result, copy_Pionter += b_in2_led.Length, b_out2_led.Length);
            Buffer.BlockCopy(b_wait2_led, 0, result, copy_Pionter += b_out2_led.Length, b_wait2_led.Length);
            Buffer.BlockCopy(b_fault_led, 0, result, copy_Pionter += b_wait2_led.Length, b_fault_led.Length);
            Buffer.BlockCopy(b_stop_led, 0, result, copy_Pionter += b_fault_led.Length, b_stop_led.Length);
            Buffer.BlockCopy(b_run_led, 0, result, copy_Pionter += b_stop_led.Length, b_run_led.Length);

            return result;
        }

        public bool WirteToPLC(CallDave.daveConnection _dVC,int i,int j)
        {
            byte[] tempbytes = this.ToBytes();
            tempbytes[SRM_PlatStatusWrite.cGroupLen - 1] = 0; //设置和为未读
            //var rec = _dVC.writeBytes(CallDave.daveDB, address.db, address.start, SRM_PlatStatusWrite.cGroupLen, tempbytes);
            var rec = _dVC.writeBits(CallDave.daveDB, address.db, i, 1, new byte[] { (byte)j });
            if (rec != 0) return false;
            return true;
        }

        public bool CheckOverRead(CallDave.daveConnection _dVC)
        {
            byte[] flag_OverRead = new byte[] { 1, 1 };
            var res = _dVC.writeBytes(CallDave.daveDB, address.db, SRM_PlatStatusWrite.cGroupLen, 0, flag_OverRead);
            return res == 0;
        }

        public void LoadByte(byte[] bytes)
        {
            int copy_Pionter = 0;

            try
            {
                Buffer.BlockCopy(bytes, copy_Pionter += 0, b_in1_led, 0, b_in1_led.Length);
                Buffer.BlockCopy(bytes, copy_Pionter += b_in1_led.Length, b_out1_led, 0, b_out1_led.Length);
                Buffer.BlockCopy(bytes, copy_Pionter += b_out1_led.Length, b_wait1_led, 0, b_wait1_led.Length);
                Buffer.BlockCopy(bytes, copy_Pionter += b_wait1_led.Length, b_in2_led, 0, b_in2_led.Length);
                Buffer.BlockCopy(bytes, copy_Pionter += b_in2_led.Length, b_out2_led, 0, b_out2_led.Length);
                Buffer.BlockCopy(bytes, copy_Pionter += b_out2_led.Length, b_wait2_led, 0, b_wait2_led.Length);
                Buffer.BlockCopy(bytes, copy_Pionter += b_wait2_led.Length, b_fault_led, 0, b_fault_led.Length);
                Buffer.BlockCopy(bytes, copy_Pionter += b_fault_led.Length, b_stop_led, 0, b_stop_led.Length);
                Buffer.BlockCopy(bytes, copy_Pionter += b_stop_led.Length, b_run_led, 0, b_run_led.Length);
            }
            catch (Exception e)
            {
                throw new ApplicationException(string.Format("ErrorMSG:{0},Pointer:{1},Bytes:{2}", e.Message, copy_Pionter, ByteHelper.ToMessage(bytes, bytes.Length)));
            }
        }

        public bool LoadFromPLC(CallDave.daveConnection _dVC)
        {
            byte[] tempbytes = new byte[SRM_PlatStatusWrite.cGroupLen];
            var rec = _dVC.readBytes(CallDave.daveDB, address.db, address.start, SRM_PlatStatusWrite.cGroupLen, tempbytes);
            if (rec != 0) return false;
            this.LoadByte(tempbytes);
            return true;
        }
    }
}

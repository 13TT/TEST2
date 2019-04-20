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
        public const int cGroupLen = 23;

        public byte[] b_Number = new byte[2]; //序号
        public byte[] b_FrPattern = new byte[2]; //当前执行模式
        public byte[] b_FrStand = new byte[2]; //当前执行站
        public byte[] b_FrLine = new byte[2]; //当前执行列
        public byte[] b_FrGrid = new byte[2]; //当前执行格
        public byte[] b_FrTier = new byte[2]; //当前执行层

        public byte[] b_ToStand = new byte[2]; //当前执行站
        public byte[] b_ToLine = new byte[2]; //当前执行列
        public byte[] b_ToGrid = new byte[2]; //当前执行格
        public byte[] b_ToTier = new byte[2]; //当前执行层

        public byte[] b_Execute = new byte[1]; //执行信号
        public byte[] b_Fault = new byte[1]; //故障复位
        public byte[] b_Heartbeat = new byte[1]; //心跳信号

        public UInt16 s_ID {
            get
            {
                byte[] tmpbyte = new byte[b_Number.Length];
                Buffer.BlockCopy(b_Number, 0, tmpbyte, 0, b_Number.Length);
                Array.Reverse(tmpbyte);
                return BitConverter.ToUInt16(tmpbyte, 0);
            }
            set
            {
                byte[] tmpbyte = BitConverter.GetBytes(value);
                Array.Reverse(tmpbyte);
                b_Number = tmpbyte;
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

        public byte s_Execute { get { return b_Execute[0]; } set { b_Execute[0] = value; } }
        public byte s_Fault { get { return b_Fault[0]; } set { b_Fault[0] = value; } }
        public byte s_Hearbeat { get { return b_Heartbeat[0]; } set { b_Heartbeat[0] = value; } }

        public byte[] ToBytes()
        {
            int copy_Pionter = 0;

            byte[] result = new byte[cGroupLen];
            Buffer.BlockCopy(b_Number, 0, result, copy_Pionter += 0, b_Number.Length);
            Buffer.BlockCopy(b_FrPattern, 0, result, copy_Pionter += b_Number.Length, b_FrPattern.Length);
            Buffer.BlockCopy(b_FrStand, 0, result, copy_Pionter += b_FrPattern.Length, b_FrStand.Length);
            Buffer.BlockCopy(b_FrLine, 0, result, copy_Pionter += b_FrStand.Length, b_FrLine.Length);
            Buffer.BlockCopy(b_FrGrid, 0, result, copy_Pionter += b_FrLine.Length, b_FrGrid.Length);
            Buffer.BlockCopy(b_FrTier, 0, result, copy_Pionter += b_FrGrid.Length, b_FrTier.Length);
            Buffer.BlockCopy(b_ToStand, 0, result, copy_Pionter += b_FrTier.Length, b_ToStand.Length);
            Buffer.BlockCopy(b_ToLine, 0, result, copy_Pionter += b_ToStand.Length, b_ToLine.Length);
            Buffer.BlockCopy(b_ToGrid, 0, result, copy_Pionter += b_ToLine.Length, b_ToGrid.Length);
            Buffer.BlockCopy(b_ToTier, 0, result, copy_Pionter += b_ToGrid.Length, b_ToTier.Length);
            Buffer.BlockCopy(b_Execute, 0, result, copy_Pionter += b_ToTier.Length, b_Execute.Length);
            Buffer.BlockCopy(b_Fault, 0, result, copy_Pionter += b_Execute.Length, b_Fault.Length);
            Buffer.BlockCopy(b_Heartbeat, 0, result, copy_Pionter += b_Fault.Length, b_Heartbeat.Length);

            return result;
        }

        public bool WirteToPLC(CallDave.daveConnection _dVC)
        {
            byte[] tempbytes = this.ToBytes();
            tempbytes[SRM_Respone.cGroupLen-1] = 0; //设置和为未读
            var rec = _dVC.writeBytes(CallDave.daveDB, address.db, address.start, SRM_Respone.cGroupLen, tempbytes);
            if (rec != 0) return false;
            return true;
        }
        public bool CheckOverRead(CallDave.daveConnection _dVC)
        {
            byte[] flag_OverRead = new byte[] { 0, 0 };
            var res = _dVC.readBytes(CallDave.daveDB, address.db, 18, 2, flag_OverRead);
            return res == 0;
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

        public PLCAddress address = new PLCAddress();

        public const int cGroupLen = 4;
        public byte[] b_ok1 = new byte[1];//1站台确定
        public byte[] b_ok2 = new byte[1];//2站台确定
        public byte[] b_something1 = new byte[1];//1站是否有物
        public byte[] b_something2 = new byte[1];//2站是否有物

        public byte s_ok1 { get { return b_ok1[0]; } set { b_ok1[0] = value; } }
        public byte s_ok2 { get { return b_ok2[0]; } set { b_ok2[0] = value; } }
        
        public byte s_something1 { get { return b_something1[0]; } set { b_something1[0] = value; } }
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
                Buffer.BlockCopy(bytes, copy_Pionter += 0, b_ok1, 0, b_ok1.Length);
                Buffer.BlockCopy(bytes, copy_Pionter += b_ok1.Length, b_ok2, 0, b_ok2.Length);
                Buffer.BlockCopy(bytes, copy_Pionter += b_ok2.Length, b_something1, 0, b_something1.Length);
                Buffer.BlockCopy(bytes, copy_Pionter += b_something1.Length, b_something2, 0, b_something2.Length);
            }catch(Exception e)
            {
                throw new ApplicationException(string.Format("ErrorMSG:{0},Pointer:{1},Bytes:{2}", e.Message, copy_Pionter, ByteHelper.ToMessage(bytes, bytes.Length)));
            }
        }

        public byte[] ToBytes()
        {
            int copy_Pionter = 0;
            byte[] result = new byte[cGroupLen];
            Buffer.BlockCopy(b_ok1, 0, result, copy_Pionter += 0, b_ok1.Length);
            Buffer.BlockCopy(b_ok2, 0, result, copy_Pionter += b_ok1.Length, b_ok2.Length);
            Buffer.BlockCopy(b_something1, 0, result, copy_Pionter += b_ok2.Length, b_something1.Length);
            Buffer.BlockCopy(b_something2, 0, result, copy_Pionter += b_something1.Length, b_something2.Length);
            return result;
        }

        public bool LoadFromPLC(CallDave.daveConnection _dVC)
        {
            byte[] tempBytes = new byte[SRM_PlatformStatus.cGroupLen];
            var rec = _dVC.readBytes(CallDave.daveDB, address.db, address.start, SRM_PlatformStatus.cGroupLen, tempBytes);
            if (rec != 0) return false;
            this.LoadByte(tempBytes);
            return true;
        }

        public bool SetOverRead(CallDave.daveConnection _dVc)
        {
            byte[] tempbytes = this.ToBytes();
            //var rec = _dVc.writeBytes(CallDave.daveDB, address.db, SRM_PlatformStatus.cGroupLen, 0, new byte[] { 0, 0 });
            var rec = _dVc.writeBytes(CallDave.daveDB, address.db, 0, 0, new byte[] { 0, 0 });
            return rec == 0;
        }
    }
    //站台
    public class SRM_PlatStatusWrite
    {
        public const string C_FillChar = " ";

        public const int C_leftorRigh = 0;

        public PLCAddress address = new PLCAddress();
        public const int cGroupLen = 2;
        public byte[] b_LED1 = new byte[1];//1站捡料指示
        public byte[] b_LED2 = new byte[1];//2站捡料指示

        public byte s_LED1 { get { return b_LED1[0]; } set { b_LED1[0] = value; } }
        public byte s_LED2 { get { return b_LED2[0]; } set { b_LED2[0] = value; } }

        public byte[] ToBytes()
        {
            int copy_Pionter = 0;

            byte[] result = new byte[cGroupLen];
            Buffer.BlockCopy(b_LED1, 0, result, copy_Pionter += 0, b_LED1.Length);
            Buffer.BlockCopy(b_LED2, 0, result, copy_Pionter += b_LED1.Length, b_LED2.Length);

            return result;
        }

        public bool WirteToPLC(CallDave.daveConnection _dVC)
        {
            byte[] tempbytes = this.ToBytes();
            tempbytes[SRM_PlatStatusWrite.cGroupLen - 1] = 0; //设置和为未读
            var rec = _dVC.writeBytes(CallDave.daveDB, address.db, address.start, SRM_PlatStatusWrite.cGroupLen, tempbytes);
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
                Buffer.BlockCopy(bytes, copy_Pionter += 0, b_LED1, 0, b_LED1.Length);
                Buffer.BlockCopy(bytes, copy_Pionter += b_LED1.Length, b_LED2, 0, b_LED2.Length);
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

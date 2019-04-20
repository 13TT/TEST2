using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LIB_Common;
using LIB_Communation;
namespace SettingPara
{
    /// <summary>
    /// 输送机状态类
    /// </summary>
    public class SRM_Status
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
        public const int cGroupLen = 56;

        /// <summary>
        /// 根据Byte数据填充类
        /// </summary>
        /// <param name="bytes"></param>
        private void LoadBytes(byte[] bytes)
        {
        }
        /// <summary>
        /// 读取PLC
        /// </summary>
        /// <param name="_dVC"></param>
        /// <returns></returns>
        public bool LoadFromPLC(CallDave.daveConnection _dVC)
        {
            return true;
        }
        /// <summary>
        /// 转换为Byte数组
        /// </summary>
        /// <returns></returns>
        public byte[] Tobytes()
        {
            byte[] result = new byte[cGroupLen];
            return result;
        }
    }
}

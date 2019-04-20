using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SettingPara
{
    /// <summary>
    /// Byte字节处理工具类
    /// </summary>
    public class ByteHelper
    {
        //copy one Byte Array to the begin of the other Byte Array
        public static void ByteCopy(Byte[] copyto, Byte[] copyfrom)
        {
            int l = 0;
            if (copyto.Length > copyfrom.Length)
            {
                l = copyfrom.Length;
            }
            else
            {
                l = copyto.Length;
            }

            for (int i = 0; i < l; i++)
            {
                copyto[i] = copyfrom[i];
            }
        }

        //Get a sub Byte Array from another Byte array
        public static Byte[] GetSubByte(Byte[] copyfrom, int pos, int len)
        {
            if ((len <= 0) || (pos < 0))
                return null;

            int f = 0;

            if (pos > 0)
                pos = pos - 1;

            if (pos > copyfrom.Length)
                return null;


            if (len > copyfrom.Length)
            {
                f = copyfrom.Length - pos;
            }
            else
            {
                f = len;
            }

            Byte[] b = new Byte[f];

            for (int i = 0; i < f; i++)
            {
                b[i] = copyfrom[i + pos];
            }

            return b;

        }

        //copy the set length of one Byte Array to the target position of the other Byte Array
        //position count from 1 not 0
        public static void ByteCopy(Byte[] copyto, Byte[] copyfrom, int pos, int len)
        {
            int l = 0;
            int f = 0;

            if ((len <= 0) || (pos < 0))
                return;

            if (pos > copyto.Length)
                return;

            l = copyto.Length - pos + 1;

            if (len > copyfrom.Length)
            {
                f = copyfrom.Length;
            }
            else
            {
                f = len;
            }

            if (l >= f)
                l = f;

            for (int i = pos - 1; i < l + pos - 1; i++)
            {
                copyto[i] = copyfrom[i - pos + 1];
            }
        }

        //set the Byte Array to a value
        public static void ByteSet(Byte[] copyto, byte b)
        {
            for (int i = 0; i < copyto.Length; i++)
            {
                copyto[i] = b;
            }

        }

        //copy a string to the Byte Array as ASCII code
        public static void StringCopy(Byte[] copyto, String s)
        {
            Byte[] b = Encoding.ASCII.GetBytes(s);
            ByteCopy(copyto, b);
        }

        //copy a string to the Byte Array as ASCII code
        public static void StringCopy(Byte[] copyto, String s, int pos, int len)
        {
            Byte[] b = Encoding.ASCII.GetBytes(s);
            ByteCopy(copyto, b, pos, len);
        }

        //copy a string to the Byte Array as ASCII code and fill with assign charactor to reach the length
        //left = 0, and right = 1
        public static void StringCopy(Byte[] copyto, String s, int pos, int len, String c, int direct)
        {
            StringBuilder sb = new StringBuilder();

            if ((len <= 0) || (pos < 0))
                return;

            for (int i = 0; i < len - s.Length; i++)
            {
                sb.Append(c);
            }
            if (direct == 0)
            {
                sb.Append(s);
            }
            else
            {
                sb.Insert(0, s);
            }
            Byte[] b = Encoding.ASCII.GetBytes(sb.ToString());
            Buffer.BlockCopy(b, 0, copyto, pos, len);
            //ByteCopy(copyto, b, pos, len);
        }

        //convert a Byte Array to string
        public static String ToString(Byte[] b)
        {
            return Encoding.ASCII.GetString(b);
        }

        //copy a int to the Byte Array as defined length 
        public static void IntCopy(Byte[] copyto, int data, int len)
        {
            StringBuilder format = new StringBuilder();
            for (int i = 0; i < len; i++)
            {
                format.Append("0");
            }
            String s = data.ToString(format.ToString());
            Byte[] b = Encoding.ASCII.GetBytes(s);
            StringCopy(copyto, s);
        }

        //copy a ushort value to a 2 length Byte Array 
        public static void HexCopy(Byte[] copyto, ushort data)
        {
            Byte[] b = { 0, 0 };
            b[0] = Convert.ToByte(data >> 8);
            b[1] = Convert.ToByte(data & 0x00FF);
            ByteCopy(copyto, b);
        }

        //copy a ushort value to a 2 length Byte Array 
        public static void HexCopy(Byte[] copyto, short data, int pos, int len)
        {
            Byte[] b = { 0, 0 };
            b[0] = Convert.ToByte(data >> 8);
            b[1] = Convert.ToByte(data & 0x00FF);
            ByteCopy(copyto, b, pos, len);
        }

        //convert the 2 length Byte Array to a int value
        public static int ToInt(Byte[] b)
        {
            int i = Convert.ToInt32(b[0] << 8);
            i = i + Convert.ToInt32(b[1]);
            return i;
        }

        //convert a Byte Array to a message string
        public static String ToMessage(Byte[] b, int len)
        {
            if ((len <= 0))
                return null;

            int l = len;
            Int16 value;
            StringBuilder s = new StringBuilder();
            if (l > b.Length)
            {
                l = b.Length;
            }

            for (int i = 0; i < l; i++)
            {
                value = Convert.ToInt16(b[i]);
                if ((value >= 48) && (value <= 122))
                {
                    s.Append(Encoding.ASCII.GetString(b, i, 1));
                }
                else
                {
                    s.Append("[" + Convert.ToInt16(b[i]).ToString("X") + "]");
                }

            }
            return s.ToString();
        }


        public static void SwapValue(ref string value1, ref string value2)
        {
            string tmp;
            tmp = value1; value1 = value2; value2 = tmp;
        }
        public static void SwapValue(ref int value1, ref int value2)
        {
            int tmp;
            tmp = value1; value1 = value2; value2 = tmp;
        }
        public static void SwapValue(ref object value1, ref object value2)
        {
            object tmp;
            tmp = value1; value1 = value2; value2 = tmp;
        }

        public static byte[] GetByesToTimTime(DateTime dt)
        {
            Byte[] b = new Byte[8];
            int i = dt.Year - 2000;
            if (i < 0)
                i = 100 + i;
            b[0] = Convert.ToByte(i);

            i = dt.Month;
            b[1] = Convert.ToByte(i);

            i = dt.Day;
            b[2] = Convert.ToByte(i);

            i = dt.Hour;
            b[3] = Convert.ToByte(i);

            i = dt.Minute;
            b[4] = Convert.ToByte(i);

            i = dt.Second;
            b[5] = Convert.ToByte(i);

            i = dt.Millisecond;
            b[6] = Convert.ToByte(Math.Floor((Double)i / 10));

            int j = i % 10;
            b[7] = Convert.ToByte((j << 4) + Convert.ToInt16(dt.DayOfWeek) + 1);

            return b;
        }
        public static DateTime GetDateTimeToByTimeByes(byte[] bt)
        {

            int Year = Convert.ToInt32(bt[0]) + 2000;
            int Month = Convert.ToInt32(bt[1]);
            int Day = Convert.ToInt32(bt[2]);
            int Hour = Convert.ToInt32(bt[3]);
            int Minute = Convert.ToInt32(bt[4]);
            int Second = Convert.ToInt32(bt[5]);
            int Millisecond = Convert.ToInt32(bt[6]) * 10;

            DateTime dtime = new DateTime(Year, Month, Day, Hour, Minute, Second, Millisecond);
            int i = Convert.ToInt32(bt[7]) - (int)dtime.DayOfWeek;
            dtime.AddMilliseconds(dtime.Millisecond + (i >> 4));
            return dtime;
        }
    }
}

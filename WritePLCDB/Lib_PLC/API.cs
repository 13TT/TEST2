using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lib_PLC
{
    public class API
    {
        [DllImport("libnodave.dll")]
        private extern static int load_tool(byte nr, string device, byte[,] adr_table);

    }
}

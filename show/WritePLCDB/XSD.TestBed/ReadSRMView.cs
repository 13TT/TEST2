using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using XSD.Lib_PLC;
namespace XSD.TestBed
{
    public class ConnectionPool
    {
        public List<CallDave.daveConnection> Connections = new List<CallDave.daveConnection>();
        public List<CallDave.daveInterface> interfaces = new List<CallDave.daveInterface>();
        public void AddConnection(CallDave.daveInterface di)
        {
            if (di != null)
            {
                CallDave.daveConnection dc = new CallDave.daveConnection(di, 2, 0, 2);
                Connections.Add(dc);
                interfaces.Add(di);
            }
        }
        public int StartAll()
        {
            foreach (CallDave.daveConnection dcon in Connections)
            {
                int res = dcon.connectPLC();
                if (res != 0)
                {
                    return res;
                }
            }
            return 0;
        }
        public void StopAll()
        {
            foreach (CallDave.daveConnection dcon in Connections)
            {
                int res = dcon.disconnectPLC();
                
            }
            foreach(CallDave.daveInterface di in interfaces)
            {
                 di.disconnectAdapter();
         
            }
            
            GC.Collect();
	         GC.WaitForPendingFinalizers();
        }
        public static CallDave.daveInterface CreateDaveInterface_Define(string ip, int port)
        {
            CallDave.daveOSserialType fds;
            CallDave.daveInterface di = null;
            fds.rfd = CallDave.openSocket(port, ip);
            fds.wfd = fds.rfd;
            if (fds.rfd > 0)
            {
                di = new CallDave.daveInterface(fds, "IF1", 0, CallDave.daveProtoISOTCP, CallDave.daveSpeed187k);
                di.setTimeout(1000000);
                int res = di.initAdapter();
            }


            return di;
        }
    }
    public class SRMStatusView
    {
        public const int MaxSRMNo = 2;

        public ConnectionPool CP = new ConnectionPool();

        public void InitConnection()
        {
            for (int i = 0; i < MaxSRMNo; i++)
            {
                CP.AddConnection(ConnectionPool.CreateDaveInterface_Define(string.Format("10..101.82.{0}", 201 + i), 102));
            }

        }
        public DataTable GetDt_SRMStatus()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ID");
            dt.Columns.Add("Xerify");
            dt.Columns.Add("RunMode");
            dt.Columns.Add("OPMode");
            dt.Columns.Add("RunStatus");
            dt.Columns.Add("Horizontal");
            dt.Columns.Add("Vertical");
            dt.Columns.Add("CurrnetCol");
            dt.Columns.Add("CurrnetLVL");
            dt.Columns.Add("Fork");
            try
            {
                foreach (var dconn in CP.Connections)
                {
                    var dr = dt.NewRow();
                    byte[] buffer = new byte[28];
                    dconn.start();
                    int res = dconn.readBytes(CallDave.daveDB, 101, 0, 28, buffer);
                    if (res == 0)
                    {
                        dr["ID"] = string.Format("SRM{0}", CP.Connections.IndexOf(dconn)+1);
                        dr["Xerify"] = dconn.getU8();
                        dr["RunMode"] = dconn.getU8();
                        dr["OPMode"] = dconn.getU8();
                        dr["RunStatus"] = dconn.getU8();
                        dr["Horizontal"] = dconn.getS32();
                        dr["Vertical"] = dconn.getS32();
                        dr["CurrnetCol"] = dconn.getU8();
                        dr["CurrnetLVL"] = dconn.getU8();
                        dr["Fork"] = dconn.getU8();
                        dt.Rows.Add(dr);
                    } 
                    dconn.stop();

                    //int l = dconn.getAnswLen();
                    //CallDave.resultSet rs=new CallDave.resultSet();
                    //CallDave.PDU p = new CallDave.PDU();
        
                    //dconn.execReadRequest(new CallDave.PDU(), rs);
                    //int ur = dconn.useResult(rs, 0);

                    //res = dconn.disconnectPLC();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }


    }
}

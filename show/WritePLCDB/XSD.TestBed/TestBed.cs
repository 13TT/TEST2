using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using XSD.Lib_PLC;
namespace XSD.TestBed
{
    public partial class TestBed : Form
    {
        SRMStatusView sv = new SRMStatusView();

        public TestBed()
        {
            InitializeComponent();
        }

        private void btn_Connect_Click(object sender, EventArgs e)
        {
            CallDave.daveOSserialType fds;
            CallDave.daveInterface di;
            CallDave.daveConnection dc;
            int localMPI = 0;
            int plcMPI = 2;
            int i, a = 0, j, res, b = 0, c = 0;
            float d = 0;

            fds.rfd = CallDave.openSocket(102, "192.168.1.4");
            //fds.rfd = CallDave.setPort("80001", "38400", 'O');
            fds.wfd = fds.rfd;
            if (fds.rfd > 0)
            {
                di = new CallDave.daveInterface(fds, "IF1", localMPI, CallDave.daveProtoISOTCP, CallDave.daveSpeed187k);
                di.setTimeout(10000);
                res = di.initAdapter();
                if (res == 0)
                {
                    dc = new CallDave.daveConnection(di, plcMPI, 0, 2);
                    if (0 == dc.connectPLC())
                    {

                        byte[] buffer = { 0, 1 };
                        //int res = e.PLCConn.writeBits(CallDave.daveDB, 1354, 114, 1, buffer);

                        res = dc.writeBytes(CallDave.daveDB, 1354, 114, 2, buffer);

                        //buffer[0] = 2;
                        //res = dc.writeBytes(CallDave.daveDB, 101, 0, 1, buffer);

                        //res = dc.readBytes(CallDave.daveDB, 101, 0, 28, buffer);
                        //CallDave.PDU pdu = dc.prepareReadRequest();
                        //pdu.addBitVarToReadRequest(CallDave.daveDB, 101, 0, 1);
                        //pdu.addBitVarToReadRequest(CallDave.daveDB, 101, 0, 1);
                        //pdu.addBitVarToReadRequest(CallDave.daveDB, 101, 0, 1);
                        //if (res == 0)
                        //{
                        //    //a = dc.getS32();
                        //    //b = dc.getS32();
                        //    //c = dc.getS32();
                        //    //d = dc.getFloat();
                        //    int b1 = dc.getU8();
                        //    int b2 = dc.getU8();
                        //    int b3 = dc.getU8();
                        //    int b4 = dc.getU8();
                        //    int B5 = dc.getS32();
                        //    int B6 = dc.getS32();
                        //    int b7 = dc.getU8();
                        //    int b8 = dc.getU8();
                        //    int B9 = dc.getU8();

                        //}
                        res = dc.disconnectPLC();
                        //res = dc.writeBytes(CallDave.daveDB, 0, 1, 16, null);
                    }
                }
            }
        }


        private void Connection()
        {
            sv.InitConnection();
            sv.CP.StartAll();
            //sv.CP.StartAll();
        }

        private void btn_Start_Click(object sender, EventArgs e)
        {
            btn_Connect_Click(null, null);
            //Connection();
            //timer_ReadPLC.Interval = 2000;
            //timer_ReadPLC.Enabled = true;
            //timer_ReadPLC.Start(); ;
        }

        private void timer_ReadPLC_Tick_1(object sender, EventArgs e)
        {

            dgv_SRMStatus.DataSource = sv.GetDt_SRMStatus();
            //sv.CP.StopAll();
        }
    }
}

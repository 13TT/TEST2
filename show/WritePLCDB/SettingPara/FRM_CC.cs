using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LIB_Common;
using LIB_Communation;
using System.Configuration;
using System.Diagnostics;
using System.Threading;

namespace SettingPara
{

    public partial class FRM_CC : Form
    {
        DataTable dt_sts = new DataTable();
        DataTable dt_task = new DataTable();
        DataTable dt_Srm = new DataTable();
        DataTable dt_T = new DataTable();
        public FRM_CC()
        {
            InitializeComponent();
        }

        private void FRM_CC_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.None;     //设置窗体为无边框样式
            this.WindowState = FormWindowState.Maximized;

            timer_SRM_Tick(sender, e);
        }

        private void Fill_T()
        {
            WMBusiness wmbus = new WMBusiness();
        }
    
        private void timer_CCSTS_Tick(object sender, EventArgs e)
        {
            
        }

        private void timer_Request_Tick(object sender, EventArgs e)
        {

        }

        private void txt_DestinationLoc_TextChanged(object sender, EventArgs e)
        {

        }

        private void FRM_CC_FormClosing(object sender, FormClosingEventArgs e)
        {
            Process.GetCurrentProcess().Kill();
            this.Close();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void timer_SRM_Tick(object sender, EventArgs e)
        {
            try
            {
                var db = new DBAccess_MySql("MySql");
                string outString = "";
                DataTable dt;
                string sql;
                sql = "SELECT order_user,order_style FROM order_info WHERE order_state = 0 or order_state = 1 LIMIT 1";
                var rlt = db.QuerySQL_ToTable(sql, out dt, out outString);
                DataRow dr = dt.Rows[0];
                string order_state = dr[0].ToString();
                string order_style = dr[1].ToString();

                sql = "SELECT goods_code,goods_name,goods_style FROM goods_info WHERE goods_id = " + int.Parse(order_state) + "";
                var rlt1 = db.QuerySQL_ToTable(sql, out dt, out outString);
                DataRow dr1 = dt.Rows[0];
                lab_name.Text = dr1[0].ToString();
                string styles = dr1[1].ToString();
                lab_house.Text = dr1[2].ToString();

                if (styles == "端拾器")
                {
                    lab_zhanhao.Text = "1";
                }
                if (styles == "检具")
                {
                    lab_zhanhao.Text = "2";
                }
                if (order_style == "2")
                {
                    lab_leixing.Text = "出库";
                }
                if (order_style == "1")
                {
                    lab_leixing.Text = "入库";
                }
            }
            catch
            {
                lab_name.Text = "";
                lab_house.Text = "";
                lab_zhanhao.Text = "";
                lab_leixing.Text = "";
            }
        }

        private void timer_T_ShowData_Tick(object sender, EventArgs e)
        {
        }

        public enum OrderStyle
        {
            InStore = 1,
            OutStore = 2,
            PickStore = 3,
            AddInStore = 4,
            S2S = 5,
            H2H = 6,
            Cycle = 8
        }
        public class SysParameter
        {
            public static OrderStyle defaultOutStyle = OrderStyle.OutStore;
            public static int TitleMoveStep = 2;
            public const int cLine = 5;
            public const int cList = 2;
            public const int blank = 15;
            public const int floor = 5;
        }

        private void tm_updateOrder_Tick(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
        }

        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
        }

        //重写居中
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            int x = (int)(0.5 * (this.Width - groupBox2.Width));
            int y = groupBox2.Location.Y;
            groupBox2.Location = new System.Drawing.Point(x, y);

            base.OnResize(e);
            int x1 = (int)(0.5 * (this.Width - groupBox1.Width));
            int y1 = groupBox1.Location.Y;
            groupBox1.Location = new System.Drawing.Point(x1, y1);

            base.OnResize(e);
            int x2 = (int)(0.5 * (this.Width - groupBox3.Width));
            int y2 = groupBox3.Location.Y;
            groupBox3.Location = new System.Drawing.Point(x2, y2);
        }

        private void btn_yanshi_Click(object sender, EventArgs e)
        {
            //timerT_Time();
            Outyanshi(1, 2, 1);
        }

        private void Inyanshi(int list,int blank,int floor)
        {
        }

        private void Outyanshi(int list,int blank,int floor)
        {
        }

        private void button9_Click(object sender, EventArgs e)
        {
            Outyanshi(1, 3, 1);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Inyanshi(1, 3, 1);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Inyanshi(1, 2, 1);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Outyanshi(1, 2, 2);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Inyanshi(1, 2, 2);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Outyanshi(1, 3, 2);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Inyanshi(1, 3, 2);
        }

        private void button10_Click(object sender, EventArgs e)
        {
            
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }
    }
}

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
using SettingPara.UI;

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
            PLCSystem_SRM.InitItem();
            PLCSystem_SRM.InitConn();
            PLCSystem_SRM.Start();

            PLCSystem_CON.InitItem();
            PLCSystem_CON.InitConn();
            PLCSystem_CON.Start();

            timer_CCSTS.Enabled = true;
            timer_CCSTS.Interval = 3000;
            timer_CCSTS.Start();

            timer_SRM.Enabled = true;
            timer_SRM.Interval = 3000;
            timer_SRM.Start();

            timer_T_ShowData.Enabled = true;
            timer_T_ShowData.Interval = 3000;
            timer_T_ShowData.Start();

            //this.Location = new Point(245, 39);
            this.DataGridLoad();
            tm_updateOrder.Start();

            groupBox1.Controls.Clear();
            SrmStatus ss = new SrmStatus();
            groupBox1.Controls.Add(ss);
            ss.Dock = DockStyle.Fill;

            OneIn pm = new OneIn();
            homeContent.Controls.Add(pm);
            pm.Dock = DockStyle.Fill;

            treeView1.SelectedNode = treeView1.Nodes[3];
            treeView1.SelectedNode.Expand();
            treeView1.SelectedNode = treeView1.Nodes[4];
            treeView1.SelectedNode.Expand();
        }

        private bool SplitplcAddress(string plcAddress, out int db, out int start, out string OutMsg)
        {
            db = 0;
            start = 0;
            OutMsg = "";
            try
            {
                string[] temp = plcAddress.Split('.');

                if (temp.Length == 2)
                {
                    if (temp[0].Substring(0, 2).ToUpper() == "DB".ToUpper())
                    {
                        db = int.Parse(temp[0].Substring(2, temp[0].Length - 2));
                    }
                    else
                    {
                        return false;
                    }
                    if (temp[1].Substring(0, 3).ToUpper() == "DBW".ToUpper())
                    {
                        start = int.Parse(temp[1].Substring(3, temp[1].Length - 3));
                    }
                    else
                    {
                        return false;
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
            PLCSystem_SRM.stop();
            Program.Log_Agent_CON.Stop();
            Program.Log_Agent_SRM.Stop();
            Program.Log_Agent_BUS.Stop();
            Program.Log_Agent_DB.Stop();
            Program.Log_Agent_TEMP.Stop();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //thinkserver td340
        }

        private void timer_SRM_Tick(object sender, EventArgs e)
        {
            lbl_SRMStatus.Text = PLCSystem_SRM.GetPLCSts();
        }

        private void timer_T_ShowData_Tick(object sender, EventArgs e)
        {
            Fill_T();
            foreach (var kv in PLCSystem_SRM.Dic_SRM_Res)
            {
                string s1 = kv.Value.s_FrGrid.ToString();//格位
                string s2 = kv.Value.s_FrTier.ToString();//层位
                textBox1.Text = s1;
                textBox2.Text = s2;
                break;
            }
        }

        #region
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
            public const int blank = 7;
            public const int floor = 7;
        }
        private void DataGridLoad()
        {
            int iRowCount = SysParameter.blank;
            int iColumnCount = SysParameter.floor + 1;

            dgv_list11.Columns.Clear();
            dgv_list11.RowCount = iRowCount;
            dgv_list11.ColumnCount = iColumnCount;

            dgv_list12.Columns.Clear();
            dgv_list12.RowCount = iRowCount;
            dgv_list12.ColumnCount = iColumnCount;

            for (int i = 0; i < iRowCount; i++)
            {
                if (i < 7)
                {
                    dgv_list11.Rows[i].Cells[0].Value = string.Format("{0}", i + 1);
                    dgv_list12.Rows[i].Cells[0].Value = string.Format("{0}", i + 1);
                }
            }
            dgv_list11.Columns[0].HeaderText = "格位";
            dgv_list12.Columns[0].HeaderText = "格位";
            for (int i = 1; i < iColumnCount; i++)
            {
                dgv_list11.Columns[i].HeaderText = string.Format("{0}层", i);
                dgv_list12.Columns[i].HeaderText = string.Format("{0}层", i);
            }


            for (int i = 0; i < iColumnCount; i++)
            {
                dgv_list11.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                dgv_list12.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            update_dgv(dgv_list11, 1, 1);
            update_dgv(dgv_list12, 1, 2);
        }
        private void update_dgv(DataGridView dgv, int line, int list)
        {
            ////查询sql
            string outString = null;
            var db = new DBAccess_MySql("MySql");
            string sql = string.Format("SELECT blank, floor, house_state FROM house_info WHERE list = {0} and line={1} ORDER BY blank ASC,floor ASC", list, line);
            DataTable dt = db.QueryDataTable(sql, out outString);
            if (dt.Rows.Count == dgv.RowCount * (dgv.ColumnCount - 1))
            {
                for (int blankIndex = 0; blankIndex < dgv.RowCount; blankIndex++)
                {
                    for (int floorIndex = 0; floorIndex < dgv.ColumnCount - 1; floorIndex++)
                    {
                        if (dt.Rows[blankIndex * (dgv.ColumnCount - 1) + floorIndex].ItemArray[2].ToString().Trim() == "N")
                        {
                            dgv.Rows[blankIndex].Cells[floorIndex + 1].Value = ' ';
                            dgv.Rows[blankIndex].Cells[floorIndex + 1].Style.BackColor = Color.White;
                        }
                        else if (dt.Rows[blankIndex * (dgv.ColumnCount - 1) + floorIndex].ItemArray[2].ToString().Trim() == "S")
                        {
                            dgv.Rows[blankIndex].Cells[floorIndex + 1].Value = 'S';
                            dgv.Rows[blankIndex].Cells[floorIndex + 1].Style.BackColor = Color.Yellow;
                        }
                        else if (dt.Rows[blankIndex * (dgv.ColumnCount - 1) + floorIndex].ItemArray[2].ToString().Trim() == "I")
                        {
                            dgv.Rows[blankIndex].Cells[floorIndex + 1].Value = 'I';
                            dgv.Rows[blankIndex].Cells[floorIndex + 1].Style.BackColor = Color.MediumSlateBlue;
                        }
                        else if (dt.Rows[blankIndex * (dgv.ColumnCount - 1) + floorIndex].ItemArray[2].ToString().Trim() == "O")
                        {
                            dgv.Rows[blankIndex].Cells[floorIndex + 1].Value = 'O';
                            dgv.Rows[blankIndex].Cells[floorIndex + 1].Style.BackColor = Color.LimeGreen;
                        }
                        else if (dt.Rows[blankIndex * (dgv.ColumnCount - 1) + floorIndex].ItemArray[2].ToString().Trim() == "E")
                        {
                            dgv.Rows[blankIndex].Cells[floorIndex + 1].Value = 'E';
                            dgv.Rows[blankIndex].Cells[floorIndex + 1].Style.BackColor = Color.PaleVioletRed;
                        }
                        else if (dt.Rows[blankIndex * (dgv.ColumnCount - 1) + floorIndex].ItemArray[2].ToString().Trim() == "C")
                        {
                            dgv.Rows[blankIndex].Cells[floorIndex + 1].Value = 'C';
                            dgv.Rows[blankIndex].Cells[floorIndex + 1].Style.BackColor = Color.Green;
                        }
                        else if (dt.Rows[blankIndex * (dgv.ColumnCount - 1) + floorIndex].ItemArray[2].ToString().Trim() == "X")
                        {
                            dgv.Rows[blankIndex].Cells[floorIndex + 1].Value = 'X';
                            dgv.Rows[blankIndex].Cells[floorIndex + 1].Style.BackColor = Color.Red;
                        }
                    }
                }
            }
        }
        #endregion

        private void tm_updateOrder_Tick(object sender, EventArgs e)
        {
            DataGridLoad();
        }

        private void dgv_list11_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

            if (dgv_list11.CurrentCell.RowIndex >= 0 && dgv_list11.CurrentCell.RowIndex < dgv_list11.RowCount && dgv_list11.CurrentCell.ColumnIndex < dgv_list11.ColumnCount && dgv_list11.CurrentCell.ColumnIndex > 0)
            {
                int floorIndex = dgv_list11.CurrentCell.ColumnIndex;
                int blankIndex = dgv_list11.CurrentCell.RowIndex + 1;
                var db = new DBAccess_MySql("MySql");
                string outString = "";
                DataTable dt;
                string sql = "SELECT house_state,before_change FROM house_info WHERE (house_state='N' or house_state='S' or house_state='X') and line=" + 1 + " and list =1 and blank =" + blankIndex + " and floor =" + floorIndex + "";
                var rlt = db.QuerySQL_ToTable(sql, out dt, out outString);
                if (dt.Rows.Count != 0)//没有预约计划的仓库
                {
                    if (dt.Rows[0].ItemArray[0].ToString().Trim() == "X")
                    {
                        //当前是处于禁止使用的状态，那么把该值放到数据库的静止使用表儿里面
                        //更新物品存放表儿里面的数据
                        sql = "UPDATE house_info SET house_state ='" + dt.Rows[0].ItemArray[1].ToString() + "' WHERE line=" + 1 + " and list =1 and blank =" + blankIndex + " and floor =" + floorIndex + "";
                        var rlt2 = db.QuerySQL_ToTable(sql, out dt, out outString);
                    }
                    else if (dt.Rows[0].ItemArray[0].ToString().Trim() == "N" || dt.Rows[0].ItemArray[0].ToString().Trim() == "S")
                    {
                        sql = "UPDATE house_info SET house_state ='X',before_change = '" + dt.Rows[0].ItemArray[0].ToString() + "' WHERE line=" + 1 + " and list =1 and blank =" + blankIndex + " and floor =" + floorIndex + "";
                        var rlt3 = db.QuerySQL_ToTable(sql, out dt, out outString);
                    }
                    this.DataGridLoad();
                }
                else
                {
                    MessageBox.Show("仅适合使用于空储位N、库存储位S、禁用库位X");
                }
            }
        }

        private void dgv_list12_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgv_list12.CurrentCell.RowIndex >= 0 && dgv_list12.CurrentCell.RowIndex < dgv_list12.RowCount && dgv_list12.CurrentCell.ColumnIndex < dgv_list12.ColumnCount && dgv_list12.CurrentCell.ColumnIndex > 0)
            {
                int floorIndex = dgv_list12.CurrentCell.ColumnIndex;
                int blankIndex = dgv_list12.CurrentCell.RowIndex + 1;
                var db = new DBAccess_MySql("MySql");
                string outString = "";
                DataTable dt;
                string sql = "SELECT house_state,before_change FROM house_info WHERE (house_state='N' or house_state='S' or house_state='X') and line=" + 1 + " and list =2 and blank =" + blankIndex + " and floor =" + floorIndex + "";
                var rlt = db.QuerySQL_ToTable(sql, out dt, out outString);
                if (dt.Rows.Count != 0)//没有预约计划的仓库
                {
                    if (dt.Rows[0].ItemArray[0].ToString().Trim() == "X")
                    {
                        //当前是处于禁止使用的状态，那么把该值放到数据库的静止使用表儿里面
                        //更新物品存放表儿里面的数据
                        sql = "UPDATE house_info SET house_state ='" + dt.Rows[0].ItemArray[1].ToString() + "' WHERE line=" + 1 + " and list =2 and blank =" + blankIndex + " and floor =" + floorIndex + "";
                        var rlt2 = db.QuerySQL_ToTable(sql, out dt, out outString);
                    }
                    else if (dt.Rows[0].ItemArray[0].ToString().Trim() == "N" || dt.Rows[0].ItemArray[0].ToString().Trim() == "S")
                    {
                        sql = "UPDATE house_info SET house_state ='X',before_change = '" + dt.Rows[0].ItemArray[0].ToString() + "' WHERE line=" + 1 + " and list =2 and blank =" + blankIndex + " and floor =" + floorIndex + "";
                        var rlt3 = db.QuerySQL_ToTable(sql, out dt, out outString);
                    }
                    this.DataGridLoad();
                }
                else
                {
                    MessageBox.Show("仅适合使用于空储位N、库存储位S、禁用库位X");
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            foreach (var kv in PLCSystem_SRM.Dic_SRM_STSClass)
            {
                string s1 = kv.Value.s_Grid.ToString();
                string s2 = kv.Value.s_Layer.ToString();
                string s3 = kv.Value.s_Pattern.ToString();
                string s12 = kv.Value.s_FrPattern.ToString();
                textBox1.Text = s1;
                textBox2.Text = s2;
                if (s3 == "0")
                {
                    textBox3.Text = "维护";
                }
                else if (s3 == "1")
                {
                    textBox3.Text = "手动";
                }
                else if (s3 == "2")
                {
                    textBox3.Text = "自动";
                }

                if (s12 == "1")
                {
                    textBox12.Text = "出库";
                }
                if (s12 == "2")
                {
                    textBox12.Text = "入库";
                }
                break;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            foreach (var s in PLCSystem_SRM.Dic_SRM_Request)
            {
                SRM_Respone cr = null;

                PLCSystem_SRM.Dic_SRM_Res.TryGetValue(s.Key, out cr);

                //if(comboBox1.Text == "入库")
                //{
                //    cr.s_FrPattern = 2;
                //}
                //if (comboBox1.Text == "出库")
                //{
                //    cr.s_FrPattern = 1;
                //}

                //cr.s_FrStand = (ushort)int.Parse(textBox4.Text);
                //cr.s_FrLine = (ushort)int.Parse(textBox5.Text);
                //cr.s_FrGrid = (ushort)int.Parse(textBox6.Text);
                //cr.s_FrTier = (ushort)int.Parse(textBox7.Text);

                //cr.s_ToStand = (ushort)int.Parse(textBox11.Text);
                //cr.s_ToLine = (ushort)int.Parse(textBox10.Text);
                //cr.s_ToGrid = (ushort)int.Parse(textBox9.Text);
                //cr.s_ToTier = (ushort)int.Parse(textBox8.Text);

                //cr.s_Execute = 1;
                break;
            }
        }

        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.Text.Trim() == "用户维护")
            {
                homeContent.Controls.Clear();
                UserMaintain um = new UserMaintain();
                homeContent.Controls.Add(um);
                um.Dock = DockStyle.Fill;
            }
            //if (e.Node.Text.Trim() == "用户权限")
            //{
            //    homeContent.Controls.Clear();
            //    UserJurisdiction uj = new UserJurisdiction();
            //    homeContent.Controls.Add(uj);
            //    uj.Dock = DockStyle.Fill;
            //}
            //if (e.Node.Text.Trim() == "物品资料")
            //{
            //    homeContent.Controls.Clear();
            //    MerchandiseData md = new MerchandiseData();
            //    homeContent.Controls.Add(md);
            //    md.Dock = DockStyle.Fill;
            //}
            if (e.Node.Text.Trim() == "库位绑定")
            {
                homeContent.Controls.Clear();
                MerchandiseQuery mq = new MerchandiseQuery();
                homeContent.Controls.Add(mq);
                mq.Dock = DockStyle.Fill;
            }
            if (e.Node.Text.Trim() == "当前命令")
            {
                homeContent.Controls.Clear();
                OrderInfo oi = new OrderInfo();
                homeContent.Controls.Add(oi);
                oi.Dock = DockStyle.Fill;
            }
            if (e.Node.Text.Trim() == "历史数据")
            {
                homeContent.Controls.Clear();
                LocationData ld = new LocationData();
                homeContent.Controls.Add(ld);
                ld.Dock = DockStyle.Fill;
            }
            if (e.Node.Text.Trim() == "物料入库")
            {
                homeContent.Controls.Clear();
                OneIn oi = new OneIn();
                homeContent.Controls.Add(oi);
                oi.Dock = DockStyle.Fill;
            }
            if (e.Node.Text.Trim() == "整批入库")
            {
                MessageBox.Show("整批入库");
            }
            if (e.Node.Text.Trim() == "空箱入库")
            {
                MessageBox.Show("空箱入库");
            }
            if (e.Node.Text.Trim() == "物料出库")
            {
                homeContent.Controls.Clear();
                OneOut oo = new OneOut();
                homeContent.Controls.Add(oo);
                oo.Dock = DockStyle.Fill;
            }
            if (e.Node.Text.Trim() == "整批出库")
            {
                MessageBox.Show("整批出库");
            }
            if (e.Node.Text.Trim() == "日（月）报表")
            {
                MessageBox.Show("日（月）报表");
            }
            if (e.Node.Text.Trim() == "储位数据报表")
            {
                homeContent.Controls.Clear();
                LocationDataReport ldr = new LocationDataReport();
                homeContent.Controls.Add(ldr);
                ldr.Dock = DockStyle.Fill;
            }
            if (e.Node.Text.Trim() == "库对库")
            {
                MessageBox.Show("库对 库");
            }
            if (e.Node.Text.Trim() == "站对站")
            {
                MessageBox.Show("站对站");
            }
            if (e.Node.Text.Trim() == "库存盘点")
            {
                homeContent.Controls.Clear();
                TakeStock ts = new TakeStock();
                homeContent.Controls.Add(ts);
                ts.Dock = DockStyle.Fill;
            }
            if (e.Node.Text.Trim() == "拣料物料")
            {
                homeContent.Controls.Clear();
                PickMaterial pm = new PickMaterial();
                homeContent.Controls.Add(pm);
                pm.Dock = DockStyle.Fill;
            }
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
        }
    }
}

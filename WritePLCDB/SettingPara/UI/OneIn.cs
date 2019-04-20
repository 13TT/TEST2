using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using SettingPara.Wdata;

namespace SettingPara.UI
{
    public partial class OneIn : UserControl
    {
        private string sql;

        public OneIn()
        {
            InitializeComponent();
        }

        public void update_goodInfo()
        {
            DataTable dt;
            var db = new DBAccess_MySql("MySql");
            dataGridView1.Columns.Clear();
            string outString = "";
            sql = "SELECT gi.goods_id,gi.goods_name,gi.goods_code,gi.goods_style FROM goods_info gi,house_info hi WHERE gi.goods_style = hi.house_number AND hi.house_state = 'N'";
            var rlt = db.QuerySQL_ToTable(sql, out dt, out outString);
            dataGridView1.DataSource = dt;
            dataGridView1.AlternatingRowsDefaultCellStyle.BackColor = Color.LightBlue;
            dataGridView1.Columns[0].HeaderText = "物料编号";
            dataGridView1.Columns[1].HeaderText = "物料名称";
            dataGridView1.Columns[2].HeaderText = "物料规格";
            dataGridView1.Columns[3].HeaderText = "库位编号";
        }

        private void OneIn_Load(object sender, EventArgs e)
        {
            update_goodInfo();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //创建入库预约
            try
            {
                DataTable dt;
                var db = new DBAccess_MySql("MySql");
                dataGridView1.Columns.Clear();
                string outString = "";
                string houseNumber = "";
                int line = 0;
                int list = 0;
                int blank = 0;
                int floor = 0;
                sql = "SELECT house_number,line,list,blank,floor FROM house_info WHERE house_number = '" + comboBox1.Text + "'";
                var rlt = db.QuerySQL_ToTable(sql, out dt, out outString);
                if (dt == null || dt.Rows.Count <= 0) return;
                houseNumber = dt.Rows[0]["house_number"].ToString();
                line = int.Parse(dt.Rows[0]["line"].ToString());
                list = int.Parse(dt.Rows[0]["list"].ToString());
                blank = int.Parse(dt.Rows[0]["blank"].ToString());
                floor = int.Parse(dt.Rows[0]["floor"].ToString());

                foreach (var s in PLCSystem_SRM.Dic_SRM_Request)
                {
                    SRM_Respone cr = null;

                    PLCSystem_SRM.Dic_SRM_Res.TryGetValue(s.Key, out cr);

                    //if (button1.Text == "入库")
                    //{
                    //    cr.s_FrPattern = 2;
                    //}
                    //if (button1.Text == "出库")
                    //{
                    //    cr.s_FrPattern = 1;
                    //}

                    //cr.s_ToStand = 0;
                    //cr.s_FrLine = 0;
                    //cr.s_FrGrid = 0;
                    //cr.s_FrTier = 0;

                    //cr.s_FrStand = 2;
                    //cr.s_ToLine = (ushort)list;
                    //cr.s_ToGrid = (ushort)blank;
                    //cr.s_ToTier = (ushort)floor;

                    //cr.s_Execute = 1;
                    //cr.s_Fault = 1;
                    break;
                }
                //跟新货位状态
                sql = "UPDATE house_info SET house_state = 'S' WHERE house_number = '" + houseNumber + "'";
                rlt = db.ExecSql(sql, out outString);
                //更新库存数据
                sql = "INSERT INTO house_data(house_number,goods_code,goods_name,have_number,in_house_time) VALUES('" + houseNumber + "','" + tb_suppliesName.Text + "','" + tb_specification.Text + "'," + int.Parse(textBox1.Text) + ",SYSDATE())";
                rlt = db.ExecSql(sql, out outString);
                update_goodInfo();
                comboBox1.Text = "";
            }
            catch
            {
                MessageBox.Show("操作不正确！");
            }
        }

        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                lbID.Text = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                tb_sort.Text = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
                tb_suppliesName.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
                tb_specification.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
                //dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                //dataGridView1.DefaultCellStyle.SelectionForeColor = Color.Blue;
                //dataGridView1.DefaultCellStyle.SelectionBackColor = Color.LightYellow;

                //DataTable dt;
                //var db = new DBAccess_MySql("MySql");
                //dataGridView1.Columns.Clear();
                //string outString = "";
                //sql = "SELECT goods_code,goods_name,goods_style FROM goods_info WHERE goods_id = "+int.Parse(lbID.Text)+"";
                //var rlt = db.QuerySQL_ToTable(sql, out dt, out outString);
                //if (dt == null || dt.Rows.Count <= 0) return;
                //tb_specification.Text = dt.Rows[0]["goods_code"].ToString();
                //tb_suppliesName.Text = dt.Rows[0]["goods_name"].ToString();
                //tb_sort.Text = dt.Rows[0]["goods_style"].ToString();
            }
            catch
            {

            }
            //update_goodInfo();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("库位编号：" + tb_sort.Text + "  物料名称：" + tb_suppliesName.Text + "  物料规格：" + tb_specification.Text, "确认入库！", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                #region //创建入库预约

                try
                {
                    DataTable dt;
                    var db = new DBAccess_MySql("MySql");
                    dataGridView1.Columns.Clear();
                    string outString = "";
                    string houseNumber = "";
                    int line = 0;
                    int list = 0;
                    int blank = 0;
                    int floor = 0;
                    sql = "SELECT house_number,line,list,blank,floor FROM house_info WHERE house_state = 'N' and house_number = '" + tb_sort.Text + "'";
                    var rlt = db.QuerySQL_ToTable(sql, out dt, out outString);
                    if (dt == null || dt.Rows.Count <= 0) return;
                    houseNumber = dt.Rows[0]["house_number"].ToString();
                    line = int.Parse(dt.Rows[0]["line"].ToString());
                    list = int.Parse(dt.Rows[0]["list"].ToString());
                    blank = int.Parse(dt.Rows[0]["blank"].ToString());
                    floor = int.Parse(dt.Rows[0]["floor"].ToString());

                    //foreach (var s in PLCSystem_SRM.Dic_SRM_Request)
                    //{
                    //    s.Value.s_serial = 1;
                    //    s.Value.s_pattern = 2;

                    //    if (tb_suppliesName.Text == "端拾器")
                    //    {
                    //        s.Value.s_FrStand = 1;
                    //    }

                    //    if (tb_suppliesName.Text == "检具")
                    //    {
                    //        s.Value.s_FrStand = 2;
                    //    }

                    //    s.Value.s_FrLine = 0;
                    //    s.Value.s_FrGrid = 0;
                    //    s.Value.s_FrTier = 0;

                    //    s.Value.s_ToStand = 0;
                    //    s.Value.s_ToLine = (ushort)list;
                    //    s.Value.s_ToGrid = (ushort)blank;
                    //    s.Value.s_ToTier = (ushort)floor;

                    //    s.Value.s_execute_signal = 0;

                    //    //break;
                    //}
                    ////跟新货位状态
                    //sql = "UPDATE house_info SET house_state = 'S' WHERE house_number = '" + houseNumber + "'";
                    //rlt = db.ExecSql(sql, out outString);
                    ////更新库存数据
                    //sql = "INSERT INTO house_data(house_number,goods_code,goods_name,have_number,in_house_time) VALUES('" + houseNumber + "','" + tb_suppliesName.Text + "','" + tb_specification.Text + "'," + int.Parse(textBox1.Text) + ",SYSDATE())";
                    //rlt = db.ExecSql(sql, out outString);
                    //update_goodInfo();
                    //comboBox1.Text = "";
                    sql = "INSERT INTO order_info(order_state,order_style,station_from,station_to,house_from,house_to,order_user,order_time) VALUES(0,1,1,0,'" + tb_sort.Text + "','0'," + int.Parse(lbID.Text) + ",SYSDATE())";
                    db.ExecSql(sql, out outString);

                    //string str = @"Server=PLCWMSDBCONNECT;Database=PLCWMSDB;User ID=sa;Password=123456";
                    //SqlConnection conn = new SqlConnection(str);
                    //SqlCommand cmd = conn.CreateCommand();

                    //cmd.CommandText = "INSERT INTO dbo.Filtering(FilLocId) values(@FilLocId)";
                    //cmd.Parameters.Add(new SqlParameter("@FilLocId", LocId));

                    //conn.Open();
                    //cmd.ExecuteNonQuery();

                    //conn.Close();

                    sql = "UPDATE house_info SET house_state = 'I' WHERE house_number = '" + tb_sort.Text + "'";
                    db.ExecSql(sql, out outString);
                }
                catch
                {
                    MessageBox.Show("操作不正确！");
                }
                update_goodInfo();
                #endregion
            }
            else
            {
                return;
            }

        }

        private void comboBox1_DropDown(object sender, EventArgs e)
        {
            DataTable dt;
            var db = new DBAccess_MySql("MySql");
            dataGridView1.Columns.Clear();
            string outString = "";
            sql = "SELECT house_number FROM house_info WHERE house_state = 'N'";
            var rlt = db.QuerySQL_ToTable(sql, out dt, out outString);
            comboBox1.DataSource = dt;
            comboBox1.DisplayMember = "house_number";
            comboBox1.Text = "";
            update_goodInfo();
        }

        private void tb_suppliesName_TextChanged(object sender, EventArgs e)
        {
            //dataGridView1.Columns.Clear();
            //seleteGoodsData("SELECT gi.goods_id,gi.goods_name,gi.goods_code,gi.goods_style FROM goods_info gi,house_info hi WHERE gi.goods_style = hi.house_number AND hi.house_state = 'N' AND gi.goods_name LIKE '%"+tb_suppliesName.Text+"%'");
        }

        private void tb_specification_TextChanged(object sender, EventArgs e)
        {
            dataGridView1.Columns.Clear();
            seleteGoodsData("SELECT gi.goods_id,gi.goods_name,gi.goods_code,gi.goods_style FROM goods_info gi,house_info hi WHERE gi.goods_style = hi.house_number AND hi.house_state = 'N' AND gi.goods_code LIKE '%" + tb_specification.Text + "%'");
        }

        private void seleteGoodsData(string sql)
        {
            DataTable dt;
            var db = new DBAccess_MySql("MySql");
            dataGridView1.Columns.Clear();
            string outString = "";
            var rlt = db.QuerySQL_ToTable(sql, out dt, out outString);
            dataGridView1.DataSource = dt;
            dataGridView1.AlternatingRowsDefaultCellStyle.BackColor = Color.LightBlue;
            dataGridView1.Columns[0].HeaderText = "物料编号";
            dataGridView1.Columns[1].HeaderText = "物料名称";
            dataGridView1.Columns[2].HeaderText = "物料规格";
            dataGridView1.Columns[3].HeaderText = "库位编号";
        }
        
        WOrderInfo w = new WOrderInfo();
        private void btn_tin_Click(object sender, EventArgs e)
        {
            //w.Write();
            //int a = w.Median();
            w.Account();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            w.ClearAccount();
        }
    }
}

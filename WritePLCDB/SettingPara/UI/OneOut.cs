﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SettingPara.UI
{
    public partial class OneOut : UserControl
    {
        private string sql;

        public OneOut()
        {
            InitializeComponent();
        }

        public void update_houseInfo()
        {
            DataTable dt;
            var db = new DBAccess_MySql("MySql");
            dataGridView1.Columns.Clear();
            string outString = "";
            sql = "SELECT house_number,goods_code,goods_name,in_house_time,have_number FROM house_data";
            var rlt = db.QuerySQL_ToTable(sql, out dt, out outString);
            dataGridView1.DataSource = dt;
            dataGridView1.AlternatingRowsDefaultCellStyle.BackColor = Color.LightBlue;
            dataGridView1.Columns[0].HeaderText = "库位编号";
            dataGridView1.Columns[1].HeaderText = "物料名称";
            dataGridView1.Columns[2].HeaderText = "物料规格";
            dataGridView1.Columns[3].HeaderText = "入库时间";
            dataGridView1.Columns[4].HeaderText = "物料数量";
        }

        private void OneOut_Load(object sender, EventArgs e)
        {
            update_houseInfo();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //创建出库预约
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

                    //cr.s_FrStand = 0;
                    //cr.s_FrLine = (ushort)list;
                    //cr.s_FrGrid = (ushort)blank;
                    //cr.s_FrTier = (ushort)floor;

                    //cr.s_ToStand = 2;
                    //cr.s_ToLine = 0;
                    //cr.s_ToGrid = 0;
                    //cr.s_ToTier = 0;

                    //cr.s_Execute = 1;
                    ////cr.s_Fault = 1;
                    //break;
                }
                //跟新货位状态
                sql = "UPDATE house_info SET house_state = 'N' WHERE house_number = '" + houseNumber + "'";
                rlt = db.ExecSql(sql, out outString);
                //更新库存数据
                sql = "DELETE FROM house_data WHERE house_number = '" + houseNumber + "'";
                rlt = db.ExecSql(sql, out outString);
                update_houseInfo();
                comboBox1.Text = "";
            }
            catch
            {
                MessageBox.Show("存在操作错误！");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("库位编号：" + tb_sort.Text + "  物料名称：" + tb_suppliesName.Text + "  物料规格：" + tb_specification.Text, "确认出库！", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                #region //创建出库预约
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
                    sql = "SELECT house_number,line,list,blank,floor FROM house_info WHERE house_number = '" + tb_sort.Text + "'";
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
                    //    s.Value.s_pattern = 1;

                    //    s.Value.s_FrStand = 0;
                    //    s.Value.s_FrLine = (ushort)list;
                    //    s.Value.s_FrGrid = (ushort)blank;
                    //    s.Value.s_FrTier = (ushort)floor;



                    //    if (tb_suppliesName.Text == "端拾器")
                    //    {
                    //        s.Value.s_ToStand = 1;
                    //    }

                    //    if (tb_suppliesName.Text == "检具")
                    //    {
                    //        s.Value.s_ToStand = 2;
                    //    }

                    //    s.Value.s_ToLine = 0;
                    //    s.Value.s_ToGrid = 0;
                    //    s.Value.s_ToTier = 0;

                    //    s.Value.s_execute_signal = 0;

                    //    //break;
                    //}
                    sql = "SELECT goods_id FROM goods_info WHERE goods_name = '" + tb_suppliesName.Text + "' AND goods_code = '" + tb_specification.Text + "'";
                    db.QuerySQL_ToTable(sql, out dt, out outString);
                    DataRow dd = dt.Rows[0];
                    string id = dd[0].ToString();

                    sql = "INSERT INTO order_info(order_state,order_style,station_from,station_to,house_from,house_to,order_user,order_time) VALUES(0,2,1,0,'0','" + tb_sort.Text + "'," + int.Parse(id) + ",SYSDATE())";
                    db.ExecSql(sql, out outString);

                    sql = "UPDATE house_info SET house_state = 'O' WHERE house_number = '" + tb_sort.Text + "'";
                    db.ExecSql(sql, out outString);

                    //删除
                    sql = "DELETE FROM house_data WHERE house_number = '" + tb_sort.Text + "'";
                    db.ExecSql(sql, out outString);
                    update_houseInfo();
                }
                catch
                {
                    MessageBox.Show("存在操作错误！");
                }
                #endregion
            }
            else
            {
                return;
            }
        }

        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                tb_sort.Text = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                tb_suppliesName.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
                tb_specification.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
                //textBox1.Text = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();
                //dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                //dataGridView1.DefaultCellStyle.SelectionForeColor = Color.Blue;
                //dataGridView1.DefaultCellStyle.SelectionBackColor = Color.LightYellow;
            }
            catch
            {
               
            }
            //update_houseInfo();
        }

        private void comboBox1_DropDown(object sender, EventArgs e)
        {
            DataTable dt;
            var db = new DBAccess_MySql("MySql");
            dataGridView1.Columns.Clear();
            string outString = "";
            sql = "SELECT house_number FROM house_data";
            var rlt = db.QuerySQL_ToTable(sql, out dt, out outString);
            comboBox1.DataSource = dt;
            comboBox1.DisplayMember = "house_number";
            comboBox1.Text = "";
            update_houseInfo();
        }

        private void tb_suppliesName_TextChanged(object sender, EventArgs e)
        {
            //dataGridView1.Columns.Clear();
            //seleteGoodsData("SELECT house_number,goods_code,goods_name,in_house_time,have_number FROM house_data WHERE goods_code LIKE '%" + tb_suppliesName.Text+"%'");
        }

        private void tb_specification_TextChanged(object sender, EventArgs e)
        {
            dataGridView1.Columns.Clear();
            seleteGoodsData("SELECT house_number,goods_code,goods_name,in_house_time,have_number FROM house_data WHERE goods_name LIKE '%" + tb_specification.Text + "%'");
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
            dataGridView1.Columns[0].HeaderText = "库位编号";
            dataGridView1.Columns[1].HeaderText = "物料名称";
            dataGridView1.Columns[2].HeaderText = "物料规格";
            dataGridView1.Columns[3].HeaderText = "入库时间";
            dataGridView1.Columns[4].HeaderText = "物料数量";
        }
    }
}

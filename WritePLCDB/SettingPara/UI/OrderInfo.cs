using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SettingPara.UI
{
    public partial class OrderInfo : UserControl
    {
        private string sql;

        public OrderInfo()
        {
            InitializeComponent();
        }
        public void update_orderInfo()
        {
            DataTable dt;
            var db = new DBAccess_MySql("MySql");
            dataGridView1.Columns.Clear();
            string outString = "";
            sql = "SELECT order_id,order_state,order_style,station_from,station_to,house_from,house_to,order_user,order_time FROM order_info WHERE order_state = 0 or order_state = 1";
            var rlt = db.QuerySQL_ToTable(sql, out dt, out outString);
            dataGridView1.DataSource = dt;
            dataGridView1.AlternatingRowsDefaultCellStyle.BackColor = Color.LightBlue;
            dataGridView1.Columns[0].HeaderText = "命令编号";
            dataGridView1.Columns[1].HeaderText = "命令状态";
            dataGridView1.Columns[2].HeaderText = "命令类别";
            dataGridView1.Columns[3].HeaderText = "站号来源";
            dataGridView1.Columns[4].HeaderText = "站号目的";
            dataGridView1.Columns[5].HeaderText = "库位来源";
            dataGridView1.Columns[6].HeaderText = "库位目的";
            dataGridView1.Columns[7].HeaderText = "物料编号";
            dataGridView1.Columns[8].HeaderText = "创建时间";
        }

        private void OrderInfo_Load(object sender, EventArgs e)
        {
            update_orderInfo();
        }

        private void btnAlter_Click(object sender, EventArgs e)
        {
            var db = new DBAccess_MySql("MySql");
            string outString = "";
            DataTable dt;
            //执行中不能强制过账
            if (tb_suppliesName.Text == "0")
            {
                //获取数据指令数据
                sql = "SELECT order_id,order_state,order_style,station_from,station_to,house_from,house_to,order_user,order_time FROM order_info WHERE order_id = "+ int.Parse(tb_goodsid.Text)+"";
                var rlt = db.QuerySQL_ToTable(sql, out dt, out outString);
                DataRow dr = dt.Rows[0];
                string order_state = dr[1].ToString();
                string order_style = dr[2].ToString();
                string station_from = dr[3].ToString();
                string station_to = dr[4].ToString();
                string house_from = dr[5].ToString();
                string house_to = dr[6].ToString();
                string order_user = dr[7].ToString();

                //分解物料
                sql = "SELECT goods_code,goods_name,goods_style FROM goods_info WHERE goods_id = " + int.Parse(order_user) + "";
                db.QuerySQL_ToTable(sql, out dt, out outString);
                DataRow dd = dt.Rows[0];
                string goods_code = dd[0].ToString();
                string goods_name = dd[1].ToString();
                string goods_style = dd[2].ToString();

                if (order_style == "1")//入库    order_state = 3 完成   order_state = 2 未完成
                {
                    ////跟新货位状态
                    sql = "UPDATE house_info SET house_state = 'S' WHERE house_number = '" + goods_style + "'";
                    db.ExecSql(sql, out outString);
                    //更新库存数据
                    sql = "INSERT INTO house_data(house_number,goods_code,goods_name,have_number,in_house_time) VALUES('" + goods_style + "','" + goods_name + "','" + goods_code + "'," + 1 + ",SYSDATE())";
                    db.ExecSql(sql, out outString);
                    //修改
                    sql = "UPDATE order_info SET order_state = 3 WHERE order_id = " + int.Parse(tb_goodsid.Text) + "";
                    db.ExecSql(sql, out outString);

                    ////完成后生成历史清单
                    //sql = "INSERT INTO doc_info(doc_id,doc_style,send_user,rec_user,doc_time) VALUES('" + int.Parse(tb_goodsid.Text) + "','" + goods_style + "','完成','入库',SYSDATE())";
                    //db.ExecSql(sql, out outString);
                }
                if (order_style == "2")//出库
                {
                    ////跟新货位状态
                    sql = "UPDATE house_info SET house_state = 'N' WHERE house_number = '" + house_to + "'";
                    db.ExecSql(sql, out outString);
                    
                    //修改
                    sql = "DELETE FROM house_data WHERE house_number = '" + house_to + "'";
                    db.ExecSql(sql, out outString);

                    sql = "UPDATE order_info SET order_state = 3 WHERE order_id = " + int.Parse(tb_goodsid.Text) + "";
                    db.ExecSql(sql, out outString);

                    ////完成后生成历史清单
                    //sql = "INSERT INTO doc_info(doc_id,doc_style,send_user,rec_user,doc_time) VALUES('" + int.Parse(tb_goodsid.Text) + "','" + house_to + "','完成','出库',SYSDATE())";
                    //db.ExecSql(sql, out outString);
                }
            }
            else
            {
                MessageBox.Show("不满足强制过账条件,运行中！");
            }
            update_orderInfo();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            //执行中不能强制删除
            var db = new DBAccess_MySql("MySql");
            string outString = "";
            DataTable dt;
            if (tb_suppliesName.Text == "0")
            {
                //获取数据指令数据
                sql = "SELECT order_id,order_state,order_style,station_from,station_to,house_from,house_to,order_user,order_time FROM order_info WHERE order_id = " + int.Parse(tb_goodsid.Text) + "";
                var rlt = db.QuerySQL_ToTable(sql, out dt, out outString);
                DataRow dr = dt.Rows[0];
                string order_state = dr[1].ToString();
                string order_style = dr[2].ToString();
                string station_from = dr[3].ToString();
                string station_to = dr[4].ToString();
                string house_from = dr[5].ToString();
                string house_to = dr[6].ToString();
                string order_user = dr[7].ToString();

                if(order_style == "1")//入库
                {
                    sql = "UPDATE order_info SET order_state = 2 WHERE order_id = " + int.Parse(tb_goodsid.Text) + "";
                    db.ExecSql(sql, out outString);

                    sql = "UPDATE house_info SET house_state = 'N' WHERE house_number = '" + house_from + "'";
                    db.ExecSql(sql, out outString);

                    ////完成后生成历史清单
                    //sql = "INSERT INTO doc_info(doc_id,doc_style,send_user,rec_user,doc_time) VALUES('" + int.Parse(tb_goodsid.Text) + "','" + house_from + "','未完成','入库',SYSDATE())";
                    //db.ExecSql(sql, out outString);
                }
                if (order_style == "2")//出库
                {
                    sql = "UPDATE order_info SET order_state = 2 WHERE order_id = " + int.Parse(tb_goodsid.Text) + "";
                    db.ExecSql(sql, out outString);

                    sql = "UPDATE house_info SET house_state = 'S' WHERE house_number = '" + house_to + "'";
                    db.ExecSql(sql, out outString);

                    sql = "SELECT goods_code,goods_name,goods_style FROM goods_info WHERE goods_style = " + house_to + "";
                    db.QuerySQL_ToTable(sql, out dt, out outString);
                    DataRow dd = dt.Rows[0];
                    string goods_code = dd[0].ToString();
                    string goods_name = dd[1].ToString();
                    string goods_style = dd[2].ToString();

                    //更新库存数据
                    sql = "INSERT INTO house_data(house_number,goods_code,goods_name,have_number,in_house_time) VALUES('" + house_to + "','" + goods_name + "','" + goods_code + "'," + 1 + ",SYSDATE())";
                    db.ExecSql(sql, out outString);

                    //完成后生成历史清单
                    //sql = "INSERT INTO doc_info(doc_id,doc_style,send_user,rec_user,doc_time) VALUES('" + int.Parse(tb_goodsid.Text) + "','" + house_from + "','未完成','出库',SYSDATE())";
                    //db.ExecSql(sql, out outString);
                }
            }
            else
            {
                MessageBox.Show("不满足删除条件,运行中！");
            }
            update_orderInfo();
        }

        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                tb_sort.Text = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
                tb_suppliesName.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
                tb_goodsid.Text = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
            }
            catch
            {

            }
        }
    }
}

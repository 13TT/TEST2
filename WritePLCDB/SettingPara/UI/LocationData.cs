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
    public partial class LocationData : UserControl
    {
        private string sql;

        public LocationData()
        {
            InitializeComponent();
        }

        public void update_houseData()
        {
            DataTable dt;
            var db = new DBAccess_MySql("MySql");
            dataGridView1.Columns.Clear();
            string outString = "";
            //sql = "SELECT * FROM doc_info";
            sql = "SELECT order_id,order_state,order_style,house_from,house_to,order_time FROM order_info WHERE order_state = 3 OR order_state = 2 ORDER BY order_id DESC LIMIT 500";
            var rlt = db.QuerySQL_ToTable(sql, out dt, out outString);
            dataGridView1.DataSource = dt;
            dataGridView1.AlternatingRowsDefaultCellStyle.BackColor = Color.LightBlue;
            dataGridView1.Columns[0].HeaderText = "指令编号";
            dataGridView1.Columns[1].HeaderText = "指令状态";
            dataGridView1.Columns[2].HeaderText = "指令类型";
            dataGridView1.Columns[3].HeaderText = "库位编号";
            dataGridView1.Columns[4].HeaderText = "库位编号";
            dataGridView1.Columns[5].HeaderText = "时间日期";
        }

        private void LocationData_Load(object sender, EventArgs e)
        {
            update_houseData();
        }

        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataTable dt;
            string sql;
            try
            {
                //tb_houseNumber.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
                //string s = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
                var db = new DBAccess_MySql("MySql");
                string outString = "";
                //sql = "SELECT goods_code FROM goods_info WHERE goods_style = '"+ tb_houseNumber.Text + "'";
                //var rlt = db.QuerySQL_ToTable(sql, out dt, out outString);
                //DataRow dr = dt.Rows[0];
                //string goods_code = dr[0].ToString();
                //tb_sort.Text = goods_code;
                string housenumber;
                housenumber = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
                
                if (housenumber != "0")
                {
                    tb_houseNumber.Text = housenumber;
                }
                else
                {
                    tb_houseNumber.Text = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();
                }
                sql = "SELECT goods_code FROM goods_info WHERE goods_style = '" + tb_houseNumber.Text + "'";
                var rlt = db.QuerySQL_ToTable(sql, out dt, out outString);
                DataRow dr = dt.Rows[0];
                string goods_code = dr[0].ToString();
                tb_sort.Text = goods_code;

                string state = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
                string style = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();

                if (state == "3")
                {
                    tb_state.Text = "完成";
                }
                if(state == "2")
                {
                    tb_state.Text = "未完成";
                }
                if (style == "1")
                {
                    tb_style.Text = "入库";
                }
                if (style == "2")
                {
                    tb_style.Text = "出库";
                }
            }
            catch
            {

            }
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}

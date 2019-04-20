using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Office.Interop.Excel;

namespace SettingPara.UI
{
    public partial class MerchandiseQuery : UserControl
    {
        private string sql;
        int i = 0;

        public MerchandiseQuery()
        {
            InitializeComponent();
        }

        public void update_houseData()
        {
            System.Data.DataTable dt;
            var db = new DBAccess_MySql("MySql");
            dataGridView1.Columns.Clear();
            string outString = "";
            sql = "SELECT goods_id,goods_name,goods_code,goods_height,goods_style,modify_time FROM goods_info";
            var rlt = db.QuerySQL_ToTable(sql, out dt, out outString);
            dataGridView1.DataSource = dt;
            dataGridView1.AlternatingRowsDefaultCellStyle.BackColor = Color.LightBlue;
            //dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            //dataGridView1.DefaultCellStyle.SelectionForeColor = Color.Blue;
            //dataGridView1.DefaultCellStyle.SelectionBackColor = Color.LightYellow;
            dataGridView1.Columns[0].HeaderText = "物料编号";
            dataGridView1.Columns[1].HeaderText = "物料名称";
            dataGridView1.Columns[2].HeaderText = "物料规格";
            dataGridView1.Columns[3].HeaderText = "物料高度";
            dataGridView1.Columns[4].HeaderText = "库位号";
            dataGridView1.Columns[5].HeaderText = "创建时间";
            
        }

        private string SelectState(string name)
        {
            try
            {
                System.Data.DataTable dt;
                var db = new DBAccess_MySql("MySql");
                string outString = "";
                sql = "SELECT house_state FROM house_info WHERE house_number = '" + name + "'";
                var rlt = db.QuerySQL_ToTable(sql, out dt, out outString);
                DataRow dr = dt.Rows[0];
                string state = dr[0].ToString();
                return state;
            }
            catch
            {
                return name;
            }
        }

        private void MerchandiseQuery_Load(object sender, EventArgs e)
        {
            update_houseData();
            //tb_specification.ReadOnly = true;


            System.Data.DataTable dt;
            var db = new DBAccess_MySql("MySql");
            string outString = "";

            sql = "SELECT * FROM config_unit";
            var rlt1 = db.QuerySQL_ToTable(sql, out dt, out outString);
            DataRow dr = dt.Rows[0];
            string s = dr[0].ToString();
            string name = dr[0].ToString();

            if (name == "S")
            {
                btnBinding.Visible = true;
                btn_cancel.Visible = true;
                btn_updata.Visible = true;
            }
            else
            {
                btnBinding.Visible = false;
                btn_cancel.Visible = false;
                btn_updata.Visible = false;
            }
        }

        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            
           
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("名称：" + tb_suppliesName.Text + "  规格：" + tb_specification.Text + "  高度：" + tb_sort.Text + "  绑定库位："+cb_houseNumber.Text, "绑定库位！", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                //MessageBox.Show(textBox1.Text);
                //DataTable dt;
                var db = new DBAccess_MySql("MySql");
                string outString = "";
                //sql = "SELECT bf_h_to_h,house_number,house_state,house_style FROM house_info WHERE bf_h_to_h = '" + tb_sort.Text+"' AND house_style LIKE '%"+(tb_suppliesName.Text).Substring(1,1)+ "%' AND house_state = 'N'";
                //var rlt = db.QuerySQL_ToTable(sql, out dt, out outString);
                //DataRow dr = dt.Rows[0];
                //string house_number = dr[1].ToString();
                //string house_state = dr[2].ToString();
                //if (cb_houseNumber.Text==house_number)
                //{
                //    sql = "UPDATE goods_info SET goods_style = '"+house_number+"' WHERE goods_id = "+label7.Text+"";
                //    db.ExecSql(sql, out outString);
                //    update_houseData();
                //}
                sql = "UPDATE goods_info SET goods_style = '" + cb_houseNumber.Text + "' WHERE goods_id = " + label7.Text + "";
                db.ExecSql(sql, out outString);
                try
                {
                    sql = "UPDATE house_data SET house_number = '" + cb_houseNumber.Text + "' WHERE goods_name = '" + tb_specification.Text + "'";
                    db.ExecSql(sql, out outString);
                }
                catch
                {

                }

                //修改仓库库存数据

                update_houseData();
            }
            
        }

        private void btn_updata_Click(object sender, EventArgs e)
        {
            System.Data.DataTable dt;
            if (MessageBox.Show("名称：" + tb_suppliesName.Text + "  规格：" + tb_specification.Text + "  高度：" + tb_sort.Text, "确定修改！", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                var db = new DBAccess_MySql("MySql");
                string outString = "";
                sql = "UPDATE goods_info SET goods_code = '" + tb_specification.Text + "',goods_name = '" + tb_suppliesName.Text + "' WHERE goods_id = " + label7.Text + "";
                db.ExecSql(sql, out outString);
                try
                {
                    sql = "SELECT goods_code,goods_name,goods_style FROM goods_info WHERE goods_id = "+ label7.Text + "";
                    var rlt = db.QuerySQL_ToTable(sql, out dt, out outString);
                    DataRow dr = dt.Rows[0];
                    string goods_code = dr[0].ToString();
                    string goods_name = dr[1].ToString();
                    string goods_style = dr[2].ToString();

                    sql = "UPDATE house_data SET goods_code = '" + goods_name + "',goods_name = '" + goods_code + "' WHERE house_number = '" + goods_style + "'";
                    db.ExecSql(sql, out outString);
                }
                catch
                {

                }
            }
            update_houseData();
        }

        private void cb_houseNumber_DropDown(object sender, EventArgs e)
        {
            try
            {
                System.Data.DataTable dt;
                var db = new DBAccess_MySql("MySql");
                string outString = "";
                //sql = "SELECT bf_h_to_h,house_number,house_state,house_style FROM house_info WHERE bf_h_to_h = '" + tb_sort.Text + "' AND house_style LIKE '%" + (tb_suppliesName.Text).Substring(1, 1) + "%' AND house_state = 'N' AND before_change = 'N'";
                sql = "SELECT bf_h_to_h,house_number,house_state,house_style FROM house_info WHERE before_change = 'N' OR before_change = 'B' ";
                var rlt = db.QuerySQL_ToTable(sql, out dt, out outString);
                DataRow dr = dt.Rows[0];
                string house_number = dr[1].ToString();
                string house_state = dr[2].ToString();
                cb_houseNumber.DataSource = dt;
                cb_houseNumber.DisplayMember = "house_number";
            }
            catch
            {
                MessageBox.Show("请填入物料信息！");
            }
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            //if (MessageBox.Show("名称：" + tb_suppliesName.Text + "  规格：" + tb_specification.Text + "  高度：" + tb_sort.Text + "  绑定库位：" + cb_houseNumber.Text, "取消绑定！", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            //{
            //    if (label8.Text == "N")
            //    {
            //        var db = new DBAccess_MySql("MySql");
            //        string outString = "";
            //        sql = "UPDATE goods_info SET goods_style = '' WHERE goods_id = " + label7.Text + "";
            //        db.ExecSql(sql, out outString);
            //        sql = "UPDATE house_info SET before_change = 'N' WHERE house_number = '" + cb_houseNumber.Text + "'";
            //        db.ExecSql(sql, out outString);
            //        update_houseData();
            //    }
            //    else
            //    {
            //        MessageBox.Show("物料预约中不能修改！！");
            //    }
            //}

            if (dataGridView1.Rows.Count == 0)
            {
                return;
            }
            Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
            excel.Application.Workbooks.Add(true);
            excel.Visible = true;
            Range allColumn = excel.Columns;
            allColumn.ColumnWidth = 15;
            for (int i = 0; i < dataGridView1.ColumnCount; i++)
            {
                excel.Cells[1, i + 1] = dataGridView1.Columns[i].HeaderText;
            }
            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                for (int j = 0; j < dataGridView1.ColumnCount; j++)
                {
                    if (dataGridView1[j, i].ValueType == typeof(string))
                    {
                        excel.Cells[i + 2, j + 1] = "'" + dataGridView1[j, i].Value.ToString();
                    }
                    else
                    {
                        excel.Cells[i + 2, j + 1] = dataGridView1[j, i].Value.ToString();
                    }
                }
            }
        }

        private void tb_specification_TextChanged(object sender, EventArgs e)
        {
            dataGridView1.Columns.Clear();
            seleteGoodsData("SELECT goods_id,goods_name,goods_code,goods_height,goods_style,modify_time FROM goods_info WHERE goods_code LIKE '%" + tb_specification.Text + "%'");
        }
        private void seleteGoodsData(string sql)
        {
            System.Data.DataTable dt;
            var db = new DBAccess_MySql("MySql");
            dataGridView1.Columns.Clear();
            string outString = "";
            //sql = "SELECT goods_id,goods_name,goods_code,goods_height,goods_style,modify_time FROM goods_info";
            var rlt = db.QuerySQL_ToTable(sql, out dt, out outString);
            dataGridView1.DataSource = dt;
            dataGridView1.AlternatingRowsDefaultCellStyle.BackColor = Color.LightBlue;
            dataGridView1.Columns[0].HeaderText = "物料编号";
            dataGridView1.Columns[1].HeaderText = "物料名称";
            dataGridView1.Columns[2].HeaderText = "物料规格";
            dataGridView1.Columns[3].HeaderText = "物料高度";
            dataGridView1.Columns[4].HeaderText = "库位号";
            dataGridView1.Columns[5].HeaderText = "创建时间";
        }

        private void tb_suppliesName_TextChanged(object sender, EventArgs e)
        {
            //dataGridView1.Columns.Clear();
            //seleteGoodsData("SELECT goods_id,goods_name,goods_code,goods_height,goods_style,modify_time FROM goods_info WHERE goods_name LIKE '%" + tb_suppliesName.Text + "%'");
        }

        private void tb_sort_TextChanged(object sender, EventArgs e)
        {
            //dataGridView1.Columns.Clear();
            //seleteGoodsData("SELECT goods_id,goods_name,goods_code,goods_height,goods_style,modify_time FROM goods_info WHERE goods_height LIKE '%" + tb_sort.Text + "%'");
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            //for (i = 0; i < 98; i++)
            //{
            //    string name = (string)dataGridView1.Rows[i].Cells[4].Value;
            //    //MessageBox.Show(name);
            //    name = SelectState(name);
            //    if (name == "S")
            //    {
            //        dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.Yellow;
            //    }
            //}
            try
            {
                string name = (string)dataGridView1.Rows[e.RowIndex].Cells[4].Value;
                name = SelectState(name);
                if (name == "S")
                    dataGridView1[4, e.RowIndex].Style.BackColor = Color.Yellow;
                if (name == "I")
                    dataGridView1[4, e.RowIndex].Style.BackColor = Color.MediumSlateBlue;
                if (name == "O")
                    dataGridView1[4, e.RowIndex].Style.BackColor = Color.LimeGreen;
                label8.Text = name;

                label7.Text = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                tb_sort.Text = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
                tb_suppliesName.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
                tb_specification.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
                cb_houseNumber.Text = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();
            }
            catch
            {
                //for (i = 0; i < 6; i++)
                //{
                //    string name = (string)dataGridView1.Rows[i].Cells[4].Value;
                //    //MessageBox.Show(name);
                //    name = SelectState(name);
                //    if (name == "S")
                //    {
                //        dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.Yellow;
                //    }
                //}
            }
        }
    }
}

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
    public partial class PickMaterial : UserControl
    {
        private string sql;

        TextBox tb_houseNumber = new TextBox();
        TextBox tb_suppliesName = new TextBox();
        TextBox tb_specification = new TextBox();

        List<string> listNew = new List<string>();

        public PickMaterial()
        {
            InitializeComponent();
        }

        public void update_houseData()
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
            dataGridView1.Columns[3].HeaderText = "创建时间";
            dataGridView1.Columns[4].HeaderText = "物料数量";
        }

        private void PickMaterial_Load(object sender, EventArgs e)
        {
            update_houseData();
            //button1.Enabled = false;
            //button2.Enabled = false;
            //btn_seleteData.Enabled = false;

            btn_Add.Visible = false;
            btn_out.Visible = false;
            btn_seleteData.Visible = false;
        }

        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                //cb_houseNumber.Text = "";
                //cb_suppliesName.Text = "";
                //cb_specification.Text = "";
                //textBox1.Text = "";
                string a = dataGridView1.Rows[0].Cells[0].Value.ToString();
                string s = a.Substring(0, 1);
                if (s != "0")
                {
                    cb_houseNumber.Text = "";
                    cb_suppliesName.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
                    cb_specification.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
                }
                //else
                //{

                //}
                //if (dataGridView1.Columns[0].HeaderText == "库位编号")
                //{
                //    cb_houseNumber.Text = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                //    cb_suppliesName.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
                //    cb_specification.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
                //    textBox1.Text = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();
                //}
                if (s == "0")
                {
                    cb_houseNumber.Text = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                    cb_suppliesName.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
                    cb_specification.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
                    textBox1.Text = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();
                }
                    
            }
            catch
            {

            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            //if (checkBox1.Checked == true)
            //{
            //    button1.Enabled = true;
            //}
            //else
            //{
            //    button1.Enabled = false;
            //}
            checkBox2.Checked = false;
            if (checkBox1.Checked == true)
            {
                btn_Add.Visible = true;
            }
            else
            {
                btn_Add.Visible = false;
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            //if (checkBox2.Checked == true)
            //{
            //    button2.Enabled = true;
            //}
            //else
            //{
            //    button2.Enabled = false;
            //}
            checkBox1.Checked = false;
            cb_seleteData.Checked = false;
            if (checkBox2.Checked == true)
            {
                btn_out.Visible = true;
            }
            else
            {
                btn_out.Visible = false;
            }
            //textBox1.Text = "";
        }

        private void cb_houseNumber_DropDown(object sender, EventArgs e)
        {
            DataTable dt;
            var db = new DBAccess_MySql("MySql");
            dataGridView1.Columns.Clear();
            string outString = "";
            sql = "SELECT house_number FROM house_info";
            var rlt = db.QuerySQL_ToTable(sql, out dt, out outString);
            cb_houseNumber.DataSource = dt;
            cb_houseNumber.DisplayMember = "house_number";
            cb_houseNumber.Text = "";
            update_houseData();
        }

        private void cb_houseNumber_TextUpdate(object sender, EventArgs e)
        {
            #region 点击下拉框 模糊查询并显示下拉列表数据
            //DataTable dt;
            //var db = new DBAccess_MySql("MySql");
            //string outString = "";
            //sql = "SELECT house_number FROM house_info WHERE house_number LIKE '%" + cb_houseNumber.Text + "%'";
            //var rlt = db.QuerySQL_ToTable(sql, out dt, out outString);
            ////cb_houseNumber.DataSource = dt;
            //string strName = null;
            ////清空combobox
            //this.cb_houseNumber.Items.Clear();
            ////清空listNew
            //listNew.Clear();
            //for (int i = 0; i < dt.Rows.Count; i++)//遍历库位编号
            //{
            //    strName = dt.Rows[i]["house_number"].ToString();
            //    listNew.Add(strName);
            //}
            ////combobox添加已经查到的关键词
            //this.cb_houseNumber.Items.AddRange(listNew.ToArray());
            ////设置光标位置，否则光标位置始终保持在第一列，造成输入关键词的倒序排列
            //this.cb_houseNumber.SelectionStart = this.cb_houseNumber.Text.Length;
            ////保持鼠标指针原来状态，有时候鼠标指针会被下拉框覆盖，所以要进行一次设置。
            //Cursor = Cursors.Default;
            ////自动弹出下拉框
            //this.cb_houseNumber.DroppedDown = true;
            #endregion

            if (cb_houseNumber.Text != "")
            {
                seleteData("SELECT house_number,goods_code,goods_name,in_house_time,have_number FROM house_data WHERE house_number LIKE'%" + cb_houseNumber.Text + "%'");
            }
            else
            {
                seleteData("SELECT house_number,goods_code,goods_name,in_house_time,have_number FROM house_data");
            }
        }

        private void cb_seleteData_CheckedChanged(object sender, EventArgs e)
        {
            //if (cb_seleteData.Checked == true)
            //{
            //    cb_seleteData.Enabled = true;
            //}
            //else
            //{
            //    cb_seleteData.Enabled = false;
            //}
            checkBox2.Checked = false;
            if (cb_seleteData.Checked == true)
            {
                btn_seleteData.Visible = true;
                cb_houseNumber.Visible = false;
                cb_suppliesName.Visible = false;
                cb_specification.Visible = false;

                //创建textBox控件
                tb_houseNumber.Name = "tb_houseNumber";
                tb_houseNumber.Location = new System.Drawing.Point(103, 50);
                tb_houseNumber.Size = new System.Drawing.Size(121, 23);
                this.panel4.Controls.Add(tb_houseNumber);

                tb_suppliesName.Name = "tb_houseNumber";
                tb_suppliesName.Location = new System.Drawing.Point(103, 90);
                tb_suppliesName.Size = new System.Drawing.Size(121, 23);
                this.panel4.Controls.Add(tb_suppliesName);

                tb_specification.Name = "tb_houseNumber";
                tb_specification.Location = new System.Drawing.Point(103, 131);
                tb_specification.Size = new System.Drawing.Size(121, 23);
                this.panel4.Controls.Add(tb_specification);
            }
            else
            {
                btn_seleteData.Visible = false;
                cb_houseNumber.Visible = true;
                cb_suppliesName.Visible = true;
                cb_specification.Visible = true;
            }
            cb_houseNumber.Text = "";
            cb_suppliesName.Text = "";
            cb_specification.Text = "";
            textBox1.Text = "";
        }

        private void btn_seleteData_Click(object sender, EventArgs e)
        {
            if (tb_houseNumber.Text != "")
            {
                seleteData("SELECT house_number,goods_code,goods_name,in_house_time,have_number FROM house_data WHERE house_number LIKE'%" + tb_houseNumber.Text + "%'");
            }
            if (tb_suppliesName.Text != "")
            {
                seleteData("SELECT house_number,goods_code,goods_name,in_house_time,have_number FROM house_data WHERE goods_code LIKE'%" + tb_suppliesName.Text + "%'");
            }
            if (tb_specification.Text != "")
            {
                seleteData("SELECT house_number,goods_code,goods_name,in_house_time,have_number FROM house_data WHERE goods_name LIKE'%" + tb_specification.Text + "%'");
            }
            if(tb_houseNumber.Text == ""&& tb_suppliesName.Text == ""&& tb_specification.Text == "")
            {
                seleteData("SELECT house_number,goods_code,goods_name,in_house_time,have_number FROM house_data");
            }

        }

        //查询库存数据
        private void seleteData(string sql)
        {
            DataTable dt;
            var db = new DBAccess_MySql("MySql");
            dataGridView1.Columns.Clear();
            string outString = "";
            //sql = "SELECT house_number,goods_code,goods_name,in_house_time,have_number FROM house_data";
            var rlt = db.QuerySQL_ToTable(sql, out dt, out outString);
            dataGridView1.DataSource = dt;
            dataGridView1.AlternatingRowsDefaultCellStyle.BackColor = Color.LightBlue;
            dataGridView1.Columns[0].HeaderText = "库位编号";
            dataGridView1.Columns[1].HeaderText = "物料名称";
            dataGridView1.Columns[2].HeaderText = "物料规格";
            dataGridView1.Columns[3].HeaderText = "创建时间";
            dataGridView1.Columns[4].HeaderText = "物料数量";
        }

        //查询物料数据
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
            dataGridView1.Columns[3].HeaderText = "物料类别";
            dataGridView1.Columns[4].HeaderText = "创建时间";
        }

        private void cb_suppliesName_TextUpdate(object sender, EventArgs e)
        {
            dataGridView1.Columns.Clear();
            //seleteData("SELECT house_number,goods_code,goods_name,in_house_time,have_number FROM house_data WHERE goods_code LIKE'%" + cb_suppliesName.Text + "%'");
            seleteGoodsData("SELECT goods_id, goods_name, goods_code, goods_style, modify_time FROM goods_info WHERE goods_name like '%" + cb_suppliesName.Text + "%'");
        }

        private void cb_suppliesName_TextChanged(object sender, EventArgs e)
        {
            //MessageBox.Show(cb_suppliesName.Text);
        }

        private void cb_specification_TextUpdate(object sender, EventArgs e)
        {
            dataGridView1.Columns.Clear();
            seleteGoodsData("SELECT goods_id, goods_name, goods_code, goods_style, modify_time FROM goods_info WHERE goods_name like '%" + cb_specification.Text + "%'");
        }

        private void btn_Add_Click(object sender, EventArgs e)
        {
            int sum = 0;
            if (textBox1.Text == "")
            {
                sum = int.Parse("0") + int.Parse(tb_count.Text);
            }
            else
            {
                sum = int.Parse(textBox1.Text) + int.Parse(tb_count.Text);
            }
            
            string houseNumber = "";
            int list = 0;
            int blank = 0;
            int floor = 0;
            //加料入库
            var db = new DBAccess_MySql("MySql");
            string outString = "";
            DataTable dt;

            sql = "SELECT * FROM house_data WHERE house_number = '"+cb_houseNumber.Text+"' AND goods_code = '"+cb_suppliesName.Text+"' AND goods_name = '"+cb_specification.Text+"'";
            var rlt3 = db.QuerySQL_ToTable(sql, out dt, out outString);
            string str = "";
            foreach (DataRow dr in dt.Rows)
            {
                str = dr["house_number"].ToString();
            }

            if (str != "")
            {
                if (btn_Add.Text == "入库")
                {
                    sql = "UPDATE house_data SET have_number = " + sum + " WHERE house_number = '" + cb_houseNumber.Text + "' AND goods_code = '" + cb_suppliesName.Text + "' AND goods_name = '" + cb_specification.Text + "'";
                    var rlt = db.ExecSql(sql, out outString);
                    list = int.Parse((cb_houseNumber.Text).Substring(0,2));
                    blank = int.Parse((cb_houseNumber.Text).Substring(2, 2));
                    floor = int.Parse((cb_houseNumber.Text).Substring(4, 2));
                    WriteSRM.WriteSrmData(btn_Add.Text, 2, list, blank, floor);
                    btn_Add.Text = "出库";
                    cb_houseNumber.Text = "";
                    cb_specification.Text = "";
                    cb_suppliesName.Text = "";
                    textBox1.Text = "";
                }
                else
                {

                    sql = "SELECT house_number,list,blank,floor FROM house_info WHERE house_number = '" + cb_houseNumber.Text + "'";
                    var rlt1 = db.QuerySQL_ToTable(sql, out dt, out outString);
                    if (dt == null || dt.Rows.Count <= 0) return;
                    houseNumber = dt.Rows[0]["house_number"].ToString();
                    list = int.Parse(dt.Rows[0]["list"].ToString());
                    blank = int.Parse(dt.Rows[0]["blank"].ToString());
                    floor = int.Parse(dt.Rows[0]["floor"].ToString());
                    WriteSRM.WriteSrmData(btn_Add.Text, 2, list, blank, floor);
                    btn_Add.Text = "入库";
                }

            }
            else
            {
                if (btn_Add.Text == "入库")
                {
                    //跟新货位状态
                    sql = "INSERT INTO house_data(house_number,goods_name,goods_code,have_number,in_house_time) VALUES('" + cb_houseNumber.Text + "','" + cb_suppliesName.Text + "','" + cb_specification.Text + "'," + int.Parse(tb_count.Text) + ",SYSDATE())";
                    var rlt2 = db.ExecSql(sql, out outString);
                }

                sql = "SELECT house_number,list,blank,floor FROM house_info WHERE house_number = '" + cb_houseNumber.Text + "'";
                var rlt1 = db.QuerySQL_ToTable(sql, out dt, out outString);
                if (dt == null || dt.Rows.Count <= 0) return;
                houseNumber = dt.Rows[0]["house_number"].ToString();
                list = int.Parse(dt.Rows[0]["list"].ToString());
                blank = int.Parse(dt.Rows[0]["blank"].ToString());
                floor = int.Parse(dt.Rows[0]["floor"].ToString());
                WriteSRM.WriteSrmData(btn_Add.Text, 2, list, blank, floor);
                btn_Add.Text = "入库";
            }
            update_houseData();
        }

        private void btn_out_Click(object sender, EventArgs e)
        {
            int sum = int.Parse(textBox1.Text) - int.Parse(tb_count.Text);
            string houseNumber = "";
            int list = 0;
            int blank = 0;
            int floor = 0;
            //加料入库
            var db = new DBAccess_MySql("MySql");
            string outString = "";

            if (btn_out.Text == "入库")
            {
                if (sum == 0)
                {
                    sql = "UPDATE house_data SET have_number = " + sum + " WHERE house_number = '" + cb_houseNumber.Text + "' AND goods_code = '" + cb_suppliesName.Text + "' AND goods_name = '" + cb_specification.Text + "'";
                    db.ExecSql(sql, out outString);

                    sql = "DELETE FROM house_data WHERE have_number = " + 0;
                    db.ExecSql(sql, out outString);
                    list = int.Parse((cb_houseNumber.Text).Substring(0, 2));
                    blank = int.Parse((cb_houseNumber.Text).Substring(2, 2));
                    floor = int.Parse((cb_houseNumber.Text).Substring(4, 2));
                    WriteSRM.WriteSrmData(btn_out.Text, 2, list, blank, floor);
                    btn_out.Text = "出库";
                    cb_houseNumber.Text = "";
                    cb_specification.Text = "";
                    cb_suppliesName.Text = "";
                    textBox1.Text = "";
                }
                else
                {
                    sql = "UPDATE house_data SET have_number = " + sum + " WHERE house_number = '" + cb_houseNumber.Text + "' AND goods_code = '" + cb_suppliesName.Text + "' AND goods_name = '" + cb_specification.Text + "'";
                    var rlt = db.ExecSql(sql, out outString);
                    list = int.Parse((cb_houseNumber.Text).Substring(0, 2));
                    blank = int.Parse((cb_houseNumber.Text).Substring(2, 2));
                    floor = int.Parse((cb_houseNumber.Text).Substring(4, 2));
                    WriteSRM.WriteSrmData(btn_out.Text, 2, list, blank, floor);
                    btn_out.Text = "出库";
                    cb_houseNumber.Text = "";
                    cb_specification.Text = "";
                    cb_suppliesName.Text = "";
                    textBox1.Text = "";
                }
            }
            else
            {
                DataTable dt;
                sql = "SELECT house_number,list,blank,floor FROM house_info WHERE house_number = '" + cb_houseNumber.Text + "'";
                var rlt1 = db.QuerySQL_ToTable(sql, out dt, out outString);
                if (dt == null || dt.Rows.Count <= 0) return;
                houseNumber = dt.Rows[0]["house_number"].ToString();
                list = int.Parse(dt.Rows[0]["list"].ToString());
                blank = int.Parse(dt.Rows[0]["blank"].ToString());
                floor = int.Parse(dt.Rows[0]["floor"].ToString());
                WriteSRM.WriteSrmData(btn_out.Text, 2, list, blank, floor);
                btn_out.Text = "入库";
            }
            update_houseData();
        }
    }
}

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
    public partial class MerchandiseData : UserControl
    {
        private string sql;

        public MerchandiseData()
        {
            InitializeComponent();
        }

        public void update_goodInfo()
        {
            DataTable dt;
            var db = new DBAccess_MySql("MySql");
            dataGridView1.Columns.Clear();
            string outString = "";
            sql = "SELECT goods_id,goods_name,goods_code,goods_height,modify_time FROM goods_info";
            var rlt = db.QuerySQL_ToTable(sql, out dt, out outString);
            dataGridView1.DataSource = dt;
            dataGridView1.AlternatingRowsDefaultCellStyle.BackColor = Color.LightBlue;
            dataGridView1.Columns[0].HeaderText = "物料编号";
            dataGridView1.Columns[1].HeaderText = "物料名称";
            dataGridView1.Columns[2].HeaderText = "物料规格";
            dataGridView1.Columns[3].HeaderText = "物料高度";
            dataGridView1.Columns[4].HeaderText = "创建时间";
        }

        private void MerchandiseData_Load(object sender, EventArgs e)
        {
            update_goodInfo();
        }

        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                //e.RowIndex是行索引，e.ColumnIndex是列索引
                tb_goodsid.Text = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                tb_suppliesName.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
                tb_specification.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
                tb_sort.Text = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
            }
            catch
            {

            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                string suppliesName = tb_suppliesName.Text;
                string specification = tb_specification.Text;
                string sort = tb_sort.Text;
                var db = new DBAccess_MySql("MySql");
                dataGridView1.Columns.Clear();
                string outString = "";

                //查询物料表中是否重复物料
                DBAccess_MySql dby = new DBAccess_MySql();
                sql = "SELECT * FROM goods_info WHERE goods_name = '" + suppliesName + "' AND goods_code = '" + specification + "'";
                dby = db.ReturnSQL_String(sql, out outString);
                string goodsName = null;
                string goodsCode = null;
                try
                {
                    while (dby.rec.Read())
                    {
                        goodsName = dby.rec.GetString(1);
                        goodsCode = dby.rec.GetString(2);
                    }
                    dby.connMysql.Close();
                }
                catch
                {

                }
                if (goodsCode == suppliesName && goodsName == specification)
                {
                    update_goodInfo();
                    MessageBox.Show("已录入该物料产品！");
                }
                else
                {
                    if(specification==""|| sort == "")
                    {
                        sql = "INSERT into goods_info(goods_name) VALUES('" + suppliesName + "')";
                        db.ExecSql(sql, out outString);
                    }
                    else
                    {
                        sql = "INSERT into goods_info(goods_name,goods_code,goods_height) VALUES('" + suppliesName + "','" + specification + "','" + sort + "')";
                        var rlt = db.ExecSql(sql, out outString);
                    }
                    
                }
                update_goodInfo();
            }
            catch
            {
                MessageBox.Show("重复操作！");
            }
        }

        private void btnAlter_Click(object sender, EventArgs e)
        {
            try
            {
                int goodid = int.Parse(tb_goodsid.Text);
                string suppliesName = tb_suppliesName.Text;
                string specification = tb_specification.Text;
                string sort = tb_sort.Text;
                var db = new DBAccess_MySql("MySql");
                dataGridView1.Columns.Clear();
                string outString = "";

                sql = "UPDATE goods_info SET goods_name = '" + suppliesName + "',goods_code = '" + specification + "',goods_height = '" + sort + "' WHERE goods_id = " + goodid;
                var rlt = db.ExecSql(sql, out outString);
               
                update_goodInfo();
            }
            catch
            {
                MessageBox.Show("重复操作！");
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                string goodid = tb_goodsid.Text;
                var db = new DBAccess_MySql("MySql");
                dataGridView1.Columns.Clear();
                string outString = "";
                sql = "DELETE FROM goods_info WHERE goods_id = " + int.Parse(goodid);
                var rlt = db.ExecSql(sql, out outString);
                update_goodInfo();
            }
            catch
            {
                MessageBox.Show("重复操作！");
            }
        }
    }
}

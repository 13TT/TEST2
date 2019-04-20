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
    public partial class TakeStock : UserControl
    {
        private string sql;

        public TakeStock()
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

        private void TakeStock_Load(object sender, EventArgs e)
        {
            update_houseData();
        }
    }
}

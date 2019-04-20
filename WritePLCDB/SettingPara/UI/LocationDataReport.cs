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
    public partial class LocationDataReport : UserControl
    {
        private string sql;

        public LocationDataReport()
        {
            InitializeComponent();
        }
        public void update_houseData()
        {
            System.Data.DataTable dt;
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

        private void LocationDataReport_Load(object sender, EventArgs e)
        {
            update_houseData();
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
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
            for(int i = 0; i < dataGridView1.RowCount; i++)
            {
                for(int j = 0; j < dataGridView1.ColumnCount; j++)
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
    }
}

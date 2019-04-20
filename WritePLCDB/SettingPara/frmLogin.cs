using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SettingPara
{
    public partial class frmLogin : Form
    {
        private string sql;

        public frmLogin()
        {
            InitializeComponent();
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            DataTable dt;
            var db = new DBAccess_MySql("MySql");
            string outString = "";

            groupBox1.BackColor = Color.Transparent;
            groupBox1.Parent = panel1;

            sql = "SELECT user_code FROM user_info LIMIT 1,2";
            var rlt1 = db.QuerySQL_ToTable(sql, out dt, out outString);
            DataRow dr = dt.Rows[0];
            tb_userName.Text = dr[0].ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Process.GetCurrentProcess().Kill();
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DataTable dt;
            var db = new DBAccess_MySql("MySql");
            string outString = "";
            sql = "SELECT * FROM user_info WHERE user_code = '" + tb_userName.Text + "' AND pass_word = '" + tb_userPassword.Text + "'";
            var rlt = db.QuerySQL_ToTable(sql, out dt, out outString);

            if (dt.Rows.Count == 1)
            {
                int i = dt.Rows.Count;
                FRM_CC fc = new FRM_CC();
                this.Visible = false;
                fc.Show();
            }
            else
            {
                int i = dt.Rows.Count;
                MessageBox.Show("请检查用户名和密码是否正确！");
            }

            sql = "SELECT use_pl_new_pallet FROM user_info WHERE user_code = '" + tb_userName.Text + "'";
            var rlt1 = db.QuerySQL_ToTable(sql, out dt, out outString);
            try
            {
                DataRow dr = dt.Rows[0];
                string s = dr[0].ToString();

                sql = "UPDATE config_unit SET table_name = '" + tb_userName.Text + "',unit_name = '" + s + "'";
                db.ExecSql(sql, out outString);
            }
            catch
            {

            }
        }

        private void frmLogin_FormClosed(object sender, FormClosedEventArgs e)
        {
            Process.GetCurrentProcess().Kill();
            this.Close();
        }
    }
}

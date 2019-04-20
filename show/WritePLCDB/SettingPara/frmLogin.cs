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
            groupBox1.BackColor = Color.Transparent;
            groupBox1.Parent = panel1;
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
        }

        private void frmLogin_FormClosed(object sender, FormClosedEventArgs e)
        {
            Process.GetCurrentProcess().Kill();
            this.Close();
        }
    }
}

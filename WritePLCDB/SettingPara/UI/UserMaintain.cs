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
    public partial class UserMaintain : UserControl
    {
        private string sql;
        DataTable dt;
        string outString = "";

        public UserMaintain()
        {
            InitializeComponent();
        }

        private void comboBox1_DropDown(object sender, EventArgs e)
        {
            var db = new DBAccess_MySql("MySql");
            dataGridView1.Columns.Clear();
            sql = "SELECT user_code FROM user_info";
            var rlt = db.QuerySQL_ToTable(sql, out dt, out outString);
            comboBox1.DataSource = dt;
            comboBox1.DisplayMember = "user_code";
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var db = new DBAccess_MySql("MySql");
            sql = "SELECT id,user_code,user_name,pass_word FROM user_info WHERE user_code = '" + comboBox1.Text + "'";
            db.QuerySQL_ToTable(sql, out dt, out outString);
            if (dt == null || dt.Rows.Count <= 0) return;
            string id = dt.Rows[0]["id"].ToString();
            string user_code = dt.Rows[0]["user_code"].ToString();
            string user_name = dt.Rows[0]["user_name"].ToString();
            string pass_word = dt.Rows[0]["pass_word"].ToString();
            lab_id.Text = id;
            textBox2.Text = user_name;
            textBox3.Text = pass_word;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (lb_S.Text == "S")
            {
                var db = new DBAccess_MySql("MySql");
                string code = comboBox1.Text;
                string name = textBox2.Text;
                string paw = textBox3.Text;
                string pasword = textBox4.Text;
                if (MessageBox.Show("用户名：" + code + "  姓名：" + name + "  密码：" + pasword, "确认添加！", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    sql = "INSERT INTO user_info(user_code,user_name,pass_word) VALUES('" + code + "','" + name + "','" + paw + "')";
                    db.ExecSql(sql, out outString);
                    MessageBox.Show("添加成功!");
                    TestCBox();
                    return;
                }
                else
                {
                    return;
                }
            }
            else
            {
                MessageBox.Show("没有权限！");
            }
        }

        private void btnAlter_Click(object sender, EventArgs e)
        {
            if (lb_S.Text == "S")
            {
                var db = new DBAccess_MySql("MySql");
                string code = comboBox1.Text;
                string name = textBox2.Text;
                string paw = textBox3.Text;
                string pasword = textBox4.Text;
                if (MessageBox.Show("用户名：" + code + "  姓名：" + name + "  密码：" + pasword, "确认修改！", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    if (paw == pasword)
                    {
                        sql = "UPDATE user_info SET user_code = '" + code + "',user_name='" + name + "',pass_word='" + pasword + "' WHERE id = '" + int.Parse(lab_id.Text) + "'";
                        db.ExecSql(sql, out outString);
                        MessageBox.Show("修改成功！");
                        TestCBox();
                        return;
                    }
                    else
                    {
                        MessageBox.Show("确定用户名和密码一致！");
                    }
                }
                else
                {
                    return;
                }
            }
            else
            {
                MessageBox.Show("没有权限！");
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (lb_S.Text == "S")
            {
                var db = new DBAccess_MySql("MySql");
                string code = comboBox1.Text;
                string name = textBox2.Text;
                string paw = textBox3.Text;
                string pasword = textBox4.Text;
                if (MessageBox.Show("用户名：" + code + "  姓名：" + name + "  密码：" + pasword, "确认删除！", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    sql = "DELETE FROM user_info WHERE id = '" + int.Parse(lab_id.Text) + "'";
                    db.ExecSql(sql, out outString);
                    MessageBox.Show("删除成功！");
                    TestCBox();
                    return;
                }
                else
                {
                    return;
                }
            }
            else
            {
                MessageBox.Show("没有权限");
            }
        }

        private void UserMaintain_Load(object sender, EventArgs e)
        {
            DataTable dt;
            var db = new DBAccess_MySql("MySql");
            string outString = "";

            sql = "SELECT * FROM config_unit";
            var rlt1 = db.QuerySQL_ToTable(sql, out dt, out outString);
            DataRow dr = dt.Rows[0];
            string s = dr[0].ToString();
            lb_S.Text = dr[0].ToString();
        }

        private void TestCBox()
        {
            comboBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
        }
    }
}

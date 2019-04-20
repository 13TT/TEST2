using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace SettingPara.UI
{
    public partial class SrmStatus : UserControl
    {
        public SrmStatus()
        {
            InitializeComponent();
        }

        private void SrmStatus_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            button2.Location = new Point(this.button2.Location.X, 44);
            button2.Location = new Point(this.button2.Location.Y, 182);
            int a = 75;
            int i = 0;
            while (i < a) 
            {
                button2.Left += 10;
                i++;
                Thread.Sleep(100);
            }
        }
    }
}

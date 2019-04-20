namespace SettingPara
{
    partial class FRM_CC
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FRM_CC));
            this.timer_Request = new System.Windows.Forms.Timer(this.components);
            this.timer_CCSTS = new System.Windows.Forms.Timer(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.timer_SRM = new System.Windows.Forms.Timer(this.components);
            this.timer_T_ShowData = new System.Windows.Forms.Timer(this.components);
            this.tm_updateOrder = new System.Windows.Forms.Timer(this.components);
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.lab_zhanhao = new System.Windows.Forms.Label();
            this.lab_leixing = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lab_house = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lab_name = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // timer_Request
            // 
            this.timer_Request.Tick += new System.EventHandler(this.timer_Request_Tick);
            // 
            // timer_CCSTS
            // 
            this.timer_CCSTS.Tick += new System.EventHandler(this.timer_CCSTS_Tick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(25, 267);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 12);
            this.label1.TabIndex = 3;
            // 
            // timer_SRM
            // 
            this.timer_SRM.Enabled = true;
            this.timer_SRM.Tick += new System.EventHandler(this.timer_SRM_Tick);
            // 
            // timer_T_ShowData
            // 
            this.timer_T_ShowData.Tick += new System.EventHandler(this.timer_T_ShowData_Tick);
            // 
            // tm_updateOrder
            // 
            this.tm_updateOrder.Interval = 3000;
            this.tm_updateOrder.Tick += new System.EventHandler(this.tm_updateOrder_Tick);
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.lab_zhanhao);
            this.groupBox3.Controls.Add(this.lab_leixing);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Font = new System.Drawing.Font("宋体", 42F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox3.ForeColor = System.Drawing.Color.Red;
            this.groupBox3.Location = new System.Drawing.Point(154, 528);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(788, 180);
            this.groupBox3.TabIndex = 7;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "指令类型";
            // 
            // lab_zhanhao
            // 
            this.lab_zhanhao.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.lab_zhanhao.AutoSize = true;
            this.lab_zhanhao.Location = new System.Drawing.Point(719, 69);
            this.lab_zhanhao.Name = "lab_zhanhao";
            this.lab_zhanhao.Size = new System.Drawing.Size(0, 56);
            this.lab_zhanhao.TabIndex = 4;
            // 
            // lab_leixing
            // 
            this.lab_leixing.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.lab_leixing.AutoSize = true;
            this.lab_leixing.Location = new System.Drawing.Point(299, 76);
            this.lab_leixing.Name = "lab_leixing";
            this.lab_leixing.Size = new System.Drawing.Size(0, 56);
            this.lab_leixing.TabIndex = 2;
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(521, 76);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(192, 56);
            this.label5.TabIndex = 3;
            this.label5.Text = "站号：";
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(101, 76);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(192, 56);
            this.label4.TabIndex = 2;
            this.label4.Text = "类型：";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.lab_house);
            this.groupBox2.Font = new System.Drawing.Font("宋体", 42F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox2.ForeColor = System.Drawing.Color.Red;
            this.groupBox2.Location = new System.Drawing.Point(154, 322);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(788, 180);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "库位编号 ";
            this.groupBox2.Enter += new System.EventHandler(this.groupBox2_Enter);
            // 
            // lab_house
            // 
            this.lab_house.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.lab_house.AutoSize = true;
            this.lab_house.Location = new System.Drawing.Point(375, 56);
            this.lab_house.Name = "lab_house";
            this.lab_house.Size = new System.Drawing.Size(248, 56);
            this.lab_house.TabIndex = 1;
            this.lab_house.Text = "库位编号";
            this.lab_house.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.lab_name);
            this.groupBox1.Font = new System.Drawing.Font("宋体", 42F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox1.ForeColor = System.Drawing.Color.Red;
            this.groupBox1.Location = new System.Drawing.Point(154, 109);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(788, 180);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "物料名称";
            // 
            // lab_name
            // 
            this.lab_name.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.lab_name.AutoSize = true;
            this.lab_name.Location = new System.Drawing.Point(235, 56);
            this.lab_name.Name = "lab_name";
            this.lab_name.Size = new System.Drawing.Size(248, 56);
            this.lab_name.TabIndex = 0;
            this.lab_name.Text = "名称规格";
            this.lab_name.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 42F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.ForeColor = System.Drawing.Color.Red;
            this.label2.Location = new System.Drawing.Point(333, 50);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(584, 56);
            this.label2.TabIndex = 4;
            this.label2.Text = "长安汽车股份有限公司";
            // 
            // FRM_CC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(1096, 602);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FRM_CC";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "立体仓库管理系统 (V1.2.1)";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FRM_CC_FormClosing);
            this.Load += new System.EventHandler(this.FRM_CC_Load);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer timer_Request;
        private System.Windows.Forms.Timer timer_CCSTS;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Timer timer_SRM;
        private System.Windows.Forms.Timer timer_T_ShowData;
        private System.Windows.Forms.Timer tm_updateOrder;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label lab_zhanhao;
        private System.Windows.Forms.Label lab_leixing;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label lab_house;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lab_name;
        private System.Windows.Forms.Label label2;
    }
}
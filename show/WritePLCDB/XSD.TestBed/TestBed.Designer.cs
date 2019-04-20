namespace XSD.TestBed
{
    partial class TestBed
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
            this.tab_Test = new System.Windows.Forms.TabControl();
            this.Page_PLC = new System.Windows.Forms.TabPage();
            this.btn_Start = new System.Windows.Forms.Button();
            this.dgv_SRMStatus = new System.Windows.Forms.DataGridView();
            this.Page_SRM = new System.Windows.Forms.TabPage();
            this.timer_ReadPLC = new System.Windows.Forms.Timer(this.components);
            this.tab_Test.SuspendLayout();
            this.Page_PLC.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_SRMStatus)).BeginInit();
            this.SuspendLayout();
            // 
            // tab_Test
            // 
            this.tab_Test.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tab_Test.Controls.Add(this.Page_PLC);
            this.tab_Test.Controls.Add(this.Page_SRM);
            this.tab_Test.Location = new System.Drawing.Point(1, -1);
            this.tab_Test.Name = "tab_Test";
            this.tab_Test.SelectedIndex = 0;
            this.tab_Test.Size = new System.Drawing.Size(949, 445);
            this.tab_Test.TabIndex = 0;
            // 
            // Page_PLC
            // 
            this.Page_PLC.Controls.Add(this.btn_Start);
            this.Page_PLC.Controls.Add(this.dgv_SRMStatus);
            this.Page_PLC.Location = new System.Drawing.Point(4, 22);
            this.Page_PLC.Name = "Page_PLC";
            this.Page_PLC.Padding = new System.Windows.Forms.Padding(3);
            this.Page_PLC.Size = new System.Drawing.Size(941, 419);
            this.Page_PLC.TabIndex = 0;
            this.Page_PLC.Text = "PLC";
            this.Page_PLC.UseVisualStyleBackColor = true;
            // 
            // btn_Start
            // 
            this.btn_Start.Location = new System.Drawing.Point(7, 14);
            this.btn_Start.Name = "btn_Start";
            this.btn_Start.Size = new System.Drawing.Size(75, 25);
            this.btn_Start.TabIndex = 1;
            this.btn_Start.Text = "Start";
            this.btn_Start.UseVisualStyleBackColor = true;
            this.btn_Start.Click += new System.EventHandler(this.btn_Start_Click);
            // 
            // dgv_SRMStatus
            // 
            this.dgv_SRMStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dgv_SRMStatus.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_SRMStatus.Location = new System.Drawing.Point(7, 46);
            this.dgv_SRMStatus.Name = "dgv_SRMStatus";
            this.dgv_SRMStatus.RowTemplate.Height = 23;
            this.dgv_SRMStatus.Size = new System.Drawing.Size(931, 376);
            this.dgv_SRMStatus.TabIndex = 0;
            // 
            // Page_SRM
            // 
            this.Page_SRM.Location = new System.Drawing.Point(4, 22);
            this.Page_SRM.Name = "Page_SRM";
            this.Page_SRM.Padding = new System.Windows.Forms.Padding(3);
            this.Page_SRM.Size = new System.Drawing.Size(672, 303);
            this.Page_SRM.TabIndex = 1;
            this.Page_SRM.Text = "SRM";
            this.Page_SRM.UseVisualStyleBackColor = true;
            // 
            // timer_ReadPLC
            // 
            this.timer_ReadPLC.Interval = 1000;
            this.timer_ReadPLC.Tick += new System.EventHandler(this.timer_ReadPLC_Tick_1);
            // 
            // TestBed
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(950, 439);
            this.Controls.Add(this.tab_Test);
            this.Name = "TestBed";
            this.Text = "Form1";
            this.tab_Test.ResumeLayout(false);
            this.Page_PLC.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_SRMStatus)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tab_Test;
        private System.Windows.Forms.TabPage Page_PLC;
        private System.Windows.Forms.Button btn_Start;
        private System.Windows.Forms.DataGridView dgv_SRMStatus;
        private System.Windows.Forms.TabPage Page_SRM;
        private System.Windows.Forms.Timer timer_ReadPLC;
    }
}


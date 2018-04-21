namespace ServerBackup
{
    partial class FrmServerBackup
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.lblST = new System.Windows.Forms.Label();
            this.lblIP = new System.Windows.Forms.Label();
            this.txtInfo = new System.Windows.Forms.TextBox();
            this.lblLight = new System.Windows.Forms.Label();
            this.lblProcess = new System.Windows.Forms.Label();
            this.lblMem = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblST
            // 
            this.lblST.AutoSize = true;
            this.lblST.Location = new System.Drawing.Point(198, 21);
            this.lblST.Name = "lblST";
            this.lblST.Size = new System.Drawing.Size(65, 12);
            this.lblST.TabIndex = 0;
            this.lblST.Text = "A机或者B机";
            // 
            // lblIP
            // 
            this.lblIP.AutoSize = true;
            this.lblIP.Location = new System.Drawing.Point(18, 21);
            this.lblIP.Name = "lblIP";
            this.lblIP.Size = new System.Drawing.Size(41, 12);
            this.lblIP.TabIndex = 0;
            this.lblIP.Text = "公用IP";
            // 
            // txtInfo
            // 
            this.txtInfo.Location = new System.Drawing.Point(12, 53);
            this.txtInfo.Multiline = true;
            this.txtInfo.Name = "txtInfo";
            this.txtInfo.Size = new System.Drawing.Size(402, 356);
            this.txtInfo.TabIndex = 1;
            // 
            // lblLight
            // 
            this.lblLight.AutoSize = true;
            this.lblLight.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblLight.ForeColor = System.Drawing.SystemColors.Highlight;
            this.lblLight.Location = new System.Drawing.Point(373, 17);
            this.lblLight.Name = "lblLight";
            this.lblLight.Size = new System.Drawing.Size(32, 21);
            this.lblLight.TabIndex = 2;
            this.lblLight.Text = "●";
            // 
            // lblProcess
            // 
            this.lblProcess.AutoSize = true;
            this.lblProcess.Location = new System.Drawing.Point(13, 417);
            this.lblProcess.Name = "lblProcess";
            this.lblProcess.Size = new System.Drawing.Size(71, 12);
            this.lblProcess.TabIndex = 3;
            this.lblProcess.Text = "MSS进程检查";
            // 
            // lblMem
            // 
            this.lblMem.AutoSize = true;
            this.lblMem.Location = new System.Drawing.Point(246, 417);
            this.lblMem.Name = "lblMem";
            this.lblMem.Size = new System.Drawing.Size(71, 12);
            this.lblMem.TabIndex = 3;
            this.lblMem.Text = "MSS内存检查";
            // 
            // FrmServerBackup
            // 
            this.ClientSize = new System.Drawing.Size(426, 435);
            this.Controls.Add(this.lblMem);
            this.Controls.Add(this.lblProcess);
            this.Controls.Add(this.lblLight);
            this.Controls.Add(this.txtInfo);
            this.Controls.Add(this.lblIP);
            this.Controls.Add(this.lblST);
            this.Name = "FrmServerBackup";
            this.Text = "双机热备切换工具";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmServerBackup_FormClosing);
            this.Load += new System.EventHandler(this.FrmServerBackup_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblST;
        private System.Windows.Forms.Label lblIP;
        private System.Windows.Forms.TextBox txtInfo;
        private System.Windows.Forms.Label lblLight;
        private System.Windows.Forms.Label lblProcess;
        private System.Windows.Forms.Label lblMem;
    }
}


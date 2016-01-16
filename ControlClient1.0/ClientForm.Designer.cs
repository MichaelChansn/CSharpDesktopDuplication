namespace ControlClient1._0
{
    partial class ClientForm
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
            this.components = new System.ComponentModel.Container();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.pictureBoxRec = new System.Windows.Forms.PictureBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.labeldispalyQueue = new System.Windows.Forms.Label();
            this.labelDif = new System.Windows.Forms.Label();
            this.labelQueueCap = new System.Windows.Forms.Label();
            this.textBoxInfo = new System.Windows.Forms.TextBox();
            this.textBoxIP = new System.Windows.Forms.TextBox();
            this.buttonConnect = new System.Windows.Forms.Button();
            this.timerGC = new System.Windows.Forms.Timer(this.components);
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxRec)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.pictureBoxRec);
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(787, 411);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "接收图像";
            // 
            // pictureBoxRec
            // 
            this.pictureBoxRec.BackColor = System.Drawing.Color.SpringGreen;
            this.pictureBoxRec.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBoxRec.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxRec.Location = new System.Drawing.Point(3, 17);
            this.pictureBoxRec.Name = "pictureBoxRec";
            this.pictureBoxRec.Size = new System.Drawing.Size(781, 391);
            this.pictureBoxRec.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxRec.TabIndex = 0;
            this.pictureBoxRec.TabStop = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.button1);
            this.groupBox2.Controls.Add(this.labeldispalyQueue);
            this.groupBox2.Controls.Add(this.labelDif);
            this.groupBox2.Controls.Add(this.labelQueueCap);
            this.groupBox2.Controls.Add(this.textBoxInfo);
            this.groupBox2.Controls.Add(this.textBoxIP);
            this.groupBox2.Controls.Add(this.buttonConnect);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox2.Location = new System.Drawing.Point(0, 417);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(787, 85);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "控制信息";
            // 
            // labeldispalyQueue
            // 
            this.labeldispalyQueue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.labeldispalyQueue.AutoSize = true;
            this.labeldispalyQueue.ForeColor = System.Drawing.Color.Red;
            this.labeldispalyQueue.Location = new System.Drawing.Point(498, 62);
            this.labeldispalyQueue.Name = "labeldispalyQueue";
            this.labeldispalyQueue.Size = new System.Drawing.Size(77, 12);
            this.labeldispalyQueue.TabIndex = 4;
            this.labeldispalyQueue.Text = "显示队列大小";
            // 
            // labelDif
            // 
            this.labelDif.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.labelDif.AutoSize = true;
            this.labelDif.ForeColor = System.Drawing.Color.Red;
            this.labelDif.Location = new System.Drawing.Point(498, 40);
            this.labelDif.Name = "labelDif";
            this.labelDif.Size = new System.Drawing.Size(77, 12);
            this.labelDif.TabIndex = 4;
            this.labelDif.Text = "差异队列大小";
            // 
            // labelQueueCap
            // 
            this.labelQueueCap.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.labelQueueCap.AutoSize = true;
            this.labelQueueCap.ForeColor = System.Drawing.Color.Red;
            this.labelQueueCap.Location = new System.Drawing.Point(498, 17);
            this.labelQueueCap.Name = "labelQueueCap";
            this.labelQueueCap.Size = new System.Drawing.Size(77, 12);
            this.labelQueueCap.TabIndex = 4;
            this.labelQueueCap.Text = "接收队列大小";
            // 
            // textBoxInfo
            // 
            this.textBoxInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.textBoxInfo.Font = new System.Drawing.Font("宋体", 11F);
            this.textBoxInfo.Location = new System.Drawing.Point(225, 21);
            this.textBoxInfo.Name = "textBoxInfo";
            this.textBoxInfo.Size = new System.Drawing.Size(225, 24);
            this.textBoxInfo.TabIndex = 2;
            this.textBoxInfo.TextChanged += new System.EventHandler(this.textBoxInfo_TextChanged);
            // 
            // textBoxIP
            // 
            this.textBoxIP.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.textBoxIP.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBoxIP.ForeColor = System.Drawing.Color.Red;
            this.textBoxIP.Location = new System.Drawing.Point(9, 34);
            this.textBoxIP.Name = "textBoxIP";
            this.textBoxIP.Size = new System.Drawing.Size(182, 30);
            this.textBoxIP.TabIndex = 1;
            this.textBoxIP.Text = "127.0.0.1:8888";
            // 
            // buttonConnect
            // 
            this.buttonConnect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.buttonConnect.Location = new System.Drawing.Point(652, 30);
            this.buttonConnect.Name = "buttonConnect";
            this.buttonConnect.Size = new System.Drawing.Size(104, 34);
            this.buttonConnect.TabIndex = 0;
            this.buttonConnect.Text = "连接服务器";
            this.buttonConnect.UseVisualStyleBackColor = true;
            this.buttonConnect.Click += new System.EventHandler(this.buttonConnect_Click);
            // 
            // timerGC
            // 
            this.timerGC.Enabled = true;
            this.timerGC.Interval = 20;
            this.timerGC.Tick += new System.EventHandler(this.timerGC_Tick);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(225, 50);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(225, 29);
            this.button1.TabIndex = 5;
            this.button1.Text = "发送命令";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // ClientForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(787, 502);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "ClientForm";
            this.Text = "接收端";
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxRec)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.PictureBox pictureBoxRec;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox textBoxIP;
        private System.Windows.Forms.Button buttonConnect;
        private System.Windows.Forms.TextBox textBoxInfo;
        private System.Windows.Forms.Label labelQueueCap;
        private System.Windows.Forms.Label labeldispalyQueue;
        private System.Windows.Forms.Label labelDif;
        private System.Windows.Forms.Timer timerGC;
        private System.Windows.Forms.Button button1;
    }
}


namespace ControlServer1._0
{
    partial class ServerForm
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
            this.pictureBoxSender = new System.Windows.Forms.PictureBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.textBoxSend = new System.Windows.Forms.TextBox();
            this.textBoxDif = new System.Windows.Forms.TextBox();
            this.textBoxCopy = new System.Windows.Forms.TextBox();
            this.textBoxTimeShow = new System.Windows.Forms.TextBox();
            this.buttonAverageTest = new System.Windows.Forms.Button();
            this.buttonBitBltTest = new System.Windows.Forms.Button();
            this.textBoxInfoShow = new System.Windows.Forms.TextBox();
            this.buttonLocalTest = new System.Windows.Forms.Button();
            this.buttonStartSendPic = new System.Windows.Forms.Button();
            this.buttonStartSocket = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSender)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBoxSender
            // 
            this.pictureBoxSender.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBoxSender.Location = new System.Drawing.Point(8, 29);
            this.pictureBoxSender.Name = "pictureBoxSender";
            this.pictureBoxSender.Size = new System.Drawing.Size(632, 362);
            this.pictureBoxSender.TabIndex = 0;
            this.pictureBoxSender.TabStop = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.pictureBoxSender);
            this.groupBox1.Location = new System.Drawing.Point(4, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(649, 406);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "桌面图像";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.textBoxSend);
            this.groupBox2.Controls.Add(this.textBoxDif);
            this.groupBox2.Controls.Add(this.textBoxCopy);
            this.groupBox2.Controls.Add(this.textBoxTimeShow);
            this.groupBox2.Controls.Add(this.buttonAverageTest);
            this.groupBox2.Controls.Add(this.buttonBitBltTest);
            this.groupBox2.Controls.Add(this.textBoxInfoShow);
            this.groupBox2.Controls.Add(this.buttonLocalTest);
            this.groupBox2.Controls.Add(this.buttonStartSendPic);
            this.groupBox2.Controls.Add(this.buttonStartSocket);
            this.groupBox2.Location = new System.Drawing.Point(5, 418);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(648, 132);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "操作";
            // 
            // textBoxSend
            // 
            this.textBoxSend.Location = new System.Drawing.Point(257, 98);
            this.textBoxSend.Name = "textBoxSend";
            this.textBoxSend.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxSend.Size = new System.Drawing.Size(100, 21);
            this.textBoxSend.TabIndex = 5;
            // 
            // textBoxDif
            // 
            this.textBoxDif.Location = new System.Drawing.Point(257, 70);
            this.textBoxDif.Name = "textBoxDif";
            this.textBoxDif.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxDif.Size = new System.Drawing.Size(100, 21);
            this.textBoxDif.TabIndex = 5;
            // 
            // textBoxCopy
            // 
            this.textBoxCopy.Location = new System.Drawing.Point(257, 43);
            this.textBoxCopy.Name = "textBoxCopy";
            this.textBoxCopy.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxCopy.Size = new System.Drawing.Size(100, 21);
            this.textBoxCopy.TabIndex = 5;
            // 
            // textBoxTimeShow
            // 
            this.textBoxTimeShow.Location = new System.Drawing.Point(257, 16);
            this.textBoxTimeShow.Name = "textBoxTimeShow";
            this.textBoxTimeShow.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxTimeShow.Size = new System.Drawing.Size(100, 21);
            this.textBoxTimeShow.TabIndex = 5;
            // 
            // buttonAverageTest
            // 
            this.buttonAverageTest.Location = new System.Drawing.Point(506, 88);
            this.buttonAverageTest.Name = "buttonAverageTest";
            this.buttonAverageTest.Size = new System.Drawing.Size(105, 25);
            this.buttonAverageTest.TabIndex = 4;
            this.buttonAverageTest.Text = "压力测试";
            this.buttonAverageTest.UseVisualStyleBackColor = true;
            this.buttonAverageTest.Click += new System.EventHandler(this.buttonAverageTest_Click);
            // 
            // buttonBitBltTest
            // 
            this.buttonBitBltTest.Location = new System.Drawing.Point(506, 53);
            this.buttonBitBltTest.Name = "buttonBitBltTest";
            this.buttonBitBltTest.Size = new System.Drawing.Size(106, 25);
            this.buttonBitBltTest.TabIndex = 3;
            this.buttonBitBltTest.Text = "BitBlt测试";
            this.buttonBitBltTest.UseVisualStyleBackColor = true;
            this.buttonBitBltTest.Click += new System.EventHandler(this.buttonBitBltTest_Click);
            // 
            // textBoxInfoShow
            // 
            this.textBoxInfoShow.Location = new System.Drawing.Point(10, 16);
            this.textBoxInfoShow.Multiline = true;
            this.textBoxInfoShow.Name = "textBoxInfoShow";
            this.textBoxInfoShow.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxInfoShow.Size = new System.Drawing.Size(241, 108);
            this.textBoxInfoShow.TabIndex = 2;
            // 
            // buttonLocalTest
            // 
            this.buttonLocalTest.Location = new System.Drawing.Point(506, 20);
            this.buttonLocalTest.Name = "buttonLocalTest";
            this.buttonLocalTest.Size = new System.Drawing.Size(106, 27);
            this.buttonLocalTest.TabIndex = 1;
            this.buttonLocalTest.Text = "CopyFrom测试";
            this.buttonLocalTest.UseVisualStyleBackColor = true;
            this.buttonLocalTest.Click += new System.EventHandler(this.buttonLocalTest_Click);
            // 
            // buttonStartSendPic
            // 
            this.buttonStartSendPic.Location = new System.Drawing.Point(382, 82);
            this.buttonStartSendPic.Name = "buttonStartSendPic";
            this.buttonStartSendPic.Size = new System.Drawing.Size(111, 37);
            this.buttonStartSendPic.TabIndex = 1;
            this.buttonStartSendPic.Text = "发送图像";
            this.buttonStartSendPic.UseVisualStyleBackColor = true;
            this.buttonStartSendPic.Click += new System.EventHandler(this.buttonStartSendPic_Click);
            // 
            // buttonStartSocket
            // 
            this.buttonStartSocket.Location = new System.Drawing.Point(382, 20);
            this.buttonStartSocket.Name = "buttonStartSocket";
            this.buttonStartSocket.Size = new System.Drawing.Size(111, 37);
            this.buttonStartSocket.TabIndex = 0;
            this.buttonStartSocket.Text = "启动Socket";
            this.buttonStartSocket.UseVisualStyleBackColor = true;
            this.buttonStartSocket.Click += new System.EventHandler(this.buttonStartSocket_Click);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 20;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // ServerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(656, 563);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ServerForm";
            this.Text = "图像差异传输测试服务端";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSender)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBoxSender;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox textBoxInfoShow;
        private System.Windows.Forms.Button buttonStartSendPic;
        private System.Windows.Forms.Button buttonStartSocket;
        private System.Windows.Forms.Button buttonLocalTest;
        private System.Windows.Forms.Button buttonBitBltTest;
        private System.Windows.Forms.Button buttonAverageTest;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.TextBox textBoxTimeShow;
        private System.Windows.Forms.TextBox textBoxCopy;
        private System.Windows.Forms.TextBox textBoxSend;
        private System.Windows.Forms.TextBox textBoxDif;
    }
}


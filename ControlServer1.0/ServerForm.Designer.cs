using System;
using System.Drawing;
using System.Windows.Forms;
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
            try
            {
                base.Dispose(disposing);
            }
            catch(Exception e){}
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ServerForm));
            this.buttonClose = new System.Windows.Forms.Button();
            this.panelTop = new System.Windows.Forms.Panel();
            this.labelTop = new System.Windows.Forms.Label();
            this.buttonMin = new System.Windows.Forms.Button();
            this.panelBackground = new System.Windows.Forms.Panel();
            this.buttonServer = new System.Windows.Forms.Button();
            this.panelLine = new System.Windows.Forms.Panel();
            this.panelClient = new System.Windows.Forms.Panel();
            this.labelAddr = new System.Windows.Forms.Label();
            this.textBoxHost = new System.Windows.Forms.TextBox();
            this.labelHost = new System.Windows.Forms.Label();
            this.textBoxAddr = new System.Windows.Forms.TextBox();
            this.labelClientInfo = new System.Windows.Forms.Label();
            this.panelServer = new System.Windows.Forms.Panel();
            this.labelSDQ = new System.Windows.Forms.Label();
            this.labelFPS = new System.Windows.Forms.Label();
            this.labelDBQ = new System.Windows.Forms.Label();
            this.textBoxFPS = new System.Windows.Forms.TextBox();
            this.labelCSQ = new System.Windows.Forms.Label();
            this.textBoxDBQ = new System.Windows.Forms.TextBox();
            this.textBoxCSQ = new System.Windows.Forms.TextBox();
            this.textBoxSDQ = new System.Windows.Forms.TextBox();
            this.timerGC = new System.Windows.Forms.Timer(this.components);
            this.panelTop.SuspendLayout();
            this.panelBackground.SuspendLayout();
            this.panelClient.SuspendLayout();
            this.panelServer.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonClose
            // 
            this.buttonClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(158)))), ((int)(((byte)(146)))));
            this.buttonClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.buttonClose.Cursor = System.Windows.Forms.Cursors.Hand;
            this.buttonClose.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(158)))), ((int)(((byte)(146)))));
            this.buttonClose.FlatAppearance.BorderSize = 0;
            this.buttonClose.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Red;
            this.buttonClose.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Red;
            this.buttonClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonClose.Font = new System.Drawing.Font("仿宋", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(134)));
            this.buttonClose.ForeColor = System.Drawing.Color.White;
            this.buttonClose.Location = new System.Drawing.Point(296, 0);
            this.buttonClose.Margin = new System.Windows.Forms.Padding(0);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(26, 23);
            this.buttonClose.TabIndex = 0;
            this.buttonClose.Text = "×";
            this.buttonClose.UseVisualStyleBackColor = false;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // panelTop
            // 
            this.panelTop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.panelTop.Controls.Add(this.labelTop);
            this.panelTop.Controls.Add(this.buttonMin);
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Margin = new System.Windows.Forms.Padding(0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(322, 50);
            this.panelTop.TabIndex = 1;
            this.panelTop.MouseDown += new System.Windows.Forms.MouseEventHandler(this.top_MouseDown);
            this.panelTop.MouseMove += new System.Windows.Forms.MouseEventHandler(this.top_MouseMove);
            // 
            // labelTop
            // 
            this.labelTop.AutoSize = true;
            this.labelTop.CausesValidation = false;
            this.labelTop.Font = new System.Drawing.Font("宋体", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelTop.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(183)))), ((int)(((byte)(183)))));
            this.labelTop.Location = new System.Drawing.Point(56, 9);
            this.labelTop.Name = "labelTop";
            this.labelTop.Size = new System.Drawing.Size(207, 27);
            this.labelTop.TabIndex = 0;
            this.labelTop.Text = "Remote Server";
            this.labelTop.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelTop.MouseDown += new System.Windows.Forms.MouseEventHandler(this.top_MouseDown);
            this.labelTop.MouseMove += new System.Windows.Forms.MouseEventHandler(this.top_MouseMove);
            // 
            // buttonMin
            // 
            this.buttonMin.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(158)))), ((int)(((byte)(146)))));
            this.buttonMin.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.buttonMin.Cursor = System.Windows.Forms.Cursors.Hand;
            this.buttonMin.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(158)))), ((int)(((byte)(146)))));
            this.buttonMin.FlatAppearance.BorderSize = 0;
            this.buttonMin.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Red;
            this.buttonMin.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Red;
            this.buttonMin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonMin.Font = new System.Drawing.Font("仿宋", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(134)));
            this.buttonMin.ForeColor = System.Drawing.Color.White;
            this.buttonMin.Location = new System.Drawing.Point(269, 0);
            this.buttonMin.Margin = new System.Windows.Forms.Padding(0);
            this.buttonMin.Name = "buttonMin";
            this.buttonMin.Size = new System.Drawing.Size(26, 23);
            this.buttonMin.TabIndex = 0;
            this.buttonMin.Text = "﹣";
            this.buttonMin.UseVisualStyleBackColor = false;
            this.buttonMin.Click += new System.EventHandler(this.buttonMin_Click);
            // 
            // panelBackground
            // 
            this.panelBackground.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.panelBackground.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panelBackground.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelBackground.Controls.Add(this.buttonServer);
            this.panelBackground.Controls.Add(this.panelLine);
            this.panelBackground.Controls.Add(this.panelClient);
            this.panelBackground.Controls.Add(this.labelClientInfo);
            this.panelBackground.Controls.Add(this.panelServer);
            this.panelBackground.Location = new System.Drawing.Point(0, 50);
            this.panelBackground.Margin = new System.Windows.Forms.Padding(0);
            this.panelBackground.Name = "panelBackground";
            this.panelBackground.Size = new System.Drawing.Size(322, 311);
            this.panelBackground.TabIndex = 2;
            // 
            // buttonServer
            // 
            this.buttonServer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(158)))), ((int)(((byte)(146)))));
            this.buttonServer.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.buttonServer.Cursor = System.Windows.Forms.Cursors.Hand;
            this.buttonServer.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(158)))), ((int)(((byte)(146)))));
            this.buttonServer.FlatAppearance.BorderSize = 0;
            this.buttonServer.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Red;
            this.buttonServer.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Red;
            this.buttonServer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonServer.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.buttonServer.ForeColor = System.Drawing.Color.White;
            this.buttonServer.Location = new System.Drawing.Point(35, 261);
            this.buttonServer.Margin = new System.Windows.Forms.Padding(0);
            this.buttonServer.Name = "buttonServer";
            this.buttonServer.Size = new System.Drawing.Size(250, 37);
            this.buttonServer.TabIndex = 6;
            this.buttonServer.Text = "START SERVER";
            this.buttonServer.UseVisualStyleBackColor = false;
            this.buttonServer.Click += new System.EventHandler(this.buttonServer_Click);
            // 
            // panelLine
            // 
            this.panelLine.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(123)))), ((int)(((byte)(123)))), ((int)(((byte)(123)))));
            this.panelLine.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panelLine.Location = new System.Drawing.Point(2, 153);
            this.panelLine.Margin = new System.Windows.Forms.Padding(0);
            this.panelLine.Name = "panelLine";
            this.panelLine.Size = new System.Drawing.Size(316, 1);
            this.panelLine.TabIndex = 4;
            // 
            // panelClient
            // 
            this.panelClient.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelClient.Controls.Add(this.labelAddr);
            this.panelClient.Controls.Add(this.textBoxHost);
            this.panelClient.Controls.Add(this.labelHost);
            this.panelClient.Controls.Add(this.textBoxAddr);
            this.panelClient.Location = new System.Drawing.Point(19, 37);
            this.panelClient.Name = "panelClient";
            this.panelClient.Size = new System.Drawing.Size(280, 100);
            this.panelClient.TabIndex = 3;
            // 
            // labelAddr
            // 
            this.labelAddr.AutoSize = true;
            this.labelAddr.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold);
            this.labelAddr.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(183)))), ((int)(((byte)(183)))));
            this.labelAddr.Location = new System.Drawing.Point(7, 57);
            this.labelAddr.Name = "labelAddr";
            this.labelAddr.Size = new System.Drawing.Size(44, 16);
            this.labelAddr.TabIndex = 1;
            this.labelAddr.Text = "ADDR";
            // 
            // textBoxHost
            // 
            this.textBoxHost.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.textBoxHost.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxHost.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBoxHost.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(183)))), ((int)(((byte)(183)))));
            this.textBoxHost.Location = new System.Drawing.Point(51, 17);
            this.textBoxHost.Margin = new System.Windows.Forms.Padding(0);
            this.textBoxHost.Name = "textBoxHost";
            this.textBoxHost.Size = new System.Drawing.Size(216, 29);
            this.textBoxHost.TabIndex = 0;
            this.textBoxHost.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // labelHost
            // 
            this.labelHost.AutoSize = true;
            this.labelHost.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold);
            this.labelHost.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(183)))), ((int)(((byte)(183)))));
            this.labelHost.Location = new System.Drawing.Point(7, 23);
            this.labelHost.Name = "labelHost";
            this.labelHost.Size = new System.Drawing.Size(44, 16);
            this.labelHost.TabIndex = 1;
            this.labelHost.Text = "HOST";
            // 
            // textBoxAddr
            // 
            this.textBoxAddr.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.textBoxAddr.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxAddr.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBoxAddr.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(183)))), ((int)(((byte)(183)))));
            this.textBoxAddr.Location = new System.Drawing.Point(51, 52);
            this.textBoxAddr.Margin = new System.Windows.Forms.Padding(0);
            this.textBoxAddr.Name = "textBoxAddr";
            this.textBoxAddr.Size = new System.Drawing.Size(216, 29);
            this.textBoxAddr.TabIndex = 0;
            this.textBoxAddr.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // labelClientInfo
            // 
            this.labelClientInfo.AutoSize = true;
            this.labelClientInfo.Font = new System.Drawing.Font("宋体", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelClientInfo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(183)))), ((int)(((byte)(183)))));
            this.labelClientInfo.Location = new System.Drawing.Point(99, 11);
            this.labelClientInfo.Name = "labelClientInfo";
            this.labelClientInfo.Size = new System.Drawing.Size(118, 18);
            this.labelClientInfo.TabIndex = 2;
            this.labelClientInfo.Text = "CLIENT INFO";
            // 
            // panelServer
            // 
            this.panelServer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelServer.Controls.Add(this.labelSDQ);
            this.panelServer.Controls.Add(this.labelFPS);
            this.panelServer.Controls.Add(this.labelDBQ);
            this.panelServer.Controls.Add(this.textBoxFPS);
            this.panelServer.Controls.Add(this.labelCSQ);
            this.panelServer.Controls.Add(this.textBoxDBQ);
            this.panelServer.Controls.Add(this.textBoxCSQ);
            this.panelServer.Controls.Add(this.textBoxSDQ);
            this.panelServer.Location = new System.Drawing.Point(19, 166);
            this.panelServer.Name = "panelServer";
            this.panelServer.Size = new System.Drawing.Size(280, 86);
            this.panelServer.TabIndex = 8;
            // 
            // labelSDQ
            // 
            this.labelSDQ.AutoSize = true;
            this.labelSDQ.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Bold);
            this.labelSDQ.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(183)))), ((int)(((byte)(183)))));
            this.labelSDQ.Location = new System.Drawing.Point(149, 56);
            this.labelSDQ.Name = "labelSDQ";
            this.labelSDQ.Size = new System.Drawing.Size(31, 14);
            this.labelSDQ.TabIndex = 7;
            this.labelSDQ.Text = "SDQ";
            // 
            // labelFPS
            // 
            this.labelFPS.AutoSize = true;
            this.labelFPS.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Bold);
            this.labelFPS.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(183)))), ((int)(((byte)(183)))));
            this.labelFPS.Location = new System.Drawing.Point(33, 13);
            this.labelFPS.Name = "labelFPS";
            this.labelFPS.Size = new System.Drawing.Size(31, 14);
            this.labelFPS.TabIndex = 7;
            this.labelFPS.Text = "FPS";
            // 
            // labelDBQ
            // 
            this.labelDBQ.AutoSize = true;
            this.labelDBQ.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Bold);
            this.labelDBQ.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(183)))), ((int)(((byte)(183)))));
            this.labelDBQ.Location = new System.Drawing.Point(149, 13);
            this.labelDBQ.Name = "labelDBQ";
            this.labelDBQ.Size = new System.Drawing.Size(31, 14);
            this.labelDBQ.TabIndex = 7;
            this.labelDBQ.Text = "DBQ";
            // 
            // textBoxFPS
            // 
            this.textBoxFPS.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.textBoxFPS.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxFPS.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBoxFPS.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(183)))), ((int)(((byte)(183)))));
            this.textBoxFPS.Location = new System.Drawing.Point(69, 10);
            this.textBoxFPS.Name = "textBoxFPS";
            this.textBoxFPS.Size = new System.Drawing.Size(49, 23);
            this.textBoxFPS.TabIndex = 5;
            this.textBoxFPS.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // labelCSQ
            // 
            this.labelCSQ.AutoSize = true;
            this.labelCSQ.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Bold);
            this.labelCSQ.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(183)))), ((int)(((byte)(183)))));
            this.labelCSQ.Location = new System.Drawing.Point(33, 56);
            this.labelCSQ.Name = "labelCSQ";
            this.labelCSQ.Size = new System.Drawing.Size(31, 14);
            this.labelCSQ.TabIndex = 7;
            this.labelCSQ.Text = "CSQ";
            // 
            // textBoxDBQ
            // 
            this.textBoxDBQ.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.textBoxDBQ.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxDBQ.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBoxDBQ.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(183)))), ((int)(((byte)(183)))));
            this.textBoxDBQ.Location = new System.Drawing.Point(186, 9);
            this.textBoxDBQ.Name = "textBoxDBQ";
            this.textBoxDBQ.Size = new System.Drawing.Size(49, 23);
            this.textBoxDBQ.TabIndex = 5;
            this.textBoxDBQ.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBoxCSQ
            // 
            this.textBoxCSQ.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.textBoxCSQ.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxCSQ.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBoxCSQ.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(183)))), ((int)(((byte)(183)))));
            this.textBoxCSQ.Location = new System.Drawing.Point(69, 52);
            this.textBoxCSQ.Name = "textBoxCSQ";
            this.textBoxCSQ.Size = new System.Drawing.Size(49, 23);
            this.textBoxCSQ.TabIndex = 5;
            this.textBoxCSQ.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBoxSDQ
            // 
            this.textBoxSDQ.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.textBoxSDQ.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxSDQ.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBoxSDQ.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(183)))), ((int)(((byte)(183)))));
            this.textBoxSDQ.Location = new System.Drawing.Point(186, 52);
            this.textBoxSDQ.Name = "textBoxSDQ";
            this.textBoxSDQ.Size = new System.Drawing.Size(49, 23);
            this.textBoxSDQ.TabIndex = 5;
            this.textBoxSDQ.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // timerGC
            // 
            this.timerGC.Enabled = true;
            this.timerGC.Interval = 50;
            this.timerGC.Tick += new System.EventHandler(this.timerGC_Tick);
            // 
            // ServerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(322, 360);
            this.Controls.Add(this.panelBackground);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.panelTop);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ServerForm";
            this.Text = "服务端";
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            this.panelBackground.ResumeLayout(false);
            this.panelBackground.PerformLayout();
            this.panelClient.ResumeLayout(false);
            this.panelClient.PerformLayout();
            this.panelServer.ResumeLayout(false);
            this.panelServer.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Panel panelBackground;
        private System.Windows.Forms.Label labelTop;
        private System.Windows.Forms.TextBox textBoxHost;
        private System.Windows.Forms.TextBox textBoxAddr;
        private System.Windows.Forms.Label labelAddr;
        private System.Windows.Forms.Label labelHost;
        private Label labelClientInfo;
        private Panel panelClient;
        private Panel panelLine;
        private Button buttonServer;
        private TextBox textBoxSDQ;
        private TextBox textBoxCSQ;
        private TextBox textBoxDBQ;
        private TextBox textBoxFPS;
        private Label labelCSQ;
        private Label labelFPS;
        private Label labelSDQ;
        private Label labelDBQ;
        private Panel panelServer;
        private Button buttonMin;
        private Timer timerGC;    
    
    }
}


namespace ComparerTest
{
    partial class Form1
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.文件ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.打开图像1ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.打开图像2ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.比较图像ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.测试内存截图ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.文件ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(580, 25);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 文件ToolStripMenuItem
            // 
            this.文件ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.打开图像1ToolStripMenuItem,
            this.打开图像2ToolStripMenuItem,
            this.toolStripMenuItem1,
            this.比较图像ToolStripMenuItem,
            this.测试内存截图ToolStripMenuItem});
            this.文件ToolStripMenuItem.Name = "文件ToolStripMenuItem";
            this.文件ToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
            this.文件ToolStripMenuItem.Text = "操作";
            // 
            // 打开图像1ToolStripMenuItem
            // 
            this.打开图像1ToolStripMenuItem.Name = "打开图像1ToolStripMenuItem";
            this.打开图像1ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.打开图像1ToolStripMenuItem.Text = "装载图像1";
            this.打开图像1ToolStripMenuItem.Click += new System.EventHandler(this.打开图像1ToolStripMenuItem_Click);
            // 
            // 打开图像2ToolStripMenuItem
            // 
            this.打开图像2ToolStripMenuItem.Name = "打开图像2ToolStripMenuItem";
            this.打开图像2ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.打开图像2ToolStripMenuItem.Text = "装载图像2";
            this.打开图像2ToolStripMenuItem.Click += new System.EventHandler(this.打开图像2ToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(149, 6);
            // 
            // 比较图像ToolStripMenuItem
            // 
            this.比较图像ToolStripMenuItem.Name = "比较图像ToolStripMenuItem";
            this.比较图像ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.比较图像ToolStripMenuItem.Text = "比较图像";
            this.比较图像ToolStripMenuItem.Click += new System.EventHandler(this.比较图像ToolStripMenuItem_Click);
            // 
            // 测试内存截图ToolStripMenuItem
            // 
            this.测试内存截图ToolStripMenuItem.Name = "测试内存截图ToolStripMenuItem";
            this.测试内存截图ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.测试内存截图ToolStripMenuItem.Text = "测试内存截图";
            this.测试内存截图ToolStripMenuItem.Click += new System.EventHandler(this.测试内存截图ToolStripMenuItem_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(580, 507);
            this.Controls.Add(this.menuStrip1);
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "图像比较";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 文件ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 打开图像1ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 打开图像2ToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem 比较图像ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 测试内存截图ToolStripMenuItem;
    }
}


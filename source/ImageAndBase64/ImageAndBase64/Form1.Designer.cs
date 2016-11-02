namespace ImageAndBase64
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
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btnImageOpen = new System.Windows.Forms.Button();
            this.btnImageSave = new System.Windows.Forms.Button();
            this.btnBase64Open = new System.Windows.Forms.Button();
            this.btnBase64Save = new System.Windows.Forms.Button();
            this.lblImageInfo = new System.Windows.Forms.Label();
            this.lblBase64Info = new System.Windows.Forms.Label();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.btnCleanMemory = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(12, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(200, 200);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // btnImageOpen
            // 
            this.btnImageOpen.Location = new System.Drawing.Point(12, 235);
            this.btnImageOpen.Name = "btnImageOpen";
            this.btnImageOpen.Size = new System.Drawing.Size(39, 23);
            this.btnImageOpen.TabIndex = 0;
            this.btnImageOpen.Text = "&open";
            this.btnImageOpen.UseVisualStyleBackColor = true;
            this.btnImageOpen.Click += new System.EventHandler(this.btnImageOpen_Click);
            // 
            // btnImageSave
            // 
            this.btnImageSave.Location = new System.Drawing.Point(57, 235);
            this.btnImageSave.Name = "btnImageSave";
            this.btnImageSave.Size = new System.Drawing.Size(39, 23);
            this.btnImageSave.TabIndex = 1;
            this.btnImageSave.Text = "&save";
            this.btnImageSave.UseVisualStyleBackColor = true;
            this.btnImageSave.Click += new System.EventHandler(this.btnImageSave_Click);
            // 
            // btnBase64Open
            // 
            this.btnBase64Open.Location = new System.Drawing.Point(218, 235);
            this.btnBase64Open.Name = "btnBase64Open";
            this.btnBase64Open.Size = new System.Drawing.Size(39, 23);
            this.btnBase64Open.TabIndex = 4;
            this.btnBase64Open.Text = "o&pen";
            this.btnBase64Open.UseVisualStyleBackColor = true;
            this.btnBase64Open.Click += new System.EventHandler(this.btnBase64Open_Click);
            // 
            // btnBase64Save
            // 
            this.btnBase64Save.Location = new System.Drawing.Point(263, 235);
            this.btnBase64Save.Name = "btnBase64Save";
            this.btnBase64Save.Size = new System.Drawing.Size(39, 23);
            this.btnBase64Save.TabIndex = 5;
            this.btnBase64Save.Text = "s&ave";
            this.btnBase64Save.UseVisualStyleBackColor = true;
            this.btnBase64Save.Click += new System.EventHandler(this.btnBase64Save_Click);
            // 
            // lblImageInfo
            // 
            this.lblImageInfo.AutoSize = true;
            this.lblImageInfo.Location = new System.Drawing.Point(12, 215);
            this.lblImageInfo.Name = "lblImageInfo";
            this.lblImageInfo.Size = new System.Drawing.Size(107, 12);
            this.lblImageInfo.TabIndex = 2;
            this.lblImageInfo.Text = "image information";
            // 
            // lblBase64Info
            // 
            this.lblBase64Info.AutoSize = true;
            this.lblBase64Info.Location = new System.Drawing.Point(216, 215);
            this.lblBase64Info.Name = "lblBase64Info";
            this.lblBase64Info.Size = new System.Drawing.Size(113, 12);
            this.lblBase64Info.TabIndex = 6;
            this.lblBase64Info.Text = "base64 information";
            // 
            // richTextBox1
            // 
            this.richTextBox1.BackColor = System.Drawing.SystemColors.Window;
            this.richTextBox1.DetectUrls = false;
            this.richTextBox1.Location = new System.Drawing.Point(218, 12);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ReadOnly = true;
            this.richTextBox1.Size = new System.Drawing.Size(335, 200);
            this.richTextBox1.TabIndex = 3;
            this.richTextBox1.Text = "";
            // 
            // btnCleanMemory
            // 
            this.btnCleanMemory.Location = new System.Drawing.Point(453, 235);
            this.btnCleanMemory.Name = "btnCleanMemory";
            this.btnCleanMemory.Size = new System.Drawing.Size(100, 23);
            this.btnCleanMemory.TabIndex = 7;
            this.btnCleanMemory.Text = "&Clean Memory";
            this.btnCleanMemory.UseVisualStyleBackColor = true;
            this.btnCleanMemory.Click += new System.EventHandler(this.btnCleanMemory_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(565, 266);
            this.Controls.Add(this.btnCleanMemory);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.btnBase64Save);
            this.Controls.Add(this.btnImageSave);
            this.Controls.Add(this.btnBase64Open);
            this.Controls.Add(this.btnImageOpen);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.lblImageInfo);
            this.Controls.Add(this.lblBase64Info);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "ImageAndBase64";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button btnImageOpen;
        private System.Windows.Forms.Button btnImageSave;
        private System.Windows.Forms.Button btnBase64Open;
        private System.Windows.Forms.Button btnBase64Save;
        private System.Windows.Forms.Label lblImageInfo;
        private System.Windows.Forms.Label lblBase64Info;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Button btnCleanMemory;
    }
}


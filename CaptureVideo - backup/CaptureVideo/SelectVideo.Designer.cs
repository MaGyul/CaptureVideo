namespace CaptureVideo
{
    partial class SelectVideo
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
            this.videoSelect = new System.Windows.Forms.ComboBox();
            this.audioSelect = new System.Windows.Forms.ComboBox();
            this.okBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // videoSelect
            // 
            this.videoSelect.FormattingEnabled = true;
            this.videoSelect.Location = new System.Drawing.Point(36, 41);
            this.videoSelect.Name = "videoSelect";
            this.videoSelect.Size = new System.Drawing.Size(230, 20);
            this.videoSelect.TabIndex = 0;
            // 
            // audioSelect
            // 
            this.audioSelect.FormattingEnabled = true;
            this.audioSelect.Location = new System.Drawing.Point(36, 116);
            this.audioSelect.Name = "audioSelect";
            this.audioSelect.Size = new System.Drawing.Size(230, 20);
            this.audioSelect.TabIndex = 1;
            // 
            // okBtn
            // 
            this.okBtn.Location = new System.Drawing.Point(98, 198);
            this.okBtn.Name = "okBtn";
            this.okBtn.Size = new System.Drawing.Size(100, 30);
            this.okBtn.TabIndex = 2;
            this.okBtn.Text = "연결";
            this.okBtn.UseVisualStyleBackColor = true;
            this.okBtn.Click += new System.EventHandler(this.okBtn_Click);
            // 
            // SelectVideo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(294, 281);
            this.Controls.Add(this.okBtn);
            this.Controls.Add(this.audioSelect);
            this.Controls.Add(this.videoSelect);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(310, 320);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(310, 320);
            this.Name = "SelectVideo";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "비디오 캡쳐 선택";
            this.Load += new System.EventHandler(this.LoadEvent);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox videoSelect;
        private System.Windows.Forms.ComboBox audioSelect;
        private System.Windows.Forms.Button okBtn;
    }
}
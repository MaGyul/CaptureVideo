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
            this._a = new System.Windows.Forms.Label();
            this._b = new System.Windows.Forms.Label();
            this._c = new System.Windows.Forms.Label();
            this.outSelect = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // videoSelect
            // 
            this.videoSelect.FormattingEnabled = true;
            this.videoSelect.Location = new System.Drawing.Point(36, 41);
            this.videoSelect.Name = "videoSelect";
            this.videoSelect.Size = new System.Drawing.Size(230, 20);
            this.videoSelect.TabIndex = 0;
            this.videoSelect.Text = "캡쳐 장치 선택";
            // 
            // audioSelect
            // 
            this.audioSelect.FormattingEnabled = true;
            this.audioSelect.Location = new System.Drawing.Point(36, 87);
            this.audioSelect.Name = "audioSelect";
            this.audioSelect.Size = new System.Drawing.Size(230, 20);
            this.audioSelect.TabIndex = 1;
            this.audioSelect.Text = "입력 장치 선택";
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
            // _a
            // 
            this._a.AutoSize = true;
            this._a.Location = new System.Drawing.Point(34, 26);
            this._a.Name = "_a";
            this._a.Size = new System.Drawing.Size(57, 12);
            this._a.TabIndex = 3;
            this._a.Text = "캡쳐 장치";
            // 
            // _b
            // 
            this._b.AutoSize = true;
            this._b.Location = new System.Drawing.Point(34, 72);
            this._b.Name = "_b";
            this._b.Size = new System.Drawing.Size(29, 12);
            this._b.TabIndex = 4;
            this._b.Text = "입력";
            // 
            // _c
            // 
            this._c.AutoSize = true;
            this._c.Location = new System.Drawing.Point(34, 119);
            this._c.Name = "_c";
            this._c.Size = new System.Drawing.Size(29, 12);
            this._c.TabIndex = 6;
            this._c.Text = "출력";
            // 
            // outSelect
            // 
            this.outSelect.FormattingEnabled = true;
            this.outSelect.Location = new System.Drawing.Point(36, 134);
            this.outSelect.Name = "outSelect";
            this.outSelect.Size = new System.Drawing.Size(230, 20);
            this.outSelect.TabIndex = 5;
            this.outSelect.Text = "출럭 장치 선택";
            // 
            // SelectVideo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(294, 281);
            this.Controls.Add(this._c);
            this.Controls.Add(this.outSelect);
            this.Controls.Add(this._b);
            this.Controls.Add(this._a);
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
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox videoSelect;
        private System.Windows.Forms.ComboBox audioSelect;
        private System.Windows.Forms.Button okBtn;
        private System.Windows.Forms.Label _a;
        private System.Windows.Forms.Label _b;
        private System.Windows.Forms.Label _c;
        private System.Windows.Forms.ComboBox outSelect;
    }
}
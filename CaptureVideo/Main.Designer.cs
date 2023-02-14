namespace CaptureVideo
{
    partial class Main
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
            System.Windows.Forms.ToolStripMenuItem __;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.videoOpenCM = new System.Windows.Forms.ToolStripMenuItem();
            this.fullscreenCM = new System.Windows.Forms.ToolStripMenuItem();
            this.shutdownCM = new System.Windows.Forms.ToolStripMenuItem();
            this._ = new System.Windows.Forms.ToolStripSeparator();
            this.frameTime = new System.Windows.Forms.ToolStripTextBox();
            this.___ = new System.Windows.Forms.ToolStripSeparator();
            this.____ = new System.Windows.Forms.ToolStripMenuItem();
            this.openCapture = new System.Windows.Forms.Label();
            this.trackBarCM = new CaptureVideo.TrackBarMenuItem();
            __ = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // __
            // 
            __.Enabled = false;
            __.Name = "__";
            __.Size = new System.Drawing.Size(166, 22);
            __.Text = "프레임 조절 ↓";
            // 
            // contextMenu
            // 
            this.contextMenu.BackColor = System.Drawing.SystemColors.Control;
            this.contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.videoOpenCM,
            this.fullscreenCM,
            this.shutdownCM,
            this._,
            __,
            this.frameTime,
            this.___,
            this.____,
            this.trackBarCM});
            this.contextMenu.Name = "contextMenu";
            this.contextMenu.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.contextMenu.Size = new System.Drawing.Size(167, 196);
            this.contextMenu.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.MenuItemClicked);
            // 
            // videoOpenCM
            // 
            this.videoOpenCM.Name = "videoOpenCM";
            this.videoOpenCM.Size = new System.Drawing.Size(166, 22);
            this.videoOpenCM.Text = "캡쳐 비디오 열기";
            // 
            // fullscreenCM
            // 
            this.fullscreenCM.Name = "fullscreenCM";
            this.fullscreenCM.ShortcutKeys = System.Windows.Forms.Keys.F11;
            this.fullscreenCM.Size = new System.Drawing.Size(166, 22);
            this.fullscreenCM.Text = "전체화면";
            this.fullscreenCM.ToolTipText = "창을 전체화면으로 바꿉니다.";
            // 
            // shutdownCM
            // 
            this.shutdownCM.Name = "shutdownCM";
            this.shutdownCM.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.shutdownCM.Size = new System.Drawing.Size(166, 22);
            this.shutdownCM.Text = "종료";
            // 
            // _
            // 
            this._.Name = "_";
            this._.Size = new System.Drawing.Size(163, 6);
            // 
            // frameTime
            // 
            this.frameTime.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.frameTime.Name = "frameTime";
            this.frameTime.Size = new System.Drawing.Size(100, 23);
            this.frameTime.Text = "60";
            // 
            // ___
            // 
            this.___.Name = "___";
            this.___.Size = new System.Drawing.Size(163, 6);
            this.___.Visible = false;
            // 
            // ____
            // 
            this.____.Enabled = false;
            this.____.Name = "____";
            this.____.Size = new System.Drawing.Size(166, 22);
            this.____.Text = "볼륨 조절 ↓";
            // 
            // openCapture
            // 
            this.openCapture.Dock = System.Windows.Forms.DockStyle.Fill;
            this.openCapture.Font = new System.Drawing.Font("메이플스토리 OTF", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.openCapture.Location = new System.Drawing.Point(0, 0);
            this.openCapture.Name = "openCapture";
            this.openCapture.Size = new System.Drawing.Size(838, 441);
            this.openCapture.TabIndex = 1;
            this.openCapture.Text = "클릭을 해서 캡쳐 장지 설정";
            this.openCapture.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.openCapture.MouseClick += new System.Windows.Forms.MouseEventHandler(this.CMouseClick);
            // 
            // trackBarCM
            // 
            this.trackBarCM.Margin = new System.Windows.Forms.Padding(0);
            this.trackBarCM.Name = "trackBarCM";
            this.trackBarCM.Size = new System.Drawing.Size(104, 45);
            this.trackBarCM.Text = "trackBar";
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(838, 441);
            this.Controls.Add(this.openCapture);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(854, 480);
            this.Name = "Main";
            this.Text = "CaptureVideo";
            this.Load += new System.EventHandler(this.OnLoad);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.KeyUpEvent);
            this.contextMenu.ResumeLayout(false);
            this.contextMenu.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip contextMenu;
        private System.Windows.Forms.ToolStripMenuItem videoOpenCM;
        public System.Windows.Forms.ToolStripMenuItem fullscreenCM;
        public System.Windows.Forms.ToolStripTextBox frameTime;
        private System.Windows.Forms.ToolStripSeparator _;
        private System.Windows.Forms.Label openCapture;
        private System.Windows.Forms.ToolStripSeparator ___;
        private System.Windows.Forms.ToolStripMenuItem ____;
        private System.Windows.Forms.ToolStripMenuItem shutdownCM;
        public TrackBarMenuItem trackBarCM;
    }
}
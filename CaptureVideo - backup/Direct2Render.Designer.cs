namespace CaptureVideo
{
    partial class Direct2Render
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
            System.Windows.Forms.ToolStripMenuItem a;
            this.contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.videoOpenCM = new System.Windows.Forms.ToolStripMenuItem();
            this.fullscreenCM = new System.Windows.Forms.ToolStripMenuItem();
            this.frameTime = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.openCapture = new System.Windows.Forms.Label();
            a = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenu
            // 
            this.contextMenu.BackColor = System.Drawing.SystemColors.Control;
            this.contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.videoOpenCM,
            this.fullscreenCM,
            this.toolStripSeparator1,
            a,
            this.frameTime});
            this.contextMenu.Name = "contextMenu";
            this.contextMenu.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.contextMenu.Size = new System.Drawing.Size(167, 101);
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
            // frameTime
            // 
            this.frameTime.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.frameTime.Name = "frameTime";
            this.frameTime.Size = new System.Drawing.Size(100, 23);
            this.frameTime.Text = "60";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(163, 6);
            // 
            // a
            // 
            a.Enabled = false;
            a.Name = "a";
            a.Size = new System.Drawing.Size(166, 22);
            a.Text = "프레임 조절 ↓";
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
            // Direct2Render
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(838, 441);
            this.Controls.Add(this.openCapture);
            this.MinimumSize = new System.Drawing.Size(854, 480);
            this.Name = "Direct2Render";
            this.Text = "Direct2Render";
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
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.Label openCapture;
    }
}
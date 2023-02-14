namespace CaptureVideo
{
    partial class Main
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.bWorker = new System.ComponentModel.BackgroundWorker();
            this.contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.videoOpenCM = new System.Windows.Forms.ToolStripMenuItem();
            this.fullscreenCM = new System.Windows.Forms.ToolStripMenuItem();
            this.videoResetCM = new System.Windows.Forms.ToolStripMenuItem();
            this.threadDelay = new System.Windows.Forms.ToolStripTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.contextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox
            // 
            this.pictureBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox.Location = new System.Drawing.Point(0, 0);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(838, 441);
            this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox.TabIndex = 0;
            this.pictureBox.TabStop = false;
            this.pictureBox.MouseClick += new System.Windows.Forms.MouseEventHandler(this.PictureBoxClickEvent);
            // 
            // bWorker
            // 
            this.bWorker.WorkerReportsProgress = true;
            this.bWorker.WorkerSupportsCancellation = true;
            this.bWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.DoWorkEvent);
            this.bWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.ProgressChangedEvent);
            // 
            // contextMenu
            // 
            this.contextMenu.BackColor = System.Drawing.SystemColors.Control;
            this.contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.videoOpenCM,
            this.fullscreenCM,
            this.videoResetCM,
            this.threadDelay});
            this.contextMenu.Name = "contextMenu";
            this.contextMenu.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.contextMenu.Size = new System.Drawing.Size(223, 95);
            this.contextMenu.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.MenuItemClicked);
            // 
            // videoOpenCM
            // 
            this.videoOpenCM.Name = "videoOpenCM";
            this.videoOpenCM.Size = new System.Drawing.Size(222, 22);
            this.videoOpenCM.Text = "캡쳐 비디오 열기";
            // 
            // fullscreenCM
            // 
            this.fullscreenCM.Name = "fullscreenCM";
            this.fullscreenCM.ShortcutKeys = System.Windows.Forms.Keys.F11;
            this.fullscreenCM.Size = new System.Drawing.Size(222, 22);
            this.fullscreenCM.Text = "전체화면";
            this.fullscreenCM.ToolTipText = "창을 전체화면으로 바꿉니다.";
            // 
            // videoResetCM
            // 
            this.videoResetCM.Name = "videoResetCM";
            this.videoResetCM.Size = new System.Drawing.Size(222, 22);
            this.videoResetCM.Text = "비디오 크기에 맞춰 창 조절";
            this.videoResetCM.ToolTipText = "출럭되는 비디오 크기에 맞춰 창을 조절합니다.";
            // 
            // threadDelay
            // 
            this.threadDelay.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.threadDelay.Name = "threadDelay";
            this.threadDelay.Size = new System.Drawing.Size(100, 23);
            this.threadDelay.Text = "10";
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(838, 441);
            this.Controls.Add(this.pictureBox);
            this.MinimumSize = new System.Drawing.Size(854, 480);
            this.Name = "Main";
            this.Text = "비디오 캡쳐";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormClosingEvent);
            this.Load += new System.EventHandler(this.FormLoadEvent);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.KeyUpEvent);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.contextMenu.ResumeLayout(false);
            this.contextMenu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox;
        private System.ComponentModel.BackgroundWorker bWorker;
        private System.Windows.Forms.ContextMenuStrip contextMenu;
        public System.Windows.Forms.ToolStripMenuItem fullscreenCM;
        public System.Windows.Forms.ToolStripMenuItem videoResetCM;
        private System.Windows.Forms.ToolStripMenuItem videoOpenCM;
        public System.Windows.Forms.ToolStripTextBox threadDelay;
    }
}


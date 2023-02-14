using DirectShowLib;
using OpenCvSharp;
using OpenCvSharp.WpfExtensions;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Media.Imaging;

namespace CaptureVideo
{
    public partial class Main : Form
    {
        const double widthRatio = 16;
        const double heightRatio = 9.45;

        const int WM_SIZING = 0x214;
        const int WMSZ_LEFT = 1;
        const int WMSZ_RIGHT = 2;
        const int WMSZ_TOP = 3;
        const int WMSZ_BOTTOM = 6;

        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        private SimpleConsole console = new SimpleConsole();
        private List<DsDevice> cameraDevices;
        private Mat mat;
        private readonly VideoCapture capture;
        private System.Drawing.Point lastLoc;
        private System.Drawing.Point lastPoint;

        public Main()
        {
            InitializeComponent();
            mat = new Mat();
            capture = new VideoCapture();
            cameraDevices = new List<DsDevice>();
            Height = (int)(heightRatio * Width / widthRatio);
        }

        private void FormLoadEvent(object sender, System.EventArgs e)
        {
            cameraDevices.AddRange(from DsDevice dsDevice in DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice)
                                   where !dsDevice.DevicePath.Contains("device:sw")
                                   select dsDevice);
            if (cameraDevices.Count <= 0)
            {
                System.Windows.Forms.MessageBox.Show("등록된 비디오 캡쳐/카메라가 없습니다.", "주의", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                int index;
                if (!capture.Open(index = cameraDevices.FindIndex(d => d.Name == "USB Video Device")))
                {
                    System.Windows.Forms.MessageBox.Show(cameraDevices[index].Name + ", 연결에 실패했습니다.", "연결 실패", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    //ClientSize = new System.Drawing.Size(capture.FrameWidth, capture.FrameHeight);
                    bWorker.RunWorkerAsync();
                }
            }
        }

        private void FormClosingEvent(object sender, FormClosingEventArgs e)
        {
            bWorker.CancelAsync();
            mat.Dispose();
            capture.Dispose();
        }

        private void DoWorkEvent(object sender, DoWorkEventArgs e)
        {
            var bgWorker = (BackgroundWorker)sender;
            var sw = new Stopwatch();

            while (!bgWorker.CancellationPending)
            {
                if (capture.IsOpened())
                {
                    sw.Restart();
                    sw.Start();
                    using (var frameMat = capture.RetrieveMat())
                    {
                        var frameBitmap = BitmapFromBitmapSource(frameMat.ToBitmapSource());
                        bgWorker.ReportProgress(0, frameBitmap);
                    }
                    /*
                    capture.Read(mat);
                    if (!mat.Empty())
                    {
                        var frameBitmap = MatToBitmap(mat);//BitmapFromWriteableBitmap(WriteableBitmapConverter.ToWriteableBitmap(mat));
                        bgWorker.ReportProgress(0, frameBitmap);
                    }
                    */
                    sw.Stop();
                    console.log(sw.ElapsedMilliseconds);
                    try
                    {
                        var delay = int.Parse(threadDelay.Text);
                        if (delay > 500) delay = 500;
                        Thread.Sleep(delay);
                    }
                    catch
                    {
                        Thread.Sleep(16);
                    }
                }
                else
                {
                    Thread.Sleep(1000);
                }
            }
        }

        private void ProgressChangedEvent(object sender, ProgressChangedEventArgs e)
        {
            var frameBitmap = (Bitmap)e.UserState;
            pictureBox.BackgroundImage?.Dispose();
            pictureBox.BackgroundImage = frameBitmap;
        }

        private void KeyUpEvent(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.F11)
            {
                FullScreen(fullscreenCM.Checked = !fullscreenCM.Checked);
            }
        }

        private void FullScreen(bool b)
        {
            if (b)
            {
                FormBorderStyle = FormBorderStyle.None;
                lastLoc = new System.Drawing.Point(Width, Height);
                console.log(lastLoc);
                lastPoint = Location;
                Width = Screen.PrimaryScreen.Bounds.Width;
                Height = Screen.PrimaryScreen.Bounds.Height;
                Location = new System.Drawing.Point(0, 0);
            }
            else
            {
                FormBorderStyle = FormBorderStyle.Sizable;
                console.log(lastLoc);
                Width = lastLoc.X + 16;
                Height = lastLoc.Y + 39;
                Location = lastPoint;
            }
        }

        private void PictureBoxClickEvent(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                contextMenu.Show(this, e.Location);
            }
        }

        private void MenuItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            var item = e.ClickedItem;
            switch (item.Name)
            {
                case "fullscreenCM":
                    FullScreen(fullscreenCM.Checked = !fullscreenCM.Checked);
                    break;
                case "videoResetCM":
                    break;
                case "videoOpenCM":
                    break;
            }
        }

        private Bitmap BitmapFromBitmapSource(BitmapSource writeBmp)
        {
            Bitmap bmp;
            using (MemoryStream outStream = new MemoryStream())
            {
                BitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(writeBmp));
                enc.Save(outStream);
                bmp = new Bitmap(outStream);
            }
            return bmp;
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_SIZING)
            {
                RECT rc = (RECT)Marshal.PtrToStructure(m.LParam, typeof(RECT));
                int res = m.WParam.ToInt32();
                if (res == WMSZ_LEFT || res == WMSZ_RIGHT)
                {
                    //Left or right resize -> adjust height (bottom)
                    rc.Bottom = rc.Top + (int)(heightRatio * this.Width / widthRatio);
                }
                else if (res == WMSZ_TOP || res == WMSZ_BOTTOM)
                {
                    //Up or down resize -> adjust width (right)
                    rc.Right = rc.Left + (int)(widthRatio * this.Height / heightRatio);
                }
                else if (res == WMSZ_RIGHT + WMSZ_BOTTOM)
                {
                    //Lower-right corner resize -> adjust height (could have been width)
                    rc.Bottom = rc.Top + (int)(heightRatio * this.Width / widthRatio);
                }
                else if (res == WMSZ_LEFT + WMSZ_TOP)
                {
                    //Upper-left corner -> adjust width (could have been height)
                    rc.Left = rc.Right - (int)(widthRatio * this.Height / heightRatio);
                }
                Marshal.StructureToPtr(rc, m.LParam, true);
            }

            base.WndProc(ref m);
        }
    }
}

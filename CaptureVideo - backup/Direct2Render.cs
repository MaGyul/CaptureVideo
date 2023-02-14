using CaptureVideo.Properties;
using DirectShowLib;
using NAudio.Wave;
using OpenCvSharp;
using SharpDX;
using SharpDX.Direct2D1;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace CaptureVideo
{
    public partial class Direct2Render : Form
    {
        const double widthRatio = 16;
        const double heightRatio = 9.6;

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

        private Config config = new Config();

        private SimpleConsole console = new SimpleConsole();
        private int[] lastLoc = new int[2];
        private System.Drawing.Point lastPoint;
        private VideoCapture capture;
        private WaveInEvent waveIn;
        private WaveOut waveOut;
        private BufferedWaveProvider bwp;
        public List<DsDevice> cameraDevices;
        public List<DsDevice> audioDevices;

        private RenderTarget2D _renderTarget;
        private Thread camera;
        private BlockingCollection<Bitmap> _q = new BlockingCollection<Bitmap>();
        private DataStream _bitmapData;

        private byte[] whiteSource;

        public Direct2Render()
        {
            InitializeComponent();
            _renderTarget = new RenderTarget2D();
            capture = new VideoCapture();
            waveIn = new WaveInEvent
            {
                WaveFormat = new WaveFormat(rate: 48000, bits: 16, channels: 2),
                BufferMilliseconds = 10,
            };
            waveIn.DataAvailable += OnDataAvailable;
            waveOut = new WaveOut
            {
                DesiredLatency = 10,
                NumberOfBuffers = 100,
            };
            bwp = new BufferedWaveProvider(waveIn.WaveFormat);
            bwp.DiscardOnBufferOverflow = true;
            waveOut.Init(bwp);
            cameraDevices = new List<DsDevice>();
            cameraDevices.AddRange(from DsDevice dsDevice in DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice)
                                   where !dsDevice.DevicePath.Contains("device:sw")
                                   select dsDevice);
            audioDevices = new List<DsDevice>();
            audioDevices.AddRange(from DsDevice dsDevice in DsDevice.GetDevicesOfCat(FilterCategory.AudioInputDevice)
                                  where !dsDevice.DevicePath.Contains("device:sw")
                                  select dsDevice);
            //console.log(audioDevices.FindIndex(a => a.Name.Contains("2- USB")));
            using (MemoryStream s = new MemoryStream())
            {
                Resources.white.Save(s, Resources.white.RawFormat);
                whiteSource = s.ToArray();
            }
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
                if (MinimumSize.Width != lastLoc[0])
                    lastLoc[0] = Width + 16;
                else
                    lastLoc[0] = Width;
                if (MinimumSize.Height != lastLoc[1])
                    lastLoc[1] = Height + 39;
                else
                    lastLoc[1] = Height;
                //console.log(lastLoc[0] + "x" + lastLoc[1]);
                lastPoint = Location;
                Width = Screen.PrimaryScreen.Bounds.Width;
                Height = Screen.PrimaryScreen.Bounds.Height;
                Location = new System.Drawing.Point(0, 0);
            }
            else
            {
                FormBorderStyle = FormBorderStyle.Sizable;
                //console.log(lastLoc[0] + "x" + lastLoc[1]);
                Width = lastLoc[0];
                Height = lastLoc[1];
                Location = lastPoint;
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
                case "videoOpenCM":
                    waveIn.StopRecording();
                    waveOut.Stop();
                    SelectVideo pop = new SelectVideo(this);
                    pop.Owner = this;
                    pop.ShowDialog();
                    break;
            }
        }

        public void OpenVideoCapture(int index, int audio)
        {
            if (capture != null)
            {
                capture.Dispose();
                capture = null;
                capture = new VideoCapture();
            }
            if (!capture.Open(index, VideoCaptureAPIs.ANY))
            {
                MessageBox.Show("연결에 실패했습니다.", "연결 실패", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                openCapture.Visible = false;
                camera = new Thread(new ThreadStart(CaptureCameraCallback));
                camera.IsBackground = true;
                camera.Start();
                config.Write("video", "video", "" + index);
                if (audio != -99)
                {
                    config.Write("video", "audio", "" + audio);
                    Text = cameraDevices[index].Name;
                    waveIn.DeviceNumber = audio;
                    waveIn.StartRecording();
                    waveOut.Play();
                }
            }
        }

        private void OnDataAvailable(object sender, WaveInEventArgs args)
        {
            console.log(args.Buffer.Length+" / "+args.BytesRecorded);
            bwp.AddSamples(args.Buffer, 0, args.BytesRecorded);
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);

            if (e.Button == MouseButtons.Right)
            {
                contextMenu.Show(this, e.Location);
            }
        }

        private void CMouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                contextMenu.Show(this, e.Location);
            }
            if (e.Button == MouseButtons.Left)
            {
                waveIn.StopRecording();
                waveOut.Stop();
                SelectVideo pop = new SelectVideo(this);
                pop.Owner = this;
                pop.ShowDialog();
            }
        }

        ////////////////////////////////////////////////////////////////////

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            capture.Dispose();
            waveIn.Dispose();
            waveOut.Dispose();
            _renderTarget.Dispose();
            base.OnFormClosing(e);
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            while (true)
            {
                if (_q.TryTake(out Bitmap item) == true)
                {
                    _renderTarget.Render(
                        (renderer) =>
                        {
                            renderer.DrawBitmap(item, 1.0f, BitmapInterpolationMode.Linear);
                        });
                    item.Dispose();
                }
                else
                {
                    break;
                }
            }
        }

        private void CaptureCameraCallback()
        {
            Invoke((Action)(() =>
            {
                _renderTarget.Initialize(Handle, capture.FrameWidth, capture.FrameHeight);
            }));

            _bitmapData = new DataStream(capture.FrameWidth * capture.FrameHeight * 4, false, false);
            int lastFps;
            int fps = GetFPS();
            lastFps = fps;
            int expectedProcessTimePerFrame = 1000 / fps;
            Stopwatch st = new Stopwatch();
            st.Start();

            using (Mat image = new Mat())
            {
                while (true)
                {
                    if (capture == null || capture.IsDisposed)
                    {
                        console.log("capture is null -> clear");
                        break;
                    }
                    if (lastFps != (fps = GetFPS()))
                    {
                        lastFps = fps;
                        expectedProcessTimePerFrame = 1000 / fps;
                    }
                    long started = st.ElapsedMilliseconds;

                    capture?.Read(image);
                    if (image.Empty() == true)
                    {
                        break;
                    }

                    _q.Add(ToSharpDXBitmap(image));

                    try
                    {
                        Invoke((Action)(() => Invalidate()));
                    }
                    catch (ObjectDisposedException) { }
                    catch (InvalidOperationException) { }

                    int elapsed = (int)(st.ElapsedMilliseconds - started);
                    int delay = expectedProcessTimePerFrame - elapsed;

                    if (delay > 0)
                    {
                        Thread.Sleep(delay);
                    }
                }
            }

        }

        private Bitmap ToSharpDXBitmap(Mat org)
        {
            using (Mat orgData = org.CvtColor(ColorConversionCodes.BGR2BGRA))
            {
                IntPtr dstPtr = _bitmapData.DataPointer;

                Utilities.CopyMemory(dstPtr, orgData.Data, (int)orgData.Total() * orgData.ElemSize());

                return _renderTarget.CreateBitmap(_bitmapData);
            }
        }

        private int GetFPS()
        {
            if (IsDisposed) return 1;
            if (frameTime.IsDisposed) return 1;
            int fps;
            try
            {
                var text = frameTime?.Text;
                if (text == null) text = "60";
                fps = int.Parse(text);
                if (fps < 5) fps = 5;
            }
            catch
            {
                fps = 60;
            }

            return fps;
        }

        private void OnLoad(object sender, EventArgs e)
        {
            try
            {
                int video = int.Parse(config.Read("video", "video", "-99"));
                int audio = int.Parse(config.Read("video", "audio", "-99"));
                if (video != -99)
                {
                    OpenVideoCapture(video, audio);
                }
            }
            catch { }

            Height = (int)(heightRatio * Width / widthRatio);
            MinimumSize = new System.Drawing.Size(Width, Height);
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

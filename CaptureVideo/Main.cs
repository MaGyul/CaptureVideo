using DirectShowLib;
using LumiSoft.Media.Wave;
using OpenCvSharp;
using SharpDX;
using SharpDX.Direct2D1;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace CaptureVideo
{
    public partial class Main : Form
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
        private System.Drawing.Size lastClientSize;
        private System.Drawing.Point lastLocation;
        private VideoCapture capture;
        private WaveIn waveIn;
        private WaveOut waveOut;

        public List<DsDevice> cameraDevices;

        private RenderTarget2D _renderTarget;
        private Thread camera;
        private System.Windows.Forms.Timer fadeOut = new System.Windows.Forms.Timer();
        private BlockingCollection<SharpDX.Direct2D1.Bitmap> _q = new BlockingCollection<SharpDX.Direct2D1.Bitmap>();
        private DataStream _bitmapData;

        public Main()
        {
            InitializeComponent();
            capture = new VideoCapture();
            cameraDevices = new List<DsDevice>();
            cameraDevices.AddRange(from DsDevice dsDevice in DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice)
                                   select dsDevice);
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
                var screen = Screen.FromControl(this);
                FormBorderStyle = FormBorderStyle.None;
                StartPosition = FormStartPosition.Manual;

                lastClientSize = ClientSize;
                lastLocation = Location;
                ClientSize = new System.Drawing.Size(screen.Bounds.Width, screen.Bounds.Height);
                Location = new System.Drawing.Point(screen.Bounds.Left, screen.Bounds.Top);
                /*

                var fullScreen_bounds = Rectangle.Empty;

                foreach (var screen in Screen.AllScreens)
                {
                    fullScreen_bounds = Rectangle.Union(fullScreen_bounds, screen.Bounds);
                }
                */
            }
            else
            {
                FormBorderStyle = FormBorderStyle.Sizable;
                StartPosition = FormStartPosition.WindowsDefaultLocation;
                ClientSize = lastClientSize;
                Location = lastLocation;
                //Height = (int)(heightRatio * Width / widthRatio);
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
                    SelectVideo pop = new SelectVideo(this, config);
                    pop.Owner = this;
                    pop.ShowDialog(this);
                    break;
                case "shutdownCM":
                    Close();
                    break;
            }
        }

        public void OpenVideoCapture(int index, WavInDevice audio, WavOutDevice output)
        {
            if (capture != null)
            {
                capture.Dispose();
                capture = null;
            }
            if (waveIn != null)
            {
                waveIn.Dispose();
                waveIn = null;
            }
            if (waveOut != null)
            {
                waveOut.Dispose();
                waveOut = null;
            }
            if (camera != null)
            {
                camera.Interrupt();
                camera.Join();
                camera = null;
            }
            capture = new VideoCapture();
            if (!capture.Open(index, VideoCaptureAPIs.ANY))
            {
                capture.Dispose();
                capture = null;
                MessageBox.Show(this, "연결에 실패했습니다.", "연결 실패", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                if (_renderTarget != null)
                {
                    var b = _renderTarget;
                    _renderTarget = null;
                    b.Dispose();
                    b = null;
                }
                _renderTarget = new RenderTarget2D();
                openCapture.Visible = false;
                camera = new Thread(new ThreadStart(CaptureCameraCallback));
                camera.IsBackground = true;
                camera.Start();
                config.Write("video", "video", "" + index);
                Text = cameraDevices[index].Name;
                if (output != null)
                {
                    config.Write("audio", "out", "" + output.Index);
                    waveOut = new WaveOut(output, 48000, 16, 2);
                    try {
                        int value = int.Parse(config.Read("audio", "vol", "5"));
                        if (value < trackBarCM.trackBar.Minimum) value = trackBarCM.trackBar.Minimum;
                        if (value > trackBarCM.trackBar.Maximum) value = trackBarCM.trackBar.Maximum;
                        trackBarCM.trackBar.Value = value;
                    } catch {
                        trackBarCM.trackBar.Value = 5;
                    }
                }
                else
                {
                    config.Write("audio", "out", "-1");
                }
                if (audio != null)
                {
                    config.Write("audio", "in", "" + audio.Index);
                    waveIn = new WaveIn(audio, 48000, 16, 2, 1920);
                    waveIn.BufferFull += new BufferFullHandler(WaveInBufferFull);
                    waveIn.Start();
                }
                else
                {
                    config.Write("audio", "in", "-1");
                }
            }
        }

        private void WaveInBufferFull(byte[] buffer)
        {
            try
            {
                if (waveOut != null && !waveOut.IsDisposed)
                {
                    waveOut.Play(buffer, 0, buffer.Length);
                }
            } catch (Exception e)
            {
                console.error("[Sound] " + e.Message);
            }
        }

        private void TrackBar_ValueChanged(object sender, EventArgs e)
        {
            if (waveOut != null && !waveOut.IsDisposed)
            {
                var value = trackBarCM.trackBar.Value;
                config.Write("audio", "vol", "" + value);
                waveOut.SetVolume(value);
            }
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);

            if (e.Delta < 0)
            {
                if (trackBarCM.trackBar.Value <= trackBarCM.trackBar.Minimum) return;
                trackBarCM.trackBar.Value--;
            }
            else
            {
                if (trackBarCM.trackBar.Value >= trackBarCM.trackBar.Maximum) return;
                trackBarCM.trackBar.Value++;
            }
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
                SelectVideo pop = new SelectVideo(this, config);
                pop.Owner = this;
                pop.ShowDialog(this);
            }
        }

        ////////////////////////////////////////////////////////////////////

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (capture != null && !capture.IsDisposed) capture.Dispose();
            if (waveIn != null && !waveIn.IsDisposed) waveIn.Dispose();
            if (waveOut != null && !waveOut.IsDisposed) waveOut.Dispose();
            if (_renderTarget != null) _renderTarget.Dispose();
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
                if (_renderTarget != null)
                    if (_q.TryTake(out SharpDX.Direct2D1.Bitmap item) == true)
                    {
                        try
                        {
                            _renderTarget.Render(
                                (renderer) =>
                                {
                                    renderer.DrawBitmap(item, 1.0f, BitmapInterpolationMode.Linear);
                                });
                        }
                        catch (SharpDXException err)
                        {
                            console.error("[Render-SharpDX] " + err.Message);
                        }
                        item.Dispose();
                    }
                    else
                    {
                        break;
                    }
                else break;
            }
        }

        private void CaptureCameraCallback()
        {
            try
            {
                using (Mat mat = new Mat())
                {
                    capture.Read(mat);
                    capture.FrameWidth = 7680;
                    capture.FrameHeight = 4320;
                }
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

                        try
                        {
                            capture?.Read(image);
                            //console.log(image.Width + " / " + image.Height);
                        }
                        catch (ObjectDisposedException expr)
                        {
                            console.error("[Capture-ObjectDisposed] " + expr.Message);
                        }
                        catch (InvalidOperationException expr)
                        {
                            console.error("[Capture-InvalidOperation] " + expr.Message);
                        }
                        if (image.Empty() == true)
                        {
                            break;
                        }

                        _q.Add(ToSharpDXBitmap(image));

                        try
                        {
                            Invoke((Action)(() => Invalidate()));
                        }
                        catch (ObjectDisposedException expr)
                        {
                            console.error("[Invalidate-ObjectDisposed] " + expr.Message);
                        }
                        catch (InvalidOperationException expr)
                        {
                            console.error("[Invalidate-InvalidOperation] " + expr.Message);
                        }

                        int elapsed = (int)(st.ElapsedMilliseconds - started);
                        int delay = expectedProcessTimePerFrame - elapsed;

                        if (delay > 0)
                        {
                            Thread.Sleep(delay);
                        }
                    }
                }
            } catch (Exception e)
            {
                console.error("[Thread] " + e.Message);
            }
        }

        private SharpDX.Direct2D1.Bitmap ToSharpDXBitmap(Mat org)
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
                int video = int.Parse(config.Read("video", "video", "-1"));
                int audio_in = int.Parse(config.Read("audio", "in", "-1"));
                int audio_out = int.Parse(config.Read("audio", "out", "-1"));
                if (video != -1)
                {
                    WavInDevice input = null;
                    WavOutDevice output = null;
                    if (audio_in != -1)
                    {
                        input = WaveIn.Devices.ToList().Find(w => w.Index == audio_in);
                    }
                    if (audio_out != -1)
                    {
                        output = WaveOut.Devices.ToList().Find(w => w.Index == audio_out);
                    }
                    OpenVideoCapture(video, input, output);
                }
            }
            catch { }

            Height = (int)(heightRatio * Width / widthRatio);
            MinimumSize = new System.Drawing.Size(Width, Height);
            trackBarCM.trackBar.Maximum = 16;
            trackBarCM.trackBar.Minimum = 0;
            trackBarCM.trackBar.LargeChange = 2;
            //trackBarCM.trackBar.TickStyle = TickStyle.None;
            trackBarCM.trackBar.ValueChanged += TrackBar_ValueChanged;
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

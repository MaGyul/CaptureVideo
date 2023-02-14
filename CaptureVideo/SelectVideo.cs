using DirectShowLib;
using LumiSoft.Media.Wave;
using System;
using System.Windows.Forms;

namespace CaptureVideo
{
    public partial class SelectVideo : Form
    {
        private Main parent;
        private Config config;

        public SelectVideo(Main parent, Config config)
        {
            this.parent = parent;
            this.config = config;
            InitializeComponent();
        }

        private void LoadEvent(object sender, EventArgs e)
        {
            if (parent.cameraDevices.Count <= 0)
            {
                MessageBox.Show("등록된 비디오 캡쳐/카메라가 없습니다.", "주의", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            foreach (DsDevice dsDevice in parent.cameraDevices)
            {
                videoSelect.Items.Add(dsDevice.Name);
            }

            if (WaveIn.Devices.Length > 0)
            {
                foreach (var device in WaveIn.Devices)
                {
                    audioSelect.Items.Add(device.Name);
                }
            }
            if (WaveOut.Devices.Length > 0)
            {
                foreach (var device in WaveOut.Devices)
                {
                    outSelect.Items.Add(device.Name);
                }
            }
            try
            {
                var video = int.Parse(config.Read("video", "video", "-1"));
                var audio_in = int.Parse(config.Read("audio", "in", "-1"));
                var audio_out = int.Parse(config.Read("audio", "out", "-1"));
                videoSelect.SelectedIndex = video;
                audioSelect.SelectedIndex = audio_in;
                outSelect.SelectedIndex = audio_out;
            }
            catch { }
        }

        private void okBtn_Click(object sender, EventArgs e)
        {
            if (videoSelect.SelectedIndex == -1)
            {
                MessageBox.Show(this, "캡쳐 장치를 선택해주세요.", "알림", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                WavInDevice wavIn = null;
                WavOutDevice wavOut = null;
                if (audioSelect.SelectedIndex != -1) wavIn = WaveIn.Devices[audioSelect.SelectedIndex];
                if (outSelect.SelectedIndex != -1) wavOut = WaveOut.Devices[outSelect.SelectedIndex];
                if (wavIn != null && wavOut == null)
                {
                    MessageBox.Show(this, "출력 장치를 선택해주세요.", "알림", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                parent.OpenVideoCapture(videoSelect.SelectedIndex, wavIn, wavOut);
                Close();
            }
        }
    }
}

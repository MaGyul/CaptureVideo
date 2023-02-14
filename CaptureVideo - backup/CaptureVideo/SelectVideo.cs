using DirectShowLib;
using System;
using System.Windows.Forms;

namespace CaptureVideo
{
    public partial class SelectVideo : Form
    {
        private Direct2Render parent;

        public SelectVideo(Direct2Render parent)
        {
            this.parent = parent;
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

            if (parent.audioDevices.Count > 0)
            {
                foreach (DsDevice dsDevice in parent.audioDevices)
                {
                    audioSelect.Items.Add(dsDevice.Name);
                }
            }

            videoSelect.SelectedIndex = 0;
            audioSelect.SelectedIndex = 0;
        }

        private void okBtn_Click(object sender, EventArgs e)
        {
            parent.OpenVideoCapture(videoSelect.SelectedIndex, audioSelect.SelectedIndex);
            Close();
        }
    }
}

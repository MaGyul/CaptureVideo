using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.Design;
using System.Windows.Forms;

namespace CaptureVideo
{
    [ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.MenuStrip |
                                   ToolStripItemDesignerAvailability.ContextMenuStrip)]
    public class TrackBarMenuItem : ToolStripControlHost
    {
        public TrackBar trackBar;

        public TrackBarMenuItem() : base(new TrackBar())
        {
            this.trackBar = this.Control as TrackBar;
        }

        // Add properties, events etc. you want to expose...
    }
}

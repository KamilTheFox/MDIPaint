using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LibraryForPaint
{
    public interface IAddon
    {
        string Name { get; }
        string Description { get; }
        Image Icon { get; }

        ToolStripButton PostAddonToToolStip();

        void PostAddonToWindowMDI(Form formMDI);
    }
}

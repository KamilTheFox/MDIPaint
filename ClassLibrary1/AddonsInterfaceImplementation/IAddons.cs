using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryForPaint
{
    public interface IAddons
    {
        InfoAddon InfoAddons { get; }

        IAddonDraw[] addons { get; }

    }
}

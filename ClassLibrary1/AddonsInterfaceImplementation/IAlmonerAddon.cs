using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryForPaint
{
    public interface IAlmonerAddon
    {
        InfoAddon[] GetAddonsInfo();

        IAddons[] GetAutoLoadAddons();

        IAddons GetAddonsToGroup(InfoAddon infoAddon);

        bool IsAutoLoad(InfoAddon Dll);
        void SetAutoLoad(InfoAddon Dll, bool autoLoad);

    }
}

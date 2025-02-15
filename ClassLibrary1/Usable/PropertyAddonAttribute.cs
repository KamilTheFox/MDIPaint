using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryForPaint
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public class PropertyAddonAttribute : Attribute
    {
        public string Version { get; private set; }
        public TypeToolStip TypeToolStip { get; private set; }
        public PropertyAddonAttribute(string version, TypeToolStip typeToolStip)
        {
            Version = version;  
            TypeToolStip = typeToolStip;
        }
    }
}

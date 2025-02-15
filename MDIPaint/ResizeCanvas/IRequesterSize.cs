using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDIPaint
{
    public interface IRequesterSize
    {
        int Width { get; }
        int Height { get; }

        void SetDefaultValue(int width, int height);
    }
}

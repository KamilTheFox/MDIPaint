using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDIPaint
{
    public interface ISavedImage
    {
        string PathImage { get; set; }

        Bitmap GetImage();

        void SetImage(Bitmap bitmapOpen);

    }
}

using System;
using System.Drawing;
using System.Windows.Forms;

namespace LibraryForPaint
{
    public interface IAddonDraw : IAddon
    {
        Color Color { get; set; }

        int Width { get; set; }

        void EventDragCursor(Bitmap bitmap, Point_Int previousPos, Point_Int currentPos);

        void EventDownCursor(Bitmap bitmap, Point_Int Position);

        void EventUpCursor(Bitmap bitmap, Point_Int Position);

    }
}

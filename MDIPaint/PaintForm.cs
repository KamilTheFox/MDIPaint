using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LibraryForPaint;

namespace MDIPaint
{
    public partial class PaintForm : Form, IReceiverSize, ISavedImage
    {
        private Point_Int previousPoint;
        private Bitmap bitmap;
        private Graphics graphics;

        public IAddonDraw Brush => MainForm.CurrentBrush;

        string ISavedImage.PathImage { get; set; }

        public PaintForm()
        {
            InitiliseField();
        }

        public PaintForm(Bitmap bitmapOpen)
        {
            bitmap = bitmapOpen;
            InitiliseField();
        }

        private void InitiliseField()
        {
            InitializeComponent();
            if (bitmap == null)
                bitmap = new Bitmap(400, 400, PixelFormat.Format32bppArgb);
            revertBackground();
            graphics = Graphics.FromImage(bitmap);
            this.DoubleBuffered = true;
        }
        private void ScrollAuto(int imageWidth, int imageHeight)
        {
            this.AutoScroll = true;
            this.AutoScrollMinSize = new Size(imageWidth, imageHeight);
        }
        private void revertBackground()
        {
            var bitmapBackGround = new Bitmap(bitmap.Width, bitmap.Height, PixelFormat.Format32bppArgb);
            for (int x = 0; x < bitmapBackGround.Width; x++)
                for (int y = 0; y < bitmapBackGround.Height; y++)
                {
                    bool isDarkSquare = x / 10 % 2 == 0 || y / 10 % 2 == 0;
                    Color color = isDarkSquare ? Color.DarkGray : Color.Gray;

                    bitmapBackGround.SetPixel(x,y,color);
                }
            this.BackgroundImage = bitmapBackGround;
            this.AutoScrollOffset = new Point(bitmap.Width, bitmap.Height);
        }

        public void SetSeze(IRequesterSize requester)
        {
            var oldBitMap = bitmap;
            bitmap = new Bitmap(requester.Width, requester.Height, PixelFormat.Format32bppArgb);
            graphics = Graphics.FromImage(bitmap);
            if (oldBitMap != null)
            {
                graphics.DrawImage(oldBitMap, new PointF(0,0));
            }
            revertBackground();
            ScrollAuto(requester.Width, requester.Height);
        }

        

        

        private Point GetAmendmentPointMouse()
        {
            int scrollX = Math.Abs(AutoScrollPosition.X);
            int scrollY = Math.Abs(AutoScrollPosition.Y);
            return new Point(-scrollX, -scrollY);
        }

        private void PaintForm_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImage(bitmap, GetAmendmentPointMouse());
        }

        void IReceiverSize.SetDefaultSize(IRequesterSize requester)
        {
            requester.SetDefaultValue(bitmap.Width, bitmap.Height);
        }

        Bitmap ISavedImage.GetImage()
        {
            return bitmap;
        }

        void ISavedImage.SetImage(Bitmap bitmapOpen)
        {
            bitmap = bitmapOpen;
            graphics = Graphics.FromImage(bitmap);
            var res = new ResizeImageBackground();
            res.SetDefaultValue(bitmap.Width, bitmap.Height);
            SetSeze(res);
            this.Refresh();
        }

        

        private struct ResizeImageBackground : IRequesterSize
        {
            public int Width { get; set; }

            public int Height { get; set; }

            public void SetDefaultValue(int width, int height)
            {
                Width = width;
                Height = height;
            }
        }
        private void PaintForm_MouseMove(object sender, MouseEventArgs e)
        {
            Point amendment = GetAmendmentPointMouse();
            if (e.Button == MouseButtons.Left)
            {
                int _xMouse = e.X - amendment.X;
                int _yMouse = e.Y - amendment.Y;

                Brush.EventDragCursor(bitmap, previousPoint, new Point_Int(_xMouse, _yMouse));

                Invalidate();
                previousPoint.x = _xMouse;
                previousPoint.y = _yMouse;
            }

        }
        private void PaintForm_MouseUp(object sender, MouseEventArgs e)
        {
            Point amendment = GetAmendmentPointMouse();

            previousPoint.x = e.X - amendment.X;
            previousPoint.y = e.Y - amendment.Y;

            Brush.EventUpCursor(bitmap, previousPoint);

            Invalidate();
        }
        private void PaintForm_MouseDown(object sender, MouseEventArgs e)
        {
            Point amendment = GetAmendmentPointMouse();

            previousPoint.x = e.X - amendment.X;
            previousPoint.y = e.Y - amendment.Y;

            Brush.EventDownCursor(bitmap, previousPoint);
        }
    }
}

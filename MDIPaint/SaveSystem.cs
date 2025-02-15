using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MDIPaint
{
    public static class SaveSystem
    {
        private const string SaveFileFilter =   "Все файлы ()*.*|*.*|"+ 
                                                "Windows Bitmap (*.bmp)|*.bmp|" +
                                                "Файлы JPEG (*.jpg; *.jpeg)|*.jpg;*.jpeg|" +
                                                "Portable Network Graphics (*.png)|*.png|" +
                                                "Graphics Interchange Format (*.gif)|*.gif|" +
                                                "Tagged Image File Format (*.tiff)|*.tiff";

        public static void OpenImage(ISavedImage saved)
        {
            var dialogFile = new OpenFileDialog();
            dialogFile.Filter = SaveFileFilter;
            if (dialogFile.ShowDialog() == DialogResult.OK)
            {
                saved.PathImage = dialogFile.FileName;
                saved.SetImage(new Bitmap(saved.PathImage));
            }
            else
            {
                (saved as Form).Close();
            }
        }

        public static void SaveImage(ISavedImage saved, bool isSaveAs)
        {
            DialogResult dialog = DialogResult.OK;

            if (string.IsNullOrEmpty(saved.PathImage) || !File.Exists(saved.PathImage) || isSaveAs)
            {
                var dialogFile = new SaveFileDialog();
                dialogFile.Filter = SaveFileFilter;
                dialog = dialogFile.ShowDialog();
                if (dialog == DialogResult.OK)
                {
                    saved.PathImage = dialogFile.FileName;
                }

            }

            if (dialog == DialogResult.Cancel) return;

            string extension = Path.GetExtension(saved.PathImage).ToLower();

            Bitmap bitmap = saved.GetImage();

            try
            {
                switch (extension)
                {
                    case ".bmp":
                        bitmap.Save(saved.PathImage, ImageFormat.Bmp);
                        break;

                    case ".jpg":
                    case ".jpeg":
                        using (EncoderParameters encoderParameters = new EncoderParameters(1))
                        using (EncoderParameter parameter = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 100L))
                        {
                            encoderParameters.Param[0] = parameter;
                            ImageCodecInfo codecInfo = GetEncoderInfo("image/jpeg");
                            bitmap.Save(saved.PathImage, codecInfo, encoderParameters);
                        }
                        break;

                    case ".png":
                        bitmap.Save(saved.PathImage, ImageFormat.Png);
                        break;

                    case ".gif":
                        bitmap.Save(saved.PathImage, ImageFormat.Gif);
                        break;

                    case ".tiff":
                        bitmap.Save(saved.PathImage, ImageFormat.Tiff);
                        break;

                    default:
                        throw new ArgumentException("Неподдерживаемый формат изображения!");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при сохранении изображения: {ex.Message}");
            }
        }

        private static ImageCodecInfo GetEncoderInfo(string mimeType)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();

            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.MimeType == mimeType)
                    return codec;
            }

            return null;
        }
    }
}

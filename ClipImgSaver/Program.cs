using System;
using System.Windows;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Media.Imaging;

namespace ClipImgSaver {
    class Program {
        [STAThread]
        static void Main(string[] args) {
            //クリップボードにBitmapデータがあるか調べる（調べなくても良い）
            if (Clipboard.ContainsImage()) {
                //クリップボードにあるデータの取得
                using (Image img = GetBitmap(Clipboard.GetImage())) {
                    if (img != null) {
                        string folder = Path.Combine(System.Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), "ClipImages");
                        if (!Directory.Exists(folder)) {
                            Directory.CreateDirectory(folder);
                        }
                        int n = 1;
                        while (File.Exists(Path.Combine(folder, $"Image_{n}.jpg"))) {
                            n++;
                        }

                        img.Save(Path.Combine(folder, $"Image_{n}.jpg"), ImageFormat.Jpeg);
                    }
                }
            }
        }

        /// <summary>
        /// Bitmap <-> BitmapSource converter
        /// https://gist.github.com/nashby/916300
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        static Bitmap GetBitmap(BitmapSource source) {
            Bitmap bmp = new Bitmap
            (
              source.PixelWidth,
              source.PixelHeight,
              System.Drawing.Imaging.PixelFormat.Format32bppPArgb
            );

            BitmapData data = bmp.LockBits
            (
                new System.Drawing.Rectangle(System.Drawing.Point.Empty, bmp.Size),
                ImageLockMode.WriteOnly,
                System.Drawing.Imaging.PixelFormat.Format32bppPArgb
            );

            source.CopyPixels
            (
              Int32Rect.Empty,
              data.Scan0,
              data.Height * data.Stride,
              data.Stride
            );

            bmp.UnlockBits(data);

            return bmp;
        }
    }
}

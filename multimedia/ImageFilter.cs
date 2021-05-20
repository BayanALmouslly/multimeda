using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace multimedia
{
   public static class ImageFilter
    {
        public static Image filterImage(this Image inputImage,int opacity,int red,int green,int blue)
        {
            Bitmap bitmap = new Bitmap(inputImage.Width, inputImage.Height);
            Graphics imageGraphics = Graphics.FromImage(bitmap);
            imageGraphics.DrawImage(inputImage, 0, 0);
            imageGraphics.FillRectangle(new SolidBrush(Color.FromArgb(opacity, red, green, blue)), 0, 0, bitmap.Width, bitmap.Height);
            return bitmap;
        }
        public static Image negative(this Image inputImage)
        {
            Bitmap bitmap = new Bitmap(inputImage.Width, inputImage.Height);
           
            for (int y = 0; y < inputImage.Height; y++)
            {
                for (int x = 0; x < inputImage.Width; x++)
                {
                    Color p = bitmap.GetPixel(x, y);
                    int a = p.A;
                    int r = p.R;
                    int g = p.G;
                    int b = p.B;
                    r = 255 - r;
                    g = 255 - g;
                    b = 255 - b;
                    bitmap.SetPixel(x, y, Color.FromArgb(a, r, g, b));
                }
            }
            return bitmap;
        }
    }
}

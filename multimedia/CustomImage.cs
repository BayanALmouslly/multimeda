using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace multimedia
{
    public class CustomImage
    {
        public CustomImage(int id, string name, byte[] img)
        {
            this.id = id;
            this.Name = name;
            this.Binary = img;
            this.Bitmap = GetBitmap(img);
        }

        public CustomImage(string namne, string path)
        {
            this.Name = namne;
            this.Binary = GetBinary(path);
            this.Bitmap = GetBitmap(this.Binary);
        }
        public CustomImage(string name, Bitmap bitmap)
        {
            this.Name = name;
            this.Binary = GetBinary(bitmap);
            this.Bitmap = bitmap;
        }
        public CustomImage(string name, Image bitmap)
        {
            this.Name = name;
            this.Binary = GetBinary(bitmap);
            this.Bitmap = GetBitmap(this.Binary);
        }
        public int id { get; set; }
        public string Name { get; set; }
        public byte[] Binary { get; set; }
        public Bitmap Bitmap { get; set; }
        public static Bitmap GetBitmap(byte[] blob)
        {
            using (MemoryStream mStream = new MemoryStream())
            {
                mStream.Write(blob, 0, blob.Length);
                mStream.Seek(0, SeekOrigin.Begin);

                Bitmap bm = new Bitmap(mStream);
                return bm;
            }
        }

        public static byte[] GetBinary(string path)
        {
            if (File.Exists(path))
                return File.ReadAllBytes(path);
            return new byte[0];
        }
        public static byte[] GetBinary(Image img)
        {
            ImageConverter converter = new ImageConverter();
            return (byte[])converter.ConvertTo(img, typeof(byte[]));
        }
        public static byte[] GetBinary(Bitmap img)
        {
            ImageConverter converter = new ImageConverter();
            return (byte[])converter.ConvertTo(img, typeof(byte[]));
        }
        public static implicit operator ImageTable(CustomImage customImage)
        {
            return new ImageTable()
            {
                Name = customImage.Name,
                Image = customImage.Binary
            };
        }
        public static implicit operator CustomImage(ImageTable imageTable)
        {

            var img = new CustomImage(imageTable.Id, imageTable.Name, imageTable.Image);
            return img;
        }
    }
}

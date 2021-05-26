﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.DataVisualization.Charting;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;
namespace multimedia
{
    public  partial  class  Form1 : Form
    {
        bool _streaming;
        Capture _capture;
        Image tempImage;
        Image SaveFilterImage;

        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            unVisiblePanel();

            trZoom.Minimum = 1;
            trZoom.Maximum = 6;
            trZoom.SmallChange = 1;
            trZoom.LargeChange = 1;
            trZoom.UseWaitCursor = false;

            this.DoubleBuffered = true;


        }
        private void unVisiblePanel()
        {
            panCamera.Visible = false;
            pnzoom.Visible = false;
            panfilter.Visible = false;
            pnResize.Visible = false;
            pantext.Visible = false;
            pnConvert.Visible = false;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void openimg_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "Image Files(*.jpg; *.jpeg; *.gif; *.bmp)|*.jpg; *.jpeg; *.gif; *.bmp";
            if (open.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Image = new Bitmap(open.FileName);
                tempImage = pictureBox1.Image;
                imgZoom = new PictureBox();
                imgZoom.Image = pictureBox1.Image;
                //textBox1.Text = open.FileName;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //var img = _capture.QueryFrame().ToImage<Bgr, byte>();
            //var bmp = img.Bitmap;
            //pictureBox1.Image = bmp;
            pictureBox1.Image = ptstream.Image;
            tempImage = pictureBox1.Image;

            ptstream.Visible = false;
            label1.Visible = false;
            btnStream.Visible = false;
            button2.Visible = false;

        }

        private void btnStream_Click(object sender, EventArgs e)
        {
            if (!_streaming)
            {
                Application.Idle += streaming;
                btnStream.Text = @"Stop Streaming";
            }
            else
            {
                Application.Idle -= streaming;
                btnStream.Text = @"Start Streaming";
            }
            _streaming = !_streaming;
        }

       
        private void streaming(object sender, System.EventArgs e)
        {
            var img = _capture.QueryFrame().ToImage<Bgr, byte>();
            var bmp = img.Bitmap;
            ptstream.Image = bmp;
        }

        private void ptstream_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            unVisiblePanel();

            panCamera.Visible = true;
        }

        private void changeScroll(object sender, EventArgs e)
        {
            pictureBox1.Image = SaveFilterImage;
            pictureBox1.Image = pictureBox1.Image.filterImage(100, trRed.Value, trGreen.Value, trBlue.Value);
        }

        private void btnFilter_Click(object sender, EventArgs e)
        {
            unVisiblePanel();
            SaveFilterImage = pictureBox1.Image;
            panfilter.Visible = true;
        }

        private void btnSaveFilter_Click(object sender, EventArgs e)
        {
            SaveFilterImage = pictureBox1.Image;
            panfilter.Visible=true;

        }

        private void btnCancelFilter_Click(object sender, EventArgs e)
        {
            unVisiblePanel();

            pictureBox1.Image = SaveFilterImage;
            panfilter.Visible=true;
        }
       

        private void btnRemoveEdit_Click(object sender, EventArgs e)
        {
            unVisiblePanel();

            pictureBox1.Image = tempImage;
        }

        private void chNegative_CheckedChanged(object sender, EventArgs e)
        {
            //  pictureBox1.Image = pictureBox1.Image.negative();
            Bitmap bmp = new Bitmap(pictureBox1.Image);
            int width = bmp.Width;
            int height = bmp.Height;

            //negative
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    //get pixel value
                    Color p = bmp.GetPixel(x, y);

                    //extract ARGB value from p
                    int a = p.A;
                    int r = p.R;
                    int g = p.G;
                    int b = p.B;

                    //find negative value
                    r = 255 - r;
                    g = 255 - g;
                    b = 255 - b;

                    //set new ARGB value in pixel
                    bmp.SetPixel(x, y, Color.FromArgb(a, r, g, b));
                }
            }
            pictureBox1.Image = bmp;

        }
        Bitmap mirrorImg;
        Bitmap beforMirror;
        private void btnMirror_Click(object sender, EventArgs e)
        {
            unVisiblePanel();

            beforMirror = new Bitmap(pictureBox1.Image);
            mirrorImg = new Bitmap(pictureBox1.Image.Width*2,pictureBox1.Image.Height);
            for (int y = 0; y < pictureBox1.Image.Height; y++)
            {
                for (int lx = 0, rx = pictureBox1.Image.Width * 2 - 1; lx < pictureBox1.Image.Width; lx++, rx--)
                {
                    //get source pixel value
                    Color p = beforMirror.GetPixel(lx, y);

                    //set mirror pixel value
                    mirrorImg.SetPixel(lx, y, p);
                    mirrorImg.SetPixel(rx, y, p);
                }
            }
            pictureBox1.Image = mirrorImg;

        }
        Image ZoomPicture(Image img, Size size)
        {
            Bitmap bm = new Bitmap(img, Convert.ToInt32(img.Width * size.Width),
                Convert.ToInt32(img.Height * size.Height));
            Graphics gpu = Graphics.FromImage(bm);
            gpu.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            return bm;
        }
        PictureBox imgZoom;
        private void trZoom_Scroll(object sender, EventArgs e)
        {
            if (trZoom.Value != 0)
            {
                pictureBox1.Image = ZoomPicture(imgZoom.Image, new Size(trZoom.Value, trZoom.Value));
                
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //trancparent
            unVisiblePanel();
            Bitmap pic = new Bitmap(pictureBox1.Image);
            for (int w = 0; w < pic.Width; w++)
            {
                for (int h = 0; h < pic.Height; h++)
                {
                    Color c = pic.GetPixel(w, h);
                    Color newC = Color.FromArgb(50, c);
                    pic.SetPixel(w, h, newC);
                }
            }
            pictureBox1.Image = pic;
        }

        private void txtwidth_TextChanged(object sender, EventArgs e)
        {
        }

        private void btnResize_Click(object sender, EventArgs e)
        {
            unVisiblePanel();
            pnResize.Visible = true;
            

        }

        private void btnRotate_Click(object sender, EventArgs e)
        {
            unVisiblePanel();
            Image img = pictureBox1.Image;
            img.RotateFlip(RotateFlipType.Rotate90FlipNone);
            pictureBox1.Image = img;
        }

        private void btnReverse_Click(object sender, EventArgs e)
        {
            unVisiblePanel();
            Image img = pictureBox1.Image;
            img.RotateFlip(RotateFlipType.Rotate270FlipNone);
            pictureBox1.Image = img;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //copy
            unVisiblePanel();

            Image img = new Bitmap(pictureBox1.Width, pictureBox1.Height);

            Graphics g = Graphics.FromImage(img);

            g.CopyFromScreen(PointToScreen(pictureBox1.Location), new Point(0, 0), new Size(pictureBox1.Width, pictureBox1.Height));

            Clipboard.SetImage(img);

            g.Dispose();
        }

        private void btnWriteText_Click(object sender, EventArgs e)
        {
            unVisiblePanel();

            pantext.Visible = true;
        }

        private void btnmerge_Click(object sender, EventArgs e)
        {

            

           
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //zoom
            unVisiblePanel();
            pnzoom.Visible = true;
        }

        private void btnAddText_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                using (Font font1 = new Font("Arial", 17f))
                {
                    Bitmap bmp = new Bitmap(pictureBox1.Image);
                    using (Graphics gr = Graphics.FromImage(bmp))
                    {
                        var p = pictureBox1.PointToClient(Cursor.Position);
                        gr.DrawString(textBox1.Text.ToString(), font1, Brushes.Black, p);
                    }
                    using (pictureBox1.Image) pictureBox1.Image = bmp;
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            unVisiblePanel();

            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "PNG Image|*.png|JPeg Image|*.jpg";
            dialog.Title = "Save Chart As Image File";
            dialog.FileName = "Sample.png";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                int width = Convert.ToInt32(pictureBox1.Image.Width);
                int height = Convert.ToInt32(pictureBox1.Image.Height);
                Bitmap bmp = new Bitmap(width, height);
                DrawToBitmap(bmp, new Rectangle(0, 0, width, height));
                bmp.Save(dialog.FileName, ImageFormat.Jpeg);
            }
          
        }
        public void convert(string selectformat)
        {
            try
            {
                if (selectformat == "JPEG")
                {
                    pictureBox1.Image.Save(@"C:\Users\اا\Desktop\photo.Jpeg", ImageFormat.Jpeg);
                }
                else if (selectformat == "PNG")
                {
                    pictureBox1.Image.Save(@"C:\Users\اا\Desktop\photo.Png", ImageFormat.Png);
                }
                else if (selectformat == "Gif")
                {
                    pictureBox1.Image.Save(@"C:\Users\اا\Desktop\photo.Gif", ImageFormat.Gif);
                }
                else if (selectformat == "Tiff")
                {
                    pictureBox1.Image.Save(@"C:\Users\اا\Desktop\photo.Tiff", ImageFormat.Tiff);
                }
            }
            catch(Exception)
            {

            }

        }
        private void save(Image image)
        {
            image.Save(@"C:\photo.Jpeg", ImageFormat.Jpeg);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
            convert(comboBox1.Text);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            //show convert panel
            pnConvert.Visible = true;
        }

        private void btnmerge_Click_1(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "Image Files(*.jpg; *.jpeg; *.gif; *.bmp)|*.jpg; *.jpeg; *.gif; *.bmp";
            if (open.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.BackgroundImage = new Bitmap(open.FileName);
              
            }

            Bitmap pic = new Bitmap(pictureBox1.Image);
            for (int w = 0; w < pic.Width; w++)
            {
                for (int h = 0; h < pic.Height; h++)
                {
                    Color c = pic.GetPixel(w, h);
                    Color newC = Color.FromArgb(50, c);
                    pic.SetPixel(w, h, newC);
                }
            }
            pictureBox1.Image = pic;

            Bitmap pic2 = new Bitmap(pictureBox1.BackgroundImage);
            for (int w = 0; w < pic2.Width; w++)
            {
                for (int h = 0; h < pic2.Height; h++)
                {
                    Color c = pic2.GetPixel(w, h);
                    Color newC = Color.FromArgb(50, c);
                    pic2.SetPixel(w, h, newC);
                }
            }
            pictureBox1.BackgroundImage = pic2;
        }

        private void button6_Click_1(object sender, EventArgs e)
        {
            //convertformat image
            unVisiblePanel();
            pnConvert.Visible = true;
        }

        private void pnConvert_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnSaveResize_Click(object sender, EventArgs e)
        {
            pictureBox1.Width = Convert.ToInt32(txtwidth.Text);
            pictureBox1.Height = Convert.ToInt32(txtheight.Text);
        }

        private void btnSaveDB_Click(object sender, EventArgs e)
        {
            DataAccessLayer dal = new DataAccessLayer();
            Bitmap bitmap = new Bitmap(pictureBox1.Image);
            CustomImage cm = new CustomImage("photo", bitmap);
            dal.SaveImage(cm);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using MultiMediaCV;


namespace multimedia
{
    public  partial  class  Form1 : Form
    {
        //bool _streaming;
       // Capture _capture;
        Image tempImage;
        Image SaveFilterImage;

        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            
            unVisiblePanel();
            enabledButton();
            this.DoubleBuffered = true;
            lbtext();


        }
        Stack<Image> undoIamges = new Stack<Image>();
        //Image undoImg;
        private void unVisiblePanel()
        {
            panCamera.Visible = false;
            panfilter.Visible = false;
            pnResize.Visible = false;
            pantext.Visible = false;
            pnSearch.Visible = false;
            pnSave.Visible = false;
            undoIamges.Push(pictureBox1.Image);
            //undoImg = pictureBox1.Image;
            lbTxt.Text = null;
            button2.Visible = false;

            enabledButton();



        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {


        }

        private void openimg_Click(object sender, EventArgs e)
        {
            try
            {
                pictureBox1.GetImageFromDevice(this);
                tempImage = pictureBox1.Image;
                unVisiblePanel();
                enabledButton();
            }
            catch (Exception)
            {

            }
            

        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                pictureBox1.CaptuerWebCam(this);
                unVisiblePanel();
                enabledButton();
            }
            catch
            {

            }
           

        }

        private void btnStream_Click(object sender, EventArgs e)
        {
            //if (!_streaming)
            //{
            //    Application.Idle += streaming;
            //    btnStream.Text = @"Stop Streaming";
            //}
            //else
            //{
            //    Application.Idle -= streaming;
            //    btnStream.Text = @"Start Streaming";
            //}
            //_streaming = !_streaming;
            //ptstream.GetImageFromCamera();
            //button2.Enabled = true;
        }

       
        //private void streaming(object sender, System.EventArgs e)
        //{
        //    var img = _capture.QueryFrame().ToImage<Bgr, byte>();
        //    var bmp = img.Bitmap;
        //    ptstream.Image = bmp;
        //}

        private void ptstream_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            unVisiblePanel();
            button2.Visible = true;
            pictureBox1.GetImageFromCamera();
            enabledButton();
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
            lbTxt.Text = null;
            pictureBox1.Image = tempImage;
            pictureBox1.ResetZoom();

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
        //PictureBox imgZoom;
        //private void trZoom_Scroll(object sender, EventArgs e)
        //{
        //    if (trZoom.Value != 0)
        //    {
        //        pictureBox1.Image = ZoomPicture(imgZoom.Image, new Size(trZoom.Value, trZoom.Value));
                
        //    }
        //}

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

            Clipboard.SetDataObject(pictureBox1.Image);

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
        }

        private void btnAddText_Click(object sender, EventArgs e)
        {
            
            
            lbTxt.Text += textBox1.Text;
            textBox1.Text = "";

           
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            unVisiblePanel();

            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "PNG Image|*.png|JPeg Image|*.jpg";
            dialog.Title = "Save Chart As Image File";
            dialog.FileName = txtNameDb+".png";
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
                    pictureBox1.Image.Save(@"C:\Users\اا\Desktop\"+txtNameDb.Text+".Jpeg", ImageFormat.Jpeg);
                }
                else if (selectformat == "PNG")
                {
                    pictureBox1.Image.Save(@"C:\Users\اا\Desktop\" + txtNameDb.Text + ".Png", ImageFormat.Png);
                }
                else if (selectformat == "Gif")
                {
                    pictureBox1.Image.Save(@"C:\Users\اا\Desktop\" + txtNameDb.Text + ".Gif", ImageFormat.Gif);
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
        }

        private void btnmerge_Click_1(object sender, EventArgs e)
        {
            unVisiblePanel();
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
            //save panel image
            unVisiblePanel();
            pnSave.Visible = true;

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
            CustomImage cm = new CustomImage(txtNameDb.Text, bitmap);
            dal.SaveImage(cm);
        }


        private void btnCropImg_Click(object sender, EventArgs e)
        {
           
            pictureBox1.Crop(Cursor);

        }

        private void lbTxt_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_MouseEnter(object sender, EventArgs e)
        {
            base.OnMouseEnter(e);
            Cursor = Cursors.Cross;
        }

        private void btnRecoveryTxt_Click(object sender, EventArgs e)
        {
            textBox1.Text = lbTxt.Text;
        }

        private void btnFont_Click(object sender, EventArgs e)
        {
            FontDialog fontDialog = new FontDialog();
            fontDialog.ShowColor = true;
            if (fontDialog.ShowDialog() == DialogResult.OK & !string.IsNullOrEmpty(textBox1.Text))
            {
                textBox1.Font = fontDialog.Font;
                textBox1.ForeColor = fontDialog.Color;
                lbTxt.Font = fontDialog.Font;
                lbTxt.ForeColor = fontDialog.Color;

            }
        }

        //private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        //{
        //    base.OnMouseMove(e);
        //    if (e.Button == System.Windows.Forms.MouseButtons.Left)
        //    {
        //        pictureBox1.Refresh();
        //        //set width and height for crop rectangle.
        //        rectW = e.X - crpX;
        //        rectH = e.Y - crpY;
        //        Graphics g = pictureBox1.CreateGraphics();
        //        g.DrawRectangle(crpPen, crpX, crpY, rectW, rectH);
        //        g.Dispose();
        //    }
        //}
        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            Cursor = Cursors.Default;
        }
        private void lbtext()
        {
            ControlExtension.Draggable(lbTxt, true);
            lbTxt.Parent = pictureBox1;
            lbTxt.BackColor = Color.Transparent;
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            GetImage.CloseWebCam();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            DataAccessLayer dal = new DataAccessLayer();
            CustomImage img;
            var res = dal.Search(txtSearch.Text);
            for (int i = 0; i <res.Count; i++)
            {
                img = new CustomImage(res[i].id, res[i].Name,res[i].Binary);
                listBox1.Items.Add(img.Bitmap);
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            pictureBox1.Image = (Image) listBox1.SelectedItem;
        }

        private void btnShowSearch_Click(object sender, EventArgs e)
        {
            unVisiblePanel();
            pnSearch.Visible = true;
        }

        private void btnRemoveImg_Click(object sender, EventArgs e)
        {
            unVisiblePanel();
            pictureBox1.Image = null;
            pictureBox1.BackgroundImage = null;
            enabledButton();
        }

        private void btnUndo_Click(object sender, EventArgs e)
        {
            Image undoimage = undoIamges.Pop();
            if (undoimage != null)
                pictureBox1.Image = undoimage;
            else
                pictureBox1.Image = tempImage;
        }

        private void btnSaveTxt_Click(object sender, EventArgs e)
        {
            using (Font font = new Font(lbTxt.Font.FontFamily, lbTxt.Font.Size))
            using (Graphics G = Graphics.FromImage(pictureBox1.Image))
            {
                G.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixel;
                G.DrawString(lbTxt.Text, font, Brushes.Red, lbTxt.Location.X, lbTxt.Location.Y);
            }
            pictureBox1.Invalidate();
            unVisiblePanel();
        }

        private void openimg_MouseHover(object sender, EventArgs e)
        {
            toolTip1.Show("OpenImage", openimg);
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            pictureBox1.ResetZoom();

        }

        private void pictureBox1_MouseMove_1(object sender, MouseEventArgs e)
        {
            try
            {
                pictureBox1.MouseMoveCV(e);

            }
            catch (Exception)
            {

            }

        }

        private void pictureBox1_MouseDown_1(object sender, MouseEventArgs e)
        {
            try
            {
                pictureBox1.MouseDownCV(e, Cursor, Color.Red);

            }
            catch
            {

            }
        }
       private void enabledButton()
        {
            if (pictureBox1.Image == null)
            {
                panel1.Enabled = false;
                openimg.Enabled = true;
                button1.Enabled = true;
                btnUndo.Enabled = false;
                button3.Enabled = false;
                btnCropImg.Enabled = false;
            }
            else
            {
                panel1.Enabled = true;
                openimg.Enabled = true;
                button1.Enabled = true;
                btnUndo.Enabled = true;
                button3.Enabled = true;
                btnCropImg.Enabled = true;
            }

        }
    }
}

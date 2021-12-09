using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;


namespace EDgeDet
{

    public partial class Form1 : Form
  
    {
        public Form1()
        {
            InitializeComponent();
            Class1 vay = null;
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog dosya = new OpenFileDialog();
            dosya.Filter = "Resim Dosyası |*.jpg;*.nef;*.png;*.jpeg| Video|*.avi| Tüm Dosyalar |*.*";
            dosya.ShowDialog();
            string DosyaYolu = dosya.FileName;
            pictureBox1.ImageLocation = DosyaYolu;
        }
        private Bitmap gray(Bitmap image)
        {

            for (int i =0; i< image.Height-1; i++)
            { for( int j=0; j< image.Width - 1; j++)
                {
                    int ortalama = (image.GetPixel(j, i).R + image.GetPixel(j, i).G + image.GetPixel(j, i).B) / 3;
                    Color gri;
                    gri = Color.FromArgb(ortalama, ortalama, ortalama);
                    image.SetPixel(j, i, gri);

                }

            }

            return image;
        }

        int degerw; //resmin piksel cinsinden genişlik değeri
        int degerh; //resmin piksel cinsinden yükseklik değeri

        private void button2_Click(object sender, EventArgs e)
        {
            Bitmap resim = new Bitmap(pictureBox1.Image);
            degerh = pictureBox1.Image.Height;
            degerw = pictureBox1.Image.Width;
            label5.Text = "Resim Boyutları W=" + degerw + "H=" + degerh;
            Bitmap griresim = gray(resim); 
            pictureBox2.Image = griresim;
            
        }

        int scale = new int(); // resize ölçüsü

        private Bitmap edgedetection(Bitmap image)
        {
            scale = 1; 
            this.pictureBox3.Size = new System.Drawing.Size(degerw /scale, degerh /scale);
            Bitmap resim = new Bitmap(pictureBox1.Image);
            Bitmap griresim = gray(resim);
            int dikey, yatay;
            

            int[,] Y = new int[3, 3];

            Y[0, 0] = -1; Y[0, 1] = 0; Y[0, 2] = 1;
            Y[1, 0] = -2; Y[1, 1] = 0; Y[1, 2] = 2;
            Y[2, 0] = -1; Y[2, 1] = 0; Y[2, 2] = 1;
            

            int[,] X = new int[3, 3];

            X[0, 0] = 1; X[0, 1] = 2; X[0, 2] = 1;
            X[1, 0] = 0; X[1, 1] = 0; X[1, 2] = 0;
            X[2, 0] = -1; X[2, 1] = -2; X[2, 2] = -1;

            for (int i = 0; i < image.Height - 1; i++)
            {
                for (int j = 0; j < image.Width - 1; j++)
                {
                    if(i==0 || j==0|| i==image.Height-1|| j == image.Width - 1)
                    {

                        dikey = 0;
                        yatay = 0;

                    }
                    else
                    {
                            yatay = griresim.GetPixel(j - 1, i - 1).R * X[0, 0] +
                            griresim.GetPixel(j, i-1).R * X[0, 1] +
                            griresim.GetPixel(j+1, i-1).R * X[0, 2] +
                            griresim.GetPixel(j-1, i).R * X[1, 0] +
                            griresim.GetPixel(j, i).R * X[1, 1] +
                            griresim.GetPixel(j+1, i).R * X[1, 2] +
                            griresim.GetPixel(j-1, i+ 1).R * X[2, 0] +
                            griresim.GetPixel(j, i+1).R * X[2, 1] +
                            griresim.GetPixel(j + 1, i + 1).R * X[2, 2];


                            dikey= griresim.GetPixel(j - 1, i - 1).R * Y[0, 0] +
                            griresim.GetPixel(j, i - 1).R * Y[0, 1] +
                            griresim.GetPixel(j + 1, i - 1).R * Y[0, 2] +
                            griresim.GetPixel(j - 1, i).R * Y[1, 0] +
                            griresim.GetPixel(j, i).R * Y[1, 1] +
                            griresim.GetPixel(j + 1, i).R * Y[1, 2] +
                            griresim.GetPixel(j - 1, i + 1).R * Y[2, 0] +
                            griresim.GetPixel(j, i + 1).R * Y[2, 1] +
                            griresim.GetPixel(j + 1, i + 1).R * Y[2, 2];

                        int gradient;
                        gradient =(int) (Math.Abs(yatay) + Math.Abs(dikey));

                        if(gradient<0) { gradient = 0; }
                        if (gradient > 255) { gradient = 255; }

                        Color renk;

                        renk = Color.FromArgb(gradient, gradient, gradient);
                        image.SetPixel(j, i, renk);

                    }

                }

            }

            return image;
        }
        private void button3_Click(object sender, EventArgs e)
        {
            Bitmap resim = new Bitmap(pictureBox1.Image);
            Bitmap kenarlar= edgedetection(resim);
            pictureBox3.Image = kenarlar;

        }
        
        double piksel1x = new double(); // pic.box 3 içerisindeki seçilen 1. piksel değerleri
        double piksel1y = new double();

        private void pictureBox3_MouseClick(object sender, MouseEventArgs e)
        {

            label1.Text = "Birinci Noktanın Piksel değerleri X=" + e.X + " Y=" + e.Y;

            piksel1x = e.X;
            piksel1y = e.Y;

            double db=Math.Sqrt(Math.Pow(piksel1x- piksel2x, 2) + Math.Pow(piksel1y - piksel2y, 2));
            double db2= ((Math.Sqrt(Math.Pow(piksel1x - piksel2x, 2) + Math.Pow(piksel1y - piksel2y, 2)))/(3.3/scale)) ;
            string st = db.ToString();
            string st2 = db2.ToString();
            label3.Text = "piksel adedi"+ st;
            label4.Text = "ölçülen mesafe"+st2 + "um";
        

        }

        int piksel2x = new int(); //pic.box 3 içerisindeki seçilen 2. piksel değerleri
        int piksel2y = new int();

        private void pictureBox3_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            label2.Text = "İkinci Noktanın Piksel değerleri X=" + e.X + "Y=" + e.Y;
            piksel2x = e.X;
            piksel2y = e.Y;
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
         
        }

        private void Form1_Resize(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
         

        }
        
        private void pictureBox3_Paint(object sender, PaintEventArgs e)
        {
        }

        
    }
}

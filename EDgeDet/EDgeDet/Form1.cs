using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Linq;
using TIS.Imaging;



namespace EDgeDet
{

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        //----------------------------------------------------------------- 
        int degerw; //resmin piksel cinsinden genişlik değeri
        int degerh; //resmin piksel cinsinden yükseklik değeri
        int scale = new int(); // resize ölçüsü
        int thresholdkontrast = 5;
        //-----------------------------------------------------------------
        Bitmap grayimage; // griye dönüştürülmüş map
        float piksel1x = new float(); // pic.box 3 içerisindeki seçilen 1. piksel x değerleri
        float piksel1y = new float(); // pic.box 3 içerisindeki seçilen 1. piksel y değerleri
        int piksel2x = new int(); //pic.box 3 içerisindeki seçilen 2. piksel x değerleri
        int piksel2y = new int(); //pic.box 3 içerisindeki seçilen 2. piksel y değerleri
        double atanankatsayi = new double(); // manuel ölçümde kullanılacak katsayi 
        bool ismouseclicktrue = new bool(); // rectangle i çizer
        Rectangle rec1 = new Rectangle();
        Rectangle rec2 = new Rectangle();
        Rectangle rec3 = new Rectangle();
        //----------------------------------------------------------------- KALİBRASYON
        double tekraredenkatsayi1 = new double(); // kalibrasyonda tekrarı en çok olan 1. mesafe
        double tekraredenkatsayi2 = new double(); // kalibrasyonda tekrarı en çok olan 2. mesafe
        int count = new int();
        int count2 = new int();
        int tempcount = new int();
        double tempkatsayi = new double();
        double tempkatsayi2 = new double();
        int tempcount2 = new int();
        double sonuc = new double(); // hesaplanan kalibrasyon katsayısı
        //------------------------------------------------------------------
        bool button6WasClicked = false;
        bool button7click = new bool(); // sol köşe ölçümü aktifleştirme
        bool button8click = new bool(); // sağ köşe 
        bool button10click = new bool(); // üst köşe
        bool button9click = new bool(); // alt köşe

        bool buttonbrinellsolclick = new bool();
        bool buttonbrinellsagclick = new bool();
        bool buttonbrinellüstclick = new bool();
        bool buttonbrinellaltclick = new bool();
        bool buttonmatristersiclick = new bool();

        bool dataekle = new bool(); //datagrid veri ekleme
        bool cros1click = new bool(); //manuel cross çizimi
        bool cros2click = new bool();

        //------------------------------------------------------------------KALİBRASYON VERİSİ KAYDETMEK
        double gorecedeger5x = Properties.kaydet.Default.gorecedeger5xk;
        double gorecedeger10x = Properties.kaydet.Default.gorecedeger10xk;
        double gorecedeger20x = Properties.kaydet.Default.gorecedeger20xk;
        double gorecedeger50x = Properties.kaydet.Default.gorecedeger50xk;
        double katsayi = new double();
        //------------------------------------------------------------------
        Point kesisimsol = new Point(); //uç noktalar vickers
        Point kesisimsag = new Point();
        Point kesisimust = new Point();
        Point kesisimalt = new Point();

        Point brinellsol = new Point(); // uç noktaları brinell
        Point brinellsag = new Point();
        Point brinellalt = new Point();
        Point brinellust = new Point();
        //-------------------------------------------------------------------
        int pikselrecx = new int();
        int pikselrecy = new int();

        bool vickersactive = new bool();
        bool brinelactive = new bool();

        // BUTONLAR//------------------------------------------------------------------------------------------------------
        private void button1_Click(object sender, EventArgs e) //--> Dosyadan Resim Açma 
        {
            OpenFileDialog dosya = new OpenFileDialog();
            dosya.Filter = "Resim Dosyası |*.jpg;*.nef;*.png;*.jpeg| Video|*.avi| Tüm Dosyalar |*.*";
            dosya.ShowDialog();
            string DosyaYolu = dosya.FileName;
            pictureBox1.ImageLocation = DosyaYolu;
            //Bitmap resim = new Bitmap(Image.FromFile("test.bmp"));
            Bitmap resim = new Bitmap(Image.FromFile(DosyaYolu));
            degerh = resim.Height;
            degerw = resim.Width;
            label5.Text = " W=" + degerw + "H=" + degerh;
            Bitmap griresim = gray(resim);
            pictureBox3.Image = griresim;

       // C: \Users\kuvvet\Desktop\WindowsFormsAppe\WindowsFormsAppe\bin\Debug\test.bmp

        }

        private void button21_Click(object sender, EventArgs e) // Kameradan görüntü alma
        {
            Bitmap resim = new Bitmap(Image.FromFile("test.bmp"));
            degerh = resim.Height;
            degerw = resim.Width;
            label5.Text = " W=" + degerw + "H=" + degerh;
            Bitmap griresim = gray(resim);
            pictureBox3.Image = griresim;
        }

        //public void button2_Click(object sender, EventArgs e) //--> Resmi S/B Dönüştürme Butonu
        //{
        //    Bitmap resim = new Bitmap(pictureBox1.Image);
        //    degerh = pictureBox1.Image.Height;
        //    degerw = pictureBox1.Image.Width;
        //    label5.Text = "Resim Boyutları W=" + degerw + "H=" + degerh;
        //    Bitmap griresim = gray(resim);
        //    pictureBox2.Image = griresim;

        //}
        //public void button3_Click(object sender, EventArgs e) //--> Kenar Algılama Butonu
        //{
        //    Bitmap resim = new Bitmap(pictureBox1.Image);

        //    grayimage = gray(resim);
        //    pictureBox3.Image = grayimage;

        //}
        private void button4_Click(object sender, EventArgs e) //--> Kalibrasyon Butonu.
        {
            Bitmap resim = new Bitmap(pictureBox3.Image);
            katsayi = Katsayibul(resim);

            switch (comboBox1.SelectedIndex)
            {
                case 0: //5x

                    switch (comboBox2.SelectedIndex)
                    {
                        case 0:// 5x 10um
                            gorecedeger5x = katsayi / 10;
                            label7.Text = "Kalibrasyon Katsayısı:" + Convert.ToString(gorecedeger5x);
                            Properties.kaydet.Default.gorecedeger5xk = gorecedeger5x; break;

                        case 1: //5x 20um
                            gorecedeger5x = katsayi / 20;
                            label7.Text = "Kalibrasyon Katsayısı:" + Convert.ToString(gorecedeger5x);
                            Properties.kaydet.Default.gorecedeger5xk = gorecedeger5x; break;

                        case 2: //5x 200um
                            gorecedeger5x = katsayi / 200;
                            label7.Text = "Kalibrasyon Katsayısı:" + Convert.ToString(gorecedeger5x);
                            Properties.kaydet.Default.gorecedeger5xk = gorecedeger5x; break;

                    }; break;

                case 1: //10x

                    switch (comboBox2.SelectedIndex)
                    {
                        case 0:// 10x 10um
                            gorecedeger10x = katsayi / 10;
                            label7.Text = "Kalibrasyon Katsayısı:" + Convert.ToString(gorecedeger10x);
                            Properties.kaydet.Default.gorecedeger10xk = gorecedeger10x;
                            Properties.kaydet.Default.Save(); break;

                        case 1: //10x 20um
                            gorecedeger10x = katsayi / 20;
                            label7.Text = "Kalibrasyon Katsayısı:" + Convert.ToString(gorecedeger10x);
                            Properties.kaydet.Default.gorecedeger10xk = gorecedeger10x;
                            Properties.kaydet.Default.Save(); break;

                        case 2: //10x 200um
                            gorecedeger10x = katsayi / 200;
                            label7.Text = "Kalibrasyon Katsayısı:" + Convert.ToString(gorecedeger10x);
                            Properties.kaydet.Default.gorecedeger10xk = gorecedeger10x;
                            Properties.kaydet.Default.Save(); break;

                    }; break;

                case 2: //20x

                    switch (comboBox2.SelectedIndex)
                    {
                        case 0:// 20x 10um
                            gorecedeger20x = katsayi / 10;
                            label7.Text = "Kalibrasyon Katsayısı:" + Convert.ToString(gorecedeger20x);
                            Properties.kaydet.Default.gorecedeger20xk = gorecedeger20x;
                            Properties.kaydet.Default.Save(); break;


                        case 1: //20x 20um
                            gorecedeger20x = katsayi / 20;
                            label7.Text = "Kalibrasyon Katsayısı:" + Convert.ToString(gorecedeger20x);
                            Properties.kaydet.Default.gorecedeger20xk = gorecedeger20x;
                            Properties.kaydet.Default.Save(); break;


                        case 2: //20x 200um
                            gorecedeger20x = katsayi / 200;
                            label7.Text = "Kalibrasyon Katsayısı:" + Convert.ToString(gorecedeger20x);
                            Properties.kaydet.Default.gorecedeger20xk = gorecedeger20x;
                            Properties.kaydet.Default.Save(); break;

                    }; break;

                case 3: // 50x

                    switch (comboBox2.SelectedIndex)
                    {
                        case 0:// 50x 10um
                            gorecedeger50x = katsayi / 10;
                            label7.Text = "Kalibrasyon Katsayısı:" + Convert.ToString(gorecedeger50x);
                            Properties.kaydet.Default.gorecedeger50xk = gorecedeger50x;
                            Properties.kaydet.Default.Save(); break;

                        case 1: //50x 20um
                            gorecedeger50x = katsayi / 20;
                            label7.Text = "Kalibrasyon Katsayısı:" + Convert.ToString(gorecedeger50x);
                            Properties.kaydet.Default.gorecedeger50xk = gorecedeger50x;
                            Properties.kaydet.Default.Save(); break;

                        case 2: //50x 200um
                            gorecedeger50x = katsayi / 200;
                            label7.Text = "Kalibrasyon Katsayısı:" + Convert.ToString(gorecedeger50x);
                            Properties.kaydet.Default.gorecedeger50xk = gorecedeger50x;
                            Properties.kaydet.Default.Save(); break;

                    }; break;

                default: MessageBox.Show("Yapılmayan Seçimler Var."); break;
            }

            label10.Text = "Piksel Adedi:" + katsayi;
        }
        private void button5_Click(object sender, EventArgs e)
        {
            switch (comboBox3son.SelectedIndex)
            {
                case 0:// 50x 10um
                    atanankatsayi = gorecedeger5x; break;

                case 1: //50x 20um
                    atanankatsayi = gorecedeger10x; break;

                case 2: //50x 200um
                    atanankatsayi = gorecedeger20x; break;

                case 3: //50x
                    atanankatsayi = gorecedeger50x; break;

                default: MessageBox.Show("Lens seçiniz."); break;
            }
        }
        private void button6_Click(object sender, EventArgs e) //-----> Kalibrasyon için hizala butonu.
        {
            button6WasClicked = true;
        }
        private void button7_Click(object sender, EventArgs e) 
        {
            button7click = !button7click;
            button8click = false;
            button9click = false;
            button10click = false;
        } // -----> sol kenar
        private void button8_Click(object sender, EventArgs e) // -----> sağ kenar
        {
            button8click = !button8click;
            button7click = false;
            button9click = false;
            button10click = false;
        }
        private void button9_Click(object sender, EventArgs e) // -----> alt kenar 
        {
            button9click = !button9click;
            button7click = false;
            button8click = false;
            button10click = false;
        }
        private void button10_Click(object sender, EventArgs e) // -----> üst kenar
        {
            button10click = !button10click;
            button7click = false;
            button9click = false;
            button8click = false;
        }
        private void ButtonAutoMeas_Click(object sender, EventArgs e) // vickers oto ölçüm
        {
            List<Point> points = new List<Point>();
            List<Point> points2 = new List<Point>();
            List<Point> points3 = new List<Point>();
            List<Point> bestpoints = new List<Point>();
            List<Point> bestpoints2 = new List<Point>();
            List<Point> bestpoints3 = new List<Point>();
            List<Point> draw = new List<Point>();
            List<Point> draw2 = new List<Point>();
            List<Point> draw3 = new List<Point>();
            double intercept2 = new double();
            double intercept3 = new double();
            double slope2 = new double();
            double slope3 = new double();
            
            Point ortanokta = new Point();
            Bitmap gereksiz;

            if (button7click == true) //sol
            {
                Oto_olcum_sol_köse(rec1, out points, out gereksiz);
                ortanoktabul_sol(points, rec1, out ortanokta);
                // drawcross(ortanokta); orta nokta bulunmuş mu kontrol


                rec2 = new Rectangle(new Point(rec1.X, rec1.Y), new Size(rec1.Width, ortanokta.Y - rec1.Y));
                rec3 = new Rectangle(new Point(rec1.X, ortanokta.Y), new Size(rec1.Width, rec1.Y + rec1.Height - ortanokta.Y));
                //bulunan orta noktaya göre rec2 ve rec3 çizdirildi.

                Oto_olcum_sol_köse(rec2, out points2, out gereksiz);
                BestFitLine2(rec2, out bestpoints2, out intercept2, out slope2);
                Eliminate(points2, bestpoints2, out draw2);
                // rec2 için "best line" tayini

                Oto_olcum_sol_köse(rec3, out points3, out gereksiz);
                BestFitLine2(rec3, out bestpoints3, out intercept3, out slope3);
                Eliminate(points3, bestpoints3, out draw3);
                // rec3 için "best line" tayini

                dogrukesisimbul(intercept2, intercept3, slope2, slope3, out kesisimsol);
                //doğruların kesiştiği nokta bulundu.


                Graphics g = pictureBox3.CreateGraphics();
                Pen REDPen = new Pen(Color.FromArgb(255, 0, 0), 1 / 2);

                drawcross(kesisimsol,REDPen);

                Point p1 = new Point(draw2[0].X, draw2[0].Y); // try catch kullan hata veriyor
                Point P2 = new Point(draw2[draw2.Count - 1].X, draw2[draw2.Count - 1].Y);
                g.DrawLine(REDPen, p1, P2);

                Point p3 = new Point(draw3[0].X, draw3[0].Y); // try catch kullan
                Point P4 = new Point(draw3[draw3.Count - 1].X, draw3[draw3.Count - 1].Y);
                g.DrawLine(REDPen, p3, P4);
                wait(100);
                //line1 line2 ve cross çizdirildi.

            }
            if (button10click == true) // üst
            {

                Oto_olcum_üst_köse(rec1, out points, out gereksiz);
                ortanoktabul_üst(points, rec1, out ortanokta);
                //drawcross(ortanokta); orta nokta doğru mu kontrol.

                rec2 = new Rectangle(new Point(rec1.X, rec1.Y), new Size(ortanokta.X - rec1.X, rec1.Height));
                rec3 = new Rectangle(new Point(ortanokta.X, rec1.Y), new Size(rec1.X + rec1.Width - ortanokta.X, rec1.Height));
                //bulunan orta noktaya göre rec2 ve rec3 çizdirildi.

                Oto_olcum_üst_köse(rec2, out points2, out gereksiz);
                BestFitLine2(rec2, out bestpoints2, out intercept2, out slope2);
                Eliminate(points2, bestpoints2, out draw2);
                // rec2 için "best line" tayini

                Oto_olcum_üst_köse(rec3, out points3, out gereksiz);
                BestFitLine2(rec3, out bestpoints3, out intercept3, out slope3);
                Eliminate(points3, bestpoints3, out draw3);
                // rec3 için "best line" tayini

                dogrukesisimbul(intercept2, intercept3, slope2, slope3, out kesisimust);
                //doğruların kesiştiği nokta bulundu.

                Graphics g = pictureBox3.CreateGraphics();
                Pen REDPen = new Pen(Color.FromArgb(255, 0, 0), 1 / 2);

                drawcross(kesisimust,REDPen);

                Point p1 = new Point(draw2[0].X, draw2[0].Y); // try catch kullan hata veriyor
                Point P2 = new Point(draw2[draw2.Count - 1].X, draw2[draw2.Count - 1].Y);
                g.DrawLine(REDPen, p1, P2);

                Point p3 = new Point(draw3[0].X, draw3[0].Y); // try catch kullan
                Point P4 = new Point(draw3[draw3.Count - 1].X, draw3[draw3.Count - 1].Y);
                g.DrawLine(REDPen, p3, P4);
                wait(100);
                //line1 line2 ve cross çizdirildi.

            }
            if (button8click == true) //sağ 
            {

                Oto_olcum_sag_köse(rec1, out points, out gereksiz);
                ortanoktabul_sag(points, rec1, out ortanokta);
                rec2 = new Rectangle(new Point(rec1.X, rec1.Y), new Size(rec1.Width, ortanokta.Y - rec1.Y));
                rec3 = new Rectangle(new Point(rec1.X, ortanokta.Y), new Size(rec1.Width, rec1.Y + rec1.Height - ortanokta.Y));

                Oto_olcum_sag_köse(rec2, out points2, out gereksiz);
                BestFitLine2(rec2, out bestpoints2, out intercept2, out slope2);
                Eliminate(points2, bestpoints2, out draw2);

                Oto_olcum_sag_köse(rec3, out points3, out gereksiz);
                BestFitLine2(rec3, out bestpoints3, out intercept3, out slope3);
                Eliminate(points3, bestpoints3, out draw3);

                dogrukesisimbul(intercept2, intercept3, slope2, slope3, out kesisimsag);

                //BestFitLine2(rec1, out bestpoints);
                //Eliminate(points, bestpoints, out draw);

                Graphics g = pictureBox3.CreateGraphics();
                Pen REDPen = new Pen(Color.FromArgb(255, 0, 0), 1 / 2);

                drawcross(kesisimsag, REDPen);
                Point p1 = new Point(draw2[0].X, draw2[0].Y); // try catch kullan hata veriyor
                Point P2 = new Point(draw2[draw2.Count - 1].X, draw2[draw2.Count - 1].Y);
                g.DrawLine(REDPen, p1, P2);
                Point p3 = new Point(draw3[0].X, draw3[0].Y); // try catch kullan
                Point P4 = new Point(draw3[draw3.Count - 1].X, draw3[draw3.Count - 1].Y);
                g.DrawLine(REDPen, p3, P4);
                wait(100);

            }
            if (button9click == true) // alt
            {

                Oto_olcum_alt_köse(rec1, out points, out gereksiz);
                ortanoktabul_alt(points, rec1, out ortanokta);
                //drawcross(ortanokta);
                // orta nokta doğru mu kontrol.

                rec2 = new Rectangle(new Point(rec1.X, rec1.Y), new Size(ortanokta.X - rec1.X, rec1.Height));
                rec3 = new Rectangle(new Point(ortanokta.X, rec1.Y), new Size(rec1.X + rec1.Width - ortanokta.X, rec1.Height));
                //bulunan orta noktaya göre rec2 ve rec3 çizdirildi.

                Oto_olcum_alt_köse(rec2, out points2, out gereksiz);
                BestFitLine2(rec2, out bestpoints2, out intercept2, out slope2);
                Eliminate(points2, bestpoints2, out draw2);
                // rec2 için "best line" tayini

                Oto_olcum_alt_köse(rec3, out points3, out gereksiz);
                BestFitLine2(rec3, out bestpoints3, out intercept3, out slope3);
                Eliminate(points3, bestpoints3, out draw3);
                // rec3 için "best line" tayini

                dogrukesisimbul(intercept2, intercept3, slope2, slope3, out kesisimalt);
                //doğruların kesiştiği nokta bulundu.

                Graphics g = pictureBox3.CreateGraphics();
                Pen REDPen = new Pen(Color.FromArgb(255, 0, 0), 1 / 2);

                drawcross(kesisimalt,REDPen);

                Point p1 = new Point(draw2[0].X, draw2[0].Y); // try catch kullan hata veriyor
                Point P2 = new Point(draw2[draw2.Count - 1].X, draw2[draw2.Count - 1].Y);
                g.DrawLine(REDPen, p1, P2);

                Point p3 = new Point(draw3[0].X, draw3[0].Y); // try catch kullan
                Point P4 = new Point(draw3[draw3.Count - 1].X, draw3[draw3.Count - 1].Y);
                g.DrawLine(REDPen, p3, P4);
                wait(100);
                //line1 line2 ve cross çizdirildi.

            }
        }   

        //------------------------------------------------------------------------ Buton Brinell
        private void buttonbrinellAutoMeas_Click(object sender, EventArgs e)
        {
            if (buttonbrinellsolclick == true)
            {
                Bitmap bmpbrinell = new Bitmap(pictureBox3.Image);
                Oto_olcum_sol_köse(rec1, out List<Point> points, out Bitmap gereksiz);
                circlebestfit2(points, out int radius, out Point center);
                int i = new int();

                for (i = 0; i < points.Count; i++)
                {
                  bmpbrinell.SetPixel(points[i].X, points[i].Y, Color.Red);
                            
                }

                pictureBox3.Image= bmpbrinell;
                wait(100);
                //circlebestfit(points, rec1, out List<Point> bfcirclepoints, out double Radius, out Point drawpt1, out Point drawpt2);
                //pictureBox3.Image = gereksiz;

                Graphics g = pictureBox3.CreateGraphics();
                Pen REDPen = new Pen(Color.FromArgb(255, 0, 0), 1 / 2);
                Pen Pen = new Pen(Color.FromArgb(0, 0, 255), 1 / 2);

                //getCircle(g, REDPen, center, radius);
                brinellsol = new Point(center.X - radius, center.Y);
                drawcross(brinellsol,Pen);

            }

            if (buttonbrinellsagclick == true)
            {
                Oto_olcum_sag_köse(rec1, out List<Point> points, out Bitmap gereksiz);
                circlebestfit2(points, out int radius, out Point center);
                //circlebestfit(points, rec1, out List<Point> bfcirclepoints, out double Radius, out Point drawpt1, out Point drawpt2);
                pictureBox3.Image = gereksiz;
                wait(100);

                Graphics g = pictureBox3.CreateGraphics();
                Pen REDPen = new Pen(Color.FromArgb(255, 0, 0), 1 / 2);
                Pen Pen = new Pen(Color.FromArgb(0, 0, 255), 1 / 2);

                //getCircle(g, REDPen, center, radius);
                brinellsag = new Point(center.X + radius, center.Y);
                drawcross(brinellsag, Pen);

            }
            if (buttonbrinellüstclick == true)
            {
                Oto_olcum_üst_köse(rec1, out List<Point> points, out Bitmap gereksiz);
                circlebestfit2(points, out int radius, out Point center);
                //circlebestfit(points, rec1, out List<Point> bfcirclepoints, out double Radius, out Point drawpt1, out Point drawpt2);
                pictureBox3.Image = gereksiz;
                wait(100);

                Graphics g = pictureBox3.CreateGraphics();
                Pen REDPen = new Pen(Color.FromArgb(255, 0, 0), 1 / 2);
                Pen Pen = new Pen(Color.FromArgb(0, 0, 255), 1 / 2);

                //getCircle(g, REDPen, center, radius);
                brinellust = new Point(center.X, center.Y - radius);
                drawcross(brinellust, Pen);
            }
            if (buttonbrinellaltclick == true)
            {
                Oto_olcum_alt_köse(rec1, out List<Point> points, out Bitmap gereksiz);
                circlebestfit2(points, out int radius, out Point center);
                //circlebestfit(points, rec1, out List<Point> bfcirclepoints, out double Radius, out Point drawpt1, out Point drawpt2);
                pictureBox3.Image = gereksiz;
                wait(100);

                Graphics g = pictureBox3.CreateGraphics();
                Pen REDPen = new Pen(Color.FromArgb(255, 0, 0), 1 / 2);
                Pen Pen = new Pen(Color.FromArgb(0, 0, 255), 1 / 2);

                //getCircle(g, REDPen, center, radius);
                brinellalt = new Point(center.X, center.Y + radius);
                drawcross(brinellalt, Pen);
            }
        }
        private void buttonbrinellsol_Click(object sender, EventArgs e)
        {
            buttonbrinellsolclick = !buttonbrinellsolclick;
            buttonbrinellsagclick = false;
            buttonbrinellüstclick = false;
            buttonbrinellaltclick = false;
        }
        private void buttonbrinellsag_Click(object sender, EventArgs e)
        {
            buttonbrinellsagclick = !buttonbrinellsagclick;
            buttonbrinellsolclick = false;
            buttonbrinellüstclick = false;
            buttonbrinellaltclick = false;
        }
        private void button14_Click_1(object sender, EventArgs e)
        {
            buttonbrinellüstclick = !buttonbrinellüstclick;
            buttonbrinellsolclick = false;
            buttonbrinellsagclick = false;
            buttonbrinellaltclick = false;
        } //üst
        private void button15_Click(object sender, EventArgs e)
        {
            buttonbrinellaltclick = !buttonbrinellaltclick;
            buttonbrinellsolclick = false;
            buttonbrinellsagclick = false;
            buttonbrinellüstclick = false;
        } //alt
        private void button11_Click(object sender, EventArgs e)
        {
            buttonmatristersiclick = !buttonmatristersiclick;
        }

        //------------------------------------------------------------------- (MENU KISMI) group box'ları öne getir/ arkaya gönder 
        private void button3_Click(object sender, EventArgs e)
        {
            this.groupBox9.BringToFront();
            vickersactive = false;
            brinelactive = true;
        }
        private void buttonkalibrasyon_Click(object sender, EventArgs e)
        {
            this.groupBox2.BringToFront();
            label11.Text = "Kalibrasyon";
        }
        private void buttonmanuel_Click(object sender, EventArgs e)
        {
            this.groupBox3.BringToFront();
            label11.Text = "Manuel Ölçüm";
        }
        private void buttonoto_Click(object sender, EventArgs e)
        {
            this.groupBox14.BringToFront();
            label11.Text = "Otomatik Ölçüm";
        }
        private void buttonekfiltre_Click(object sender, EventArgs e)
        {
            this.groupBox6.BringToFront();
            label11.Text = "Ek Filtreler";
        }
        private void button16_Click(object sender, EventArgs e)
        {
            this.groupBox1.BringToFront();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            this.groupBox5.BringToFront();
            vickersactive = true;
            brinelactive = false;
        }

        //-------------------------------------------------------------------- Button manuel cross ekleme
        private void button17_Click(object sender, EventArgs e)
        {
            cros1click = !cros1click;
            cros2click = false;
        }
        private void button18_Click(object sender, EventArgs e)
        {
            cros2click = !cros2click;
            cros1click = false;
        }

        //-------------------------------------------------------------------- Data gride ekleme
        private void button19_Click(object sender, EventArgs e) 
        {   
            dataekle = !dataekle;
            
            kosegen_capbul(out double horizantalv, out double verticalv, out double horizantalb, out double verticalb, out double absv, out double absb);
            if( vickersactive == true)
            {
                dataGridView1.Rows.Add(horizantalv, verticalv, absv);
                
            }
            if (brinelactive == true)
            {
                dataGridView1.Rows.Add(horizantalb, verticalb, absb);
            }

        }
        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            thresholdkontrast = trackBar1.Value;
            label26.Text = " Eşik Değer:" + thresholdkontrast;
        }




        // GENEL //-------------------------------------------------------------------------------
        private void Form1_Load(object sender, EventArgs e) 
        {
            trackBar1.Maximum = 15;
            trackBar1.Minimum = 1;
           
        }
        private void pictureBox3_MouseClick(object sender, MouseEventArgs e)
        {
            double db = new double();
            double db2= new double();
            string st;
            string st2;
            Pen PenY = new Pen(Color.FromArgb(0, 255, 0), 1);
            Pen PenB = new Pen(Color.FromArgb(0, 0, 255), 1);
            //ismouseclicktrue = true;


            if (cros1click== true)
            {
                
                label1.Text = "1. Piksel X=" + e.X + " Y=" + e.Y;
                piksel1x = e.X;
                piksel1y = e.Y;
                
                drawcross(new Point(Convert.ToInt32(piksel1x), Convert.ToInt32(piksel1y)), PenY);
                cros1click = false;
            }
            if (cros2click == true)
            {
                label2.Text = "2. Piksel X=" + e.X + "Y=" + e.Y;
                piksel2x = e.X;
                piksel2y = e.Y;
                
                drawcross(new Point(Convert.ToInt32(piksel2x), Convert.ToInt32(piksel2y)), PenB);
                cros2click = false;

            }

            db = Math.Sqrt(Math.Pow(piksel1x - piksel2x, 2) + Math.Pow(piksel1y - piksel2y, 2));
            db2 = ((Math.Sqrt(Math.Pow(piksel1x - piksel2x, 2) + Math.Pow(piksel1y - piksel2y, 2))) / (atanankatsayi / scale)); // atanan katsayi= ölçümde combobox seçiminin döndürdüğü değer (line 443)
            st = db.ToString();
            st2 = db2.ToString();
            label3.Text = "piksel adedi" + st;
            label4.Text = "ölçülen mesafe" + st2 + "um";
        }
        private void pictureBox3_MouseDoubleClick_1(object sender, MouseEventArgs e)
        {
            ismouseclicktrue = true;
            pikselrecx = e.X;
            pikselrecy = e.Y;
        }
        private void pictureBox3_Paint_1(object sender, PaintEventArgs e) //--> ölçümdeki rec tanımı ve kalibrasyondaki line tanımı
        {
            if (grayimage != null) //----> seçilen koordinatlara düz çizgi çeker.
            {

                if (button6WasClicked == true)
                {
                    Pen redPen = new Pen(Color.FromArgb(255, 0, 0), 2);
                    // e.Graphics.DrawRectangle(redPen, 10, 10, 100, 50); 
                    PointF point1 = new PointF(piksel1x, piksel1y);
                    PointF point2 = new PointF(piksel2x, piksel1y);
                    e.Graphics.DrawLine(redPen, point1, point2);

                }
            }

            //  köşe ölçümü aktifleştiğinde kırmızı rectangle yerleştirmeyi sağlar.
            if (button7click == true || button8click == true || button9click == true || button10click == true || buttonbrinellsagclick== true || buttonbrinellsolclick == true || buttonbrinellüstclick == true || buttonbrinellaltclick == true)
            {
                Pen REDPen = new Pen(Color.FromArgb(255, 0, 0), 2);
                Pen REDPen2 = new Pen(Color.FromArgb(255, 0, 0), 1 / 5);
                // e.Graphics.DrawRectangle(REDPen, 10, 10, 100, 50);

                if (ismouseclicktrue == true)
                {
                    int xx = Convert.ToInt32(pikselrecx);
                    int yy = Convert.ToInt32(pikselrecy);
                    int recboyutw = Convert.ToInt32(textBox1.Text);
                    int recboyuth = Convert.ToInt32(textBox2.Text);

                    Graphics g = pictureBox3.CreateGraphics();
                    drawrect(xx, yy, recboyutw, recboyuth, REDPen2, g);
                    //rec1 = new Rectangle(new Point(xx - recboyutw / 2, yy - recboyuth / 2), new Size(recboyutw, recboyuth));
                    //g.DrawRectangle(REDPen2, rec1);
                    //g.DrawLine(REDPen2, new Point(xx - recboyutw / 2, yy), new Point(xx + recboyutw / 2, yy));
                    //g.DrawLine(REDPen2, new Point(xx, yy - recboyuth / 2), new Point(xx, yy + recboyuth / 2));
                    //wait(100);
                }
            }

        }


       

        // KALİBRASYON //--------------------------------------------------------------------------
        private double Katsayibul(Bitmap image) // --> Kalibrasyon katsayısını bulan fonksiyon
        {
            Bitmap resim = new Bitmap(pictureBox3.Image);
            Bitmap griresim = gray(resim);

            double[,] arr1 = new double[image.Width, 1];
            int i, j;

            for (j = 1; j < image.Width; j++)
            {
                arr1[j, 0] = griresim.GetPixel(j, image.Height / 2).R; // 400 10xlik
            }

            ArrayList wtobgecisler = new ArrayList();

            for (j = 0; j < image.Width; j++)
            {
                if (arr1[j, 0] < 150)
                {

                    string b = Convert.ToString(j);
                    wtobgecisler.Add(b);
                    
                    while (arr1[j, 0] < 150) { if (j == image.Width - 1) { break; } j++; }
                }
            }

            double[] whitetoblack = new double[wtobgecisler.Count];

            for (i = 3; i < wtobgecisler.Count; i++)
            {
                whitetoblack[i] = Convert.ToDouble((string)wtobgecisler[i]);
            }

            double tekrarlıkatsayi = new double();
            double[] arrson = new double[wtobgecisler.Count - 3]; /// -5 

            for (i = 3; i < wtobgecisler.Count - 1; i++) /// -2
            {
                arrson[i - 3] = Math.Abs(whitetoblack[i] - whitetoblack[i + 1]);
                // avg = Queryable.Average(arrson.AsQueryable());     
            }

            tekrarlıkatsayi = Tekraredendegerbul(arrson);

            return tekrarlıkatsayi;
        }

        // Kalibrasyon katsayısında kullanılacak en sık rastlanan piksel mesafelerinin ortalamasını alan fonksiyon.
        public double Tekraredendegerbul(double[] array) 
        {
            int i, j;

            for (i = 0; i < array.Length; i++)
            {
                tempkatsayi = array[i];
                tempcount = 0;

                for (j = 0; j < array.Length; j++)
                {
                    if (array[j] == tempkatsayi) { tempcount++; }
                }
                if (tempcount > count)
                {
                    count = tempcount;
                    tekraredenkatsayi1 = tempkatsayi;
                }
            }

            count2 = 0;
            for (i = 0; i < array.Length; i++)
            {
                tempkatsayi2 = array[i];
                tempcount2 = 0;

                for (j = 0; j < array.Length; j++)
                {
                    if (array[j] == tempkatsayi2 && tekraredenkatsayi1 != tempkatsayi2) { tempcount2++; }
                }
                if (tempcount2 > count2)
                {
                    count2 = tempcount2;
                    tekraredenkatsayi2 = tempkatsayi2;
                }
            }
            sonuc = (tekraredenkatsayi1 + tekraredenkatsayi2) / 2;

            count = 0;
            count2 = 0;

            return sonuc;
        }


        // OTO ÖLÇÜM POINT ÇIKARIMI  //--------------------------------------------------------------------
        public void Oto_olcum_sol_köse(Rectangle rectangle, out List<Point> gecisnoktalari, out Bitmap resim)
        {
            resim = new Bitmap(pictureBox3.Image);
            gecisnoktalari = new List<Point>();

            int i, j = new int();

            for (j = rectangle.Y + rectangle.Height; j > rectangle.Y; j--)
            {
                for (i = rectangle.Width + rectangle.X; i > rectangle.X; i--)
                {
                    int a = Convert.ToInt32(resim.GetPixel(i, j).R);
                    int b = Convert.ToInt32(resim.GetPixel(i - 1, j).R);

                    if (a < b - thresholdkontrast)
                    {
                        Point pt = new Point(i - 3, j);
                        gecisnoktalari.Add(pt);
                        resim.SetPixel(i - 3, j, Color.Red);
                        break;
                    }
                }
            }
        } //rec1 in tarama yönlerini belirler.
        public void Oto_olcum_üst_köse(Rectangle rectangle, out List<Point> gecisnoktalari, out Bitmap resim)
        {
            resim = new Bitmap(pictureBox3.Image);
            gecisnoktalari = new List<Point>();

            int i, j = new int();

            for (j = rectangle.X + rectangle.Width; j > rectangle.X; j--)
            {
                for (i = rectangle.Height + rectangle.Y; i > rectangle.Y; i--)
                {
                    int a = Convert.ToInt32(resim.GetPixel(j, i).R);
                    int b = Convert.ToInt32(resim.GetPixel(j, i - 1).R);

                    if (a < b - thresholdkontrast)
                    {
                        Point pt = new Point(j, i - 3);
                        gecisnoktalari.Add(pt);
                        resim.SetPixel(j, i - 3, Color.Red);
                        break;
                    }
                }
            }
        }
        public void Oto_olcum_alt_köse(Rectangle rectangle, out List<Point> gecisnoktalari, out Bitmap resim)
        {
            resim = new Bitmap(pictureBox3.Image);
            gecisnoktalari = new List<Point>();

            int i, j = new int();

            for (j = rectangle.X; j < rectangle.X + rectangle.Width; j++)
            {
                for (i = rectangle.Y; i < rectangle.Height + rectangle.Y; i++)
                {
                    int a = Convert.ToInt32(resim.GetPixel(j, i).R);
                    int b = Convert.ToInt32(resim.GetPixel(j, i + 1).R);

                    if (a < b - thresholdkontrast)
                    {
                        Point pt = new Point(j, i + 3);
                        gecisnoktalari.Add(pt);
                        resim.SetPixel(j, i + 3, Color.Red);
                        break;
                    }
                }
            }
        }
        public void Oto_olcum_sag_köse(Rectangle rectangle, out List<Point> gecisnoktalari, out Bitmap resim)
        {
            resim = new Bitmap(pictureBox3.Image);
            gecisnoktalari = new List<Point>();

            int i, j = new int();

            for (j = rectangle.Y; j < rectangle.Y + rectangle.Height; j++)
            {
                for (i = rectangle.X; i < rectangle.Width + rectangle.X; i++)
                {
                    int a = Convert.ToInt32(resim.GetPixel(i, j).R);
                    int b = Convert.ToInt32(resim.GetPixel(i - 1, j).R);

                    if (b < a - thresholdkontrast)
                    {
                        Point pt = new Point(i+3 , j);
                        gecisnoktalari.Add(pt);
                        resim.SetPixel(i+3 , j, Color.Red);
                        break;
                    }
                }
            }
        }


        // OTO ÖLÇÜM VİCKERS //--------------------------------------------------------------------
        //--> 1. Best Fit doğruyu rectangle ile bulma
        public void BestFitLine2(Rectangle rectangle, out List<Point> bfpoints, out double intercept, out double slope) 
        {
            bfpoints = new List<Point>();
            Bitmap resim = new Bitmap(pictureBox3.Image);
            slope = new double();
            intercept = new double();

            if (button10click == true) // üst butonu açıksa çalışır
            {

                // Oto_olcum_sol_ve_üst_köse(rectangle, out List<Point> gecisnoktalari, out Bitmap resim2);
                Oto_olcum_üst_köse(rectangle, out List<Point> gecisnoktalari, out Bitmap resim2);

                int i, j;

                double xtoplam = new double();
                double ytoplam = new double();

                for (i = 0; i < gecisnoktalari.Count; i++)
                {
                    xtoplam = gecisnoktalari[i].X + xtoplam;
                    ytoplam = gecisnoktalari[i].Y + ytoplam;
                }

                double xavg = new double();
                double yavg = new double();
                double xyavg = new double();
                double xavgsquare = new double();

                xavg = xtoplam / (gecisnoktalari.Count);
                yavg = ytoplam / (gecisnoktalari.Count);  //------------------> x ve y avg değerler
                xyavg = xavg * yavg;
                xavgsquare = Math.Pow(xavg, 2);

                double xiyitop = new double();
                double xisquaretop = new double();

                for (i = 0; i < gecisnoktalari.Count; i++)
                {
                    xiyitop = (gecisnoktalari[i].X * gecisnoktalari[i].Y) + xiyitop;
                    xisquaretop = Math.Pow(gecisnoktalari[i].X, 2) + xisquaretop;
                }


                slope = (xiyitop - (gecisnoktalari.Count * xyavg)) / (xisquaretop - (gecisnoktalari.Count * xavgsquare));
                intercept = yavg - (slope * xavg);

                for (i = 0; i < gecisnoktalari.Count; i++) //--> doğrunun noktalarını buldu
                {
                    int y = new int();
                    int x = new int();
                    x = Convert.ToInt32(gecisnoktalari[i].X);
                    y = Convert.ToInt32(intercept + (slope * gecisnoktalari[i].X));
                    Point pt = new Point(x, y);
                    bfpoints.Add(pt);
                }
            }

            if (button8click == true) // sağ butonu açıksa çalışır
            {
                Oto_olcum_sag_köse(rectangle, out List<Point> gecisnoktalari, out Bitmap resim2);

                int i, j;

                double xtoplam = new double();
                double ytoplam = new double();

                for (i = 0; i < gecisnoktalari.Count; i++)
                {
                    xtoplam = gecisnoktalari[i].X + xtoplam;
                    ytoplam = gecisnoktalari[i].Y + ytoplam;
                }

                double xavg = new double();
                double yavg = new double();
                double xyavg = new double();
                double xavgsquare = new double();

                xavg = xtoplam / (gecisnoktalari.Count);
                yavg = ytoplam / (gecisnoktalari.Count);  //------------------> x ve y avg değerler
                xyavg = xavg * yavg;
                xavgsquare = Math.Pow(xavg, 2);

                double xiyitop = new double();
                double xisquaretop = new double();

                for (i = 0; i < gecisnoktalari.Count; i++)
                {
                    xiyitop = (gecisnoktalari[i].X * gecisnoktalari[i].Y) + xiyitop;
                    xisquaretop = Math.Pow(gecisnoktalari[i].X, 2) + xisquaretop;
                }


                slope = (xiyitop - (gecisnoktalari.Count * xyavg)) / (xisquaretop - (gecisnoktalari.Count * xavgsquare));
                intercept = yavg - (slope * xavg);

                for (i = 0; i < gecisnoktalari.Count; i++) //--> doğrunun noktalarını buldu
                {
                    int y = new int();
                    int x = new int();
                    x = Convert.ToInt32(gecisnoktalari[i].X);
                    y = Convert.ToInt32(intercept + (slope * gecisnoktalari[i].X));
                    Point pt = new Point(x, y);
                    bfpoints.Add(pt);
                }
            }
            if (button9click == true) // alt butonu açıksa çalışır
            {
                Oto_olcum_alt_köse(rectangle, out List<Point> gecisnoktalari, out Bitmap resim2);

                int i, j;

                double xtoplam = new double();
                double ytoplam = new double();

                for (i = 0; i < gecisnoktalari.Count; i++)
                {
                    xtoplam = gecisnoktalari[i].X + xtoplam;
                    ytoplam = gecisnoktalari[i].Y + ytoplam;
                }

                double xavg = new double();
                double yavg = new double();
                double xyavg = new double();
                double xavgsquare = new double();

                xavg = xtoplam / (gecisnoktalari.Count);
                yavg = ytoplam / (gecisnoktalari.Count);  //------------------> x ve y avg değerler
                xyavg = xavg * yavg;
                xavgsquare = Math.Pow(xavg, 2);

                double xiyitop = new double();
                double xisquaretop = new double();

                for (i = 0; i < gecisnoktalari.Count; i++)
                {
                    xiyitop = (gecisnoktalari[i].X * gecisnoktalari[i].Y) + xiyitop;
                    xisquaretop = Math.Pow(gecisnoktalari[i].X, 2) + xisquaretop;
                }


                slope = (xiyitop - (gecisnoktalari.Count * xyavg)) / (xisquaretop - (gecisnoktalari.Count * xavgsquare));
                intercept = yavg - (slope * xavg);

                for (i = 0; i < gecisnoktalari.Count; i++) //--> doğrunun noktalarını buldu
                {
                    int y = new int();
                    int x = new int();
                    x = Convert.ToInt32(gecisnoktalari[i].X);
                    y = Convert.ToInt32(intercept + (slope * gecisnoktalari[i].X));
                    Point pt = new Point(x, y);
                    bfpoints.Add(pt);
                }
            }
            if (button7click == true) // sağ butonu açıksa çalışır
            {
                Oto_olcum_sol_köse(rectangle, out List<Point> gecisnoktalari, out Bitmap resim2);

                int i, j;

                double xtoplam = new double();
                double ytoplam = new double();

                for (i = 0; i < gecisnoktalari.Count; i++)
                {
                    xtoplam = gecisnoktalari[i].X + xtoplam;
                    ytoplam = gecisnoktalari[i].Y + ytoplam;
                }

                double xavg = new double();
                double yavg = new double();
                double xyavg = new double();
                double xavgsquare = new double();

                xavg = xtoplam / (gecisnoktalari.Count);
                yavg = ytoplam / (gecisnoktalari.Count);  //------------------> x ve y avg değerler
                xyavg = xavg * yavg;
                xavgsquare = Math.Pow(xavg, 2);

                double xiyitop = new double();
                double xisquaretop = new double();

                for (i = 0; i < gecisnoktalari.Count; i++)
                {
                    xiyitop = (gecisnoktalari[i].X * gecisnoktalari[i].Y) + xiyitop;
                    xisquaretop = Math.Pow(gecisnoktalari[i].X, 2) + xisquaretop;
                }


                slope = (xiyitop - (gecisnoktalari.Count * xyavg)) / (xisquaretop - (gecisnoktalari.Count * xavgsquare));
                intercept = yavg - (slope * xavg);

                for (i = 0; i < gecisnoktalari.Count; i++) //--> doğrunun noktalarını buldu
                {
                    int y = new int();
                    int x = new int();
                    x = Convert.ToInt32(gecisnoktalari[i].X);
                    y = Convert.ToInt32(intercept + (slope * gecisnoktalari[i].X));
                    Point pt = new Point(x, y);
                    bfpoints.Add(pt);
                }
            }
        }
        public void BestFitLine3(List<Point> eliminatedpoint, out List<Point> bfpoints) //--> eliminasyon sonrası best fit çıkarımı
        {
            bfpoints = new List<Point>();
            Bitmap resim = new Bitmap(pictureBox3.Image);

            int i, j;
            double xtoplam = new double();
            double ytoplam = new double();

            for (i = 0; i < eliminatedpoint.Count; i++)
            {
                xtoplam = eliminatedpoint[i].X + xtoplam;
                ytoplam = eliminatedpoint[i].Y + ytoplam;
            }

            double xavg = new double();
            double yavg = new double();
            double xyavg = new double();
            double xavgsquare = new double();

            xavg = xtoplam / (eliminatedpoint.Count);
            yavg = ytoplam / (eliminatedpoint.Count);  //------------------> x ve y avg değerler
            xyavg = xavg * yavg;
            xavgsquare = Math.Pow(xavg, 2);

            double xiyitop = new double();
            double xisquaretop = new double();

            for (i = 0; i < eliminatedpoint.Count; i++)
            {
                xiyitop = (eliminatedpoint[i].X * eliminatedpoint[i].Y) + xiyitop;
                xisquaretop = Math.Pow(eliminatedpoint[i].X, 2) + xisquaretop;
            }

            double slope = new double();
            double intercept = new double();
            slope = (xiyitop - (eliminatedpoint.Count * xyavg)) / (xisquaretop - (eliminatedpoint.Count * xavgsquare));
            intercept = yavg - (slope * xavg);

            for (i = 0; i < eliminatedpoint.Count; i++) //--> doğrunun noktalarını buldu
            {
                int y = new int();
                int x = new int();
                x = Convert.ToInt32(eliminatedpoint[i].X);
                y = Convert.ToInt32(Math.Abs(intercept + (slope * eliminatedpoint[i].X)));

                Point pt = new Point(x, y);
                bfpoints.Add(pt);
            }

        }
        public void Eliminate(List<Point> points, List<Point> bestlinepoints, out List<Point> drawlist)
        {
            drawlist = new List<Point>();
            int i, j, k = new int();
            double little = new double(); //-------------> i. noktanın doğruya en kısa uzaklığı
            double nextpointdistance = new double();
            int[] threshold = new int[6];

            //?
            threshold[0] = 32;
            threshold[1] = 16;
            threshold[2] = 8;
            threshold[3] = 4;
            threshold[4] = 2;
            threshold[5] = 2;

            List<Point> eliminatedpoints = new List<Point>();
            int iteration = threshold.Length;
            eliminatedpoints.AddRange(points); // pointleri eliminatedpoints listesine ekler
            little = 5000;

            for (k = 1; k <= iteration; k++)
            {
                for (i = 0; i < points.Count; i++)
                {

                    for (j = 0; j < bestlinepoints.Count; j++)
                    {
                        nextpointdistance = Math.Sqrt((Math.Pow(points[i].X - bestlinepoints[j].X, 2)) + (Math.Pow(points[i].Y - bestlinepoints[j].Y, 2)));
                        if (nextpointdistance < little) { little = nextpointdistance; }
                    }

                    if (little > threshold[k - 1])
                    {
                        Point pt = new Point(points[i].X, points[i].Y);
                        eliminatedpoints.Remove(pt);
                    }
                }

                BestFitLine3(eliminatedpoints, out drawlist); //elimine sonrası best fit çıkarımı  
            }
        }
        public void ortanoktabul_sol(List<Point> points, Rectangle rectangle, out Point ortanokta)
        {
            ortanokta = new Point();
            int i, j = new int();
            int maxdistance = new int();

            for (i = 0; i < points.Count; i++)
            {
                if (Math.Abs(points[i].X - (rectangle.Width + rectangle.X)) > maxdistance)
                {
                    maxdistance = Math.Abs(points[i].X - (rectangle.Width + rectangle.X));
                    ortanokta = new Point(points[i].X, points[i].Y);
                }
            }
        }
        public void ortanoktabul_üst(List<Point> points, Rectangle rectangle, out Point ortanokta)
        {
            ortanokta = new Point();
            int i, j = new int();
            int maxdistance = new int();

            for (i = 0; i < points.Count; i++)
            {
                if (Math.Abs(points[i].Y - (rectangle.Height + rectangle.Y)) > maxdistance)
                {
                    maxdistance = Math.Abs(points[i].Y - (rectangle.Height + rectangle.Y));
                    ortanokta = new Point(points[i].X, points[i].Y);
                }
            }
        }
        public void ortanoktabul_sag(List<Point> points, Rectangle rectangle, out Point ortanokta)
        {
            ortanokta = new Point();
            int i, j = new int();
            int mindistance = 5000;

            for (i = 0; i < points.Count; i++)
            {
                if (Math.Abs(points[i].X - (rectangle.Width + rectangle.X)) < mindistance)
                {
                    mindistance = Math.Abs(points[i].X - (rectangle.Width + rectangle.X));
                    ortanokta = new Point(points[i].X, points[i].Y);
                }
            }
        }
        public void ortanoktabul_alt(List<Point> points, Rectangle rectangle, out Point ortanokta)
        {
            ortanokta = new Point();
            int i, j = new int();
            int maxdistance = new int();

            for (i = 0; i < points.Count; i++)
            {
                if (Math.Abs(points[i].Y - rectangle.Y) > maxdistance)
                {
                    maxdistance = Math.Abs(points[i].Y - rectangle.Y);
                    ortanokta = new Point(points[i].X, points[i].Y);
                }
            }
        }
        public void dogrukesisimbul(double intercept1, double intercept2, double slope1, double slope2, out Point kesisim)
        {
            int X, Y = new int();
            X = Convert.ToInt32((intercept2 - intercept1) / (slope1 - slope2));
            Y = Convert.ToInt32(intercept1 + (slope1 * X));
            kesisim = new Point(X, Y);

        }


        // OTO ÖLÇÜM BRINELL //--------------------------------------------------------------------
        public void circlebestfit2(List<Point> points, out int radius, out Point center)
        {
            center = new Point();
            radius = new int();

            int i = new int();
            double xkaretop = new double();
            double ykaretop = new double();
            double xytop = new double();
            double xtop = new double();
            double ytop = new double();
            double n = points.Count;

            double index0 = new double();
            double index1 = new double();
            double index2 = new double();

            for (i = 0; i < points.Count; i++)
            {
                xkaretop = Math.Pow(points[i].X, 2) + xkaretop;
                ykaretop = Math.Pow(points[i].Y, 2) + ykaretop;
                xytop = points[i].X * points[i].Y + xytop;
                xtop = points[i].X + xtop;
                ytop = points[i].Y + ytop;

                index0 =(points[i].X * (Math.Pow(points[i].X, 2) + Math.Pow(points[i].Y, 2))) + index0;
                index1=(points[i].Y * (Math.Pow(points[i].X, 2) + Math.Pow(points[i].Y, 2))) + index1;
                index2 = (Math.Pow(points[i].X, 2) + Math.Pow(points[i].Y, 2)) + index2;
            }

            double[][] matris1 = new double[3][];
            matris1[0] = new double[3];
            matris1[1] = new double[3];
            matris1[2] = new double[3];

            matris1[0][0] = xkaretop;
            matris1[0][1] = xytop;
            matris1[0][2] = xtop;
            matris1[1][0] = xytop;
            matris1[1][1] = ykaretop;
            matris1[1][2] = ytop;
            matris1[2][0] = xtop;
            matris1[2][1] = ytop;
            matris1[2][2] = n;

            double[][] matrisinverse = new double[3][];
            matrisinverse[0] = new double[3];
            matrisinverse[1] = new double[3];
            matrisinverse[2] = new double[3];
            matrisinverse= MatrixInverse(matris1);

            double A, B, C = new double();

            A= (matrisinverse[0][0] * index0) + (matrisinverse[1][0] * index1) + (matrisinverse[2][0] * index2);
            B= (matrisinverse[0][1] * index0) + (matrisinverse[1][1] * index1) + (matrisinverse[2][1] * index2);
            C= (matrisinverse[0][2] * index0) + (matrisinverse[1][2] * index1) + (matrisinverse[2][2] * index2);

            
            center.X =Convert.ToInt32(A / 2);
            center.Y= Convert.ToInt32(B / 2);

            
            radius =Convert.ToInt32( Math.Sqrt((4 * C) + (A * A) + (B * B)) / 2);

        }
        //public void circlebestfit(List<Point> points, Rectangle rectangle, out List<Point> bfcirclepoints, out double Radius, out Point drawpt1, out Point drawpt2)
        //{
        //    bfcirclepoints = new List<Point>();
        //    double Xtop = new double();
        //    double Ytop = new double();

        //    double Su = new double();
        //    double Sv = new double();

        //    double Suu = new double();
        //    double Svv = new double();
        //    double Suv = new double();

        //    double Suuu = new double();
        //    double Svvv = new double();
        //    double Suvv = new double();
        //    double Svuu = new double();

        //    double u = new double();
        //    double v = new double();

        //    Point m = new Point();
        //    drawpt1 = new Point();
        //    drawpt2 = new Point();

        //    int i, j = new int();

        //    for (i = 0; i < points.Count; i++)
        //    {
        //        Xtop = points[i].X + Xtop;
        //        Ytop = points[i].Y + Ytop;
        //    }

        //    double Xavg = Xtop / points.Count;
        //    double Yavg = Ytop / points.Count;

        //    for (i = 0; i < points.Count; i++)
        //    {
        //        Su = points[i].X + Su;
        //        Sv = points[i].Y + Sv;

        //        Suu = (Math.Pow(points[i].X, 2)) + Suu;
        //        Svv = (Math.Pow(points[i].Y, 2)) + Svv;
        //        Suv = (points[i].X * points[i].Y) + Suv;

        //        Suuu = (Math.Pow(points[i].X, 3)) + Suuu;
        //        Svvv = (Math.Pow(points[i].Y, 3)) + Svvv;
        //        Suvv = (Math.Pow(points[i].Y, 2) * points[i].X) + Suvv;
        //        Svuu = (Math.Pow(points[i].X, 2) * points[i].Y) + Svuu;
        //    }

        //    double e = (Suuu + Suvv) / 2;
        //    double f = (Svvv + Svuu) / 2;

        //    u = ((Suv * f) - (Svv * e)) / ((Suv * Suv) - (Svv * Suu));
        //    v = (e - (Suu * u)) / Suv;
        //    Radius = Math.Sqrt(Math.Pow(u, 2) + Math.Pow(v, 2) + (Suu + Svv) / points.Count);

        //    m.X = Convert.ToInt32(Xavg + u);
        //    m.Y = Convert.ToInt32(Yavg + v);

        //    drawpt1.X = Convert.ToInt32((Xavg + u) - Radius);
        //    drawpt1.Y = Convert.ToInt32((Yavg + v) - Radius);

        //    drawpt2.X = Convert.ToInt32((Xavg + u) + Radius);
        //    drawpt2.Y = Convert.ToInt32((Yavg + v) + Radius);

        //}









        // ---------------------------------------------------------------------------------------- tERS
        static double[][] MatrixDuplicate(double[][] matrix)
        {
            // allocates/creates a duplicate of a matrix.
            double[][] result = MatrixCreate(matrix.Length, matrix[0].Length);
            for (int i = 0; i < matrix.Length; ++i) // copy the values
                for (int j = 0; j < matrix[i].Length; ++j)
                    result[i][j] = matrix[i][j];
            return result;
        }
        static double[][] MatrixCreate(int rows, int cols)
        {
            double[][] result = new double[rows][];
            for (int i = 0; i < rows; ++i)
                result[i] = new double[cols];
            return result;
        }
        static double[][] MatrixDecompose(double[][] matrix, out int[] perm, out int toggle)
        {
            // Doolittle LUP decomposition with partial pivoting.
            // rerturns: result is L (with 1s on diagonal) and U;
            // perm holds row permutations; toggle is +1 or -1 (even or odd)
            int rows = matrix.Length;
            int cols = matrix[0].Length; // assume square
            if (rows != cols)
                throw new Exception("Attempt to decompose a non-square m");

            int n = rows; // convenience

            double[][] result = MatrixDuplicate(matrix);

            perm = new int[n]; // set up row permutation result
            for (int i = 0; i < n; ++i) { perm[i] = i; }

            toggle = 1; // toggle tracks row swaps.
                        // +1 -greater-than even, -1 -greater-than odd. used by MatrixDeterminant

            for (int j = 0; j < n - 1; ++j) // each column
            {
                double colMax = Math.Abs(result[j][j]); // find largest val in col
                int pRow = j;
                //for (int i = j + 1; i less-than n; ++i)
                //{
                //  if (result[i][j] greater-than colMax)
                //  {
                //    colMax = result[i][j];
                //    pRow = i;
                //  }
                //}

                // reader Matt V needed this:
                for (int i = j + 1; i < n; ++i)
                {
                    if (Math.Abs(result[i][j]) > colMax)
                    {
                        colMax = Math.Abs(result[i][j]);
                        pRow = i;
                    }
                }
                // Not sure if this approach is needed always, or not.

                if (pRow != j) // if largest value not on pivot, swap rows
                {
                    double[] rowPtr = result[pRow];
                    result[pRow] = result[j];
                    result[j] = rowPtr;

                    int tmp = perm[pRow]; // and swap perm info
                    perm[pRow] = perm[j];
                    perm[j] = tmp;

                    toggle = -toggle; // adjust the row-swap toggle
                }

                // --------------------------------------------------
                // This part added later (not in original)
                // and replaces the 'return null' below.
                // if there is a 0 on the diagonal, find a good row
                // from i = j+1 down that doesn't have
                // a 0 in column j, and swap that good row with row j
                // --------------------------------------------------

                if (result[j][j] == 0.0)
                {
                    // find a good row to swap
                    int goodRow = -1;
                    for (int row = j + 1; row < n; ++row)
                    {
                        if (result[row][j] != 0.0)
                            goodRow = row;
                    }

                    if (goodRow == -1)
                        throw new Exception("Cannot use Doolittle's method");

                    // swap rows so 0.0 no longer on diagonal
                    double[] rowPtr = result[goodRow];
                    result[goodRow] = result[j];
                    result[j] = rowPtr;

                    int tmp = perm[goodRow]; // and swap perm info
                    perm[goodRow] = perm[j];
                    perm[j] = tmp;

                    toggle = -toggle; // adjust the row-swap toggle
                }
                // --------------------------------------------------
                // if diagonal after swap is zero . .
                //if (Math.Abs(result[j][j]) less-than 1.0E-20) 
                //  return null; // consider a throw

                for (int i = j + 1; i < n; ++i)
                {
                    result[i][j] /= result[j][j];
                    for (int k = j + 1; k < n; ++k)
                    {
                        result[i][k] -= result[i][j] * result[j][k];
                    }
                }


            } // main j column loop

            return result;
        }
        static double[] HelperSolve(double[][] luMatrix, double[] b)
        {
            // before calling this helper, permute b using the perm array
            // from MatrixDecompose that generated luMatrix
            int n = luMatrix.Length;
            double[] x = new double[n];
            b.CopyTo(x, 0);

            for (int i = 1; i < n; ++i)
            {
                double sum = x[i];
                for (int j = 0; j < i; ++j)
                    sum -= luMatrix[i][j] * x[j];
                x[i] = sum;
            }

            x[n - 1] /= luMatrix[n - 1][n - 1];
            for (int i = n - 2; i >= 0; --i)
            {
                double sum = x[i];
                for (int j = i + 1; j < n; ++j)
                    sum -= luMatrix[i][j] * x[j];
                x[i] = sum / luMatrix[i][i];
            }

            return x;
        }
        static double[][] MatrixInverse(double[][] matrix)
        {
            int n = matrix.Length;
            double[][] result = MatrixDuplicate(matrix);

            int[] perm;
            int toggle;
            double[][] lum = MatrixDecompose(matrix, out perm,
              out toggle);
            if (lum == null)
                throw new Exception("Unable to compute inverse");

            double[] b = new double[n];
            for (int i = 0; i < n; ++i)
            {
                for (int j = 0; j < n; ++j)
                {
                    if (i == perm[j])
                        b[j] = 1.0;
                    else
                        b[j] = 0.0;
                }

                double[] x = HelperSolve(lum, b);

                for (int j = 0; j < n; ++j)
                    result[j][i] = x[j];
            }
            return result;
        }
        private void getCircle(Graphics drawingArea, Pen penToUse, Point center, int radius)
        {
            Rectangle rect = new Rectangle(center.X - radius, center.Y - radius, radius * 2, radius * 2);
            drawingArea.DrawEllipse(penToUse, rect);
        }

        //EK METHODLAR //--------------------------------------------------------------------------
        private Bitmap gray(Bitmap image) //--> Resmi S/B Dönüştüren Fonksiyon
        {
            scale = 1; // Resmin ölçeği piksel kaybı olmasın diye 1
            this.pictureBox3.Size = new System.Drawing.Size(degerw / scale, degerh / scale); // pb3' ün boyutu
            for (int i = 0; i < image.Height - 1; i++)
            {
                for (int j = 0; j < image.Width - 1; j++)
                {
                    int ortalama = (image.GetPixel(j, i).R + image.GetPixel(j, i).G + image.GetPixel(j, i).B) / 3;
                    Color gri;
                    gri = Color.FromArgb(ortalama, ortalama, ortalama);
                    image.SetPixel(j, i, gri);

                }
            }
            return image;
        }
        private Bitmap edgedetection(Bitmap image) //---> Sobel Edge Detection 
        {
            scale = 1; // Resmin ölçeği piksel kaybı olmasın diye 1
                       // this.pictureBox3.Size = new System.Drawing.Size(degerw / scale, degerh / scale); // pb3' ün boyutu
            Bitmap resim = new Bitmap(pictureBox3.Image);
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
                    if (i == 0 || j == 0 || i == image.Height - 1 || j == image.Width - 1)
                    {
                        dikey = 0;
                        yatay = 0;
                    }
                    else
                    {
                        yatay = griresim.GetPixel(j - 1, i - 1).R * X[0, 0] +
                        griresim.GetPixel(j, i - 1).R * X[0, 1] +
                        griresim.GetPixel(j + 1, i - 1).R * X[0, 2] +
                        griresim.GetPixel(j - 1, i).R * X[1, 0] +
                        griresim.GetPixel(j, i).R * X[1, 1] +
                        griresim.GetPixel(j + 1, i).R * X[1, 2] +
                        griresim.GetPixel(j - 1, i + 1).R * X[2, 0] +
                        griresim.GetPixel(j, i + 1).R * X[2, 1] +
                        griresim.GetPixel(j + 1, i + 1).R * X[2, 2];

                        dikey = griresim.GetPixel(j - 1, i - 1).R * Y[0, 0] +
                        griresim.GetPixel(j, i - 1).R * Y[0, 1] +
                        griresim.GetPixel(j + 1, i - 1).R * Y[0, 2] +
                        griresim.GetPixel(j - 1, i).R * Y[1, 0] +
                        griresim.GetPixel(j, i).R * Y[1, 1] +
                        griresim.GetPixel(j + 1, i).R * Y[1, 2] +
                        griresim.GetPixel(j - 1, i + 1).R * Y[2, 0] +
                        griresim.GetPixel(j, i + 1).R * Y[2, 1] +
                        griresim.GetPixel(j + 1, i + 1).R * Y[2, 2];

                        int gradient;
                        gradient = (int)(Math.Abs(yatay) + Math.Abs(dikey));

                        if (gradient < 0) { gradient = 0; }
                        if (gradient > 255) { gradient = 255; }

                        Color renk;

                        renk = Color.FromArgb(gradient, gradient, gradient);
                        image.SetPixel(j, i, renk);
                    }
                }
            }
            return image;
        }
        public void wait(int milliseconds) //--> Wait fonksiyonu.
        {
            var timer1 = new System.Windows.Forms.Timer();
            if (milliseconds == 0 || milliseconds < 0) return;

            // Console.WriteLine("start wait timer");
            timer1.Interval = milliseconds;
            timer1.Enabled = true;
            timer1.Start();

            timer1.Tick += (s, e) =>
            {
                timer1.Enabled = false;
                timer1.Stop();
                // Console.WriteLine("stop wait timer");
            };

            while (timer1.Enabled)
            {
                Application.DoEvents();
            }
        }
        public void drawcross(Point p, Pen pen)
        {
            Graphics g = pictureBox3.CreateGraphics();


            Point pt1 = new Point(p.X, p.Y - 10);
            Point pt2 = new Point(p.X, p.Y + 10);
            Point pt3 = new Point(p.X - 10, p.Y);
            Point pt4 = new Point(p.X + 10, p.Y);
            Point pt5 = new Point(p.X + 2, p.Y);
            Point pt6 = new Point(p.X - 2, p.Y);
            Point pt7 = new Point(p.X, p.Y + 2);
            Point pt8 = new Point(p.X, p.Y - 2);

            g.DrawLine(pen, pt8, pt1);
            g.DrawLine(pen, pt7, pt2);
            g.DrawLine(pen, pt6, pt3);
            g.DrawLine(pen, pt5, pt4);

        }
        private void drawrect(int xx, int yy, int recboyutw, int recboyuth, Pen pen, Graphics g)
        {
            rec1 = new Rectangle(new Point(xx - recboyutw / 2, yy - recboyuth / 2), new Size(recboyutw, recboyuth));
            g.DrawRectangle(pen, rec1);
            g.DrawLine(pen, new Point(xx - recboyutw / 2, yy), new Point(xx + recboyutw / 2, yy));
            g.DrawLine(pen, new Point(xx, yy - recboyuth / 2), new Point(xx, yy + recboyuth / 2));
            wait(100);
        }
        public void kosegen_capbul(out double horizantalv, out double verticalv, out double horizantalb, out double verticalb, out double absv, out double absb)
        {
            horizantalv = new double();
            verticalv = new double();
            absv = new double();

            horizantalv = Math.Abs(kesisimsol.X - kesisimsag.X) / atanankatsayi;
            verticalv = Math.Abs(kesisimust.Y - kesisimalt.Y) / atanankatsayi;
            absv = (horizantalv + verticalv) / 2;

            horizantalb = new double();
            verticalb = new double();
            absb = new double();

            horizantalb = Math.Abs(brinellsag.X - brinellsol.X) / atanankatsayi;
            verticalb = Math.Abs(brinellust.Y - brinellalt.Y) / atanankatsayi;
            absb = (horizantalb + verticalb) / 2;
        }
        public static double[,] GaussianBlur(int lenght, double weight)
        {
            double[,] kernel = new double[lenght, lenght];
            double kernelSum = 0;
            int foff = (lenght - 1) / 2;
            double distance = 0;
            double constant = 1d / (2 * Math.PI * weight * weight);
            for (int y = -foff; y <= foff; y++)
            {
                for (int x = -foff; x <= foff; x++)
                {
                    distance = ((y * y) + (x * x)) / (2 * weight * weight);
                    kernel[y + foff, x + foff] = constant * Math.Exp(-distance);
                    kernelSum += kernel[y + foff, x + foff];
                }
            }
            for (int y = 0; y < lenght; y++)
            {
                for (int x = 0; x < lenght; x++)
                {
                    kernel[y, x] = kernel[y, x] * 1d / kernelSum;
                }
            }
            return kernel;
        } //----------------> Gausssian  kernel
        public static Bitmap Convolve(Bitmap srcImage, double[,] kernel)
        {
            int width = srcImage.Width;
            int height = srcImage.Height;
            BitmapData srcData = srcImage.LockBits(new Rectangle(0, 0, width, height),
                ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            int bytes = srcData.Stride * srcData.Height;
            byte[] buffer = new byte[bytes];
            byte[] result = new byte[bytes];
            Marshal.Copy(srcData.Scan0, buffer, 0, bytes);
            srcImage.UnlockBits(srcData);
            int colorChannels = 3;
            double[] rgb = new double[colorChannels];
            int foff = (kernel.GetLength(0) - 1) / 2;
            int kcenter = 0;
            int kpixel = 0;
            for (int y = foff; y < height - foff; y++)
            {
                for (int x = foff; x < width - foff; x++)
                {
                    for (int c = 0; c < colorChannels; c++)
                    {
                        rgb[c] = 0.0;
                    }
                    kcenter = y * srcData.Stride + x * 4;
                    for (int fy = -foff; fy <= foff; fy++)
                    {
                        for (int fx = -foff; fx <= foff; fx++)
                        {
                            kpixel = kcenter + fy * srcData.Stride + fx * 4;
                            for (int c = 0; c < colorChannels; c++)
                            {
                                rgb[c] += (double)(buffer[kpixel + c]) * kernel[fy + foff, fx + foff];
                            }
                        }
                    }
                    for (int c = 0; c < colorChannels; c++)
                    {
                        if (rgb[c] > 255)
                        {
                            rgb[c] = 255;
                        }
                        else if (rgb[c] < 0)
                        {
                            rgb[c] = 0;
                        }
                    }
                    for (int c = 0; c < colorChannels; c++)
                    {
                        result[kcenter + c] = (byte)rgb[c];
                    }
                    result[kcenter + 3] = 255;
                }
            }
            Bitmap resultImage = new Bitmap(width, height);
            BitmapData resultData = resultImage.LockBits(new Rectangle(0, 0, width, height),
                ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            Marshal.Copy(result, 0, resultData.Scan0, bytes);
            resultImage.UnlockBits(resultData);
            return resultImage;
        } //----------------> Gausssian func.

        // ----------------------------------------------------------------------------------------
























        //---------------------------------------------------------------------------------------------------------------------------------------------//
        Bitmap gaussianimage;
        private void button12_Click(object sender, EventArgs e)
        {
            Bitmap resim = new Bitmap(pictureBox3.Image);
            double[,] kernel = GaussianBlur(3, 3);
            gaussianimage = Convolve(resim, kernel);
            pictureBox3.Image = gaussianimage;
        }
        Bitmap resim;
        private void button13_Click(object sender, EventArgs e)
        {
            Bitmap resim = new Bitmap(pictureBox3.Image);
            resim = edgedetection(resim);
            pictureBox3.Image = resim;

        }
        private void button14_Click(object sender, EventArgs e)
        {
            //HoughLineTransformation lineTransform = new HoughLineTransformation();
            //// apply Hough line transofrm
            //lineTransform.ProcessImage(resim);
            //Bitmap houghLineImage = lineTransform.ToBitmap();
            //// get lines using relative intensity
            //HoughLine[] lines = lineTransform.GetLinesByRelativeIntensity(0.5);

            //foreach (HoughLine line in lines)
            //{
            //    // get line's radius and theta values
            //    int r = line.Radius;
            //    double t = line.Theta;

            //    // check if line is in lower part of the image
            //    if (r < 0)
            //    {
            //        t += 180;
            //        r = -r;
            //    }

            //    // convert degrees to radians
            //    t = (t / 180) * Math.PI;

            //    // get image centers (all coordinate are measured relative
            //    // to center)
            //    int w2 = resim.Width / 2;
            //    int h2 = resim.Height / 2;

            //    double x0 = 0, x1 = 0, y0 = 0, y1 = 0;

            //    if (line.Theta != 0)
            //    {
            //        // none-vertical line
            //        x0 = -w2; // most left point
            //        x1 = w2;  // most right point

            //        // calculate corresponding y values
            //        y0 = (-Math.Cos(t) * x0 + r) / Math.Sin(t);
            //        y1 = (-Math.Cos(t) * x1 + r) / Math.Sin(t);
            //    }
            //    else
            //    {
            //        // vertical line
            //        x0 = line.Radius;
            //        x1 = line.Radius;

            //        y0 = h2;
            //        y1 = -h2;
            //    }

            //    // draw line on the image
            //    Drawing.Line(resim,
            //        new IntPoint((int)x0 + w2, h2 - (int)y0),
            //        new IntPoint((int)x1 + w2, h2 - (int)y1),
            //        Color.Red);
            //}
            //}

            //}
        }
      
        public void Rec_bolme_solkenar(Rectangle rectangle, out Point ortanokta)
        {
            ortanokta = new Point();
            resim = new Bitmap(pictureBox3.Image);

            int i, j, k = new int();
            bool ortanoktabulundu = new bool();

            for (i = rectangle.X; i < rectangle.Width + rectangle.X; i++) // satır
            {
                for (j = rectangle.Y; j < rectangle.Y + rectangle.Height; j++) // sütun
                {
                    int a = Convert.ToInt32(resim.GetPixel(i, j - 1).R); // yukarı 
                    int b = Convert.ToInt32(resim.GetPixel(i, j).R); // aşağı 

                    if (b < a - 20)
                    {
                        for (k = i; k < rectangle.Width + rectangle.X; k++)
                        {
                            int c = Convert.ToInt32(resim.GetPixel(k - 1, j).R);
                            int d = Convert.ToInt32(resim.GetPixel(k, j).R);
                            if (d < c - 5)
                            {
                                break;
                            }
                            else
                            {
                                ortanoktabulundu = true;
                                ortanokta = new Point(k, j);
                                resim.SetPixel(k, j, Color.Blue);
                                break;
                            }
                        }
                    }
                    if (ortanoktabulundu == true) { break; }
                }
                if (ortanoktabulundu == true) { break; }
            }

        }
        public void Rec_bolme_sagkenar(Rectangle rectangle, out Point ortanokta)
        {
            ortanokta = new Point();
            resim = new Bitmap(pictureBox3.Image);

            int i, j, k = new int();
            bool ortanoktabulundu = new bool();

            for (i = rectangle.Width + rectangle.X; i > rectangle.X; i--) // satır
            {
                for (j = rectangle.Y; j < rectangle.Y + rectangle.Height; j++) // sütun
                {
                    int a = Convert.ToInt32(resim.GetPixel(i, j - 1).R); // yukarı 
                    int b = Convert.ToInt32(resim.GetPixel(i, j).R); // aşağı 

                    if (b < a - 5)
                    {
                        for (k = i; k > rectangle.Width; k--)
                        {

                            int c = Convert.ToInt32(resim.GetPixel(k + 1, j).R);
                            int d = Convert.ToInt32(resim.GetPixel(k, j).R);
                            if (d < c - 5)
                            {
                                break;
                            }
                            else
                            {
                                ortanoktabulundu = true;
                                ortanokta = new Point(i, j);
                                resim.SetPixel(i, j, Color.Blue);
                                break;
                            }
                        }
                    }
                    if (ortanoktabulundu == true) { break; }
                }
                if (ortanoktabulundu == true) { break; }
            }

        }

        private void button20_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.ıcImagingControl1.LiveStart();
            form2.Show();
        }

       
    }
}

//namespace matrix
//{

//    class Program
//    {

//        static void Main(string[] args)
//        {

//            double[][] m = new double[][] { new double[] { 7, 2, 1 }, new double[] { 0, 3, -1 }, new double[] { -3, 4, 2 } };
//            double[][] inv = MatrixInverse(m);


//            //printing the inverse
//            for (int i = 0; i < 3; i++)
//            {
//                for (int j = 0; j < 3; j++)
//                    Console.Write(Math.Round(inv[i][j], 1).ToString().PadLeft(5, ' ') + "|");
//                Console.WriteLine();
//            }

//        }

//        static double[][] MatrixCreate(int rows, int cols)
//        {
//            double[][] result = new double[rows][];
//            for (int i = 0; i < rows; ++i)
//                result[i] = new double[cols];
//            return result;
//        }

//        static double[][] MatrixIdentity(int n)
//        {
//            // return an n x n Identity matrix
//            double[][] result = MatrixCreate(n, n);
//            for (int i = 0; i < n; ++i)
//                result[i][i] = 1.0;

//            return result;
//        }

//        static double[][] MatrixProduct(double[][] matrixA, double[][] matrixB)
//        {
//            int aRows = matrixA.Length; int aCols = matrixA[0].Length;
//            int bRows = matrixB.Length; int bCols = matrixB[0].Length;
//            if (aCols != bRows)
//                throw new Exception("Non-conformable matrices in MatrixProduct");

//            double[][] result = MatrixCreate(aRows, bCols);

//            for (int i = 0; i < aRows; ++i) // each row of A
//                for (int j = 0; j < bCols; ++j) // each col of B
//                    for (int k = 0; k < aCols; ++k) // could use k less-than bRows
//                        result[i][j] += matrixA[i][k] * matrixB[k][j];

//            return result;
//        }

//        static double[][] MatrixInverse(double[][] matrix)
//        {
//            int n = matrix.Length;
//            double[][] result = MatrixDuplicate(matrix);

//            int[] perm;
//            int toggle;
//            double[][] lum = MatrixDecompose(matrix, out perm,
//              out toggle);
//            if (lum == null)
//                throw new Exception("Unable to compute inverse");

//            double[] b = new double[n];
//            for (int i = 0; i < n; ++i)
//            {
//                for (int j = 0; j < n; ++j)
//                {
//                    if (i == perm[j])
//                        b[j] = 1.0;
//                    else
//                        b[j] = 0.0;
//                }

//                double[] x = HelperSolve(lum, b);

//                for (int j = 0; j < n; ++j)
//                    result[j][i] = x[j];
//            }
//            return result;
//        }

//        static double[][] MatrixDuplicate(double[][] matrix)
//        {
//            // allocates/creates a duplicate of a matrix.
//            double[][] result = MatrixCreate(matrix.Length, matrix[0].Length);
//            for (int i = 0; i < matrix.Length; ++i) // copy the values
//                for (int j = 0; j < matrix[i].Length; ++j)
//                    result[i][j] = matrix[i][j];
//            return result;
//        }

//        static double[] HelperSolve(double[][] luMatrix, double[] b)
//        {
//            // before calling this helper, permute b using the perm array
//            // from MatrixDecompose that generated luMatrix
//            int n = luMatrix.Length;
//            double[] x = new double[n];
//            b.CopyTo(x, 0);

//            for (int i = 1; i < n; ++i)
//            {
//                double sum = x[i];
//                for (int j = 0; j < i; ++j)
//                    sum -= luMatrix[i][j] * x[j];
//                x[i] = sum;
//            }

//            x[n - 1] /= luMatrix[n - 1][n - 1];
//            for (int i = n - 2; i >= 0; --i)
//            {
//                double sum = x[i];
//                for (int j = i + 1; j < n; ++j)
//                    sum -= luMatrix[i][j] * x[j];
//                x[i] = sum / luMatrix[i][i];
//            }

//            return x;
//        }

//        static double[][] MatrixDecompose(double[][] matrix, out int[] perm, out int toggle)
//        {
//            // Doolittle LUP decomposition with partial pivoting.
//            // rerturns: result is L (with 1s on diagonal) and U;
//            // perm holds row permutations; toggle is +1 or -1 (even or odd)
//            int rows = matrix.Length;
//            int cols = matrix[0].Length; // assume square
//            if (rows != cols)
//                throw new Exception("Attempt to decompose a non-square m");

//            int n = rows; // convenience

//            double[][] result = MatrixDuplicate(matrix);

//            perm = new int[n]; // set up row permutation result
//            for (int i = 0; i < n; ++i) { perm[i] = i; }

//            toggle = 1; // toggle tracks row swaps.
//                        // +1 -greater-than even, -1 -greater-than odd. used by MatrixDeterminant

//            for (int j = 0; j < n - 1; ++j) // each column
//            {
//                double colMax = Math.Abs(result[j][j]); // find largest val in col
//                int pRow = j;
//                //for (int i = j + 1; i less-than n; ++i)
//                //{
//                //  if (result[i][j] greater-than colMax)
//                //  {
//                //    colMax = result[i][j];
//                //    pRow = i;
//                //  }
//                //}

//                // reader Matt V needed this:
//                for (int i = j + 1; i < n; ++i)
//                {
//                    if (Math.Abs(result[i][j]) > colMax)
//                    {
//                        colMax = Math.Abs(result[i][j]);
//                        pRow = i;
//                    }
//                }
//                // Not sure if this approach is needed always, or not.

//                if (pRow != j) // if largest value not on pivot, swap rows
//                {
//                    double[] rowPtr = result[pRow];
//                    result[pRow] = result[j];
//                    result[j] = rowPtr;

//                    int tmp = perm[pRow]; // and swap perm info
//                    perm[pRow] = perm[j];
//                    perm[j] = tmp;

//                    toggle = -toggle; // adjust the row-swap toggle
//                }

//                // --------------------------------------------------
//                // This part added later (not in original)
//                // and replaces the 'return null' below.
//                // if there is a 0 on the diagonal, find a good row
//                // from i = j+1 down that doesn't have
//                // a 0 in column j, and swap that good row with row j
//                // --------------------------------------------------

//                if (result[j][j] == 0.0)
//                {
//                    // find a good row to swap
//                    int goodRow = -1;
//                    for (int row = j + 1; row < n; ++row)
//                    {
//                        if (result[row][j] != 0.0)
//                            goodRow = row;
//                    }

//                    if (goodRow == -1)
//                        throw new Exception("Cannot use Doolittle's method");

//                    // swap rows so 0.0 no longer on diagonal
//                    double[] rowPtr = result[goodRow];
//                    result[goodRow] = result[j];
//                    result[j] = rowPtr;

//                    int tmp = perm[goodRow]; // and swap perm info
//                    perm[goodRow] = perm[j];
//                    perm[j] = tmp;

//                    toggle = -toggle; // adjust the row-swap toggle
//                }
//                // --------------------------------------------------
//                // if diagonal after swap is zero . .
//                //if (Math.Abs(result[j][j]) less-than 1.0E-20) 
//                //  return null; // consider a throw

//                for (int i = j + 1; i < n; ++i)
//                {
//                    result[i][j] /= result[j][j];
//                    for (int k = j + 1; k < n; ++k)
//                    {
//                        result[i][k] -= result[i][j] * result[j][k];
//                    }
//                }


//            } // main j column loop

//            return result;
//        }




//    }
//}


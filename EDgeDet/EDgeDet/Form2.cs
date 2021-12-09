using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TIS.Imaging;

namespace EDgeDet
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        Form1 form1 = new Form1();
        public new Bitmap bmp;
        
        private void Form2_Load(object sender, EventArgs e)
        {
            ıcImagingControl1.LiveStart();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {

            try
            {
                // Use a timeout of 1000ms.
                ıcImagingControl1.MemorySnapImage(1000);
                bmp = ıcImagingControl1.ImageActiveBuffer.Bitmap;
                // Save the snapped image, which is stored in the ImageBuffer "ImageActiveBuffer" to a file.
                ıcImagingControl1.ImageActiveBuffer.SaveAsBitmap("test.bmp");
            }
            catch (ICException Ex)
            {
                MessageBox.Show(Ex.Message, "MemorySnapImage Error");
            }
            ıcImagingControl1.LiveStop();
   

        }

        //private void button2_Click(object sender, EventArgs e)
        //{
        //    form1.pictureBox1.Image = bmp;
        //}
    }
}

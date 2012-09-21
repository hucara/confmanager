using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Configuration_Manager
{
    public partial class SplashScreen : Form
    {
        Bitmap bm;
        const int lowShineWidth = 20;
        const int highShineWidth = 5;
        public int progress;

        public SplashScreen()
        {
            InitializeComponent();
            bm = (Bitmap)pictureBox1.Image;
            this.SendToBack();
            this.Opacity = .00;
        }

        public void SetBarPercentage(int p)
        {
            if (p > 0 && p < 101)
                this.progressBar1.Value = p;
        }

        public void IncreasePercentage(int p)
        {
            int t = this.progressBar1.Value + p;
            if (t > 0 && t < 101)
                this.progressBar1.Value += p;

            this.progressBar1.Update();
        }

        public void Show(String title, String version)
        {
            // Worst Splash Screen ever.
            this.toolNameLabel.Text = title;
            this.versionLabel.Text = version;

            base.Show();

            //GetBitmapRectangle();
            for (double i = .00 ; i < 1.00; i += 0.08)
            {
                this.Opacity = i;
                
                if(this.progressBar1.Value + 8 < 100) this.progressBar1.Value += 8;
                System.Threading.Thread.Sleep(80);

                this.Update();
            }

            this.progressBar1.Value = this.progressBar1.Maximum;
            System.Threading.Thread.Sleep(10000);
        }

        private void GetBitmapRectangle()
        {
            // Define format, manipulation mode and area to modify
            System.Drawing.Imaging.PixelFormat pxf = System.Drawing.Imaging.PixelFormat.Format24bppRgb;
            System.Drawing.Imaging.ImageLockMode ilm = System.Drawing.Imaging.ImageLockMode.ReadWrite;
            Rectangle r = new Rectangle(0, 0, bm.Width, bm.Height);

            // Lock the bitmap bits
            System.Drawing.Imaging.BitmapData bmData = this.bm.LockBits(r, ilm, pxf);

            // Get the address of the first line
            IntPtr ptr = bmData.Scan0;
            
            // Declare an array to hold the bytes of the bitmap
            int numBytes = bm.Width * bm.Height * 3;
            byte[] rgbValues = new byte[numBytes];

            // Copy the RGB values into the array.
            System.Runtime.InteropServices.Marshal.Copy(ptr, rgbValues, 0, numBytes);

            // Manipulate the bitmap, sucha as changing the
            // blue value for every other pixel in the bitmap.
            for (int counter = 0; counter < rgbValues.Length; counter += 6)
                rgbValues[counter] = 200;

            // Copy the RGB values back to the bitmap
            System.Runtime.InteropServices.Marshal.Copy(rgbValues, 0, ptr, numBytes);

            // Unlock the bits
            bm.UnlockBits(bmData);
        }
    }
}
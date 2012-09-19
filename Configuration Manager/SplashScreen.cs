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
        }

        public void SetBarPercentage(int p)
        {
            if (p > 0 && p < 101)
            {
                this.progressBar1.Value = p;
            }
        }

        public void IncreasePercentage(int p)
        {
            int t = this.progressBar1.Value + p;
            if (t > 0 && t < 101)
            {
                this.progressBar1.Value += p;
            }

            this.progressBar1.Update();
        }

        public void Show(String title, String version)
        {
            // Worst Splash Screen ever.

            this.toolNameLabel.Text = title;
            this.versionLabel.Text = version;

            this.Opacity = .00;
            base.Show();
            for (double i = .00 ; i < 1.00; i += 0.08)
            {
                this.Opacity = i;
                if(this.progressBar1.Value + 8 < 100) this.progressBar1.Value += 8;
                System.Threading.Thread.Sleep(80);
                this.Update();
            }
            this.progressBar1.Value = this.progressBar1.Maximum;
            this.Update();
            System.Threading.Thread.Sleep(10000);
        }
    }
}

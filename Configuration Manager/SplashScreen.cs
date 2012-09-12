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
        int xForProgress;
        const int lowShineWidth = 20;
        const int highShineWidth = 5;

        public SplashScreen()
        {
            InitializeComponent();
            bm = (Bitmap)pictureBox1.Image;
            xForProgress = bm.Width / 100;
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
            System.Diagnostics.Debug.WriteLine("Reading UI..."+e.ProgressPercentage+"%");
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            System.Threading.Thread.Sleep(500);
            this.Close();
        }
    }
}

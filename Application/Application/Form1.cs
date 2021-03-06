﻿using System;
using System.Drawing;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
#pragma warning disable 618

namespace Application
{
    public partial class Form1 : Form
    {
        private Capture _cap;
        private HaarCascade _haar;

        public Form1()
        {
            InitializeComponent();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            using (Image<Bgr, byte> nextFrame = _cap.QueryFrame())
            {
                if (nextFrame != null)
                {
                    // there's only one channel (greyscale), hence the zero index
                    //var faces = nextFrame.DetectHaarCascade(haar)[0];
                    Image<Gray, byte> grayframe = nextFrame.Convert<Gray, byte>();
                    var faces =
                            grayframe.DetectHaarCascade(
                                    _haar, 1.4, 4,
                                    HAAR_DETECTION_TYPE.DO_CANNY_PRUNING,
                                    new Size(nextFrame.Width / 8, nextFrame.Height / 8)
                                    )[0];

                    foreach (var face in faces)
                    {
                        nextFrame.Draw(face.rect, new Bgr(0, double.MaxValue, 0), 3);
                    }
                    pictureBox1.Image = nextFrame.ToBitmap();
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // passing 0 gets zeroth webcam
            _cap = new Capture(0);
            // adjust path to find your xml
            _haar = new HaarCascade(
    "haarcascade_frontalface_alt2.xml");
        }
    }
}

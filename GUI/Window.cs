using System;
using System.Windows.Forms;
using System.Threading;
using System.ComponentModel;
using System.Drawing;
using System.Collections.Generic;
using RayCasting2D;

namespace GUI {
    public partial class Window : Form {
        private BackgroundWorker BackgroundWorker { get; set; }

        private RayCasting RayCasting { get; set; }
        private Cords MouseCords { get; set; }
        private Bitmap RenderedImage { get; set; }

        public Window(){
            InitializeComponent();

            InitializeRayCasting();
            InitializeBackgroundWorker();

            Size = new Size(RayCasting.RenderSettings.Width, RayCasting.RenderSettings.Height);
        }

        private void InitializeRayCasting() {
            RayCasting = new RayCasting();

            RayCasting.RenderSettings.Width = 1500;
            RayCasting.RenderSettings.Height = 900;

            List<Wall> walls = new List<Wall>() {
                new Wall(150, 90, 450, 210),
                new Wall(1200, 450, 900, 30),
                new Wall(240, 750, 840, 750),
                new Wall(640, 450, 840, 360),
                new Wall(210, 450, 440, 680),
                new Wall(1200, 800, 1400, 700)
            };

            RayCasting.Scene.Walls = walls;
        }
        private void InitializeBackgroundWorker() {
            BackgroundWorker = new BackgroundWorker();

            BackgroundWorker.DoWork += RenderLoop;
            BackgroundWorker.ProgressChanged += UpdateGUI;
            BackgroundWorker.WorkerReportsProgress = true;

            BackgroundWorker.RunWorkerAsync();
        }

        //Render
        private void RenderLoop(object sender, DoWorkEventArgs e) {
            int wait_ms = 0;

            while (true) {
                Thread.Sleep(wait_ms);

                RenderedImage = RayCasting.Render(MouseCords);

                BackgroundWorker.ReportProgress(0);

                FPS++;
            }
        }
        private void UpdateGUI(object sender, ProgressChangedEventArgs e) {
            ImageDisplayBox.Image = RenderedImage;

            UpdateFPS();
        }

        private int FPS = 0;
        private DateTime LastTime = DateTime.Now;
        private string FormText = "2D Ray Casting";
        private void UpdateFPS() {
            TimeSpan elapsedTime = DateTime.Now - LastTime;

            if (elapsedTime.TotalSeconds >= 1) {
                double totalFPS = (1.0 / elapsedTime.TotalSeconds) * FPS;

                Text = FormText + " | " + totalFPS.ToString("0.00") + " FPS";

                FPS = 0;
                LastTime = DateTime.Now;
            }
        }

        //Update MouseCords
        private void ImageDisplayBox_MouseMove(object sender, MouseEventArgs e){
            if (RenderedImage != null) {
                Point imageLocation = new Point(
                    (ImageDisplayBox.Width - RenderedImage.Width) / 2,
                    (ImageDisplayBox.Height - RenderedImage.Height) / 2
                );

                Point relativeCords = new Point(e.X - imageLocation.X, e.Y - imageLocation.Y);

                double scale = RayCasting.RenderSettings.Width / (double)RenderedImage.Width;
                MouseCords = new Cords(
                    (int)(relativeCords.X * scale),
                    (int)((RenderedImage.Height - relativeCords.Y - 1) * scale)
                );
            }
        }

        //Resize image
        private Bitmap ResizeBitmap(Bitmap sourceBMP){
            int pictureWidth = ImageDisplayBox.Width;
            int pictureHeight = ImageDisplayBox.Height;

            int width = (int)((sourceBMP.Width / (double)sourceBMP.Height) * pictureHeight);
            int height = pictureHeight;

            if (width > pictureWidth) {
                width = pictureWidth;
                height = (int)((sourceBMP.Height / (double)sourceBMP.Width) * width);
            }

            if (width == 0 || height == 0) return sourceBMP;

            Bitmap result = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(result)) {
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                g.DrawImage(sourceBMP, 0, 0, width, height);
            }
            return result;
        }
    }
}

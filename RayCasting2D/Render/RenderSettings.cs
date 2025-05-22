using System;
using System.Drawing;

namespace RayCasting2D {
    public class RenderSettings {
        public int NumberOfRays { get; set; }

        public int Width { get; set; }
        public int Height { get; set; }

        public Color BackgroundColor { get; set; }
        public Color WallColor { get; set; }
        public Color RayColor { get; set; }

        public RenderSettings() {
            NumberOfRays = 200;

            Width = 500;
            Height = 300;

            BackgroundColor = Color.Black;
            WallColor = Color.White;
            RayColor = Color.White;
        }
    }
}

using System;
using System.Drawing;

namespace RayCasting2D {
    public class RayCasting {
        public Scene Scene { get; set; }
        public RenderSettings RenderSettings { get; set; }

        private Render RenderEngine { get; set; }

        public RayCasting() {
            Scene = new Scene();
            RenderSettings = new RenderSettings();
            RenderEngine = new Render(RenderSettings);
        }

        public Bitmap Render(Cords lightSource) {
            return RenderEngine.RenderScene(Scene, lightSource);
        }
    }
}

using System;
using System.Collections.Generic;

namespace RayCasting2D {
    public class Scene {
        public List<Wall> Walls { get; set; }

        public Scene() {
            Walls = new List<Wall>();
        }
        public Scene(List<Wall> walls) {
            Walls = walls;
        }
    }
}

using System;

namespace RayCasting2D {
    public struct Ray {
        public Cords Origin { get; set; }
        public Cords Point2 { get; set; }

        public Ray(Cords origin, Cords point2) {
            Origin = origin;
            Point2 = point2;
        }
    }
}

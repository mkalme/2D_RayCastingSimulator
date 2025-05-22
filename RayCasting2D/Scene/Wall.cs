using System;

namespace RayCasting2D{
    public class Wall {
        public Cords Point1 { get; set; }
        public Cords Point2 { get; set; }

        public Wall(float x1, float y1, float x2, float y2){
            Point1 = new Cords(x1, y1);
            Point2 = new Cords(x2, y2);
        }
        public Wall(Cords point1, Cords point2){
            Point1 = point1;
            Point2 = point2;
        }
    }
}

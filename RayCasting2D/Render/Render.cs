using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace RayCasting2D {
    class Render {
        public RenderSettings RenderSettings { get; set; }

        private Scene Scene { get; set; }
        private Cords LightSource { get; set; }
        private Bitmap Bitmap { get; set; }
        private Graphics Graphics { get; set; }

        private Border Border { get; set; }

        public Render() {
            RenderSettings = new RenderSettings();
        }
        public Render(RenderSettings renderSettings) {
            RenderSettings = renderSettings;
        }

        public Bitmap RenderScene(Scene scene, Cords lightSource) {
            Scene = scene;
            LightSource = lightSource;

            Bitmap = new Bitmap(RenderSettings.Width, RenderSettings.Height);
            Graphics = Graphics.FromImage(Bitmap);

            Border = new Border(new Cords(0, 0), new Cords(RenderSettings.Width, RenderSettings.Height));

            Ray[] rays = GetAllRays(RenderSettings.NumberOfRays, LightSource);
            List<Cords> intersections = GetAllIntersections(rays, Scene.Walls, Border.GetAllowedWalls(LightSource));

            DrawScene();
            DrawIntersections(intersections);

            return Bitmap;
        }

        //Get intersections
        private static Ray[] GetAllRays(int numberOfRays, Cords lightSource) {
            if (numberOfRays < 1) {
                return new Ray[0];
            }

            Ray[] rays = new Ray[numberOfRays];
            
            float increment = 360.0F / numberOfRays;
            for (int i = 0; i < numberOfRays; i++) {
                rays[i] = GetRayFromAngle(lightSource, i * increment);
            }

            return rays;
        }
        private static Ray GetRayFromAngle(Cords origin, float angle){
            if (angle >= 0 && angle < 90) {
                return new Ray(origin, GetCordsFromAngle(origin, angle, 1, -1));
            } else if (angle >= 90 && angle < 180) {
                angle = 180 - angle;

                return new Ray(origin, GetCordsFromAngle(origin, angle, -1, -1));
            } else if (angle >= 180 && angle < 270) {
                angle -= 180;

                return new Ray(origin, GetCordsFromAngle(origin, angle, -1, 1));
            } else if (angle >= 270 && angle <= 360) {
                angle = 360 - angle;

                return new Ray(origin, GetCordsFromAngle(origin, angle, 1, 1));
            }

            return new Ray();
        }
        private static Cords GetCordsFromAngle(Cords origin, float angle, float widthReverse, float heightReverse){
            float height = (float)Math.Cos(angle / 180 * Math.PI) * heightReverse;
            float width = (float)Math.Sin(angle / 180 * Math.PI) * widthReverse;

            return new Cords(origin.X + width, origin.Y + height);
        }

        private static List<Cords> GetAllIntersections(Ray[] rays, List<Wall> walls, List<Wall> allowedWalls) {
            List<Cords> intersections = new List<Cords>();

            for (int i = 0; i < rays.Length; i++) {
                Cords intersection = new Cords();

                bool intersects = RayIntersects(rays[i], walls, allowedWalls, ref intersection);
                if (intersects) {
                    intersections.Add(intersection);
                }
            }

            return intersections;
        }
        private static bool RayIntersects(Ray ray, List<Wall> walls, List<Wall> borderWalls, ref Cords intersectionPoint) {
            //Check walls
            List<KeyValuePair<Cords, float>> intersections = new List<KeyValuePair<Cords, float>>();

            for (int i = 0; i < walls.Count; i++) {
                Cords intersection = new Cords();

                bool intersects = RayIntersectsWall(ray, walls[i], ref intersection);
                if (intersects) {
                    intersections.Add(new KeyValuePair<Cords, float>(intersection, TwoPointDistance(ray.Origin, intersection)));
                }
            }

            if (intersections.Count > 0) {
                intersectionPoint = intersections.OrderBy(kvp => kvp.Value).First().Key;

                return true;
            } else { //Check borders
                for (int i = 0; i < borderWalls.Count; i++) {
                    Cords intersection = new Cords();

                    bool intersects = RayIntersectsWall(ray, borderWalls[i], ref intersection);
                    if (intersects) {
                        intersectionPoint = intersection;

                        return true;
                    }
                }
            }

            return false;
        }
        private static bool RayIntersectsWall(Ray ray, Wall wall, ref Cords intersectionPoint) {
            float s1_x, s1_y, s2_x, s2_y;
            s1_x = ray.Point2.X - ray.Origin.X; s1_y = ray.Point2.Y - ray.Origin.Y;
            s2_x = wall.Point2.X - wall.Point1.X; s2_y = wall.Point2.Y - wall.Point1.Y;

            float s, t;
            s = (-s1_y * (ray.Origin.X - wall.Point1.X) + s1_x * (ray.Origin.Y - wall.Point1.Y)) / (-s2_x * s1_y + s1_x * s2_y);
            t = (s2_x * (ray.Origin.Y - wall.Point1.Y) - s2_y * (ray.Origin.X - wall.Point1.X)) / (-s2_x * s1_y + s1_x * s2_y);

            if (s >= 0 && s <= 1 && t >= 0) { // Is collision
                intersectionPoint = new Cords(ray.Origin.X + (t * s1_x), ray.Origin.Y + (t * s1_y));

                return true;
            }

            return false; // No collision
        }

        private static float TwoPointDistance(Cords point1, Cords point2) {
            float width = Math.Abs(point2.X - point1.X);
            float height = Math.Abs(point2.Y - point1.Y);

            return (float)Math.Sqrt(width * width + height * height);
        }

        //Draw Scene
        private void DrawScene() {
            Graphics.Clear(RenderSettings.BackgroundColor);

            for (int i = 0; i < Scene.Walls.Count; i++) {
                DrawWall(Scene.Walls[i]);
            }
        }
        private void DrawIntersections(List<Cords> intersections) {
            for (int i = 0; i < intersections.Count; i++) {
                DrawIntersection(intersections[i]);
            }
        }

        private void DrawWall(Wall wall) {
            int height = RenderSettings.Height;

            Point point1 = new Point((int)wall.Point1.X, (int)(height - wall.Point1.Y - 1));
            Point point2 = new Point((int)wall.Point2.X, (int)(height - wall.Point2.Y - 1));

            Graphics.DrawLine(new Pen(RenderSettings.WallColor, 1), point1, point2);
        }
        private void DrawIntersection(Cords cords) {
            int height = RenderSettings.Height;

            Graphics.DrawLine(new Pen(RenderSettings.RayColor),
                (int)LightSource.X, (int)(height - LightSource.Y - 1),
                (int)cords.X, (int)(height - cords.Y - 1)
            );
        }
    }

    class Border {
        public Cords Point1 { get; set; }
        public Cords Point2 { get; set; }

        private Wall[] Walls { get; set; }

        public Border(Cords point1, Cords point2) {
            Point1 = point1;
            Point2 = point2;

            CreateWalls();
        }

        private void CreateWalls() {
            Walls = new Wall[4];

            Walls[0] = new Wall(new Cords(0, Point2.Y), new Cords(Point2.X, Point2.Y));
            Walls[1] = new Wall(Walls[0].Point2, new Cords(Point2.X, 0));
            Walls[2] = new Wall(new Cords(0, 0), Walls[1].Point2);
            Walls[3] = new Wall(new Cords(0, 0), Walls[0].Point1);
        }

        public Wall[] GetWalls() {
            return Walls;
        }

        public List<Wall> GetAllowedWalls(Cords point) {
            List<Wall> walls = new List<Wall>();

            float x = point.X, y = point.Y;
            float x2 = Point2.X, y2 = Point2.Y;

            if (x > 0 && y > 0 && x < x2 && y < y2) { //If middle
                walls.AddRange(Walls);
            } else if (x > 0 && x < x2 && y >= y2) { //If top
                walls = new List<Wall>() { Walls[1], Walls[2], Walls[3] };
            } else if (x >= x2 && y >= y2) { //If top right
                walls = new List<Wall>() { Walls[2], Walls[3] };
            } else if (x >= x2 && y > 0 && y < y2) { //If right
                walls = new List<Wall>() { Walls[0], Walls[2], Walls[3] };
            } else if (x >= x2 && y <= y2) { //If bottom right 
                walls = new List<Wall>() { Walls[0], Walls[3] };
            } else if (x > 0 && x < x2 && y <= 0) { //If bottom 
                walls = new List<Wall>() { Walls[0], Walls[1], Walls[3] };
            } else if (x <= 0 && y <= 0) { //If bottom left
                walls = new List<Wall>() { Walls[0], Walls[1] };
            } else if (x <= 0 && y > 0 && y < y2) { //If left
                walls = new List<Wall>() { Walls[0], Walls[1], Walls[2] };
            } else{ //If top left
                walls = new List<Wall>() { Walls[1], Walls[2] };
            }

            return walls;
        }
    }
}

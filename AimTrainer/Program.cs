using System.Drawing;
using WindowsInput;

namespace AimTrainer {
    internal class Program {
        public static List<Point> points = new List<Point>();
        static void Main(string[] args) {
            Point point1 = new Point(400, 250), point2 = new Point(1550, 750);
            ComputePositions(point1, point2);

            Console.WriteLine("write 'start' to begin");
            while (true) {
                string line = Console.ReadLine();
                if (line == "start") break;
            }
            Console.WriteLine("it has begun");

            int hits = 0;
            InputSimulator simulator = new InputSimulator();
            Point pointhere = UpScalePoint(new Point(950, 500));
            simulator.Mouse.MoveMouseTo(pointhere.X, pointhere.Y);
            simulator.Mouse.LeftButtonClick();



            while (true) {
                int target = GetPixelColor(point1, point2);
                Point point = UpScalePoint(points[target]);
                simulator.Mouse.MoveMouseTo(point.X, point.Y);
                simulator.Mouse.LeftButtonClick();
                hits++;
                Thread.Sleep(50);
                if (hits >= 30) break;
            }



        }
        static int GetPixelColor(Point upperLeft, Point lowerRight) {
            Bitmap screenCapture = new Bitmap(lowerRight.X - upperLeft.X, lowerRight.Y - upperLeft.Y);
            Graphics g = Graphics.FromImage(screenCapture);
            g.CopyFromScreen(upperLeft, new Point(0, 0), screenCapture.Size);

            List<Color> colors = new List<Color>();

            for (int i = 0; i < points.Count; i++) {
                Color pixel = screenCapture.GetPixel(points[i].X - upperLeft.X, points[i].Y - upperLeft.Y);
                if (pixel.R >= 50 && pixel.G >= 140 && pixel.B >= 200)
                    return i;
            }



            return -1;
        }

        static Point UpScalePoint(Point point) {
            // the resolution of the screen, in my case its 1080p
            float xResolution = 1920, yResolution = 1080;
            // inputsimulators weird max value for some reason
            int scaleMax = 65535;

            Point newPoint = new();

            newPoint.X = (int)((point.X / xResolution) * scaleMax);
            newPoint.Y = (int)((point.Y / yResolution) * scaleMax);

            return newPoint;
        }
        static void ComputePositions(Point upperLeft, Point lowerRight) {
            int xLen = 80, yLen = 80;

            for (int i = 0; i < 7; i++) {
                for (int j = 0; j < 14; j++) {
                    points.Add(new Point(upperLeft.X + xLen * j, upperLeft.Y + yLen * i));
                }
            }
        }
    }
}
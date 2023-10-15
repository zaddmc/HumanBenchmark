using System.Drawing;
using WindowsInput;

namespace SequenceTest {
    internal class Program {
        public static List<Point> points = new List<Point>();
        static void Main(string[] args) {
            ComputePositions(new Point(786, 366), new Point(1108, 702));

            Console.WriteLine("write 'start' to begin");
            while (true) {
                string line = Console.ReadLine();
                if (line == "start") break;
            }
            // it has begun
            Console.WriteLine("it has begun");

            List<int> flashList = new List<int>();
            DateTime lastFlash = DateTime.Now;

            while (true) {
                for (int i = 0; i < points.Count; i++) {
                    // check if block is white
                    Color color = GetPixelColor(points[i]);
                    if (color.R >= 200 && color.G >= 200 && color.B >= 200) {
                        if (flashList.Count == 0 || flashList[^1] != i) {
                            Console.WriteLine("Point added: " + i);
                            flashList.Add(i);
                            lastFlash = DateTime.Now;
                        }
                    }
                }

                // if 3 seconds have passed
                if ((DateTime.Now - lastFlash).TotalSeconds >= 3) {
                    InputSimulator simulator = new InputSimulator();
                    for (int i = 0; i < flashList.Count; i++) {
                        Console.WriteLine("point to be hit: "+ flashList[i].ToString());
                        Point point = UpScalePoint(points[flashList[i]]);
                        simulator.Mouse.MoveMouseTo(point.X, point.Y);
                        simulator.Mouse.LeftButtonClick();
                    }
                    flashList.Clear();
                    lastFlash = DateTime.Now;

                }

                // checks 10 times a second 
                Thread.Sleep(100);

            }

        }
        static Color GetPixelColor(Point point) {
            using (Bitmap screenCapture = new Bitmap(1, 1)) {
                using (Graphics g = Graphics.FromImage(screenCapture)) {
                    g.CopyFromScreen(point.X, point.Y, 0, 0, new Size(1, 1));
                }

                // return the color of the pixel
                return screenCapture.GetPixel(0, 0);
            }
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
            int xLen = (int)((lowerRight.X - upperLeft.X) / 2);
            int yLen = (int)((lowerRight.Y - upperLeft.Y) / 2);

            for (int i = 0; i < 3; i++) {
                for (int j = 0; j < 3; j++) {
                    points.Add(new Point(upperLeft.X + xLen * j, upperLeft.Y + yLen * i));
                }
            }
        }
    }
}
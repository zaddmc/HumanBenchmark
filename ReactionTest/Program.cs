using System;
using System.Drawing;
using System.Runtime.InteropServices;
using WindowsInput;

namespace ReactionTest {
    internal class Program {
        [DllImport("user32.dll")]
        public static extern bool GetCursorPos(out Point lpPoint);

        [DllImport("user32.dll")]
        public static extern IntPtr GetDC(IntPtr hwnd);

        [DllImport("gdi32.dll")]
        public static extern uint GetPixel(IntPtr hdc, int nXPos, int nYPos);

        [DllImport("user32.dll")]
        public static extern int ReleaseDC(IntPtr hwnd, IntPtr hdc);

        // windows shitty scalling input here
        public const float Scalling = 1.25f;
        static void Main(string[] args) {
            Console.WriteLine("Hello World!");

            Point cursorPos;
            InputSimulator simulator = new InputSimulator();


            while (true) {
                Console.ReadKey(true);
                if (GetCursorPos(out cursorPos)) {
                    IntPtr screenDC = GetDC(IntPtr.Zero);
                    uint pixelColor = GetPixel(screenDC, (int)(cursorPos.X * Scalling), (int)(cursorPos.Y * Scalling));
                    ReleaseDC(IntPtr.Zero, screenDC);

                    Color color = Color.FromArgb((int)(pixelColor & 0x000000FF), (int)((pixelColor & 0x0000FF00) >> 8), (int)((pixelColor & 0x00FF0000) >> 16));

                    Console.WriteLine($"Pixel color at cursor position ({(int)(cursorPos.X * Scalling)}, {(int)(cursorPos.Y * Scalling)}): R={color.R}, G={color.G}, B={color.B}");
                    if (color.G >= 200) {
                        //simulator.Mouse.LeftButtonClick();
                    }
                }
            }
        }
    }
}
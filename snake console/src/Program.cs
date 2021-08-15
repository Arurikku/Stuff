using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleGame
{
    class Program
    {

        [DllImport("user32.dll")]
        internal extern static int SetWindowLong(IntPtr hwnd, int index, int value);

        [DllImport("user32.dll")]
        internal extern static int GetWindowLong(IntPtr hwnd, int index);

        [DllImport("kernel32.dll", ExactSpelling = true)]
        private static extern IntPtr GetConsoleWindow();
        [DllImport("User32.dll")]
        public static extern IntPtr GetDC(IntPtr hwnd);
        [DllImport("User32.dll")]
        static extern int ReleaseDC(IntPtr hwnd, IntPtr dc);
        static Graphics gr;
        static int direction = 1;
        static ConsoleKey key;
        static List<int[]> parts = new List<int[]>();
        static int winWidth = 1024, winHeight = 512, playerSize = 3, playerSpeed = 300;
        static bool alive = false;
        static int ax = 0, ay = 0;
        static int score = 0;
        static int getSmallerDelay = 200;
        static BufferedGraphics graphicsBuffer;
        static IntPtr desktop;
        static Graphics g;
        [DllImport("user32.dll", SetLastError = true)]
        internal static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);
        static void checkKeypress()
        {
            while (alive)
            {
                key = Console.ReadKey(true).Key;
                switch (key)
                {
                    case ConsoleKey.LeftArrow:
                        if (direction != 1)
                            direction = -1;
                        break;
                    case ConsoleKey.RightArrow:
                        if (direction != -1)
                            direction = 1;
                        break;
                    case ConsoleKey.UpArrow:
                        if (direction != -2)
                            direction = 2;
                        break;
                    case ConsoleKey.DownArrow:
                        if (direction != 2)
                            direction = -2;
                        break;
                }
            }
        }
        static void timerEpicThing()
        {
            while (alive)
            {
                Thread.Sleep(getSmallerDelay);
                if(winWidth > 180)
                {
                    winWidth -= 2;
                }
                if (winHeight > 130)
                {
                    winHeight--;
                }
                MoveWindow(Process.GetCurrentProcess().MainWindowHandle, 0, 0, winWidth+39, winHeight+39, true);
            }
        }
        static void Main(string[] args)
        {
            winWidth = 1024;
            winHeight = 512;
            const int GWL_STYLE = -16;
            long value = GetWindowLong(Process.GetCurrentProcess().MainWindowHandle, GWL_STYLE);
            SetWindowLong(Process.GetCurrentProcess().MainWindowHandle, GWL_STYLE, (int)(value & -131073 & -65537));
            parts = new List<int[]>();
            direction = 1;
            Console.WriteLine("Snake Console By Alric");
            Console.WriteLine("Press a key to start");
            Console.ReadKey();
            Console.Title = "Score: " + 0;
            desktop = GetDC(Process.GetCurrentProcess().MainWindowHandle);
            g = Graphics.FromHdc(desktop);
            g.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighSpeed;
            graphicsBuffer = BufferedGraphicsManager.Current.Allocate(g, new Rectangle(0, 0, winWidth, winHeight));
            gr = graphicsBuffer.Graphics;
            int x = 10, y = 10;
            alive = true;
            Console.Clear();
            Task consoleKeyTask = Task.Run(() => { checkKeypress(); });
            Task reduceSizeOfConsoleThingCoolEpic = Task.Run(() => { timerEpicThing(); });
            parts.Add(new int[] { 0, 0 });
            Random rnd = new Random();
            ax = rnd.Next(playerSize, winWidth - (playerSize * 2));
            ay = rnd.Next(playerSize, winHeight - (playerSize * 2));
            while (alive)
            {
                gr.Clear(Color.Black);
                if (y > winHeight - (playerSize * 2))
                {
                    y = playerSize;
                }
                if (y < playerSize)
                {
                    y = winHeight - (playerSize * 2);
                }
                if (x > winWidth - playerSize)
                {
                    x = playerSize;
                }
                if(x < playerSize)
                {
                    x = winWidth - (playerSize*2);
                }
                switch (direction)
                {
                    case -1:
                        x--;
                        break;
                    case 1:
                        x++;
                        break;
                    case 2:
                        y--;
                        break;
                    case -2:
                        y++;
                        break;
                }
                gr.FillRectangle(Brushes.White, 0, 0, winWidth, winHeight);
                if(ax > winWidth - (playerSize * 2) - 2 || ay > winHeight - (playerSize * 2) - 2)
                {
                    ax = rnd.Next(playerSize, winWidth - (playerSize * 2));
                    ay = rnd.Next(playerSize, winHeight - (playerSize * 2));
                }
                parts[0] = new int[] { x, y };
                    drawPlayer();
                drawBapple();
                graphicsBuffer.Render(g);
                Thread.Sleep(1000/playerSpeed);
            }
            gr.DrawString("You died with a score of " + score, new Font("Arial", 8), Brushes.Red, 0, 0);
            gr.DrawString("Press a key to restart", new Font("Arial", 8), Brushes.Red, 0, 20);
            graphicsBuffer.Render(g);
            ReleaseDC(Process.GetCurrentProcess().MainWindowHandle, desktop);
            Console.ReadKey();
            Console.Clear();
            Main(args);
        }
        static void drawPlayer()
        {
            for (int i = parts.Count-1; i >= 0; i--)
            {
                if(i == 0)
                {
                    gr.FillRectangle(Brushes.DarkGreen, parts[0][0], parts[0][1], playerSize, playerSize);
                }
                else
                {
                    gr.FillRectangle(Brushes.Lime, parts[i - 1][0], parts[i - 1][1], playerSize, playerSize);
                    if(i > 1)
                    {
                        var diffx = Math.Abs(parts[i][0] - parts[0][0]);
                        var diffy = Math.Abs(parts[i][1] - parts[0][1]);
                        if(diffx <1 && diffy <1)
                        {
                            alive = false;
                        }
                    }
                    parts[i] = parts[i-1];
                }
            }
        }
        static void drawBapple()
        {
            gr.FillEllipse(Brushes.Red, ax, ay, 8, 8);
            var diffx = Math.Abs(parts[0][0] - ax);
            var diffy = Math.Abs(parts[0][1] - ay);
            if (diffx < 8 && diffy < 8)
            {
                parts.Add(new int[] { parts[0][0], parts[0][1] });
                parts.Add(new int[] { parts[0][0], parts[0][1] });
                parts.Add(new int[] { parts[0][0], parts[0][1] });
                parts.Add(new int[] { parts[0][0], parts[0][1] });
                Random rnd = new Random();
                ax = rnd.Next(playerSize, winWidth - (playerSize * 2));
                ay = rnd.Next(playerSize, winHeight - (playerSize * 2));
                score++;
                Console.Title = "Score: " + score;
            }
        }
    }
}

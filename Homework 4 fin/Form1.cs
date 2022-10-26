using Homework_4_fin;
using System;
using System.Diagnostics.Metrics;
using System.Runtime.Intrinsics.X86;

namespace Homework_4_fin
{
    public partial class Form1 : Form
    {

        Bitmap b;
        Graphics g;
        Random r = new Random();
        Pen Pen = new Pen(Color.Blue, 1);
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            b = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            g = Graphics.FromImage(b);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            g.Clear(Color.White);


            int numberoftrial = 100;
            double successProbability = 0.5;
            int TrajectoryNumber = 30;

            double minX = 0;
            double minY = 0;

            double maxX = numberoftrial;
            double maxY = numberoftrial;

            Rectangle virtualWindow = new Rectangle(20, 20, b.Width - 300, b.Height - 40);
            g.DrawRectangle(Pens.Black, virtualWindow);

            List<Point> lasttrial = new List<Point>();

            for (int i = 0; i < TrajectoryNumber; i++)
            {
                List<Point> points = new List<Point>();
                double success = 0;

                for (int X = 0; X < numberoftrial; X++)
                {
                    int uniform = r.Next(2);

                    if (uniform < successProbability)
                        success = success + 1;

                    int xCord = FromXrealToXVirtual(X, minX, maxX, virtualWindow.Left, virtualWindow.Width);
                    int yCord = FromYRealToYVirtual(success, minY, maxY, virtualWindow.Top, virtualWindow.Height);

                    Point p = new Point(xCord, yCord);
                    points.Add(p);

                    if (X == numberoftrial - 1)
                    {
                        lasttrial.Add(p);
                    }
                }
                g.DrawLines(Pen, points.ToArray());
            }

            int min_y = lasttrial.Min(p => p.Y);
            int max_y = lasttrial.Max(p => p.Y);

            Rectangle r1 = new Rectangle(virtualWindow.Right + 10, 20, 260, b.Height - 40);
            g.DrawRectangle(Pens.Black, r1);

            if (TrajectoryNumber == 1)
            {
                foreach (Point p in lasttrial)
                {
                    Rectangle rec = new Rectangle(r1.Left + 10, p.Y - 10, r1.Right - r1.Left - 20, 20);
                    g.FillRectangle(Brushes.Green, rec);
                    g.DrawRectangle(Pens.Black, rec);
                }
            }
            else
            {
                int intervals_number = TrajectoryNumber / 2;
                if (intervals_number > 15)
                {
                    intervals_number = 15;
                }

                int span = max_y - min_y;
                int interval_size = (max_y - min_y) / intervals_number;

                while (min_y + interval_size * intervals_number < max_y + 1)
                {
                    intervals_number++;
                }

                int minimo = min_y;

                Dictionary<Interval, int> intervalli = new Dictionary<Interval, int>();

                for (int j = 0; j < intervals_number; j++)
                {
                    Interval intervallo = new Interval(minimo, minimo + interval_size);
                    minimo = minimo + interval_size;
                    intervalli[intervallo] = 0;
                }

                foreach (Point p in lasttrial)
                {
                    List<Interval> inter = intervalli.Keys.ToList();
                    int intY = (int)p.Y;

                    foreach (Interval i in inter)
                    {
                        if (intY >= i.down && intY < i.up)
                        {
                            intervalli[i]++;
                        }
                    }
                }

                List<Rectangle> chart = new List<Rectangle>();

                int dimdisp = r1.Right - r1.Left - 20;
                int maxValue = intervalli.Values.Max();

                foreach (var v in intervalli)
                {
                    double intensity = ((double)v.Value / (double)maxValue) * dimdisp;
                    Rectangle rect = new Rectangle(r1.Left + 10, v.Key.down, (int)intensity, interval_size);
                    chart.Add(rect);
                }

                foreach (Rectangle re in chart)
                {
                    g.FillRectangle(Brushes.Green, re);
                    g.DrawRectangle(Pens.White, re);
                }

            }

           
                pictureBox1.Image = b;
               
         }
        
        public int FromXrealToXVirtual(double X, double minX, double maxX, int Left, int W)
        {
            return Left + (int)(W * ((X - minX) / (maxX - minX)));
        }

        public int FromYRealToYVirtual(double Y, double minY, double maxY, int Top, int H)
        {
            return Top + (int)(H - H * ((Y - minY) / (maxY - minY)));
        }


        private void button2_Click(object sender, EventArgs e)
        {
            b = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            g = Graphics.FromImage(b);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            g.Clear(Color.White);



            int numberoftrial = 100;
            double successProbability = 0.5;
            int TrajectoryNumber = 30;

            double minX = 0;
            double maxX = (double)numberoftrial;
            double minY = 0;
            double maxY = 1;

            Rectangle rect = new Rectangle(20, 20, b.Width - 300, b.Height - 40);
            g.DrawRectangle(Pens.Black, rect);

            List<Point> lasttrial = new List<Point>();

            for (int t = 0; t < TrajectoryNumber; t++)
            {
                List<Point> points = new List<Point>();
                double Yt = 0;

                for (int X = 0; X < numberoftrial; X++)
                {
                    double uniform = r.NextDouble();

                    if (r.NextDouble() < successProbability) Yt = Yt + 1;

                    double Y = Yt / (X + 1);

                    int xCord = FromXrealToXVirtual(X, minX, maxX, rect.Left, rect.Width);
                    int yCord = FromYRealToYVirtual(Y, minY, maxY, rect.Top, rect.Height);

                    Point p = new Point(xCord, yCord);
                    points.Add(p);

                    if (X == numberoftrial - 1)
                    {
                        lasttrial.Add(p);
                    }
                }
                g.DrawLines(Pen, points.ToArray());
            }

            int min_y = lasttrial.Min(p => p.Y);
            int max_y = lasttrial.Max(p => p.Y);

            Rectangle r2 = new Rectangle(rect.Right + 10, 20, 260, b.Height - 40);
            g.DrawRectangle(Pens.Black, r2);

            if (TrajectoryNumber == 1)
            {
                foreach (Point p in lasttrial)
                {
                    Rectangle re = new Rectangle(r2.Left + 10, p.Y - 10, r2.Right - r2.Left - 20, 20);
                    g.FillRectangle(Brushes.Green, re);
                    g.DrawRectangle(Pens.White, re);
                }
            }
            else
            {
                int intervals_number = TrajectoryNumber / 2;
                if (intervals_number > 15)
                {
                    intervals_number = 15;
                }

                int span = max_y - min_y;
                int interval_size = (max_y - min_y) / intervals_number;

                while (min_y + interval_size * intervals_number < max_y + 1)
                {
                    intervals_number++;
                }

                int minimo = min_y;

                Dictionary<Interval, int> intervalli = new Dictionary<Interval, int>();

                for (int j = 0; j < intervals_number; j++)
                {
                    Interval intervallo = new Interval(minimo, minimo + interval_size);
                    minimo = minimo + interval_size;
                    intervalli[intervallo] = 0;
                }

                foreach (Point p in lasttrial)
                {
                    List<Interval> inter = intervalli.Keys.ToList();
                    int intY = (int)p.Y;

                    foreach (Interval i in inter)
                    {
                        if (intY >= i.down && intY < i.up)
                        {
                            intervalli[i]++;
                        }
                    }
                }

                List<Rectangle> chart = new List<Rectangle>();

                int dimdisp = r2.Right - r2.Left - 20;
                int maxValue = intervalli.Values.Max();

                foreach (var v in intervalli)
                {
                    double intensity = ((double)v.Value / (double)maxValue) * dimdisp;
                    Rectangle rect1 = new Rectangle(r2.Left + 10, v.Key.down, (int)intensity, interval_size);
                    chart.Add(rect1);
                }

                foreach (Rectangle re in chart)
                {
                    g.FillRectangle(Brushes.Green, re);
                    g.DrawRectangle(Pens.White, re);
                }

            }

            pictureBox1.Image = b;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            b = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            g = Graphics.FromImage(b);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            g.Clear(Color.White);

           

            int numberOfTrial = 100;
            double successProbability = 0.5;
            int TrajectoryNumber = 30;

            double minX = 0;
            double maxX = numberOfTrial;
            double minY = 0;
            double maxY = numberOfTrial * successProbability;

            Rectangle r3 = new Rectangle(20, 20, b.Width - 300, b.Height -40);
            g.DrawRectangle(Pens.Black, r3);

            List<Point> lastpoints = new List<Point>();

            for (int t = 0; t < TrajectoryNumber; t++)
            {
                List<Point> points = new List<Point>();
                double Yt = 0;

                for (int X = 0; X < numberOfTrial; X++)
                {
                    r.NextDouble();

                    if (r.NextDouble() < successProbability) Yt = Yt + 1;

                    double Y = Yt / Math.Sqrt(X + 1);
                    int xCord = FromXrealToXVirtual(X, minX, maxX, r3.Left, r3.Width);
                    int yCord = FromYRealToYVirtual(Y, minY, maxY, r3.Top, r3.Height);

                    Point p = new Point(xCord, yCord);
                    points.Add(p);

                    if (X == numberOfTrial - 1)
                    {
                        lastpoints.Add(p);
                    }
                }
                g.DrawLines(Pen, points.ToArray());
            }

            int min_y = lastpoints.Min(p => p.Y);
            int max_y = lastpoints.Max(p => p.Y);

            Rectangle r2 = new Rectangle(r3.Right + 10, 20, 260, b.Height - 40);
            g.DrawRectangle(Pens.Black, r2);

            if (TrajectoryNumber == 1)
            {
                foreach (Point p in lastpoints)
                {
                    Rectangle re = new Rectangle(r2.Left + 10, p.Y - 10, r2.Right - r2.Left - 20, 20);
                    g.FillRectangle(Brushes.Green, re);
                    g.DrawRectangle(Pens.White, re);
                }
            }
            else
            {
                int intervals_number = TrajectoryNumber / 6;

                int span = max_y - min_y;
                int interval_size = (max_y - min_y) / intervals_number;

                while (min_y + interval_size * intervals_number < max_y + 1)
                {
                    intervals_number++;
                }

                int minimo = min_y;

                Dictionary<Interval, int> intervalli = new Dictionary<Interval, int>();

                for (int j = 0; j < intervals_number; j++)
                {
                    Interval intervallo = new Interval(minimo, minimo + interval_size);
                    minimo = minimo + interval_size;
                    intervalli[intervallo] = 0;
                }

                foreach (Point p in lastpoints)
                {
                    List<Interval> inter = intervalli.Keys.ToList();
                    int intY = (int)p.Y;

                    foreach (Interval i in inter)
                    {
                        if (intY >= i.down && intY < i.up)
                        {
                            intervalli[i]++;
                        }
                    }
                }

                List<Rectangle> chart = new List<Rectangle>();

                int dimdisp = r2.Right - r2.Left - 20;
                int maxValue = intervalli.Values.Max();

                foreach (var v in intervalli)
                {
                    double intensity = ((double)v.Value / (double)maxValue) * dimdisp;
                    Rectangle rect = new Rectangle(r2.Left + 10, v.Key.down, (int)intensity, interval_size);
                    chart.Add(rect);
                }

                foreach (Rectangle re in chart)
                {
                    g.FillRectangle(Brushes.Green, re);
                    g.DrawRectangle(Pens.White, re);
                }

            }

            pictureBox1.Image = b;
        }
    
    }
}
using System;
using System.Drawing;
using System.Windows.Forms;

namespace SimpleForms
{
    public partial class HeatmapControl : Control
    {
        public HeatmapControl()
        {
            InitializeComponent();
            NumberOfPoints = 100;
            data = new double[NumberOfPoints];
            MinX = 0;
            MaxX = 99;

            ColourMapper = DefaultColourMapper;
        }

        static Color[] colours = { Color.DarkBlue, Color.DarkBlue, Color.DarkBlue, Color.Green, Color.Yellow, Color.Red, Color.Magenta, Color.White, Color.White };

        public double Threshold { get; set; } = 10;

        Color DefaultColourMapper(double x)
        {
            if (x <= 0 || double.IsNaN(x)) return Color.Black;

            if (x < Threshold) return colours[0];

            int i = (int)(x*0.5);
            if(i>=colours.Length-1) i = colours.Length-2;

            Color c1 = colours[i], c2 = colours[i + 1];

            double r = x - (int)x;

            return Color.FromArgb((int)(c1.R * (1.0 - r) + c2.R * r), (int)(c1.G * (1.0 - r) + c2.G * r), (int)(c1.B * (1.0 - r) + c2.B * r));
        }

        Func<double, Color> ColourMapper { get; set; }

        protected override void OnPaint(PaintEventArgs pe)
        {
            if (data.Length == 0) return;

            for (int x = pe.ClipRectangle.X; x < pe.ClipRectangle.Right; ++x)
            {
                var index1 = (int)PointToIndex(x);
                var index2 = (int)PointToIndex(x + 1);
                var c = Color.Black;
                if (lastUpdate >= index1 && lastUpdate < index2)
                {
                    c = Color.Magenta;
                }
                else
                {
                    // Look at all points in the region
                    double v = data[index1 < data.Length && index1>=0 ? index1: data.Length-1];
                    for (int px = (int)index1 + 1; px < index2; ++px)
                        v = Math.Max(v, data[px]);
                    c = ColourMapper(v);
                }
                Pen p = new Pen(c);
                pe.Graphics.DrawLine(p, x, 0, x, Size.Height);
            }
        }

        public int NumberOfPoints
        {
            get => data.Length;
            set
            {
                data = new double[value];
            }
        }

        public double MinX { get; set; }
        public double MaxX { get; set; }

        int lastUpdate;
        double[] data;

        double PointToIndex(int p) => (double)p * (double)NumberOfPoints / (double)Size.Width;
        double IndexToPoint(int index) => (double)index * (double)Size.Width / (double)NumberOfPoints;

        void InvalidateX(int x)
        {
            var px = (int)IndexToPoint(x);
            Invalidate(new Rectangle(px, 0, 1, Size.Height));
        }

        public void Update(double x, double value) =>
            Update((int)(data.Length * (x-MinX)/(MaxX-MinX)), value);

        private void Update(int point, double value)
        {
            InvalidateX(lastUpdate);
            lastUpdate = point;
            if(point < data.Length)
                data[point] = value;
            InvalidateX(lastUpdate);
        }
    }
}

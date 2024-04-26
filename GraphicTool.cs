using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace WinFormsApp1
{
    public partial class GraphicTool : UserControl
    {
        private PointF[] points;
        private float scaleX = 1.0f;
        private float scaleY = 1.0f;
        private float offsetX = 0.0f;
        private float offsetY = 0.0f;

        public GraphicTool()
        {

            this.Size = new Size(400, 400);
            this.BackColor = Color.White;
        }

        public void PlotPoints(PointF[] points)
        {
            this.points = points;
            CalculateScaleAndOffset();
            this.Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            Pen axisPen = new Pen(Color.Black);
            g.DrawLine(axisPen, 0, Height / 2, Width, Height / 2);
            g.DrawLine(axisPen, Width / 2, 0, Width / 2, Height);

            Font axisFont = new Font("Arial", 8);
            Brush axisBrush = Brushes.Black;
            StringFormat format = new StringFormat();
            format.Alignment = StringAlignment.Center;
            g.DrawString("X", axisFont, axisBrush, Width - 10, Height / 2 - 10, format);
            g.DrawString("Y", axisFont, axisBrush, Width / 2 + 10, 0, format);
            if (points != null && points.Length > 1)
            {

                Pen linePen = new Pen(Color.Red);
                for (int i = 0; i < points.Length - 1; i++)
                {
                    g.DrawLine(linePen, TranslatePoint(points[i]), TranslatePoint(points[i + 1]));
                }
                g.DrawLine(linePen, TranslatePoint(points[points.Length - 1]), TranslatePoint(points[points.Length - 1]));
            }
        }

        private PointF TranslatePoint(PointF point)
        {

            float x = point.X * scaleX + Width / 2 + offsetX;
            float y = -point.Y * scaleY + Height / 2 + offsetY;
            return new PointF(x, y);
        }

        private void CalculateScaleAndOffset()
        {
            if (points == null || points.Length == 0)
            return;

            // Находим абсолютные максимальные значения координат
            float maxX = Math.Abs(points[0].X);
            float maxY = Math.Abs(points[0].Y);

            foreach (PointF point in points)
            {
                maxX = Math.Max(maxX, Math.Abs(point.X));
                maxY = Math.Max(maxY, Math.Abs(point.Y));
            }

            // Находим максимальное значение между maxX и maxY
            float maxCoordinate = Math.Max(maxX, maxY);

            // Рассчитываем масштаб для обеих осей так, чтобы единичные отрезки были одинаковыми
            scaleX = Width / (2 * maxCoordinate); // Учитываем двунаправленность координатной оси
            scaleY = Height / (2 * maxCoordinate); // Учитываем двунаправленность координатной оси

            // Проверяем, виден ли график
            if (scaleX < 1 ||  scaleY < 1)
                   {
                // Увеличиваем масштаб, чтобы график был виден
                float scaleFactor = Math.Max(1, Math.Max(1 / scaleX, 1 / scaleY));
                scaleX *= scaleFactor;
                scaleY *= scaleFactor;
            }

            offsetX = 0; // Не нужно смещать график по X, если используем максимальные координаты
            offsetY = 0; // Не нужно смещать график по Y, если используем максимальные координаты
        }
    }
}

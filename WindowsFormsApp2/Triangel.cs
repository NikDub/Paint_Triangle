using System;
using System.Drawing;

namespace WindowsFormsApp2
{
    class Triangel
    {
        public PointF[] PointCoordinates { get; set; } //координаты
        public int ColorNumber { get; set; } //виличена оттенка цвета
        public SolidBrush Brush { get; set; } //кисть для закрашиания
        public Triangel(PointF point1, PointF point2, PointF point3) 
        {
            this.PointCoordinates = new PointF[] { point1, point2, point3 };
        }
        public Triangel(string[] pointArr)
        {
            PointCoordinates = new PointF[]
            {
                new PointF(Convert.ToInt32(pointArr[0]), Convert.ToInt32(pointArr[1])),
                new PointF(Convert.ToInt32(pointArr[2]), Convert.ToInt32(pointArr[3])),
                new PointF(Convert.ToInt32(pointArr[4]), Convert.ToInt32(pointArr[5]))
            };
        }

        private float Multiplication(float x1, float y1, float x2, float y2)
        {
            return x1 * y2 - x2 * y1;
        }
        private bool AreCrossingCut(PointF a1, PointF b1, PointF a2, PointF b2) //проверка на пересечение 2 отрезков
        {
            float v1 = Multiplication(b2.X - a2.X, b2.Y - a2.Y, a1.X - a2.X, a1.Y - a2.Y);
            float v2 = Multiplication(b2.X - a2.X, b2.Y - a2.Y, b1.X - a2.X, b1.Y - a2.Y);
            float v3 = Multiplication(b1.X - a1.X, b1.Y - a1.Y, a2.X - a1.X, a2.Y - a1.Y);
            float v4 = Multiplication(b1.X - a1.X, b1.Y - a1.Y, b2.X - a1.X, b2.Y - a1.Y);
            if ((v1 * v2) < 0 && (v3 * v4) < 0)
                return true;
            return false;
        }
        public bool AreCrossingTriangle(PointF[] triang2) //проверка на пересечении треугольников
        {
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                    if (AreCrossingCut(PointCoordinates[i], PointCoordinates[(i + 1) % 3], triang2[j], triang2[(j + 1) % 3]))
                        return true;
            return false;
        }
        public bool Check_point(PointF point) //проверка на вхождение точки в область треугольника
        {
            double a = (PointCoordinates[0].X - point.X) * (PointCoordinates[1].Y - PointCoordinates[0].Y) - (PointCoordinates[1].X - PointCoordinates[0].X) * (PointCoordinates[0].Y - point.Y);
            double b = (PointCoordinates[1].X - point.X) * (PointCoordinates[2].Y - PointCoordinates[1].Y) - (PointCoordinates[2].X - PointCoordinates[1].X) * (PointCoordinates[1].Y - point.Y);
            double c = (PointCoordinates[2].X - point.X) * (PointCoordinates[0].Y - PointCoordinates[2].Y) - (PointCoordinates[0].X - PointCoordinates[2].X) * (PointCoordinates[2].Y - point.Y);
            if (a * b * c == 0)
            {
                return false;
            }
            return (((a <= 0) ^ (b <= 0)) || ((a <= 0) ^ (c <= 0)));
        }
    }
}

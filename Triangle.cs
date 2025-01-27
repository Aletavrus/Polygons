using System.Linq;
using Avalonia.Platform;
using System;

namespace Polygons;
using Avalonia;
using System;
using Avalonia.Media;

sealed class Triangle : Shape
{
    public Triangle(int x, int y) : base(x, y)
    {
    }
    
    public override void Draw(DrawingContext dc)
    {
        Pen pen = new Pen(Brushes.BlueViolet, 1, lineCap:PenLineCap.Square);
        Brush brush = new SolidColorBrush(Colors.DodgerBlue);

        Point[] points = new Point[4]
        {
            new Point(x, y - R), 
            new Point(x + Math.Sqrt(3)*R/2, y + R/2), 
            new Point(x - Math.Sqrt(3)*R/2, y + R/2),
            new Point(x, y - R)
        };
        
        PolylineGeometry geometry = new PolylineGeometry(points, true);
        
        
        dc.DrawGeometry(brush, pen, geometry);
    }

    public override bool IsInside(int x, int y)
    {
        Point P = new Point(x, y);
        Point top = new Point(this.X, this.Y-R);
        Point right = new Point(this.X + Math.Sqrt(3) * R / 2, this.Y + R / 2);
        Point left = new Point(this.X - Math.Sqrt(3) * R / 2, this.Y + R / 2);
        double area = Area(left, right, top);
        double area1 = Area(left, right, P);
        double area2 = Area(left, top, P);
        double area3 = Area(right, top, P);
        if (area1 + area2 + area3 - area < 0.1)
        {
            return true;
        }
        return false;
    }

    private double Area(Point p1, Point p2, Point p3)
    {
        return double.Abs((p1.X*(p2.Y-p3.Y) + p2.X*(p3.Y-p3.Y) + p3.X*(p1.Y-p2.Y))/2.0);
    }
}
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
        Console.WriteLine("DRAWING TRIANGLE");
    }
}
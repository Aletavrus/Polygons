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
        Pen pen = new Pen(Brushes.BlueViolet, 5, lineCap:PenLineCap.Square);
        Brush brush = new SolidColorBrush(Colors.DodgerBlue);

        double xDiff = Math.Sqrt(3) * R / 2;
        double yDiff = R / 2;
        
        StreamGeometry geometry = new StreamGeometry();
        using (var ctx = geometry.Open())
        {
            ctx.BeginFigure(new Point(X, Y-R), true); // Start at the top vertex
            ctx.LineTo(new Point(X+xDiff, Y+yDiff));   // Draw to bottom-right vertex
            ctx.LineTo(new Point(X-xDiff, Y+yDiff));    // Draw to bottom-left vertex
            ctx.LineTo(new Point(X, Y-R));           // Close the triangle
        }
        
        dc.DrawGeometry(brush, pen, geometry);
        Console.WriteLine("DRAWING TRIANGLE");
    }
}
using Avalonia.Media;
using System;
using Avalonia;

namespace Polygons;

sealed class Square : Shape
{
    public Square(int x, int y) : base(x, y)
    {
    }

    public override void Draw(DrawingContext dc)
    {
        Pen pen = new Pen(Brushes.SandyBrown, 5, lineCap:PenLineCap.Square);
        Brush brush = new SolidColorBrush(Colors.DarkRed);
        
        dc.DrawRectangle(brush, pen, new Rect(new Point(x - R/2, y - R/2), new Size(R, R)));
        Console.WriteLine("DRAWING SQUARE");
    }
}
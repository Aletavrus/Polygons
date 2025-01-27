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
        Pen pen = new Pen(Brushes.SandyBrown, 1, lineCap:PenLineCap.Square);
        Brush brush = new SolidColorBrush(Colors.DarkRed);
        
        double diff = R * Math.Sqrt(2); 
        
        dc.DrawRectangle(brush, pen, new Rect(new Point(x-diff/2, y-diff/2), new Size(diff, diff)));
    }

    public override bool IsInside(int x, int y)
    {
        double compare = R*Math.Sqrt(2)/2;
        return (Math.Abs(X - x) <= compare & Math.Abs(Y - y) <= compare);
    }
}
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
        double diff = R * Math.Sqrt(2); 
        
        dc.DrawRectangle(Brush, Pen, new Rect(new Point(x-diff/2, y-diff/2), new Size(diff, diff)));
    }

    public override bool IsInside(int x, int y)
    {
        double compare = R*Math.Sqrt(2)/2;
        return (Math.Abs(X - x) <= compare & Math.Abs(Y - y) <= compare);
    }
}
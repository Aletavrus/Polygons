namespace Polygons;
using Avalonia;
using Avalonia.Media;
using System;

sealed class Circle : Shape
{
    public Circle(int x, int y) : base(x, y)
    {
    }
    
    public override void Draw(DrawingContext dc)
    {
        Pen pen = new Pen(Brushes.White, 1, lineCap:PenLineCap.Square);
        Brush brush = new SolidColorBrush(Colors.ForestGreen);
        
        dc.DrawEllipse(brush, pen, new Point(x, y), R, R);
    }

    public override bool IsInside(int x, int y)
    {
        if (Math.Pow(x - this.x, 2) + Math.Pow(y - this.y, 2) <= Math.Pow(R, 2))
        {
            return true;
        }
        return false;
    }
}
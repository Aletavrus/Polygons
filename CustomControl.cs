using Avalonia.Controls;
using Avalonia.Media;
using System.Collections.Generic;
using Avalonia;
using System;

namespace Polygons;

public class CustomControl: UserControl
{
    List<Shape> polygons = new List<Shape>();
    public override void Render(DrawingContext drawingContext)
    {
        foreach (Shape shape in polygons)
        {
            shape.Draw(drawingContext);
        }
    }

    public void CCLeftPressed(int x, int y) //check if something was captured
    {
        bool flag = true;
        if (polygons.Count != 0)
        {
            foreach (Shape shape in polygons)
            {
                if (shape.IsInside(x, y))
                {
                    shape.Captured = true;
                    flag = false;
                    shape.DiffX = x - shape.X;
                    shape.DiffY = y - shape.Y;
                }
            }
        }
        if (flag)
        {
            Circle triangle = new Circle(x, y);
            polygons.Add(triangle);
        }
        InvalidateVisual();
    }

    public void CCRightPressed(int x, int y)
    {
        bool flag = false;
        polygons.Reverse();
        if (polygons.Count != 0)
        {
            foreach (Shape shape in polygons)
            {
                if (shape.IsInside(x, y))
                {
                    polygons.Remove(shape);
                    flag = true;
                    break;
                }
            }

            if (flag)
            {
                InvalidateVisual();
            }
        }
    }

    public void CCMoved(int x, int y)
    {
        bool flag = false;
        if (polygons.Count != 0)
        {
            foreach (Shape shape in polygons)
            {
                if (shape.Captured)
                {
                    shape.X = x - shape.DiffX;
                    shape.Y = y - shape.DiffY;
                    flag = true;
                }
            }

            if (flag)
            {
                InvalidateVisual();
            }
        }
    }

    public void CCReleased(int x, int y)
    {
        bool flag = false;
        if (polygons.Count != 0)
        {
            foreach (Shape shape in polygons)
            {
                if (shape.Captured)
                {
                    shape.Captured = false;
                    flag = true;
                }
            }
        }

        if (flag)
        {
            InvalidateVisual();
        }
    }
}
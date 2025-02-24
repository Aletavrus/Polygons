using Avalonia.Controls;
using Avalonia.Media;
using System.Collections.Generic;
using Avalonia;
using System;
using HarfBuzzSharp;

namespace Polygons;

public class CustomControl: UserControl
{
    private List<Shape> _polygons = new List<Shape>();
    private List<Shape[]> _lines = new List<Shape[]>();
    private Pen _pen = new Pen(Brushes.LawnGreen, 10, lineCap:PenLineCap.Square);
    private bool _pointReleased = false;
    private string _shape = "Triangle";

    public override void Render(DrawingContext drawingContext)
    {
            if (_pointReleased && _polygons.Count >= 3)
            {
                foreach (Shape shape in _polygons)
                {
                    shape.isBorder = false;
                }

                _lines.Clear();
                _pointReleased = false;

                for (int i = 0; i < _polygons.Count - 1; i++)
                {
                    Point pi = new Point(_polygons[i].X, _polygons[i].Y);
                    for (int j = i + 1; j < _polygons.Count; j++)
                    {
                        Point pj = new Point(_polygons[j].X, _polygons[j].Y);
                        double k = (pi.Y - pj.Y) / (pi.X - pj.X);
                        double b = pi.Y - k * pi.X;
                        bool above = false;
                        bool below = false;
                        if (pi.X == pj.X)
                        {
                            for (int d = 0; d < _polygons.Count; d++)
                            {
                                if (d != i && d != j)
                                {
                                    Point pd = new Point(_polygons[d].X, _polygons[d].Y);
                                    if (pi.X > pd.X)
                                    {
                                        above = true;
                                    }
                                    else
                                    {
                                        below = true;
                                    }
                                }
                            }
                        }
                        else
                        {
                            for (int d = 0; d < _polygons.Count; d++)
                            {
                                if (d != i && d != j)
                                {
                                    Point pd = new Point(_polygons[d].X, _polygons[d].Y);
                                    double yd = k * pd.X + b;
                                    if (yd < pd.Y)
                                    {
                                        above = true;
                                    }
                                    else
                                    {
                                        below = true;
                                    }
                                }
                            }
                        }

                        if (above == !below)
                        {
                            _polygons[i].isBorder = true;
                            _polygons[j].isBorder = true;
                            _lines.Add(new Shape[2] { _polygons[i], _polygons[j] });
                        }
                    }
                }

                RemoveInsideBorders();

            }
            else if (_polygons.Count < 3)
            {
                _lines.Clear();
            }

            RenderContent(drawingContext);
    }

    public void CCLeftPressed(int x, int y) //check if something was captured
    {
        bool outsideShape = true;
        _pointReleased = true;
        if (_polygons.Count != 0)
        {
            foreach (Shape shape in _polygons)
            {
                if (shape.IsInside(x, y))
                {
                    shape.Captured = true;
                    outsideShape = false;
                    shape.DiffX = x - shape.X;
                    shape.DiffY = y - shape.Y;
                }
            }
        }
        if (outsideShape)
        {
            bool inside = FindLimits(x, y);
            if (inside) //check if we are inside a whole polygon
            {
                foreach (Shape shape in _polygons)
                {
                    shape.Captured = true;
                    shape.DiffX = x - shape.X;
                    shape.DiffY = y - shape.Y;
                }
            }
            else
            {
                switch (_shape)
                {
                    case "Triangle":
                        _polygons.Add(new Triangle(x, y));
                        break;
                    case "Square":
                        _polygons.Add(new Square(x, y));
                        break;
                    case "Circle":
                        _polygons.Add(new Circle(x, y));
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }
        }
        InvalidateVisual();
    }

    public void CCRightPressed(int x, int y)
    {
        bool insideShape = false;
        _pointReleased = true;
        _polygons.Reverse();
        if (_polygons.Count != 0)
        {
            foreach (Shape shape in _polygons)
            {
                if (shape.IsInside(x, y))
                {
                    _polygons.Remove(shape);
                    insideShape = true;
                    break;
                }
            }
            _polygons.Reverse();
            if (insideShape)
            {
                InvalidateVisual();
            }
        }
    }

    public void CCMoved(int x, int y)
    {
        bool shapeMoving = false;
        if (_polygons.Count != 0)
        {
            foreach (Shape shape in _polygons)
            {
                if (shape.Captured)
                {
                    shape.X = x - shape.DiffX;
                    shape.Y = y - shape.DiffY;
                    shapeMoving = true;
                }
            }

            if (shapeMoving)
            {
                InvalidateVisual();
            }
        }
    }

    public void CCReleased(int x, int y)
    {
        bool flag = false;
        _pointReleased = true;
        if (_polygons.Count != 0)
        {
            foreach (Shape shape in _polygons)
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

    private void RenderContent(DrawingContext drawingContext)
    {
        foreach (Shape[] line in _lines)
        {
            drawingContext.DrawLine(_pen, new Point(line[0].X, line[0].Y), new Point(line[1].X, line[1].Y));
        }
        foreach (Shape shape in _polygons)
        {
            shape.Draw(drawingContext);
        }
    }

    private void RemoveInsideBorders()
    {
        List<Shape> shapesRemove = new List<Shape>();
        foreach (Shape shape in _polygons)
        {
            if (!shape.isBorder)
            {
                shapesRemove.Add(shape);
            }
        }
        foreach (Shape shape in shapesRemove)
        {
            _polygons.Remove(shape);
        }
    }

    public void SetShape(string menuShape)
    {
        _shape = menuShape;
    }

    private bool FindLimits(int x, int y)
    {
        bool inside = false;
        foreach (Shape[] line in _lines)
        {
            Point p1 = new Point(line[0].X, line[0].Y);
            Point p2 = new Point(line[1].X, line[1].Y);
    
            if (y >= Math.Min(p1.Y, p2.Y))
            {
                if (y <= Math.Max(p1.Y, p2.Y))
                {
                    if (x <= Math.Max(p1.X, p2.X))
                    {
                        double xIntersection = (y - p1.Y) * (p2.X - p1.X) / (p2.Y - p1.Y) + p1.X;

                        if (Convert.ToInt16(p1.X) == Convert.ToInt16(p2.X) || x <= xIntersection)
                        {
                            inside = !inside;
                        }
                    }
                }
            }
        }

        return inside;
    }
}
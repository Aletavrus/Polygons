using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using System.Collections.Generic;
using System;

namespace Polygons;

public class CustomControl: UserControl
{
    private List<Shape> _polygons = new List<Shape>();
    private List<Shape[]> _lines = new List<Shape[]>();
    private Pen _pen = new Pen(Brushes.LawnGreen, 10, lineCap:PenLineCap.Square);
    private bool _pointReleased = false;
    private string _shape = "Triangle";
    private string _algorithm = "Jarvis";

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
            
            switch (_algorithm)
            {
                case "Jarvis":
                    Jarvis();
                    break;
                case "Default":
                    Default();
                    break;
                default:
                    throw new Exception("Unknown Algorithm");
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
            bool inside = IsInsidePolygon(x, y);
            if (inside)
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

    public void SetAlgorithm(string menuAlgorithm)
    {
        if (menuAlgorithm == "Chart")
        {
            new ChartWindow().Show();
            _algorithm = "Jarvis";
            return;
        }
        _algorithm = menuAlgorithm;
    }

    private bool IsInsidePolygon(int x, int y)
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

    private void Jarvis()
    {
        int constA = FindA();
        int A = constA;
        int B = 0;
        int C = 0;

        Shape tempShape = (Shape)_polygons[A].Clone();
        tempShape.X = _polygons[A].X + 100;
        C = FindCos(_polygons[A], tempShape);
        
        _lines.Add([_polygons[A], _polygons[C]]);
        _polygons[A].isBorder = true;
        _polygons[C].isBorder = true;
        B = A;
        A = C;
        
        int index = 1;
        while (index < _polygons.Count)
        {
            C = FindCos(_polygons[A], _polygons[B]);
            _lines.Add([_polygons[A], _polygons[C]]);
            _polygons[A].isBorder = true;
            _polygons[C].isBorder = true;
            
            B = A;
            A = C;
            index++;
        }
    }

    private int FindA()
    {
        Shape A = null;
        foreach (Shape shape in _polygons)
        {
            if (A is null)
            {
                A = shape;
            }
            else
            {
                if (shape.Y > A.Y)
                {
                    A = shape;
                }
                else if (shape.Y == A.Y)
                {
                    if (shape.X > A.X)
                    {
                        A = shape;
                    }
                }
            }
        }
        return _polygons.IndexOf(A);
    }

    private int FindCos(Shape A, Shape B)
    {
        double minCos = 2;
        Shape point = null;

        foreach (Shape s in _polygons)
        {
            if (point is null)
            {
                point = s;
            }
            if (s != A && s != B) // Исключаем точки A и B
            {
                double AC = Math.Sqrt(Math.Pow((A.X - s.X), 2) + Math.Pow((A.Y - s.Y), 2));
                double AB = Math.Sqrt(Math.Pow((B.Y - A.Y), 2) + Math.Pow((B.X - A.X), 2));
                double nowCos = ((s.X - A.X) * (B.X - A.X) + (s.Y - A.Y) * (B.Y - A.Y)) / (AB * AC);

                if (nowCos < minCos)
                {
                    minCos = nowCos;
                    point = s;
                }
                else if (nowCos == minCos)
                {
                    double distCurrent = Math.Pow((point.X - A.X), 2) + Math.Pow((point.Y - A.Y), 2);
                    double distNew = Math.Pow((s.X - A.X), 2) + Math.Pow((s.Y - A.Y), 2);

                    if (distNew > distCurrent)
                    {
                        point = s;
                    }
                }
            }
        }

        return _polygons.IndexOf(point);
    }

    private void Default()
    { 
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
    }
    
    public void UpdateRadius(object? sender, RadiusEventArgs e)
    {
        Shape.R = e.R;
        InvalidateVisual();
    }

    public void UpdateColor(object? sender, ColorEventArgs e)
    {
        Shape.Brush = new SolidColorBrush(e.Color);
        InvalidateVisual();
    }
}
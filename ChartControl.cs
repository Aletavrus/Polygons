using Avalonia.Controls;
using System.Collections.Generic;
using Avalonia;
using System;
using Avalonia.Media;
namespace Polygons;

public class ChartControl: UserControl
{
    public override void Render(DrawingContext drawingContext)
    {
        GraphContent(drawingContext, GenerateData());
    }
    
    private List<Point[]> GenerateData()
    {
        List<int[]> times = new List<int[]>();
        List<Shape> testPolygons = new List<Shape>();
        List<Shape> testPolygons2 = new List<Shape>();
        for (int i = 10; i<100; i+=10)
        {
            for (int j = 1; j < i; j++)
            {
                testPolygons.Add(GenerateShape());
            }

            int timeJarvis = 0;
            int timeDefault = 0;
            testPolygons2 = testPolygons;
            DateTime startTime = DateTime.Now;
            GraphJarvis(testPolygons);
            timeJarvis = (int)(DateTime.Now - startTime).TotalMilliseconds;
            startTime = DateTime.Now;
            GraphDefault(testPolygons2);
            timeDefault = (int)(DateTime.Now - startTime).TotalMilliseconds;
            times.Add(new int[]{timeJarvis, timeDefault});
        }
        List<Point[]> graphLines = new List<Point[]>();
        int yJarvis = 500;
        int yByDef = 500;
        int x = 20;
        foreach (int[] point in times)
        {
            graphLines.Add(new Point[] { new Point(x , yJarvis), new Point(x + 40, yJarvis - point[0] )});
            graphLines.Add(new Point[] { new Point(x , yByDef), new Point(x + 40, yByDef - point[1]  )});
            yJarvis -= point[0];
            yByDef -= point[1];
            x += 40;
        }
        return graphLines;
    }
    
    private void GraphContent(DrawingContext drawingContext, List<Point[]> graphLines)
    {
        int index = 0;
        foreach (Point[] line in graphLines)
        {
            if (index % 2 == 0)
            {
                drawingContext.DrawLine(new Pen(Brushes.DeepPink, 3, lineCap: PenLineCap.Square), new Point(line[0].X, line[0].Y), new Point(line[1].X, line[1].Y));
            }
            else
            {
                drawingContext.DrawLine(new Pen(Brushes.DeepSkyBlue, 3, lineCap:PenLineCap.Square), new Point(line[0].X, line[0].Y), new Point(line[1].X, line[1].Y));
            }
            index++;
        }
    }

    private Shape GenerateShape()
    {
        int ind = Random.Shared.Next(0, 3);
        Shape shp;
        switch (ind)
        {
            case 0:
                shp = new Triangle(Random.Shared.Next(), Random.Shared.Next());
                break;
            case 1:
                shp = new Circle(Random.Shared.Next(), Random.Shared.Next());
                break;
            default:
                shp = new Square(Random.Shared.Next(), Random.Shared.Next());
                break;
        }
        return shp;
    }

    private void GraphJarvis(List<Shape> polygons)
    {
        int constA = FindA(polygons);
        int A = constA;
        int B = 0;
        int C = 0;

        Shape tempShape = (Shape)polygons[A].Clone();
        tempShape.X = polygons[A].X + 100;
        C = FindCos(polygons[A], tempShape, polygons);
        
        polygons[A].isBorder = true;
        polygons[C].isBorder = true;
        B = A;
        A = C;
        
        int index = 1;
        while (index < polygons.Count)
        {
            C = FindCos(polygons[A], polygons[B], polygons);
            polygons[A].isBorder = true;
            polygons[C].isBorder = true;
            
            B = A;
            A = C;
            index++;
        }
    }

    private int FindA(List<Shape> polygons)
    {
        Shape A = null;
        foreach (Shape shape in polygons)
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
        return polygons.IndexOf(A);
    }
    
    private int FindCos(Shape A, Shape B, List<Shape> polygons)
    {
        double minCos = 2;
        Shape point = null;

        foreach (Shape s in polygons)
        {
            if (point is null)
            {
                point = s;
            }
            if (s != A && s != B)
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

        return polygons.IndexOf(point);
    }

    private void GraphDefault(List<Shape> polygons)
    { 
        for (int i = 0; i < polygons.Count - 1; i++)
        {
            Point pi = new Point(polygons[i].X, polygons[i].Y);
            for (int j = i + 1; j < polygons.Count; j++)
            {
                Point pj = new Point(polygons[j].X, polygons[j].Y);
                double k = (pi.Y - pj.Y) / (pi.X - pj.X);
                double b = pi.Y - k * pi.X;
                bool above = false;
                bool below = false;
                if (pi.X == pj.X)
                {
                    for (int d = 0; d < polygons.Count; d++)
                    {
                        if (d != i && d != j)
                        {
                            Point pd = new Point(polygons[d].X, polygons[d].Y);
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
                    for (int d = 0; d < polygons.Count; d++)
                    {
                        if (d != i && d != j)
                        {
                            Point pd = new Point(polygons[d].X, polygons[d].Y);
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
                    polygons[i].isBorder = true;
                    polygons[j].isBorder = true;
                }
            }
        }
    }
}
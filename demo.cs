using Avalonia.Controls;
using Avalonia.Media;
using Avalonia;
using System;

namespace Polygons;

public class CustomControl: UserControl
{
    public override void Render(DrawingContext drawingContext)
    {
        Square sq = new Square(100, 100);
        sq.Draw(drawingContext);
        
        Circle circle = new Circle(500, 125);
        circle.Draw(drawingContext);
        
        Triangle triangle = new Triangle(300, 390);
        triangle.Draw(drawingContext);
    }
}
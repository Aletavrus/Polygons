using Avalonia.Controls;
using Avalonia.Media;
using Avalonia;
using System;

namespace Polygons;

public class CustomControl: UserControl
{
    public override void Render(DrawingContext drawingContext)
    {
        
        Circle circle = new Circle(100, 100);
        circle.Draw(drawingContext);
        
        Square sq = new Square(100, 100);
        sq.Draw(drawingContext);
        
        // Triangle triangle = new Triangle(100, 100);
        // triangle.Draw(drawingContext);
    }
}
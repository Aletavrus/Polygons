﻿using System;
using System.Text.Json.Serialization;
using Avalonia.Media;

namespace Polygons;

[JsonDerivedType(typeof(Triangle), "Triangle")]
[JsonDerivedType(typeof(Circle), "Circle")]
[JsonDerivedType(typeof(Square), "Square")]
abstract class Shape : ICloneable
{
    protected int x, y;
    protected bool captured = false;
    static int _r;
    private static Pen pen;
    private static Brush brush;

    public static int R
    {
        get => _r;
        set => _r = value;
    }
    [JsonInclude]
    public int X
    {
        get => x;
        set => x = value;
    }
    [JsonInclude]
    public int Y
    {
        get => y;
        set => y = value;
    }
    
    public int DiffX{get;set;}
    public int DiffY{get;set;}

    public bool Captured
    {
        get => captured;
        set => captured = value;
    }
    public bool isBorder { get; set; }

    public Pen Pen
    {
        get => pen;
        set => pen = value;
    }
    public void ChangeBorderColor(Color color)
    {
        pen.Brush = new SolidColorBrush(color, 100D);
    }
    public void ChangeThickness(int thickness)
    {
        pen.Thickness = thickness;
    }

    public static Brush Brush
    {
        get => brush;
        set
        {
            brush = value;
            pen = new Pen(brush, 3);
        }
    }
    public void ChangeFillColor(Color color)
    {
        brush = new SolidColorBrush(color);
    }
    
    protected Shape(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
    static Shape()
    {
        _r = 50;
        pen = new Pen(Brushes.SandyBrown, 2D, lineCap:PenLineCap.Square);
        brush = new SolidColorBrush(Colors.DarkRed, 100D);
    }

    public abstract void Draw(DrawingContext dc);
    public abstract bool IsInside(int x, int y);
    
    public object Clone()
    {
        return MemberwiseClone(); // Creates a shallow copy
    }
}
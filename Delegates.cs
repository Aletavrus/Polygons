using System;
using Avalonia.Controls;
using Avalonia.Media;

namespace Polygons;

public delegate void RadiusChangedHandler(object sender, RadiusEventArgs e);

public class RadiusEventArgs : EventArgs
{
    private int _r;
    public int R { get => _r; set => _r = value; }
    
    public RadiusEventArgs(int r)
    {
        _r = r;
    }
}

public delegate void ColorChangedHandler(object sender, ColorEventArgs e);

public class ColorEventArgs : EventArgs
{
    private Color _color;
    public Color Color { get => _color; set => _color = value; }
    
    public ColorEventArgs(Color color)
    {
        _color = color;
    }
}
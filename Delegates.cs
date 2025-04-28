using System;

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
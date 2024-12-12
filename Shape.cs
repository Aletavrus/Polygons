using Avalonia.Media;

namespace Polygons;

abstract class Shape
{
    protected int x, y;
    static int r;

    public int R
    {
        get => r;
    }
    public int X
    {
        get => x;
        set => x = value;
    }
    public int Y
    {
        get => y;
        set => y = value;
    }
    protected Shape(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
    static Shape()
    {
        r = 50;
    }

    public abstract void Draw(DrawingContext dc);
}
using System.Collections.Generic;
using Avalonia.Media;
using System;
using System.Text.Json.Serialization;

namespace Polygons;

class SavedState
{
    public List<SaveShape> Polygons { get; set; }
    public int R { get; set; }
    public string BrushColor { get; set; }
    public SavedState(List<SaveShape> polygons, int r, Brush brushColor)
    {
        Polygons = polygons;
        R = r;
        BrushColor = brushColor.ToString();
    }
    public SavedState(){}
}

class SaveShape
{
    public int X { get; set; }
    public int Y { get; set; }
    public string Type { get; set; }

    [JsonConstructor]
    public SaveShape(int x, int y, string type)
    {
        X = x;
        Y = y;
        Type = type;
    }
    
    public SaveShape(){}
}
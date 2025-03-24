using Avalonia;
using Avalonia.Controls;
using System.Collections.Generic;
using System;
namespace Polygons;

public partial class ChartWindow : Window
{
    public ChartWindow()
    {
        this.Width = 800;
        this.Height = 600;
        this.Title = "Graph Window";
        this.Content = new ChartControl();
    }
}
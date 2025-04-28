using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using System;

namespace Polygons;

public partial class RadiusWindow : Window
{
    public RadiusWindow(int radius)
    {
        InitializeComponent();
        this.Width = 400;
        this.Height = 100;
        slider.Value = radius;
        this.Title = "Radius Window";
    }

    public event RadiusChangedHandler RC;

    private void Slider_OnValueChanged(object? sender, RangeBaseValueChangedEventArgs e)
    {
        if (RC != null)
        {
            RC(this, new RadiusEventArgs(Convert.ToInt32(e.NewValue)));
        }
    }
}
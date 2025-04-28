using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Interactivity;

namespace Polygons
{
    public partial class ColorWindow : Window
    {
        private Color _color;
        public event ColorChangedHandler ColorChanged;
        public ColorWindow(Color color)
        {
            InitializeComponent();
            ColorPicker.Color = color;
            Title = "Color Window";
        }
        private void OkButton(object sender, RoutedEventArgs e)
        {
            _color = ColorPicker.Color;
            if (ColorChanged != null)
            {
                ColorChanged(this, new ColorEventArgs(ColorPicker.Color));
            }
            Close();
        }

        private void CancelButton(object sender, RoutedEventArgs e)
        {
            ColorPicker.Color = _color;
            Close();
        }
    }
}
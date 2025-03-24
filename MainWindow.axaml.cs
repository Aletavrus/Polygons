using System;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;

namespace Polygons;

public partial class MainWindow : Window
{
    private bool _menuClicked = false;
    public MainWindow()
    {
        InitializeComponent();
    }
    
    private void MenuClicked(object? sender, PointerPressedEventArgs e)
    {
        _menuClicked = true;
    }

    private void ItemClicked(object? sender, RoutedEventArgs e)
    {
        if (sender is MenuItem menuItem)
        {
            CustomControl cc = this.FindControl<CustomControl>("CustomControl");
            string tag = menuItem.Tag.ToString();
            string header = menuItem.Header.ToString();
            switch (tag)
            {
                case "Type":
                    cc.SetShape(header);
                    break;
                case "Algorithm":
                    Console.WriteLine(header);
                    cc.SetAlgorithm(header);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
    }

    private void MousePressed(object? sender, PointerPressedEventArgs e)
    {
        if (_menuClicked)
        {
            _menuClicked = false;
            return;
        }
        
        CustomControl cc = this.FindControl<CustomControl>("CustomControl");
        if (e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
        {
            cc.CCLeftPressed(Convert.ToInt32(e.GetPosition(cc).X), Convert.ToInt32(e.GetPosition(cc).Y));
        }
        else
        {
            cc.CCRightPressed(Convert.ToInt32(e.GetPosition(cc).X), Convert.ToInt32(e.GetPosition(cc).Y));
        }
    }

    private void MouseMoved(object? sender, PointerEventArgs e)
    {
        CustomControl cc = this.FindControl<CustomControl>("CustomControl");
        cc.CCMoved(Convert.ToInt32(e.GetPosition(cc).X), Convert.ToInt32(e.GetPosition(cc).Y));
    }

    private void MouseReleased(object? sender, PointerReleasedEventArgs e)
    { 
        CustomControl cc = this.FindControl<CustomControl>("CustomControl");
        cc.CCReleased(Convert.ToInt32(e.GetPosition(cc).X), Convert.ToInt32(e.GetPosition(cc).Y));
    }
}
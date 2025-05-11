using System;
using Avalonia.Controls;
using Avalonia;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Media;
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
                    cc.SetAlgorithm(header);
                    break;
                case "Settings":
                    bool flag = false;
                    switch (header)
                    { 
                        case "Radius":
                            flag = FindWindow("RadiusWindow");
                            if (!flag)
                            {
                                var window = new RadiusWindow(Shape.R);
                                window.RC += cc.UpdateRadius;
                                window.Show();
                            }
                            break;
                        case "Color":
                            flag = FindWindow("ColorWindow");
                            if (!flag)
                            {
                                var window = new ColorWindow(((SolidColorBrush)Shape.Brush).Color);
                                window.ColorChanged += cc.UpdateColor;
                                window.Show();
                            }
                            break;
                        default:
                            throw new NotImplementedException();
                    }
                    break;
                default:
                    throw new NotImplementedException();
                case "File":
                    switch (header)
                    {
                        case "Open":
                            cc.LoadState();
                            break;
                        case "Save":
                            cc.SaveState();
                            break;
                        default:
                            throw new NotImplementedException();
                    }
                    break;
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

    private bool FindWindow(string name)
    {
        var windows = ((IClassicDesktopStyleApplicationLifetime)Application.Current.ApplicationLifetime).Windows;
        foreach (Window wind in windows)
        {
            if (wind.Title == name)
            {
                return true;
            }
        }
        return false;
    }
}
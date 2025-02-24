    using System;
    using System.Threading.Tasks;
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
    
    private void MenuClicked(object? sender, RoutedEventArgs e)
    {
        if (sender is MenuItem menuItem)
        {
            string shape = menuItem.Header.ToString();
            _menuClicked = true;
            CustomControl cc = this.FindControl<CustomControl>("CustomControl");
            cc.SetShape(shape);
        }
    }

    private async void MousePressed(object? sender, PointerPressedEventArgs e)
    {
        await Task.Delay(100);
        
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
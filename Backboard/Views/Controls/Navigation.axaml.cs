using System;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using SukiUI.Controls;

namespace Backboard.Views.Controls;

public partial class Navigation : UserControl
{
    public delegate void MenuItemChangedEventHandler(object sender, string header);

    public static readonly StyledProperty<bool> WinUIStyleProperty =
        AvaloniaProperty.Register<SideMenu, bool>(nameof(WinUIStyle));

    public Navigation()
    {
        InitializeComponent();
    }

    public bool WinUIStyle
    {
        get => GetValue(WinUIStyleProperty);
        set
        {
            SetValue(WinUIStyleProperty, value);
            if (!value)
                return;

            var border = this.FindControl<Border>("ContentBorder");
            border.BorderThickness = new Thickness(1, 1, 0, 0);
            border.CornerRadius = new CornerRadius(13, 0, 0, 0);
        }
    }

    public event MenuItemChangedEventHandler MenuItemChanged;

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private void PaneIsClosing(object? sender, SplitViewPaneClosingEventArgs splitViewPaneClosingEventArgs)
    {
        ((SideMenuModel)DataContext).MenuVisibility = false;
    }

    private void MenuItemSelectedChanged(object sender, RoutedEventArgs e)
    {
        var rButton = (RadioButton)sender;
        if (rButton.IsChecked != true)
            return;
        try
        {
            var header = ((TextBlock)((DockPanel)((Grid)rButton.Content).Children.First()).Children.Last()).Text;
            MenuItemChanged?.Invoke(this, header);
        }
        catch (Exception exc)
        {
        }
    }
}
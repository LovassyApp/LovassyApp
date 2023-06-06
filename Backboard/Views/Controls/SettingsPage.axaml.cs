using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Backboard.Views.Controls;

public partial class SettingsPage : UserControl
{
    public SettingsPage()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
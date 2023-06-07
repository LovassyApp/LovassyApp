using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Backboard.Views.Controls;

public partial class Navigation : UserControl
{
    public Navigation()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
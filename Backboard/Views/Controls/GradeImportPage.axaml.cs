using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Backboard.Views.Controls;

public partial class GradeImportPage : UserControl
{
    public GradeImportPage()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
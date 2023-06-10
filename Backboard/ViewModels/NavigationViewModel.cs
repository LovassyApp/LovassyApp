using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Controls;
using Material.Icons;
using ReactiveUI;

namespace Backboard.ViewModels;

public class SideMenuItem
{
    public string Header { get; set; } = default;
    public MaterialIconKind Icon { get; set; } = MaterialIconKind.Circle;

    public object Content { get; set; } = new Grid();

    public List<SideMenuItem> Items { get; set; } = new();
}

public class SideMenuModel : ReactiveObject
{
    private object currentPage = new Grid();

    private List<SideMenuItem> footermenuItems = new();

    private object headerContent = new Grid();


    private bool headerContentOverlapsToggleSidebarButton;

    private ObservableCollection<SideMenuItem> menuItems = new();

    private bool menuvisibility = true;

    public SideMenuModel()
    {
        Task.Run(() =>
        {
            // Not proud of this but here we go
            Thread.Sleep(1500);
            ChangePage(MenuItems.First().Content);
        });
    }

    public bool MenuVisibility
    {
        get => menuvisibility;
        set
        {
            this.RaiseAndSetIfChanged(ref menuvisibility, value);
            this.RaisePropertyChanged("SpacerEnabled");
        }
    }

    public object CurrentPage
    {
        get => currentPage;
        set => this.RaiseAndSetIfChanged(ref currentPage, value);
    }

    public object HeaderContent
    {
        get => headerContent;
        set => this.RaiseAndSetIfChanged(ref headerContent, value);
    }

    public ObservableCollection<SideMenuItem> MenuItems
    {
        get => menuItems;
        set => this.RaiseAndSetIfChanged(ref menuItems, value);
    }

    public List<SideMenuItem> FooterMenuItems
    {
        get => footermenuItems;
        set => this.RaiseAndSetIfChanged(ref footermenuItems, value);
    }

    /// <summary>
    ///     Defines if header content can overlap sidebar visibility button.
    ///     If true - they can take the same spot in the UI, which can lead to bugs when the content is too wide.
    ///     If false - header content moves below the sidebar button.
    ///     Default is true.
    /// </summary>
    public bool HeaderContentOverlapsToggleSidebarButton
    {
        get => headerContentOverlapsToggleSidebarButton;
        set => this.RaiseAndSetIfChanged(ref headerContentOverlapsToggleSidebarButton, value);
    }

    /// <summary>
    ///     Defines if element that moves menu buttons down is enabled.
    /// </summary>
    // Property name must be equal to string inside MenuVisibility property.
    private bool SpacerEnabled => HeaderContentOverlapsToggleSidebarButton && !MenuVisibility;

    public int HeaderMinHeight => HeaderContentOverlapsToggleSidebarButton ? 40 : 0;

    public void ChangeMenuVisibility()
    {
        MenuVisibility = !MenuVisibility;
    }


    public void ChangePage(object o)
    {
        CurrentPage = o;
    }
}
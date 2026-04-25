using Avalonia.Controls;
using LHGServerManager.ViewModels;

namespace LHGServerManager.Views;

public partial class SettingsView : UserControl
{
    public SettingsView()
    {
        InitializeComponent();
        DataContext = new SettingsViewModel();
    }
}

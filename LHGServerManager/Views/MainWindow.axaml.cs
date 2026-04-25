using Avalonia.Controls;
using LHGServerManager.ViewModels;

namespace LHGServerManager.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new MainWindowViewModel();
    }
}

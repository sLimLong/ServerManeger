using Avalonia.Controls;
using My7DTDManager.ViewModels;

namespace My7DTDManager.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new MainWindowViewModel();
    }
}

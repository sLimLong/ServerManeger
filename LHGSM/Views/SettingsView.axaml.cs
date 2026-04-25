using Avalonia.Controls;

namespace LHGSM.Views;

public partial class SettingsView : UserControl
{
    public SettingsView()
    {
        InitializeComponent();
        // НЕ создавайте DataContext здесь - он уже установлен через MainWindow
    }
}
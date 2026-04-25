using ReactiveUI;

namespace My7DTDManager.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    public DashboardViewModel Dashboard { get; } = new();
    public SettingsViewModel Settings { get; } = new();
}

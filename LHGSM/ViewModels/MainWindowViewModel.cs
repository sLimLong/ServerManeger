using ReactiveUI;
using System.Collections.ObjectModel;
using System.Reactive;
using System.IO;
using LHGSM.Services;

namespace LHGSM.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    public DashboardViewModel DashboardVM { get; }
    public SettingsViewModel SettingsVM { get; }

    public MainWindowViewModel()
    {
        DashboardVM = new DashboardViewModel();
        SettingsVM = new SettingsViewModel(); // ← БЕЗ аргументов!
    }
}

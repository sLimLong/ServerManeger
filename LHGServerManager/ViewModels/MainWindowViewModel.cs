namespace LHGServerManager.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    public DashboardViewModel DashboardVM { get; } = new();
    public ConsoleViewModel ConsoleVM { get; } = new();
    public PlayersViewModel PlayersVM { get; } = new();
    public ConfigViewModel ConfigVM { get; } = new();
    public ModsViewModel ModsVM { get; } = new();
    public BackupsViewModel BackupsVM { get; } = new();
    public LogsViewModel LogsVM { get; } = new();
    public SettingsViewModel SettingsVM { get; } = new();
}



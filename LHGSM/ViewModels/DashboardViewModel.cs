using ReactiveUI;
using System.Collections.ObjectModel;
using System.Reactive;
using System.IO;
using LHGSM.Services;

namespace LHGSM.ViewModels;

public class DashboardViewModel : ViewModelBase
{
    private readonly ServerProcessService _server;

    public ObservableCollection<string> Log { get; } = new();

    public ReactiveCommand<Unit, Unit> StartCommand { get; }
    public ReactiveCommand<Unit, Unit> StopCommand { get; }

    private string _status = "Stopped";
    public string Status
    {
        get => _status;
        set => this.RaiseAndSetIfChanged(ref _status, value);
    }

    public DashboardViewModel()
    {
        _server = new ServerProcessService();

        _server.OnOutput += msg => Log.Add(msg);
        _server.OnError += msg => Log.Add("[ERROR] " + msg);
        _server.OnStopped += () => Status = "Stopped";

        StartCommand = ReactiveCommand.Create(StartServer);
        StopCommand = ReactiveCommand.Create(() => _server.Stop());
    }

    private void StartServer()
    {
        var settings = SettingsService.Load();

        Log.Add($"[DEBUG] Loaded ServerFolder = '{settings.ServerFolder}'");

        var exe = Path.Combine(settings.ServerFolder, "7DaysToDieServer.exe");

        Log.Add($"[DEBUG] exe = '{exe}'");
        Log.Add($"[DEBUG] exists = {File.Exists(exe)}");

        if (!File.Exists(exe))
        {
            Log.Add("[ERROR] Server executable not found.");
            return;
        }

        Status = "Starting";

        if (_server.Start(exe, settings.ServerFolder))
            Status = "Running";
        else
            Status = "Error";
    }
}

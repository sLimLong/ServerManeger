using ReactiveUI;
using System.Reactive;
using System.Threading.Tasks;
using System.IO;
using My7DTDManager.Services;

namespace My7DTDManager.ViewModels;

public class DashboardViewModel : ViewModelBase
{
    private readonly ServerProcessService _serverService = new();

    private string _serverStatus = "Stopped";
    private string _lastOutput = string.Empty;

    public string ServerStatus
    {
        get => _serverStatus;
        set => this.RaiseAndSetIfChanged(ref _serverStatus, value);
    }

    public string LastOutput
    {
        get => _lastOutput;
        set => this.RaiseAndSetIfChanged(ref _lastOutput, value);
    }

    public ReactiveCommand<Unit, Unit> StartServerCommand { get; }
    public ReactiveCommand<Unit, Unit> StopServerCommand { get; }
    public ReactiveCommand<Unit, Unit> RestartServerCommand { get; }

    public DashboardViewModel()
    {
        _serverService.OutputReceived += line => LastOutput = line;
        _serverService.Exited += code => ServerStatus = $"Exited ({code})";

        StartServerCommand = ReactiveCommand.Create(StartServer);
        StopServerCommand = ReactiveCommand.Create(StopServer);
        RestartServerCommand = ReactiveCommand.CreateFromTask(RestartServerAsync);
    }

    private void StartServer()
    {
        if (_serverService.IsRunning)
        {
            ServerStatus = "Already running";
            return;
        }

        var settings = new SettingsService().Load();
        var exePath = settings.ServerExePath;

        if (string.IsNullOrWhiteSpace(exePath) || !File.Exists(exePath))
        {
            ServerStatus = "Invalid server path";
            return;
        }

        ServerStatus = "Starting...";
        _serverService.Start(exePath);
        ServerStatus = "Running";
    }

    private void StopServer()
    {
        if (!_serverService.IsRunning)
        {
            ServerStatus = "Already stopped";
            return;
        }

        ServerStatus = "Stopping...";
        _serverService.Stop();
        ServerStatus = "Stopped";
    }

    private async Task RestartServerAsync()
    {
        StopServer();
        await Task.Delay(1000);
        StartServer();
    }
}

using ReactiveUI;
using System.Reactive;
using My7DTDManager.Models;
using My7DTDManager.Services;
using Avalonia.Controls;

namespace My7DTDManager.ViewModels;

public class SettingsViewModel : ViewModelBase
{
    private readonly SettingsService _settingsService = new();
    private AppSettings _settings;

    public string ServerExePath
    {
        get => _settings.ServerExePath;
        set
        {
            _settings.ServerExePath = value;
            this.RaisePropertyChanged();
        }
    }

    public ReactiveCommand<Unit, Unit> BrowseCommand { get; }
    public ReactiveCommand<Unit, Unit> SaveCommand { get; }

    public SettingsViewModel()
    {
        _settings = _settingsService.Load();

        BrowseCommand = ReactiveCommand.CreateFromTask(OpenFileDialogAsync);
        SaveCommand = ReactiveCommand.Create(Save);
    }

    private async Task OpenFileDialogAsync()
    {
        var dialog = new OpenFileDialog
        {
            Title = "Select 7DaysToDieServer.exe",
            AllowMultiple = false,
            Filters =
            {
                new FileDialogFilter { Name = "Executable", Extensions = { "exe" } }
            }
        };

        var window = App.Current!.ApplicationLifetime!
            .GetType()
            .GetProperty("MainWindow")!
            .GetValue(App.Current!.ApplicationLifetime) as Window;

        var result = await dialog.ShowAsync(window);

        if (result != null && result.Length > 0)
            ServerExePath = result[0];
    }

    private void Save()
    {
        _settingsService.Save(_settings);
    }
}

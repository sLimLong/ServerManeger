using ReactiveUI;
using System.Reactive;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Platform.Storage;
using Avalonia.Threading;
using LHGSM.Services;

namespace LHGSM.ViewModels;

public class SettingsViewModel : ViewModelBase
{
    private readonly SettingsService _settings;

    public string ServerFolder
    {
        get => _settings.ServerFolder;
        set
        {
            _settings.ServerFolder = value;
            this.RaisePropertyChanged();
            _settings.Save();
        }
    }

    public bool AutoRestart
    {
        get => _settings.AutoRestart;
        set
        {
            _settings.AutoRestart = value;
            this.RaisePropertyChanged();
            _settings.Save();
        }
    }

    public ReactiveCommand<Unit, Unit> BrowseFolderCommand { get; }

    public SettingsViewModel()
    {
        _settings = SettingsService.Load();

        BrowseFolderCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            await Dispatcher.UIThread.InvokeAsync(async () =>
            {
                var lifetime = Application.Current?.ApplicationLifetime
                    as IClassicDesktopStyleApplicationLifetime;

                var mainWindow = lifetime?.MainWindow;
                if (mainWindow == null)
                    return;

                // Avalonia 12 — обязательно нужны options
                var options = new FolderPickerOpenOptions
                {
                    Title = "Select server folder",
                    AllowMultiple = false
                };

                var folders = await mainWindow.StorageProvider.OpenFolderPickerAsync(options);

                if (folders != null && folders.Count > 0)
                {
                    ServerFolder = folders[0].Path.LocalPath;
                }
            });
        });
    }
}

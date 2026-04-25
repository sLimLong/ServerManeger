using ReactiveUI;
using System.Reactive;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Platform.Storage;  // ← ДОБАВИТЬ ЭТУ СТРОКУ
using LHGServerManager.Services;
using System.IO;

namespace LHGServerManager.ViewModels;

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
            var lifetime = Application.Current?.ApplicationLifetime 
                as IClassicDesktopStyleApplicationLifetime;

            var mainWindow = lifetime?.MainWindow;

            if (mainWindow == null)
                return;

            var folders = await mainWindow.StorageProvider.OpenFolderPickerAsync(
                new FolderPickerOpenOptions
                {
                    Title = "Select server folder",
                    AllowMultiple = false
                });

            if (folders != null && folders.Count > 0)
            {
                ServerFolder = folders[0].Path.LocalPath;
            }
        });
    }
}
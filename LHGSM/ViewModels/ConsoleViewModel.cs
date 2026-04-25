using ReactiveUI;
using System;  // ← Добавьте этот using

namespace LHGSM.ViewModels;

public class ConsoleViewModel : ViewModelBase
{
    private string _consoleText = string.Empty;
    
    public string ConsoleText
    {
        get => _consoleText;
        set => this.RaiseAndSetIfChanged(ref _consoleText, value);
    }
    
    public void AddMessage(string message)
    {
        var timestamp = DateTime.Now.ToString("HH:mm:ss");
        ConsoleText += $"[{timestamp}] {message}\n";
    }
    
    public void ClearConsole()
    {
        ConsoleText = string.Empty;
    }
}
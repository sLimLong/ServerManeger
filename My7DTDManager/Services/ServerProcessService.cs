using System;
using System.Diagnostics;

namespace My7DTDManager.Services;

public class ServerProcessService
{
    private Process? _process;

    public bool IsRunning => _process is { HasExited: false };

    public event Action<string>? OutputReceived;
    public event Action<string>? ErrorReceived;
    public event Action<int>? Exited;

    public void Start(string exePath, string? arguments = null, string? workingDir = null)
    {
        if (IsRunning)
            return;

        var psi = new ProcessStartInfo
        {
            FileName = exePath,
            Arguments = arguments ?? string.Empty,
            WorkingDirectory = workingDir ?? System.IO.Path.GetDirectoryName(exePath)!,
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            CreateNoWindow = true
        };

        _process = new Process { StartInfo = psi, EnableRaisingEvents = true };

        _process.OutputDataReceived += (_, e) =>
        {
            if (!string.IsNullOrEmpty(e.Data))
                OutputReceived?.Invoke(e.Data);
        };

        _process.ErrorDataReceived += (_, e) =>
        {
            if (!string.IsNullOrEmpty(e.Data))
                ErrorReceived?.Invoke(e.Data);
        };

        _process.Exited += (_, _) =>
        {
            var code = _process!.ExitCode;
            _process = null;
            Exited?.Invoke(code);
        };

        _process.Start();
        _process.BeginOutputReadLine();
        _process.BeginErrorReadLine();
    }

    public void Stop()
    {
        if (!IsRunning)
            return;

        try
        {
            _process!.CloseMainWindow();
            if (!_process.WaitForExit(5000))
                _process.Kill(true);
        }
        catch
        {
            // логирование по желанию
        }
        finally
        {
            _process = null;
        }
    }
}

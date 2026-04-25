using System;
using System.Diagnostics;
using System.IO;

namespace LHGSM.Services;

public class ServerProcessService
{
    private Process? _process;

    public event Action<string>? OnOutput;
    public event Action<string>? OnError;
    public event Action? OnStopped;

    public bool IsRunning => _process != null && !_process.HasExited;

    public bool Start(string exePath, string workingDir)
    {
        try
        {
            OnOutput?.Invoke($"[DEBUG] Start() exePath = {exePath}");
            OnOutput?.Invoke($"[DEBUG] workingDir = {workingDir}");
            OnOutput?.Invoke($"[DEBUG] File.Exists(exePath) = {File.Exists(exePath)}");

            if (!File.Exists(exePath))
            {
                OnError?.Invoke($"Executable not found: {exePath}");
                return false;
            }

            var logFile = Path.Combine(workingDir, "output_log_manager.txt");

            var args =
                $"-logfile \"{logFile}\" " +
                "-quit -batchmode -nographics " +
                "-configfile=serverconfig.xml " +
                "-dedicated";

            OnOutput?.Invoke($"[DEBUG] Arguments = {args}");

            var psi = new ProcessStartInfo
            {
                FileName = exePath,
                WorkingDirectory = workingDir,
                Arguments = args,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };

            _process = new Process
            {
                StartInfo = psi,
                EnableRaisingEvents = true
            };

            _process.OutputDataReceived += (_, e) =>
            {
                if (!string.IsNullOrWhiteSpace(e.Data))
                    OnOutput?.Invoke(e.Data);
            };

            _process.ErrorDataReceived += (_, e) =>
            {
                if (!string.IsNullOrWhiteSpace(e.Data))
                    OnError?.Invoke(e.Data);
            };

            _process.Exited += (_, _) =>
            {
                OnOutput?.Invoke("[DEBUG] Process exited");
                OnStopped?.Invoke();
            };

            var started = _process.Start();

            OnOutput?.Invoke($"[DEBUG] Process.Start() returned {started}");

            _process.BeginOutputReadLine();
            _process.BeginErrorReadLine();

            return started;
        }
        catch (Exception ex)
        {
            OnError?.Invoke($"Failed to start server: {ex.Message}");
            return false;
        }
    }

    public void Stop()
    {
        try
        {
            if (IsRunning)
            {
                OnOutput?.Invoke("[DEBUG] Killing process...");
                _process?.Kill();
            }
        }
        catch (Exception ex)
        {
            OnError?.Invoke($"Failed to stop server: {ex.Message}");
        }
    }
}

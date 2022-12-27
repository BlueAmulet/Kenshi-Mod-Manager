using Core;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows;

namespace KenshiModTool
{
    public static class CommonService
    {
        internal static void StartGame()
        {
            ProcessStartInfo psi;
            if (LoadService.config.SteamModsPath != "NONE")
            {
                psi = new ProcessStartInfo
                {
                    FileName = "steam://rungameid/233860",
                    UseShellExecute = true
                };
            }
            else
            {
                string ExePath = Path.Combine(LoadService.config.GamePath, "kenshi_x64.exe");
                if (!File.Exists(ExePath))
                {
                    ExePath = Path.Combine(LoadService.config.GamePath, "kenshi_GOG_x64.exe");
                }
                if (!File.Exists(ExePath))
                {
                    MessageBox.Show("Can't find the game's exe, check Game Path in Configuration.");
                    return;
                }
                psi = new ProcessStartInfo
                {
                    FileName = ExePath,
                    WorkingDirectory = LoadService.config.GamePath
                };
            }
            Process.Start(psi);
        }

        internal static void StartFCS()
        {
            var psi = new ProcessStartInfo
            {
                FileName = Path.Combine(LoadService.config.GamePath, "forgotten construction set.exe"),
                WorkingDirectory = LoadService.config.GamePath
            };
            Process.Start(psi);
        }

        internal static void OpenFolder(string path, Action action)
        {
            if (Directory.Exists(path))
            {
                var psi = new ProcessStartInfo
                {
                    FileName = path,
                    UseShellExecute = true
                };
                Process.Start(psi);
            }
            else
            {
                action();
            }
        }
    }
}
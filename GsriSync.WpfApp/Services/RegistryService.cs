using Microsoft.Win32;
using System;

namespace GsriSync.WpfApp.Services
{
    internal class RegistryService
    {
        public string Arma3Path => ReadLocalMachine32(@"software\bohemia interactive\arma 3", "main");

        public string SteamPath => ReadLocalMachine32(@"software\valve\steam", "InstallPath");

        public string TeamspeakPath => ReadLocalMachine64(@"software\TeamSpeak 3 Client", string.Empty);

        public void CleanupOldKeys()
        {
            try
            {
                RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Registry64)
                    .DeleteSubKeyTree(@"SOFTWARE\GSRI");
            }
            catch (Exception) { }
        }

        public string ReadLocalMachine32(string path, string valueName)
        {
            return RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32)
                .OpenSubKey(path)
                ?.GetValue(valueName)
                ?.ToString();
        }

        public string ReadLocalMachine64(string path, string valueName)
        {
            return RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64)
                .OpenSubKey(path)
                ?.GetValue(valueName)
                ?.ToString();
        }
    }
}

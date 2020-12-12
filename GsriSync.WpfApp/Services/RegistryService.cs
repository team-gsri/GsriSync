using GsriSync.WpfApp.Repositories.Errors;
using Microsoft.Win32;
using System;

namespace GsriSync.WpfApp.Services
{
    internal class RegistryService
    {
        private const string TEAMSPEAK_PATH = @"software\TeamSpeak 3 Client";

        public string Arma3Path => ReadRegistry(RegistryHive.LocalMachine, RegistryView.Registry32, @"software\bohemia interactive\arma 3", "main");

        public string SteamPath => ReadRegistry(RegistryHive.LocalMachine, RegistryView.Registry32, @"software\valve\steam", "InstallPath");

        public string TeamspeakPath => ReadRegistry(RegistryHive.LocalMachine, RegistryView.Registry64, TEAMSPEAK_PATH, string.Empty)
            ?? ReadRegistry(RegistryHive.LocalMachine, RegistryView.Registry32, TEAMSPEAK_PATH, string.Empty)
            ?? ReadRegistry(RegistryHive.CurrentUser, RegistryView.Registry64, TEAMSPEAK_PATH, string.Empty)
            ?? ReadRegistry(RegistryHive.CurrentUser, RegistryView.Registry32, TEAMSPEAK_PATH, string.Empty);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Minor Vulnerability", "S2486:Generic exceptions should not be ignored", Justification = "This feature is for backward compatibility")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S108:Nested blocks of code should not be left empty", Justification = "This feature is for backward compatibility")]
        public void CleanupOldKeys()
        {
            try
            {
                RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Registry64)
                    .DeleteSubKeyTree(@"SOFTWARE\GSRI");
            }
            catch (Exception) { }
        }

        public void VerifyThirdParties()
        {
            if (string.IsNullOrEmpty(SteamPath))
            {
                throw new RepositoryException<ThirdPartyErrors>(ThirdPartyErrors.SteamMissing);
            }
            if (string.IsNullOrEmpty(Arma3Path))
            {
                throw new RepositoryException<ThirdPartyErrors>(ThirdPartyErrors.ArmaMissing);
            }
            if (string.IsNullOrEmpty(TeamspeakPath))
            {
                throw new RepositoryException<ThirdPartyErrors>(ThirdPartyErrors.TeamspeakMissing);
            }
        }

        private string ReadRegistry(RegistryHive hive, RegistryView view, string path, string value)
        {
            return RegistryKey.OpenBaseKey(hive, view).OpenSubKey(path)?.GetValue(value)?.ToString();
        }
    }
}

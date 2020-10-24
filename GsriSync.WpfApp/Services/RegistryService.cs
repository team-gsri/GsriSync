using GsriSync.WpfApp.Repositories.Errors;
using Microsoft.Win32;
using System;

namespace GsriSync.WpfApp.Services
{
    internal class RegistryService
    {
        public string Arma3Path => ReadLocalMachine32(@"software\bohemia interactive\arma 3", "main");

        public string SteamPath => ReadLocalMachine32(@"software\valve\steam", "InstallPath");

        public string TeamspeakPath => ReadLocalMachine64(@"software\TeamSpeak 3 Client", string.Empty);

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
    }
}

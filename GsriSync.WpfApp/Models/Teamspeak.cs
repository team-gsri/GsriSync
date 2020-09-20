using System;
using System.Threading.Tasks;
using Windows.Storage;

namespace GsriSync.WpfApp.Models
{
    internal class Teamspeak : Expandable
    {
        private const string APPDATA = nameof(APPDATA);

        protected override string Archive => "TFAR.zip";

        protected override string ExpandPath => $@"{AppData}\TS3Client\plugins";

        private string AppData => Environment.GetEnvironmentVariable(APPDATA);

        protected async override Task<StorageFolder> GetOrMakeExpandFolderAsync(string expandPath)
        {
            return await StorageFolder.GetFolderFromPathAsync(expandPath);
        }
    }
}

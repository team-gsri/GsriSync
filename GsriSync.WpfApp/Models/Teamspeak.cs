using GsriSync.WpfApp.Services;
using System;
using System.IO;
using System.Linq;

namespace GsriSync.WpfApp.Models
{
    internal class Teamspeak : Expandable
    {
        private const string APPDATA = nameof(APPDATA);

        private const string TFAR_PREFIX = "TFAR_";

        private readonly RegistryService _registry = new RegistryService();

        public override string LocalExpandPath => $@"{AppData}\TS3Client\plugins";

        protected override string LocalDownloadPath => $@"{_registry.CurrentPath}\.dl";

        private string AppData => Environment.GetEnvironmentVariable(APPDATA);

        public override void Delete()
        {
            Directory.Delete($@"{LocalExpandPath}\radio-sounds", true);
            foreach (var file in Directory.GetFiles(LocalExpandPath).Where(filename => filename.StartsWith(TFAR_PREFIX)))
            {
                File.Delete(file);
            }
        }
    }
}

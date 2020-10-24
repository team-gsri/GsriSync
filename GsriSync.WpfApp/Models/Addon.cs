using System;

namespace GsriSync.WpfApp.Models
{
    public class Addon
    {
        public string Hash { get; set; }

        public string Name { get; set; }

        public string RemoteUrl { get; set; }

        public ulong Size { get; set; }
    }
}

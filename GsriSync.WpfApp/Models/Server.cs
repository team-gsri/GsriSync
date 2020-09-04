using System.Collections.Generic;

namespace GsriSync.WpfApp.Models
{
    internal class Server
    {
        public IEnumerable<string> Addons { get; set; } = new string[0];

        public string Hostname { get; set; }

        public string Name { get; set; }

        public int Port { get; set; }

        public string TeamspeakUrl { get; set; }
    }
}

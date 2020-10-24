using System.Collections.Generic;
using System.Linq;
using Windows.Storage;

namespace GsriSync.WpfApp.Models
{
    public class Server
    {
        public IEnumerable<string> Addons { get; set; }

        public string Hostname { get; set; }

        public string Name { get; set; }

        public uint Port { get; set; }

        public string TeamspeakUrl { get; set; }

        public string GetArgs()
        {
            var mods = string.Join(";", Addons.Select(name => @$"{ApplicationData.Current.LocalCacheFolder.Path}\{name}"));
            return $"-connect={Hostname} -port={Port} \"-mod={mods}\"";
        }
    }
}

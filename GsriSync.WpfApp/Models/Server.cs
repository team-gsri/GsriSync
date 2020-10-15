using System.Collections.Generic;
using System.Diagnostics;
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

        public void Play(string arma3Path, string customCliArgs)
        {
            var mods = string.Join(";", Addons.Select(name => @$"{ApplicationData.Current.LocalCacheFolder.Path}\{name}"));
            var args = $"-connect={Hostname} -port={Port} \"-mod={mods}\" {customCliArgs}";
            var exe = $@"{arma3Path}\arma3_x64.exe";
            Process.Start(exe, args);
        }

        public void Talk()
        {
            Process.Start(new ProcessStartInfo(TeamspeakUrl) { UseShellExecute = true, Verb = "open" });
        }
    }
}

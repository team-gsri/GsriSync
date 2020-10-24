using GsriSync.WpfApp.Models.Comparers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Windows.Storage;

namespace GsriSync.WpfApp.Models
{
    internal class Modpack
    {
        private const string MANIFEST_FILE = "manifest.json";

        public bool IsSync => new ManifestComparer().Equals(Local, Remote);

        public Manifest Local { get; private set; }

        public Manifest Remote { get; private set; }

        public Modpack(Manifest local, Manifest remote)
        {
            Local = local;
            Remote = remote;
        }

        public async Task Delete(Addon addon)
        {
            Local.Addons.Remove(addon);
            await UpdateManifest();
        }

        public async Task Download(Addon remote)
        {
            var comparer = new AddonNameComparer();
            var local = Local.Addons.FirstOrDefault(addon => comparer.Equals(addon, remote));
            Local.Addons.Remove(local);
            Local.Addons.Add(remote);
            await UpdateManifest();
        }

        public IEnumerable<Addon> GetDeleteAddons()
        {
            return Local.Addons.Except(Remote.Addons, new AddonNameComparer());
        }

        public IEnumerable<Addon> GetDownloadAddons()
        {
            return Remote.Addons.Except(Local.Addons, new AddonHashComparer());
        }

        public void Install()
        {
            Local = Remote;
        }

        private async Task UpdateManifest()
        {
            var file = await ApplicationData.Current.LocalCacheFolder.CreateFileAsync(MANIFEST_FILE, CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(file, JsonSerializer.Serialize(Local));
        }
    }
}

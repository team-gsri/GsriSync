using GsriSync.WpfApp.Models;
using System.Threading.Tasks;

namespace GsriSync.WpfApp.Repositories.ManifestProviders
{
    internal interface IAsyncManifestProvider
    {
        Task<Manifest> ProvideManifestAsync();
    }
}

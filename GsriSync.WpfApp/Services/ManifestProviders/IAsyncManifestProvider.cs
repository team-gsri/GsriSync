using GsriSync.WpfApp.Models;
using System.Threading.Tasks;

namespace GsriSync.WpfApp.Services.ManifestProviders
{
    internal interface IAsyncManifestProvider
    {
        Task<Manifest> ProvideManifestAsync();
    }
}

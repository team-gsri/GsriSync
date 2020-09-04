using GsriSync.WpfApp.Services;

namespace GsriSync.WpfApp.Models
{
    internal class Addon : Expandable
    {
        private readonly RegistryService _registry = new RegistryService();

        public override string LocalExpandPath => $@"{_registry.CurrentPath}\{Name}";

        protected override string LocalDownloadPath => $@"{_registry.CurrentPath}\.dl";
    }
}

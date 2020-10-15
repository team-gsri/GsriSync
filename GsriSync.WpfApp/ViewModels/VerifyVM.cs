using GsriSync.WpfApp.Services;
using GsriSync.WpfApp.Utils;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GsriSync.WpfApp.ViewModels
{
    internal class VerifyVM : NotifyPropertyChangedBase
    {
        private readonly ManifestService _manifest;

        private readonly NavigationService _navigation;

        private readonly RegistryService _registry;

        private Task running;

        public string Message { get; set; }

        public VerifyVM(
            NavigationService navigation,
            ManifestService manifest,
            RegistryService registry)
        {
            _navigation = navigation ?? throw new ArgumentNullException(nameof(navigation));
            _manifest = manifest ?? throw new ArgumentNullException(nameof(manifest));
            _registry = registry ?? throw new ArgumentNullException(nameof(registry));
        }

        public async Task VerifyAsync()
        {
            VerifyThirdParty();
            try
            {
                var is_sync = await _manifest.VerifyAsync();
                _navigation.NavigateAfterVerify(is_sync);
            }
            catch (Exception)
            {
                _navigation.NavigateAfterVerify(false);
            }
            finally
            {
                running = null;
            }
        }

        internal void ScheduleVerify()
        {
            running ??= Task.Factory.StartNew(VerifyAsync, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void VerifyThirdParty()
        {
            if (string.IsNullOrEmpty(_registry.SteamPath)) { Message = "Steam n'est pas installé"; NotifyPropertyChanged(nameof(Message)); throw new InvalidOperationException(); }
            if (string.IsNullOrEmpty(_registry.Arma3Path)) { Message = "Arma 3 n'est pas installé"; NotifyPropertyChanged(nameof(Message)); throw new InvalidOperationException(); }
            if (string.IsNullOrEmpty(_registry.TeamspeakPath)) { Message = "Teamspeak n'est pas installé"; NotifyPropertyChanged(nameof(Message)); throw new InvalidOperationException(); }
        }
    }
}

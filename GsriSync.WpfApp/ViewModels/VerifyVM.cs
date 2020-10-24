using GsriSync.WpfApp.Repositories.Errors;
using GsriSync.WpfApp.Services;
using GsriSync.WpfApp.Utils;
using GsriSync.WpfApp.ViewModels.Errors;
using System;
using System.Threading;
using System.Threading.Tasks;
using static GsriSync.WpfApp.Events.NavigationChangedEventArgs;

namespace GsriSync.WpfApp.ViewModels
{
    internal class VerifyVM : NotifyPropertyChangedBase
    {
        private readonly ManifestService _manifest;

        private readonly NavigationService _navigation;

        private readonly RegistryService _registry;

        private Task verify;

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
            try
            {
                _registry.VerifyThirdParties();
                await _manifest.VerifyAsync();
                _navigation.NavigateTo(Pages.Play);
            }
            catch (RepositoryException<ManifestErrors> ex)
            when (ex.Error == ManifestErrors.CannotReadLocalCacheDir)
            {
                SetErrorMessage(ErrorMessages.MSG_CANNOT_IO);
            }
            catch (RepositoryException<ManifestErrors> ex)
            when (ex.Error == ManifestErrors.CannotReadLocalCacheFile)
            {
                SetErrorMessage(ErrorMessages.MSG_CANNOT_IO);
            }
            catch (RepositoryException<ManifestErrors> ex)
            when (ex.Error == ManifestErrors.NetworkErrorDownloadingManifest)
            {
                SetErrorMessage(ErrorMessages.MSG_CANNOT_DOWNLOAD);
            }
            catch (RepositoryException<ManifestErrors> ex)
            when (ex.Error == ManifestErrors.InvalidRemoteManifest)
            {
                SetErrorMessage(ErrorMessages.MSG_CANNOT_PARSE);
            }
            catch (RepositoryException<ThirdPartyErrors> ex)
            when (ex.Error == ThirdPartyErrors.ArmaMissing)
            {
                SetErrorMessage(ErrorMessages.MSG_VERIFY_ARMA);
            }
            catch (RepositoryException<ThirdPartyErrors> ex)
            when (ex.Error == ThirdPartyErrors.SteamMissing)
            {
                SetErrorMessage(ErrorMessages.MSG_VERIFY_STEAM);
            }
            catch (RepositoryException<ThirdPartyErrors> ex)
            when (ex.Error == ThirdPartyErrors.TeamspeakMissing)
            {
                SetErrorMessage(ErrorMessages.MSG_VERIFY_TS);
            }
            catch (Exception ex)
            {
                SetErrorMessage($"{ErrorMessages.MSG_UNKNOWN} : {ex.Message}");
            }
            finally
            {
                verify = null;
            }
        }

        internal void ScheduleVerify()
        {
            verify ??= Task.Factory.StartNew(VerifyAsync, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void SetErrorMessage(string message)
        {
            Message = message;
            NotifyPropertyChanged(nameof(Message));
        }
    }
}

using GsriSync.WpfApp.Services;
using GsriSync.WpfApp.Utils;
using System;
using System.Collections.Generic;

namespace GsriSync.WpfApp.ViewModels
{
    internal class MainWindowsVM : NotifyPropertyChangedBase
    {
        private readonly ConfigurationVM _configuration;

        private readonly DownloadVM _download;

        private readonly InstallVM _install;

        private readonly Dictionary<NavigationService.Pages, Func<MainWindowsVM, object>> _map = new Dictionary<NavigationService.Pages, Func<MainWindowsVM, object>>
        {
            { NavigationService.Pages.Config, vm => vm._configuration },
            { NavigationService.Pages.Download, vm => vm._download },
            { NavigationService.Pages.Install, vm => vm._install },
            { NavigationService.Pages.Play, vm => vm._play },
            { NavigationService.Pages.Verify, vm => vm._verify },
        };

        private readonly NavigationService _navigation;

        private readonly PlayVM _play;

        private readonly VerifyVM _verify;

        public object CurrentPage { get; set; }

        public MenuVM Menu { get; }

        public MainWindowsVM(
            NavigationService navigation,
            ConfigurationVM configuration,
            DownloadVM download,
            InstallVM install,
            PlayVM play,
            VerifyVM verify,
            MenuVM menu)
        {
            _navigation = navigation ?? throw new ArgumentNullException(nameof(navigation));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _download = download ?? throw new ArgumentNullException(nameof(download));
            _install = install ?? throw new ArgumentNullException(nameof(install));
            _play = play ?? throw new ArgumentNullException(nameof(play));
            _verify = verify ?? throw new ArgumentNullException(nameof(verify));
            _navigation.NavigationChanged += OnNavigationChanged;
            Menu = menu ?? throw new ArgumentNullException(nameof(menu));
            CurrentPage = _verify;
        }

        private void OnNavigationChanged(object sender, NavigationService.NavigationChangedEventArgs e)
        {
            CurrentPage = _map[e.NewPage](this);
            NotifyPropertyChanged(nameof(CurrentPage));
        }
    }
}

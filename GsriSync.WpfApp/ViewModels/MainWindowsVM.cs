using GsriSync.WpfApp.Events;
using GsriSync.WpfApp.Services;
using GsriSync.WpfApp.Utils;
using System;
using System.Collections.Generic;
using static GsriSync.WpfApp.Events.NavigationChangedEventArgs;

namespace GsriSync.WpfApp.ViewModels
{
    internal class MainWindowsVM : NotifyPropertyChangedBase
    {
        private readonly ConfigurationVM _configuration;

        private readonly DownloadVM _download;

        private readonly Dictionary<Pages, Func<MainWindowsVM, object>> _map = new Dictionary<Pages, Func<MainWindowsVM, object>>
        {
            { Pages.Config, vm => vm._configuration },
            { Pages.Download, vm => vm._download },
            { Pages.Play, vm => vm._play },
            { Pages.Verify, vm => vm._verify },
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
            PlayVM play,
            VerifyVM verify,
            MenuVM menu)
        {
            _navigation = navigation ?? throw new ArgumentNullException(nameof(navigation));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _download = download ?? throw new ArgumentNullException(nameof(download));
            _play = play ?? throw new ArgumentNullException(nameof(play));
            _verify = verify ?? throw new ArgumentNullException(nameof(verify));
            Menu = menu ?? throw new ArgumentNullException(nameof(menu));
            _navigation.NavigationChanged += OnNavigationChanged;
            CurrentPage = _verify;
        }

        private void OnNavigationChanged(object sender, NavigationChangedEventArgs e)
        {
            CurrentPage = _map[e.NewPage](this);
            NotifyPropertyChanged(nameof(CurrentPage));
        }
    }
}

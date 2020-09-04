using GsriSync.WpfApp.Utils;
using System;

namespace GsriSync.WpfApp.ViewModels
{
    internal class MainWindowsVM : NotifyPropertyChangedBase
    {
        public object CurrentPage { get; set; }

        private ConfigurationVM ConfigurationVM { get; }

        private DownloadVM DownloadVM { get; }

        private InstallVM InstallVM { get; }

        private PlayVM PlayVM { get; }

        private VerifyVM VerifyVM { get; }

        public MainWindowsVM()
        {
            ConfigurationVM = new ConfigurationVM(this);
            DownloadVM = new DownloadVM(this);
            InstallVM = new InstallVM(this);
            PlayVM = new PlayVM(this);
            VerifyVM = new VerifyVM(this);
            NavigateToVerify();
        }

        public void NavigateToConfiguration() => NavigateTo(ConfigurationVM);

        public void NavigateToInstall() => NavigateTo(InstallVM);

        public void NavigateToPlay() => NavigateTo(PlayVM);

        public void NavigateToPostVerify(bool isSync) => NavigateTo(isSync ? PlayVM as object : InstallVM);

        public void NavigateToVerify() => NavigateTo(VerifyVM);

        internal void NavigateToDownload() => NavigateTo(DownloadVM);

        private void NavigateTo(object page)
        {
            if (CurrentPage == page) return;
            CurrentPage = page;
            NotifyPropertyChanged(nameof(CurrentPage));
        }
    }
}

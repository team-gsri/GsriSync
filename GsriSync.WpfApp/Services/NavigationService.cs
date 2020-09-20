using System;

namespace GsriSync.WpfApp.Services
{
    internal class NavigationService
    {
        private Pages? _previous;

        public Pages Current { get; private set; }

        public event NavigationChangedEventHandler NavigationChanged;

        public void NavigateAfterVerify(bool isSync) => NavigateTo(isSync ? Pages.Play : Pages.Install);

        public void NavigateBack()
        {
            NavigateTo(_previous ?? Current);
        }

        public void NavigateTo(Pages target)
        {
            if (target == Pages.Config) { _previous = Current; }
            Current = target;
            NavigationChanged?.Invoke(this, new NavigationChangedEventArgs(target));
        }

        internal enum Pages { Config, Verify, Install, Download, Play }

        internal delegate void NavigationChangedEventHandler(object sender, NavigationChangedEventArgs e);

        internal class NavigationChangedEventArgs : EventArgs
        {
            public Pages NewPage { get; }

            public NavigationChangedEventArgs(Pages target)
            {
                this.NewPage = target;
            }
        }
    }
}

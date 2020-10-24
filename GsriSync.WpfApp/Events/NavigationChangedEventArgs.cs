using System;

namespace GsriSync.WpfApp.Events
{
    public class NavigationChangedEventArgs : EventArgs
    {
        public Pages NewPage { get; }

        public NavigationChangedEventArgs(Pages target)
        {
            this.NewPage = target;
        }

        public enum Pages { Config, Verify, Download, Play }
    }
}

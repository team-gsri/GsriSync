using GsriSync.WpfApp.Events;
using static GsriSync.WpfApp.Events.NavigationChangedEventArgs;

namespace GsriSync.WpfApp.Services
{
    internal class NavigationService : INotifyNavigationChanged
    {
        private Pages? _previous;

        public Pages Current { get; private set; }

        public event NavigationChangedEventHandler NavigationChanged;

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
    }
}

using GsriSync.WpfApp.Services;
using GsriSync.WpfApp.Utils;
using System.Threading;
using System.Threading.Tasks;

namespace GsriSync.WpfApp.ViewModels
{
    internal class VerifyVM : NotifyPropertyChangedBase
    {
        private readonly MainWindowsVM _parent;

        private readonly SynchronizeService _sync = new SynchronizeService();

        private Task running;

        public VerifyVM(MainWindowsVM parent)
        {
            _parent = parent;
        }

        public async Task VerifyAsync()
        {
            var isSync = await _sync.IsSynchronizedAsync();
            _parent.NavigateToPostVerify(isSync);
            running = null;
        }

        internal void ScheduleVerify()
        {
            running = running ?? Task.Factory.StartNew(VerifyAsync, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.FromCurrentSynchronizationContext());
        }
    }
}

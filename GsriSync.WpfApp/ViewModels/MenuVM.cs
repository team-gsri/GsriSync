using GsriSync.WpfApp.Utils;
using System.Windows.Input;

namespace GsriSync.WpfApp.ViewModels
{
    internal class MenuVM
    {
        private readonly MainWindowsVM _parent;

        public ICommand NavigateConfigurationCommand => new DelegateCommand(parameter => _parent.NavigateToConfiguration());

        public MenuVM(MainWindowsVM parent)
        {
            this._parent = parent;
        }
    }
}

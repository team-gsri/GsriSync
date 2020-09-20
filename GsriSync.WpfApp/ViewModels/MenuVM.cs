using GsriSync.WpfApp.Services;
using GsriSync.WpfApp.Utils;
using System.Windows.Input;

namespace GsriSync.WpfApp.ViewModels
{
    internal class MenuVM
    {
        private readonly NavigationService _navigation;

        public ICommand NavigateConfigurationCommand => new DelegateCommand(NavigateAction);

        public MenuVM(NavigationService navigation)
        {
            this._navigation = navigation;
        }

        private void NavigateAction(object parameter)
        {
            _navigation.NavigateTo(NavigationService.Pages.Config);
        }
    }
}

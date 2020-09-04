using GsriSync.WpfApp.ViewModels;
using System.Windows.Controls;

namespace GsriSync.WpfApp.Views
{
    /// <summary>
    /// Logique d'interaction pour Install.xaml
    /// </summary>
    public partial class Install : UserControl
    {
        public Install()
        {
            InitializeComponent();
            this.LayoutUpdated += Verify_LayoutUpdated;
        }

        private void Verify_LayoutUpdated(object sender, System.EventArgs e)
        {
            (DataContext as InstallVM)?.ScheduleVerify();
        }
    }
}
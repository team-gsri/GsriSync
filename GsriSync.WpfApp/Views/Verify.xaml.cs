using GsriSync.WpfApp.ViewModels;
using System.Windows.Controls;

namespace GsriSync.WpfApp.Views
{
    /// <summary>
    /// Logique d'interaction pour Verify.xaml
    /// </summary>
    public partial class Verify : UserControl
    {
        public Verify()
        {
            InitializeComponent();
            this.LayoutUpdated += this.Verify_LayoutUpdated;
        }

        private void Verify_LayoutUpdated(object sender, System.EventArgs e)
        {
            (DataContext as VerifyVM)?.ScheduleVerify();
        }
    }
}
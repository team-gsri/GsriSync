using GsriSync.WpfApp.ViewModels;
using System.Windows.Controls;

namespace GsriSync.WpfApp.Views
{
    /// <summary>
    /// Logique d'interaction pour Play.xaml
    /// </summary>
    public partial class Play : UserControl
    {
        public Play()
        {
            InitializeComponent();
            LayoutUpdated += OnLayoutUpdated;
        }

        private void OnLayoutUpdated(object sender, System.EventArgs e)
        {
            (DataContext as PlayVM)?.RefreshCommandVisibility();
        }
    }
}

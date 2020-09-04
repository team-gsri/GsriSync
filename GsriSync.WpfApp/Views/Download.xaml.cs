using GsriSync.WpfApp.ViewModels;
using System;
using System.Windows.Controls;

namespace GsriSync.WpfApp.Views
{
    /// <summary>
    /// Logique d'interaction pour Download.xaml
    /// </summary>
    public partial class Download : UserControl
    {
        public Download()
        {
            InitializeComponent();
            LayoutUpdated += this.Download_LayoutUpdated;
        }

        private void Download_LayoutUpdated(object sender, EventArgs e)
        {
            (DataContext as DownloadVM)?.ScheduleDownload();
        }
    }
}
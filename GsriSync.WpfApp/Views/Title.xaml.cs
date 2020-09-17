using System.Windows;
using System.Windows.Controls;

namespace GsriSync.WpfApp.Views
{
    /// <summary>
    /// Logique d'interaction pour Title.xaml
    /// </summary>
    public partial class Title : UserControl
    {
        public Title()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}

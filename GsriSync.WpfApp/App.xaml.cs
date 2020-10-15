using GsriSync.WpfApp.Repositories;
using GsriSync.WpfApp.Services;
using GsriSync.WpfApp.Services.ManifestProviders;
using GsriSync.WpfApp.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;

namespace GsriSync.WpfApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly IServiceProvider _serviceProvider;

        public App()
        {
            var collection = new ServiceCollection();
            ConfigureServices(collection);
            _serviceProvider = collection.BuildServiceProvider();
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var mainWindow = _serviceProvider.GetService<MainWindow>();
            mainWindow.DataContext = _serviceProvider.GetService<MainWindowsVM>();
            mainWindow.Show();
        }

        private void ConfigureServices(ServiceCollection services)
        {
            services.AddSingleton<MainWindow>();
            services.AddSingleton<MainWindowsVM>();
            services.AddSingleton<VerifyVM>();
            services.AddSingleton<SettingsService>();
            services.AddSingleton<ManifestService>();
            services.AddSingleton<RegistryService>();
            services.AddSingleton<NavigationService>();
            services.AddSingleton<ConfigurationVM>();
            services.AddSingleton<DownloadVM>();
            services.AddSingleton<InstallVM>();
            services.AddSingleton<MenuVM>();
            services.AddSingleton<PlayVM>();
            services.AddSingleton<VerifyVM>();
            services.AddSingleton<System.Net.Http.HttpClient>();
            services.AddSingleton<ManifestRepository>();
            services.AddSingleton<LocalContentReaderProvider>();
            services.AddSingleton<LocalFileManifestProvider>();
            services.AddSingleton<LocalStrategyProvider>();
            services.AddSingleton<RemoteManifestProvider>();
        }
    }
}

using GsriSync.WpfApp.Repositories;
using GsriSync.WpfApp.Repositories.ManifestProviders;
using GsriSync.WpfApp.Services;
using GsriSync.WpfApp.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
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

        private static void ConfigureInjectionRepositories(ServiceCollection services)
        {
            services.AddSingleton<AddonRepository>();
            services.AddSingleton<ManifestRepository>();
            services.AddSingleton<LocalContentReaderProvider>();
            services.AddSingleton<LocalFileManifestProvider>();
            services.AddSingleton<LocalStrategyProvider>();
            services.AddSingleton<RemoteManifestProvider>();
        }

        private static void ConfigureInjectionServices(ServiceCollection services)
        {
            services.AddSingleton<SettingsService>();
            services.AddSingleton<ManifestService>();
            services.AddSingleton<RegistryService>();
            services.AddSingleton<NavigationService>();
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var mainWindow = _serviceProvider.GetService<MainWindow>();
            mainWindow.DataContext = _serviceProvider.GetService<MainWindowsVM>();
            mainWindow.Show();
        }

        private void ConfigureInjectionViewModels(ServiceCollection services)
        {
            //services.AddSingleton<InstallVM>();
            services.AddSingleton<MainWindowsVM>();
            services.AddSingleton<VerifyVM>();
            services.AddSingleton<ConfigurationVM>();
            services.AddSingleton<DownloadVM>();
            services.AddSingleton<MenuVM>();
            services.AddSingleton<PlayVM>();
            services.AddSingleton<VerifyVM>();
        }

        private void ConfigureServices(ServiceCollection services)
        {
            services.AddSingleton<MainWindow>();
            services.AddSingleton<HttpClient>();
            ConfigureInjectionServices(services);
            ConfigureInjectionRepositories(services);
            ConfigureInjectionViewModels(services);
        }
    }
}

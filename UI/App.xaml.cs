using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using CryptoPulse.UI.ViewModels;

namespace CryptoPulse.UI
{
    public partial class App : Application
    {
        private readonly ServiceProvider _serviceProvider;

        public App()
        {
            var services = new ServiceCollection();
            services.AddSingleton<MainViewModel>();
            _serviceProvider = services.BuildServiceProvider();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var mainViewModel = _serviceProvider.GetService<MainViewModel>();
            var mainWindow = new MainWindow { DataContext = mainViewModel };
            mainWindow.Show();
        }
    }
}
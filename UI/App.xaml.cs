using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using CryptoPulse.UI.ViewModels;
using CryptoPulse.Application.Features.Users;
using CryptoPulse.Application.Features.Rates.Interfaces;
using CryptoPulse.Application.Features.Rates.Services;

namespace CryptoPulse.UI
{
    public partial class App : System.Windows.Application
    {
        private readonly ServiceProvider _serviceProvider;

        public App()
        {
            var services = new ServiceCollection();
            services.AddSingleton<IRatesService, RatesService>();
            services.AddSingleton<UserRegistrationService>(_ => 
                new UserRegistrationService("Server=localhost,1433;Database=CryptoPulseDB;User Id=sa;Password=useruser_123;"));
            services.AddSingleton<MainViewModel>(provider => 
                new MainViewModel(provider.GetRequiredService<IRatesService>()));
            _serviceProvider = services.BuildServiceProvider();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var userRegistrationService = _serviceProvider.GetRequiredService<UserRegistrationService>();
            var mainViewModel = _serviceProvider.GetRequiredService<MainViewModel>();
            var mainWindow = new MainWindow(userRegistrationService, mainViewModel)
            {
                DataContext = mainViewModel
            };
            mainWindow.Show();
        }
    }
}
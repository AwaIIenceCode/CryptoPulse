using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CryptoTracker.UI.Views;

namespace CryptoTracker.UI.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        public PortfolioView PortfolioView { get; } = new PortfolioView();
        public NewsView NewsView { get; } = new NewsView();
        public RatesView RatesView { get; } = new RatesView();

        [ObservableProperty]
        private object _currentView;

        public MainViewModel()
        {
            CurrentView = PortfolioView;
        }

        [RelayCommand]
        private void SwitchToPortfolio()
        {
            CurrentView = PortfolioView;
        }

        [RelayCommand]
        private void SwitchToNews()
        {
            CurrentView = NewsView;
        }

        [RelayCommand]
        private void SwitchToRates()
        {
            CurrentView = RatesView;
        }
    }
}
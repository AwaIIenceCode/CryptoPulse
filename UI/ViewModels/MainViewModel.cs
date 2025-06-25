using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CryptoPulse.UI.Views;
using CryptoPulse.Application.Features.Rates.Interfaces;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace CryptoPulse.UI.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<string> guestData = new ObservableCollection<string>();

        [ObservableProperty]
        private object _currentView;

        [ObservableProperty]
        private string coinRates = string.Empty; // Инициализация по умолчанию

        private readonly IRatesService _ratesService;

        public PortfolioView PortfolioView { get; } = new PortfolioView();
        public NewsView NewsView { get; } = new NewsView();
        public RatesView RatesView { get; } = new RatesView();

        public MainViewModel(IRatesService ratesService)
        {
            _ratesService = ratesService ?? throw new ArgumentNullException(nameof(ratesService));
            CurrentView = PortfolioView;
            EnterGuestModeCommand = new RelayCommand(EnterGuestMode);
            SwitchToPortfolioCommand = new RelayCommand(SwitchToPortfolio);
            SwitchToNewsCommand = new RelayCommand(SwitchToNews);
            SwitchToRatesCommand = new RelayCommand(SwitchToRates);
            LoadRatesCommand = new AsyncRelayCommand(LoadRatesAsync);
        }

        public IRelayCommand EnterGuestModeCommand { get; }
        public IRelayCommand SwitchToPortfolioCommand { get; }
        public IRelayCommand SwitchToNewsCommand { get; }
        public IRelayCommand SwitchToRatesCommand { get; }
        public IAsyncRelayCommand LoadRatesCommand { get; }

        private void EnterGuestMode()
        {
            GuestData.Clear();
            GuestData.Add("Гость_1"); // Данные для гостя
        }

        private void SwitchToPortfolio()
        {
            CurrentView = PortfolioView;
        }

        private void SwitchToNews()
        {
            CurrentView = NewsView;
        }

        private void SwitchToRates()
        {
            CurrentView = RatesView;
        }

        private async Task LoadRatesAsync()
        {
            CoinRates = await _ratesService.GetCoinRatesAsync();
        }
    }
}
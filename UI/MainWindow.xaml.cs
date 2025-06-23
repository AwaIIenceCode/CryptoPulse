using System.Windows;

namespace CryptoPulse.UI
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            // Пока просто выводим сообщение
            MessageBox.Show("Регистрация скоро будет!");
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            // Пока просто выводим сообщение
            MessageBox.Show("Вход скоро будет!");
        }
    }
}
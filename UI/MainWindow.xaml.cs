using System.Windows;
using System.Windows.Controls;

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

        private void MediaElement_MediaEnded(object sender, RoutedEventArgs e)
        {
            (sender as MediaElement).Position = TimeSpan.Zero;
            (sender as MediaElement).Play();
        }
    }
}
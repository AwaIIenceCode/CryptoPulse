using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Media;
using CryptoPulse.Application.Features.Users;
using CryptoPulse.UI.ViewModels;

namespace CryptoPulse.UI
{
    public partial class MainWindow : Window
    {
        private readonly UserRegistrationService _userRegistrationService;
        private readonly MainViewModel _viewModel;

        public MainWindow(UserRegistrationService userRegistrationService, MainViewModel viewModel)
        {
            InitializeComponent();
            _userRegistrationService = userRegistrationService ?? throw new ArgumentNullException(nameof(userRegistrationService));
            _viewModel = viewModel ?? throw new ArgumentNullException(nameof(viewModel));
            DataContext = _viewModel;
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Name == "RegisterButton")
            {
                MainContent.Visibility = System.Windows.Visibility.Collapsed;
                ShowRegisterPanel();
            }
            else
            {
                string login = LoginTextBox.Text;
                string email = EmailTextBox.Text;
                string password = PasswordBox.Password;

                if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                {
                    MessageBox.Show("Заполни все поля, чувак!");
                    return;
                }

                if (_userRegistrationService.RegisterUser(login, email, password))
                {
                    MessageBox.Show("Юзер зареган, поздравляю!");
                    ResetRegisterPanel();
                }
                else
                {
                    MessageBox.Show("Ошибка при регистрации, попробуй ещё раз!");
                }
            }
        }

        private void GuestButton_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.EnterGuestModeCommand.Execute(null);
            MessageBox.Show("Добро пожаловать, гость!");
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Вход скоро будет!");
        }

        private void MediaElement_MediaEnded(object sender, RoutedEventArgs e)
        {
            if (sender is MediaElement media)
            {
                media.Position = TimeSpan.Zero;
                media.Play();
            }
        }

        private void ShowRegisterPanel()
        {
            RegisterPanel.Visibility = Visibility.Visible;

            var scaleTransform = new ScaleTransform(0.0, 0.0);
            RegisterPanel.RenderTransform = scaleTransform;
            RegisterPanel.RenderTransformOrigin = new Point(0.5, 0.5);

            var scaleUpX = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(0.8))
            {
                EasingFunction = new QuinticEase { EasingMode = EasingMode.EaseOut }
            };
            var scaleUpY = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(0.8))
            {
                EasingFunction = new QuinticEase { EasingMode = EasingMode.EaseOut }
            };
            var fadeIn = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(0.8));

            RegisterPanel.BeginAnimation(UIElement.OpacityProperty, fadeIn);
            scaleTransform.BeginAnimation(ScaleTransform.ScaleXProperty, scaleUpX);
            scaleTransform.BeginAnimation(ScaleTransform.ScaleYProperty, scaleUpY);
        }

        private void ResetRegisterPanel()
        {
            var scaleTransform = RegisterPanel.RenderTransform as ScaleTransform;

            var scaleDownX = new DoubleAnimation(1, 0, TimeSpan.FromSeconds(0.8))
            {
                EasingFunction = new QuinticEase { EasingMode = EasingMode.EaseIn }
            };
            var scaleDownY = new DoubleAnimation(1, 0, TimeSpan.FromSeconds(0.8))
            {
                EasingFunction = new QuinticEase { EasingMode = EasingMode.EaseIn }
            };
            var fadeOut = new DoubleAnimation(1, 0, TimeSpan.FromSeconds(0.8));

            fadeOut.Completed += (s, e) =>
            {
                RegisterPanel.Visibility = Visibility.Collapsed;
                MainContent.Visibility = Visibility.Visible;
            };

            RegisterPanel.BeginAnimation(UIElement.OpacityProperty, fadeOut);
            scaleTransform?.BeginAnimation(ScaleTransform.ScaleXProperty, scaleDownX);
            scaleTransform?.BeginAnimation(ScaleTransform.ScaleYProperty, scaleDownY);
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            ResetRegisterPanel();
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                if (textBox.Name == "LoginTextBox") LoginHint.Visibility = System.Windows.Visibility.Collapsed;
                else if (textBox.Name == "EmailTextBox") EmailHint.Visibility = System.Windows.Visibility.Collapsed;
            }
            else if (sender is PasswordBox passwordBox)
            {
                PasswordHint.Visibility = System.Windows.Visibility.Collapsed;
            }
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                if (string.IsNullOrEmpty(textBox.Text))
                {
                    if (textBox.Name == "LoginTextBox") LoginHint.Visibility = System.Windows.Visibility.Visible;
                    else if (textBox.Name == "EmailTextBox") EmailHint.Visibility = System.Windows.Visibility.Visible;
                }
            }
            else if (sender is PasswordBox passwordBox)
            {
                if (string.IsNullOrEmpty(passwordBox.Password))
                {
                    PasswordHint.Visibility = System.Windows.Visibility.Visible;
                }
            }
        }
    }
}
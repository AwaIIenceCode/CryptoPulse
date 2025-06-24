using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Media; 
using Microsoft.Data.SqlClient;

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

                using (var connection = new SqlConnection("Server=localhost,1433;Database=CryptoPulseDB;User Id=sa;Password=useruser_123;"))
                {
                    try
                    {
                        connection.Open();

                        string createTable = @"
                            IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Users')
                            CREATE TABLE Users (
                                Id INT IDENTITY(1,1) PRIMARY KEY,
                                Login NVARCHAR(50) NOT NULL,
                                Email NVARCHAR(100) NOT NULL,
                                Password NVARCHAR(100) NOT NULL
                            )";
                        using (var command = new SqlCommand(createTable, connection))
                        {
                            command.ExecuteNonQuery();
                        }

                        string checkLogin = "SELECT COUNT(*) FROM Users WHERE Login = @Login";
                        using (var command = new SqlCommand(checkLogin, connection))
                        {
                            command.Parameters.AddWithValue("@Login", login);
                            int count = (int)command.ExecuteScalar();
                            if (count > 0)
                            {
                                MessageBox.Show("Такой логин уже есть, придумай другой!");
                                return;
                            }
                        }

                        string insertUser = "INSERT INTO Users (Login, Email, Password) VALUES (@Login, @Email, @Password)";
                        using (var command = new SqlCommand(insertUser, connection))
                        {
                            command.Parameters.AddWithValue("@Login", login);
                            command.Parameters.AddWithValue("@Email", email);
                            command.Parameters.AddWithValue("@Password", password);
                            command.ExecuteNonQuery();
                        }

                        MessageBox.Show("Юзер зареган, поздравляю!");
                        ResetRegisterPanel();
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show($"Ошибка с БД: {ex.Message}");
                    }
                }
            }
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
            RegisterPanel.Visibility = System.Windows.Visibility.Visible;
            DoubleAnimation animation = new DoubleAnimation
            {
                From = -200,
                To = 0,
                Duration = TimeSpan.FromSeconds(0.5),
                EasingFunction = new QuadraticEase()
            };
            RegisterTransform.BeginAnimation(TranslateTransform.YProperty, animation);
        }

        private void ResetRegisterPanel()
        {
            DoubleAnimation animation = new DoubleAnimation
            {
                From = 0,
                To = -200,
                Duration = TimeSpan.FromSeconds(0.5),
                EasingFunction = new QuadraticEase()
            };
            animation.Completed += (s, e) =>
            {
                RegisterPanel.Visibility = System.Windows.Visibility.Collapsed;
                MainContent.Visibility = System.Windows.Visibility.Visible;
                RegisterTransform.BeginAnimation(TranslateTransform.YProperty, null);
                LoginTextBox.Text = "";
                EmailTextBox.Text = "";
                PasswordBox.Password = "";
                LoginHint.Visibility = System.Windows.Visibility.Visible;
                EmailHint.Visibility = System.Windows.Visibility.Visible;
                PasswordHint.Visibility = System.Windows.Visibility.Visible;
            };
            RegisterTransform.BeginAnimation(TranslateTransform.YProperty, animation);
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
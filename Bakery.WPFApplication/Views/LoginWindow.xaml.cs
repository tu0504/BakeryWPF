using System.Windows;
using Bakery.BLLL.Services;

namespace Bakery.WPFApplication.Views
{
    public partial class LoginWindow : Window
    {
        private readonly AuthService _authService;

        public LoginWindow()
        {
            InitializeComponent();
            _authService = ((App)Application.Current).AuthService;
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            var email = EmailTextBox.Text?.Trim() ?? "";
            var password = PasswordBox.Password ?? "";

            var (success, err, user) = _authService.Login(email, password);
            if (!success)
            {
                MessageBox.Show(err ?? "Login failed", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            MessageBox.Show($"Welcome, {user?.UserName}", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

            var main = new MainWindow();
            main.Show();
            this.Close();
        }

        private void OpenRegisterButton_Click(object sender, RoutedEventArgs e)
        {
            var reg = new RegisterWindow();
            reg.Owner = this;
            reg.ShowDialog();
        }
    }
}

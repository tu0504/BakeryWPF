using System;
using System.Windows;
using Bakery.BLLL.Services;

namespace Bakery.WPFApplication.Views
{
    public partial class RegisterWindow : Window
    {
        private readonly AuthService _authService;

        public RegisterWindow()
        {
            InitializeComponent();
            _authService = ((App)Application.Current).AuthService;
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            var userName = UserNameTextBox.Text?.Trim() ?? "";
            var email = EmailTextBox.Text?.Trim() ?? "";
            var password = PasswordBox.Password ?? "";
            var confirm = ConfirmPasswordBox.Password ?? "";

            var (success, err) = _authService.Register(userName, email, password, confirm);
            if (!success)
            {
                MessageBox.Show(err ?? "Register failed", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            MessageBox.Show("Registration successful. You can now log in.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            this.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

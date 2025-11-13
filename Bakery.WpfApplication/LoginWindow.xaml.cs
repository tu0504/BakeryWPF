using Bakery.Repository.Models;
using Bakery.Repository.Repositories.Implement;
using Bakery.Repository.Repositories.Interface;
using Bakery.Service;
using Bakery.Service.Implement;
using Bakery.Service.Interface;
using System;
using System.Windows;

namespace Bakery.WpfApplication
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {

        private readonly IUserService _userService;

        public LoginWindow()
        {
            InitializeComponent();
            // keep lightweight initialization; if this fails you will get a clear message instead of NRE later
            try
            {
                IUserRepository userRepository = new UserRepository();
                _userService = new UserServices(userRepository);
            }
            catch (Exception ex)
            {
                // log or show a friendly error so developer can see what failed during construction
                MessageBox.Show($"Failed to create default services: {ex.Message}", "Initialization error", MessageBoxButton.OK, MessageBoxImage.Error);
                _userService = null;
            }
        }

        public LoginWindow(IUserService userService)
        {
            InitializeComponent();
            _userService = userService;
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string email = txtEmail.Text?.Trim() ?? string.Empty;
                string password = txtPass.Password ?? string.Empty;

                var user = _userService.GetUserByEmailAndPassword(email, password);

                if (user != null)
                {
                    // Kiểm tra vai trò
                    if (string.Equals(user.Role, "AD", StringComparison.OrdinalIgnoreCase))
                    {
                        this.Hide();
                        AdminDashboardWindow admin = new AdminDashboardWindow();
                        admin.Show();
                    }
                    else if (string.Equals(user.Role, "US", StringComparison.OrdinalIgnoreCase))
                    {
                        this.Hide();
                        ShopWindow shopWindow = new ShopWindow(user);
                        shopWindow.Show();
                    }
                    else
                    {
                        MessageBox.Show("User role not recognized!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Email or password is incorrect!", "Login Failed", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Login Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            RegisterWindow register = new RegisterWindow();
            register.ShowDialog();
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            RegisterWindow register = new RegisterWindow();
            register.ShowDialog();
        }

        private void txtEmail_GotFocus(object sender, RoutedEventArgs e)
        {
            txtPlaceholder.Visibility = Visibility.Collapsed;
        }

        private void txtEmail_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtEmail.Text))
            {
                txtPlaceholder.Visibility = Visibility.Visible;
            }
        }

        private void txtPass_GotFocus(object sender, RoutedEventArgs e)
        {
            txtPlaceholderP.Visibility = Visibility.Collapsed;
        }

        private void txtPass_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtPass.Password))
            {
                txtPlaceholderP.Visibility = Visibility.Visible;
            }
        }
    }
}

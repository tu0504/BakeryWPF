using Bakery.Repository.Models;
using Bakery.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Bakery.WpfApplication
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private readonly UserService _userService;
        public LoginWindow()
        {
            InitializeComponent();
            _userService = new UserService();
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            RegisterWindow registerWindow = new RegisterWindow();
            registerWindow.Show();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string email = txtEmail.Text.Trim();
                string password = txtPass.Password;

                // Gọi hàm xác thực từ service
                User user = _userService.Authenticate(email, password);

                if (user != null)
                {
                    // Kiểm tra vai trò
                    if (user.Role.Equals("AD"))
                    {
                        this.Hide();
                        AdminDashboardWindow admin = new AdminDashboardWindow(user.UserName);
                        admin.Show();
                    }
                    else if (user.Role.Equals("US"))
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

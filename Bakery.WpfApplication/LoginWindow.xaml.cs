using Bakery.Repository.Models;
using Bakery.Repository.Repositories.Implement;
using Bakery.Repository.Repositories.Interface;
using Bakery.Service;
using Bakery.Service.Implement;
using Bakery.Service.Interface;
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
        private IUserService? _userService;
        public LoginWindow()
        {
            InitializeComponent();
            try
            {
                IUserRepository userRepository = new UserRepository();
                _userService = new UserServices(userRepository);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to create default services: {ex.Message}", "Initialization error", MessageBoxButton.OK, MessageBoxImage.Error);
                _userService = null;
            }
        }
        public LoginWindow(IUserService userService)
        {
            InitializeComponent();
            _userService = userService;
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            RegisterWindow regis = new();
            regis.ShowDialog();

        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {

            string email = txtEmail.Text.Trim();
            string password = txtPass.Password;
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Please enter full Email and Password.", "Missing information", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            try
            {

            User? authenticatedUser = _userService.AuthenticateUser(email, password);

            if (authenticatedUser != null)
            {

                if (authenticatedUser.Status == false)
                {
                    MessageBox.Show("Your account has been locked. Please contact your administrator.", "Access denied", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (authenticatedUser.Role == "AD")
                {
                    AdminDashboardWindow admin = new();
                    admin.Show();
                    this.Hide();
                }
                
                MessageBox.Show($"Login successful! Welcome, {authenticatedUser.FullName}.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                
            }
            else
            {
                // Lỗi xác thực (User không tồn tại hoặc mật khẩu sai)
                MessageBox.Show("Email hoặc mật khẩu không chính xác.", "Lỗi đăng nhập", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    catch (Exception ex)
    {
        MessageBox.Show($"Lỗi hệ thống: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
    
}
}
    }
}

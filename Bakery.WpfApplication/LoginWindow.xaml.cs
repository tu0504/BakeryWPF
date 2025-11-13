using Bakery.Service;
using System.Windows;

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

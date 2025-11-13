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
    /// Interaction logic for RegisterWindow.xaml
    /// </summary>
    public partial class RegisterWindow : Window
    {
        private readonly UserService _userService;
        public RegisterWindow()
        {
            InitializeComponent();
            _userService = new UserService();
        }

        private void txtName_GotFocus(object sender, RoutedEventArgs e)
        {
            txtPlaceholderN.Visibility = Visibility.Collapsed;
        }
        private void txtName_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtName.Text))
            {
                txtPlaceholderN.Visibility = Visibility.Visible;
            }
        }

        private void txtPhone_GotFocus(object sender, RoutedEventArgs e)
        {
            txtPlaceholderPhone.Visibility = Visibility.Collapsed;
        }
        private void txtPhone_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtPhone.Text))
            {
                txtPlaceholderPhone.Visibility = Visibility.Visible;
            }
        }

        private void txtA_GotFocus(object sender, RoutedEventArgs e)
        {
            txtPlaceholderA.Visibility = Visibility.Collapsed;
        }
        private void txtA_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtAddress.Text))
            {
                txtPlaceholderA.Visibility = Visibility.Visible;
            }
        }

        private void txtEmail_GotFocus(object sender, RoutedEventArgs e)
        {
            txtPlaceholderA.Visibility = Visibility.Collapsed;
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

        private void txtPassC_GotFocus(object sender, RoutedEventArgs e)
        {
            txtPlaceholderPC.Visibility = Visibility.Collapsed;
        }
        private void txtPassC_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtPass.Password))
            {
                txtPlaceholderPC.Visibility = Visibility.Visible;
            }
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string name = txtName.Text.Trim();
                string phone = txtPhone.Text.Trim();
                string address = txtAddress.Text.Trim();
                string email = txtEmail.Text.Trim();
                string password = txtPass.Password;
                string confirmPassword = txtPassC.Password;

                if (string.IsNullOrEmpty(name) ||
                    string.IsNullOrEmpty(phone) ||
                    string.IsNullOrEmpty(address) ||
                    string.IsNullOrEmpty(email) ||
                    string.IsNullOrEmpty(password) ||
                    string.IsNullOrEmpty(confirmPassword))
                {
                    MessageBox.Show("Please fill in all fields.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                User newUser = new User
                {
                    UserName = name,
                    Email = email,
                    Phone = phone,
                    Address = address,
                    Password = password,
                    CreatedAt = DateTime.Now,
                };

                _userService.RegisterNewUser(newUser, confirmPassword);
                MessageBox.Show("Registration successful!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                LoginWindow login = new LoginWindow();
                login.Show();
                this.Close();
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Registration Failed", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unexpected error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}

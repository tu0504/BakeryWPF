using Bakery.Repository.Models;
using Bakery.Service;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

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

        private void txtName_GotFocus(object sender, RoutedEventArgs e) => txtPlaceholderN.Visibility = Visibility.Collapsed;
        private void txtName_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text)) txtPlaceholderN.Visibility = Visibility.Visible;
        }

        private void txtPhone_GotFocus(object sender, RoutedEventArgs e) => txtPlaceholderPhone.Visibility = Visibility.Collapsed;
        private void txtPhone_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPhone.Text)) txtPlaceholderPhone.Visibility = Visibility.Visible;
        }

        private void txtA_GotFocus(object sender, RoutedEventArgs e) => txtPlaceholderA.Visibility = Visibility.Collapsed;
        private void txtA_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtAddress.Text)) txtPlaceholderA.Visibility = Visibility.Visible;
        }

        private void txtEmail_GotFocus(object sender, RoutedEventArgs e) => txtPlaceholder.Visibility = Visibility.Collapsed;
        private void txtEmail_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtEmail.Text)) txtPlaceholder.Visibility = Visibility.Visible;
        }

        private void txtPass_GotFocus(object sender, RoutedEventArgs e) => txtPlaceholderP.Visibility = Visibility.Collapsed;
        private void txtPass_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPass.Password)) txtPlaceholderP.Visibility = Visibility.Visible;
        }

        private void txtPassC_GotFocus(object sender, RoutedEventArgs e) => txtPlaceholderPC.Visibility = Visibility.Collapsed;
        private void txtPassC_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPassC.Password)) txtPlaceholderPC.Visibility = Visibility.Visible;
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            // Basic validation
            if (string.IsNullOrWhiteSpace(txtName.Text) || string.IsNullOrWhiteSpace(txtEmail.Text) || string.IsNullOrWhiteSpace(txtPass.Password))
            {
                MessageBox.Show("Please fill in required fields: Name, Email, Password.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!Regex.IsMatch(txtEmail.Text, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                MessageBox.Show("Email format is invalid.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (txtPass.Password != txtPassC.Password)
            {
                MessageBox.Show("Passwords do not match.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                var user = new User
                {
                    UserName = txtName.Text.Trim(),
                    FullName = txtName.Text.Trim(),
                    Phone = txtPhone.Text?.Trim(),
                    Address = txtAddress.Text?.Trim(),
                    Email = txtEmail.Text.Trim(),
                    Password = txtPass.Password,
                    Role = "US",
                    Status = true
                };

                _userService.SaveUser(user);
                MessageBox.Show("Register successful.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Registration failed: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}

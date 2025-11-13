using Bakery.Repository.Models;
using Bakery.Repository.Repositories.Implement;
using Bakery.Repository.Repositories.Interface;
using Bakery.Service.Implement;
using Bakery.Service.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace Bakery.WpfApplication
{
    /// <summary>
    /// Interaction logic for RegisterWindow.xaml
    /// </summary>
    public partial class RegisterWindow : Window
    {
        private readonly IUserService _userService;
        public RegisterWindow()
        {
            InitializeComponent();
            IUserRepository userRepository = new UserRepository(); 
            _userService = new UserServices(userRepository);

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

                if (string.IsNullOrWhiteSpace(name))
                {
                    MessageBox.Show("Vui lòng nhập tên.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                    txtName.Focus();
                    return;
                }
                if (string.IsNullOrWhiteSpace(phone))
                {
                    MessageBox.Show("Vui lòng nhập số điện thoại.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                    txtPhone.Focus();
                    return;
                }
                if (string.IsNullOrWhiteSpace(email) || !IsValidEmail(email))
                {
                    MessageBox.Show("Vui lòng nhập email hợp lệ.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                    txtEmail.Focus();
                    return;
                }
                if (string.IsNullOrWhiteSpace(password) || password.Length < 6)
                {
                    MessageBox.Show("Mật khẩu phải có ít nhất 6 ký tự.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                    txtPass.Focus();
                    return;
                }
                if (password != confirmPassword)
                {
                    MessageBox.Show("Mật khẩu và xác nhận mật khẩu không khớp.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                    txtPassC.Focus();
                    return;
                }

                var newUser = new User
                {
                    FullName = name,
                    Phone = phone,
                    Address = address,
                    Email = email,
                    Password = password,
                    Role = "US",
                    UserName = !string.IsNullOrEmpty(email) ? email : ("user" + Guid.NewGuid().ToString("N")),
                    CreatedAt = DateTime.Now
                };

                _userService.SaveUser(newUser);
                MessageBox.Show("Đăng ký thành công.", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);


                this.Close();

            }
            catch (ArgumentException aex)
            {
                MessageBox.Show(aex.Message, "Lỗi đăng ký", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Có lỗi xảy ra khi đăng ký: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return false;
            try
            {
                // Regex đơn giản kiểm tra định dạng email
                return Regex.IsMatch(email,
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch
            {
                return false;
            }
        }


        // ======= Name =======

        private void txtName_GotFocus(object sender, RoutedEventArgs e)
        {
            txtPlaceholderN.Visibility = Visibility.Collapsed;
        }
        private void txtName_LostFocus(object sender, RoutedEventArgs e)
        {
            txtPlaceholderN.Visibility =
                string.IsNullOrWhiteSpace(txtName.Text) ? Visibility.Visible : Visibility.Collapsed;
        }
        // ======= Phone =======
        private void txtPhone_GotFocus(object sender, RoutedEventArgs e)
        {
            txtPlaceholderPhone.Visibility = Visibility.Collapsed;
        }
        private void txtPhone_LostFocus(object sender, RoutedEventArgs e)
        {
            txtPlaceholderPhone.Visibility =
                string.IsNullOrWhiteSpace(txtPhone.Text) ? Visibility.Visible : Visibility.Collapsed;
        }
        // ====== Address (txtA_*) ======
        private void txtA_GotFocus(object sender, RoutedEventArgs e)
        {
            txtPlaceholderA.Visibility = Visibility.Collapsed;
        }
        private void txtA_LostFocus(object sender, RoutedEventArgs e)
        {
            txtPlaceholderA.Visibility =
                string.IsNullOrWhiteSpace(txtAddress.Text) ? Visibility.Visible : Visibility.Collapsed;
        }

        // ====== Email ======
        private void txtEmail_GotFocus(object sender, RoutedEventArgs e)
        {
            txtPlaceholder.Visibility = Visibility.Collapsed;
        }
        private void txtEmail_LostFocus(object sender, RoutedEventArgs e)
        {
            txtPlaceholder.Visibility =
                string.IsNullOrWhiteSpace(txtEmail.Text) ? Visibility.Visible : Visibility.Collapsed;
        }

        // ====== Password ======
        private void txtPass_GotFocus(object sender, RoutedEventArgs e)
        {
            txtPlaceholderP.Visibility = Visibility.Collapsed;
        }
        private void txtPass_LostFocus(object sender, RoutedEventArgs e)
        {
            txtPlaceholderP.Visibility =
                string.IsNullOrEmpty(txtPass.Password) ? Visibility.Visible : Visibility.Collapsed;
        }

        // ====== Password Confirm ======
        private void txtPassC_GotFocus(object sender, RoutedEventArgs e)
        {
            txtPlaceholderPC.Visibility = Visibility.Collapsed;
        }
        private void txtPassC_LostFocus(object sender, RoutedEventArgs e)
        {
            txtPlaceholderPC.Visibility =
                string.IsNullOrEmpty(txtPassC.Password) ? Visibility.Visible : Visibility.Collapsed;
        }

    }

}

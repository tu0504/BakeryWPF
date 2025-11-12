using Bakery.Repository.Models;
using Bakery.Service.Implement;
using Bakery.Repository.Repositories.Implement;
using Bakery.Repository.Repositories.Interface;
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

namespace Bakery.WpfApplication.View
{
    /// <summary>
    /// Interaction logic for UserManagement.xaml
    /// </summary>
    public partial class UserManagement : UserControl
    {
        // Make the field nullable so we can assign a fallback implementation when needed.
        private IUserService? _userService;

        public UserManagement()
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

        public UserManagement(IUserService userService)
        {
            InitializeComponent();
            _userService = userService;
        }

        public void LoadUserList()
        {
            try
            {
                if (_userService == null)
                {
                    MessageBox.Show("User service is not initialized.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var userList = _userService.GetAll();

                if (userList != null && userList.Count > 0)
                {
                    dgData.ItemsSource = userList;
                }
                else
                {
                    dgData.ItemsSource = null;
                    MessageBox.Show("No data found.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải dữ liệu: {ex.Message}", "Lỗi Hệ Thống", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally { ResetInput(); }
        }

        private void btnCreateUser(object sender, RoutedEventArgs e)
        {
            try
            {
                User user = new User();
                user.UserName = txtUserName.Text;
                user.FullName = txtFullName.Text;
                user.Password = txtPassword.Text;
                user.Email = txtEmail.Text;
                user.Role = txtRole.Text;
                user.Address = txtAddress.Text;
                user.Phone = txtPhone.Text;
                user.Status = rbStatusActive.IsChecked == true ? true : true;

                _userService.SaveUser(user);
                MessageBox.Show("Create Successfully", "Successfully", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            finally { LoadUserList(); }
        }
        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            
        }
        private void btnUpdateUser(object sender, RoutedEventArgs e)
        {
            User? selected = dgData.SelectedItem as User;
            if (selected == null)
            {
                MessageBox.Show("Please select row / an user before editing.", "Select a row", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtFullName.Text) || string.IsNullOrWhiteSpace(txtUserName.Text))
            {
                MessageBox.Show("Please enter full Name.", "Missing Information", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(txtUserName.Text))
            {
                MessageBox.Show("Please enter Username.", "Missing Information", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                MessageBox.Show("Please enter Email", "Missing Information", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Please enter Password", "Missing Information", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(txtRole.Text))
            {
                MessageBox.Show("Please enter Role", "Missing Information", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(txtAddress.Text))
            {
                MessageBox.Show("Please enter Address", "Missing Information", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(txtPhone.Text))
            {
                MessageBox.Show("Please enter Phone", "Missing Information", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }


            try
            {

                int userId = Int32.Parse(txtUserId.Text);


                User updatedUser = new User
                {
                    UserId = userId,
                    UserName = txtUserName.Text,
                    FullName = txtFullName.Text,
                    Password = txtPassword.Text,
                    Role = txtRole.Text,
                    Address = txtAddress.Text,
                    Phone = txtPhone.Text,
                    Status = rbStatusActive.IsChecked == true ? true : false
                };

                // 4. Gọi Service để cập nhật
                _userService.UpdateCustomer(updatedUser);

                MessageBox.Show($"Update user ID {userId} successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (FormatException)
            {
                MessageBox.Show("Data format error: Please check UserId, RoleId, or Birthday.", "Data Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"System update error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                // 5. Tải lại danh sách (Luôn chạy)
                LoadUserList();
            }
        }

        private void btnDeleteUser(object sender, RoutedEventArgs e)
        {
            User? selected = dgData.SelectedItem as User;
            if (selected == null)
            {
                MessageBox.Show("Please select row / an user before deleting.", "Select a row", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            if (_userService == null)
            {
                MessageBox.Show("User service is not initialized.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            _userService.DeleteCustomer(selected);
            MessageBox.Show("Delete Successfully", "Successfully", MessageBoxButton.OK, MessageBoxImage.Information);
            LoadUserList();
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            string fullName = txtSearch.Text.Trim();
            if (string.IsNullOrEmpty(fullName))
            {
                LoadUserList();
            }
            else
            {
                List<User> customers = _userService.SearchByName(fullName);
                if (customers.Any() )
                {
                    dgData.ItemsSource = customers;
                }
                else
                {
                    MessageBox.Show("No customers found!", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }

        }

        private void txtSearch_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtSearch.Text == "Search")
                txtSearch.Text = string.Empty;
        }

        private void txtSearch_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSearch.Text))
                txtSearch.Text = "Search";
        }
        private void ResetInput()
        {
            txtUserId.Text = " ";
            txtUserName.Text = " ";
            txtFullName.Text = "";
            txtPassword.Text = "";
            txtAddress.Text = "";
            txtPhone.Text = "";
            rbStatusActive.IsChecked = false;
            rbStatusInactive.IsChecked = false;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            // Ensure a service exists before loading
            if (_userService == null)
            {
                try
                {
                    IUserRepository userRepository = new UserRepository();
                    _userService = new UserServices(userRepository);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Cannot initialize user service: {ex.Message}", "Initialization error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }

            LoadUserList();
        }




    }
}
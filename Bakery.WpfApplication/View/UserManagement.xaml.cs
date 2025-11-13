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
                user.Status = rbStatusActive.IsChecked == true ? true : false;

                _userService.SaveUser(user);
                MessageBox.Show("Create Successfully", "Successfully", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            finally { LoadUserList(); }
        }
        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button == null) return;

            User selectedUser = button.DataContext as User;

            if (selectedUser != null)
            {

                FillElement(selectedUser);
                dgData.SelectedItem = selectedUser;
                txtUserName.Focus();

            }
        }
        private void FillElement(User u)
        {
            if (u == null) return;
            txtUserId.Text = u.UserId.ToString();
            txtUserName.Text = u.UserName.ToString();
            txtFullName.Text = u.FullName.ToString();
            txtEmail.Text = u.Email.ToString();
            txtPassword.Text = u.Password.ToString();
            txtRole.Text = u.Role.ToString();
            txtAddress.Text = u.Address.ToString();
            txtPhone.Text = u.Phone.ToString();
            rbStatusActive.IsChecked = u.Status;
            rbStatusInactive.IsChecked = !u.Status;
            HideAllPlaceholders();

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

                User x = new();
                {
                    x.UserId = Int32.Parse(txtUserId.Text);
                    x.UserName = txtUserName.Text;
                    x.FullName = txtFullName.Text;
                    x.Password = txtPassword.Text;
                    x.Role = txtRole.Text;
                    x.Email = txtEmail.Text;
                    x.Address = txtAddress.Text;
                    x.Phone = txtPhone.Text;
                    x.Status = rbStatusActive.IsChecked == true ? true : false;
                }
                ;


                _userService.UpdateCustomer(x);

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


            if (string.IsNullOrWhiteSpace(fullName) || fullName.Equals("Search by name or email", StringComparison.OrdinalIgnoreCase))
            {

                LoadUserList();
            }
            else
            {

                try
                {
                    List<User> customers = _userService.SearchByNameOrEmail(fullName);

                    if (customers != null && customers.Any())
                    {
                        dgData.ItemsSource = customers;
                    }
                    else
                    {
                        dgData.ItemsSource = null;
                        MessageBox.Show("No customers found!", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi tìm kiếm: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        private void txtSearch_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtSearch.Text == "Search by name or email")
                txtSearch.Text = string.Empty;
            txtSearch.Foreground = Brushes.Black;
        }

        private void txtSearch_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSearch.Text))
                txtSearch.Text = "Search by name or email";
            txtSearch.Foreground = Brushes.Gray;
        }
        private void ResetInput()
        {
            txtUserId.Text = string.Empty;
            txtUserName.Text = string.Empty;
            txtFullName.Text = string.Empty;
            txtPassword.Text = string.Empty;
            txtEmail.Text = string.Empty;
            txtRole.Text = string.Empty;
            txtAddress.Text = string.Empty;
            txtPhone.Text = string.Empty;
            rbStatusActive.IsChecked = false;
            rbStatusInactive.IsChecked = false;

            txtInput_LostFocus(txtUserName, null);
            txtInput_LostFocus(txtFullName, null);
            txtInput_LostFocus(txtEmail, null);
            txtInput_LostFocus(txtPassword, null);
            txtInput_LostFocus(txtRole, null);
            txtInput_LostFocus(txtAddress, null);
            txtInput_LostFocus(txtPhone, null);

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

        private void txtInput_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox == null) return;


            Grid parentGrid = VisualTreeHelper.GetParent(textBox) as Grid;

            if (parentGrid != null)
            {

                TextBlock placeholderText = parentGrid.Children.OfType<TextBlock>().FirstOrDefault();

                if (placeholderText != null)
                {

                    placeholderText.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void txtInput_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox == null) return;


            if (string.IsNullOrWhiteSpace(textBox.Text))
            {

                Grid parentGrid = VisualTreeHelper.GetParent(textBox) as Grid;

                if (parentGrid != null)
                {

                    TextBlock placeholderText = parentGrid.Children.OfType<TextBlock>().FirstOrDefault();

                    if (placeholderText != null)
                    {

                        placeholderText.Visibility = Visibility.Visible;
                    }
                }
            }

        }

        private void HideAllPlaceholders()
        {
            List<TextBox> textInputs = new List<TextBox> {
                txtUserId, txtUserName, txtFullName, txtEmail, txtPassword,
                txtRole, txtAddress, txtPhone
            };

            foreach (var textBox in textInputs)
            {
                Grid parentGrid = VisualTreeHelper.GetParent(textBox) as Grid;

                if (parentGrid != null)
                {
                    // Tìm TextBlock (Placeholder) bên trong Grid
                    TextBlock placeholderText = parentGrid.Children.OfType<TextBlock>().FirstOrDefault();

                    if (placeholderText != null)
                    {
                        // Ẩn TextBlock
                        placeholderText.Visibility = Visibility.Collapsed;
                    }
                }
            }
        }
        private void dgData_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            User selectedUser = dgData.SelectedItem as User;

            if (selectedUser != null)
            {

                FillElement(selectedUser);
            }

            else
            {

                ResetInput();
            }
        }


    }
}
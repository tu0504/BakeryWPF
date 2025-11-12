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
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            RegisterWindow regis = new();
            regis.ShowDialog();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            AdminDashboardWindow adminDashboard = new AdminDashboardWindow();
            adminDashboard.ShowDialog();
        }




        private void txtUser_GotFocus(object sender, RoutedEventArgs e)
        {
            txtPlaceholder.Visibility = Visibility.Collapsed;
        }
        private void txtUser_LostFocus(object sender, RoutedEventArgs e)
        {
            txtPlaceholder.Visibility =
                string.IsNullOrWhiteSpace(txtUser.Text) ? Visibility.Visible : Visibility.Collapsed;
        }
        private void txtPass_GotFocus(object sender, RoutedEventArgs e)
        {
            txtPlaceholderP.Visibility = Visibility.Collapsed;
        }
        private void txtPass_LostFocus(object sender, RoutedEventArgs e)
        {
            txtPlaceholderP.Visibility =
                string.IsNullOrWhiteSpace(txtPlaceholderP.Text) ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}

using Bakery.Service.Implement;
using Bakery.Service.Interface;
using Bakery.Service;
using Bakery.WpfApplication.View;
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
    /// Interaction logic for AdminDashboardWindow.xaml
    /// </summary>
    public partial class AdminDashboardWindow : Window
    {
    private readonly IUserService _userService;

        public AdminDashboardWindow()
        {
            InitializeComponent();
            // initialize a default user service for admin views
            _userService = new Bakery.Service.UserService();
            // show orders view by default
            try
            {
                ContentArea.Content = new View.OrderManagement();
            }
            catch
            {
                // ignore — UI will show nothing if content fails to load here
            }
        }

        private void btnHome_OnClick(object sender, RoutedEventArgs e)
        {

        }

        private void btnUser_Click(object sender, RoutedEventArgs e)
        {
            ContentArea.Content = new View.UserManagement(_userService);
        }
        private void btnBakery_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnOrder_Click(object sender, RoutedEventArgs e)
        {
            ContentArea.Content = new View.OrderManagement();

        }

        private void btnLogOut_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}

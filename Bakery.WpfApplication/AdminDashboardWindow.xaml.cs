using Bakery.Service;
using Bakery.Service.Implement;
using Bakery.Service.Interface;
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
using System.Windows.Media.Animation;
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
        private bool _isMenuOpen = false;

        public AdminDashboardWindow()
        {
            InitializeComponent();
          
            _userService = new Bakery.Service.UserService();
        
        }

        private void btnHome_OnClick(object sender, RoutedEventArgs e)
        {


            double expandedHeight = SubMenu.ActualHeight;   // chiều cao thật của submenu
            double collapsedHeight = 0;

            DoubleAnimation animation = new DoubleAnimation
            {
                Duration = TimeSpan.FromMilliseconds(250),
                AccelerationRatio = 0.3,
                DecelerationRatio = 0.3
            };

            if (_isMenuOpen)
            {
                animation.From = expandedHeight;
                animation.To = collapsedHeight;
            }
            else
            {
                animation.From = collapsedHeight;
                animation.To = expandedHeight;
            }

            SubMenuContainer.BeginAnimation(HeightProperty, animation);
            _isMenuOpen = !_isMenuOpen;

        }

        private void btnUser_Click(object sender, RoutedEventArgs e)
        {
            ContentArea.Content = new View.UserManagement(_userService);
        }
        private void btnBakery_Click(object sender, RoutedEventArgs e)
        {
        ContentArea.Content = new View.BakeryManagement();
        }

        private void btnOrder_Click(object sender, RoutedEventArgs e)
        {
            ContentArea.Content = new View.OrderManagement();

        }

        private void btnLogOut_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow l = new();
            l.Show();
            Close();
        }
    }
}

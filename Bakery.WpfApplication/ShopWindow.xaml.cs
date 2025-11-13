using Bakery.Repository.Models;
using Bakery.Service;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Bakery.WpfApplication
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class ShopWindow : Window
    {
        private readonly UserService _userServices;
        private readonly User user;
        private readonly OrderService _orderService;
        private Order order;
        private List<OrderDetail> orderDetails = new List<OrderDetail>();

        public ShopWindow(User user)
        {
            InitializeComponent();
            _userServices = new UserService();
            Login.Content = $"Hello, {user.UserName}";
            //_orderService = new OrderService();
            orderDetails = new List<OrderDetail>();
            order = new Order();
            this.user = user;
        }
        private void bakeryList_Click(object sender, RoutedEventArgs e)
        {
            Random rnd = new Random();
            order = new Order
            {
                UserId = user.UserId,
                OrderDate = DateTime.Now,
                TotalAmount = 0,
                Status = "Pending"
            };
            int cartItems = int.Parse(CartItems.Text);
            ContentArea.Content = new Shop.BakeryList(order, cartItems, this);
        }

        public void AddOrderDetail(OrderDetail orderDetail)
        {
            orderDetails.Add(orderDetail);
        }

        public void UpdateCartItems(int count)
        {
            CartItems.Text = count.ToString();
        }

        private void btnShoppingClick(object sender, RoutedEventArgs e)
        {
            ContentArea.Content = new Shop.CheckOut(order, orderDetails, this);
        }

        private void logOut_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow login = new LoginWindow();
            login.Show();
            this.Close();
        }

        private void CloseButton(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
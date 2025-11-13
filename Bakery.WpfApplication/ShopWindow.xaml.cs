using Bakery.Repository.Models;
using Bakery.Service;
using Bakery.WpfApplication.Shop;
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
        private readonly OrderService _orderService;
        private readonly ProductService _productService;
        private readonly OrderDetailService _orderDetailService;
        private readonly CategoryService _categoryService;
        private readonly User user;
        private Order order;
        private List<OrderDetail> orderDetails = new List<OrderDetail>();

        public ShopWindow(User user, UserService userService, OrderService orderService, OrderDetailService orderDetailService, ProductService productService, CategoryService categoryService)
        {
            InitializeComponent();
            _userServices = userService;
            _productService = productService;
            _orderDetailService = orderDetailService;
            Login.Content = $"Hello, {user.UserName}";
            _orderService = orderService;
            orderDetails = new List<OrderDetail>();
            order = new Order();
            this.user = user;
            _categoryService = categoryService;
        }
        public ShopWindow(User user)
    : this(user, new UserService(), new OrderService(), new OrderDetailService(), new ProductService(), new CategoryService())
        {
        }

        public void UpdateCartItems(int count)
        {
            CartItems.Text = count.ToString();
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }
        private bool IsMaximize = false;
        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                if (IsMaximize)
                {
                    this.WindowState = WindowState.Normal;
                    this.Width = 1250;
                    this.Height = 830;

                    IsMaximize = false;
                }
                else
                {
                    this.WindowState = WindowState.Maximized;

                    IsMaximize = true;
                }
            }
        }

        private void bakeryList_Click(object sender, RoutedEventArgs e)
        {
            order = new Order
            {
                UserId = user.UserId,
                OrderDate = DateTime.Now,
                TotalAmount = 0,
                Status = "Pending"
            };
            int cartItems = int.Parse(CartItems.Text);

            ContentArea.Content = new BakeryList(order, cartItems, this,
                _orderService, _productService, _orderDetailService, _categoryService);
        }

        public void AddOrderDetail(OrderDetail orderDetail)
        {
            orderDetails.Add(orderDetail);
        }

        private void btnShoppingClick(object sender, RoutedEventArgs e)
        {
            ContentArea.Content = new Shop.CheckOut(order, orderDetails, _productService, _orderDetailService ,_orderService, this);
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
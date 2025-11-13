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

        public User _currentUser = new User();
        public Order _currentOrder = new Order();
        public List<OrderDetail> _orderDetails = new List<OrderDetail>();

        public ShopWindow(User user)
        {
            InitializeComponent();
            _currentUser = user;
            Login.Content = $"Hello, {_currentUser.UserName}";
            _userServices = new UserService();
            _productService = new ProductService();
            _orderDetailService = new OrderDetailService();
            _orderService = new OrderService();
            _categoryService = new CategoryService();
        }

        public void UpdateCartItems(int count)
        {
            CartItems.Text = count.ToString();
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                DragMove();
            }
        }
        private bool IsMaximize = false;
        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                if (IsMaximize)
                {
                    WindowState = WindowState.Normal;
                    Width = 1250;
                    Height = 830;
                   
                    IsMaximize = false;
                }
                else
                {
                    WindowState = WindowState.Maximized;

                    IsMaximize = true;
                }
            }
        }

        private void bakeryList_Click(object sender, RoutedEventArgs e)
        {
            _currentOrder = new Order
            {
                UserId = _currentUser.UserId,
                OrderDate = DateTime.Now,
                TotalAmount = 0,
                Status = "Pending"
            };

            int cartItems = int.Parse(CartItems.Text);

            ContentArea.Content = new BakeryList(
                _currentOrder,
                cartItems,
                this,
                _orderService,
                _productService,
                _orderDetailService,
                _categoryService
            );
        }


        public void AddOrderDetail(OrderDetail orderDetail)
        {
            // Load the Product if it's not already loaded
            if (orderDetail.Product == null && orderDetail.ProductId > 0)
            {
                orderDetail.Product = _productService.GetProductById(orderDetail.ProductId);
            }

            // Check if the product already exists in the cart
            var existingOrderDetail = _orderDetails.FirstOrDefault(od => od.ProductId == orderDetail.ProductId);

            if (existingOrderDetail != null)
            {
                // Product already exists, increase the quantity
                existingOrderDetail.Quantity += orderDetail.Quantity;
            }
            else
            {
                // Product doesn't exist, add it to the cart
                _orderDetails.Add(orderDetail);
            }
        }

        private void logOut_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow login = new LoginWindow();
            login.Show();
            Close();
        }

        private void btnBuy_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Validate cart is not empty
                if (_orderDetails == null || _orderDetails.Count == 0)
                {
                    MessageBox.Show("Your cart is empty. Please add products before checking out.",
                        "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Calculate total amount
                decimal totalAmount = _orderDetails.Sum(od => od.Quantity * od.UnitPrice);

                // Confirm quick checkout
                var result = MessageBox.Show(
                    $"Quick Checkout\n\nItems in cart: {_orderDetails.Count}\nTotal Amount: {totalAmount:C}\n\nDo you want to complete this order?",
                    "Confirm Quick Checkout",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (result == MessageBoxResult.No)
                {
                    return;
                }

                // Create the order with all order details
                var newOrder = new Order
                {
                    UserId = _currentUser.UserId,
                    OrderDate = DateTime.Now,
                    TotalAmount = totalAmount,
                    Status = "Completed"
                };

                // Add all order details to the order before saving
                foreach (var orderDetail in _orderDetails)
                {
                    var newOrderDetail = new OrderDetail
                    {
                        ProductId = orderDetail.ProductId,
                        Quantity = orderDetail.Quantity,
                        UnitPrice = orderDetail.UnitPrice
                    };

                    newOrder.OrderDetails.Add(newOrderDetail);
                }

                // Save the complete order with all order details in one transaction
                _orderService.Create(newOrder);

                // Update product stock
                foreach (var orderDetail in _orderDetails)
                {
                    var product = _productService.GetProductById(orderDetail.ProductId);
                    if (product != null)
                    {
                        product.Stock -= orderDetail.Quantity;
                        _productService.UpdateProduct(product);
                    }
                }

                // Show success message
                MessageBox.Show(
                    $"Order completed successfully!\n\nTotal Amount: {totalAmount:C}\n\nThank you for your purchase!",
                    "Success",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                // Clear the cart
                _orderDetails.Clear();
                UpdateCartItems(0);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred during quick checkout: {ex.Message}",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnShoppingCart_Click(object sender, RoutedEventArgs e)
        {
            ContentArea.Content = new CheckOut(this, _currentOrder, _orderDetails);
        }
    }
}
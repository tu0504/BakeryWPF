using Bakery.Repository.Models;
using Bakery.Service;
using System.Windows;
using System.Windows.Controls;

namespace Bakery.WpfApplication.Shop
{
    /// <summary>
    /// Interaction logic for CheckOut.xaml
    /// </summary>
    public partial class CheckOut : UserControl
    {
        private readonly ProductService _productService;
        private readonly OrderDetailService _orderDetailService;
        private readonly OrderService _orderService;
        private readonly ShopWindow _shopWindow;
        public Order _currentOrder;
        public List<OrderDetail> _currentOrderDetails;

        public CheckOut(ShopWindow shopWindow, Order currentOrder, List<OrderDetail> currentOrderDetail)
        {
            InitializeComponent();
            _productService = new ProductService();
            _orderDetailService = new OrderDetailService();
            _orderService = new OrderService();
            _shopWindow = shopWindow;
            _currentOrder = currentOrder;
            _currentOrderDetails = currentOrderDetail;
        }

        private void FillDataGrid()
        {
            dgData.ItemsSource = null;
            dgData.ItemsSource = _currentOrderDetails;

            decimal totalPrice = _currentOrderDetails.Sum(od => od.Quantity * od.UnitPrice);
            txtTotalPrice.Text = totalPrice.ToString("đ");
        }

        private void dgData_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedOrderDetail = dgData.SelectedItem as OrderDetail;
            txtQuantity.Text = selectedOrderDetail != null ? selectedOrderDetail.Quantity.ToString() : "0";
        }

        private void btnUpdateQuantity_Click(object sender, RoutedEventArgs e)
        {
            // Check if an item is selected
            if (dgData.SelectedItem == null)
            {
                MessageBox.Show("Please select a product to update quantity.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Get the selected OrderDetail
            var selectedOrderDetail = dgData.SelectedItem as OrderDetail;
            if (selectedOrderDetail == null)
            {
                return;
            }

            // Validate quantity input
            if (!int.TryParse(txtQuantity.Text, out int newQuantity) || newQuantity <= 0)
            {
                MessageBox.Show("Please enter a valid quantity (greater than 0).", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtQuantity.Text = selectedOrderDetail.Quantity.ToString();
                return;
            }

            // Check if product has enough stock
            var product = _productService.GetProductById(selectedOrderDetail.ProductId);
            if (product != null)
            {
                int currentQuantityInCart = selectedOrderDetail.Quantity;
                int quantityDifference = newQuantity - currentQuantityInCart;

                if (quantityDifference > 0 && product.Stock < quantityDifference)
                {
                    MessageBox.Show($"Not enough stock available. Only {product.Stock} items left in stock.", 
                        "Insufficient Stock", MessageBoxButton.OK, MessageBoxImage.Warning);
                    txtQuantity.Text = currentQuantityInCart.ToString();
                    return;
                }
            }

            // Update the quantity
            selectedOrderDetail.Quantity = newQuantity;

            // Refresh the DataGrid and total price
            FillDataGrid();

            MessageBox.Show($"Quantity updated successfully to {newQuantity}.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (_currentOrder != null && _currentOrderDetails != null)
            {
                FillDataGrid();
            }

            if (_currentOrderDetails.Count == 0)
            {
                btnCheckout.IsEnabled = false;
                btnRemoveProduct.IsEnabled = false;
                MessageBox.Show("Your cart is empty!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void btnCheckout_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Validate cart is not empty
                if (_currentOrderDetails == null || _currentOrderDetails.Count == 0)
                {
                    MessageBox.Show("Your cart is empty. Please add products before checking out.",
                        "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Confirm checkout
                var result = MessageBox.Show(
                    $"Are you sure you want to complete this order?\n\nTotal Amount: {txtTotalPrice.Text}",
                    "Confirm Checkout",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (result == MessageBoxResult.No)
                {
                    return;
                }

                // Calculate total amount
                decimal totalAmount = _currentOrderDetails.Sum(od => od.Quantity * od.UnitPrice);

                // Create the order with all order details
                var newOrder = new Order
                {
                    UserId = _currentOrder.UserId,
                    OrderDate = DateTime.Now,
                    TotalAmount = totalAmount,
                    Status = "Pending"
                };

                // Add all order details to the order before saving
                foreach (var orderDetail in _currentOrderDetails)
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
                foreach (var orderDetail in _currentOrderDetails)
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
                _currentOrderDetails.Clear();
                _shopWindow.UpdateCartItems(0);

                // Refresh the UI
                FillDataGrid();

                // Disable checkout button
                btnCheckout.IsEnabled = false;
                btnRemoveProduct.IsEnabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred during checkout: {ex.Message}",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnRemoveProduct_Click(object sender, RoutedEventArgs e)
        {
            // Check if an item is selected
            if (dgData.SelectedItem == null)
            {
                MessageBox.Show("Please select a product to remove.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Get the selected OrderDetail
            var selectedOrderDetail = dgData.SelectedItem as OrderDetail;
            if (selectedOrderDetail == null)
            {
                return;
            }

            // Confirm removal
            var result = MessageBox.Show(
                $"Are you sure you want to remove '{selectedOrderDetail.Product?.ProductName}' from your cart?",
                "Confirm Removal",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                // Remove from the list
                _currentOrderDetails.Remove(selectedOrderDetail);

                // Update the cart count in ShopWindow
                _shopWindow.UpdateCartItems(_currentOrderDetails.Count);

                // Refresh the DataGrid and total price
                FillDataGrid();

                // Disable buttons if cart is empty
                if (_currentOrderDetails.Count == 0)
                {
                    btnCheckout.IsEnabled = false;
                    btnRemoveProduct.IsEnabled = false;
                    MessageBox.Show("Your cart is now empty!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }
    }
}
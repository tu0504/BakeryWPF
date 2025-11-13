using Bakery.Repository.Context;
using Bakery.Repository.Models;
using Bakery.Service;
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

namespace Bakery.WpfApplication.Shop
{
    /// <summary>
    /// Interaction logic for BakeryList.xaml
    /// </summary>
    public partial class BakeryList : UserControl
    {
        private readonly ProductService productService;
        private readonly OrderDetailService orderDetailService;
        private readonly OrderService orderService;
        private readonly ShopWindow _shopWindow;
        private Order _order;
        private int count;
        public BakeryList(Order order, int cartItems, ShopWindow shopWindow)
        {
            InitializeComponent();
            productService = new ProductService();
            orderDetailService = new OrderDetailService();
            orderService = new OrderService();
            _order = order;
            count = cartItems;
            _shopWindow = shopWindow;

            LoadProducts();
        }

        private void LoadProducts()
        {
            using var db = new BakeryContext();
            var category = new CategoryService();
            List<Product> productList = productService.GetAllProducts().ToList();

            foreach (var product in productList)
            {
                Border productBorder = new Border
                {
                    BorderBrush = System.Windows.Media.Brushes.Gray,
                    BorderThickness = new Thickness(2),
                    CornerRadius = new CornerRadius(5),
                    Margin = new Thickness(10),
                    Width = 200
                };

                StackPanel productPanel = new StackPanel { Margin = new Thickness(10) };

                if (!string.IsNullOrEmpty(product.ImageUrl))
                {
                    Image productImage = new Image
                    {
                        Source = new BitmapImage(new Uri(product.ImageUrl, UriKind.RelativeOrAbsolute)),
                        Width = 100,
                        Height = 150,
                        Margin = new Thickness(5)
                    };
                    productPanel.Children.Add(productImage);
                }

                // Hiển thị thông tin sản phẩm
                productPanel.Children.Add(new TextBlock { Text = $"ID: {product.ProductId}", Margin = new Thickness(5), FontWeight = FontWeights.Bold });
                productPanel.Children.Add(new TextBlock { Text = $"Name: {product.ProductName}", Margin = new Thickness(5), FontWeight = FontWeights.Bold });
                productPanel.Children.Add(new TextBlock { Text = $"Price: ${product.Price}", Margin = new Thickness(5) });
                productPanel.Children.Add(new TextBlock { Text = $"Stock: {product.Stock}", Margin = new Thickness(5) });

                string categoryName = category.GetCategoryById(product.CategoryId).CategoryName;
                productPanel.Children.Add(new TextBlock { Text = $"Category: {categoryName}", Margin = new Thickness(5) });

                productBorder.Child = productPanel;

                // Thêm nút "Add to cart"
                Button buttonCard = new Button
                {
                    Content = "Add to cart",
                    Margin = new Thickness(0, 20, 0, 0),
                    Height = 30,
                    Width = 100
                };

                buttonCard.Click += (s, e) =>
                {
                    OrderDetail orderDetail = new OrderDetail
                    {
                        ProductId = product.ProductId,
                        OrderId = _order.OrderId,
                        Quantity = 1,
                        UnitPrice = product.Price
                    };

                    ++count;
                    _shopWindow.AddOrderDetail(orderDetail);
                    _shopWindow.UpdateCartItems(count);

                    MessageBox.Show("Added to cart successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                };

                productPanel.Children.Add(buttonCard);

                // Thêm nút "Buy Now" (nếu bạn muốn xử lý mua ngay)
                //Button buttonBuy = new Button
                //{
                //    Content = "Buy Now",
                //    Margin = new Thickness(0, 10, 0, 0),
                //    Height = 30,
                //    Width = 100
                //};

                //buttonBuy.Click += (s, e) =>
                //{
                //    // Logic mua ngay: thêm vào Order, rồi chuyển đến trang thanh toán
                //    OrderDetail orderDetail = new OrderDetail
                //    {
                //        ProductId = product.ProductId,
                //        OrderId = _order.OrderId,
                //        Quantity = 1,
                //        UnitPrice = product.Price
                //    };

                //    _shopWindow.AddOrderDetail(orderDetail);
                //    _order.TotalAmount += product.Price;
                //    orderService.UpdateOrder(_order);

                //    MessageBox.Show("Purchase successful!", "Order", MessageBoxButton.OK, MessageBoxImage.Information);
                //};

                //productPanel.Children.Add(buttonBuy);
                DataWrapPanel.Children.Add(productBorder);
            }
        }
    }
}

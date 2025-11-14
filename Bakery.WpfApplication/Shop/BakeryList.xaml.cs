using Bakery.Repository.Context;
using Bakery.Repository.Models;
using Bakery.Service;
using System;
using System.Collections.Generic;
using System.IO;
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
        private readonly ProductService _productService;
        private readonly OrderDetailService _orderDetailService;
        private readonly OrderService _orderService;
        private readonly CategoryService _categoryService;
        private readonly ShopWindow _shopWindow;
        private readonly Order _order;
        private int count;

        public BakeryList(Order order, int cartItems, ShopWindow shopWindow,
                          OrderService orderService,
                          ProductService productService,
                          OrderDetailService orderDetailService,
                          CategoryService categoryService)
        {
            InitializeComponent();

            _order = order;
            count = cartItems;
            _shopWindow = shopWindow;

            _orderService = orderService;
            _productService = productService;
            _orderDetailService = orderDetailService;
            _categoryService = categoryService;

            // Gọi load sản phẩm **sau khi service đã gán**
            LoadProducts();
        }

        private void LoadProducts()
        {
            if (_productService == null || _categoryService == null)
            {
                MessageBox.Show("Services not initialized!");
                return;
            }

            var products = _productService.GetAllProducts().ToList();
            DataWrapPanel.Children.Clear();
            if (_productService == null || _categoryService == null)
            {
                MessageBox.Show("Services not initialized!");
                return;
            }

            var products = _productService.GetAllProducts().ToList();
            DataWrapPanel.Children.Clear();

            foreach (var product in products)
            {
                Border border = new Border
                {
                    Width = 180,
                    MinHeight = 250,
                    Margin = new Thickness(10),
                    Background = Brushes.White,
                    BorderBrush = Brushes.Gray,
                    BorderThickness = new Thickness(1),
                    CornerRadius = new CornerRadius(5)
                };

                StackPanel panel = new StackPanel { Margin = new Thickness(5) };
                if (!string.IsNullOrEmpty(product.ImageUrl))
                {
                    try
                    {

                        panel.Children.Add(new Image
                        {
                            Source = new BitmapImage(new Uri($"pack://application:,,,/{product.ImageUrl}")),
                            Width = 150,
                            Height = 120
                        });
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Error loading image", ex);
                    }

                }

                var productPrice = (int)product.Price;
                panel.Children.Add(new TextBlock { Text = product.ProductName, FontWeight = FontWeights.Bold, Margin = new Thickness(5) });
                panel.Children.Add(new TextBlock { Text = $"Price: {productPrice}đ", Margin = new Thickness(5) });

                Button btn = new Button { Content = "Add to cart", Width = 100, Margin = new Thickness(0, 5, 0, 0),
                    Background = Brushes.Orange,
                    Foreground = Brushes.White,
                    FontWeight = FontWeights.Bold,
                    BorderBrush = Brushes.DarkOrange,
                    Cursor = Cursors.Hand
                };
                btn.Click += (s, e) =>
                {
                    var detail = new OrderDetail
                    {
                        OrderId = _order.OrderId,
                        ProductId = product.ProductId,
                        Quantity = 1,
                        UnitPrice = product.Price
                    };
                    count++;
                    _shopWindow.AddOrderDetail(detail);
                    _shopWindow.UpdateCartItems(count);
                    MessageBox.Show("Added to cart!", "Notice", MessageBoxButton.OK, MessageBoxImage.Information);
                };

                panel.Children.Add(btn);
                border.Child = panel;
                DataWrapPanel.Children.Add(border);
            }
        }
    }

}

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

namespace Bakery.WpfApplication.View
{
    /// <summary>
    /// Interaction logic for BakeryManagement.xaml
    /// </summary>
    public partial class BakeryManagement : UserControl
    {
        private readonly ProductService _productService;
        public BakeryManagement()
        {
            InitializeComponent();
            _productService = new ProductService();

        }
        private void txtSearch_GotFocus(object sender, RoutedEventArgs e)
        {
            txtPlaceholder.Visibility = Visibility.Collapsed;
        }

        private void txtSearch_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                txtPlaceholder.Visibility = Visibility.Visible;
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

        }

        public void FillDataGrid(List<Product> data)
        {
            dgData.ItemsSource = null;
            dgData.ItemsSource = data;
        }

        private void dgData_Loaded(object sender, RoutedEventArgs e)
        {
            FillDataGrid(_productService.GetAllProducts());

        }
    }
}

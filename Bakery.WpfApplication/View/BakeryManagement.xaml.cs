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
        private  ProductService _productService = new();
        private  CategoryService _categoryService;
        public BakeryManagement(ProductService productService, CategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
            InitializeComponent();
   
        }

        public void LoadProduct()
        {
            try
            {

                var bakeryList = _productService.GetAllProducts();
                dgData.ItemsSource = bakeryList;

            }
            catch (Exception ex) { }
        }

        private void BakeryManagement_OnLoaded(object sender, RoutedEventArgs e)
        {
            LoadProduct();
        }

        private void btnCreateKoi(object sender, RoutedEventArgs e)
        {

        }

        private void BrowseImage_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnDeleteKoi(object sender, RoutedEventArgs e)
        {

        }
        private void btnSearch_Click (object sender, RoutedEventArgs e)
        {

        }

        private void DgData_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void btnUpdateKoi(object sender, RoutedEventArgs e)
        {

        }

        private void txtSearch_GotFocus(object sender, RoutedEventArgs e)
        {
            txtPlaceholder.Visibility = Visibility.Collapsed;
        }

        private void txtSearch_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtSearch.Text))
            {
                txtPlaceholder.Visibility = Visibility.Visible;
            }
        }

        private void dgData_Loaded(object sender, RoutedEventArgs e)
        {
            LoadProduct();
        }
    }
}

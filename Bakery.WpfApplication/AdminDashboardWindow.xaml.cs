using Bakery.Repository.Models;
using Bakery.Service;
using System.Windows;


namespace Bakery.WpfApplication
{
    /// <summary>
    /// Interaction logic for AdminDashboardWindow.xaml
    /// </summary>
    public partial class AdminDashboardWindow : Window
    {
        public ProductService _productService;
        public CategoryService _categoryService;

        public AdminDashboardWindow()
        {
            InitializeComponent();
           
        }
        public AdminDashboardWindow(ProductService productService, CategoryService categoryService)
        {
            InitializeComponent();
            _productService = productService;
            _categoryService = categoryService;
           
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
            ContentArea.Content = new View.BakeryManagement(_productService, _categoryService);
        }

        private void btnOrder_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnLogOut_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}

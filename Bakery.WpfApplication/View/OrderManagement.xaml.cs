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
    /// Interaction logic for OrderManagement.xaml
    /// </summary>
    public partial class OrderManagement : UserControl
    {
        private readonly Bakery.Service.IOrderService _orderService;

        public OrderManagement()
        {
            InitializeComponent();

            // create repository + service instances
            var orderRepo = new Bakery.Repository.Repositories.OrderRepo();
            _orderService = new Bakery.Service.OrderService(orderRepo);

            this.Loaded += OrderManagement_Loaded;
            dgData.SelectionChanged += DgData_SelectionChanged;
        }

        private void OrderManagement_Loaded(object sender, RoutedEventArgs e)
        {
            LoadOrders();
        }

        private void LoadOrders()
        {
            try
            {
                var orders = _orderService.GetAll();
                if (orders == null)
                {
                    dgData.ItemsSource = new List<Bakery.Repository.Models.Order>();
                    dgDetails.ItemsSource = null;
                    MessageBox.Show("No orders found.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                dgData.ItemsSource = orders;
                dgDetails.ItemsSource = null;
            }
            catch (Exception ex)
            {
                // Try to call repository directly to get more detailed error information
                try
                {
                    var repo = new Bakery.Repository.Repositories.OrderRepo();
                    var orders = repo.GetAll();
                    dgData.ItemsSource = orders ?? new List<Bakery.Repository.Models.Order>();
                    dgDetails.ItemsSource = null;
                    return;
                }
                catch (Exception inner)
                {
                    // Show both messages to help diagnose DB/EF problems
                    MessageBox.Show($"Failed to load orders: {ex.Message}\nDetail: {inner.Message}\n\nSee inner exception for more details.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void DgData_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selected = dgData.SelectedItem as Bakery.Repository.Models.Order;
            if (selected == null)
            {
                dgDetails.ItemsSource = null;
                return;
            }

            // Populate the details grid with the selected order's details
            dgDetails.ItemsSource = selected.OrderDetails;

            // Populate simple form fields
            txtOrderId.Text = selected.OrderId.ToString();
            txtDate.Text = selected.OrderDate?.ToString("g") ?? string.Empty;
            txtTotalMoney.Text = selected.TotalAmount.ToString();
            cboUser.ItemsSource = new List<string> { selected.User?.UserName ?? string.Empty };
            cboUser.SelectedIndex = 0;
        }
    }
}

using Bakery.Repository.Repositories;
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
    public partial class OrderManagement : UserControl
    {
        private readonly Bakery.Service.IOrderService _orderService;
        private readonly Bakery.Service.OrderDetailService _orderDetailService;

        public OrderManagement()
        {
            InitializeComponent();

            var orderRepo = new Bakery.Repository.Repositories.OrderRepo();
            _orderService = new Bakery.Service.OrderService(orderRepo);
            var orderDetailRepo = new Bakery.Repository.Repositories.OrderDetailRepo();
            _orderDetailService = new Bakery.Service.OrderDetailService(orderDetailRepo);

            this.Loaded += OrderManagement_Loaded;
            dgData.SelectionChanged += DgData_SelectionChanged;
            dgDetails.SelectionChanged += DgDetails_SelectionChanged;
            btnDeleteOrder.IsEnabled = false;
            btnDeleteOrderDetail.IsEnabled = false;
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
                if (dgData.Items.Count > 0)
                {
                    dgData.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                try
                {
                    var repo = new Bakery.Repository.Repositories.OrderRepo();
                    var orders = repo.GetAll();
                    dgData.ItemsSource = orders ?? new List<Bakery.Repository.Models.Order>();
                    dgDetails.ItemsSource = null;
                    if (dgData.Items.Count > 0)
                    {
                        dgData.SelectedIndex = 0;
                    }
                    return;
                }
                catch (Exception inner)
                {
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
                btnDeleteOrder.IsEnabled = false;
                return;
            }

            dgDetails.ItemsSource = selected.OrderDetails;

            txtOrderId.Text = selected.OrderId.ToString();
            txtDate.Text = selected.OrderDate?.ToString("g") ?? string.Empty;
            txtTotalMoney.Text = selected.TotalAmount.ToString("F2");
            cboUser.ItemsSource = new List<string> { selected.User?.UserName ?? string.Empty };
            cboUser.SelectedIndex = 0;
            txtStatus.Text = selected.Status ?? string.Empty;
            btnDeleteOrder.IsEnabled = true;
        }

        private void DgDetails_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedDetail = dgDetails.SelectedItem as Bakery.Repository.Models.OrderDetail;
            if (selectedDetail == null)
            {
                cboKoi.ItemsSource = null;
                txtQuantity.Text = string.Empty;
                txtPrice.Text = string.Empty;
                btnDeleteOrderDetail.IsEnabled = false;
                return;
            }

            cboKoi.ItemsSource = new List<string> { selectedDetail.Product?.ProductName ?? string.Empty };
            cboKoi.SelectedIndex = 0;
            txtQuantity.Text = selectedDetail.Quantity.ToString();
            txtPrice.Text = selectedDetail.UnitPrice.ToString("F2");
            btnDeleteOrderDetail.IsEnabled = true;
        }

        private void btnDeleteOrder_Click(object sender, RoutedEventArgs e)
        {
            var selected = dgData.SelectedItem as Bakery.Repository.Models.Order;
            if (selected == null)
            {
                MessageBox.Show("No order selected.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var result = MessageBox.Show($"Are you sure you want to delete Order #{selected.OrderId}?", "Confirm delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result != MessageBoxResult.Yes) return;

            try
            {
                var ok = _orderService.Remove(selected);
                if (ok)
                {
                    MessageBox.Show("Order deleted.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    var prevId = selected.OrderId;
                    LoadOrders();
                    foreach (var o in dgData.Items)
                    {
                        if (o is Bakery.Repository.Models.Order ord && ord.OrderId == prevId)
                        {
                            dgData.SelectedItem = ord;
                            break;
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Failed to delete order. It may not exist.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting order: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                btnDeleteOrder.IsEnabled = false;
                btnDeleteOrderDetail.IsEnabled = false;
            }
        }

        private void btnDeleteOrderDetail_Click(object sender, RoutedEventArgs e)
        {
            var selectedDetail = dgDetails.SelectedItem as Bakery.Repository.Models.OrderDetail;
            if (selectedDetail == null)
            {
                MessageBox.Show("No order detail selected.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var result = MessageBox.Show($"Delete detail for product '{selectedDetail.Product?.ProductName}' (Qty {selectedDetail.Quantity})?", "Confirm delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result != MessageBoxResult.Yes) return;

            try
            {
                var ok = _orderDetailService.Remove(selectedDetail);
                if (ok)
                {
                    MessageBox.Show("Order detail deleted.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                    var selectedOrder = dgData.SelectedItem as Bakery.Repository.Models.Order;
                    if (selectedOrder != null)
                    {
                        try
                        {
                            using (var ctx = new Bakery.Repository.Context.BakeryContext())
                            {
                                var repo = new OrderRepo();
                                var fresh = repo.GetById(selectedOrder.OrderId);
                                dgDetails.ItemsSource = fresh?.OrderDetails ?? new List<Bakery.Repository.Models.OrderDetail>();
                            }
                            dgData.ItemsSource = _orderService.GetAll();
                        }
                        catch
                        {
                            LoadOrders();
                        }
                    }
                    else
                    {
                        LoadOrders();
                    }
                }
                else
                {
                    MessageBox.Show("Failed to delete order detail.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting order detail: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                btnDeleteOrderDetail.IsEnabled = false;
            }
        }
    }
}

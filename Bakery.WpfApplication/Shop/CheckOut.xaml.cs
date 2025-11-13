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
    /// Interaction logic for CheckOut.xaml
    /// </summary>
    public partial class CheckOut : UserControl
    {
        public CheckOut(Order order, List<OrderDetail> detailList, ProductService productService, OrderDetailService orderDetailService, OrderService orderService, ShopWindow shopWindow)
        {
            InitializeComponent();
        }
        private void btnCheckout(object sender, RoutedEventArgs e)
        {
           
        }

       
        private void btnDeleteOrder(object sender, RoutedEventArgs e)
        {
     
         
        }
        private void DgData_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           
        }
    }
}

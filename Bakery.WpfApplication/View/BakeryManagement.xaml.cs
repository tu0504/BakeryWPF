using Bakery.Repository.Models;
using Bakery.Service;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

using System.Windows.Input;
using System.Windows.Media;


namespace Bakery.WpfApplication.View
{
    /// <summary>
    /// Interaction logic for BakeryManagement.xaml
    /// </summary>
    public partial class BakeryManagement : UserControl
    {
        private readonly ProductService _productService;
        private readonly CategoryService _categoryService;
        public BakeryManagement()
        {
            InitializeComponent();
            _productService = new ProductService();
            _categoryService = new CategoryService();

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
            var categories = _categoryService.GetAllCategories();
            cboCategory.ItemsSource = categories;
            cboCategory.DisplayMemberPath = "CategoryName";
            cboCategory.SelectedValuePath = "CategoryId";
            FillDataGrid(_productService.GetAllProducts());
         
        }

        public void FillDataGrid(List<Product> data)
        {
            dgData.ItemsSource = null;
            
            dgData.ItemsSource = data;
        }

        private void dgData_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgData.SelectedItem is Product selected)
            {
                BakeryId.Text = selected.ProductId.ToString();
                BakeryName.Text = selected.ProductName;
                Description.Text = selected.Description;
                Price.Text = selected.Price.ToString();
                Stock.Text = selected.Stock.ToString();
                CreatedAt.Text = selected.CreatedAt?.ToString("yyyy-MM-dd") ?? "";

                cboCategory.SelectedValue = selected.CategoryId;

                if (!string.IsNullOrEmpty(selected.ImageUrl))
                {
                    Image.Text = selected.ImageUrl.Replace("pack://application:,,,/", "");
                }

                CreateButton.IsEnabled = false;
                UpdateButton.IsEnabled = true;
                DeleteButton.IsEnabled = true;
            }
            else
            {
                CreateButton.IsEnabled = true;
                UpdateButton.IsEnabled = false;
                DeleteButton.IsEnabled = false;
            }
        }

        private void dgData_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var depObj = (DependencyObject)e.OriginalSource;

          
            while (depObj != null && depObj is not DataGridCell && depObj is not DataGridColumnHeader)
            {
                depObj = VisualTreeHelper.GetParent(depObj);
            }

            if (depObj == null)
            {

                ResetTextBox();
            }
        }
        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            string name = txtSearch.Text.Trim();
            if (string.IsNullOrWhiteSpace(name))
            {
                FillDataGrid(_productService.GetAllProducts());
            }
            else 
            {
                List <Product> products = _productService.GetProductsByName(name);
                if (products.Any())
                {
                    dgData.ItemsSource = products;
                }
                else
                {
                    MessageBox.Show("No product found.", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }


        }
        public void ResetTextBox ()
        {
            dgData.UnselectAll();
            BakeryId.Clear();
            BakeryName.Clear();
            Description.Clear();
            Price.Clear();
            Stock.Clear();
            CreatedAt.Clear();
            Image.Clear();
            cboCategory.SelectedIndex = -1;
        }

      
        private void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.OpenFileDialog
            {
                Title = "Chọn hình ảnh sản phẩm",
                Filter = "Image files (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png|All files (*.*)|*.*"
            };

            if (dialog.ShowDialog() == true)
            {
             
                string selectedFile = dialog.FileName;

            
                string fileName = System.IO.Path.GetFileName(selectedFile);
                string relativePath = $"Images/{fileName}";

               
                Image.Text = relativePath;

            
                if (dgData.SelectedItem is Product selected)
                {
                    selected.ImageUrl = relativePath;
                }

        
                string destPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images", fileName);
                if (!System.IO.File.Exists(destPath))
                {
                    System.IO.File.Copy(selectedFile, destPath, overwrite: false);
                }
            }
        }

        private void cboCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgData.SelectedItem is Product selected && cboCategory.SelectedValue is int categoryId)
            {
                selected.CategoryId = categoryId;
            }
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            var selected = (Product)dgData.SelectedItem;
            if (!ValidateProductInputs()) return;
            selected.ProductName = BakeryName.Text.Trim();
            selected.Description = Description.Text.Trim();
            selected.Price = decimal.TryParse(Price.Text, out var price) ? price : 0;
            selected.Stock = int.TryParse(Stock.Text, out var stock) ? stock : 0;
            selected.CategoryId = cboCategory.SelectedValue is int id ? id : 0;
            selected.ImageUrl = Image.Text.Trim();

            _productService.UpdateProduct(selected);
            MessageBox.Show("Product updated successfully!", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
            FillDataGrid(_productService.GetAllProducts());
            ResetTextBox();
        }



        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateProductInputs()) return;

            Product newProduct = new Product
            {
                ProductName = BakeryName.Text.Trim(),
                Description = Description.Text.Trim(),
                Price = decimal.Parse(Price.Text),
                Stock = int.Parse(Stock.Text),
                CategoryId = (int)cboCategory.SelectedValue,
                CreatedAt = DateTime.Now,
                ImageUrl = Image.Text.Trim()
            };

            _productService.CreateProduct(newProduct);
            MessageBox.Show("Product created successfully!", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);

            FillDataGrid(_productService.GetAllProducts());

            ResetTextBox();

        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var selected = (Product)dgData.SelectedItem;
      

            var confirm = MessageBox.Show(
                $"Are you sure you want to delete this product {selected.ProductName} ?",
                "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (confirm == MessageBoxResult.Yes)
            {
                _productService.DeleteProduct(selected);
                MessageBox.Show("Product deleted successfully!", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);

            
                FillDataGrid(_productService.GetAllProducts());

                ResetTextBox();
            }
        }

        private bool ValidateProductInputs()
        {
           
            if (string.IsNullOrWhiteSpace(BakeryName.Text))
            {
                MessageBox.Show("Name is required!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                BakeryName.Focus();
                return false;
            }

       
            if (string.IsNullOrWhiteSpace(Price.Text))
            {
                MessageBox.Show("Price is required!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                Price.Focus();
                return false;
            }
            if (!decimal.TryParse(Price.Text, out decimal priceValue))
            {
                MessageBox.Show("Invalid price format. Please enter a valid number.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                Price.Focus();
                return false;
            }

            if (priceValue <= 0)
            {
                MessageBox.Show("Price must be greater than 0!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                Price.Focus();
                return false;
            }


            if (string.IsNullOrWhiteSpace(Stock.Text))
            {
                MessageBox.Show("Stock is required!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                Stock.Focus();
                return false;
            }
            if (!int.TryParse(Stock.Text, out int stockValue))
            {
                MessageBox.Show("Invalid stock format. Please enter a valid integer.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                Stock.Focus();
                return false;
            }

            if (stockValue < 0)
            {
                MessageBox.Show("Stock must be 0 or greater!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                Stock.Focus();
                return false;
            }

            if (cboCategory.SelectedValue == null)
            {
                MessageBox.Show("Please select a category!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                cboCategory.Focus();
                return false;
            }

        
            if (string.IsNullOrWhiteSpace(Image.Text))
            {
                MessageBox.Show("Image is required!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                Image.Focus();
                return false;
            }

            return true;
        }
    }
}

using System;
using System.Windows;
using System.Windows.Controls;
using Labb3ProgTemplate.DataModels.Products;
using Labb3ProgTemplate.Enums;
using Labb3ProgTemplate.Managerrs;
using Microsoft.Win32;

namespace Labb3ProgTemplate.Views
{

    public partial class AdminView : UserControl
    {
        private Uri _productIcon;

        public AdminView()
        {
            InitializeComponent();
            UserManager.CurrentUserChanged += UserManager_CurrentUserChanged;
            ProductManager.ProductListChanged += ProductManager_OnProductListChanged;

            ProductTypeComboBox.DataContext = Enum.GetValues(typeof(ProductTypes));
            ProductTypeComboBox.SelectedItem = ProductTypes.Products;
        }

        private Uri AddIcon()
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Icon Files | *.jpg; *.png; *.jpeg; *.gif; *.bmp";
            Uri uriIcon = null;

            if (openFileDialog.ShowDialog() == true)
            {
                string selectedFilePath = openFileDialog.FileName;

                uriIcon = new Uri(selectedFilePath, UriKind.Absolute);
            }

            return uriIcon;
        }

        private void UserManager_CurrentUserChanged()
        {
            ProdList.ItemsSource = ProductManager.Products;
        }

        private void ProductManager_OnProductListChanged()
        {
            ProdList.ItemsSource = null;
            ProdList.ItemsSource = ProductManager.Products;
        }

        private void ProdList_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ProdList.SelectedItem is Product selectedProduct)
            {
                ProductNameTextBox.Text = selectedProduct.Name;
                ProductPriceTextBox.Text = selectedProduct.Price.ToString();
                ProductTypeComboBox.SelectedItem = selectedProduct.Type;
                
            }
            
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            string productName = ProductNameTextBox.Text;
            string productPrice = ProductPriceTextBox.Text;
            ProductTypes productType = (ProductTypes)ProductTypeComboBox.SelectedItem;

            _productIcon = null;

            if (string.IsNullOrWhiteSpace(productName) &&
                string.IsNullOrWhiteSpace(productPrice))
            {
                MessageBox.Show("The lines are empty.");
                return;
            }

            if (string.IsNullOrWhiteSpace(productName))
            {
                MessageBox.Show("Please fill in the name of the product");
                return;
            }

            if (string.IsNullOrWhiteSpace(productPrice))
            {
                MessageBox.Show("Invalid price");
                return;
            }

            if (productType == ProductTypes.Products)
            {
                MessageBox.Show("Please select a product type.");
                return;
            }

            MessageBoxResult result = MessageBox.Show("Do you want to add your own icon?", "Confirmation", MessageBoxButton.YesNoCancel);
            if (result == MessageBoxResult.Yes)
            {
                _productIcon = AddIcon();
            }
            else if (result == MessageBoxResult.No)
            {
                if (ProductTypeComboBox.SelectedItem is ProductTypes selectedTypeItem)
                {
                    string? iconPath = ProductManager.GetIconPathByType(selectedTypeItem);
                    _productIcon = new Uri(iconPath, UriKind.Relative);
                }
            }
            else if (result == MessageBoxResult.Cancel)
            {
                return;
            }

            if (_productIcon is null)
            {
                result = MessageBox.Show("Do you want to use the default icon?", "Confirmation", MessageBoxButton.YesNo);

                if (result == MessageBoxResult.Yes)
                {
                    if (ProductTypeComboBox.SelectedItem is ProductTypes selectedTypeItem)
                    {
                        string? iconPath = ProductManager.GetIconPathByType(selectedTypeItem);
                        _productIcon = new Uri(iconPath, UriKind.Relative);
                    }
                }
                else
                {
                    return;
                }
            }

            if (Double.TryParse(productPrice, out double price))
            {
                if (ProdList.SelectedItem is Product selectedProduct)
                {
                    selectedProduct.Name = productName;
                    selectedProduct.Price = price;
                    selectedProduct.Type = productType;
                    selectedProduct.Icon = _productIcon;

                    ProdList.SelectedItem = null;
                }
                else
                {
                    ProductManager.AddProduct(new NewItems(productName, price, _productIcon, productType));
                }

                ClearTextBox();
            }
        }

        private void RemoveBtn_Click(object sender, RoutedEventArgs e)
        {
            if (ProdList.SelectedItem is Product selectedProduct)
            {
                ProductManager.RemoveProduct(selectedProduct);

                ClearTextBox();
            }
            else
            {
                MessageBox.Show("Please select a product to remove");
            }
        }

        private void LogoutBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Do you wish to logout?", "Confirmation", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                UserManager.LogOut();
                Window.GetWindow(this).Close();
            }
        }

        private void ResetBtn_OnClick(object sender, RoutedEventArgs e)
        {
            ClearTextBox();
            ProdList.SelectedItem = null;
        }

        private void ClearTextBox()
        {
            ProductNameTextBox.Clear();
            ProductPriceTextBox.Clear();

            ProductTypeComboBox.SelectedItem = ProductTypeComboBox.Items[0];
        }
    }
}

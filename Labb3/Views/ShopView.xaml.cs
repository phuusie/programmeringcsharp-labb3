using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Labb3ProgTemplate.DataModels.Products;
using Labb3ProgTemplate.Enums;
using Labb3ProgTemplate.Managerrs;

namespace Labb3ProgTemplate.Views
{

    public partial class ShopView : UserControl
    {
        private string _name;

        public ObservableCollection<Cart> Cart { get; } = new();

        public ShopView()
        {
            InitializeComponent();
            UserManager.CurrentUserChanged += UserManager_CurrentUserChanged;
            ProductManager.ProductListChanged += ProductManager_OnProductListChanged;

            DataContext = this;
            ProductTypeComboBox.DataContext = Enum.GetValues(typeof(ProductTypes));
            ProductTypeComboBox.SelectedItem = ProductTypes.Products;
        }

        private void UserManager_CurrentUserChanged()
        {
            ProdList.ItemsSource = ProductManager.Products;

            _name = UserManager.CurrentUser.Name;
            UserName.Text = $"Welcome, {_name}";
        }
        private void ProductManager_OnProductListChanged()
        {
            ProdList.ItemsSource = null;
            ProdList.ItemsSource = ProductManager.Products;
        }

        private void AddToCart(Product product)
        {
            var cartItem = new Cart
            {
                Name = product.Name,
                Price = product.Price,
                Icon = product.Icon
            };

            Cart.Add(cartItem);
        }

        private void RemoveBtn_Click(object sender, RoutedEventArgs e)
        {

            if (CartList.SelectedItem is Cart cartItem)
            {
                Cart.Remove(cartItem);
            }
            else
            {
                MessageBox.Show("You can only remove a selected item in your cart.");
            }
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            if (ProdList.SelectedItem is Product selectedProduct)
            {
                AddToCart(selectedProduct);
            }
            else
            {
                MessageBox.Show("Please selected a product to add to your cart.");
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

        private void CheckoutBtn_Click(object sender, RoutedEventArgs e)
        {
            double sum = 0;

            foreach (var product in Cart)
            {
                sum += product.Price;
            }

            if (sum == 0)
            {
                MessageBox.Show("Your cart is empty.");
            }

            MessageBoxResult result = MessageBox.Show($"Your total is: {sum}kr\nDo you wish to continue with your payment?", "Confirmation", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                MessageBox.Show($"Thank you {UserManager.CurrentUser.Name} for shopping with us\nHope too see you soon again!");
                Cart.Clear();
            }
        }

        private void ClearBtn_OnClick(object sender, RoutedEventArgs e)
        {
            Cart.Clear();
        }

        private void ProductTypeComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ProductTypeComboBox.SelectedItem != null)
            {
                var selectedProductType = (ProductTypes)ProductTypeComboBox.SelectedItem;

                var filteredProducts = ProductManager.Products?.Where(p => p.Type == selectedProductType).ToList();

                if(selectedProductType == ProductTypes.Products)
                {
                    ProdList.ItemsSource = ProductManager.Products;
                }
                else
                {
                    ProdList.ItemsSource = filteredProducts;
                }
            }

        }
    }
}

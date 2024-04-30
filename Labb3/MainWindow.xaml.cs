using System.Windows;
using Labb3ProgTemplate.Managerrs;

namespace Labb3ProgTemplate
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent(); 
            UserManager.CurrentUserChanged += UserManager_CurrentUserChanged;

        }

        private void UserManager_CurrentUserChanged()
        {

            if (UserManager.IsAdminLoggedIn)
            {
                AdminTab.Visibility = Visibility.Visible;
                ShopTab.Visibility = Visibility.Visible;
                LoginTab.Visibility = Visibility.Collapsed;

                AdminTab.IsSelected = true;

                AdminTab.IsEnabled = true;
                ShopTab.IsEnabled = true;
            }
            else
            {
                ShopTab.Visibility = Visibility.Visible;
                AdminTab.Visibility = Visibility.Collapsed;
                LoginTab.Visibility = Visibility.Collapsed;

                ShopTab.IsSelected = true;

                ShopTab.IsEnabled = true;
            }
        }
    }
}

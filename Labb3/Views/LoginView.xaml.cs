using System.Windows;
using System.Windows.Controls;
using Labb3ProgTemplate.Enums;
using Labb3ProgTemplate.Managerrs;

namespace Labb3ProgTemplate.Views
{
    public partial class LoginView : UserControl
    {

        public LoginView()
        {
            InitializeComponent();
            UserManager.CurrentUserChanged += UserManager_CurrentUserChanged;
        }

        private void UserManager_CurrentUserChanged()
        {
        }

        private void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            string name = LoginName.Text.ToUpper();
            string password = LoginPwd.Password;

            UserManager.LoginUser(name, password);

            if (!UserManager.LoginUser(name, password))
            {
                MessageBox.Show("Invalid login");
            }
        }

        private void RegisterAdminBtn_Click(object sender, RoutedEventArgs e)
        {
            string name = RegisterName.Text.ToUpper();
            string password = RegisterPwd.Password;

            UserManager.ChangeCurrentUser(name, password, UserTypes.Admin);

            RegisterName.Clear();
            RegisterPwd.Clear();

        }

        private void RegisterCustomerBtn_Click(object sender, RoutedEventArgs e)
        {
            string name = RegisterName.Text.ToUpper();
            string password = RegisterPwd.Password;

            UserManager.ChangeCurrentUser(name, password, UserTypes.Customer);

            RegisterName.Clear();
            RegisterPwd.Clear();
        }
    }
}

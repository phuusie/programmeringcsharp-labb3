using System.Windows;
using Labb3ProgTemplate.Managerrs;

namespace Labb3ProgTemplate
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            await UserManager.LoadUsersFromFile();
            await ProductManager.LoadProductsFromFile();
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            await UserManager.SaveUsersToFile();
            await ProductManager.SaveProductsToFile();

        }
    }
}

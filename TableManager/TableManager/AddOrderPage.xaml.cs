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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TableManager
{
    /// <summary>
    /// Логика взаимодействия для AddOrderPage.xaml
    /// </summary>
    public partial class AddOrderPage : Page
    {
        public AddOrderPage()
        {
            InitializeComponent();
        }

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            //go back to main page
            NavigationService.Navigate(new TablesPage());
        }

        private void buttonCompleteOrder_Click(object sender, RoutedEventArgs e)
        {
            //when all dishes are added "complete order" saves them and navigates to the main page
            NavigationService.Navigate(new TablesPage());
        }

        private void buttonAddDish_Click(object sender, RoutedEventArgs e)
        {
            //adding dich to order
        }
    }
}
